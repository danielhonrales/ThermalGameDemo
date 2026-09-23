using UnityEngine;
using UnityEngine.Rendering;

/// <summary>Compact cyan charge, spline flight trail, and brief cold impact.</summary>
[DisallowMultipleComponent]
public sealed class IceGrenadeEffects : MonoBehaviour
{
    public const int NoPlayerContact = 0;
    public const int PlayerContact = 1;
    public const int ShieldContact = 2;

    [SerializeField, Min(0.1f)] private float explosionRadius = 1.65f;
    [SerializeField] private float grenadeVisualScale = 0.14f;
    [SerializeField] private string chargeAudioResourcePath = "CustomAssets/Audio/iceBeam";
    [SerializeField, Range(0f, 1f)] private float chargeVolume = 0.45f;
    [SerializeField] private string explosionAudioResourcePath = "CustomAssets/Audio/IceBlast";
    [SerializeField, Range(0f, 1f)] private float explosionVolume = 0.65f;

    private readonly Vector3[] history = new Vector3[9];
    private readonly Transform[] fragments = new Transform[36];
    private readonly Vector3[] fragmentDirections = new Vector3[36];
    private readonly LineRenderer[] blizzard = new LineRenderer[32];
    private readonly LineRenderer[] chargeHelices = new LineRenderer[3];
    private readonly LineRenderer[] hitBrackets = new LineRenderer[4];
    private Material lineMaterial;
    private Material shardMaterial;
    private Material radiusMaterial;
    private Mesh shardMesh;
    private Mesh radiusMesh;
    private Transform chargeRoot;
    private Transform chargeShard;
    private Transform grenadeTemplate;
    private Transform impactRoot;
    private Transform radiusField;
    private Transform hitRoot;
    private Transform trailTarget;
    private Transform flyingVisual;
    private LineRenderer chargeArc;
    private LineRenderer chargeOrbit;
    private LineRenderer trail;
    private LineRenderer trailOrbit;
    private LineRenderer radiusRing;
    private LineRenderer innerRadiusRing;
    private LineRenderer impactArc;
    private LineRenderer domeArcA;
    private LineRenderer domeArcB;
    private LineRenderer hitRing;
    private AudioSource chargeAudio;
    private AudioSource impactAudio;
    private int historyCount;
    private float lastSampleAt;
    private float impactAt = -1f;
    private float hitAt = -1f;
    private Color hitColor;

    public float ExplosionRadius => explosionRadius;

    private void Awake()
    {
        EnsureEffects();
        HideAll();
        impactRoot.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateTrail();
        UpdateImpact();
        UpdateHitCue();
    }

    public Transform CreateGrenadeVisual(Transform parent)
    {
        EnsureEffects();
        Transform visual = Instantiate(grenadeTemplate, parent, false);
        visual.localPosition = Vector3.zero;
        visual.localRotation = Quaternion.identity;
        visual.localScale = Vector3.one * grenadeVisualScale;
        visual.gameObject.SetActive(true);
        return visual;
    }

    public void ShowCharge(Vector3 palmPosition, Quaternion palmRotation, float progress, bool playAudio = true)
    {
        EnsureEffects();
        progress = Mathf.Clamp01(progress);
        chargeRoot.gameObject.SetActive(true);
        chargeRoot.SetPositionAndRotation(palmPosition + palmRotation * Vector3.forward * 0.03f,
            palmRotation);
        chargeShard.localScale = Vector3.one * Mathf.Lerp(0.09f, 0.16f, progress);
        chargeShard.localRotation = Quaternion.Euler(Time.time * 65f, Time.time * 110f, 0f);
        CombatVfxStyle.SetRing(chargeArc, Vector3.zero, Quaternion.identity,
            Mathf.Lerp(0.075f, 0.15f, progress), 40,
            90f - Time.time * 45f, Mathf.Lerp(90f, 285f, progress));
        chargeArc.startColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.Cold,
            Mathf.Lerp(0.2f, 0.62f, progress));
        chargeArc.endColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.Cold, 0.03f);
        CombatVfxStyle.SetRing(chargeOrbit, Vector3.zero, Quaternion.identity,
            Mathf.Lerp(0.13f, 0.22f, progress), 48,
            -70f + Time.time * 72f, Mathf.Lerp(80f, 240f, progress));
        chargeOrbit.startColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.ColdCore,
            0.3f + 0.42f * progress);
        chargeOrbit.endColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.Cold, 0.02f);
        for (int i = 0; i < chargeHelices.Length; i++)
        {
            LineRenderer helix = chargeHelices[i];
            CombatVfxStyle.SetHelix(helix, 0.22f, 0.055f + progress * 0.035f,
                2.8f, Time.time * (i == 1 ? -8f : 7f) + i * 2.094f);
            Color tint = CombatVfxStyle.WithAlpha(i == 1 ? CombatVfxStyle.ColdCore : CombatVfxStyle.Cold,
                0.5f + 0.45f * progress);
            helix.startColor = tint;
            helix.endColor = CombatVfxStyle.WithAlpha(tint, 0.18f);
        }
        if (chargeAudio != null)
        {
            chargeAudio.transform.position = palmPosition;
            chargeAudio.volume = chargeVolume * Mathf.Lerp(0.25f, 1f, progress);
            if (playAudio && !chargeAudio.isPlaying) chargeAudio.Play();
            if (!playAudio && chargeAudio.isPlaying) chargeAudio.Stop();
        }
    }

    public void HideCharge()
    {
        if (chargeRoot != null) chargeRoot.gameObject.SetActive(false);
        if (chargeAudio != null && chargeAudio.isPlaying) chargeAudio.Stop();
    }

    public void StartFlightTrail(Transform grenadeTransform)
    {
        EnsureEffects();
        if (grenadeTransform == null) return;
        trailTarget = grenadeTransform;
        MeshRenderer visualRenderer = grenadeTransform.GetComponentInChildren<MeshRenderer>();
        flyingVisual = visualRenderer != null ? visualRenderer.transform.parent : null;
        historyCount = 1;
        history[0] = grenadeTransform.position;
        lastSampleAt = Time.time;
        trail.enabled = true;
        trailOrbit.enabled = true;
    }

    public void StopFlightTrail()
    {
        trailTarget = null;
        flyingVisual = null;
        historyCount = 0;
        if (trail != null) trail.enabled = false;
        if (trailOrbit != null) trailOrbit.enabled = false;
    }

    public void PlayExplosion(Vector3 position)
    {
        PlayExplosion(position, NoPlayerContact, position);
    }

    public void PlayExplosion(Vector3 position, int contactKind, Vector3 contactPoint)
    {
        EnsureEffects();
        HideAll();
        impactRoot.position = position;
        impactRoot.gameObject.SetActive(true);
        impactAt = Time.time;
        if (contactKind != NoPlayerContact)
        {
            hitRoot.position = contactPoint;
            hitRoot.gameObject.SetActive(true);
            hitAt = Time.time;
            hitColor = contactKind == ShieldContact
                ? CombatVfxStyle.Shield : CombatVfxStyle.ColdCore;
        }
        else if (hitRoot != null)
        {
            hitRoot.gameObject.SetActive(false);
            hitAt = -1f;
        }
        for (int i = 0; i < fragments.Length; i++)
        {
            float angle = i * Mathf.PI * 2f / fragments.Length;
            fragmentDirections[i] = new Vector3(Mathf.Cos(angle),
                0.25f + (i % 3) * 0.22f, Mathf.Sin(angle)).normalized;
            fragments[i].localPosition = Vector3.zero;
            fragments[i].localRotation = Quaternion.Euler(i * 31f, i * 67f, 0f);
            fragments[i].gameObject.SetActive(true);
        }
        if (impactAudio != null)
        {
            impactAudio.transform.position = position;
            CombatAudioVoice.Play(impactAudio, impactAudio.clip, explosionVolume, 1.3f);
        }
    }

    public void HideAll()
    {
        // The world-space impact survives routine pose cleanup.
        HideCharge();
        StopFlightTrail();
    }

    public void CancelAllWeaponVisuals()
    {
        HideAll();
        impactAt = -1f;
        if (impactAudio != null) impactAudio.Stop();
        if (impactRoot != null) impactRoot.gameObject.SetActive(false);
        hitAt = -1f;
        if (hitRoot != null) hitRoot.gameObject.SetActive(false);
    }

    private void UpdateTrail()
    {
        if (trailTarget == null) return;
        if (flyingVisual != null)
            flyingVisual.Rotate(0f, 380f * Time.deltaTime, 0f, Space.Self);
        if (Time.time - lastSampleAt >= 0.026f)
        {
            for (int i = Mathf.Min(historyCount, history.Length - 1); i > 0; i--)
                history[i] = history[i - 1];
            history[0] = trailTarget.position;
            historyCount = Mathf.Min(historyCount + 1, history.Length);
            lastSampleAt = Time.time;
        }
        else history[0] = trailTarget.position;

        int count = (historyCount - 1) * 3 + 1;
        trail.positionCount = count;
        trailOrbit.positionCount = count;
        for (int segment = 0; segment < historyCount - 1; segment++)
        {
            Vector3 before = history[Mathf.Max(0, segment - 1)];
            Vector3 start = history[segment];
            Vector3 end = history[segment + 1];
            Vector3 after = history[Mathf.Min(historyCount - 1, segment + 2)];
            for (int step = 0; step < 3; step++)
            {
                int index = segment * 3 + step;
                Vector3 point = CombatVfxStyle.CatmullRom(before, start, end, after,
                    step / 3f);
                Vector3 tangent = (end - start).normalized;
                Vector3 sideways = Vector3.Cross(tangent, Vector3.up);
                if (sideways.sqrMagnitude < 0.01f)
                    sideways = Vector3.Cross(tangent, Vector3.right);
                sideways.Normalize();
                trail.SetPosition(index, point);
                trailOrbit.SetPosition(index, point + sideways
                    * (0.025f * Mathf.Sin(index * 0.9f - Time.time * 13f)));
            }
        }
        trail.SetPosition(count - 1, history[historyCount - 1]);
        trailOrbit.SetPosition(count - 1, history[historyCount - 1]);
    }

    private void UpdateImpact()
    {
        if (impactAt < 0f) return;
        float age = (Time.time - impactAt) / 1.55f;
        if (age >= 1f)
        {
            impactRoot.gameObject.SetActive(false);
            impactAt = -1f;
            if (impactAudio != null) impactAudio.Stop();
            return;
        }
        float eased = 1f - Mathf.Pow(1f - age, 2f);
        radiusField.localScale = Vector3.one * Mathf.Max(0.01f, eased);
        radiusMaterial.color = new Color(1f, 1f, 1f, 1f - age);
        CombatVfxStyle.SetRing(radiusRing, Vector3.up * 0.018f,
            Quaternion.Euler(90f, 0f, 0f), explosionRadius * eased, 48);
        Color tint = CombatVfxStyle.WithAlpha(CombatVfxStyle.Cold, 0.85f * (1f - age));
        radiusRing.startColor = radiusRing.endColor = tint;
        CombatVfxStyle.SetRing(innerRadiusRing, Vector3.up * 0.035f,
            Quaternion.Euler(90f, 0f, 0f),
            explosionRadius * Mathf.Lerp(0.28f, 0.77f, eased), 48);
        innerRadiusRing.startColor = CombatVfxStyle.WithAlpha(
            CombatVfxStyle.ColdCore, 0.68f * (1f - age));
        innerRadiusRing.endColor = innerRadiusRing.startColor;
        CombatVfxStyle.SetRing(impactArc, Vector3.up * 0.1f,
            Quaternion.Euler(90f, 0f, 0f),
            Mathf.Lerp(0.06f, 0.4f, eased), 32, 0f, 280f);
        impactArc.startColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.ColdCore,
            0.4f * (1f - age));
        impactArc.endColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.ColdCore, 0.02f);
        float domeRadius = explosionRadius * Mathf.Lerp(0.2f, 1f, eased);
        CombatVfxStyle.SetRing(domeArcA, Vector3.zero,
            Quaternion.Euler(0f, 0f, 0f), domeRadius, 32, 0f, 180f);
        CombatVfxStyle.SetRing(domeArcB, Vector3.zero,
            Quaternion.Euler(0f, 90f, 0f), domeRadius, 32, 0f, 180f);
        domeArcA.startColor = domeArcB.startColor = CombatVfxStyle.WithAlpha(
            CombatVfxStyle.Cold, 0.45f * (1f - age));
        domeArcA.endColor = domeArcB.endColor = CombatVfxStyle.WithAlpha(
            CombatVfxStyle.Cold, 0.04f);
        for (int i = 0; i < fragments.Length; i++)
        {
            fragments[i].localPosition = fragmentDirections[i] * (0.12f + age * explosionRadius * 0.86f);
            fragments[i].localScale = Vector3.one * (0.04f + (i % 4) * 0.012f)
                * (1f - age);
        }
        for (int i = 0; i < blizzard.Length; i++)
        {
            LineRenderer streak = blizzard[i];
            float seed = i / (float)blizzard.Length;
            float angle = seed * Mathf.PI * 14f + Time.time * (2.4f + i % 4 * 0.27f);
            float orbit = explosionRadius * (0.22f + 0.7f * Mathf.Repeat(seed * 11.13f + age * 0.38f, 1f));
            float height = 0.06f + 1.15f * Mathf.Repeat(seed * 7.27f + age * 1.3f, 1f);
            streak.positionCount = 4;
            for (int point = 0; point < 4; point++)
            {
                float step = point / 3f;
                float a = angle + step * 0.38f;
                streak.SetPosition(point, new Vector3(Mathf.Cos(a) * (orbit + step * 0.12f),
                    height + step * 0.12f, Mathf.Sin(a) * (orbit + step * 0.12f)));
            }
            streak.startColor = CombatVfxStyle.WithAlpha(
                i % 4 == 0 ? CombatVfxStyle.ColdCore : CombatVfxStyle.Cold,
                (0.65f + 0.25f * Mathf.Sin(Time.time * 8f + i)) * (1f - age));
            streak.endColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.Cold, 0f);
            streak.enabled = true;
        }
    }

    private void UpdateHitCue()
    {
        if (hitAt < 0f) return;
        float age = (Time.time - hitAt) / 0.8f;
        if (age >= 1f)
        {
            hitRoot.gameObject.SetActive(false);
            hitAt = -1f;
            return;
        }
        Camera camera = Camera.main;
        if (camera != null)
            hitRoot.rotation = Quaternion.LookRotation(camera.transform.position - hitRoot.position);
        float radius = Mathf.Lerp(0.2f, 0.12f, age);
        CombatVfxStyle.SetRing(hitRing, Vector3.zero, Quaternion.identity,
            radius, 40);
        hitRing.startColor = hitRing.endColor = CombatVfxStyle.WithAlpha(
            hitColor, 0.85f * (1f - age));
        for (int i = 0; i < hitBrackets.Length; i++)
        {
            float angle = i * Mathf.PI * 0.5f + Mathf.PI * 0.25f;
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            Vector3 tangent = new Vector3(-direction.y, direction.x, 0f);
            LineRenderer bracket = hitBrackets[i];
            bracket.positionCount = 3;
            bracket.SetPosition(0, direction * (radius + 0.065f) - tangent * 0.035f);
            bracket.SetPosition(1, direction * (radius + 0.02f));
            bracket.SetPosition(2, direction * (radius + 0.065f) + tangent * 0.035f);
            bracket.startColor = bracket.endColor = CombatVfxStyle.WithAlpha(
                hitColor, 0.95f * (1f - age));
            bracket.enabled = true;
        }
    }

    private void EnsureEffects()
    {
        if (lineMaterial != null) return;
        lineMaterial = CombatVfxStyle.CreateMaterial("Cold lines", Color.white);
        shardMaterial = CombatVfxStyle.CreateMaterial("Cold fragments", CombatVfxStyle.ColdCore);
        shardMesh = CombatVfxStyle.CreateShard();
        chargeRoot = new GameObject("ColdCharge").transform;
        chargeRoot.SetParent(transform, false);
        chargeShard = MakeShard(chargeRoot, "ChargeShard");
        chargeArc = CombatVfxStyle.CreateLine(chargeRoot, "ColdChargeArc",
            lineMaterial, false, 0.006f);
        chargeOrbit = CombatVfxStyle.CreateLine(chargeRoot, "ColdChargeOrbit",
            lineMaterial, false, 0.007f);
        for (int i = 0; i < chargeHelices.Length; i++)
            chargeHelices[i] = CombatVfxStyle.CreateLine(chargeRoot,
                "Cold forearm spiral", lineMaterial, false, i == 1 ? 0.013f : 0.009f);
        chargeRoot.gameObject.SetActive(false);

        grenadeTemplate = new GameObject("ColdGrenadeTemplate").transform;
        grenadeTemplate.SetParent(transform, false);
        MakeShard(grenadeTemplate, "GrenadeBody");
        LineRenderer grenadeHalo = CombatVfxStyle.CreateLine(grenadeTemplate,
            "GrenadeOrbit", lineMaterial, false, 0.04f);
        CombatVfxStyle.SetRing(grenadeHalo, Vector3.zero, Quaternion.identity,
            0.55f, 40, 0f, 300f);
        grenadeHalo.startColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.Cold, 0.64f);
        grenadeHalo.endColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.Cold, 0.03f);
        grenadeTemplate.gameObject.SetActive(false);
        trail = CombatVfxStyle.CreateLine(transform, "ColdFlightSpline",
            lineMaterial, true, 0.06f);
        trail.startColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.ColdCore, 0.9f);
        trail.endColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.Cold, 0f);
        trailOrbit = CombatVfxStyle.CreateLine(transform, "ColdFlightOrbit",
            lineMaterial, true, 0.025f);
        trailOrbit.startColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.Cold, 0.7f);
        trailOrbit.endColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.Cold, 0f);

        impactRoot = new GameObject("ColdImpact").transform;
        radiusField = new GameObject("ColdRadiusField").transform;
        radiusField.SetParent(impactRoot, false);
        radiusField.localPosition = Vector3.up * 0.012f;
        radiusField.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        radiusMesh = CombatVfxStyle.CreateRadialField(explosionRadius,
            CombatVfxStyle.Cold);
        radiusField.gameObject.AddComponent<MeshFilter>().sharedMesh = radiusMesh;
        MeshRenderer radiusRenderer = radiusField.gameObject.AddComponent<MeshRenderer>();
        radiusMaterial = CombatVfxStyle.CreateMaterial("Cold radius field", Color.white);
        radiusRenderer.sharedMaterial = radiusMaterial;
        radiusRenderer.shadowCastingMode = ShadowCastingMode.Off;
        radiusRenderer.receiveShadows = false;
        radiusRing = CombatVfxStyle.CreateLine(impactRoot, "DamageRadius",
            lineMaterial, false, 0.022f);
        innerRadiusRing = CombatVfxStyle.CreateLine(impactRoot, "InnerShock",
            lineMaterial, false, 0.012f);
        impactArc = CombatVfxStyle.CreateLine(impactRoot, "ImpactArc",
            lineMaterial, false, 0.009f);
        domeArcA = CombatVfxStyle.CreateLine(impactRoot, "BlastDomeA",
            lineMaterial, false, 0.009f);
        domeArcB = CombatVfxStyle.CreateLine(impactRoot, "BlastDomeB",
            lineMaterial, false, 0.009f);
        for (int i = 0; i < fragments.Length; i++)
            fragments[i] = MakeShard(impactRoot, "ColdFragment");
        for (int i = 0; i < blizzard.Length; i++)
            blizzard[i] = CombatVfxStyle.CreateLine(impactRoot, "Blizzard spiral",
                lineMaterial, false, i % 4 == 0 ? 0.025f : 0.014f);
        impactRoot.gameObject.SetActive(false);

        hitRoot = new GameObject("ColdPlayerContact").transform;
        hitRing = CombatVfxStyle.CreateLine(hitRoot, "PlayerContactRing",
            lineMaterial, false, 0.012f);
        for (int i = 0; i < hitBrackets.Length; i++)
            hitBrackets[i] = CombatVfxStyle.CreateLine(hitRoot, "ContactBracket",
                lineMaterial, false, 0.012f);
        hitRoot.gameObject.SetActive(false);

        chargeAudio = MakeAudio(chargeAudioResourcePath, true);
        impactAudio = MakeAudio(explosionAudioResourcePath, false);
    }

    private Transform MakeShard(Transform parent, string name)
    {
        GameObject shard = new GameObject(name);
        shard.transform.SetParent(parent, false);
        shard.AddComponent<MeshFilter>().sharedMesh = shardMesh;
        MeshRenderer renderer = shard.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = shardMaterial;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        return shard.transform;
    }

    private AudioSource MakeAudio(string path, bool loop)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip == null) return null;
        return CombatAudioVoice.Create(transform, "Ice audio", clip, loop);
    }

    private void OnDisable() => CancelAllWeaponVisuals();

    private void OnDestroy()
    {
        if (impactRoot != null) Destroy(impactRoot.gameObject);
        if (hitRoot != null) Destroy(hitRoot.gameObject);
        if (lineMaterial != null) Destroy(lineMaterial);
        if (shardMaterial != null) Destroy(shardMaterial);
        if (radiusMaterial != null) Destroy(radiusMaterial);
        if (shardMesh != null) Destroy(shardMesh);
        if (radiusMesh != null) Destroy(radiusMesh);
    }
}
