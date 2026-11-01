using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// The spectator PC's end of <see cref="PlayerViewStream"/>: announces itself on the LAN so headsets
/// start sending their players' views, accepts their connections, and keeps the newest frame of each
/// player as a texture for <see cref="SpectatorDirector"/>.
/// </summary>
[DisallowMultipleComponent]
public sealed class PlayerViewReceiver : MonoBehaviour
{
    private UdpClient beacon;
    private byte[] beaconPacket;
    private float nextBeacon;
    private TcpListener listener;
    private Thread acceptThread;
    private volatile bool running;
    private readonly List<TcpClient> clients = new List<TcpClient>();

    // Written by connection threads, read on the main thread.
    private readonly object incomingLock = new object();
    private readonly Dictionary<int, byte[]> incoming = new Dictionary<int, byte[]>();

    private readonly Dictionary<int, Texture2D> textures = new Dictionary<int, Texture2D>();
    private readonly Dictionary<int, float> updatedAt = new Dictionary<int, float>();

    /// <summary>The player's live view, if a frame arrived recently.</summary>
    public bool TryGetView(int playerId, out Texture2D texture)
    {
        texture = null;
        return textures.TryGetValue(playerId, out texture) && updatedAt.TryGetValue(playerId, out float at)
            && Time.unscaledTime - at < PlayerViewStream.TimeoutSeconds;
    }

    private void Awake()
    {
        running = true;
        beaconPacket = Encoding.UTF8.GetBytes(PlayerViewStream.BeaconPrefix + PlayerViewStream.StreamPort);
        try
        {
            beacon = new UdpClient(AddressFamily.InterNetwork) { EnableBroadcast = true };
            listener = new TcpListener(IPAddress.Any, PlayerViewStream.StreamPort);
            listener.Start();
            acceptThread = new Thread(AcceptLoop) { IsBackground = true, Name = "Player view accept" };
            acceptThread.Start();
            Debug.Log($"[PlayerView] Waiting for headset views on TCP {PlayerViewStream.StreamPort}.");
        }
        catch (SocketException e)
        {
            Debug.LogWarning($"[PlayerView] Live player views unavailable: {e.Message}");
            running = false;
        }
    }

    private void Update()
    {
        if (!running) return;
        if (Time.unscaledTime >= nextBeacon)
        {
            nextBeacon = Time.unscaledTime + 1f;
            try { beacon.Send(beaconPacket, beaconPacket.Length, new IPEndPoint(IPAddress.Broadcast, PlayerViewStream.BeaconPort)); }
            catch (SocketException) { /* No network yet. */ }
        }

        Dictionary<int, byte[]> frames = null;
        lock (incomingLock)
        {
            if (incoming.Count > 0)
            {
                frames = new Dictionary<int, byte[]>(incoming);
                incoming.Clear();
            }
        }
        if (frames == null) return;
        foreach (var pair in frames)
        {
            if (!textures.TryGetValue(pair.Key, out Texture2D texture))
            {
                texture = new Texture2D(2, 2, TextureFormat.RGB24, false) { name = "Player view " + pair.Key, wrapMode = TextureWrapMode.Clamp };
                textures[pair.Key] = texture;
            }
            if (texture.LoadImage(pair.Value, false)) updatedAt[pair.Key] = Time.unscaledTime;
        }
    }

    private void AcceptLoop()
    {
        while (running)
        {
            try
            {
                TcpClient client = listener.AcceptTcpClient();
                client.NoDelay = true;
                lock (clients) clients.Add(client);
                new Thread(() => ReadLoop(client)) { IsBackground = true, Name = "Player view reader" }.Start();
            }
            catch (SocketException) { if (!running) return; }
            catch (ObjectDisposedException) { return; }
        }
    }

    private void ReadLoop(TcpClient client)
    {
        string from = client.Client.RemoteEndPoint?.ToString() ?? "headset";
        Debug.Log($"[PlayerView] {from} connected.");
        try
        {
            using (NetworkStream stream = client.GetStream())
                while (running && PlayerViewStream.TryReadFrame(stream, out int player, out byte[] jpeg))
                    lock (incomingLock) incoming[player] = jpeg;
        }
        catch (Exception e) when (e is System.IO.IOException || e is SocketException || e is ObjectDisposedException
                                  || e is System.IO.InvalidDataException)
        {
            if (running) Debug.Log($"[PlayerView] {from} disconnected: {e.Message}");
        }
        finally
        {
            lock (clients) clients.Remove(client);
            client.Close();
        }
    }

    private void OnDestroy()
    {
        running = false;
        listener?.Stop();
        beacon?.Close();
        lock (clients)
        {
            foreach (TcpClient client in clients) client.Close();
            clients.Clear();
        }
        foreach (Texture2D texture in textures.Values) if (texture != null) Destroy(texture);
    }
}
