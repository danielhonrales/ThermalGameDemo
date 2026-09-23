using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

/// <summary>Shared-mode authority for one two-minute duel and its fire-zone waves.</summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkIdentity))]
public sealed class FusionRoundDirector : NetworkBehaviour
{
    public enum RoundPhase { Waiting, Countdown, Fighting, Result }

    [SerializeField, Min(30f)] private float roundSeconds = 120f;
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

    private float nextHazardDamageAt;
    private FusionRoundHud hud;
    private FusionHazardView[] hazardViews;
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
        if (players.Count != 2 || !BothCalibrated(players))
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

        if (HazardStage == 0 && NetworkTime.time >= nextHazardAt)
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
                float pause = PhaseRemaining <= 20f ? finalWavePause : earlyWavePause;
                nextHazardAt = NetworkTime.time + pause;
            }
        }
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
        NetworkHeadTracker localHead = NetworkClient.localPlayer != null
            ? NetworkClient.localPlayer.GetComponent<NetworkHeadTracker>() : null;
        bool insideHazard = localHead != null && HazardStage > 0 &&
            (Vector3.Distance(Flatten(localHead.CanonicalHeadPosition), HazardCenterA) <= hazardRadius ||
            (HazardCount == 2 && Vector3.Distance(Flatten(localHead.CanonicalHeadPosition), HazardCenterB) <= hazardRadius));
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
        if (hazardViews != null)
            foreach (FusionHazardView view in hazardViews)
                if (view != null) Destroy(view.gameObject);
    }
}
