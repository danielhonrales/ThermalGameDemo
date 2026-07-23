using UnityEngine;

[DisallowMultipleComponent]
public sealed class IceGrenadeEffects : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color iceColor = new Color(0.25f, 0.82f, 1f, 1f);
    [SerializeField] private Color sparkleColor = new Color(0.58f, 0.8f, 1f, 1f);

    [Header("Charge")]
    [SerializeField] private string beamChargeResourcePath = "CustomAssets/Beam/Beam";
    [SerializeField] private string ledTubeResourcePath = "CustomAssets/PowerSleeve/ledTube";
    [SerializeField] private Vector3 chargeLocalOffset = new Vector3(0f, 0f, 0.03f);
    [SerializeField] private float chargeStartScale = 0.4f;
    [SerializeField] private float chargeEndScale = 1.2f;
    [SerializeField] private float chargePulseSpeed = 8f;
    [SerializeField] private float chargePulseAmount = 0.16f;
    [SerializeField] private float chargeLightIntensity = 4f;
    [Tooltip("Large imported Magic Shield/Absorb aura layers. Disabled to keep the charge local to the hand.")]
    [SerializeField] private bool showLargeChargeAuraLayers;
    [SerializeField] private float chargeLayerScale = 14f;
    [SerializeField] private float chargeParticleRate = 60f;
    [SerializeField] private string chargeTextureResourcePath = "CustomAssets/circle";
    [Tooltip("Compact smoke and sparkle layers around the throwing hand; separate from the disabled large arena aura.")]
    [SerializeField] private bool showHandChargeAtmosphere = true;
    [SerializeField, Min(0f)] private float handChargeSmokeRate = 38f;
    [SerializeField, Min(0f)] private float handChargeSparkRate = 74f;
    [SerializeField] private string chargeAudioResourcePath = "CustomAssets/Audio/iceBeam";
    [SerializeField, Range(0f, 1f)] private float chargeVolume = 0.5f;

    [Header("Grenade Body")]
    [SerializeField] private float grenadeVisualScale = 0.14f;
    [SerializeField] private float grenadeLightIntensity = 2.5f;

    [Header("Flight")]
    [SerializeField] private string trailTextureResourcePath = "CustomAssets/spark";
    [SerializeField] private float trailEmissionRate = 42f;

    [Header("Explosion")]
    [SerializeField, Min(0.1f)] private float explosionRadius = 0.8f;
    [SerializeField] private string explosionAudioResourcePath = "CustomAssets/Audio/IceBlast";
    [SerializeField, Range(0f, 1f)] private float explosionVolume = 0.7f;
    [SerializeField] private string impactSparkTextureResourcePath = "CustomAssets/spark";
    [SerializeField] private string impactSmokeTextureResourcePath = "CustomAssets/smoke";
    [SerializeField] private string explosionRangePrefabResourcePath = "CustomAssets/MagicShieldBlue";
    [SerializeField, Range(0f, 1f)] private float explosionRangeAlpha = 0.9f;
    [SerializeField, Min(0.1f)] private float explosionRangeVisibleSeconds = 1.5f;
    [SerializeField, Min(0.1f)] private float explosionRangePrefabDiameterMeters = 2f;
    [SerializeField] private float explosionRangeVerticalOffset = 0.03f;
    [SerializeField, Min(8)] private int explosionRangeRingSegments = 56;
    [SerializeField, Min(0.001f)] private float explosionRangeRingWidth = 0.065f;

    private Transform chargeRoot;
    private Transform grenadeVisualRoot;
    private ParticleSystem beamChargeParticles;
    private ParticleSystem[] layeredChargeParticles;
    private ParticleSystem fallbackChargeParticles;
    private ParticleSystem handChargeSmokeParticles;
    private ParticleSystem handChargeSparkParticles;
    private ParticleSystem flightTrail;
    private ParticleSystem explosionBurst;
    private ParticleSystem explosionSmoke;
    private GameObject explosionRangeVisual;
    private ParticleSystem[] explosionRangeParticles;
    private LineRenderer explosionRangeRing;
    private Light chargeLight;
    private Light explosionLight;
    private AudioSource chargeAudio;
    private AudioSource oneShotAudio;
    private float hideExplosionRangeAtTime;

    private void Awake()
    {
        EnsureEffects();
        HideAll();
        HideExplosionRange();
    }

    private void Update()
    {
        if (explosionRangeVisual != null
            && explosionRangeVisual.activeSelf
            && Time.time >= hideExplosionRangeAtTime)
        {
            HideExplosionRange();
        }
    }

    private void OnDestroy()
    {
        if (explosionRangeVisual != null)
        {
            Destroy(explosionRangeVisual);
        }
    }

    public Transform CreateGrenadeVisual(Transform parent)
    {
        EnsureEffects();
        if (grenadeVisualRoot == null)
        {
            return null;
        }

        Transform visual = Instantiate(grenadeVisualRoot, parent);
        visual.localScale = Vector3.one * grenadeVisualScale;
        visual.gameObject.SetActive(true);

        Light flightLight = visual.GetComponentInChildren<Light>(true);
        if (flightLight != null)
        {
            flightLight.enabled = true;
            flightLight.color = iceColor;
            flightLight.intensity = grenadeLightIntensity;
        }

        return visual;
    }

    public void ShowCharge(Vector3 palmPosition, Quaternion palmRotation, float progress)
    {
        EnsureEffects();
        progress = Mathf.Clamp01(progress);

        float pulse = 1f + Mathf.Sin(Time.time * chargePulseSpeed) * chargePulseAmount * progress;
        float scale = Mathf.Lerp(chargeStartScale, chargeEndScale, progress) * pulse;
        Vector3 chargePosition = palmPosition + palmRotation * chargeLocalOffset;

        if (chargeRoot != null)
        {
            chargeRoot.gameObject.SetActive(true);
            chargeRoot.SetPositionAndRotation(chargePosition, palmRotation);
            chargeRoot.localScale = Vector3.one * scale;
        }

        Color tint = WithAlpha(iceColor, Mathf.Lerp(0.4f, 1f, progress));
        SetParticleColor(beamChargeParticles, tint);
        PlayParticle(beamChargeParticles);
        SetLayeredChargeIntensity(tint, progress);
        PlayParticles(layeredChargeParticles);

        if (chargeLight != null)
        {
            chargeLight.enabled = true;
            chargeLight.color = iceColor;
            chargeLight.intensity = chargeLightIntensity * progress * pulse;
        }

        if (fallbackChargeParticles != null)
        {
            fallbackChargeParticles.transform.position = chargePosition;
            ParticleSystem.MainModule main = fallbackChargeParticles.main;
            main.startColor = tint;
            main.startSize = Mathf.Lerp(0.045f, 0.12f, progress);

            ParticleSystem.EmissionModule emission = fallbackChargeParticles.emission;
            emission.rateOverTime = chargeParticleRate * Mathf.Lerp(0.2f, 1f, progress);

            if (!fallbackChargeParticles.isPlaying)
            {
                fallbackChargeParticles.Play();
            }
        }

        UpdateHandChargeAtmosphere(chargePosition, tint, progress);

        if (chargeAudio != null)
        {
            chargeAudio.volume = chargeVolume * Mathf.Lerp(0.3f, 1f, progress);
            if (!chargeAudio.isPlaying)
            {
                chargeAudio.Play();
            }
        }
    }

    public void HideCharge()
    {
        if (chargeRoot != null)
        {
            chargeRoot.gameObject.SetActive(false);
        }

        StopParticle(beamChargeParticles);
        StopParticles(layeredChargeParticles);
        StopParticle(fallbackChargeParticles);
        StopParticle(handChargeSmokeParticles);
        StopParticle(handChargeSparkParticles);

        if (chargeLight != null)
        {
            chargeLight.enabled = false;
        }

        if (chargeAudio != null && chargeAudio.isPlaying)
        {
            chargeAudio.Stop();
        }
    }

    public void StartFlightTrail(Transform grenadeTransform)
    {
        EnsureEffects();
        if (flightTrail == null || grenadeTransform == null)
        {
            return;
        }

        flightTrail.transform.SetParent(grenadeTransform, false);
        flightTrail.transform.localPosition = Vector3.zero;
        flightTrail.gameObject.SetActive(true);

        ParticleSystem.MainModule main = flightTrail.main;
        main.startColor = WithAlpha(sparkleColor, 0.85f);

        if (!flightTrail.isPlaying)
        {
            flightTrail.Play();
        }
    }

    public void StopFlightTrail()
    {
        if (flightTrail == null)
        {
            return;
        }

        flightTrail.transform.SetParent(transform, false);
        flightTrail.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        flightTrail.gameObject.SetActive(false);
    }

    public void PlayExplosion(Vector3 position)
    {
        EnsureEffects();
        HideCharge();
        StopFlightTrail();

        if (explosionBurst != null)
        {
            explosionBurst.transform.position = position;
            ParticleSystem.MainModule main = explosionBurst.main;
            main.startColor = sparkleColor;
            explosionBurst.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            explosionBurst.Play();
        }

        if (explosionSmoke != null)
        {
            explosionSmoke.transform.position = position;
            ParticleSystem.MainModule main = explosionSmoke.main;
            main.startColor = WithAlpha(iceColor, 0.9f);
            explosionSmoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            explosionSmoke.Play();
        }

        if (explosionLight != null)
        {
            explosionLight.transform.position = position;
            explosionLight.color = iceColor;
            explosionLight.intensity = 11f;
            explosionLight.enabled = true;
            CancelInvoke(nameof(DisableExplosionLight));
            Invoke(nameof(DisableExplosionLight), 0.35f);
        }

        if (oneShotAudio != null)
        {
            AudioClip clip = Resources.Load<AudioClip>(explosionAudioResourcePath);
            if (clip != null)
            {
                oneShotAudio.PlayOneShot(clip, explosionVolume);
            }
        }

        ShowExplosionRange(position);
    }

    private void DisableExplosionLight()
    {
        if (explosionLight != null)
        {
            explosionLight.enabled = false;
        }
    }

    public float ExplosionRadius => explosionRadius;

    public void HideAll()
    {
        // This is called whenever the throwing pose ends. Explosion range feedback is
        // world-space and must survive that routine charge/flight cleanup.
        HideCharge();
        StopFlightTrail();
    }

    /// <summary>Used for an immediate weapon-mode change, including active grenade VFX.</summary>
    public void CancelAllWeaponVisuals()
    {
        HideAll();
        StopParticle(explosionBurst);
        StopParticle(explosionSmoke);
        HideExplosionRange();
    }

    private void EnsureEffects()
    {
        if (chargeRoot != null)
        {
            return;
        }

        GameObject rootObject = new GameObject("IceChargeRoot");
        rootObject.transform.SetParent(transform, false);
        chargeRoot = rootObject.transform;

        beamChargeParticles = ExtractBeamChargeParticles(chargeRoot);
        layeredChargeParticles = showLargeChargeAuraLayers
            ? ExtractColdChargeParticles(chargeRoot)
            : System.Array.Empty<ParticleSystem>();
        fallbackChargeParticles = CreateFallbackChargeParticles();
        fallbackChargeParticles.transform.SetParent(transform, false);
        if (showHandChargeAtmosphere)
        {
            handChargeSmokeParticles = CreateHandChargeSmokeParticles();
            handChargeSmokeParticles.transform.SetParent(transform, false);
            handChargeSparkParticles = CreateHandChargeSparkParticles();
            handChargeSparkParticles.transform.SetParent(transform, false);
        }
        grenadeVisualRoot = BuildGrenadeVisualTemplate();
        flightTrail = CreateFlightTrail();
        explosionBurst = CreateExplosionBurst();
        explosionSmoke = CreateExplosionSmoke();
        explosionRangeVisual = CreateExplosionRangeVisual();
        explosionRangeRing = CreateExplosionRangeRing();

        GameObject lightObject = new GameObject("IceChargeLight");
        lightObject.transform.SetParent(chargeRoot, false);
        chargeLight = lightObject.AddComponent<Light>();
        chargeLight.type = LightType.Point;
        chargeLight.range = 1.1f;
        chargeLight.shadows = LightShadows.None;
        chargeLight.enabled = false;

        GameObject explosionLightObject = new GameObject("IceExplosionLight");
        explosionLightObject.transform.SetParent(transform, false);
        explosionLight = explosionLightObject.AddComponent<Light>();
        explosionLight.type = LightType.Point;
        explosionLight.range = 4f;
        explosionLight.shadows = LightShadows.None;
        explosionLight.enabled = false;

        chargeAudio = gameObject.AddComponent<AudioSource>();
        chargeAudio.clip = Resources.Load<AudioClip>(chargeAudioResourcePath);
        chargeAudio.loop = true;
        chargeAudio.playOnAwake = false;
        chargeAudio.spatialBlend = 1f;
        chargeAudio.volume = chargeVolume;
        chargeAudio.minDistance = 0.15f;
        chargeAudio.maxDistance = 6f;

        oneShotAudio = gameObject.AddComponent<AudioSource>();
        oneShotAudio.playOnAwake = false;
        oneShotAudio.spatialBlend = 1f;
        oneShotAudio.minDistance = 0.2f;
        oneShotAudio.maxDistance = 12f;

        HideAll();
        HideExplosionRange();
    }

    private Transform BuildGrenadeVisualTemplate()
    {
        GameObject grenadeObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        grenadeObject.name = "IceGrenadeBody";
        Destroy(grenadeObject.GetComponent<Collider>());

        Renderer renderer = grenadeObject.GetComponent<Renderer>();
        Material bodyMaterial = CreateEmissiveMaterial("IceGrenadeBodyMaterial", iceColor);
        renderer.material = bodyMaterial;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        GameObject lightObject = new GameObject("IceGrenadeFlightLight");
        lightObject.transform.SetParent(grenadeObject.transform, false);
        Light flightLight = lightObject.AddComponent<Light>();
        flightLight.type = LightType.Point;
        flightLight.color = iceColor;
        flightLight.intensity = grenadeLightIntensity;
        flightLight.range = 1.4f;
        flightLight.shadows = LightShadows.None;
        flightLight.enabled = false;

        grenadeObject.transform.SetParent(transform, false);
        grenadeObject.SetActive(false);
        return grenadeObject.transform;
    }

    private ParticleSystem ExtractBeamChargeParticles(Transform parent)
    {
        GameObject sourcePrefab = Resources.Load<GameObject>(beamChargeResourcePath);
        if (sourcePrefab == null)
        {
            return null;
        }

        GameObject beamInstance = Instantiate(sourcePrefab, parent);
        Transform chargeTransform = beamInstance.transform.Find("Charge");
        ParticleSystem particles = chargeTransform != null ? chargeTransform.GetComponent<ParticleSystem>() : null;
        if (chargeTransform != null)
        {
            chargeTransform.SetParent(parent, false);
            chargeTransform.localPosition = Vector3.zero;
            chargeTransform.localRotation = Quaternion.identity;
            chargeTransform.localScale = Vector3.one;
        }

        Destroy(beamInstance);
        return particles;
    }

    private ParticleSystem[] ExtractColdChargeParticles(Transform parent)
    {
        GameObject sourcePrefab = Resources.Load<GameObject>(ledTubeResourcePath);
        if (sourcePrefab == null)
        {
            return System.Array.Empty<ParticleSystem>();
        }

        GameObject ledTubeInstance = Instantiate(sourcePrefab, parent);
        System.Collections.Generic.List<ParticleSystem> particles = new System.Collections.Generic.List<ParticleSystem>();
        Transform effects = ledTubeInstance.transform.Find("Effects");
        if (effects != null)
        {
            ReparentChargeLayer(effects, "ColdHit/Magic shield blue", parent, particles, chargeLayerScale);
            ReparentChargeLayer(effects, "Absorb", parent, particles, chargeLayerScale * 0.7f);
        }

        foreach (MeshRenderer meshRenderer in ledTubeInstance.GetComponentsInChildren<MeshRenderer>(true))
        {
            meshRenderer.enabled = false;
        }

        Destroy(ledTubeInstance);
        return particles.ToArray();
    }

    private static void ReparentChargeLayer(Transform effectsRoot, string relativePath, Transform parent, System.Collections.Generic.List<ParticleSystem> particles, float layerScale)
    {
        Transform source = effectsRoot.Find(relativePath);
        if (source == null)
        {
            return;
        }

        source.SetParent(parent, false);
        NormalizeChargeHierarchy(source, layerScale);
        particles.AddRange(source.GetComponentsInChildren<ParticleSystem>(true));
    }

    private static void NormalizeChargeHierarchy(Transform root, float layerScale)
    {
        root.localPosition = Vector3.zero;
        root.localRotation = Quaternion.identity;
        root.localScale = Vector3.one * layerScale;

        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        {
            if (child == root)
            {
                continue;
            }

            child.localPosition = Vector3.zero;
        }
    }

    private ParticleSystem CreateFallbackChargeParticles()
    {
        GameObject particleObject = new GameObject("IceFallbackChargeParticles");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = 0.3f;
        main.startSpeed = -0.4f;
        main.startSize = 0.065f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = chargeParticleRate;

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.075f;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(chargeTextureResourcePath));
        return particles;
    }

    private void UpdateHandChargeAtmosphere(Vector3 position, Color tint, float progress)
    {
        progress = Mathf.Clamp01(progress);
        if (handChargeSmokeParticles != null)
        {
            handChargeSmokeParticles.transform.position = position;
            ParticleSystem.MainModule main = handChargeSmokeParticles.main;
            main.startColor = WithAlpha(tint, Mathf.Lerp(0.26f, 0.72f, progress));
            main.startSize = Mathf.Lerp(0.11f, 0.23f, progress);
            ParticleSystem.EmissionModule emission = handChargeSmokeParticles.emission;
            emission.rateOverTime = handChargeSmokeRate * Mathf.Lerp(0.35f, 1f, progress);
            PlayParticle(handChargeSmokeParticles);
        }

        if (handChargeSparkParticles != null)
        {
            handChargeSparkParticles.transform.position = position;
            ParticleSystem.MainModule main = handChargeSparkParticles.main;
            main.startColor = WithAlpha(sparkleColor, Mathf.Lerp(0.5f, 1f, progress));
            main.startSize = Mathf.Lerp(0.05f, 0.1f, progress);
            ParticleSystem.EmissionModule emission = handChargeSparkParticles.emission;
            emission.rateOverTime = handChargeSparkRate * Mathf.Lerp(0.3f, 1f, progress);
            PlayParticle(handChargeSparkParticles);
        }
    }

    private ParticleSystem CreateHandChargeSmokeParticles()
    {
        GameObject particleObject = new GameObject("IceHandChargeSmoke");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.42f, 0.74f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.03f, 0.15f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.11f, 0.23f);
        main.maxParticles = 180;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = handChargeSmokeRate;
        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.08f;
        ParticleSystem.NoiseModule noise = particles.noise;
        noise.enabled = true;
        noise.strength = 0.1f;
        noise.frequency = 1.25f;
        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(impactSmokeTextureResourcePath));
        return particles;
    }

    private ParticleSystem CreateHandChargeSparkParticles()
    {
        GameObject particleObject = new GameObject("IceHandChargeSparks");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.18f, 0.34f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.3f, 0.8f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.05f, 0.1f);
        main.maxParticles = 220;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = handChargeSparkRate;
        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.06f;
        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(impactSparkTextureResourcePath));
        return particles;
    }

    private ParticleSystem CreateFlightTrail()
    {
        GameObject particleObject = new GameObject("IceFlightTrail");
        particleObject.transform.SetParent(transform, false);
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = 0.28f;
        main.startSpeed = 0.12f;
        main.startSize = 0.12f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = trailEmissionRate;

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.05f;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(trailTextureResourcePath));
        particleObject.SetActive(false);
        return particles;
    }

    private ParticleSystem CreateExplosionBurst()
    {
        GameObject particleObject = new GameObject("IceExplosionBurst");
        particleObject.transform.SetParent(transform, false);
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = false;
        main.startLifetime = 0.36f;
        main.startSpeed = Mathf.Max(1.15f, explosionRadius * 1.7f);
        main.startSize = 0.22f;
        main.maxParticles = 128;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 72) });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.07f;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(impactSparkTextureResourcePath));
        return particles;
    }

    private ParticleSystem CreateExplosionSmoke()
    {
        GameObject particleObject = new GameObject("IceExplosionSmoke");
        particleObject.transform.SetParent(transform, false);
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = false;
        main.startLifetime = 0.55f;
        main.startSpeed = Mathf.Max(0.35f, explosionRadius * 0.65f);
        main.startSize = 0.28f;
        main.maxParticles = 32;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 12) });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.09f;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(impactSmokeTextureResourcePath));
        return particles;
    }

    private GameObject CreateExplosionRangeVisual()
    {
        GameObject sourcePrefab = Resources.Load<GameObject>(explosionRangePrefabResourcePath);
        if (sourcePrefab == null)
        {
            Debug.LogWarning($"Explosion range prefab was not found in Resources at '{explosionRangePrefabResourcePath}'.", this);
            return null;
        }

        GameObject rangeVisual = Instantiate(sourcePrefab);
        rangeVisual.name = "IceExplosionRangeShield";
        rangeVisual.SetActive(false);

        foreach (Collider rangeCollider in rangeVisual.GetComponentsInChildren<Collider>(true))
        {
            rangeCollider.enabled = false;
        }

        explosionRangeParticles = rangeVisual.GetComponentsInChildren<ParticleSystem>(true);
        foreach (ParticleSystem particle in explosionRangeParticles)
        {
            ParticleSystem.MainModule main = particle.main;
            main.loop = true;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            SetParticleColor(particle, WithAlpha(iceColor, explosionRangeAlpha));

            ParticleSystemRenderer renderer = particle.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
            {
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
                MakeRendererMaterialsTransparent(renderer, explosionRangeAlpha, 2990);
            }
        }

        foreach (Renderer renderer in rangeVisual.GetComponentsInChildren<Renderer>(true))
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            if (renderer is ParticleSystemRenderer)
            {
                MakeRendererMaterialsTransparent(renderer, explosionRangeAlpha, 2990);
            }
            else
            {
                MakeRendererMaterialsOpaque(renderer);
            }
        }

        return rangeVisual;
    }

    private LineRenderer CreateExplosionRangeRing()
    {
        GameObject ringObject = new GameObject("IceExplosionRangeRing");
        ringObject.transform.SetParent(transform, false);

        LineRenderer ring = ringObject.AddComponent<LineRenderer>();
        ring.useWorldSpace = true;
        ring.loop = true;
        ring.positionCount = explosionRangeRingSegments;
        ring.startWidth = explosionRangeRingWidth;
        ring.endWidth = explosionRangeRingWidth;
        ring.numCapVertices = 4;
        ring.numCornerVertices = 2;
        ring.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        ring.receiveShadows = false;
        ring.material = CreateEmissiveMaterial("IceExplosionRangeRingMaterial", iceColor);
        ring.enabled = false;
        return ring;
    }

    private void ShowExplosionRange(Vector3 position)
    {
        Vector3 rangePosition = position + Vector3.up * explosionRangeVerticalOffset;
        float diameter = explosionRadius * 2f;

        if (explosionRangeVisual != null)
        {
            explosionRangeVisual.transform.position = rangePosition;
            explosionRangeVisual.transform.rotation = Quaternion.identity;
            explosionRangeVisual.transform.localScale = Vector3.one * (diameter / Mathf.Max(0.01f, explosionRangePrefabDiameterMeters));
            explosionRangeVisual.SetActive(true);

            if (explosionRangeParticles != null)
            {
                foreach (ParticleSystem particle in explosionRangeParticles)
                {
                    if (particle == null)
                    {
                        continue;
                    }

                    SetParticleColor(particle, WithAlpha(iceColor, explosionRangeAlpha));
                    particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    particle.Play();
                }
            }
        }

        if (explosionRangeRing != null)
        {
            explosionRangeRing.enabled = true;
            explosionRangeRing.startColor = iceColor;
            explosionRangeRing.endColor = sparkleColor;
            explosionRangeRing.startWidth = explosionRangeRingWidth;
            explosionRangeRing.endWidth = explosionRangeRingWidth;
            explosionRangeRing.positionCount = Mathf.Max(8, explosionRangeRingSegments);

            for (int i = 0; i < explosionRangeRing.positionCount; i++)
            {
                float angle = i / (float)explosionRangeRing.positionCount * Mathf.PI * 2f;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * explosionRadius;
                explosionRangeRing.SetPosition(i, rangePosition + offset);
            }
        }

        hideExplosionRangeAtTime = Time.time + explosionRangeVisibleSeconds;
    }

    private void HideExplosionRange()
    {
        if (explosionRangeVisual != null)
        {
            explosionRangeVisual.SetActive(false);
        }

        if (explosionRangeRing != null)
        {
            explosionRangeRing.enabled = false;
        }

        StopParticles(explosionRangeParticles);
    }

    private void ConfigureParticleRenderer(ParticleSystem particles, Texture2D texture)
    {
        ParticleSystemRenderer renderer = particles.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material = CreateTintedMaterial($"{particles.name}Material", Color.white, texture);
    }

    private static Material CreateEmissiveMaterial(string materialName, Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            shader = Shader.Find("Standard");
        }

        Material material = new Material(shader)
        {
            name = materialName,
            color = color
        };

        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color * 2.8f);
        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", color);
        }

        return material;
    }

    private static Material CreateTintedMaterial(string materialName, Color color, Texture2D texture = null)
    {
        Shader shader = Shader.Find("Sprites/Default");
        if (shader == null)
        {
            shader = Shader.Find("Particles/Standard Unlit");
        }

        Material material = new Material(shader)
        {
            name = materialName,
            color = color,
            mainTexture = texture
        };

        return material;
    }

    private static void MakeRendererMaterialsTransparent(Renderer renderer, float alpha, int renderQueue)
    {
        if (renderer == null)
        {
            return;
        }

        Material[] materials = renderer.materials;
        foreach (Material material in materials)
        {
            MakeMaterialTransparent(material, alpha, renderQueue);
        }
    }

    private static void MakeRendererMaterialsOpaque(Renderer renderer)
    {
        if (renderer == null)
        {
            return;
        }

        foreach (Material material in renderer.materials)
        {
            if (material == null)
            {
                continue;
            }

            if (material.HasProperty("_Color"))
            {
                Color color = material.GetColor("_Color");
                color.a = 1f;
                material.SetColor("_Color", color);
            }

            if (material.HasProperty("_BaseColor"))
            {
                Color color = material.GetColor("_BaseColor");
                color.a = 1f;
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Surface"))
            {
                material.SetFloat("_Surface", 0f);
            }

            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        }
    }

    private static void MakeMaterialTransparent(Material material, float alpha, int renderQueue)
    {
        if (material == null)
        {
            return;
        }

        if (material.HasProperty("_Color"))
        {
            Color color = material.GetColor("_Color");
            color.a = alpha;
            material.SetColor("_Color", color);
        }

        if (material.HasProperty("_BaseColor"))
        {
            Color color = material.GetColor("_BaseColor");
            color.a = alpha;
            material.SetColor("_BaseColor", color);
        }

        if (material.HasProperty("_TintColor"))
        {
            Color color = material.GetColor("_TintColor");
            color.a = alpha;
            material.SetColor("_TintColor", color);
        }

        if (material.HasProperty("_Surface"))
        {
            material.SetFloat("_Surface", 1f);
            material.SetFloat("_Blend", 0f);
            material.SetFloat("_AlphaClip", 0f);
        }

        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.SetOverrideTag("RenderType", "Transparent");
        material.renderQueue = renderQueue;
    }

    private void SetLayeredChargeIntensity(Color tint, float progress)
    {
        if (layeredChargeParticles == null)
        {
            return;
        }

        foreach (ParticleSystem particle in layeredChargeParticles)
        {
            SetParticleColor(particle, WithAlpha(tint, Mathf.Lerp(0.35f, 0.95f, progress)));
        }
    }

    private static void SetParticleColor(ParticleSystem particles, Color color)
    {
        if (particles == null)
        {
            return;
        }

        ParticleSystem.MainModule main = particles.main;
        main.startColor = color;
    }

    private static void PlayParticle(ParticleSystem particle)
    {
        if (particle != null && !particle.isPlaying)
        {
            particle.Play();
        }
    }

    private static void PlayParticles(ParticleSystem[] particles)
    {
        if (particles == null)
        {
            return;
        }

        foreach (ParticleSystem particle in particles)
        {
            PlayParticle(particle);
        }
    }

    private static void StopParticle(ParticleSystem particle)
    {
        if (particle != null)
        {
            particle.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private static void StopParticles(ParticleSystem[] particles)
    {
        if (particles == null)
        {
            return;
        }

        foreach (ParticleSystem particle in particles)
        {
            StopParticle(particle);
        }
    }

    private static Color WithAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}
