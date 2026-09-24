using UnityEngine;

/// <summary>
/// Hand-picked particle prefabs and materials, loaded from Resources so runtime-built
/// effects keep their references in Quest builds.
/// </summary>
[CreateAssetMenu(menuName = "Thermal/FX Library")]
public sealed class ThermalFxLibrary : ScriptableObject
{
    [Header("Impacts")]
    public GameObject hitExplosion;
    public GameObject hitSparks;
    public GameObject shieldSparks;
    public GameObject coldSparks;
    public GameObject healSparks;
    public GameObject electroHit;
    public GameObject muzzleFlash;
    [Header("Drones and cover")]
    public GameObject droneExplosion;
    public GameObject dustPuff;
    public GameObject smokeTrail;
    public GameObject metalSparks;
    public GameObject bigExplosion;
    [Header("Sudden death atmosphere")]
    public GameObject fireLarge;
    public GameObject fireMedium;
    public GameObject groundSmoke;
    public GameObject ceilingSparks;
    public Mesh floorTileMesh;
    public Material hellFloor;
    public Mesh droneHullMesh;
    public Material droneHull;
    [Header("Materials")]
    public Material droneBody;
    public Material droneShell;
    public Material droneEmissive;
    public Material barrier;
    public Material coverEmissive;
    [Header("Fonts")]
    public TMPro.TMP_FontAsset displayFont;
    public TMPro.TMP_FontAsset heavyFont;
    public TMPro.TMP_FontAsset bodyFont;

    private static ThermalFxLibrary instance;
    private static bool loaded;

    public static ThermalFxLibrary Instance
    {
        get
        {
            if (!loaded)
            {
                loaded = true;
                instance = Resources.Load<ThermalFxLibrary>("ThermalFX/ThermalFxLibrary");
                if (instance == null) Debug.LogWarning("ThermalFxLibrary missing from Resources/ThermalFX.");
            }
            return instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics() { instance = null; loaded = false; }

    /// <summary>Spawns a one-shot particle prefab and destroys it once finished.</summary>
    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation,
        float scale = 1f, float lifetime = 4f)
    {
        if (prefab == null) return null;
        // Many particle prefabs rely on their authored root rotation (e.g. -90° X to emit upward).
        GameObject fx = Object.Instantiate(prefab, position, rotation * prefab.transform.localRotation);
        fx.transform.localScale = prefab.transform.localScale * scale;
        FixParticleMaterials(fx);
        foreach (ParticleSystem system in fx.GetComponentsInChildren<ParticleSystem>())
        {
            ParticleSystem.MainModule main = system.main;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            main.loop = false;
        }
        Object.Destroy(fx, lifetime);
        return fx;
    }

    private static readonly System.Collections.Generic.Dictionary<Material, Material> unlitCache =
        new System.Collections.Generic.Dictionary<Material, Material>();

    /// <summary>
    /// Lit particle materials render as opaque shaded blobs in this passthrough setup; swap them
    /// for cached unlit clones that keep the texture, colour and blend settings.
    /// </summary>
    public static void FixParticleMaterials(GameObject root)
    {
        Shader unlit = Shader.Find("Universal Render Pipeline/Particles/Unlit");
        if (unlit == null) return;
        foreach (ParticleSystemRenderer renderer in root.GetComponentsInChildren<ParticleSystemRenderer>(true))
        {
            Material source = renderer.sharedMaterial;
            if (source == null || source.shader == null) continue;
            string name = source.shader.name;
            bool lit = name.Contains("Lit") && !name.Contains("Unlit");
            // Soft particles and camera fading need a depth texture that Quest doesn't render: they vanish.
            bool needsDepth = source.IsKeywordEnabled("_SOFTPARTICLES_ON") || source.IsKeywordEnabled("_FADING_ON");
            if (lit)
            {
                // Lit puffs (smoke, dust, decals) render as opaque domes without depth; drop them and
                // keep the unlit fireballs, sparks and shockwaves.
                renderer.enabled = false;
                continue;
            }
            if (!needsDepth) continue;
            if (!unlitCache.TryGetValue(source, out Material replacement) || replacement == null)
            {
                replacement = new Material(source) { name = source.name + " (Quest)" };
                if (lit)
                {
                    replacement.shader = unlit;
                    replacement.SetFloat("_Surface", 1f);
                    replacement.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    replacement.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                }
                replacement.DisableKeyword("_SOFTPARTICLES_ON");
                replacement.DisableKeyword("_FADING_ON");
                if (replacement.HasProperty("_SoftParticlesEnabled")) replacement.SetFloat("_SoftParticlesEnabled", 0f);
                if (replacement.HasProperty("_CameraFadingEnabled")) replacement.SetFloat("_CameraFadingEnabled", 0f);
                unlitCache[source] = replacement;
            }
            renderer.sharedMaterial = replacement;
        }
    }

    private static Material dustMaterial;

    /// <summary>Procedural dust burst: soft grit that billows out low and settles. No mesh puffs.</summary>
    public static void DustBurst(Vector3 position, float scale = 1f)
    {
        var go = new GameObject("Dust burst");
        go.transform.position = position + Vector3.up * 0.05f;
        var system = go.AddComponent<ParticleSystem>();
        system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var main = system.main;
        main.loop = false;
        main.duration = 0.2f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.8f, 1.6f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.6f * scale, 1.8f * scale);
        main.startSize = new ParticleSystem.MinMaxCurve(0.15f * scale, 0.45f * scale);
        main.startRotation = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(0.45f, 0.38f, 0.33f, 0.35f), new Color(0.25f, 0.2f, 0.18f, 0.5f));
        main.gravityModifier = 0.05f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 60;
        var emission = system.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, (short)(30 * scale), (short)(45 * scale)) });
        var shape = system.shape;
        shape.shapeType = ParticleSystemShapeType.Hemisphere;
        shape.radius = 0.25f * scale;
        shape.rotation = new Vector3(-90f, 0f, 0f);
        var limit = system.limitVelocityOverLifetime;
        limit.enabled = true;
        limit.dampen = 0.12f;
        limit.limit = 0.1f;
        var size = system.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.EaseInOut(0f, 0.5f, 1f, 1.8f));
        var colour = system.colorOverLifetime;
        colour.enabled = true;
        var fade = new Gradient();
        fade.SetKeys(new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0.6f, 0.4f), new GradientAlphaKey(0f, 1f) });
        colour.color = fade;
        var renderer = go.GetComponent<ParticleSystemRenderer>();
        if (dustMaterial == null)
        {
            dustMaterial = CombatVfxStyle.CreateMaterial("Dust", Color.white);
            dustMaterial.mainTexture = HudSprites.Dot().texture;
        }
        renderer.sharedMaterial = dustMaterial;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        system.Play();
        Object.Destroy(go, 2.5f);
    }

    public static GameObject Spawn(GameObject prefab, Vector3 position, float scale = 1f, float lifetime = 4f)
        => Spawn(prefab, position, Quaternion.identity, scale, lifetime);
}
