using Fusion;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkObject))]
public sealed class NetworkPlayerHealth : NetworkBehaviour
{
    [Header("Health")]
    [SerializeField, Min(1)] private int maxHealth = 100;
    [SerializeField, Min(1)] private int headshotDamage = 20;
    [SerializeField, Min(0f)] private float invulnerabilitySeconds = 2f;
    [SerializeField] private bool resetToFullHealthOnZero = true;

    [Header("Health Bar")]
    [SerializeField] private Transform headTarget;
    [SerializeField] private Vector3 worldOffset = new Vector3(0f, 0.32f, 0f);
    [SerializeField, Min(0.05f)] private float barWidth = 0.42f;
    [SerializeField, Min(0.01f)] private float barHeight = 0.045f;
    [SerializeField] private bool showLocalHealthBar = false;
    [SerializeField] private Color backgroundColor = new Color(0.03f, 0.03f, 0.03f, 0.85f);
    [SerializeField] private Color healthyColor = new Color(0.1f, 1f, 0.2f, 1f);
    [SerializeField] private Color lowHealthColor = new Color(1f, 0.08f, 0.04f, 1f);

    [Header("Debug")]
    [SerializeField] private bool logDamage = true;

    [Networked] public int CurrentHealth { get; private set; }

    private float nextDamageAllowedTime;
    private Transform healthBarRoot;
    private Transform fillBar;
    private Renderer fillRenderer;

    public bool IsLocalPlayer => Object != null && Object.HasInputAuthority;
    public bool IsAlive => CurrentHealth > 0;
    public float Health01 => maxHealth <= 0 ? 0f : Mathf.Clamp01(CurrentHealth / (float)maxHealth);

    public override void Spawned()
    {
        FindHeadTarget();

        if (Object.HasStateAuthority && CurrentHealth <= 0)
        {
            CurrentHealth = maxHealth;
        }

        EnsureHealthBar();
        UpdateHealthBar();
    }

    private void LateUpdate()
    {
        FindHeadTarget();
        EnsureHealthBar();
        UpdateHealthBar();
    }

    public void RequestHeadshotDamage()
    {
        if (Object == null)
        {
            ApplyHeadshotDamage();
            return;
        }

        if (Object.HasStateAuthority)
        {
            ApplyHeadshotDamage();
            return;
        }

        RPC_RequestHeadshotDamage();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestHeadshotDamage()
    {
        ApplyHeadshotDamage();
    }

    [ContextMenu("Reset Health")]
    public void ResetHealth()
    {
        if (Object != null && !Object.HasStateAuthority)
        {
            return;
        }

        CurrentHealth = maxHealth;
        nextDamageAllowedTime = 0f;
        UpdateHealthBar();
    }

    private void ApplyHeadshotDamage()
    {
        if (Time.time < nextDamageAllowedTime)
        {
            return;
        }

        if (CurrentHealth <= 0)
        {
            return;
        }

        CurrentHealth = Mathf.Max(0, CurrentHealth - headshotDamage);
        nextDamageAllowedTime = Time.time + invulnerabilitySeconds;

        if (logDamage)
        {
            Debug.Log($"NetworkPlayerHealth: {name} took {headshotDamage} headshot damage. Health {CurrentHealth}/{maxHealth}. Immune for {invulnerabilitySeconds:0.00}s.", this);
        }

        if (CurrentHealth <= 0 && resetToFullHealthOnZero)
        {
            CurrentHealth = maxHealth;
            nextDamageAllowedTime = Time.time + invulnerabilitySeconds;

            if (logDamage)
            {
                Debug.Log($"NetworkPlayerHealth: {name} reached 0 health and reset to {maxHealth}.", this);
            }
        }
    }

    private void FindHeadTarget()
    {
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
        if (healthBarRoot != null)
        {
            return;
        }

        GameObject root = new GameObject("HealthBar");
        healthBarRoot = root.transform;
        healthBarRoot.SetParent(transform, false);

        Transform background = CreateBarPart("Background", backgroundColor).transform;
        background.SetParent(healthBarRoot, false);
        background.localScale = new Vector3(barWidth, barHeight, 0.01f);

        fillBar = CreateBarPart("Fill", healthyColor).transform;
        fillBar.SetParent(healthBarRoot, false);
        fillBar.localScale = new Vector3(barWidth, barHeight * 0.68f, 0.012f);
        fillRenderer = fillBar.GetComponent<Renderer>();
    }

    private GameObject CreateBarPart(string partName, Color color)
    {
        GameObject part = GameObject.CreatePrimitive(PrimitiveType.Cube);
        part.name = partName;

        Collider partCollider = part.GetComponent<Collider>();
        if (partCollider != null)
        {
            Destroy(partCollider);
        }

        Renderer renderer = part.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        renderer.material.color = color;
        return part;
    }

    private void UpdateHealthBar()
    {
        if (healthBarRoot == null || fillBar == null)
        {
            return;
        }

        bool visible = (!IsLocalPlayer || showLocalHealthBar) && CurrentHealth > 0;
        healthBarRoot.gameObject.SetActive(visible);
        if (!visible)
        {
            return;
        }

        if (headTarget != null)
        {
            healthBarRoot.position = headTarget.position + worldOffset;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            healthBarRoot.rotation = mainCamera.transform.rotation;
        }

        float health = Health01;
        fillBar.localScale = new Vector3(barWidth * health, barHeight * 0.68f, 0.012f);
        fillBar.localPosition = new Vector3(-barWidth * (1f - health) * 0.5f, 0f, -0.003f);

        if (fillRenderer != null)
        {
            fillRenderer.material.color = Color.Lerp(lowHealthColor, healthyColor, health);
        }
    }
}
