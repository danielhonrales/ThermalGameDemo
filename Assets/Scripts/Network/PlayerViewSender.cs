using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Meta.XR;
using Meta.XR.EnvironmentDepth;
using Mirror;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

/// <summary>
/// Sends this player's view to the spectator PC (<see cref="PlayerViewStream"/>): the left passthrough
/// camera's picture with the game's virtual content drawn over it from that camera's pose and lens,
/// so the PC shows a proper rectangular view. The camera only runs while a spectator is announcing
/// itself on the LAN; the player is asked once for the headset camera permission.
/// </summary>
[DisallowMultipleComponent]
public sealed class PlayerViewSender : MonoBehaviour
{
    private const string CameraPermission = "horizonos.permission.HEADSET_CAMERA";

    private UdpClient beacon;
    private volatile IPEndPoint spectator;
    private float lastBeaconAt = -100f;
    private PassthroughCameraAccess cameraAccess;
    private bool permissionAsked;
    private volatile bool permissionGranted;
    private Camera viewCamera;
    private RenderTexture frame, virtualLayer;
    private Material composite;
    private bool projectionReady, readbackPending;
    private float nextFrameAt;

    // The sender thread encodes and sends the latest frame; older unsent frames are dropped.
    private Thread sender;
    private volatile bool running = true;
    private readonly AutoResetEvent wake = new AutoResetEvent(false);
    private readonly object pendingLock = new object();
    private byte[] pendingPixels;
    private int pendingPlayer, pendingWidth, pendingHeight;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Boot()
    {
        if (SpectatorSession.IsSpectator || Application.platform != RuntimePlatform.Android) return;
        if (FindFirstObjectByType<PlayerViewSender>() == null)
            DontDestroyOnLoad(new GameObject("Player view sender").AddComponent<PlayerViewSender>().gameObject);
    }

    private void Awake()
    {
        try
        {
            beacon = new UdpClient(AddressFamily.InterNetwork);
            beacon.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            beacon.Client.Bind(new IPEndPoint(IPAddress.Any, PlayerViewStream.BeaconPort));
        }
        catch (SocketException e)
        {
            Debug.LogWarning($"[PlayerView] Cannot listen for a spectator on UDP {PlayerViewStream.BeaconPort}: {e.Message}");
            enabled = false;
            return;
        }
        sender = new Thread(SendLoop) { IsBackground = true, Name = "Player view sender" };
        sender.Start();
    }

    private void Update()
    {
        ListenForSpectator();
        bool wanted = spectator != null && Time.unscaledTime - lastBeaconAt < PlayerViewStream.TimeoutSeconds;
        if (!wanted)
        {
            if (cameraAccess != null && cameraAccess.enabled) { cameraAccess.enabled = false; Debug.Log("[PlayerView] Spectator gone; camera off."); }
            return;
        }
        if (cameraAccess == null) StartCamera();
        else if (!cameraAccess.enabled) cameraAccess.enabled = true;
    }

    private void ListenForSpectator()
    {
        try
        {
            while (beacon.Available > 0)
            {
                IPEndPoint from = null;
                byte[] packet = beacon.Receive(ref from);
                if (!PlayerViewStream.TryParseBeacon(Encoding.UTF8.GetString(packet), out int port)) continue;
                var found = new IPEndPoint(from.Address, port);
                if (spectator == null || !spectator.Equals(found)) Debug.Log($"[PlayerView] Spectator at {found}.");
                spectator = found;
                lastBeaconAt = Time.unscaledTime;
            }
        }
        catch (SocketException) { /* Wi-Fi may drop; keep listening. */ }
        catch (ObjectDisposedException) { }
    }

    private void StartCamera()
    {
        if (!PassthroughCameraAccess.IsSupported) { Debug.LogWarning("[PlayerView] This headset has no passthrough camera access."); enabled = false; return; }
        if (!permissionGranted && !Permission.HasUserAuthorizedPermission(CameraPermission))
        {
            if (permissionAsked) return;
            permissionAsked = true;
            var callbacks = new PermissionCallbacks();
            callbacks.PermissionGranted += _ => permissionGranted = true;
            callbacks.PermissionDenied += _ => Debug.LogWarning("[PlayerView] Headset camera permission denied; the spectator shows the reconstructed view.");
            Permission.RequestUserPermission(CameraPermission, callbacks);
            return;
        }

        var host = new GameObject("Passthrough camera");
        host.transform.SetParent(transform, false);
        host.SetActive(false);
        cameraAccess = host.AddComponent<PassthroughCameraAccess>();
        cameraAccess.CameraPosition = PassthroughCameraAccess.CameraPositionType.Left;
        cameraAccess.RequestedResolution = new Vector2Int(1280, 960);
        cameraAccess.MaxFramerate = 30;
        host.SetActive(true);

        viewCamera = new GameObject("Player view camera").AddComponent<Camera>();
        viewCamera.transform.SetParent(transform, false);
        viewCamera.enabled = false;
        viewCamera.stereoTargetEye = StereoTargetEyeMask.None;
        // The game is drawn on its own over a transparent background, then laid over the camera picture.
        viewCamera.clearFlags = CameraClearFlags.SolidColor;
        viewCamera.backgroundColor = Color.clear;
        viewCamera.nearClipPlane = 0.05f;
        viewCamera.farClipPlane = 100f;
        viewCamera.allowHDR = false;
        viewCamera.allowMSAA = false;
        Camera eye = Camera.main;
        if (eye != null) viewCamera.cullingMask = eye.cullingMask;
        Shader shader = Resources.Load<Shader>("PlayerViewComposite");
        if (shader != null) composite = new Material(shader) { name = "Player view composite" };
        else Debug.LogWarning("[PlayerView] Composite shader missing; the view will show the camera picture only.");
        Debug.Log("[PlayerView] Passthrough camera starting.");
    }

    private void LateUpdate()
    {
        if (cameraAccess == null || !cameraAccess.enabled || !cameraAccess.IsPlaying || !cameraAccess.IsUpdatedThisFrame) return;
        if (readbackPending || Time.unscaledTime < nextFrameAt) return;
        nextFrameAt = Time.unscaledTime + 1f / PlayerViewStream.FramesPerSecond;
        Capture();
    }

    private void Capture()
    {
        Vector2Int source = cameraAccess.CurrentResolution;
        int width = PlayerViewStream.Width, height = Mathf.RoundToInt(width * (float)source.y / Mathf.Max(1, source.x));
        if (frame == null || frame.height != height)
        {
            if (frame != null) frame.Release();
            if (virtualLayer != null) virtualLayer.Release();
            frame = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32) { name = "Player view" };
            virtualLayer = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32) { name = "Player view game layer" };
            frame.Create();
            virtualLayer.Create();
        }
        Pose pose = cameraAccess.GetCameraPose();
        if (!projectionReady) { viewCamera.projectionMatrix = Projection(pose); projectionReady = true; }
        viewCamera.transform.SetPositionAndRotation(pose.position, pose.rotation);
        viewCamera.targetTexture = virtualLayer;

        // The game from the camera's viewpoint, on its own; then the camera picture with the game laid over it.
        // Environment-depth occlusion follows the eyes' depth, which doesn't line up with this camera.
        bool hard = Shader.IsKeywordEnabled(EnvironmentDepthManager.HardOcclusionKeyword);
        bool soft = Shader.IsKeywordEnabled(EnvironmentDepthManager.SoftOcclusionKeyword);
        Shader.DisableKeyword(EnvironmentDepthManager.HardOcclusionKeyword);
        Shader.DisableKeyword(EnvironmentDepthManager.SoftOcclusionKeyword);
        try { viewCamera.Render(); }
        finally
        {
            if (hard) Shader.EnableKeyword(EnvironmentDepthManager.HardOcclusionKeyword);
            if (soft) Shader.EnableKeyword(EnvironmentDepthManager.SoftOcclusionKeyword);
        }
        Graphics.Blit(cameraAccess.GetTexture(), frame);
        if (composite != null) Graphics.Blit(virtualLayer, frame, composite);

        int player = NetworkClient.localPlayer != null ? (int)NetworkClient.localPlayer.netId : 0;
        readbackPending = true;
        AsyncGPUReadback.Request(frame, 0, TextureFormat.RGBA32, request =>
        {
            readbackPending = false;
            if (request.hasError) return;
            byte[] pixels = request.GetData<byte>().ToArray();
            lock (pendingLock)
            {
                pendingPixels = pixels;
                pendingPlayer = player;
                pendingWidth = request.width;
                pendingHeight = request.height;
            }
            wake.Set();
        });
    }

    /// <summary>An off-centre projection matching the passthrough camera's lens, from rays through its image edges.</summary>
    private Matrix4x4 Projection(Pose pose)
    {
        Quaternion toCamera = Quaternion.Inverse(pose.rotation);
        Vector3 Edge(float x, float y) => toCamera * cameraAccess.ViewportPointToRay(new Vector2(x, y), pose).direction;
        Vector3 left = Edge(0f, 0.5f), right = Edge(1f, 0.5f), bottom = Edge(0.5f, 0f), top = Edge(0.5f, 1f);
        float near = viewCamera.nearClipPlane;
        return Matrix4x4.Frustum(left.x / left.z * near, right.x / right.z * near,
            bottom.y / bottom.z * near, top.y / top.z * near, near, viewCamera.farClipPlane);
    }

    private void SendLoop()
    {
        TcpClient client = null;
        NetworkStream stream = null;
        IPEndPoint connectedTo = null;
        while (running)
        {
            wake.WaitOne(500);
            byte[] pixels;
            int player, width, height;
            lock (pendingLock)
            {
                pixels = pendingPixels;
                pendingPixels = null;
                player = pendingPlayer;
                width = pendingWidth;
                height = pendingHeight;
            }
            IPEndPoint target = spectator;
            if (pixels == null || target == null) continue;
            try
            {
                byte[] jpeg = ImageConversion.EncodeArrayToJPG(pixels, GraphicsFormat.R8G8B8A8_UNorm,
                    (uint)width, (uint)height, 0, PlayerViewStream.JpegQuality);
                if (client == null || !client.Connected || !target.Equals(connectedTo))
                {
                    client?.Close();
                    client = new TcpClient(AddressFamily.InterNetwork) { NoDelay = true, SendTimeout = 2000 };
                    if (!client.ConnectAsync(target.Address, target.Port).Wait(1500)) throw new SocketException((int)SocketError.TimedOut);
                    stream = client.GetStream();
                    connectedTo = target;
                }
                PlayerViewStream.WriteFrame(stream, player, jpeg);
            }
            catch (Exception)
            {
                // The spectator may have closed or not be listening yet; try again shortly.
                client?.Close();
                client = null;
                connectedTo = null;
                Thread.Sleep(1000);
            }
        }
        client?.Close();
    }

    private void OnDestroy()
    {
        running = false;
        wake.Set();
        sender?.Join(2000);
        beacon?.Close();
        if (frame != null) frame.Release();
        if (virtualLayer != null) virtualLayer.Release();
        if (composite != null) Destroy(composite);
    }
}
