using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Client view of the one-per-round heal: a glowing green cross that drops in from above,
/// spins and bobs over a ground marker, then bursts when a player walks through it.
/// </summary>
[DisallowMultipleComponent]
public sealed class HealPickupView : MonoBehaviour
{
    internal static readonly Color Green = new Color(0.22f, 1f, 0.45f, 1f);
    internal static readonly Color GreenCore = new Color(0.8f, 1f, 0.86f, 1f);

    private const float SpawnSeconds = 0.9f;
    private const float BurstSeconds = 0.8f;
    private const int SparkCount = 14;

    private Transform cross;
    private Transform halo;
    private LineRenderer groundRing, groundSweep, dropBeam, haloRing, burstRing;
    private readonly Transform[] sparks = new Transform[SparkCount];
    private Material coreMaterial, glowMaterial, lineMaterial;
    private Mesh cubeMesh;
    private AudioSource audioSource;
    private AudioClip spawnClip, collectClip;
    private int lastState;
    private float stateChangedAt;
    private Vector3 canonicalCenter;

    private void Awake()
    {
        // Lit, emissive, depth-occluded core so the cross reads as a solid glowing object.
        Shader lit = Shader.Find("ThermalGame/OcclusionLitPlus");
        if (lit != null)
        {
            coreMaterial = new Material(lit) { name = "Heal core" };
            coreMaterial.SetColor("_BaseColor", new Color(0.3f, 0.9f, 0.45f));
            coreMaterial.SetFloat("_Metallic", 0.2f);
            coreMaterial.SetFloat("_Smoothness", 0.9f);
            coreMaterial.SetColor("_EmissionColor", new Color(0.4f, 2.6f, 0.9f));
        }
        else coreMaterial = CombatVfxStyle.CreateMaterial("Heal core", GreenCore);
        glowMaterial = CombatVfxStyle.CreateMaterial("Heal glow", CombatVfxStyle.WithAlpha(Green, 0.35f));
        lineMaterial = CombatVfxStyle.CreateMaterial("Heal lines", Color.white);
        cubeMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");

        cross = new GameObject("Green cross").transform;
        cross.SetParent(transform, false);
        // Solid core bars plus a slightly larger translucent shell read as a lit, glowing cross.
        Bar(cross, "Vertical", new Vector3(0.09f, 0.3f, 0.09f), coreMaterial);
        Bar(cross, "Horizontal", new Vector3(0.3f, 0.09f, 0.09f), coreMaterial);
        Bar(cross, "Vertical glow", new Vector3(0.13f, 0.34f, 0.13f), glowMaterial);
        Bar(cross, "Horizontal glow", new Vector3(0.34f, 0.13f, 0.13f), glowMaterial);

        halo = new GameObject("Halo").transform;
        halo.SetParent(transform, false);
        haloRing = CombatVfxStyle.CreateLine(halo, "Halo ring", lineMaterial, false, 0.012f);
        groundRing = CombatVfxStyle.CreateLine(transform, "Ground ring", lineMaterial, true, 0.025f);
        groundSweep = CombatVfxStyle.CreateLine(transform, "Ground sweep", lineMaterial, true, 0.018f);
        dropBeam = CombatVfxStyle.CreateLine(transform, "Drop beam", lineMaterial, true, 0.06f);
        burstRing = CombatVfxStyle.CreateLine(transform, "Collect burst", lineMaterial, true, 0.04f);
        for (int i = 0; i < SparkCount; i++)
        {
            Transform spark = Bar(transform, "Spark", Vector3.one * 0.018f, coreMaterial);
            sparks[i] = spark;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 0.6f;
        audioSource.maxDistance = 10f;
        spawnClip = Resources.Load<AudioClip>("CustomAssets/Audio/alienBoop");
        collectClip = Resources.Load<AudioClip>("CustomAssets/Audio/AbsorbOrb");
        gameObject.SetActive(false);
    }

    private Transform Bar(Transform parent, string name, Vector3 scale, Material material)
    {
        var bar = new GameObject(name);
        bar.transform.SetParent(parent, false);
        bar.transform.localScale = scale;
        bar.AddComponent<MeshFilter>().sharedMesh = cubeMesh;
        var renderer = bar.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = material;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        return bar.transform;
    }

    /// <param name="state">0 hidden, 1 available, 2 taken (see FusionRoundDirector.HealState).</param>
    public void Show(int state, Vector3 center, int takerId, bool takenByLocalPlayer)
    {
        if (state != lastState)
        {
            if (state == 1 || (state == 2 && lastState == 1))
            {
                stateChangedAt = Time.time;
                AudioClip clip = state == 1 ? spawnClip : collectClip;
                if (!gameObject.activeSelf) gameObject.SetActive(true);
                if (clip != null) CombatAudioVoice.Play(audioSource, clip, state == 1 ? 0.8f : 1f, 1f);
            }
            else stateChangedAt = -10f;
            lastState = state;
        }
        canonicalCenter = center;
        bool bursting = state == 2 && Time.time - stateChangedAt < BurstSeconds;
        bool visible = state == 1 || bursting;
        if (gameObject.activeSelf != visible) gameObject.SetActive(visible);
        if (!visible) return;

        Vector3 world = NetworkPlayerAlignment.HasCalibration
            ? NetworkPlayerAlignment.TransformPoint(canonicalCenter) : canonicalCenter;
        transform.position = world;
        Vector3 canonicalFloor = new Vector3(canonicalCenter.x, 0f, canonicalCenter.z);
        Vector3 floor = NetworkPlayerAlignment.HasCalibration
            ? NetworkPlayerAlignment.TransformPoint(canonicalFloor) : canonicalFloor;
        if (state == 1) Animate(world, floor);
        else AnimateBurst(world, floor, takenByLocalPlayer);
    }

    private void Animate(Vector3 world, Vector3 floor)
    {
        float age = Time.time - stateChangedAt;
        float spawn = Mathf.Clamp01(age / SpawnSeconds);
        // Drop in from above with an elastic overshoot.
        float drop = 1f - Mathf.Pow(1f - spawn, 3f);
        float overshoot = 1f + Mathf.Sin(spawn * Mathf.PI) * 0.25f * (1f - spawn);
        float bob = Mathf.Sin(Time.time * 2.2f) * 0.045f;
        cross.position = world + Vector3.up * (Mathf.Lerp(2.2f, 0f, drop) + bob);
        cross.rotation = Quaternion.Euler(0f, Time.time * 120f, Mathf.Sin(Time.time * 1.3f) * 6f);
        float pulse = 1f + 0.06f * Mathf.Sin(Time.time * 6f);
        cross.localScale = Vector3.one * drop * overshoot * pulse;
        glowMaterial.color = CombatVfxStyle.WithAlpha(Green, 0.28f + 0.12f * Mathf.Sin(Time.time * 6f));

        halo.position = cross.position;
        Camera camera = Camera.main;
        if (camera != null) halo.rotation = Quaternion.LookRotation(halo.position - camera.transform.position);
        CombatVfxStyle.SetRing(haloRing, Vector3.zero, Quaternion.identity, 0.26f * drop * pulse, 48,
            Time.time * 90f, 290f);
        haloRing.startColor = CombatVfxStyle.WithAlpha(Green, 0.8f * drop);
        haloRing.endColor = CombatVfxStyle.WithAlpha(Green, 0.05f);

        Quaternion flat = Quaternion.Euler(90f, 0f, 0f);
        CombatVfxStyle.SetRing(groundRing, floor + Vector3.up * 0.02f, flat, 0.5f * drop, 64);
        groundRing.startColor = groundRing.endColor = CombatVfxStyle.WithAlpha(Green, 0.55f * drop);
        CombatVfxStyle.SetRing(groundSweep, floor + Vector3.up * 0.03f, flat, 0.42f, 24,
            Time.time * 140f, 70f);
        groundSweep.startColor = CombatVfxStyle.WithAlpha(GreenCore, 0.9f * drop);
        groundSweep.endColor = CombatVfxStyle.WithAlpha(Green, 0f);

        // Light column that marks the drop, then fades once the cross has landed.
        float beam = 1f - Mathf.Clamp01((age - SpawnSeconds) / 0.6f);
        dropBeam.enabled = beam > 0f;
        if (beam > 0f)
        {
            dropBeam.positionCount = 2;
            dropBeam.SetPosition(0, floor);
            dropBeam.SetPosition(1, floor + Vector3.up * 3.2f);
            dropBeam.startColor = CombatVfxStyle.WithAlpha(GreenCore, 0.7f * beam);
            dropBeam.endColor = CombatVfxStyle.WithAlpha(Green, 0f);
        }
        burstRing.enabled = false;

        for (int i = 0; i < SparkCount; i++)
        {
            float t = Mathf.Repeat(Time.time * 0.45f + i / (float)SparkCount, 1f);
            float angle = i * 2.399963f + Time.time * 0.8f;
            float radius = 0.42f * (1f - t * 0.6f);
            sparks[i].position = floor + new Vector3(Mathf.Cos(angle) * radius,
                t * (world.y - floor.y + 0.35f), Mathf.Sin(angle) * radius);
            sparks[i].localScale = Vector3.one * 0.02f * Mathf.Sin(t * Mathf.PI) * drop;
            sparks[i].rotation = Quaternion.Euler(t * 360f, angle * 57f, 0f);
        }
    }

    private void AnimateBurst(Vector3 world, Vector3 floor, bool local)
    {
        float t = Mathf.Clamp01((Time.time - stateChangedAt) / BurstSeconds);
        float fade = 1f - t;
        cross.position = world + Vector3.up * (t * 0.5f);
        cross.localScale = Vector3.one * (1f + t * 1.6f) * fade;
        cross.rotation = Quaternion.Euler(0f, Time.time * 720f, 0f);
        glowMaterial.color = CombatVfxStyle.WithAlpha(Green, 0.6f * fade);
        haloRing.enabled = groundSweep.enabled = dropBeam.enabled = false;
        CombatVfxStyle.SetRing(groundRing, floor + Vector3.up * 0.02f, Quaternion.Euler(90f, 0f, 0f),
            0.5f + t * 1.4f, 64);
        groundRing.startColor = groundRing.endColor = CombatVfxStyle.WithAlpha(Green, 0.7f * fade);
        CombatVfxStyle.SetRing(burstRing, cross.position, Quaternion.Euler(90f, 0f, 0f),
            0.2f + t * (local ? 1.1f : 0.8f), 48);
        burstRing.startColor = burstRing.endColor = CombatVfxStyle.WithAlpha(GreenCore, fade);
        for (int i = 0; i < SparkCount; i++)
        {
            float angle = i * 2.399963f;
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0.4f + (i % 4) * 0.25f, Mathf.Sin(angle));
            sparks[i].position = world + direction * (t * 0.9f);
            sparks[i].localScale = Vector3.one * 0.035f * fade;
        }
    }

    private void OnDestroy()
    {
        if (coreMaterial != null) Destroy(coreMaterial);
        if (glowMaterial != null) Destroy(glowMaterial);
        if (lineMaterial != null) Destroy(lineMaterial);
    }
}
