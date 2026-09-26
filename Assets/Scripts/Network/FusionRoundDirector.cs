using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

/// <summary>Shared authority for practice, the 60-second duel, safe zones, and the heal.</summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkIdentity))]
public sealed class FusionRoundDirector : NetworkBehaviour
{
    public enum RoundPhase { Waiting, Sandbox, Countdown, Fighting, Result }

    [SerializeField, Min(30f)] private float roundSeconds = 60f;
    [SerializeField, Min(1f)] private float countdownSeconds = 5f;
    [SerializeField, Min(1f)] private float resultSeconds = 7f;
    [SerializeField, Min(1)] private int safeZoneDamage = 80;
    [Header("Sudden death")]
    [Tooltip("The final seconds of the round are sudden death.")]
    [SerializeField, Min(5f)] private float suddenDeathSeconds = 20f;
    [SerializeField, Min(1f)] private float suddenDeathDamageMultiplier = 1.5f;
    [Header("Heal pickup")]
    [SerializeField, Min(1)] private int healAmount = 90;
    [SerializeField, Min(0.1f)] private float healPickupRadius = 0.6f;
    [SerializeField] private float healHeight = 1.55f;
    [Tooltip("Arena-space point the heal floats above (centre of the 1v1 layout, over the coolant stack).")]
    [SerializeField] private Vector3 healArenaPoint = new Vector3(0.275f, 0f, 0.71f);
    private const float ResetHoldSeconds = 3f;
    private const float SoloHoldSeconds = 3f;

    [SyncVar] public bool IsDirector;
    [SyncVar] public bool IsCalibrated;
    [SyncVar] public int PhaseCode;
    [SyncVar] private double phaseEndsAt;
    [SyncVar] public int WinnerPlayerId;
    [SyncVar] public int WinsA;
    [SyncVar] public int WinsB;
    [SyncVar] public bool SoloOverride;
    [SyncVar] public int HazardSeed;
    /// <summary>0 = not yet spawned this round, 1 = available, 2 = taken.</summary>
    [SyncVar] public int HealState;
    [SyncVar] public Vector3 HealCenter;
    [SyncVar] public int HealTakerId = -1;
    /// <summary>Bit per sudden-death drone that has been shot down this round.</summary>
    [SyncVar] public int DroneDownMask;

    private FusionRoundHud hud;
    private SafeZoneHazardView safeZoneView;
    private SandboxGuideView sandboxGuide;
    private HealPickupView healView;
    private SuddenDeathDirector suddenDeath;
    private int lastSeenPhase = -1;
    private static FusionRoundDirector cachedDirector;
    private HandPoseRouter leftResetPose;
    private HandPoseRouter rightSoloPose;
    private float resetPoseSince = -1f;
    private bool resetRequested;
    private float startPoseSince = -1f;
    private bool startPoseWasLeft;
    private bool startRequested;
    private double nextResetAllowedAt;

    public RoundPhase Phase => (RoundPhase)PhaseCode;
    public float PhaseRemaining => Mathf.Max(0f, (float)(phaseEndsAt - NetworkTime.time));
    public float FightElapsed => Mathf.Max(0f, roundSeconds - PhaseRemaining);
    public bool IsFighting => IsDirector && Phase == RoundPhase.Fighting;
    public bool IsSandbox => IsDirector && Phase == RoundPhase.Sandbox;
    public bool AllowsCombat => IsFighting || IsSandbox;
    public float RoundLength => roundSeconds;
    public float SuddenDeathStartsAt => roundSeconds - suddenDeathSeconds;
    /// <summary>Seconds since sudden death began (negative before it). Only meaningful while fighting.</summary>
    public float SuddenDeathClock => Phase == RoundPhase.Fighting ? FightElapsed - SuddenDeathStartsAt : -999f;
    public bool IsSuddenDeath => IsFighting && SuddenDeathClock >= 0f;
    public float DamageMultiplier => IsSuddenDeath ? suddenDeathDamageMultiplier : 1f;

    public override void OnStartServer()
    {
        base.OnStartServer();
        IsDirector = !FindObjectsByType<FusionRoundDirector>(FindObjectsSortMode.None)
            .Any(other => other != this && other.isServer && other.IsDirector);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        CmdSetCalibration(NetworkPlayerAlignment.HasCalibration);
        var resetHand = new GameObject("Left hand match reset");
        resetHand.transform.SetParent(transform, false);
        leftResetPose = resetHand.AddComponent<HandPoseRouter>();
        leftResetPose.UseLeftHand();
        var soloHand = new GameObject("Right hand solo start");
        soloHand.transform.SetParent(transform, false);
        rightSoloPose = soloHand.AddComponent<HandPoseRouter>();
    }

    [Command]
    private void CmdSetCalibration(bool calibrated) => IsCalibrated = calibrated;

    private void FixedUpdate()
    {
        if (isOwned && IsCalibrated != NetworkPlayerAlignment.HasCalibration)
        {
            if (isServer) IsCalibrated = NetworkPlayerAlignment.HasCalibration;
            else CmdSetCalibration(NetworkPlayerAlignment.HasCalibration);
        }
        if (!isServer || !IsDirector) return;
        List<NetworkPlayerHealth> players = Players();
        if (players.Count == 2 && SoloOverride)
        {
            SoloOverride = false;
            PhaseCode = (int)RoundPhase.Waiting;
            WinsA = WinsB = 0;
            ResetHeal();
        }
        if (!CanEnterSandbox(players.Count, BothCalibrated(players)))
        {
            PhaseCode = (int)RoundPhase.Waiting;
            return;
        }
        if (Phase != RoundPhase.Waiting && Phase != RoundPhase.Sandbox
            && !CanStartRound(players.Count, true, SoloOverride))
        {
            PhaseCode = (int)RoundPhase.Sandbox;
            phaseEndsAt = 0;
            ResetHeal();
            ResetPlayers(players);
            return;
        }

        switch (Phase)
        {
            case RoundPhase.Waiting:
                PhaseCode = (int)RoundPhase.Sandbox;
                phaseEndsAt = 0;
                WinnerPlayerId = -1;
                ResetHeal();
                ResetPlayers(players);
                break;
            case RoundPhase.Sandbox:
                break;
            case RoundPhase.Countdown:
                if (NetworkTime.time >= phaseEndsAt)
                {
                    PhaseCode = (int)RoundPhase.Fighting;
                    phaseEndsAt = NetworkTime.time + roundSeconds;
                }
                break;
            case RoundPhase.Fighting:
                UpdateFight(players);
                break;
            case RoundPhase.Result:
                if (NetworkTime.time >= phaseEndsAt)
                {
                    PhaseCode = (int)RoundPhase.Sandbox;
                    phaseEndsAt = 0;
                    WinnerPlayerId = -1;
                    ResetHeal();
                    ResetPlayers(players);
                }
                break;
        }
    }

    private void UpdateFight(List<NetworkPlayerHealth> players)
    {
        NetworkPlayerHealth first = players[0];
        NetworkPlayerHealth second = players.Count > 1 ? players[1] : null;
        if (first == null) return;

        if (first.CurrentHealth <= 0 || (second != null && second.CurrentHealth <= 0)
            || NetworkTime.time >= phaseEndsAt)
        {
            int winner = second == null ? (first.CurrentHealth > 0 ? (int)first.netId : -1)
                : first.CurrentHealth == second.CurrentHealth ? -1
                : first.CurrentHealth > second.CurrentHealth ? (int)first.netId : (int)second.netId;
            WinnerPlayerId = winner;
            if (second != null && winner == (int)first.netId) WinsA++;
            else if (second != null && winner == (int)second.netId) WinsB++;
            PhaseCode = (int)RoundPhase.Result;
            phaseEndsAt = NetworkTime.time + resultSeconds;
            return;
        }

        UpdateHeal(players);

        DamagePlayersOutsideSafeZones(players);
    }

    private void ResetHeal()
    {
        HealState = 0;
        HealTakerId = -1;
        DroneDownMask = 0;
    }

    private void UpdateResetGesture()
    {
        if (leftResetPose == null || !leftResetPose.IsFirePose)
        {
            resetPoseSince = -1f;
            resetRequested = false;
            return;
        }
        if (resetPoseSince < 0f)
        {
            resetPoseSince = Time.time;
            FusionRoundHud.Current?.Toast("HOLD LEFT FINGER GUN TO RESTART", FusionRoundHud.Friendly, ResetHoldSeconds);
        }
        if (resetRequested || Time.time - resetPoseSince < ResetHoldSeconds) return;
        resetRequested = true;
        if (isServer) Active()?.ResetMatch();
        else CmdResetMatch();
    }

    [Command]
    private void CmdResetMatch() => Active()?.ResetMatch();

    private void UpdateStartGesture()
    {
        bool practicing = Active()?.Phase == RoundPhase.Sandbox && NetworkClient.localPlayer != null;
        bool left = practicing && leftResetPose != null && leftResetPose.IsThumbsUp;
        bool right = practicing && rightSoloPose != null && rightSoloPose.IsThumbsUp;
        if (!left && !right)
        {
            startPoseSince = -1f;
            startRequested = false;
            return;
        }
        if (startPoseSince < 0f || (startPoseWasLeft ? !left : !right))
        {
            startPoseSince = Time.time;
            startPoseWasLeft = left;
            FusionRoundHud.Current?.Toast("HOLD THUMBS UP TO START", FusionRoundHud.Friendly, SoloHoldSeconds);
        }
        if (startRequested || Time.time - startPoseSince < SoloHoldSeconds) return;
        startRequested = true;
        if (isServer) Active()?.TryStartRound(netIdentity);
        else CmdStartRound();
    }

    [Command]
    private void CmdStartRound() => Active()?.TryStartRound(netIdentity);

    [Server]
    private void TryStartRound(NetworkIdentity requester)
    {
        List<NetworkPlayerHealth> players = Players();
        if (Phase != RoundPhase.Sandbox || !CanEnterSandbox(players.Count, BothCalibrated(players))
            || !players.Any(player => player.netIdentity == requester)) return;
        SoloOverride = players.Count == 1;
        StartCountdown(players);
    }

    [Server]
    private void StartCountdown(List<NetworkPlayerHealth> players)
    {
        PhaseCode = (int)RoundPhase.Countdown;
        phaseEndsAt = NetworkTime.time + countdownSeconds;
        WinnerPlayerId = -1;
        HazardSeed = Random.Range(0, SafeZoneHazardView.LayoutCount);
        ResetHeal();
        ResetPlayers(players);
    }

    [Server]
    private void ResetMatch()
    {
        if (NetworkTime.time < nextResetAllowedAt) return;
        nextResetAllowedAt = NetworkTime.time + 5f;
        List<NetworkPlayerHealth> players = Players();
        PhaseCode = CanEnterSandbox(players.Count, BothCalibrated(players))
            ? (int)RoundPhase.Sandbox : (int)RoundPhase.Waiting;
        phaseEndsAt = 0;
        WinnerPlayerId = -1;
        WinsA = WinsB = 0;
        ResetHeal();
        ResetPlayers(players);
        RpcMatchReset();
    }

    [ClientRpc]
    private void RpcMatchReset() => FusionRoundHud.Current?.Toast("MATCH RESTARTING", FusionRoundHud.Friendly, 2f);

    /// <summary>Called on the local player's own director object; forwarded to the match director.</summary>
    public void RequestDroneDown(int index)
    {
        if (index < 0 || index > 30) return;
        if (isServer) Active()?.MarkDroneDown(index);
        else if (isOwned) CmdDroneDown(index);
    }

    [Command]
    private void CmdDroneDown(int index)
    {
        if (index >= 0 && index <= 30) Active()?.MarkDroneDown(index);
    }

    [Server]
    private void MarkDroneDown(int index)
    {
        if (Phase == RoundPhase.Fighting) DroneDownMask |= 1 << index;
    }

    // One heal per round, spawned in the open midway between both players.
    private void UpdateHeal(List<NetworkPlayerHealth> players)
    {
        if (players.Count == 0) return;
        if (HealState == 0 && FightElapsed >= SafeZoneHazardView.HealAt)
        {
            // Float above the arena centre (in the shared canonical frame) so it never spawns in cover.
            Transform arena = GameObject.Find("ArenaRoot")?.transform;
            Vector3 arenaPoint = arena != null ? arena.TransformPoint(healArenaPoint) : healArenaPoint;
            HealCenter = NetworkPlayerAlignment.HasCalibration
                ? Flatten(NetworkPlayerAlignment.InverseTransformPoint(arenaPoint)) + Vector3.up * healHeight
                : Flatten(arenaPoint) + Vector3.up * healHeight;
            HealState = 1;
            return;
        }
        if (HealState != 1) return;
        Vector3 pickup = Flatten(HealCenter);
        NetworkPlayerHealth taker = null;
        float best = healPickupRadius;
        foreach (NetworkPlayerHealth player in players)
        {
            NetworkHeadTracker head = player.GetComponent<NetworkHeadTracker>();
            if (head == null || !player.IsAlive) continue;
            float distance = Vector3.Distance(Flatten(head.CanonicalHeadPosition), pickup);
            if (distance <= best) { best = distance; taker = player; }
        }
        if (taker == null) return;
        taker.Heal(healAmount);
        HealTakerId = (int)taker.netId;
        HealState = 2;
    }

    private void DamagePlayersOutsideSafeZones(List<NetworkPlayerHealth> players)
    {
        if (!SafeZoneHazardView.IsExploding(FightElapsed)) return;
        foreach (NetworkPlayerHealth player in players)
        {
            NetworkHeadTracker head = player.GetComponent<NetworkHeadTracker>();
            if (head == null || !player.IsAlive) continue;
            if (!SafeZoneHazardView.Contains(HazardSeed, head.CanonicalHeadPosition))
                player.RequestDamage(safeZoneDamage, true, "safe_zone");
        }
    }

    private void Update()
    {
        if (isOwned)
        {
            UpdateResetGesture();
            UpdateStartGesture();
        }
        if (!isClient || !IsDirector) return;
        cachedDirector = this;
        if (hud == null)
        {
            hud = FindFirstObjectByType<FusionRoundHud>();
            if (hud == null) hud = new GameObject("Round HUD").AddComponent<FusionRoundHud>();
        }
        hud.Show(this);
        if (safeZoneView == null) safeZoneView = new GameObject("Safe zone hazard").AddComponent<SafeZoneHazardView>();
        safeZoneView.Show(this);
        if (sandboxGuide == null) sandboxGuide = new GameObject("Sandbox ability guide").AddComponent<SandboxGuideView>();
        sandboxGuide.Show(IsSandbox);
        if (suddenDeath == null) suddenDeath = SuddenDeathDirector.Ensure();
        suddenDeath.Show(this);
        if (healView == null) healView = new GameObject("Heal Pickup").AddComponent<HealPickupView>();
        healView.Show(HealState, HealCenter, HealTakerId,
            NetworkClient.localPlayer != null && HealTakerId == (int)NetworkClient.localPlayer.netId);

        if (lastSeenPhase != PhaseCode)
        {
            lastSeenPhase = PhaseCode;
            CombatEventOutput.Emit("round_phase", Phase.ToString().ToLowerInvariant());
            hud.AnnouncePhase(this);
        }
    }

    private static Vector3 Flatten(Vector3 value) => new Vector3(value.x, 0f, value.z);

    private static List<NetworkPlayerHealth> Players()
    {
        var players = new List<NetworkPlayerHealth>();
        foreach (NetworkConnectionToClient connection in NetworkServer.connections.Values)
            if (connection?.identity != null)
            {
                var health = connection.identity.GetComponent<NetworkPlayerHealth>();
                if (health != null) players.Add(health);
            }
        players.Sort((a, b) => a.netId.CompareTo(b.netId));
        return players;
    }

    private static bool BothCalibrated(List<NetworkPlayerHealth> players)
    {
        foreach (NetworkPlayerHealth player in players)
            if (player.GetComponent<FusionRoundDirector>()?.IsCalibrated != true) return false;
        return true;
    }

    public static bool CanStartRound(int playerCount, bool calibrated, bool soloOverride)
        => calibrated && (playerCount == 2 || (playerCount == 1 && soloOverride));

    public static bool CanEnterSandbox(int playerCount, bool calibrated)
        => calibrated && (playerCount == 1 || playerCount == 2);

    private static void ResetPlayers(List<NetworkPlayerHealth> players)
    {
        foreach (NetworkPlayerHealth player in players) player.RequestResetHealth();
    }

    public static FusionRoundDirector Active()
    {
        if (cachedDirector != null && cachedDirector.isActiveAndEnabled && cachedDirector.IsDirector)
            return cachedDirector;
        foreach (FusionRoundDirector director in FindObjectsByType<FusionRoundDirector>(FindObjectsSortMode.None))
            if (director.isActiveAndEnabled && director.IsDirector)
                return cachedDirector = director;
        return null;
    }

    private void OnDestroy()
    {
        if (cachedDirector == this)
        {
            cachedDirector = null;
            CombatEventOutput.Emit("round_disconnected", "network");
        }
        if (healView != null) Destroy(healView.gameObject);
        if (cachedDirector == null && suddenDeath != null) suddenDeath.ResetArena();
        if (safeZoneView != null) Destroy(safeZoneView.gameObject);
        if (sandboxGuide != null) Destroy(sandboxGuide.gameObject);
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused) { resetPoseSince = -1f; startPoseSince = -1f; }
    }
}
