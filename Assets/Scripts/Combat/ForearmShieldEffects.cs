using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
public sealed class ForearmShieldEffects : MonoBehaviour
{
    [Header("Shield field")]
    [SerializeField, Min(0.2f)] private float shieldDiscDiameter = 0.5f;
    [SerializeField] private Color shieldColor = new Color(0.65f, 0.43f, 0.85f, 1f);
    [SerializeField, Min(0.05f)] private float deploySeconds = 0.18f;

    private Transform handMount;
    private Transform shieldRoot;
    private MeshRenderer fieldRenderer;
    private Mesh fieldMesh;
    private Material fieldMaterial;
    private Material lineMaterial;
    private LineRenderer outline;
    private LineRenderer innerArc;
    private readonly LineRenderer[] rotatingCells = new LineRenderer[5];
    private AudioSource shieldAudio;
    private AudioClip deployClip;
    private AudioClip impactClip;
    private bool isVisible;
    private bool confirmed;
    private float shownAt;
    private float hitPulseAt = -1f;

    public bool IsVisible => isVisible;

    private void Awake()
    {
        EnsureShield();
        HideShield();
    }

    private void Update()
    {
        if (!isVisible || shieldRoot == null)
        {
            return;
        }

        float deployed = Mathf.SmoothStep(0f, 1f,
            Mathf.Clamp01(0.3f + (Time.time - shownAt) / deploySeconds));
        shieldRoot.localScale = Vector3.one * Mathf.Lerp(0.82f, 1f, deployed);
        float breathing = 0.96f + 0.04f * Mathf.Sin(Time.time * 2.8f);
        float impact = hitPulseAt >= 0f
            ? 1f - Mathf.Clamp01((Time.time - hitPulseAt) / 0.32f) : 0f;

        if (fieldMaterial != null)
        {
            fieldMaterial.color = new Color(1f, 1f, 1f,
                deployed * breathing * (confirmed ? 1f : 0.6f) * (1f + impact * 1.8f));
        }

        if (outline != null)
        {
            Color rim = CombatVfxStyle.WithAlpha(shieldColor,
                Mathf.Min(1f, (0.7f + impact * 0.3f) * deployed));
            outline.startColor = rim;
            outline.endColor = rim;
        }

        if (innerArc != null)
        {
            CombatVfxStyle.SetRing(
                innerArc, Vector3.zero, Quaternion.identity,
                shieldDiscDiameter * 0.36f, 40, Time.time * 28f, 255f);
            innerArc.startColor = CombatVfxStyle.WithAlpha(shieldColor, 0.65f * deployed);
            innerArc.endColor = CombatVfxStyle.WithAlpha(shieldColor, 0.02f);
        }

        for (int i = 0; i < rotatingCells.Length; i++)
        {
            float angle = Time.time * (i == 1 ? -32f : 24f + i * 8f) + i * 20f;
            CombatVfxStyle.SetRing(rotatingCells[i], Vector3.zero,
                Quaternion.identity, shieldDiscDiameter * (0.23f + i * 0.105f),
                6, angle, 300f);
            rotatingCells[i].startColor = CombatVfxStyle.WithAlpha(shieldColor,
                (0.48f + impact * 0.42f) * deployed);
            rotatingCells[i].endColor = CombatVfxStyle.WithAlpha(shieldColor, 0.025f);
        }
    }

    public void ConfigureHandMount(Transform mount)
    {
        handMount = mount;
        if (shieldRoot != null && handMount != null)
        {
            shieldRoot.SetParent(handMount, false);
        }
    }

    public void ShowShieldLocal(Vector3 localPosition, Quaternion localRotation, bool active = true)
    {
        EnsureShield();
        Show(active);
        shieldRoot.localPosition = localPosition;
        shieldRoot.localRotation = localRotation;
        if (shieldAudio != null) shieldAudio.transform.position = shieldRoot.position;
    }

    public void ShowShield(Vector3 worldPosition, Quaternion worldRotation)
    {
        EnsureShield();
        Show();
        shieldRoot.SetPositionAndRotation(worldPosition, worldRotation);
        if (shieldAudio != null) shieldAudio.transform.position = shieldRoot.position;
    }

    public void HideShield()
    {
        isVisible = false;
        confirmed = false;
        if (shieldAudio != null) shieldAudio.Stop();
        if (shieldRoot != null)
        {
            shieldRoot.gameObject.SetActive(false);
        }
    }

    public void PulseImpact()
    {
        if (Time.time - hitPulseAt > 0.2f)
        {
            hitPulseAt = Time.time;
            if (shieldAudio != null && impactClip != null)
                CombatAudioVoice.Play(shieldAudio, impactClip, 0.9f, 0.3f, 5.4f);
        }
    }

    private void Show(bool active = true)
    {
        if (!isVisible)
        {
            isVisible = true;
            shownAt = Time.time;
            shieldRoot.gameObject.SetActive(true);
        }
        if (active && !confirmed && shieldAudio != null && deployClip != null)
            CombatAudioVoice.Play(shieldAudio, deployClip, 0.65f, 0.4f);
        confirmed = active;
    }

    private void EnsureShield()
    {
        if (shieldRoot != null)
        {
            return;
        }

        GameObject root = new GameObject("ForearmShieldField");
        root.transform.SetParent(handMount != null ? handMount : transform, false);
        shieldRoot = root.transform;

        GameObject field = new GameObject("TranslucentHexField");
        field.transform.SetParent(shieldRoot, false);
        MeshFilter filter = field.AddComponent<MeshFilter>();
        fieldMesh = CombatVfxStyle.CreateHexField(shieldDiscDiameter * 0.5f);
        filter.sharedMesh = fieldMesh;
        fieldRenderer = field.AddComponent<MeshRenderer>();
        fieldMaterial = CombatVfxStyle.CreateMaterial("Shield field", Color.white);
        fieldRenderer.sharedMaterial = fieldMaterial;
        fieldRenderer.shadowCastingMode = ShadowCastingMode.Off;
        fieldRenderer.receiveShadows = false;

        lineMaterial = CombatVfxStyle.CreateMaterial("Shield outline", Color.white);
        outline = CombatVfxStyle.CreateLine(shieldRoot, "Six-sided rim",
            lineMaterial, false, 0.0045f);
        outline.positionCount = 7;
        for (int i = 0; i <= 6; i++)
        {
            float angle = (i / 6f + 1f / 12f) * Mathf.PI * 2f;
            float radius = shieldDiscDiameter * 0.5f;
            outline.SetPosition(i,
                new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
        }
        outline.enabled = true;

        innerArc = CombatVfxStyle.CreateLine(shieldRoot, "Quiet status sweep",
            lineMaterial, false, 0.002f);
        for (int i = 0; i < rotatingCells.Length; i++)
            rotatingCells[i] = CombatVfxStyle.CreateLine(shieldRoot,
                "ShieldCellArc", lineMaterial, false, 0.005f);
        shieldAudio = CombatAudioVoice.Create(transform, "Shield audio", null, false);
        deployClip = Resources.Load<AudioClip>("CustomAssets/Audio/AbsorbOrb");
        impactClip = Resources.Load<AudioClip>("CustomAssets/Audio/ChargeBlast");
        root.SetActive(false);
    }

    private void OnDisable() => HideShield();

    private void OnDestroy()
    {
        if (fieldMaterial != null) Destroy(fieldMaterial);
        if (lineMaterial != null) Destroy(lineMaterial);
        if (fieldMesh != null) Destroy(fieldMesh);
    }
}
