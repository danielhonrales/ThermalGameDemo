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
    [SerializeField] private float chargeLayerScale = 14f;
    [SerializeField] private float chargeParticleRate = 60f;
    [SerializeField] private string chargeTextureResourcePath = "CustomAssets/circle";
    [SerializeField] private string chargeAudioResourcePath = "CustomAssets/Audio/iceBeam";
    [SerializeField, Range(0f, 1f)] private float chargeVolume = 0.5f;

    [Header("Grenade Body")]
    [SerializeField] private float grenadeVisualScale = 0.14f;
    [SerializeField] private float grenadeLightIntensity = 2.5f;

    [Header("Flight")]
    [SerializeField] private string trailTextureResourcePath = "CustomAssets/spark";
    [SerializeField] private float trailEmissionRate = 42f;

    [Header("Explosion")]
    [SerializeField, Min(0.1f)] private float explosionRadius = 1.5f;
    [SerializeField] private string explosionAudioResourcePath = "CustomAssets/Audio/IceBlast";
    [SerializeField, Range(0f, 1f)] private float explosionVolume = 0.7f;
    [SerializeField] private string impactSparkTextureResourcePath = "CustomAssets/spark";
    [SerializeField] private string impactSmokeTextureResourcePath = "CustomAssets/smoke";

    private Transform chargeRoot;
    private Transform grenadeVisualRoot;
    private ParticleSystem beamChargeParticles;
    private ParticleSystem[] layeredChargeParticles;
    private ParticleSystem fallbackChargeParticles;
    private ParticleSystem flightTrail;
    private ParticleSystem explosionBurst;
    private ParticleSystem explosionSmoke;
    private Light chargeLight;
    private Light explosionLight;
    private AudioSource chargeAudio;
    private AudioSource oneShotAudio;

    private void Awake()
    {
        EnsureEffects();
        HideAll();
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
            main.startColor = WithAlpha(iceColor, 0.65f);
            explosionSmoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            explosionSmoke.Play();
        }

        if (explosionLight != null)
        {
            explosionLight.transform.position = position;
            explosionLight.color = iceColor;
            explosionLight.intensity = 8f;
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
        HideCharge();
        StopFlightTrail();
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
        layeredChargeParticles = ExtractColdChargeParticles(chargeRoot);
        fallbackChargeParticles = CreateFallbackChargeParticles();
        fallbackChargeParticles.transform.SetParent(transform, false);
        grenadeVisualRoot = BuildGrenadeVisualTemplate();
        flightTrail = CreateFlightTrail();
        explosionBurst = CreateExplosionBurst();
        explosionSmoke = CreateExplosionSmoke();

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
        main.startLifetime = 0.55f;
        main.startSpeed = 4.5f;
        main.startSize = 0.35f;
        main.maxParticles = 160;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 72) });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.15f;

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
        main.startLifetime = 0.9f;
        main.startSpeed = 1.6f;
        main.startSize = 0.55f;
        main.maxParticles = 48;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 20) });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.2f;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(impactSmokeTextureResourcePath));
        return particles;
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
