using UnityEngine;

[DisallowMultipleComponent]
public sealed class ForearmShieldEffects : MonoBehaviour
{
    [Header("Shield VFX")]
    [SerializeField] private string ledTubeResourcePath = "CustomAssets/PowerSleeve/ledTube";
    [SerializeField] private string shieldEffectPath = "ColdHit/Magic shield blue";
    [SerializeField] private float shieldLayerScale = 20f;
    [SerializeField] private Color shieldColor = new Color(0.72f, 0.2f, 1f, 0.78f);
    [SerializeField, Range(0f, 1f)] private float shieldParticleAlphaMultiplier = 0.55f;
    [SerializeField, Range(0f, 1f)] private float shieldDiscAlphaMultiplier = 0.9f;
    [SerializeField] private float shieldLightIntensity = 3f;

    [Header("Shield Disc")]
    [SerializeField] private float shieldDiscDiameter = 0.5f;
    [SerializeField] private float shieldDiscThickness = 0.012f;

    private Transform handMount;
    private Transform shieldRoot;
    private Transform shieldDisc;
    private ParticleSystem[] shieldParticles;
    private Light shieldLight;
    private bool isVisible;

    public void ConfigureHandMount(Transform mount)
    {
        handMount = mount;
        if (shieldRoot != null && handMount != null)
        {
            shieldRoot.SetParent(handMount, false);
        }
    }

    public void ShowShieldLocal(Vector3 localPosition, Quaternion localRotation)
    {
        EnsureShield();
        if (shieldRoot == null)
        {
            return;
        }

        isVisible = true;
        shieldRoot.gameObject.SetActive(true);
        shieldRoot.localPosition = localPosition;
        shieldRoot.localRotation = localRotation;

        if (shieldDisc != null)
        {
            shieldDisc.gameObject.SetActive(true);
        }

        TintShield(shieldColor);
        PlayShieldParticles();

        if (shieldLight != null)
        {
            shieldLight.enabled = true;
            shieldLight.color = shieldColor;
            shieldLight.intensity = shieldLightIntensity;
        }
    }

    public void ShowShield(Vector3 worldPosition, Quaternion worldRotation)
    {
        EnsureShield();
        if (shieldRoot == null)
        {
            return;
        }

        isVisible = true;
        shieldRoot.gameObject.SetActive(true);
        shieldRoot.SetPositionAndRotation(worldPosition, worldRotation);

        if (shieldDisc != null)
        {
            shieldDisc.gameObject.SetActive(true);
        }

        TintShield(shieldColor);
        PlayShieldParticles();

        if (shieldLight != null)
        {
            shieldLight.enabled = true;
            shieldLight.color = shieldColor;
            shieldLight.intensity = shieldLightIntensity;
        }
    }

    public void HideShield()
    {
        isVisible = false;

        if (shieldRoot != null)
        {
            shieldRoot.gameObject.SetActive(false);
        }

        if (shieldDisc != null)
        {
            shieldDisc.gameObject.SetActive(false);
        }

        StopShieldParticles();

        if (shieldLight != null)
        {
            shieldLight.enabled = false;
        }
    }

    public bool IsVisible => isVisible;

    private void Awake()
    {
        EnsureShield();
        HideShield();
    }

    private void EnsureShield()
    {
        if (shieldRoot != null)
        {
            return;
        }

        Transform parent = handMount != null ? handMount : transform;
        GameObject rootObject = new GameObject("ForearmShieldRoot");
        rootObject.transform.SetParent(parent, false);
        shieldRoot = rootObject.transform;

        shieldDisc = BuildShieldDisc(shieldRoot);
        shieldParticles = ExtractShieldParticles(shieldRoot);

        GameObject lightObject = new GameObject("ForearmShieldLight");
        lightObject.transform.SetParent(shieldRoot, false);
        shieldLight = lightObject.AddComponent<Light>();
        shieldLight.type = LightType.Point;
        shieldLight.range = 2.2f;
        shieldLight.shadows = LightShadows.None;
        shieldLight.enabled = false;
    }

    private Transform BuildShieldDisc(Transform parent)
    {
        GameObject discObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        discObject.name = "ForearmShieldDisc";
        Destroy(discObject.GetComponent<Collider>());
        discObject.transform.SetParent(parent, false);
        discObject.transform.localPosition = Vector3.zero;
        discObject.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        discObject.transform.localScale = new Vector3(shieldDiscDiameter, shieldDiscThickness, shieldDiscDiameter);

        Renderer renderer = discObject.GetComponent<Renderer>();
        renderer.material = CreateEmissiveMaterial("ForearmShieldDiscMaterial", shieldColor);
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;

        return discObject.transform;
    }

    private ParticleSystem[] ExtractShieldParticles(Transform parent)
    {
        GameObject sourcePrefab = Resources.Load<GameObject>(ledTubeResourcePath);
        if (sourcePrefab == null)
        {
            return CreateFallbackShieldParticles(parent);
        }

        GameObject ledTubeInstance = Instantiate(sourcePrefab, parent);
        System.Collections.Generic.List<ParticleSystem> particles = new System.Collections.Generic.List<ParticleSystem>();
        Transform effects = ledTubeInstance.transform.Find("Effects");
        if (effects != null)
        {
            Transform source = effects.Find(shieldEffectPath);
            if (source == null)
            {
                source = effects.Find("HotHit/Magic shield blue");
            }

            if (source != null)
            {
                source.SetParent(parent, false);
                NormalizeShieldHierarchy(source, shieldLayerScale);
                particles.AddRange(source.GetComponentsInChildren<ParticleSystem>(true));
            }
        }

        foreach (MeshRenderer meshRenderer in ledTubeInstance.GetComponentsInChildren<MeshRenderer>(true))
        {
            meshRenderer.enabled = false;
        }

        Destroy(ledTubeInstance);

        if (particles.Count == 0)
        {
            return CreateFallbackShieldParticles(parent);
        }

        foreach (ParticleSystem particle in particles)
        {
            ParticleSystem.MainModule main = particle.main;
            main.loop = true;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;

            ParticleSystemRenderer renderer = particle.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
            {
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
                Color particleTint = shieldColor;
                particleTint.a *= shieldParticleAlphaMultiplier;
                TintRendererMaterials(renderer, particleTint, 2990);
            }
        }

        return particles.ToArray();
    }

    private static ParticleSystem[] CreateFallbackShieldParticles(Transform parent)
    {
        GameObject particleObject = new GameObject("ForearmShieldFallback");
        particleObject.transform.SetParent(parent, false);
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = 0.45f;
        main.startSpeed = 0.04f;
        main.startSize = 0.35f;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 36f;

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.28f;
        shape.arc = 360f;

        ParticleSystemRenderer renderer = particles.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        Texture2D texture = Resources.Load<Texture2D>("CustomAssets/circle");
        Shader shader = Shader.Find("Sprites/Default");
        Material material = new Material(shader) { mainTexture = texture };
        renderer.material = material;

        return new[] { particles };
    }

    private static void NormalizeShieldHierarchy(Transform root, float layerScale)
    {
        root.localPosition = Vector3.zero;
        root.localRotation = Quaternion.identity;
        root.localScale = Vector3.one * layerScale;
    }

    private void TintShield(Color color)
    {
        if (shieldDisc != null)
        {
            Renderer renderer = shieldDisc.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color discColor = color;
                discColor.a *= shieldDiscAlphaMultiplier;
                ApplyEmissiveColor(renderer.material, discColor);
            }
        }

        if (shieldParticles == null)
        {
            return;
        }

        foreach (ParticleSystem particle in shieldParticles)
        {
            if (particle == null)
            {
                continue;
            }

            ParticleSystem.MainModule main = particle.main;
            Color particleColor = color;
            particleColor.a *= shieldParticleAlphaMultiplier;
            main.startColor = particleColor;

            ParticleSystemRenderer renderer = particle.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
            {
                TintRendererMaterials(renderer, particleColor, 2990);
            }
        }
    }

    private void PlayShieldParticles()
    {
        if (shieldParticles == null)
        {
            return;
        }

        foreach (ParticleSystem particle in shieldParticles)
        {
            if (particle == null)
            {
                continue;
            }

            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particle.Play();
        }
    }

    private void StopShieldParticles()
    {
        if (shieldParticles == null)
        {
            return;
        }

        foreach (ParticleSystem particle in shieldParticles)
        {
            if (particle != null)
            {
                particle.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
        }
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
        Color emission = new Color(color.r, color.g, color.b, 1f) * 1.6f;
        material.SetColor("_EmissionColor", emission);
        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", color);
        }

        if (material.HasProperty("_Surface"))
        {
            material.SetFloat("_Surface", 1f);
            material.SetFloat("_Blend", 0f);
            material.SetFloat("_AlphaClip", 0f);
            material.SetOverrideTag("RenderType", "Transparent");
            material.renderQueue = 3000;
        }

        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 2990;

        return material;
    }

    private static void ApplyEmissiveColor(Material material, Color color)
    {
        if (material == null)
        {
            return;
        }

        material.color = color;
        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", color);
        }

        if (material.HasProperty("_EmissionColor"))
        {
            material.SetColor("_EmissionColor", new Color(color.r, color.g, color.b, 1f) * 1.6f);
        }
    }

    private static void TintRendererMaterials(Renderer renderer, Color tint, int renderQueue)
    {
        if (renderer == null)
        {
            return;
        }

        Material[] materials = renderer.materials;
        foreach (Material material in materials)
        {
            MakeMaterialTransparent(material, tint, renderQueue);
        }
    }

    private static void MakeMaterialTransparent(Material material, Color tint, int renderQueue)
    {
        if (material == null)
        {
            return;
        }

        if (material.HasProperty("_Color"))
        {
            material.SetColor("_Color", tint);
        }

        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", tint);
        }

        if (material.HasProperty("_TintColor"))
        {
            material.SetColor("_TintColor", tint);
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
}
