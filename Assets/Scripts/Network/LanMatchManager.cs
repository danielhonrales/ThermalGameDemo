using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using kcp2k;

/// <summary>Starts a two-Quest LAN match with no cloud service or participant UI.</summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(KcpTransport), typeof(NetworkDiscovery))]
public sealed class LanMatchManager : NetworkManager
{
    [Serializable] private sealed class Settings
    {
        public string fallbackHost = "";
        public int port = 7777;
    }

    private const int ElectionPort = 47778;
    private const string BeaconPrefix = "THERMAL-LAN-1:";

    private Settings settings;
    private NetworkDiscovery discovery;
    private Coroutine reconnect;
    private float lastConnectAttempt = -10f;
    private UdpClient election;
    private readonly string electionId = Guid.NewGuid().ToString("N");
    private readonly byte[] beacon = new byte[BeaconPrefix.Length + 32];
    private float nextBeacon;
    private bool yieldingToHost;

    public override void Awake()
    {
        base.Awake();
        maxConnections = 2;
        autoCreatePlayer = true;
        settings = LoadSettings();
        var kcp = GetComponent<KcpTransport>();
        kcp.port = (ushort)Mathf.Clamp(settings.port, 1, 65535);
        transport = kcp;
        discovery = GetComponent<NetworkDiscovery>();
        discovery.transport = kcp;
        discovery.secretHandshake = 0x544845524D414C01L;
        discovery.OnServerFound.AddListener(OnServerFound);
        byte[] bytes = Encoding.ASCII.GetBytes(BeaconPrefix + electionId);
        Array.Copy(bytes, beacon, bytes.Length);
        try
        {
            election = new UdpClient(new IPEndPoint(IPAddress.Any, ElectionPort)) { EnableBroadcast = true };
        }
        catch (SocketException e) { Debug.LogWarning("LAN election: " + e.Message); }
    }

    public override void Start()
    {
        base.Start();
        Debug.Log("LAN: looking for a host Quest; the first headset will host automatically.");
        StartLooking();
    }

    public override void Update()
    {
        base.Update();
        if (election == null) return;
        try
        {
            while (election.Available > 0)
            {
                IPEndPoint from = null;
                byte[] packet = election.Receive(ref from);
                string value = Encoding.ASCII.GetString(packet);
                if (!value.StartsWith(BeaconPrefix, StringComparison.Ordinal)
                    || value.Length != beacon.Length) continue;
                string other = value.Substring(BeaconPrefix.Length);
                if (!Guid.TryParseExact(other, "N", out _) || other == electionId) continue;
                if (NetworkServer.active)
                {
                    if (!yieldingToHost && ShouldYieldTo(electionId, other, numPlayers))
                        StartCoroutine(YieldToHost(from.Address.ToString()));
                }
                else if (!yieldingToHost) ConnectToHost(from.Address.ToString());
            }
            if (NetworkServer.active && Time.unscaledTime >= nextBeacon)
            {
                nextBeacon = Time.unscaledTime + 0.25f;
                election.Send(beacon, beacon.Length, new IPEndPoint(IPAddress.Broadcast, ElectionPort));
            }
        }
        catch (SocketException) { /* Wi-Fi may disappear; Mirror retries when it returns. */ }
        catch (ObjectDisposedException) { }
    }

    public static bool ShouldYieldTo(string ours, string other, int players)
        => players < 2 && string.CompareOrdinal(other, ours) < 0;

    private IEnumerator YieldToHost(string address)
    {
        yieldingToHost = true;
        Debug.Log("LAN: another headset won host election; joining " + address + ".");
        StopHost();
        while (NetworkServer.active || NetworkClient.active) yield return null;
        yieldingToHost = false;
        ConnectToHost(address);
    }

    private static Settings LoadSettings()
    {
        string path = Path.Combine(Application.persistentDataPath, "lan-match.json");
        try
        {
            if (!File.Exists(path)) File.WriteAllText(path, JsonUtility.ToJson(new Settings(), true));
            Settings result = JsonUtility.FromJson<Settings>(File.ReadAllText(path));
            return result ?? new Settings();
        }
        catch (Exception e)
        {
            Debug.LogWarning("LAN settings: " + e.Message);
            return new Settings();
        }
    }

    private void StartLooking()
    {
        if (reconnect != null) StopCoroutine(reconnect);
        reconnect = StartCoroutine(LookForHost());
    }

    private IEnumerator LookForHost()
    {
        while (!NetworkClient.isConnected && !NetworkClient.active)
        {
            discovery.StartDiscovery();
            float started = Time.unscaledTime;
            while (Time.unscaledTime - started < 3f && !NetworkClient.active && !NetworkServer.active)
                yield return null;
            if (NetworkClient.active || NetworkServer.active) yield break;
            discovery.StopDiscovery();
            if (!string.IsNullOrWhiteSpace(settings.fallbackHost))
            {
                ConnectToHost(settings.fallbackHost);
                yield break;
            }
            StartHost();
            discovery.AdvertiseServer();
            Debug.Log($"LAN: hosting the two-player match on UDP {settings.port}.");
            yield break;
        }
    }

    private void OnServerFound(ServerResponse found)
    {
        if (found.uri == null) return;
        ConnectToHost(found.uri.Host);
    }

    private void ConnectToHost(string address)
    {
        if (NetworkClient.active || NetworkServer.active || Time.unscaledTime - lastConnectAttempt < 1f) return;
        discovery.StopDiscovery();
        networkAddress = address;
        lastConnectAttempt = Time.unscaledTime;
        Debug.Log("LAN: found host Quest at " + networkAddress + ".");
        StartClient();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        CombatEventOutput.Emit("round_disconnected", "lan");
        if (!yieldingToHost && !NetworkServer.active)
            StartCoroutine(ReconnectAfterShutdown());
    }

    private IEnumerator ReconnectAfterShutdown()
    {
        // Mirror invokes OnClientDisconnect before it clears NetworkClient.active.
        // Starting discovery in that callback would immediately exit LookForHost.
        while (NetworkClient.active) yield return null;
        yield return null;
        StartLooking();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (numPlayers >= 2) { conn.Disconnect(); return; }
        base.OnServerAddPlayer(conn);
        Debug.Log($"LAN: player {conn.identity?.netId} joined; {numPlayers}/2.");
    }

    public override void OnStopServer()
    {
        discovery?.StopDiscovery();
        base.OnStopServer();
    }

    public override void OnDestroy()
    {
        election?.Dispose();
        base.OnDestroy();
    }
}
