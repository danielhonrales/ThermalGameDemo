using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

/// <summary>Shared authority for a 60-second duel, moving laser sweeps, and the heal.</summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkIdentity))]
public sealed class FusionRoundDirector : NetworkBehaviour
{
    public enum RoundPhase { Waiting, Countdown, Fighting, Result }

    [SerializeField, Min(30f)] private float roundSeconds = 60f;
    [SerializeField, Min(1f)] private float countdownSeconds = 5f;
    [SerializeField, Min(1f)] private float resultSeconds = 7f;
    [SerializeField, Min(1)] private int laserDamage = 20;
    [Header("Sudden death")]
    [Tooltip("The final seconds of the round are sudden death.")]
    [SerializeField, Min(5f)] private float suddenDeathSeconds = 20f;
    [SerializeField, Min(1f)] private float suddenDeathDamageMultiplier = 1.5f;
    [Header("Heal pickup")]
    [SerializeField, Min(0f)] private float healSpawnSeconds = 20f;
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
    /// <summary>0 = not yet spawned this round, 1 = available, 2 = taken.</summary>
    [SyncVar] public int HealState;
    [SyncVar] public Vector3 HealCenter;
    [SyncVar] public int HealTakerId = -1;
    /// <summary>Bit per sudden-death drone that has been shot down this round.</summary>
    [SyncVar] public int DroneDownMask;

    private FusionRoundHud hud;
    private ArenaLaserSweepView laserView;
    private HealPickupView healView;
    private SuddenDeathDirector suddenDeath;
    private int lastSeenPhase = -1;
    private static FusionRoundDirector cachedDirector;
    private HandPoseRouter leftResetPose;
    private HandPoseRouter rightSoloPose;
    private float resetPoseSince = -1f;
    private bool resetRequested;
    private float soloPoseSince = -1f;
    private bool soloPoseWasLeft;
    private bool soloRequested;
    private double nextResetAllowedAt;

    public RoundPhase Phase => (RoundPhase)PhaseCode;
    public float PhaseRemaining => Mathf.Max(0f, (float)(phaseEndsAt - NetworkTime.time));
    public float FightElapsed => Mathf.Max(0f, roundSeconds - PhaseRemaining);
    public bool IsFighting => IsDirector && Phase == RoundPhase.Fighting;
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
        if (!CanStartRound(players.Count, BothCalibrated(players), SoloOverride))
        {
            PhaseCode = (int)RoundPhase.Waiting;
            return;
        }

        switch (Phase)
        {
            case RoundPhase.Waiting:
                PhaseCode = (int)RoundPhase.Countdown;
                phaseEndsAt = NetworkTime.time + countdownSeconds;
                WinnerPlayerId = -1;
                ResetHeal();
                ResetPlayers(players);
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
                    PhaseCode = (int)RoundPhase.Countdown;
                    phaseEndsAt = NetworkTime.time + countdownSeconds;
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

        DamagePlayersInsideLasers(players);
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

    private void UpdateSoloGesture()
    {
        bool waiting = Active()?.Phase == RoundPhase.Waiting && NetworkClient.localPlayer != null;
        bool left = waiting && leftResetPose != null && leftResetPose.IsThumbsUp;
        bool right = waiting && rightSoloPose != null && rightSoloPose.IsThumbsUp;
        if (!left && !right)
        {
            soloPoseSince = -1f;
            soloRequested = false;
            return;
        }
        if (soloPoseSince < 0f || (soloPoseWasLeft ? !left : !right))
        {
            soloPoseSince = Time.time;
            soloPoseWasLeft = left;
            FusionRoundHud.Current?.Toast("HOLD THUMBS UP TO START SOLO", FusionRoundHud.Friendly, SoloHoldSeconds);
        }
        if (soloRequested || Time.time - soloPoseSince < SoloHoldSeconds) return;
        soloRequested = true;
        if (isServer) Active()?.TryStartSolo(netIdentity);
        else CmdStartSolo();
    }

    [Command]
    private void CmdStartSolo() => Active()?.TryStartSolo(netIdentity);

    [Server]
    private void TryStartSolo(NetworkIdentity requester)
    {
        List<NetworkPlayerHealth> players = Players();
        if (Phase != RoundPhase.Waiting || players.Count != 1
            || players[0].netIdentity != requester || !BothCalibrated(players)) return;
        SoloOverride = true;
    }

    [Server]
    private void ResetMatch()
    {
        if (NetworkTime.time < nextResetAllowedAt) return;
        nextResetAllowedAt = NetworkTime.time + 5f;
        List<NetworkPlayerHealth> players = Players();
        PhaseCode = CanStartRound(players.Count, BothCalibrated(players), SoloOverride)
            ? (int)RoundPhase.Countdown : (int)RoundPhase.Waiting;
        phaseEndsAt = NetworkTime.time + (Phase == RoundPhase.Countdown ? countdownSeconds : 0f);
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
        if (HealState == 0 && FightElapsed >= healSpawnSeconds)
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

    private void DamagePlayersInsideLasers(List<NetworkPlayerHealth> players)
    {
        foreach (NetworkPlayerHealth player in players)
        {
            NetworkHeadTracker head = player.GetComponent<NetworkHeadTracker>();
            if (head == null || !player.IsAlive) continue;
            Vector3 position = head.CanonicalHeadPosition;
            for (int i = 0; i < ArenaLaserSweepView.Count; i++)
                if (ArenaLaserSweepView.Hits(i, FightElapsed, position))
                {
                    player.RequestDamage(laserDamage, true, "laser");
                    break;
                }
        }
    }

    private void Update()
    {
        if (isOwned)
        {
            UpdateResetGesture();
            UpdateSoloGesture();
        }
        if (!isClient || !IsDirector) return;
        cachedDirector = this;
        if (hud == null)
        {
            hud = FindFirstObjectByType<FusionRoundHud>();
            if (hud == null) hud = new GameObject("Round HUD").AddComponent<FusionRoundHud>();
        }
        hud.Show(this);
        if (laserView == null) laserView = new GameObject("Arena laser sweeps").AddComponent<ArenaLaserSweepView>();
        laserView.Show(IsFighting, FightElapsed);
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
        if (laserView != null) Destroy(laserView.gameObject);
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused) { resetPoseSince = -1f; soloPoseSince = -1f; }
    }
}
