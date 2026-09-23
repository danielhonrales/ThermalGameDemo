using System;
using System.Collections;
using System.IO;
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
        public string role = "host";
        public string fallbackHost = "";
        public int port = 7777;
    }

    private Settings settings;
    private NetworkDiscovery discovery;
    private Coroutine reconnect;
    private float lastConnectAttempt;

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
    }

    public override void Start()
    {
        base.Start();
        if (settings.role.Equals("client", StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("LAN: looking for the host Quest on this router.");
            StartLooking();
        }
        else
        {
            StartHost();
            discovery.AdvertiseServer();
            Debug.Log($"LAN: hosting the two-player match on UDP {settings.port}.");
        }
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
            while (Time.unscaledTime - started < 6f && !NetworkClient.active)
                yield return null;
            if (NetworkClient.active) yield break;
            discovery.StopDiscovery();
            if (!string.IsNullOrWhiteSpace(settings.fallbackHost))
            {
                networkAddress = settings.fallbackHost;
                lastConnectAttempt = Time.unscaledTime;
                StartClient();
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    private void OnServerFound(ServerResponse found)
    {
        if (NetworkClient.active || NetworkServer.active || Time.unscaledTime - lastConnectAttempt < 1f) return;
        if (found.uri == null) return;
        discovery.StopDiscovery();
        networkAddress = found.uri.Host;
        lastConnectAttempt = Time.unscaledTime;
        Debug.Log("LAN: found host Quest at " + networkAddress + ".");
        StartClient();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        CombatEventOutput.Emit("round_disconnected", "lan");
        if (settings != null && settings.role.Equals("client", StringComparison.OrdinalIgnoreCase))
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
}
