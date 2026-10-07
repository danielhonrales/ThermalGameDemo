using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/// <summary>Local event log plus optional, nonblocking LAN UDP snapshots for the Pi hardware.
/// Every event is sent <see cref="HardwareLeadSeconds"/> before its visual peak so the
/// Peltiers and vibros have time to respond; the game plays a build-up during that window.</summary>
public sealed class CombatEventOutput : MonoBehaviour
{
    [Serializable] public sealed class Config { public bool udpEnabled = true; public string host = "192.168.1.5"; public int port = 7779; public string deviceLabel = ""; }
    [Serializable] public sealed class Message
    {
        public int schema = 1;
        public string session, device;
        public int player = -1;
        public long seq;
        public double time;
        public string @event, source;
        public int amount, health = -1;
        /// <summary>Attack behind the latest hit or block (fire, ice, ground_fire), so the Pi can heat or cool.
        /// Kept on later messages so a snapshot that makes up a lost hit still names it.</summary>
        public string attack = "";
        public string[] active;
        public int hits, blocks, fireBursts, iceThrows, deaths;
        public int leadMs = Mathf.RoundToInt(HardwareLeadSeconds * 1000f);
    }
    public const float HardwareLeadSeconds = 0.3f;
    /// <summary>Raised for every non-snapshot event (name, source) as it is sent to the Pi.</summary>
    public static event Action<string, string> Signaled;
    private static CombatEventOutput instance;
    private static bool hostSelectionPending;
    private readonly HashSet<string> active = new HashSet<string>();
    private readonly Message message = new Message();
    private Config config = new Config();
    private StreamWriter log;
    private Socket socket;
    private IPEndPoint destination;
    private float nextSnapshot, nextFlush, nextWarning;
    private string logPath;
    private bool suspended;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics() { instance = null; Signaled = null; hostSelectionPending = false; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Boot()
    {
        // A spectator PC is not wearing the forearm hardware and must not appear as a headset to the Pi.
        if (instance != null || SpectatorSession.IsSpectator) return;
        instance = new GameObject("Combat event output").AddComponent<CombatEventOutput>();
        DontDestroyOnLoad(instance.gameObject);
    }

    private void Awake()
    {
        instance = this;
        message.session = Guid.NewGuid().ToString("N");
        const string deviceKey = "thermal.eventDevice";
        if (!PlayerPrefs.HasKey(deviceKey))
        { PlayerPrefs.SetString(deviceKey, Guid.NewGuid().ToString("N").Substring(0, 8)); PlayerPrefs.Save(); }
        message.device = PlayerPrefs.GetString(deviceKey);
        string configPath = Path.Combine(Application.persistentDataPath, "combat-output.json");
        logPath = Path.Combine(Application.persistentDataPath, "combat-events.jsonl");
        try
        {
            config = File.Exists(configPath) ? JsonUtility.FromJson<Config>(File.ReadAllText(configPath)) : new Config();
            if (!File.Exists(configPath)) File.WriteAllText(configPath, JsonUtility.ToJson(config, true));
            if (File.Exists(logPath)) { if (File.Exists(logPath + ".previous")) File.Delete(logPath + ".previous"); File.Move(logPath, logPath + ".previous"); }
            log = new StreamWriter(logPath, false, new UTF8Encoding(false));
            if (!string.IsNullOrWhiteSpace(config.deviceLabel)) message.device = config.deviceLabel;
        }
        catch (Exception e) { Debug.LogWarning("Combat output setup: " + e.Message); }
        Write("session_start", "app", 0);
    }

    // Runs after the first scene's Awake, so a startup Pi menu can claim the destination first.
    private void Start()
    {
        if (!hostSelectionPending) Connect(config.host);
    }

    /// <summary>Holds UDP output until <see cref="SelectHost"/> picks a Pi; the config file's host is ignored.</summary>
    public static void AwaitHostSelection()
    {
        hostSelectionPending = true;
        if (instance == null) return;
        instance.socket?.Dispose();
        instance.socket = null;
        instance.destination = null;
    }

    /// <summary>Sends this headset's output to the given Pi IP (port from combat-output.json).</summary>
    public static bool SelectHost(string host)
    {
        hostSelectionPending = false;
        if (instance == null || !instance.Connect(host)) return false;
        instance.Write("pi_selected", host, 0);
        return true;
    }

    private bool Connect(string host)
    {
        socket?.Dispose();
        socket = null;
        destination = null;
        if (!config.udpEnabled) return false;
        if (!IPAddress.TryParse(host, out var address) || config.port < 1 || config.port > 65535)
        {
            Debug.LogWarning($"Combat output: '{host}' is not a numeric Pi IP, or the port in combat-output.json is invalid.");
            return false;
        }
        try
        {
            destination = new IPEndPoint(address, config.port);
            socket = new Socket(address.AddressFamily, SocketType.Dgram, ProtocolType.Udp) { Blocking = false };
            Debug.Log($"[CombatOutput] Sending to Pi {destination}");
            return true;
        }
        catch (SocketException e) { Debug.LogWarning("Combat output setup: " + e.Message); destination = null; return false; }
    }

    public static void SetPlayer(int id, int health)
    {
        if (instance == null) return;
        instance.message.player = id;
        instance.message.health = health;
        Emit("player_ready", "network", 0, health);
    }

    public static void State(string name, bool enabled)
    {
        if (instance == null || instance.suspended) return;
        bool changed = enabled ? instance.active.Add(name) : instance.active.Remove(name);
        if (changed) instance.Write(name + (enabled ? "_start" : "_stop"), name, 0);
    }

    public static void Emit(string name, string source = "", int amount = 0, int health = -1)
    {
        if (instance == null || instance.suspended) return;
        if (health >= 0) instance.message.health = health;
        switch (name)
        {
            case "hit_received": instance.message.hits++; instance.message.attack = source; break;
            case "shield_block": instance.message.blocks++; instance.message.attack = source; break;
            case "fire_shot": instance.message.fireBursts++; break;
            case "ice_shot": instance.message.iceThrows++; break;
            case "death": instance.message.deaths++; break;
        }
        instance.Write(name, source, amount);
    }

    private void Write(string name, string source, int amount)
    {
        message.seq++;
        message.time = Time.realtimeSinceStartupAsDouble;
        message.@event = name;
        message.source = source;
        message.amount = amount;
        message.active = new string[active.Count];
        active.CopyTo(message.active);
        Array.Sort(message.active);
        string json = JsonUtility.ToJson(message);
        if (name != "snapshot")
        {
            Signaled?.Invoke(name, source);
            Debug.Log("[CombatOutput] " + json);
            try { log?.WriteLine(json); }
            catch (IOException e) { Warn(e); log?.Dispose(); log = null; }
        }
        if (socket == null || destination == null) return;
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            if (bytes.Length <= 1400) socket.SendTo(bytes, destination);
        }
        catch (SocketException e) { Warn(e); }
    }

    private void Update()
    {
        if (suspended) return;
        if (Time.unscaledTime >= nextSnapshot)
        { nextSnapshot = Time.unscaledTime + 0.25f; if (socket != null) Write("snapshot", "", 0); }
        if (Time.unscaledTime < nextFlush) return;
        nextFlush = Time.unscaledTime + 1f;
        try
        {
            log?.Flush();
            if (log != null && log.BaseStream.Length > 1024 * 1024)
            {
                log.Dispose();
                if (File.Exists(logPath + ".previous")) File.Delete(logPath + ".previous");
                File.Move(logPath, logPath + ".previous");
                log = new StreamWriter(logPath, false, new UTF8Encoding(false));
            }
        }
        catch (IOException e) { Warn(e); log = null; }
    }

    private void Warn(Exception e)
    {
        if (Time.unscaledTime < nextWarning) return;
        nextWarning = Time.unscaledTime + 10f;
        Debug.LogWarning("Combat output (game continues): " + e.Message);
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused) { active.Clear(); Write("session_pause", "app", 0); log?.Flush(); }
        suspended = paused;
        if (!paused) Write("session_resume", "app", 0);
    }

    private void OnDestroy()
    {
        active.Clear();
        Write("session_stop", "app", 0);
        socket?.Dispose();
        log?.Dispose();
        if (instance == this) instance = null;
    }
}
