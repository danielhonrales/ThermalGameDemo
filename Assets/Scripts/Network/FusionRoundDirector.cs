using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

/// <summary>Shared-mode authority for one 90-second duel, its fire-zone waves and the mid-round heal.</summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkIdentity))]
public sealed class FusionRoundDirector : NetworkBehaviour
{
    public enum RoundPhase { Waiting, Countdown, Fighting, Result }

    [SerializeField, Min(30f)] private float roundSeconds = 90f;
    [SerializeField, Min(1f)] private float countdownSeconds = 5f;
    [SerializeField, Min(1f)] private float resultSeconds = 7f;
    [SerializeField, Min(0f)] private float firstHazardDelay = 20f;
    [SerializeField, Min(0.5f)] private float warningSeconds = 2.5f;
    [SerializeField, Min(0.5f)] private float burnSeconds = 3f;
    [SerializeField, Min(0.5f)] private float hazardRadius = 1.12f;
    [SerializeField, Min(0f)] private float earlyWavePause = 8f;
    [SerializeField, Min(0f)] private float finalWavePause = 4f;
    [SerializeField, Min(1)] private int hazardDamagePerTick = 12;
    [SerializeField, Min(0.1f)] private float hazardDamageInterval = 0.7f;
    [Header("Testing")]
    [Tooltip("Bypass: with only one headset connected, run the full match solo (no calibration needed). " +
             "A second player joining restores normal two-player rules.")]
    [SerializeField] private bool soloTestMode = true;
    [Header("Sudden death")]
    [Tooltip("The final seconds of the round are sudden death.")]
    [SerializeField, Min(5f)] private float suddenDeathSeconds = 30f;
    [SerializeField, Min(1f)] private float suddenDeathDamageMultiplier = 1.5f;
    [Tooltip("No new fire strikes this long before and after sudden death begins, while drones swap cover.")]
    [SerializeField, Min(0f)] private float swapHazardQuiet = 12f;
    [Header("Heal pickup")]
    [SerializeField, Min(0f)] private float healSpawnSeconds = 30f;
    [SerializeField, Min(1)] private int healAmount = 90;
    [SerializeField, Min(0.1f)] private float healPickupRadius = 0.6f;
    [SerializeField] private float healHeight = 1.55f;
    [Tooltip("Arena-space point the heal floats above (centre of the 1v1 layout, over the coolant stack).")]
    [SerializeField] private Vector3 healArenaPoint = new Vector3(0.275f, 0f, 0.71f);

    [SyncVar] public bool IsDirector;
    [SyncVar] public bool IsCalibrated;
    [SyncVar] public int PhaseCode;
    [SyncVar] private double phaseEndsAt;
    [SyncVar] public int WinnerPlayerId;
    [SyncVar] public int WinsA;
    [SyncVar] public int WinsB;
    [SyncVar] public int HazardSequence;
    [SyncVar] public int HazardStage;
    [SyncVar] public int HazardCount;
    [SyncVar] public Vector3 HazardCenterA;
    [SyncVar] public Vector3 HazardCenterB;
    [SyncVar] private double hazardEndsAt;
    [SyncVar] private double nextHazardAt;
    /// <summary>0 = not yet spawned this round, 1 = available, 2 = taken.</summary>
    [SyncVar] public int HealState;
    [SyncVar] public Vector3 HealCenter;
    [SyncVar] public int HealTakerId = -1;
    /// <summary>Bit per sudden-death drone that has been shot down this round.</summary>
    [SyncVar] public int DroneDownMask;
    [SyncVar] public bool IsSoloTest;

    private float nextHazardDamageAt;
    private FusionRoundHud hud;
    private FusionHazardView[] hazardViews;
    private HealPickupView healView;
    private SuddenDeathDirector suddenDeath;
    private int lastSeenHazardSequence = -1;
    private int lastSeenPhase = -1;
    private static FusionRoundDirector cachedDirector;

    public RoundPhase Phase => (RoundPhase)PhaseCode;
    public float PhaseRemaining => Mathf.Max(0f, (float)(phaseEndsAt - NetworkTime.time));
    public float HazardRemaining => Mathf.Max(0f, (float)(hazardEndsAt - NetworkTime.time));
    public float WarningDuration => warningSeconds;
    public float BurnDuration => burnSeconds;
    public float HazardRadius => hazardRadius;
    public float FightElapsed => Mathf.Max(0f, roundSeconds - PhaseRemaining);
    public bool IsFighting => IsDirector && Phase == RoundPhase.Fighting;
    public float RoundLength => roundSeconds;
    public float SuddenDeathStartsAt => roundSeconds - suddenDeathSeconds;
    /// <summary>Seconds since sudden death began (negative before it). Only meaningful while fighting.</summary>
    public float SuddenDeathClock => Phase == RoundPhase.Fighting ? FightElapsed - SuddenDeathStartsAt : -999f;
    public bool IsSuddenDeath => IsFighting && SuddenDeathClock >= 0f;
    public float DamageMultiplier => IsSuddenDeath ? suddenDeathDamageMultiplier : 1f;

    public bool LocalPlayerInHazard
    {
        get
        {
            if (HazardStage == 0 || NetworkClient.localPlayer == null) return false;
            NetworkHeadTracker head = NetworkClient.localPlayer.GetComponent<NetworkHeadTracker>();
            if (head == null) return false;
            Vector3 position = Flatten(head.CanonicalHeadPosition);
            return Vector3.Distance(position, HazardCenterA) <= hazardRadius
                || (HazardCount == 2 && Vector3.Distance(position, HazardCenterB) <= hazardRadius);
        }
    }

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
        IsSoloTest = soloTestMode && players.Count == 1;
        // Solo bypass: the lone player fills both slots so every two-player rule still runs.
        if (IsSoloTest) players.Add(players[0]);
        else if (players.Count != 2 || !BothCalibrated(players))
        {
            PhaseCode = (int)RoundPhase.Waiting;
            HazardStage = 0;
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
                    nextHazardAt = NetworkTime.time + firstHazardDelay;
                    HazardStage = 0;
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
        NetworkPlayerHealth second = players[1];
        if (first == null || second == null) return;

        if (first.CurrentHealth <= 0 || second.CurrentHealth <= 0 || NetworkTime.time >= phaseEndsAt)
        {
            int winner = first.CurrentHealth == second.CurrentHealth ? -1
                : first.CurrentHealth > second.CurrentHealth ? (int)players[0].netId : (int)players[1].netId;
            WinnerPlayerId = winner;
            if (winner == (int)players[0].netId) WinsA++;
            else if (winner == (int)players[1].netId) WinsB++;
            PhaseCode = (int)RoundPhase.Result;
            phaseEndsAt = NetworkTime.time + resultSeconds;
            HazardStage = 0;
            return;
        }

        UpdateHeal(players);

        float sinceSuddenDeath = FightElapsed - SuddenDeathStartsAt;
        bool swapWindow = sinceSuddenDeath > -swapHazardQuiet * 0.5f && sinceSuddenDeath < swapHazardQuiet;
        if (HazardStage == 0 && NetworkTime.time >= nextHazardAt && !swapWindow)
        {
            StartHazard(players);
        }
        else if (HazardStage == 1 && NetworkTime.time >= hazardEndsAt)
        {
            HazardStage = 2;
            hazardEndsAt = NetworkTime.time + burnSeconds;
            nextHazardDamageAt = Time.time;
        }
        else if (HazardStage == 2)
        {
            if (Time.time >= nextHazardDamageAt)
            {
                nextHazardDamageAt = Time.time + hazardDamageInterval;
                DamagePlayersInsideHazards(players);
            }
            if (NetworkTime.time >= hazardEndsAt)
            {
                HazardStage = 0;
                float pause = PhaseRemaining <= suddenDeathSeconds ? finalWavePause : earlyWavePause;
                nextHazardAt = NetworkTime.time + pause;
            }
        }
    }

    private void ResetHeal()
    {
        HealState = 0;
        HealTakerId = -1;
        DroneDownMask = 0;
    }

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
        NetworkHeadTracker a = players[0].GetComponent<NetworkHeadTracker>();
        NetworkHeadTracker b = players[1].GetComponent<NetworkHeadTracker>();
        if (a == null || b == null) return;
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

    private void StartHazard(List<NetworkPlayerHealth> players)
    {
        NetworkHeadTracker a = players[0].GetComponent<NetworkHeadTracker>();
        NetworkHeadTracker b = players[1].GetComponent<NetworkHeadTracker>();
        if (a == null || b == null) return;
        HazardCenterA = Flatten(a.CanonicalHeadPosition);
        HazardCenterB = Flatten(b.CanonicalHeadPosition);
        float separation = Vector3.Distance(HazardCenterA, HazardCenterB);
        if (separation < hazardRadius * 2f + 0.2f)
        {
            HazardCenterA = Flatten((HazardCenterA + HazardCenterB) * 0.5f);
            HazardCount = 1;
        }
        else HazardCount = 2;
        HazardSequence++;
        HazardStage = 1;
        hazardEndsAt = NetworkTime.time + warningSeconds;
    }

    private void DamagePlayersInsideHazards(List<NetworkPlayerHealth> players)
    {
        foreach (NetworkPlayerHealth player in players)
        {
            NetworkHeadTracker head = player.GetComponent<NetworkHeadTracker>();
            NetworkPlayerHealth health = player;
            if (head == null || health == null) continue;
            Vector3 position = Flatten(head.CanonicalHeadPosition);
            bool inside = Vector3.Distance(position, HazardCenterA) <= hazardRadius
                || (HazardCount == 2 && Vector3.Distance(position, HazardCenterB) <= hazardRadius);
            if (inside) health.RequestDamage(hazardDamagePerTick, true, "hazard");
        }
    }

    private void Update()
    {
        if (!isClient || !IsDirector) return;
        cachedDirector = this;
        if (hud == null)
        {
            hud = FindFirstObjectByType<FusionRoundHud>();
            if (hud == null) hud = new GameObject("Round HUD").AddComponent<FusionRoundHud>();
        }
        hud.Show(this);
        bool insideHazard = LocalPlayerInHazard;
        CombatEventOutput.State("hazard_warning", insideHazard && HazardStage == 1);
        CombatEventOutput.State("hazard", insideHazard && HazardStage == 2);

        if (hazardViews == null)
            hazardViews = new[] {
                new GameObject("Fire Hazard A").AddComponent<FusionHazardView>(),
                new GameObject("Fire Hazard B").AddComponent<FusionHazardView>()
            };
        if (HazardSequence != lastSeenHazardSequence)
        {
            lastSeenHazardSequence = HazardSequence;
            if (HazardStage > 0)
            {
                hazardViews[0].Begin(HazardCenterA, hazardRadius);
                if (HazardCount == 2) hazardViews[1].Begin(HazardCenterB, hazardRadius);
            }
        }
        if (suddenDeath == null) suddenDeath = SuddenDeathDirector.Ensure();
        suddenDeath.Show(this);
        if (healView == null) healView = new GameObject("Heal Pickup").AddComponent<HealPickupView>();
        healView.Show(HealState, HealCenter, HealTakerId,
            NetworkClient.localPlayer != null && HealTakerId == (int)NetworkClient.localPlayer.netId);

        for (int i = 0; i < hazardViews.Length; i++)
            hazardViews[i].Show(i < HazardCount && HazardStage > 0,
                HazardStage, HazardRemaining, warningSeconds, burnSeconds);

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
            CombatEventOutput.State("hazard_warning", false);
            CombatEventOutput.State("hazard", false);
            CombatEventOutput.Emit("round_disconnected", "network");
        }
        if (healView != null) Destroy(healView.gameObject);
        if (cachedDirector == null && suddenDeath != null) suddenDeath.ResetArena();
        if (hazardViews != null)
            foreach (FusionHazardView view in hazardViews)
                if (view != null) Destroy(view.gameObject);
    }
}
