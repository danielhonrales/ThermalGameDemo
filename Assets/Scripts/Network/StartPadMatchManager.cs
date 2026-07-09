using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class StartPadMatchManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public enum AssignedSide
    {
        None,
        PlayerA,
        PlayerB
    }

    [Header("Fusion")]
    [SerializeField] private NetworkRunner runner;

    [Header("Start Pads")]
    [SerializeField] private Transform playerAStart;
    [SerializeField] private Transform playerBStart;
    [SerializeField, Min(0.05f)] private float startZoneRadius = 0.45f;
    [SerializeField] private bool requireLocalPlayerOnAssignedPad = false;

    [Header("Local Tracking")]
    [SerializeField] private Transform localHead;
    [SerializeField] private string localHeadObjectName = "CenterEyeAnchor";

    [Header("Start Flow")]
    [SerializeField, Min(0f)] private float countdownSeconds = 10f;
    [SerializeField] private bool requireColocationReady = true;
    [SerializeField] private ColocationArenaBinder colocationArenaBinder;
    [SerializeField] private bool logStatus = true;

    private bool countdownRunning;
    private bool matchStarted;
    private float countdownStartedAt;
    private float nextMoveToPadLogTime;
    private int lastLoggedRemaining = -1;
    private int lastPlayerCount = -1;
    private NetworkRunner registeredRunner;

    public AssignedSide LocalAssignedSide { get; private set; }
    public bool IsMatchStarted => matchStarted;
    public float CountdownRemaining => countdownRunning
        ? Mathf.Max(0f, countdownSeconds - (Time.time - countdownStartedAt))
        : countdownSeconds;

    private void Awake()
    {
        FindRunner();
        FindLocalHead();
    }

    private void OnEnable()
    {
        RegisterCallbacks();
    }

    private void Start()
    {
        RegisterCallbacks();
    }

    private void OnDisable()
    {
        if (runner != null)
        {
            runner.RemoveCallbacks(this);
            if (registeredRunner == runner)
            {
                registeredRunner = null;
            }
        }
    }

    private void Update()
    {
        FindRunner();
        FindLocalHead();
        TickStartFlow();
    }

    private void TickStartFlow()
    {
        if (runner == null || !runner.IsRunning)
        {
            ResetCountdown();
            return;
        }

        List<PlayerRef> players = GetSortedPlayers();
        int playerCount = players.Count;
        if (playerCount != lastPlayerCount)
        {
            lastPlayerCount = playerCount;
            Log($"Players in room: {playerCount}");
        }

        LocalAssignedSide = GetAssignedSide(players, runner.LocalPlayer);

        if (matchStarted)
        {
            return;
        }

        if (playerCount < 2)
        {
            ResetCountdown();
            return;
        }

        if (requireColocationReady && colocationArenaBinder != null && !colocationArenaBinder.IsColocated)
        {
            ResetCountdown();
            return;
        }

        if (requireLocalPlayerOnAssignedPad && !IsLocalPlayerOnAssignedPad())
        {
            ResetCountdown();
            LogMoveToPadOnce();
            return;
        }

        if (!countdownRunning)
        {
            countdownRunning = true;
            countdownStartedAt = Time.time;
            lastLoggedRemaining = -1;
            Log($"Two players ready. You are {LocalAssignedSide}. Starting {countdownSeconds:0}s countdown.");
        }

        int remaining = Mathf.CeilToInt(CountdownRemaining);
        if (remaining != lastLoggedRemaining)
        {
            lastLoggedRemaining = remaining;
            Log($"Match starts in {remaining}s. Local side: {LocalAssignedSide}");
        }

        if (CountdownRemaining <= 0f)
        {
            matchStarted = true;
            countdownRunning = false;
            Log($"Match started. Local side: {LocalAssignedSide}");
        }
    }

    [ContextMenu("Reset Match Start")]
    public void ResetMatchStart()
    {
        matchStarted = false;
        ResetCountdown();
    }

    public Transform GetLocalAssignedPad()
    {
        return LocalAssignedSide switch
        {
            AssignedSide.PlayerA => playerAStart,
            AssignedSide.PlayerB => playerBStart,
            _ => null
        };
    }

    public bool IsLocalPlayerOnAssignedPad()
    {
        FindLocalHead();

        Transform pad = GetLocalAssignedPad();
        if (localHead == null || pad == null)
        {
            return false;
        }

        Vector3 head = localHead.position;
        Vector3 start = pad.position;
        head.y = 0f;
        start.y = 0f;
        return Vector3.Distance(head, start) <= startZoneRadius;
    }

    private void ResetCountdown()
    {
        countdownRunning = false;
        lastLoggedRemaining = -1;
    }

    private List<PlayerRef> GetSortedPlayers()
    {
        List<PlayerRef> players = new();
        if (runner == null)
        {
            return players;
        }

        foreach (PlayerRef player in runner.ActivePlayers)
        {
            if (player.IsRealPlayer)
            {
                players.Add(player);
            }
        }

        players.Sort((left, right) => left.PlayerId.CompareTo(right.PlayerId));
        return players;
    }

    private static AssignedSide GetAssignedSide(List<PlayerRef> players, PlayerRef player)
    {
        if (!player.IsRealPlayer)
        {
            return AssignedSide.None;
        }

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == player)
            {
                return i == 0 ? AssignedSide.PlayerA : AssignedSide.PlayerB;
            }
        }

        return AssignedSide.None;
    }

    private void LogMoveToPadOnce()
    {
        if (Time.time < nextMoveToPadLogTime)
        {
            return;
        }

        nextMoveToPadLogTime = Time.time + 2f;
        Log($"Move to your assigned start pad: {LocalAssignedSide}");
    }

    private void FindRunner()
    {
        if (runner != null)
        {
            return;
        }

        runner = FindFirstObjectByType<NetworkRunner>();
    }

    private void FindLocalHead()
    {
        if (localHead != null)
        {
            return;
        }

        GameObject found = GameObject.Find(localHeadObjectName);
        if (found != null)
        {
            localHead = found.transform;
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            localHead = mainCamera.transform;
        }
    }

    private void RegisterCallbacks()
    {
        FindRunner();
        if (runner != null && registeredRunner != runner)
        {
            runner.AddCallbacks(this);
            registeredRunner = runner;
        }
    }

    private void Log(string message)
    {
        if (logStatus)
        {
            Debug.Log($"StartPadMatchManager: {message}", this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        DrawStartZone(playerAStart, Color.green);
        DrawStartZone(playerBStart, Color.cyan);
    }

    private void DrawStartZone(Transform pad, Color color)
    {
        if (pad == null)
        {
            return;
        }

        Gizmos.color = color;
        Gizmos.DrawWireSphere(pad.position, startZoneRadius);
    }

    public void OnPlayerJoined(NetworkRunner joinedRunner, PlayerRef player)
    {
        runner = joinedRunner;
        ResetMatchStart();
    }

    public void OnPlayerLeft(NetworkRunner leftRunner, PlayerRef player)
    {
        ResetMatchStart();
    }

    public void OnInput(NetworkRunner inputRunner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner inputRunner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner shutdownRunner, ShutdownReason shutdownReason) { ResetMatchStart(); }
    public void OnConnectedToServer(NetworkRunner connectedRunner) { }
    public void OnDisconnectedFromServer(NetworkRunner disconnectedRunner, NetDisconnectReason reason) { ResetMatchStart(); }
    public void OnConnectRequest(NetworkRunner requestRunner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner failedRunner, NetAddress remoteAddress, NetConnectFailedReason reason) { ResetMatchStart(); }
    public void OnUserSimulationMessage(NetworkRunner messageRunner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner sessionRunner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner authRunner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner migrationRunner, HostMigrationToken hostMigrationToken) { ResetMatchStart(); }
    public void OnSceneLoadDone(NetworkRunner sceneRunner) { }
    public void OnSceneLoadStart(NetworkRunner sceneRunner) { }
    public void OnObjectEnterAOI(NetworkRunner aoiRunner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner aoiRunner, NetworkObject obj, PlayerRef player) { }
#if FUSION_2_1_OR_NEWER
    public void OnReliableDataReceived(NetworkRunner dataRunner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data) { }
#else
    public void OnReliableDataReceived(NetworkRunner dataRunner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
#endif
    public void OnReliableDataProgress(NetworkRunner dataRunner, PlayerRef player, ReliableKey key, float progress) { }
}
