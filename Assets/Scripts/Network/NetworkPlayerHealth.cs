using Mirror;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkIdentity))]
public sealed class NetworkPlayerHealth : NetworkBehaviour
{
    [Header("Health")]
    [SerializeField, Min(1)] private int maxHealth = 300;
    [SerializeField, Min(1)] private int headshotDamage = 20;
    [SerializeField, Min(0f)] private float invulnerabilitySeconds = 2f;
    [SerializeField] private bool resetToFullHealthOnZero = false;

    [Header("Health Bar")]
    [SerializeField] private Transform headTarget;
    [SerializeField] private Vector3 worldOffset = new Vector3(0f, 0.32f, 0f);
    [SerializeField, Min(0.05f)] private float barWidth = 0.42f;
    [SerializeField, Min(0.01f)] private float barHeight = 0.045f;
    [SerializeField] private bool showLocalHealthBar = false;
    [SerializeField] private Color backgroundColor = new Color(0.03f, 0.03f, 0.03f, 0.85f);
    [SerializeField] private Color healthyColor = new Color(0.16f, 0.95f, 0.43f, 1f);
    [SerializeField] private Color lowHealthColor = new Color(0.16f, 0.95f, 0.43f, 1f);

    [Header("Debug")]
    [SerializeField] private bool logDamage = true;

    [SyncVar] private int currentHealth;
    public int CurrentHealth => currentHealth;
    [SyncVar] private bool shieldActive;

    private float nextDamageAllowedTime;
    private float nextBlockEventTime;
    private Transform healthBarRoot;
    private DemoHealthBar healthView;
    private NetworkHeadTracker headTracker;
    private CombatDamageFeedback damageFeedback;
    private int lastObservedHealth = -1;

    public bool IsLocalPlayer => isOwned;
    public bool IsAlive => CurrentHealth > 0;
    public bool IsShieldActive => shieldActive;
    public float Health01 => maxHealth <= 0 ? 0f : Mathf.Clamp01(CurrentHealth / (float)maxHealth);

    public override void OnStartClient()
    {
        base.OnStartClient();
        FindHeadTarget();

        EnsureHealthBar();
        damageFeedback = GetComponent<CombatDamageFeedback>() ?? gameObject.AddComponent<CombatDamageFeedback>();
        lastObservedHealth = CurrentHealth;

        UpdateHealthBar();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        if (currentHealth <= 0) currentHealth = maxHealth;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        CombatEventOutput.SetPlayer((int)netId, CurrentHealth);
    }

    private void LateUpdate()
    {
        FindHeadTarget();
        EnsureHealthBar();
        if (lastObservedHealth >= 0 && CurrentHealth < lastObservedHealth
            && !IsLocalPlayer && damageFeedback != null)
            damageFeedback.Play(headTracker != null ? headTracker.HeadWorldPosition : transform.position + Vector3.up * 1.5f);
        lastObservedHealth = CurrentHealth;
        UpdateHealthBar();
    }

    public void RequestHeadshotDamage(string source = "fire")
    {
        RequestDamage(headshotDamage, false, source);
    }

    public void RequestDamage(int damage, bool ignoreShield = false, string source = "unknown")
    {
        if (isServer) ApplyDamage(damage, ignoreShield, source);
        else if (NetworkClient.active) CmdRequestDamage(damage, ignoreShield, source);
    }

    [Command(requiresAuthority = false)]
    private void CmdRequestDamage(int damage, bool ignoreShield, string source, NetworkConnectionToClient sender = null)
    {
        if (sender == null || sender.identity == null || sender.identity == netIdentity) return;
        if (FusionRoundDirector.Active()?.IsFighting != true) return;
        ApplyDamage(damage, ignoreShield, source);
    }

    [Server]
    public void SetShieldActive(bool active) => shieldActive = active;

    public void RequestResetHealth() { if (isServer) ResetHealth(); }

    [Server]
    [ContextMenu("Reset Health")]
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        nextDamageAllowedTime = 0f;
        if (isOwned) ReportReset(currentHealth);
        else if (connectionToClient != null) TargetReset(connectionToClient, currentHealth);
        UpdateHealthBar();
    }

    [TargetRpc]
    private void TargetReset(NetworkConnectionToClient owner, int health) => ReportReset(health);

    private static void ReportReset(int health)
    {
        CombatEventOutput.State("dead", false);
        CombatEventOutput.Emit("health_reset", "round", 0, health);
    }

    [TargetRpc]
    private void TargetDamage(NetworkConnectionToClient owner, string source, int amount, int health, bool blocked)
        => ReportDamage(source, amount, health, blocked);

    private static void ReportDamage(string source, int amount, int health, bool blocked)
    {
        if (blocked) { CombatEventOutput.Emit("shield_block", source, amount, health); return; }
        CombatEventOutput.Emit("hit_received", source, amount, health);
        if (health <= 0)
        {
            CombatEventOutput.State("dead", true);
            CombatEventOutput.Emit("death", source, 0, 0);
        }
    }

    private void ReportToOwner(string source, int amount, bool blocked)
    {
        if (isOwned) ReportDamage(source, amount, currentHealth, blocked);
        else if (connectionToClient != null) TargetDamage(connectionToClient, source, amount, currentHealth, blocked);
    }

    private void ApplyDamage(int damage, bool ignoreShield, string source)
    {
        if (CurrentHealth <= 0) return;
        if (shieldActive && !ignoreShield)
        {
            if (Time.time >= nextBlockEventTime)
            {
                nextBlockEventTime = Time.time + 0.25f;
                ReportToOwner(source, damage, true);
            }
            if (logDamage)
            {
                Debug.Log($"NetworkPlayerHealth: {name} blocked damage with forearm shield.", this);
            }

            return;
        }

        if (Time.time < nextDamageAllowedTime)
        {
            return;
        }

        if (CurrentHealth <= 0)
        {
            return;
        }

        FusionRoundDirector round = FusionRoundDirector.Active();
        if (round != null && round.IsFighting && round.FightElapsed < 20f)
            damage = Mathf.CeilToInt(damage * 0.5f);
        damage = Mathf.Clamp(damage, 1, maxHealth);
        damage = Mathf.Min(damage, CurrentHealth);
        currentHealth = Mathf.Max(0, currentHealth - damage);
        nextDamageAllowedTime = Time.time + invulnerabilitySeconds;
        ReportToOwner(source, damage, false);

        if (logDamage)
        {
            Debug.Log($"NetworkPlayerHealth: {name} took {damage} damage. Health {CurrentHealth}/{maxHealth}. Immune for {invulnerabilitySeconds:0.00}s.", this);
        }

        if (CurrentHealth <= 0 && resetToFullHealthOnZero)
        {
            currentHealth = maxHealth;
            nextDamageAllowedTime = Time.time + invulnerabilitySeconds;

            if (logDamage)
            {
                Debug.Log($"NetworkPlayerHealth: {name} reached 0 health and reset to {maxHealth}.", this);
            }
        }
    }

    private void FindHeadTarget()
    {
        if (headTracker == null)
        {
            headTracker = GetComponent<NetworkHeadTracker>();
        }

        if (headTarget != null)
        {
            return;
        }

        Transform found = transform.Find("HeadHitbox");
        if (found != null)
        {
            headTarget = found;
        }
    }

    private void EnsureHealthBar()
    {
        if (healthView != null) return;
        healthView = new DemoHealthBar(transform, "Opponent health");
        healthBarRoot = healthView.Root;
        healthBarRoot.localScale = Vector3.one * (barWidth / 320f);
    }

    private void UpdateHealthBar()
    {
        if (healthView == null) return;
        bool visible = !IsLocalPlayer || showLocalHealthBar;
        Camera camera = Camera.main;
        Vector3 headPosition = headTracker != null ? headTracker.HeadWorldPosition
            : headTarget != null ? headTarget.position : transform.position;
        if (visible && camera != null)
        {
            int coverLayer = CombatLayers.GameplayCoverLayer;
            Vector3 toHead = headPosition - camera.transform.position;
            if (coverLayer >= 0 && toHead.magnitude > 0.05f)
                visible = !Physics.Raycast(camera.transform.position, toHead.normalized,
                    toHead.magnitude - 0.05f, 1 << coverLayer, QueryTriggerInteraction.Ignore);
        }
        healthBarRoot.gameObject.SetActive(visible);
        if (!visible) return;
        healthBarRoot.position = headPosition + worldOffset;
        if (camera != null)
        {
            Vector3 facing = healthBarRoot.position - camera.transform.position;
            if (facing.sqrMagnitude > 0.0001f)
                healthBarRoot.rotation = Quaternion.LookRotation(facing, Vector3.up);
        }
        healthView.SetHealth(CurrentHealth, Health01);
    }
}
