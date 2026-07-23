using UnityEngine;

[DisallowMultipleComponent]
public sealed class WeaponTransitionEffects : MonoBehaviour
{
    [Header("Distinct Weapon-Swap Palette")]
    [SerializeField] private Color transitionColor = new Color(0.35f, 1f, 0.32f, 0.92f);
    [SerializeField] private Color accentColor = new Color(1f, 0.92f, 0.24f, 0.95f);
    [SerializeField, Min(0.02f)] private float ringRadius = 0.22f;
    [SerializeField, Min(0.001f)] private float ringWidth = 0.014f;
    [SerializeField, Min(8)] private int ringSegments = 40;
    [SerializeField, Min(0.05f)] private float completionSeconds = 0.38f;

    private Transform effectRoot;
    private LineRenderer[] rings;
    private ParticleSystem transitionParticles;
    private ParticleSystem completionBurst;
    private Light transitionLight;
    private Material ringMaterial;
    private float completionUntilTime;
    private bool showingProgress;

    private void Update()
    {
        if (effectRoot == null)
        {
            return;
        }

        if (Time.time < completionUntilTime)
        {
            float remaining = Mathf.Clamp01((completionUntilTime - Time.time) / completionSeconds);
            effectRoot.localScale = Vector3.one * Mathf.Lerp(1.45f, 0.85f, remaining);
            effectRoot.Rotate(Vector3.up, 300f * Time.deltaTime, Space.Self);
            return;
        }

        if (!showingProgress)
        {
            effectRoot.gameObject.SetActive(false);
            if (transitionLight != null)
            {
                transitionLight.enabled = false;
            }
        }
    }

    public void ShowTransition(
        Vector3 worldPosition,
        Quaternion worldRotation,
        float progress,
        CombatWeaponMode.WeaponMode targetMode)
    {
        EnsureEffects();
        showingProgress = true;
        effectRoot.gameObject.SetActive(true);
        effectRoot.SetPositionAndRotation(worldPosition, worldRotation);
        effectRoot.localScale = Vector3.one;

        progress = Mathf.Clamp01(progress);
        float direction = targetMode == CombatWeaponMode.WeaponMode.IceGrenade ? 1f : -1f;
        float pulse = 1f + Mathf.Sin(Time.time * 16f) * 0.08f * progress;
        UpdateRing(rings[0], Quaternion.Euler(90f, 0f, Time.time * 150f * direction), progress, ringRadius * pulse);
        UpdateRing(rings[1], Quaternion.Euler(0f, 90f, Time.time * -120f * direction), progress, ringRadius * 0.78f * pulse);
        UpdateRing(rings[2], Quaternion.Euler(45f, Time.time * 180f * direction, 0f), progress, ringRadius * 0.58f * pulse);

        ParticleSystem.EmissionModule emission = transitionParticles.emission;
        emission.rateOverTime = Mathf.Lerp(10f, 70f, progress);
        if (!transitionParticles.isPlaying)
        {
            transitionParticles.Play();
        }

        transitionLight.enabled = true;
        transitionLight.color = Color.Lerp(transitionColor, accentColor, progress * 0.45f);
        transitionLight.intensity = Mathf.Lerp(0.4f, 2.4f, progress) * pulse;
    }

    public void HideTransition()
    {
        showingProgress = false;
        if (transitionParticles != null)
        {
            transitionParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }

        if (Time.time >= completionUntilTime)
        {
            SetRingsEnabled(false);
        }
    }

    public void PlayCompletion(
        Vector3 worldPosition,
        Quaternion worldRotation,
        CombatWeaponMode.WeaponMode targetMode)
    {
        EnsureEffects();
        showingProgress = false;
        effectRoot.gameObject.SetActive(true);
        effectRoot.SetPositionAndRotation(worldPosition, worldRotation);
        effectRoot.localScale = Vector3.one;
        completionUntilTime = Time.time + completionSeconds;

        Color completionColor = targetMode == CombatWeaponMode.WeaponMode.IceGrenade
            ? transitionColor
            : accentColor;
        ParticleSystem.MainModule main = completionBurst.main;
        main.startColor = completionColor;
        completionBurst.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        completionBurst.Play();

        for (int i = 0; i < rings.Length; i++)
        {
            UpdateRing(rings[i], Quaternion.Euler(i * 60f, i * 45f, 0f), 1f, ringRadius * (1f - i * 0.17f));
        }

        transitionLight.enabled = true;
        transitionLight.color = completionColor;
        transitionLight.intensity = 3.2f;
    }

    private void EnsureEffects()
    {
        if (effectRoot != null)
        {
            return;
        }

        GameObject rootObject = new GameObject("WeaponTransitionVFX");
        rootObject.transform.SetParent(transform, false);
        effectRoot = rootObject.transform;

        ringMaterial = CreateMaterial();
        rings = new LineRenderer[3];
        for (int i = 0; i < rings.Length; i++)
        {
            GameObject ringObject = new GameObject($"TransitionRing_{i + 1}");
            ringObject.transform.SetParent(effectRoot, false);
            LineRenderer ring = ringObject.AddComponent<LineRenderer>();
            ring.useWorldSpace = false;
            ring.loop = false;
            ring.alignment = LineAlignment.TransformZ;
            ring.textureMode = LineTextureMode.Stretch;
            ring.numCapVertices = 4;
            ring.startWidth = ringWidth * (1f - i * 0.15f);
            ring.endWidth = ring.startWidth;
            ring.startColor = i == 1 ? accentColor : transitionColor;
            ring.endColor = i == 1 ? transitionColor : accentColor;
            ring.material = ringMaterial;
            ring.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            ring.receiveShadows = false;
            rings[i] = ring;
        }

        transitionParticles = CreateTransitionParticles();
        transitionParticles.transform.SetParent(effectRoot, false);
        completionBurst = CreateCompletionBurst();
        completionBurst.transform.SetParent(effectRoot, false);

        GameObject lightObject = new GameObject("WeaponTransitionLight");
        lightObject.transform.SetParent(effectRoot, false);
        transitionLight = lightObject.AddComponent<Light>();
        transitionLight.type = LightType.Point;
        transitionLight.range = 1.1f;
        transitionLight.shadows = LightShadows.None;
        transitionLight.enabled = false;

        effectRoot.gameObject.SetActive(false);
    }

    private void UpdateRing(LineRenderer ring, Quaternion rotation, float progress, float radius)
    {
        int visibleSegments = Mathf.Clamp(Mathf.CeilToInt(ringSegments * Mathf.Clamp01(progress)), 2, ringSegments);
        ring.enabled = true;
        ring.transform.localRotation = rotation;
        ring.positionCount = visibleSegments + 1;
        for (int i = 0; i <= visibleSegments; i++)
        {
            float angle = i / (float)ringSegments * Mathf.PI * 2f;
            ring.SetPosition(i, new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f));
        }
    }

    private void SetRingsEnabled(bool visible)
    {
        if (rings == null)
        {
            return;
        }

        foreach (LineRenderer ring in rings)
        {
            if (ring != null)
            {
                ring.enabled = visible;
            }
        }
    }

    private ParticleSystem CreateTransitionParticles()
    {
        GameObject particleObject = new GameObject("TransitionGlyphParticles");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = 0.32f;
        main.startSpeed = -0.18f;
        main.startSize = 0.025f;
        main.startColor = transitionColor;
        main.maxParticles = 64;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 24f;
        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = ringRadius;
        shape.radiusThickness = 0.15f;

        ConfigureParticleRenderer(particles);
        return particles;
    }

    private ParticleSystem CreateCompletionBurst()
    {
        GameObject particleObject = new GameObject("TransitionCompletionBurst");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.loop = false;
        main.startLifetime = 0.32f;
        main.startSpeed = 0.8f;
        main.startSize = 0.035f;
        main.maxParticles = 48;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 28) });
        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.04f;

        ConfigureParticleRenderer(particles);
        return particles;
    }

    private void ConfigureParticleRenderer(ParticleSystem particles)
    {
        ParticleSystemRenderer renderer = particles.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material = CreateMaterial();
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
    }

    private Material CreateMaterial()
    {
        Shader shader = Shader.Find("Sprites/Default");
        Material material = new Material(shader)
        {
            mainTexture = Resources.Load<Texture2D>("CustomAssets/spark")
        };
        return material;
    }

    private void OnDestroy()
    {
        if (ringMaterial != null)
        {
            Destroy(ringMaterial);
        }
    }
}
