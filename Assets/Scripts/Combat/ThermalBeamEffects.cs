using UnityEngine;
using UnityEngine.Rendering;

/// <summary>Layered red heat beam with flowing filaments and contact bursts.</summary>
[DisallowMultipleComponent]
public sealed class ThermalBeamEffects : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private string beamLoopResourcePath = "CustomAssets/Audio/fireBeam";
    [SerializeField, Range(0f, 1f)] private float beamVolume = 0.5f;
    [SerializeField] private string chargeAudioResourcePath = "CustomAssets/Audio/beamCharge";
    [SerializeField, Range(0f, 1f)] private float chargeVolume = 0.45f;

    private readonly Vector3[] beamPoints = new Vector3[25];
    private readonly Vector3[] strandPointsA = new Vector3[25];
    private readonly Vector3[] strandPointsB = new Vector3[25];
    private readonly LineRenderer[] impactStreaks = new LineRenderer[18];
    private readonly LineRenderer[] chargeHelices = new LineRenderer[3];
    private Material lineMaterial;
    private Material shardMaterial;
    private Mesh shardMesh;
    private LineRenderer beamHalo;
    private LineRenderer beamBody;
    private LineRenderer beamCore;
    private LineRenderer strandA;
    private LineRenderer strandB;
    private LineRenderer chargeArc;
    private LineRenderer chargeOuterArc;
    private LineRenderer aimRing;
    private LineRenderer rangeRing;
    private LineRenderer impactRing;
    private LineRenderer impactInnerRing;
    private Transform chargeRoot;
    private Transform chargeShard;
    private AudioSource beamAudio;
    private AudioSource chargeAudio;
    private AudioSource impactAudio;
    private float impactAt = -1f;
    private float lastImpactAudioAt = -10f;
    private Vector3 impactPosition;
    private Quaternion impactRotation;
    private Color impactColor;

    private void Awake()
    {
        EnsureEffects();
        HideBeam();
    }

    private void Update()
    {
        if (impactAt < 0f || impactRing == null) return;
        float age = (Time.time - impactAt) / 0.47f;
        if (age >= 1f)
        {
            impactRing.enabled = false;
            impactInnerRing.enabled = false;
            foreach (LineRenderer streak in impactStreaks) streak.enabled = false;
            impactAt = -1f;
            return;
        }

        CombatVfxStyle.SetRing(impactRing, impactPosition, impactRotation,
            Mathf.Lerp(0.045f, 0.29f, age), 48);
        CombatVfxStyle.SetRing(impactInnerRing, impactPosition, impactRotation,
            Mathf.Lerp(0.05f, 0.13f, age), 32);
        Color tint = CombatVfxStyle.WithAlpha(impactColor,
            0.98f * (1f - age));
        impactRing.startColor = tint;
        impactRing.endColor = tint;
        impactInnerRing.startColor = CombatVfxStyle.WithAlpha(
            CombatVfxStyle.HeatCore, 0.65f * (1f - age));
        impactInnerRing.endColor = impactInnerRing.startColor;
        for (int i = 0; i < impactStreaks.Length; i++)
        {
            float angle = (i + 0.2f) * Mathf.PI * 2f / impactStreaks.Length;
            Vector3 direction = impactRotation * new Vector3(Mathf.Cos(angle),
                Mathf.Sin(angle), 0f);
            LineRenderer streak = impactStreaks[i];
            streak.positionCount = 2;
            float stagger = 0.65f + (i % 5) * 0.13f;
            streak.SetPosition(0, impactPosition + direction * (0.06f + age * 0.13f) * stagger);
            streak.SetPosition(1, impactPosition + direction * (0.15f + age * 0.29f) * stagger);
            streak.startColor = tint;
            streak.endColor = CombatVfxStyle.WithAlpha(impactColor, 0f);
            streak.enabled = true;
        }
    }

    public void ShowBeam(Vector3 start, Vector3 end, Color color, bool hitSomething)
    {
        ShowBeam(start, end, color, hitSomething, end, start, Quaternion.identity);
    }

    public void ShowBeam(Vector3 start, Vector3 end, Color color, bool hitSomething,
        Vector3 hitPoint, Vector3 mountPosition, Quaternion mountRotation)
    {
        EnsureEffects();
        HideChargeOnly();

        Vector3 direction = end - start;
        float length = direction.magnitude;
        if (length < 0.01f)
        {
            beamHalo.enabled = beamBody.enabled = beamCore.enabled = false;
            strandA.enabled = strandB.enabled = false;
            return;
        }

        Vector3 sideways = Vector3.Cross(direction.normalized, Vector3.up);
        if (sideways.sqrMagnitude < 0.01f)
            sideways = Vector3.Cross(direction.normalized, Vector3.right);
        sideways.Normalize();
        float sway = Mathf.Min(0.012f, length * 0.006f)
            * Mathf.Sin(Time.time * 19f);
        CombatVfxStyle.SetBezier(beamPoints, start,
            Vector3.Lerp(start, end, 0.32f) + sideways * sway,
            Vector3.Lerp(start, end, 0.68f) - sideways * sway, end);

        Vector3 secondAxis = Vector3.Cross(direction.normalized, sideways);
        for (int i = 0; i < beamPoints.Length; i++)
        {
            float t = i / (float)(beamPoints.Length - 1);
            float envelope = Mathf.Sin(t * Mathf.PI);
            float phase = t * Mathf.PI * 9f - Time.time * 17f;
            Vector3 orbit = (sideways * Mathf.Cos(phase)
                + secondAxis * Mathf.Sin(phase)) * (0.038f * envelope);
            strandPointsA[i] = beamPoints[i] + orbit;
            strandPointsB[i] = beamPoints[i] - orbit;
        }
        SetBeamLine(beamHalo, beamPoints,
            CombatVfxStyle.WithAlpha(CombatVfxStyle.Heat, 0.34f));
        SetBeamLine(beamBody, beamPoints,
            CombatVfxStyle.WithAlpha(CombatVfxStyle.Heat, 0.82f));
        SetBeamLine(beamCore, beamPoints,
            CombatVfxStyle.WithAlpha(CombatVfxStyle.HeatCore, 1f));
        SetBeamLine(strandA, strandPointsA,
            CombatVfxStyle.WithAlpha(CombatVfxStyle.Heat, 0.72f));
        SetBeamLine(strandB, strandPointsB,
            CombatVfxStyle.WithAlpha(CombatVfxStyle.HeatCore, 0.6f));

        if (hitSomething && (impactAt < 0f
            || Time.time - impactAt > 0.28f
            || Vector3.Distance(impactPosition, hitPoint) > 0.12f))
        {
            impactPosition = hitPoint;
            impactRotation = Quaternion.LookRotation(-direction.normalized);
            impactColor = color.b > color.r * 0.7f
                ? CombatVfxStyle.Shield : CombatVfxStyle.Heat;
            impactAt = Time.time;
            if (impactAudio != null && impactAudio.clip != null
                && Time.time - lastImpactAudioAt >= 0.65f)
            {
                lastImpactAudioAt = Time.time;
                impactAudio.transform.position = hitPoint;
                impactAudio.pitch = 0.92f + Random.value * 0.16f;
                CombatAudioVoice.Play(impactAudio, impactAudio.clip, 0.75f, 0.3f);
            }
        }

        if (beamAudio != null && beamAudio.clip != null)
        {
            beamAudio.transform.position = mountPosition;
            if (!beamAudio.isPlaying) beamAudio.Play();
        }
    }

    public void ShowCharge(Vector3 start, Color color)
    {
        ShowCharge(start, color, 0f, start, Quaternion.identity);
    }

    public void ShowCharge(Vector3 start, Color color, float progress)
    {
        ShowCharge(start, color, progress, start, Quaternion.identity);
    }

    public void ShowCharge(Vector3 start, Color color, float progress,
        Vector3 mountPosition, Quaternion mountRotation, bool playAudio = true)
    {
        EnsureEffects();
        beamHalo.enabled = beamBody.enabled = beamCore.enabled = false;
        strandA.enabled = strandB.enabled = false;
        if (beamAudio != null && beamAudio.isPlaying) beamAudio.Stop();

        progress = Mathf.Clamp01(progress);
        chargeRoot.gameObject.SetActive(true);
        chargeRoot.SetPositionAndRotation(start, mountRotation);
        float scale = Mathf.Lerp(0.05f, 0.095f, progress)
            * (1f + 0.035f * Mathf.Sin(Time.time * 10f));
        chargeShard.localScale = Vector3.one * scale;
        chargeShard.localRotation = Quaternion.Euler(0f, Time.time * 80f, 0f);
        CombatVfxStyle.SetRing(chargeArc, Vector3.zero, Quaternion.identity,
            Mathf.Lerp(0.07f, 0.14f, progress), 40,
            -90f + Time.time * 55f, Mathf.Lerp(90f, 285f, progress));
        chargeArc.startColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.Heat,
            Mathf.Lerp(0.22f, 0.63f, progress));
        chargeArc.endColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.Heat, 0.03f);
        CombatVfxStyle.SetRing(chargeOuterArc, Vector3.zero, Quaternion.identity,
            Mathf.Lerp(0.12f, 0.19f, progress), 48,
            90f - Time.time * 85f, Mathf.Lerp(100f, 245f, progress));
        chargeOuterArc.startColor = CombatVfxStyle.WithAlpha(
            CombatVfxStyle.HeatCore, 0.42f + progress * 0.35f);
        chargeOuterArc.endColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.Heat, 0.02f);
        for (int i = 0; i < chargeHelices.Length; i++)
        {
            LineRenderer helix = chargeHelices[i];
            CombatVfxStyle.SetHelix(helix, 0.32f, 0.045f + progress * 0.032f,
                2.5f, Time.time * (i % 2 == 0 ? 8f : -7f) + i * 2.094f);
            Color tint = CombatVfxStyle.WithAlpha(i == 1 ? CombatVfxStyle.HeatCore : CombatVfxStyle.Heat,
                0.55f + progress * 0.4f);
            helix.startColor = tint;
            helix.endColor = CombatVfxStyle.WithAlpha(tint, 0.18f);
        }

        if (chargeAudio != null && chargeAudio.clip != null)
        {
            chargeAudio.transform.position = mountPosition;
            chargeAudio.volume = chargeVolume * Mathf.Lerp(0.25f, 1f, progress);
            if (playAudio && !chargeAudio.isPlaying) chargeAudio.Play();
            if (!playAudio && chargeAudio.isPlaying) chargeAudio.Stop();
        }
    }

    public void HideBeam()
    {
        if (beamHalo != null) beamHalo.enabled = false;
        if (beamBody != null) beamBody.enabled = false;
        if (beamCore != null) beamCore.enabled = false;
        if (strandA != null) strandA.enabled = false;
        if (strandB != null) strandB.enabled = false;
        HideChargeOnly();
        if (beamAudio != null && beamAudio.isPlaying) beamAudio.Stop();
        if (impactAudio != null) impactAudio.Stop();
        HideAimMarker();
        HideRangeLimitMarker();
    }

    public void ShowAimMarker(Vector3 position, Color color)
    {
        EnsureEffects();
        Camera eye = Camera.main;
        Quaternion facing = eye != null ? eye.transform.rotation : Quaternion.identity;
        CombatVfxStyle.SetRing(aimRing, position, facing, 0.045f, 32);
        Color tint = CombatVfxStyle.WithAlpha(CombatVfxStyle.Heat, 0.9f);
        aimRing.startColor = tint;
        aimRing.endColor = tint;
    }

    public void HideAimMarker()
    {
        if (aimRing != null) aimRing.enabled = false;
    }

    public void ShowRangeLimitMarker(Vector3 position, Color color)
    {
        EnsureEffects();
        CombatVfxStyle.SetRing(rangeRing, position, Quaternion.identity, 0.045f, 24);
        Color tint = CombatVfxStyle.WithAlpha(CombatVfxStyle.Heat, 0.22f);
        rangeRing.startColor = tint;
        rangeRing.endColor = tint;
    }

    public void HideRangeLimitMarker()
    {
        if (rangeRing != null) rangeRing.enabled = false;
    }

    private void HideChargeOnly()
    {
        if (chargeRoot != null) chargeRoot.gameObject.SetActive(false);
        if (chargeAudio != null && chargeAudio.isPlaying) chargeAudio.Stop();
    }

    private static void SetBeamLine(LineRenderer line, Vector3[] points, Color tint)
    {
        line.positionCount = points.Length;
        line.SetPositions(points);
        line.startColor = tint;
        line.endColor = tint;
        line.enabled = true;
    }

    private void EnsureEffects()
    {
        if (lineMaterial != null) return;
        lineMaterial = CombatVfxStyle.CreateMaterial("Heat lines", Color.white);
        shardMaterial = CombatVfxStyle.CreateMaterial("Heat charge", CombatVfxStyle.HeatCore);
        shardMesh = CombatVfxStyle.CreateShard();
        beamHalo = CombatVfxStyle.CreateLine(transform, "Heat atmosphere", lineMaterial, true, 0.19f);
        beamBody = CombatVfxStyle.CreateLine(transform, "Heat flow", lineMaterial, true, 0.09f);
        beamCore = CombatVfxStyle.CreateLine(transform, "Heat filament", lineMaterial, true, 0.026f);
        AnimationCurve beamProfile = new AnimationCurve(
            new Keyframe(0f, 0.06f), new Keyframe(0.08f, 0.35f),
            new Keyframe(0.25f, 1f), new Keyframe(0.85f, 1f),
            new Keyframe(1f, 0.6f));
        beamHalo.widthCurve = beamProfile;
        beamBody.widthCurve = beamProfile;
        beamCore.widthCurve = beamProfile;
        strandA = CombatVfxStyle.CreateLine(transform, "Heat spiral A", lineMaterial, true, 0.022f);
        strandB = CombatVfxStyle.CreateLine(transform, "Heat spiral B", lineMaterial, true, 0.017f);
        aimRing = CombatVfxStyle.CreateLine(transform, "Heat aim", lineMaterial, true, 0.002f);
        rangeRing = CombatVfxStyle.CreateLine(transform, "Heat range", lineMaterial, true, 0.002f);
        impactRing = CombatVfxStyle.CreateLine(transform, "Heat contact", lineMaterial, true, 0.022f);
        impactInnerRing = CombatVfxStyle.CreateLine(transform, "Heat contact core", lineMaterial, true, 0.013f);
        for (int i = 0; i < impactStreaks.Length; i++)
            impactStreaks[i] = CombatVfxStyle.CreateLine(transform,
                "Heat impact streak", lineMaterial, true, i % 3 == 0 ? 0.022f : 0.014f);

        GameObject root = new GameObject("HeatCharge");
        root.transform.SetParent(transform, false);
        chargeRoot = root.transform;
        GameObject shard = new GameObject("HeatCore");
        shard.transform.SetParent(chargeRoot, false);
        shard.AddComponent<MeshFilter>().sharedMesh = shardMesh;
        MeshRenderer renderer = shard.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = shardMaterial;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        chargeShard = shard.transform;
        chargeArc = CombatVfxStyle.CreateLine(chargeRoot, "HeatChargeArc", lineMaterial, false, 0.003f);
        chargeOuterArc = CombatVfxStyle.CreateLine(chargeRoot, "HeatChargeOrbit", lineMaterial, false, 0.006f);
        for (int i = 0; i < chargeHelices.Length; i++)
            chargeHelices[i] = CombatVfxStyle.CreateLine(chargeRoot,
                "Heat forearm spiral", lineMaterial, false, i == 1 ? 0.012f : 0.008f);
        chargeRoot.gameObject.SetActive(false);

        beamAudio = CreateAudio(beamLoopResourcePath, beamVolume);
        chargeAudio = CreateAudio(chargeAudioResourcePath, chargeVolume);
        impactAudio = CreateAudio("CustomAssets/Audio/FireBlast", 0.7f);
        if (impactAudio != null) impactAudio.loop = false;
    }

    private AudioSource CreateAudio(string path, float volume)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip == null) return null;
        return CombatAudioVoice.Create(transform, "Heat audio", clip, true, volume);
    }

    private void OnDisable()
    {
        HideBeam();
        if (impactAudio != null) impactAudio.Stop();
    }

    private void OnDestroy()
    {
        if (lineMaterial != null) Destroy(lineMaterial);
        if (shardMaterial != null) Destroy(shardMaterial);
        if (shardMesh != null) Destroy(shardMesh);
    }
}
