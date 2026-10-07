using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// Presentation for a PC that joins the LAN match as a spectator (<see cref="SpectatorSession"/>).
/// Display 1 shows both players' head views side by side; Display 2 cycles through fixed arena
/// cameras (<see cref="SpectatorViewpoint"/>, or generated ones around the cover layout). The PC has
/// no passthrough, so it renders the virtual arena plus stand-in figures where the players are.
/// Gallery keys: Right arrow or Space for the next camera, Left arrow for the previous, P to pause.
/// </summary>
[DefaultExecutionOrder(1000)]
public sealed class SpectatorDirector : MonoBehaviour
{
    private static readonly Color[] PlayerColors =
        { new Color(0.3f, 0.9f, 0.55f, 1f), new Color(1f, 0.62f, 0.18f, 1f) };
    private static readonly Color Backdrop = new Color(0.035f, 0.045f, 0.06f, 1f);
    private static readonly Color Panel = new Color(0.02f, 0.03f, 0.05f, 0.72f);
    // Head poses arrive at the 20 Hz network send rate; views run this far behind and interpolate.
    private const float ViewDelay = 0.1f;
    private const float FadeSeconds = 0.25f;
    // Generated cameras stay inside the 7 m mixed-reality playfield centred on ArenaRoot.
    public const float PlayfieldHalf = 3.2f;
    private const float BarWidth = 500f;

    [SerializeField, Min(2f)] private float shotSeconds = 8f;
    [SerializeField, Range(40f, 110f)] private float playerFieldOfView = 85f;

    private sealed class PlayerView
    {
        public NetworkHeadTracker Head;
        public NetworkPlayerHealth Health;
        public readonly List<(float time, Pose pose)> Samples = new List<(float, Pose)>();
        public Pose Pose;
        public bool HasPose;
        public int Index = -1;
        public Transform Avatar, AvatarHead, Legs;
        public Material Body, Face;
        public LineRenderer Ring;
        public Renderer[] Renderers;
        public Vector3 BarScale;
    }

    private sealed class PlayerCard
    {
        public RectTransform Root;
        public TextMeshProUGUI Hp, Shield, Waiting;
        public Image Fill;
    }

    public struct Shot
    {
        public string Label;
        public SpectatorViewpoint Marker;
        public Vector3 LocalPosition;
        public Quaternion LocalRotation;
        public float FieldOfView;
        public bool HideCeiling;
    }

    private readonly Dictionary<NetworkHeadTracker, PlayerView> views = new Dictionary<NetworkHeadTracker, PlayerView>();
    private readonly List<PlayerView> ordered = new List<PlayerView>();
    private readonly List<Shot> shots = new List<Shot>();
    private readonly Camera[] playerCameras = new Camera[2];
    private readonly PlayerCard[] cards = new PlayerCard[2];
    private readonly List<TextMeshProUGUI> titles = new List<TextMeshProUGUI>();
    private readonly List<TextMeshProUGUI> details = new List<TextMeshProUGUI>();
    private readonly List<TextMeshProUGUI> scores = new List<TextMeshProUGUI>();
    private readonly List<Material> materials = new List<Material>();
    private Camera galleryCamera;
    private Transform arenaRoot;
    private Renderer[] ceiling = new Renderer[0];
    private Material lineMaterial, visorMaterial;
    private TextMeshProUGUI shotName, shotInfo;
    private Image fade;
    private int cullingMask = ~0;
    private float nextRefresh;
    private int shotIndex = -1, pendingShot = -1;
    private float shotStartedAt, fadeStartedAt = -10f;
    private bool paused;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Boot()
    {
        if (SpectatorSession.IsSpectator && FindFirstObjectByType<SpectatorDirector>() == null)
            new GameObject("Spectator director").AddComponent<SpectatorDirector>();
    }

    private void Awake()
    {
        arenaRoot = GameObject.Find("ArenaRoot")?.transform;
        // Headsets calibrate ArenaRoot and the shared player frame to the same pose, so the
        // authored arena pose is this PC's calibration.
        NetworkPlayerAlignment.SetCalibration(arenaRoot != null ? arenaRoot.position : Vector3.zero,
            arenaRoot != null ? arenaRoot.rotation : Quaternion.identity);
        Transform roof = arenaRoot != null ? arenaRoot.Find("Environment/VirtualCeiloingAlwaysVisible") : null;
        if (roof != null) ceiling = roof.GetComponentsInChildren<Renderer>(true);

        DisableLocalRig();
        lineMaterial = CombatVfxStyle.CreateMaterial("Spectator lines", Color.white);
        visorMaterial = Solid(new Color(0.08f, 0.09f, 0.1f, 1f), 0f);
        materials.Add(lineMaterial);
        CreateCameras();
        CreateOverlays();
        Bounds layout = CoverBounds();
        shots.AddRange(AuthoredShots());
        if (shots.Count == 0) shots.AddRange(GenerateShots(layout));
        var ears = new GameObject("Spectator audio").AddComponent<AudioListener>();
        ears.transform.SetParent(transform, false);
        ears.transform.position = ArenaPoint(new Vector3(layout.center.x, 1.6f, layout.center.z));

        if (!Application.isEditor && Display.displays.Length > 1) Display.displays[1].Activate();
        RenderPipelineManager.beginCameraRendering += OnBeginCamera;
        RenderPipelineManager.endCameraRendering += OnEndCamera;
        Debug.Log($"SPECTATOR: joining the LAN match without a player. Display 1 = both players' views, "
            + $"Display 2 = {shots.Count} arena cameras.");
    }

    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCamera;
        RenderPipelineManager.endCameraRendering -= OnEndCamera;
        foreach (Material material in materials) if (material != null) Destroy(material);
    }

    private void DisableLocalRig()
    {
        // This PC never plays: no headset cameras, hand-tracked weapons or calibration gestures.
        foreach (OVRCameraRig rig in FindObjectsByType<OVRCameraRig>(FindObjectsSortMode.None))
            rig.disableEyeAnchorCameras = true;
        foreach (Camera camera in FindObjectsByType<Camera>(FindObjectsSortMode.None))
        {
            if (camera.CompareTag("MainCamera")) cullingMask = camera.cullingMask;
            camera.enabled = false;
        }
        foreach (AudioListener listener in FindObjectsByType<AudioListener>(FindObjectsSortMode.None))
            listener.enabled = false;
        foreach (PalmBeamShooter weapons in FindObjectsByType<PalmBeamShooter>(FindObjectsSortMode.None))
            weapons.gameObject.SetActive(false);
        foreach (MonoBehaviour gesture in FindObjectsByType<LeftHandCalibratePose>(FindObjectsSortMode.None))
            gesture.enabled = false;
        foreach (MonoBehaviour gesture in FindObjectsByType<LeftFistRemotePlayerAdjuster>(FindObjectsSortMode.None))
            gesture.enabled = false;
    }

    private void LateUpdate()
    {
        float now = Time.unscaledTime;
        // A disconnecting player's objects are destroyed; drop them straight away.
        if (ordered.Exists(view => view.Head == null)) nextRefresh = 0f;
        RefreshPlayers(now);
        foreach (PlayerView view in ordered) UpdateView(view, now);
        for (int i = 0; i < playerCameras.Length; i++)
        {
            bool live = i < ordered.Count && ordered[i].HasPose;
            playerCameras[i].enabled = live;
            if (live) playerCameras[i].transform.SetPositionAndRotation(ordered[i].Pose.position, ordered[i].Pose.rotation);
        }
        UpdateGallery(now);
        UpdateOverlay();
    }

    // ---------- Players ----------

    private void RefreshPlayers(float now)
    {
        if (now < nextRefresh) return;
        nextRefresh = now + 0.5f;
        foreach (NetworkHeadTracker head in FindObjectsByType<NetworkHeadTracker>(FindObjectsSortMode.None))
            if (!views.ContainsKey(head)) views.Add(head, CreateView(head));
        var gone = new List<NetworkHeadTracker>();
        foreach (var pair in views) if (pair.Key == null) gone.Add(pair.Key);
        foreach (NetworkHeadTracker head in gone)
        {
            PlayerView view = views[head];
            if (view.Avatar != null) Destroy(view.Avatar.gameObject);
            if (view.Ring != null) Destroy(view.Ring.gameObject);
            views.Remove(head);
        }
        ordered.Clear();
        ordered.AddRange(views.Values);
        ordered.Sort((a, b) => a.Head.netId.CompareTo(b.Head.netId));
        if (ordered.Count > playerCameras.Length) ordered.RemoveRange(playerCameras.Length, ordered.Count - playerCameras.Length);
        for (int i = 0; i < ordered.Count; i++)
        {
            if (ordered[i].Index == i) continue;
            ordered[i].Index = i;
            Color color = PlayerColors[i];
            SetColor(ordered[i].Body, color * 0.8f, 0.25f);
            SetColor(ordered[i].Face, color, 0.45f);
            ordered[i].Ring.startColor = ordered[i].Ring.endColor = CombatVfxStyle.WithAlpha(color, 0.8f);
        }
    }

    private PlayerView CreateView(NetworkHeadTracker head)
    {
        var view = new PlayerView { Head = head, Health = head.GetComponent<NetworkPlayerHealth>() };
        // The PC has no passthrough: a simple figure stands in for the real person.
        view.Body = Solid(Color.gray, 0f);
        view.Face = Solid(Color.white, 0f);
        view.Avatar = new GameObject("Spectator stand-in").transform;
        view.Avatar.SetParent(transform, false);
        Part(PrimitiveType.Capsule, view.Avatar, view.Body, new Vector3(0f, -0.52f, 0f), new Vector3(0.36f, 0.34f, 0.22f));
        view.Legs = Part(PrimitiveType.Capsule, view.Avatar, view.Body, new Vector3(0f, -1.2f, 0f), new Vector3(0.3f, 0.35f, 0.2f));
        view.AvatarHead = new GameObject("Head").transform;
        view.AvatarHead.SetParent(view.Avatar, false);
        Part(PrimitiveType.Sphere, view.AvatarHead, view.Face, Vector3.zero, new Vector3(0.19f, 0.23f, 0.21f));
        Part(PrimitiveType.Cube, view.AvatarHead, visorMaterial, new Vector3(0f, 0.01f, 0.1f), new Vector3(0.19f, 0.1f, 0.09f));
        view.Ring = CombatVfxStyle.CreateLine(transform, "Stand-in floor ring", lineMaterial, true, 0.025f);
        var renderers = new List<Renderer>(view.Avatar.GetComponentsInChildren<Renderer>(true)) { view.Ring };
        view.Renderers = renderers.ToArray();
        view.Avatar.gameObject.SetActive(false);
        return view;
    }

    private void UpdateView(PlayerView view, float now)
    {
        Pose latest = view.Head.HeadWorldPose;
        // Before its first head update a player has a zero rotation.
        if (Quaternion.Dot(latest.rotation, latest.rotation) > 0.5f) AddSample(view.Samples, now, latest);
        view.HasPose = view.Samples.Count > 0;
        view.Avatar.gameObject.SetActive(view.HasPose);
        view.Ring.enabled = view.HasPose;
        if (!view.HasPose) return;
        view.Pose = Interpolate(view.Samples, now - ViewDelay);

        Vector3 forward = Vector3.ProjectOnPlane(view.Pose.rotation * Vector3.forward, Vector3.up);
        if (forward.sqrMagnitude < 0.0001f) forward = Vector3.Cross(view.Pose.rotation * Vector3.right, Vector3.up);
        view.Avatar.SetPositionAndRotation(view.Pose.position, Quaternion.LookRotation(forward.normalized, Vector3.up));
        view.AvatarHead.rotation = view.Pose.rotation;
        float floor = arenaRoot != null ? arenaRoot.position.y : 0f;
        float legs = Mathf.Max(0.15f, view.Pose.position.y - 0.85f - floor);
        view.Legs.localPosition = new Vector3(0f, -0.85f - legs * 0.5f, 0f);
        view.Legs.localScale = new Vector3(0.3f, legs * 0.5f, 0.2f);
        CombatVfxStyle.SetRing(view.Ring, new Vector3(view.Pose.position.x, floor + 0.02f, view.Pose.position.z),
            Quaternion.Euler(90f, 0f, 0f), 0.38f, 40);

        // Keep the overhead health bar on the smoothed head rather than the raw 20 Hz pose.
        Transform bar = view.Health != null ? view.Health.HealthBarRoot : null;
        if (bar == null) return;
        if (view.BarScale == Vector3.zero) view.BarScale = bar.localScale;
        bar.position += view.Pose.position - latest.position;
    }

    /// <summary>Buffers a received head pose; after a still period, restarts from the resting pose.</summary>
    public static void AddSample(List<(float time, Pose pose)> samples, float now, Pose pose)
    {
        int last = samples.Count - 1;
        if (last >= 0 && samples[last].pose.position == pose.position && samples[last].pose.rotation == pose.rotation) return;
        if (last >= 0 && now - samples[last].time > 0.075f) samples.Add((now - 0.05f, samples[last].pose));
        samples.Add((now, pose));
        while (samples.Count > 12) samples.RemoveAt(0);
    }

    public static Pose Interpolate(List<(float time, Pose pose)> samples, float at)
    {
        if (at <= samples[0].time) return samples[0].pose;
        for (int i = 1; i < samples.Count; i++)
        {
            if (samples[i].time < at) continue;
            var (fromTime, from) = samples[i - 1];
            var (toTime, to) = samples[i];
            float t = Mathf.InverseLerp(fromTime, toTime, at);
            return new Pose(Vector3.Lerp(from.position, to.position, t), Quaternion.Slerp(from.rotation, to.rotation, t));
        }
        return samples[samples.Count - 1].pose;
    }

    // ---------- Per-camera visibility ----------

    private void OnBeginCamera(ScriptableRenderContext context, Camera camera)
    {
        int owner = System.Array.IndexOf(playerCameras, camera);
        bool gallery = camera == galleryCamera;
        if (owner < 0 && !gallery) return;
        for (int i = 0; i < ordered.Count; i++)
        {
            PlayerView view = ordered[i];
            bool own = i == owner;
            // A player's own view shows neither their stand-in nor their health bar.
            foreach (Renderer part in view.Renderers) part.forceRenderingOff = own;
            Transform bar = view.Health != null ? view.Health.HealthBarRoot : null;
            if (bar == null || view.BarScale == Vector3.zero) continue;
            bar.localScale = own ? Vector3.zero : view.BarScale;
            Vector3 facing = bar.position - camera.transform.position;
            if (facing.sqrMagnitude > 0.0001f) bar.rotation = Quaternion.LookRotation(facing, Vector3.up);
        }
        bool hideRoof = gallery && shotIndex >= 0 && shots[shotIndex].HideCeiling;
        foreach (Renderer roof in ceiling) if (roof != null) roof.forceRenderingOff = hideRoof;
    }

    private void OnEndCamera(ScriptableRenderContext context, Camera camera)
    {
        if (camera != galleryCamera && System.Array.IndexOf(playerCameras, camera) < 0) return;
        foreach (PlayerView view in ordered)
        {
            foreach (Renderer part in view.Renderers) part.forceRenderingOff = false;
            Transform bar = view.Health != null ? view.Health.HealthBarRoot : null;
            if (bar != null && view.BarScale != Vector3.zero) bar.localScale = view.BarScale;
        }
        foreach (Renderer roof in ceiling) if (roof != null) roof.forceRenderingOff = false;
    }

    // ---------- Gallery ----------

    private void UpdateGallery(float now)
    {
        if (shots.Count == 0) return;
        Keyboard keys = Keyboard.current;
        if (keys != null)
        {
            if (keys.rightArrowKey.wasPressedThisFrame || keys.spaceKey.wasPressedThisFrame) BeginCut(1, now);
            else if (keys.leftArrowKey.wasPressedThisFrame) BeginCut(-1, now);
            if (keys.pKey.wasPressedThisFrame) paused = !paused;
        }
        if (shotIndex < 0) ApplyShot(0, now);
        else if (pendingShot < 0 && !paused && now - shotStartedAt >= shotSeconds) BeginCut(1, now);

        float fadeAge = now - fadeStartedAt;
        if (pendingShot >= 0 && fadeAge >= FadeSeconds)
        {
            ApplyShot(pendingShot, now);
            pendingShot = -1;
        }
        float dark = fadeAge < FadeSeconds ? fadeAge / FadeSeconds : 1f - (fadeAge - FadeSeconds) / FadeSeconds;
        fade.color = new Color(0f, 0f, 0f, Mathf.Clamp01(dark));

        Shot shot = shots[shotIndex];
        Vector3 position = shot.Marker != null ? shot.Marker.transform.position : ArenaPoint(shot.LocalPosition);
        Quaternion rotation = shot.Marker != null ? shot.Marker.transform.rotation
            : (arenaRoot != null ? arenaRoot.rotation : Quaternion.identity) * shot.LocalRotation;
        // A slow push-in keeps fixed cameras alive.
        float drift = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((now - shotStartedAt) / shotSeconds));
        galleryCamera.transform.SetPositionAndRotation(position + rotation * Vector3.forward * (0.3f * drift), rotation);
        galleryCamera.fieldOfView = shot.FieldOfView;
        shotInfo.text = $"CAMERA {shotIndex + 1}/{shots.Count}" + (paused ? "  ·  PAUSED (P)" : "  ·  ARROW KEYS TO SWITCH, P TO PAUSE");
    }

    private void BeginCut(int step, float now)
    {
        int from = pendingShot >= 0 ? pendingShot : shotIndex;
        pendingShot = ((from + step) % shots.Count + shots.Count) % shots.Count;
        if (now - fadeStartedAt >= FadeSeconds) fadeStartedAt = now;
    }

    private void ApplyShot(int index, float now)
    {
        shotIndex = index;
        shotStartedAt = now;
        shotName.text = shots[index].Label;
    }

    private IEnumerable<Shot> AuthoredShots()
    {
        var markers = new List<SpectatorViewpoint>(FindObjectsByType<SpectatorViewpoint>(FindObjectsSortMode.None));
        markers.Sort((a, b) => string.CompareOrdinal(a.name, b.name));
        foreach (SpectatorViewpoint marker in markers)
            yield return new Shot { Label = marker.Label, Marker = marker, FieldOfView = marker.FieldOfView, HideCeiling = marker.HideCeiling };
    }

    /// <summary>Arena-space cameras around the cover layout, kept inside the playfield.</summary>
    public static List<Shot> GenerateShots(Bounds layout)
    {
        var result = new List<Shot>();
        Vector3 centre = new Vector3(layout.center.x, 0f, layout.center.z);
        float x = Mathf.Max(1.5f, layout.extents.x), z = Mathf.Max(1.5f, layout.extents.z);
        Vector3 focus = centre + Vector3.up * 0.9f;
        void Add(string label, Vector3 position, Vector3 target, float fieldOfView, bool hideCeiling)
        {
            position.x = Mathf.Clamp(position.x, -PlayfieldHalf, PlayfieldHalf);
            position.z = Mathf.Clamp(position.z, -PlayfieldHalf, PlayfieldHalf);
            result.Add(new Shot { Label = label, LocalPosition = position, FieldOfView = fieldOfView, HideCeiling = hideCeiling,
                LocalRotation = Quaternion.LookRotation(target - position, Vector3.up) });
        }
        Add("HIGH CORNER A", centre + new Vector3(-x - 0.8f, 3.2f, -z - 0.8f), focus, 58f, true);
        Add("SIDELINE RIGHT", centre + new Vector3(x + 0.9f, 2f, 0f), centre + Vector3.up, 70f, false);
        Add("END ZONE A", centre + new Vector3(0f, 2.3f, -z - 1.1f), focus, 64f, true);
        // Straight down, framed to fit the layout on a 16:9 screen.
        float height = (Mathf.Max(z, x / (16f / 9f)) + 0.6f) / Mathf.Tan(30f * Mathf.Deg2Rad);
        result.Add(new Shot { Label = "OVERHEAD", LocalPosition = centre + Vector3.up * height, FieldOfView = 60f,
            HideCeiling = true, LocalRotation = Quaternion.LookRotation(Vector3.down, Vector3.forward) });
        Add("HIGH CORNER B", centre + new Vector3(x + 0.8f, 3.2f, z + 0.8f), focus, 58f, true);
        Add("SIDELINE LEFT", centre + new Vector3(-x - 0.9f, 2f, 0f), centre + Vector3.up, 70f, false);
        Add("END ZONE B", centre + new Vector3(0f, 2.3f, z + 1.1f), focus, 64f, true);
        return result;
    }

    private Bounds CoverBounds()
    {
        // Centre of the 1v1 layout, the same point the heal floats over.
        var fallback = new Bounds(new Vector3(0.275f, 0f, 0.71f), new Vector3(4f, 2f, 4f));
        Transform cover = arenaRoot != null ? arenaRoot.Find("GameplayRoot/GameplayCover") : null;
        if (cover == null) return fallback;
        bool any = false;
        Bounds local = fallback;
        foreach (Renderer part in cover.GetComponentsInChildren<Renderer>())
        {
            Bounds world = part.bounds;
            for (int corner = 0; corner < 8; corner++)
            {
                Vector3 sign = new Vector3((corner & 1) == 0 ? -1f : 1f, (corner & 2) == 0 ? -1f : 1f, (corner & 4) == 0 ? -1f : 1f);
                Vector3 point = arenaRoot.InverseTransformPoint(world.center + Vector3.Scale(world.extents, sign));
                if (!any) local = new Bounds(point, Vector3.zero);
                else local.Encapsulate(point);
                any = true;
            }
        }
        return local;
    }

    private Vector3 ArenaPoint(Vector3 local) => arenaRoot != null ? arenaRoot.TransformPoint(local) : local;

    // ---------- Cameras and overlays ----------

    private void CreateCameras()
    {
        Camera backdrop = MakeCamera("Spectator backdrop", 0, -10f);
        backdrop.cullingMask = 0;
        for (int i = 0; i < playerCameras.Length; i++)
        {
            playerCameras[i] = MakeCamera("Player " + (i + 1) + " view", 0, 0f);
            playerCameras[i].rect = new Rect(i * 0.5f, 0f, 0.5f, 1f);
            playerCameras[i].fieldOfView = playerFieldOfView;
            playerCameras[i].nearClipPlane = 0.03f;
            playerCameras[i].enabled = false;
        }
        galleryCamera = MakeCamera("Arena gallery", 1, 0f);
        // Billboards, the blast veil and 2D sounds follow Camera.main; on this PC that is the gallery.
        galleryCamera.tag = "MainCamera";
    }

    private Camera MakeCamera(string cameraName, int display, float depth)
    {
        var camera = new GameObject(cameraName).AddComponent<Camera>();
        camera.transform.SetParent(transform, false);
        camera.targetDisplay = display;
        camera.depth = depth;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Backdrop;
        camera.cullingMask = cullingMask;
        camera.stereoTargetEye = StereoTargetEyeMask.None;
        camera.nearClipPlane = 0.05f;
        camera.farClipPlane = 300f;
        return camera;
    }

    private void CreateOverlays()
    {
        RectTransform players = Overlay("Spectator player overlay", 0);
        RectTransform arena = Overlay("Spectator arena overlay", 1);

        var divider = HudKit.Image(players, "Divider", null, new Color(0f, 0f, 0f, 0.9f), Vector2.zero, new Vector2(6f, 0f));
        divider.rectTransform.anchorMin = new Vector2(0.5f, 0f);
        divider.rectTransform.anchorMax = new Vector2(0.5f, 1f);
        for (int i = 0; i < cards.Length; i++) cards[i] = Card(players, i);
        RoundBlock(players);

        fade = HudKit.Image(arena, "Cut fade", null, Color.clear, Vector2.zero, Vector2.zero);
        fade.rectTransform.anchorMin = Vector2.zero;
        fade.rectTransform.anchorMax = Vector2.one;
        RoundBlock(arena);
        RectTransform label = At(HudKit.Rect(arena, "Camera label", Vector2.zero, new Vector2(900f, 110f)), new Vector2(0f, 0f), new Vector2(510f, 90f));
        HudKit.Image(label, "Accent", HudSprites.Panel(4), PlayerColors[0], new Vector2(-440f, 0f), new Vector2(6f, 84f), true, 1f);
        shotName = HudKit.Text(label, "Name", HudKit.Display, 40f, Color.white, new Vector2(0f, 16f), new Vector2(860f, 50f), TextAlignmentOptions.Left);
        shotName.characterSpacing = 10f;
        shotInfo = HudKit.Text(label, "Info", HudKit.Display, 22f, FusionRoundHud.Soft, new Vector2(0f, -26f), new Vector2(860f, 36f), TextAlignmentOptions.Left);
    }

    private RectTransform Overlay(string overlayName, int display)
    {
        var go = new GameObject(overlayName, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
        go.transform.SetParent(transform, false);
        var canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.targetDisplay = display;
        canvas.sortingOrder = 50;
        var scaler = go.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;
        return (RectTransform)go.transform;
    }

    private static RectTransform At(RectTransform rect, Vector2 anchor, Vector2 position)
    {
        rect.anchorMin = rect.anchorMax = anchor;
        rect.anchoredPosition = position;
        return rect;
    }

    private PlayerCard Card(RectTransform parent, int index)
    {
        Color color = PlayerColors[index];
        float half = 0.25f + 0.5f * index;
        var card = new PlayerCard { Root = At(HudKit.Rect(parent, $"Player {index + 1} card", Vector2.zero, new Vector2(560f, 120f)), new Vector2(half, 0f), new Vector2(0f, 90f)) };
        HudKit.Image(card.Root, "Panel", HudSprites.Panel(6), Panel, Vector2.zero, new Vector2(560f, 120f), true, 1f);
        var name = HudKit.Text(card.Root, "Name", HudKit.Display, 34f, color, new Vector2(-115f, 24f), new Vector2(300f, 50f), TextAlignmentOptions.Left);
        name.text = "PLAYER " + (index + 1);
        name.characterSpacing = 10f;
        card.Hp = HudKit.Text(card.Root, "HP", HudKit.Heavy, 52f, Color.white, new Vector2(165f, 22f), new Vector2(200f, 70f), TextAlignmentOptions.Right);
        HudKit.Image(card.Root, "Track", HudSprites.Panel(4), new Color(1f, 1f, 1f, 0.12f), new Vector2(0f, -32f), new Vector2(BarWidth, 14f), true, 1f);
        card.Fill = HudKit.Image(card.Root, "Fill", HudSprites.Panel(4), color, new Vector2(-BarWidth / 2f, -32f), new Vector2(BarWidth, 14f), true, 1f);
        card.Fill.rectTransform.pivot = new Vector2(0f, 0.5f);
        card.Shield = HudKit.Text(card.Root, "Shield", HudKit.Display, 26f, CombatVfxStyle.Shield, new Vector2(0f, 88f), new Vector2(400f, 40f), TextAlignmentOptions.Center);
        card.Shield.text = "SHIELD UP";
        card.Shield.characterSpacing = 10f;
        card.Waiting = HudKit.Text(parent, $"Player {index + 1} waiting", HudKit.Display, 40f, HudKit.A(color, 0.9f), Vector2.zero, new Vector2(900f, 60f), TextAlignmentOptions.Center);
        At(card.Waiting.rectTransform, new Vector2(half, 0.5f), Vector2.zero);
        return card;
    }

    private void RoundBlock(RectTransform parent)
    {
        RectTransform block = At(HudKit.Rect(parent, "Round", Vector2.zero, new Vector2(640f, 160f)), new Vector2(0.5f, 1f), new Vector2(0f, -90f));
        HudKit.Image(block, "Panel", HudSprites.Panel(6), Panel, new Vector2(0f, 6f), new Vector2(640f, 130f), true, 1f);
        titles.Add(HudKit.Text(block, "Title", HudKit.Heavy, 60f, Color.white, new Vector2(0f, 24f), new Vector2(620f, 80f), TextAlignmentOptions.Center));
        details.Add(HudKit.Text(block, "Detail", HudKit.Display, 24f, FusionRoundHud.Soft, new Vector2(0f, -30f), new Vector2(620f, 40f), TextAlignmentOptions.Center));
        details[details.Count - 1].characterSpacing = 8f;
        scores.Add(HudKit.Text(block, "Score", HudKit.Heavy, 34f, Color.white, new Vector2(0f, -82f), new Vector2(400f, 50f), TextAlignmentOptions.Center));
    }

    private void UpdateOverlay()
    {
        FusionRoundDirector round = NetworkClient.isConnected ? FusionRoundDirector.Active() : null;
        RoundText(round, out string title, out string detail, out Color accent);
        string score = round != null && round.WinsA + round.WinsB > 0
            ? $"<color=#{ColorUtility.ToHtmlStringRGB(PlayerColors[0])}>{round.WinsA}</color>  -  "
              + $"<color=#{ColorUtility.ToHtmlStringRGB(PlayerColors[1])}>{round.WinsB}</color>"
            : "";
        for (int i = 0; i < titles.Count; i++)
        {
            titles[i].text = title;
            titles[i].color = accent;
            details[i].text = detail;
            scores[i].text = score;
        }
        for (int i = 0; i < cards.Length; i++)
        {
            PlayerView view = i < ordered.Count ? ordered[i] : null;
            PlayerCard card = cards[i];
            card.Root.gameObject.SetActive(view != null && view.Health != null);
            card.Waiting.gameObject.SetActive(view == null || !view.HasPose);
            card.Waiting.text = NetworkClient.isConnected ? "WAITING FOR PLAYER " + (i + 1) : "NOT CONNECTED";
            if (view == null || view.Health == null) continue;
            card.Hp.text = Mathf.Max(0, view.Health.CurrentHealth).ToString();
            card.Fill.rectTransform.sizeDelta = new Vector2(BarWidth * view.Health.Health01, 14f);
            card.Shield.gameObject.SetActive(view.Health.IsShieldActive);
        }
    }

    private void RoundText(FusionRoundDirector round, out string title, out string detail, out Color accent)
    {
        accent = Color.white;
        if (!NetworkClient.isConnected) { title = "SPECTATOR"; detail = "LOOKING FOR THE HOST QUEST"; return; }
        if (round == null) { title = "WAITING"; detail = "WAITING FOR PLAYERS"; return; }
        switch (round.Phase)
        {
            case FusionRoundDirector.RoundPhase.Sandbox:
                title = "PRACTICE";
                detail = "THUMBS UP FOR 3 SECONDS TO START";
                return;
            case FusionRoundDirector.RoundPhase.Countdown:
                title = Mathf.Max(1, Mathf.CeilToInt(round.PhaseRemaining)).ToString();
                detail = "ROUND STARTING";
                return;
            case FusionRoundDirector.RoundPhase.Fighting:
            {
                int seconds = Mathf.Max(0, Mathf.CeilToInt(round.PhaseRemaining));
                title = $"{seconds / 60}:{seconds % 60:00}";
                float suddenDeath = round.SuddenDeathClock;
                detail = suddenDeath >= -5f && suddenDeath < 0f ? "SUDDEN DEATH IN " + Mathf.CeilToInt(-suddenDeath)
                    : suddenDeath >= 0f ? "SUDDEN DEATH"
                    : round.SoloOverride ? "SOLO ROUND" : "FIGHT";
                if (suddenDeath >= -5f) accent = FusionRoundHud.Amber;
                return;
            }
            case FusionRoundDirector.RoundPhase.Result:
            {
                int winner = ordered.FindIndex(view => view.Head != null && (int)view.Head.netId == round.WinnerPlayerId);
                title = round.SoloOverride ? (winner >= 0 ? "SURVIVED" : "KNOCKED OUT")
                    : winner >= 0 ? $"PLAYER {winner + 1} WINS" : "DRAW";
                if (winner >= 0) accent = PlayerColors[winner];
                detail = "PRACTICE IN " + Mathf.CeilToInt(round.PhaseRemaining);
                return;
            }
            default:
                title = "WAITING";
                detail = "PLAYERS CALIBRATE AT THE START MARK";
                return;
        }
    }

    // ---------- Stand-in geometry ----------

    private static Transform Part(PrimitiveType shape, Transform parent, Material material, Vector3 position, Vector3 scale)
    {
        GameObject part = GameObject.CreatePrimitive(shape);
        Destroy(part.GetComponent<Collider>());
        part.transform.SetParent(parent, false);
        part.transform.localPosition = position;
        part.transform.localScale = scale;
        var renderer = part.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = material;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        return part.transform;
    }

    private Material Solid(Color color, float glow)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        var material = new Material(shader) { name = "Spectator stand-in" };
        material.SetFloat("_Smoothness", 0.35f);
        SetColor(material, color, glow);
        materials.Add(material);
        return material;
    }

    private static void SetColor(Material material, Color color, float glow)
    {
        material.color = color;
        if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
        // A little self-light keeps the figures readable in a dim arena.
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color * glow);
    }
}
