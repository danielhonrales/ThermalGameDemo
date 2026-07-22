using UnityEngine;

[DisallowMultipleComponent]
public sealed class ThermalBeamEffects : MonoBehaviour
{
    [Header("Beam")]
    [SerializeField, Min(0.001f)] private float coreWidth = 0.055f;
    [SerializeField, Min(0.001f)] private float glowWidth = 0.16f;
    [SerializeField, Min(0.001f)] private float haloWidth = 0.28f;
    [SerializeField, Min(0f)] private float beamStartExtension = 0.04f;
    [SerializeField, Min(0f)] private float beamEndExtension = 0.12f;
    [SerializeField] private float pulseSpeed = 18f;
    [SerializeField] private float pulseAmount = 0.18f;
    [SerializeField] private float beamScrollSpeed = 10f;
    [SerializeField] private float textureTileMeters = 0.12f;
    [SerializeField] private string beamTextureResourcePath = "CustomAssets/spark";
    [SerializeField] private Color fireCoreColor = new Color(1f, 0.97f, 0.82f, 1f);
    [SerializeField] private Color fireMidColor = new Color(1f, 0.52f, 0.06f, 0.92f);
    [SerializeField] private Color fireOuterColor = new Color(1f, 0.18f, 0.02f, 0.42f);
    [SerializeField, Min(0f)] private float beamLightIntensity = 5.5f;
    [SerializeField, Min(0f)] private float impactFlashIntensity = 7f;
    [SerializeField, Min(0f)] private float impactFlashDuration = 0.12f;
    [SerializeField] private bool showBeamTrailParticles = true;
    [SerializeField] private float beamTrailRate = 85f;
    [SerializeField] private bool showContinuousBeamSmoke = true;
    [SerializeField, Range(0f, 1f)] private float continuousSmokeAlpha = 0.68f;
    [SerializeField, Min(0f)] private float continuousSmokeRatePerMeter = 46f;
    [SerializeField, Min(0.01f)] private float continuousSmokeWidth = 0.24f;
    [SerializeField] private bool tileHotBeamParticles;
    [SerializeField, Min(0.1f)] private float hotBeamParticleTileSpacing = 0.45f;
    [SerializeField, Range(1, 96)] private int maxHotBeamParticleTiles = 64;
    [SerializeField, Min(0.01f)] private float hotBeamParticleTileScale = 1f;

    [Header("Charge")]
    [SerializeField] private string beamChargeResourcePath = "CustomAssets/Beam/Beam";
    [SerializeField] private string ledTubeChargeResourcePath = "CustomAssets/PowerSleeve/ledTube";
    [SerializeField] private Vector3 chargeLocalOffset = new Vector3(0f, 0f, 0.03f);
    [SerializeField] private float chargeStartScale = 0.45f;
    [SerializeField] private float chargeEndScale = 1.15f;
    [SerializeField] private float chargePulseSpeed = 7f;
    [SerializeField] private float chargePulseAmount = 0.14f;
    [SerializeField] private float chargeLightIntensity = 3.5f;
    [Tooltip("Large imported HotHit/Absorb aura layers. Disabled to keep the charge local to the hand.")]
    [SerializeField] private bool showLargeChargeAuraLayers;
    [SerializeField] private float chargeLayerScale = 14f;
    [SerializeField] private bool showFallbackChargeParticles = true;
    [SerializeField] private float chargeParticleRate = 55f;
    [SerializeField] private string chargeTextureResourcePath = "CustomAssets/circle";
    [Tooltip("Keeps compact smoke and sparks around the firing hand during both charge and beam fire.")]
    [SerializeField] private bool showPalmAtmosphere = true;
    [SerializeField, Min(0f)] private float palmSmokeRate = 34f;
    [SerializeField, Min(0f)] private float palmSparkRate = 68f;
    [SerializeField] private bool playChargeAudio = true;
    [SerializeField, Range(0f, 1f)] private float chargeVolume = 0.45f;
    [SerializeField] private string chargeAudioResourcePath = "CustomAssets/Audio/beamCharge";

    [Header("Impact")]
    [SerializeField] private bool showImpactParticles = true;
    [SerializeField, Min(0.04f)] private float aimMarkerSize = 0.14f;
    [SerializeField, Min(0.001f)] private float aimMarkerWidth = 0.01f;
    [SerializeField, Min(0.02f)] private float rangeLimitMarkerSize = 0.09f;
    [SerializeField, Min(0.001f)] private float rangeLimitMarkerWidth = 0.008f;
    [SerializeField] private float impactCooldownSeconds = 0.05f;
    [SerializeField] private string impactSparkTextureResourcePath = "CustomAssets/spark";
    [SerializeField] private string impactSmokeTextureResourcePath = "CustomAssets/smoke";

    [Header("Audio")]
    [SerializeField] private bool playBeamAudio = true;
    [SerializeField, Range(0f, 1f)] private float beamVolume = 0.25f;
    [SerializeField] private string beamLoopResourcePath = "CustomAssets/Audio/fireBeam";

    private LineRenderer coreLine;
    private LineRenderer glowLine;
    private LineRenderer haloLine;
    private LineRenderer aimMarkerRing;
    private LineRenderer aimMarkerCrossA;
    private LineRenderer aimMarkerCrossB;
    private LineRenderer rangeLimitMarkerRing;
    private ParticleSystem fallbackChargeParticles;
    private ParticleSystem palmSmokeParticles;
    private ParticleSystem palmSparkParticles;
    private ParticleSystem beamEmberTrail;
    private ParticleSystem beamSmokeTrail;
    private ParticleSystem muzzleEmbers;
    private ParticleSystem impactSparks;
    private ParticleSystem impactSmoke;
    private ParticleSystem impactFlare;
    private AudioSource beamAudio;
    private AudioSource chargeAudio;
    private Material coreMaterial;
    private Material glowMaterial;
    private Material haloMaterial;
    private Transform chargeRoot;
    private Transform beamTrailRoot;
    private ParticleSystem beamChargeParticles;
    private ParticleSystem[] layeredChargeParticles;
    private ParticleSystem[] hotBeamParticles;
    private Transform[] hotBeamParticleTileRoots;
    private ParticleSystem[][] hotBeamParticleTiles;
    private ParticleSystem[] visibleBeamTileParticles;
    private Light chargeLight;
    private Light beamLight;
    private Light impactFlashLight;
    private float lastImpactTime;
    private float impactFlashUntilTime;

    private void Awake()
    {
        EnsureEffects();
        SetVisible(false);
    }

    private void Update()
    {
        ScrollBeamMaterial(coreMaterial, beamScrollSpeed);
        ScrollBeamMaterial(glowMaterial, beamScrollSpeed * 0.55f);
        ScrollBeamMaterial(haloMaterial, beamScrollSpeed * 0.25f);

        if (impactFlashLight != null && Time.time >= impactFlashUntilTime)
        {
            impactFlashLight.enabled = false;
        }
    }

    public void ShowBeam(Vector3 start, Vector3 end, Color color, bool hitSomething)
    {
        ShowBeam(start, end, color, hitSomething, end, start, Quaternion.identity);
    }

    public void ShowBeam(Vector3 start, Vector3 end, Color color, bool hitSomething, Vector3 hitPoint, Vector3 mountPosition, Quaternion mountRotation)
    {
        EnsureEffects();
        HidePalmCharge();
        UpdatePalmAtmosphere(mountPosition, color, 1f);

        Vector3 direction = end - start;
        if (direction.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        direction.Normalize();
        Vector3 visualStart = start - direction * beamStartExtension;
        Vector3 visualEnd = end + direction * beamEndExtension;
        float length = Vector3.Distance(visualStart, visualEnd);
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        Color fireCore = BlendFireColor(fireCoreColor, color, 0.18f);
        Color fireMid = BlendFireColor(fireMidColor, color, 0.42f);
        Color fireOuter = BlendFireColor(fireOuterColor, color, 0.55f);
        SetLine(coreLine, visualStart, visualEnd, coreWidth * pulse, fireCore, fireMid, length);
        SetLine(glowLine, visualStart, visualEnd, glowWidth * pulse, WithAlpha(fireMid, 0.78f), WithAlpha(fireOuter, 0.42f), length);
        SetLine(haloLine, visualStart, visualEnd, haloWidth * pulse, WithAlpha(fireOuter, 0.5f), WithAlpha(fireOuter, 0.08f), length);
        UpdateBeamTrail(visualStart, visualEnd, direction, length, fireMid, pulse);

        if (fallbackChargeParticles != null)
        {
            fallbackChargeParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }

        if (chargeAudio != null && chargeAudio.isPlaying)
        {
            chargeAudio.Stop();
        }

        if (showImpactParticles && hitSomething && Time.time >= lastImpactTime + impactCooldownSeconds)
        {
            lastImpactTime = Time.time;
            PlayImpact(hitPoint, color);
        }

        if (beamAudio != null && !beamAudio.isPlaying)
        {
            beamAudio.Play();
        }
    }

    public void ShowCharge(Vector3 start, Color color)
    {
        ShowCharge(start, color, 1f);
    }

    public void ShowCharge(Vector3 start, Color color, float progress)
    {
        ShowCharge(start, color, progress, start, Quaternion.identity);
    }

    public void ShowCharge(Vector3 start, Color color, float progress, Vector3 mountPosition, Quaternion mountRotation)
    {
        EnsureEffects();
        HideLines();

        if (beamAudio != null && beamAudio.isPlaying)
        {
            beamAudio.Stop();
        }

        progress = Mathf.Clamp01(progress);
        float pulse = 1f + Mathf.Sin(Time.time * chargePulseSpeed) * chargePulseAmount * progress;
        float scale = Mathf.Lerp(chargeStartScale, chargeEndScale, progress) * pulse;
        Vector3 chargePosition = start + mountRotation * chargeLocalOffset;

        if (chargeRoot != null)
        {
            chargeRoot.gameObject.SetActive(true);
            chargeRoot.SetPositionAndRotation(chargePosition, mountRotation);
            chargeRoot.localScale = Vector3.one * scale;
        }

        Color chargeTint = BlendFireColor(fireMidColor, color, 0.35f);
        chargeTint = WithAlpha(chargeTint, Mathf.Lerp(0.45f, 1f, progress));
        SetParticleColor(beamChargeParticles, chargeTint, progress);
        PlayParticles(beamChargeParticles);
        SetLayeredChargeIntensity(color, progress);
        PlayParticles(layeredChargeParticles);

        if (chargeLight != null)
        {
            chargeLight.enabled = true;
            chargeLight.color = BlendFireColor(fireMidColor, color, 0.35f);
            chargeLight.intensity = chargeLightIntensity * progress * pulse;
        }

        if (showFallbackChargeParticles && fallbackChargeParticles != null)
        {
            fallbackChargeParticles.transform.position = chargePosition;
            ParticleSystem.MainModule main = fallbackChargeParticles.main;
            main.startColor = chargeTint;
            main.startSize = Mathf.Lerp(0.04f, 0.11f, progress);

            ParticleSystem.EmissionModule emission = fallbackChargeParticles.emission;
            emission.rateOverTime = chargeParticleRate * Mathf.Lerp(0.2f, 1f, progress);

            if (!fallbackChargeParticles.isPlaying)
            {
                fallbackChargeParticles.Play();
            }
        }

        UpdatePalmAtmosphere(chargePosition, chargeTint, progress);

        if (playChargeAudio && chargeAudio != null)
        {
            chargeAudio.volume = chargeVolume * Mathf.Lerp(0.35f, 1f, progress);
            if (!chargeAudio.isPlaying)
            {
                chargeAudio.Play();
            }
        }
    }

    public void HideBeam()
    {
        SetVisible(false);
    }

    public void ShowAimMarker(Vector3 position, Color color)
    {
        EnsureEffects();
        EnsureAimMarker();
        Color markerColor = WithAlpha(color, Mathf.Max(0.65f, color.a));

        SetAimMarkerRing(aimMarkerRing, position, aimMarkerSize, aimMarkerWidth, markerColor);
        SetAimMarkerCross(aimMarkerCrossA, position, Vector3.up, Vector3.right, markerColor);
        SetAimMarkerCross(aimMarkerCrossB, position, Vector3.forward, Vector3.right, markerColor);
    }

    public void HideAimMarker()
    {
        if (aimMarkerRing != null)
        {
            aimMarkerRing.enabled = false;
        }

        if (aimMarkerCrossA != null)
        {
            aimMarkerCrossA.enabled = false;
        }

        if (aimMarkerCrossB != null)
        {
            aimMarkerCrossB.enabled = false;
        }
    }

    public void ShowRangeLimitMarker(Vector3 position, Color color)
    {
        EnsureEffects();
        EnsureAimMarker();
        SetAimMarkerRing(rangeLimitMarkerRing, position, rangeLimitMarkerSize, rangeLimitMarkerWidth, WithAlpha(color, 0.75f));
    }

    public void HideRangeLimitMarker()
    {
        if (rangeLimitMarkerRing != null)
        {
            rangeLimitMarkerRing.enabled = false;
        }
    }

    private void EnsureEffects()
    {
        if (coreLine == null)
        {
            Texture2D beamTexture = Resources.Load<Texture2D>(beamTextureResourcePath);
            coreMaterial = CreateBeamMaterial("BeamCoreMaterial", beamTexture);
            glowMaterial = CreateBeamMaterial("BeamGlowMaterial", beamTexture);
            haloMaterial = CreateBeamMaterial("BeamHaloMaterial", beamTexture);

            coreLine = CreateLine("BeamCore", coreMaterial, 0);
            glowLine = CreateLine("BeamGlow", glowMaterial, -1);
            haloLine = CreateLine("BeamHalo", haloMaterial, -2);
        }

        EnsurePalmCharge();
        EnsureBeamTrail();

        if (showFallbackChargeParticles && fallbackChargeParticles == null)
        {
            fallbackChargeParticles = CreateChargeParticles();
            fallbackChargeParticles.transform.SetParent(transform, false);
        }

        if (showPalmAtmosphere && palmSmokeParticles == null)
        {
            palmSmokeParticles = CreatePalmSmokeParticles();
            palmSmokeParticles.transform.SetParent(transform, false);
            palmSparkParticles = CreatePalmSparkParticles();
            palmSparkParticles.transform.SetParent(transform, false);
        }

        if (showImpactParticles && impactSparks == null)
        {
            impactSparks = CreateImpactSparks();
            impactSparks.transform.SetParent(transform, false);
        }

        if (showImpactParticles && impactSmoke == null)
        {
            impactSmoke = CreateImpactSmoke();
            impactSmoke.transform.SetParent(transform, false);
        }

        if (showImpactParticles && impactFlare == null)
        {
            impactFlare = CreateImpactFlare();
            impactFlare.transform.SetParent(transform, false);
        }

        if (playBeamAudio && beamAudio == null)
        {
            beamAudio = gameObject.AddComponent<AudioSource>();
            beamAudio.clip = Resources.Load<AudioClip>(beamLoopResourcePath);
            beamAudio.loop = true;
            beamAudio.playOnAwake = false;
            beamAudio.spatialBlend = 1f;
            beamAudio.volume = beamVolume;
            beamAudio.minDistance = 0.2f;
            beamAudio.maxDistance = 8f;
        }

        if (playChargeAudio && chargeAudio == null)
        {
            chargeAudio = gameObject.AddComponent<AudioSource>();
            chargeAudio.clip = Resources.Load<AudioClip>(chargeAudioResourcePath);
            chargeAudio.loop = true;
            chargeAudio.playOnAwake = false;
            chargeAudio.spatialBlend = 1f;
            chargeAudio.volume = chargeVolume;
            chargeAudio.minDistance = 0.15f;
            chargeAudio.maxDistance = 6f;
        }
    }

    private LineRenderer CreateLine(string objectName, Material material, int sortingOrder)
    {
        GameObject lineObject = new GameObject(objectName);
        lineObject.transform.SetParent(transform, false);

        LineRenderer line = lineObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.useWorldSpace = true;
        line.textureMode = LineTextureMode.Tile;
        line.alignment = LineAlignment.View;
        line.numCapVertices = 8;
        line.numCornerVertices = 2;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        line.sortingOrder = sortingOrder;
        line.material = material;
        line.enabled = false;
        return line;
    }

    private Material CreateBeamMaterial(string materialName, Texture2D texture)
    {
        Shader shader = Shader.Find("Sprites/Default");
        if (shader == null)
        {
            shader = Shader.Find("Particles/Standard Unlit");
        }

        Material material = new Material(shader)
        {
            name = materialName,
            mainTexture = texture
        };

        if (material.HasProperty("_Color"))
        {
            material.SetColor("_Color", Color.white);
        }

        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", Color.white);
        }

        return material;
    }

    private void SetLine(LineRenderer line, Vector3 start, Vector3 end, float width, Color startColor, Color endColor, float length)
    {
        if (line == null)
        {
            return;
        }

        line.enabled = true;
        line.startWidth = width;
        line.endWidth = width * 0.72f;
        line.startColor = startColor;
        line.endColor = endColor;
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        if (line.material != null)
        {
            line.material.mainTextureScale = new Vector2(Mathf.Max(1f, length / Mathf.Max(0.01f, textureTileMeters)), 1f);
        }
    }

    private void SetVisible(bool visible)
    {
        if (visible)
        {
            return;
        }

        HideLines();
        HidePalmCharge();
        HideAimMarker();
        HideRangeLimitMarker();

        if (fallbackChargeParticles != null)
        {
            fallbackChargeParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }

        if (chargeAudio != null && chargeAudio.isPlaying)
        {
            chargeAudio.Stop();
        }

        if (beamAudio != null && beamAudio.isPlaying)
        {
            beamAudio.Stop();
        }

        StopPalmAtmosphere();
        StopBeamTrail();
    }

    private void EnsurePalmCharge()
    {
        if (chargeRoot != null)
        {
            return;
        }

        GameObject rootObject = new GameObject("PalmChargeRoot");
        rootObject.transform.SetParent(transform, false);
        chargeRoot = rootObject.transform;

        beamChargeParticles = ExtractBeamChargeParticles(chargeRoot);
        layeredChargeParticles = showLargeChargeAuraLayers
            ? ExtractLedTubeChargeParticles(chargeRoot)
            : System.Array.Empty<ParticleSystem>();

        GameObject lightObject = new GameObject("PalmChargeLight");
        lightObject.transform.SetParent(chargeRoot, false);
        chargeLight = lightObject.AddComponent<Light>();
        chargeLight.type = LightType.Point;
        chargeLight.range = 0.9f;
        chargeLight.shadows = LightShadows.None;
        chargeLight.enabled = false;

        HidePalmCharge();
    }

    private ParticleSystem ExtractBeamChargeParticles(Transform parent)
    {
        GameObject sourcePrefab = Resources.Load<GameObject>(beamChargeResourcePath);
        if (sourcePrefab == null)
        {
            return null;
        }

        GameObject beamInstance = Instantiate(sourcePrefab, parent);
        beamInstance.name = "BeamChargeSource";

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

    private ParticleSystem[] ExtractLedTubeChargeParticles(Transform parent)
    {
        GameObject sourcePrefab = Resources.Load<GameObject>(ledTubeChargeResourcePath);
        if (sourcePrefab == null)
        {
            return System.Array.Empty<ParticleSystem>();
        }

        GameObject ledTubeInstance = Instantiate(sourcePrefab, parent);
        ledTubeInstance.name = "LedTubeChargeSource";

        System.Collections.Generic.List<ParticleSystem> particles = new System.Collections.Generic.List<ParticleSystem>();
        Transform effects = ledTubeInstance.transform.Find("Effects");
        if (effects != null)
        {
            ReparentChargeLayer(effects, "HotHit", parent, particles, chargeLayerScale);
            ReparentChargeLayer(effects, "Absorb", parent, particles, chargeLayerScale * 0.65f);
        }

        foreach (Transform child in ledTubeInstance.GetComponentsInChildren<Transform>(true))
        {
            if (child.name.StartsWith("cylinder"))
            {
                child.gameObject.SetActive(false);
            }
        }

        foreach (MeshRenderer meshRenderer in ledTubeInstance.GetComponentsInChildren<MeshRenderer>(true))
        {
            meshRenderer.enabled = false;
        }

        foreach (Collider collider in ledTubeInstance.GetComponentsInChildren<Collider>(true))
        {
            collider.enabled = false;
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

    private void HidePalmCharge()
    {
        if (chargeRoot != null)
        {
            chargeRoot.gameObject.SetActive(false);
        }

        StopParticles(beamChargeParticles);
        StopParticles(layeredChargeParticles);

        if (chargeLight != null)
        {
            chargeLight.enabled = false;
        }
    }

    private void UpdatePalmAtmosphere(Vector3 position, Color tint, float intensity)
    {
        if (!showPalmAtmosphere)
        {
            return;
        }

        intensity = Mathf.Clamp01(intensity);
        if (palmSmokeParticles != null)
        {
            palmSmokeParticles.transform.position = position;
            ParticleSystem.MainModule main = palmSmokeParticles.main;
            main.startColor = WithAlpha(BlendFireColor(fireOuterColor, tint, 0.3f), Mathf.Lerp(0.28f, 0.72f, intensity));
            main.startSize = Mathf.Lerp(0.1f, 0.2f, intensity);
            ParticleSystem.EmissionModule emission = palmSmokeParticles.emission;
            emission.rateOverTime = palmSmokeRate * Mathf.Lerp(0.35f, 1f, intensity);
            PlayParticles(palmSmokeParticles);
        }

        if (palmSparkParticles != null)
        {
            palmSparkParticles.transform.position = position;
            ParticleSystem.MainModule main = palmSparkParticles.main;
            main.startColor = WithAlpha(BlendFireColor(fireCoreColor, tint, 0.35f), Mathf.Lerp(0.55f, 1f, intensity));
            main.startSize = Mathf.Lerp(0.045f, 0.09f, intensity);
            ParticleSystem.EmissionModule emission = palmSparkParticles.emission;
            emission.rateOverTime = palmSparkRate * Mathf.Lerp(0.3f, 1f, intensity);
            PlayParticles(palmSparkParticles);
        }
    }

    private void StopPalmAtmosphere()
    {
        StopParticles(palmSmokeParticles);
        StopParticles(palmSparkParticles);
    }

    private void SetLayeredChargeIntensity(Color color, float progress)
    {
        if (layeredChargeParticles == null)
        {
            return;
        }

        Color layeredColor = WithAlpha(BlendFireColor(fireMidColor, color, 0.4f), Mathf.Lerp(0.35f, 0.95f, progress));
        foreach (ParticleSystem particle in layeredChargeParticles)
        {
            SetParticleColor(particle, layeredColor, progress);
        }
    }

    private static void SetParticleColor(ParticleSystem particles, Color color, float progress)
    {
        if (particles == null)
        {
            return;
        }

        ParticleSystem.MainModule main = particles.main;
        main.startColor = color;
    }

    private void HideLines()
    {
        if (coreLine != null)
        {
            coreLine.enabled = false;
        }

        if (glowLine != null)
        {
            glowLine.enabled = false;
        }

        if (haloLine != null)
        {
            haloLine.enabled = false;
        }
    }

    private void EnsureAimMarker()
    {
        if (aimMarkerRing == null)
        {
            aimMarkerRing = CreateLine("AimMarkerRing", CreateBeamMaterial("AimMarkerRingMaterial", null), 2);
            aimMarkerRing.loop = true;
            aimMarkerRing.positionCount = 16;
        }

        if (aimMarkerCrossA == null)
        {
            aimMarkerCrossA = CreateLine("AimMarkerCrossA", CreateBeamMaterial("AimMarkerCrossAMaterial", null), 3);
        }

        if (aimMarkerCrossB == null)
        {
            aimMarkerCrossB = CreateLine("AimMarkerCrossB", CreateBeamMaterial("AimMarkerCrossBMaterial", null), 3);
        }

        if (rangeLimitMarkerRing == null)
        {
            rangeLimitMarkerRing = CreateLine("RangeLimitMarkerRing", CreateBeamMaterial("RangeLimitMarkerRingMaterial", null), 1);
            rangeLimitMarkerRing.loop = true;
            rangeLimitMarkerRing.positionCount = 12;
        }
    }

    private void SetAimMarkerRing(LineRenderer ring, Vector3 position, float size, float width, Color color)
    {
        if (ring == null)
        {
            return;
        }

        ring.enabled = true;
        ring.startWidth = width;
        ring.endWidth = width;
        ring.startColor = color;
        ring.endColor = color;

        Vector3 cameraUp = Camera.main != null ? Camera.main.transform.up : Vector3.up;
        Vector3 cameraRight = Camera.main != null ? Camera.main.transform.right : Vector3.right;
        if (cameraUp.sqrMagnitude <= 0.0001f)
        {
            cameraUp = Vector3.up;
        }

        if (cameraRight.sqrMagnitude <= 0.0001f)
        {
            cameraRight = Vector3.right;
        }

        for (int i = 0; i < ring.positionCount; i++)
        {
            float angle = i / (float)ring.positionCount * Mathf.PI * 2f;
            Vector3 offset = (Mathf.Cos(angle) * cameraRight + Mathf.Sin(angle) * cameraUp) * size * 0.5f;
            ring.SetPosition(i, position + offset);
        }
    }

    private void SetAimMarkerCross(LineRenderer line, Vector3 position, Vector3 axisA, Vector3 axisB, Color color)
    {
        if (line == null)
        {
            return;
        }

        line.enabled = true;
        line.loop = false;
        line.positionCount = 2;
        line.startWidth = aimMarkerWidth * 1.2f;
        line.endWidth = aimMarkerWidth * 1.2f;
        line.startColor = color;
        line.endColor = color;

        Vector3 direction = Vector3.Cross(axisA, axisB);
        if (direction.sqrMagnitude <= 0.0001f)
        {
            direction = axisA;
        }

        direction.Normalize();
        Vector3 offset = direction * aimMarkerSize * 0.5f;
        line.SetPosition(0, position - offset);
        line.SetPosition(1, position + offset);
    }

    private void PlayImpact(Vector3 position, Color color)
    {
        Color fireHit = BlendFireColor(fireMidColor, color, 0.5f);

        if (impactSparks != null)
        {
            impactSparks.transform.position = position;
            ParticleSystem.MainModule main = impactSparks.main;
            main.startColor = fireHit;
            impactSparks.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            impactSparks.Play();
        }

        if (impactFlare != null)
        {
            impactFlare.transform.position = position;
            ParticleSystem.MainModule main = impactFlare.main;
            main.startColor = BlendFireColor(fireCoreColor, color, 0.35f);
            impactFlare.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            impactFlare.Play();
        }

        if (impactSmoke != null)
        {
            impactSmoke.transform.position = position;
            ParticleSystem.MainModule main = impactSmoke.main;
            main.startColor = WithAlpha(BlendFireColor(fireOuterColor, color, 0.25f), 0.55f);
            impactSmoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            impactSmoke.Play();
        }

        if (impactFlashLight != null)
        {
            impactFlashLight.transform.position = position;
            impactFlashLight.color = fireHit;
            impactFlashLight.intensity = impactFlashIntensity;
            impactFlashLight.enabled = true;
            impactFlashUntilTime = Time.time + impactFlashDuration;
        }
    }

    private void EnsureBeamTrail()
    {
        if (beamTrailRoot != null)
        {
            return;
        }

        GameObject rootObject = new GameObject("BeamTrailRoot");
        rootObject.transform.SetParent(transform, false);
        beamTrailRoot = rootObject.transform;

        if (tileHotBeamParticles)
        {
            EnsureHotBeamParticleTilePool(1);
        }
        else
        {
            hotBeamParticles = ExtractHotBeamParticles(beamTrailRoot);
        }

        if (showBeamTrailParticles)
        {
            beamEmberTrail = CreateBeamEmberTrail();
            beamEmberTrail.transform.SetParent(beamTrailRoot, false);
            muzzleEmbers = CreateMuzzleEmbers();
            muzzleEmbers.transform.SetParent(beamTrailRoot, false);
        }

        if (showContinuousBeamSmoke)
        {
            beamSmokeTrail = CreateContinuousBeamSmoke();
            beamSmokeTrail.transform.SetParent(beamTrailRoot, false);
        }

        GameObject beamLightObject = new GameObject("BeamLight");
        beamLightObject.transform.SetParent(beamTrailRoot, false);
        beamLight = beamLightObject.AddComponent<Light>();
        beamLight.type = LightType.Point;
        beamLight.range = 2.4f;
        beamLight.shadows = LightShadows.None;
        beamLight.enabled = false;

        GameObject impactLightObject = new GameObject("ImpactFlashLight");
        impactLightObject.transform.SetParent(transform, false);
        impactFlashLight = impactLightObject.AddComponent<Light>();
        impactFlashLight.type = LightType.Point;
        impactFlashLight.range = 2.8f;
        impactFlashLight.shadows = LightShadows.None;
        impactFlashLight.enabled = false;
    }

    private void UpdateBeamTrail(Vector3 start, Vector3 end, Vector3 direction, float length, Color tint, float pulse)
    {
        EnsureBeamTrail();
        if (beamTrailRoot == null)
        {
            return;
        }

        beamTrailRoot.SetPositionAndRotation(start, Quaternion.LookRotation(direction, Vector3.up));
        beamTrailRoot.localScale = Vector3.one;

        Color trailTint = BlendFireColor(fireMidColor, tint, 0.35f);
        if (tileHotBeamParticles)
        {
            UpdateTiledHotBeamParticles(length, trailTint);
        }
        else
        {
            PlayParticles(hotBeamParticles, trailTint);
        }

        if (beamEmberTrail != null)
        {
            beamEmberTrail.transform.localPosition = Vector3.forward * (length * 0.5f);
            beamEmberTrail.transform.localRotation = Quaternion.identity;
            ParticleSystem.MainModule main = beamEmberTrail.main;
            main.startColor = trailTint;
            ParticleSystem.EmissionModule emission = beamEmberTrail.emission;
            emission.rateOverTime = beamTrailRate * pulse * Mathf.Clamp(length / 2f, 1f, 36f);
            ParticleSystem.ShapeModule shape = beamEmberTrail.shape;
            shape.scale = new Vector3(0.08f, 0.08f, Mathf.Max(0.05f, length));
            if (!beamEmberTrail.isPlaying)
            {
                beamEmberTrail.Play();
            }
        }

        if (beamSmokeTrail != null)
        {
            beamSmokeTrail.transform.localPosition = Vector3.forward * (length * 0.5f);
            beamSmokeTrail.transform.localRotation = Quaternion.identity;
            ParticleSystem.MainModule main = beamSmokeTrail.main;
            Color smokeColor = BlendFireColor(fireOuterColor, tint, 0.28f);
            main.startColor = WithAlpha(smokeColor, continuousSmokeAlpha);
            ParticleSystem.EmissionModule emission = beamSmokeTrail.emission;
            emission.rateOverTime = continuousSmokeRatePerMeter * Mathf.Clamp(length, 0.5f, 28f);
            ParticleSystem.ShapeModule shape = beamSmokeTrail.shape;
            shape.scale = new Vector3(continuousSmokeWidth, continuousSmokeWidth, Mathf.Max(0.05f, length));
            if (!beamSmokeTrail.isPlaying)
            {
                beamSmokeTrail.Play();
            }
        }

        if (muzzleEmbers != null)
        {
            muzzleEmbers.transform.localPosition = Vector3.zero;
            ParticleSystem.MainModule main = muzzleEmbers.main;
            main.startColor = BlendFireColor(fireCoreColor, tint, 0.2f);
            if (!muzzleEmbers.isPlaying)
            {
                muzzleEmbers.Play();
            }
        }

        if (beamLight != null)
        {
            beamLight.enabled = true;
            beamLight.transform.localPosition = Vector3.forward * (length * 0.5f);
            beamLight.color = trailTint;
            beamLight.intensity = beamLightIntensity * pulse;
            beamLight.range = Mathf.Max(2.4f, length * 0.65f);
        }
    }

    private void StopBeamTrail()
    {
        StopParticles(hotBeamParticles);
        StopTiledHotBeamParticles();
        StopParticles(beamEmberTrail);
        StopParticles(beamSmokeTrail);
        StopParticles(muzzleEmbers);

        if (beamLight != null)
        {
            beamLight.enabled = false;
        }
    }

    private ParticleSystem[] ExtractHotBeamParticles(Transform parent)
    {
        GameObject sourcePrefab = Resources.Load<GameObject>(beamChargeResourcePath);
        if (sourcePrefab == null)
        {
            return System.Array.Empty<ParticleSystem>();
        }

        GameObject beamInstance = Instantiate(sourcePrefab, parent);
        beamInstance.name = "HotBeamSource";

        System.Collections.Generic.List<ParticleSystem> particles = new System.Collections.Generic.List<ParticleSystem>();
        Transform hotBeam = beamInstance.transform.Find("HotBeam");
        if (hotBeam != null)
        {
            hotBeam.SetParent(parent, false);
            hotBeam.localPosition = Vector3.zero;
            hotBeam.localRotation = Quaternion.identity;
            hotBeam.localScale = Vector3.one;
            hotBeam.gameObject.SetActive(true);
            particles.AddRange(hotBeam.GetComponentsInChildren<ParticleSystem>(true));
        }

        Destroy(beamInstance);
        ParticleSystem[] extractedParticles = particles.ToArray();
        foreach (ParticleSystem particle in extractedParticles)
        {
            if (particle == null)
            {
                continue;
            }

            ParticleSystem.MainModule main = particle.main;
            main.loop = true;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
        }

        return extractedParticles;
    }

    private void EnsureHotBeamParticleTilePool(int requiredCount)
    {
        if (!tileHotBeamParticles || beamTrailRoot == null)
        {
            return;
        }

        int clampedRequiredCount = Mathf.Clamp(requiredCount, 1, Mathf.Max(1, maxHotBeamParticleTiles));
        if (hotBeamParticleTileRoots == null || hotBeamParticleTileRoots.Length != maxHotBeamParticleTiles)
        {
            hotBeamParticleTileRoots = new Transform[maxHotBeamParticleTiles];
            hotBeamParticleTiles = new ParticleSystem[maxHotBeamParticleTiles][];
            visibleBeamTileParticles = new ParticleSystem[maxHotBeamParticleTiles];
        }

        for (int i = 0; i < clampedRequiredCount; i++)
        {
            if (hotBeamParticleTileRoots[i] != null)
            {
                continue;
            }

            GameObject tileObject = new GameObject($"HotBeamParticleTile_{i:00}");
            tileObject.transform.SetParent(beamTrailRoot, false);
            tileObject.SetActive(false);

            hotBeamParticleTileRoots[i] = tileObject.transform;
            hotBeamParticleTiles[i] = ExtractHotBeamParticles(tileObject.transform);
            visibleBeamTileParticles[i] = CreateVisibleBeamTileParticles(tileObject.transform);
        }
    }

    private void UpdateTiledHotBeamParticles(float length, Color tint)
    {
        int tileCount = Mathf.Clamp(
            Mathf.CeilToInt(length / Mathf.Max(0.1f, hotBeamParticleTileSpacing)) + 1,
            1,
            Mathf.Max(1, maxHotBeamParticleTiles));
        EnsureHotBeamParticleTilePool(tileCount);

        for (int i = 0; i < maxHotBeamParticleTiles; i++)
        {
            Transform tileRoot = hotBeamParticleTileRoots != null && i < hotBeamParticleTileRoots.Length
                ? hotBeamParticleTileRoots[i]
                : null;
            if (tileRoot == null)
            {
                continue;
            }

            bool shouldShowTile = i < tileCount;
            tileRoot.gameObject.SetActive(shouldShowTile);
            if (!shouldShowTile)
            {
                if (hotBeamParticleTiles != null && i < hotBeamParticleTiles.Length)
                {
                    StopParticles(hotBeamParticleTiles[i]);
                }

                continue;
            }

            float t = tileCount <= 1 ? 0.5f : i / (float)(tileCount - 1);
            tileRoot.localPosition = Vector3.forward * (length * t);
            tileRoot.localRotation = Quaternion.identity;
            tileRoot.localScale = Vector3.one * hotBeamParticleTileScale;

            if (hotBeamParticleTiles != null && i < hotBeamParticleTiles.Length)
            {
                PlayParticles(hotBeamParticleTiles[i], tint);
            }

            if (visibleBeamTileParticles != null
                && i < visibleBeamTileParticles.Length
                && visibleBeamTileParticles[i] != null)
            {
                ParticleSystem tileParticles = visibleBeamTileParticles[i];
                ParticleSystem.MainModule main = tileParticles.main;
                main.startColor = WithAlpha(tint, 0.78f);
                ParticleSystem.EmissionModule emission = tileParticles.emission;
                emission.rateOverTime = beamTrailRate * 0.45f;

                if (!tileParticles.isPlaying)
                {
                    tileParticles.Play();
                }
            }
        }
    }

    private void StopTiledHotBeamParticles()
    {
        if (hotBeamParticleTileRoots == null)
        {
            return;
        }

        for (int i = 0; i < hotBeamParticleTileRoots.Length; i++)
        {
            if (hotBeamParticleTiles != null && i < hotBeamParticleTiles.Length)
            {
                StopParticles(hotBeamParticleTiles[i]);
            }

            if (visibleBeamTileParticles != null
                && i < visibleBeamTileParticles.Length
                && visibleBeamTileParticles[i] != null)
            {
                StopParticles(visibleBeamTileParticles[i]);
            }

            if (hotBeamParticleTileRoots[i] != null)
            {
                hotBeamParticleTileRoots[i].gameObject.SetActive(false);
            }
        }
    }

    private ParticleSystem CreateVisibleBeamTileParticles(Transform parent)
    {
        GameObject particleObject = new GameObject("VisibleBeamTileParticles");
        particleObject.transform.SetParent(parent, false);

        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = 0.2f;
        main.startSpeed = 0.08f;
        main.startSize = 0.13f;
        main.maxParticles = 48;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = beamTrailRate * 0.45f;

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.055f;

        ParticleSystem.VelocityOverLifetimeModule velocity = particles.velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.Local;
        velocity.z = 0.55f;

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(fireCoreColor, 0f),
                new GradientColorKey(fireMidColor, 0.45f),
                new GradientColorKey(fireOuterColor, 1f)
            },
            new[]
            {
                new GradientAlphaKey(0.95f, 0f),
                new GradientAlphaKey(0.55f, 0.55f),
                new GradientAlphaKey(0f, 1f)
            });
        colorOverLifetime.color = gradient;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(beamTextureResourcePath));
        return particles;
    }

    private ParticleSystem CreateBeamEmberTrail()
    {
        GameObject particleObject = new GameObject("BeamEmberTrail");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = 0.22f;
        main.startSpeed = 0.15f;
        main.startSize = 0.055f;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = beamTrailRate;

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(0.08f, 0.08f, 1f);

        ParticleSystem.VelocityOverLifetimeModule velocity = particles.velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.Local;
        velocity.z = 2.2f;

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(fireCoreColor, 0f),
                new GradientColorKey(fireMidColor, 0.35f),
                new GradientColorKey(fireOuterColor, 1f)
            },
            new[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0.45f, 0.65f),
                new GradientAlphaKey(0f, 1f)
            });
        colorOverLifetime.color = gradient;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(beamTextureResourcePath));
        return particles;
    }

    private ParticleSystem CreateContinuousBeamSmoke()
    {
        GameObject particleObject = new GameObject("BeamVolumeSmoke");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        // This is intentionally larger and denser than the ember stream.  In passthrough,
        // the old low-alpha, short-lived particles were effectively invisible against a room.
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.32f, 0.58f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.04f, 0.18f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.15f, 0.3f);
        main.maxParticles = 900;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = continuousSmokeRatePerMeter;

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(continuousSmokeWidth, continuousSmokeWidth, 1f);

        ParticleSystem.NoiseModule noise = particles.noise;
        noise.enabled = true;
        noise.strength = 0.14f;
        noise.frequency = 1.25f;
        noise.scrollSpeed = 0.32f;
        noise.damping = true;

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(fireCoreColor, 0f),
                new GradientColorKey(fireMidColor, 0.28f),
                new GradientColorKey(fireOuterColor, 1f)
            },
            new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(continuousSmokeAlpha, 0.12f),
                new GradientAlphaKey(continuousSmokeAlpha * 0.7f, 0.7f),
                new GradientAlphaKey(0f, 1f)
            });
        colorOverLifetime.color = gradient;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(impactSmokeTextureResourcePath));
        return particles;
    }

    private ParticleSystem CreateMuzzleEmbers()
    {
        GameObject particleObject = new GameObject("BeamMuzzleEmbers");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = 0.14f;
        main.startSpeed = 0.55f;
        main.startSize = 0.07f;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 42f;

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 18f;
        shape.radius = 0.03f;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(beamTextureResourcePath));
        return particles;
    }

    private ParticleSystem CreateImpactFlare()
    {
        GameObject particleObject = new GameObject("BeamImpactFlare");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = false;
        main.startLifetime = 0.12f;
        main.startSpeed = 0.2f;
        main.startSize = 0.22f;
        main.maxParticles = 24;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 10) });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.05f;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(chargeTextureResourcePath));
        return particles;
    }

    private static Color BlendFireColor(Color fireColor, Color semanticColor, float semanticWeight)
    {
        return Color.Lerp(fireColor, semanticColor, Mathf.Clamp01(semanticWeight));
    }

    private static void PlayParticles(ParticleSystem[] particles, Color tint)
    {
        if (particles == null)
        {
            return;
        }

        foreach (ParticleSystem particle in particles)
        {
            if (particle == null)
            {
                continue;
            }

            SetParticleColor(particle, tint, 1f);
            if (!particle.isPlaying)
            {
                particle.Play();
            }
        }
    }

    private ParticleSystem CreateChargeParticles()
    {
        GameObject particleObject = new GameObject("BeamChargeParticles");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = 0.28f;
        main.startSpeed = -0.35f;
        main.startSize = 0.06f;
        main.startColor = fireMidColor;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = chargeParticleRate;

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.07f;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(chargeTextureResourcePath));
        return particles;
    }

    private ParticleSystem CreatePalmSmokeParticles()
    {
        GameObject particleObject = new GameObject("BeamPalmSmoke");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.38f, 0.68f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.04f, 0.18f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.2f);
        main.maxParticles = 160;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = palmSmokeRate;
        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.075f;
        ParticleSystem.NoiseModule noise = particles.noise;
        noise.enabled = true;
        noise.strength = 0.12f;
        noise.frequency = 1.4f;
        noise.scrollSpeed = 0.35f;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(impactSmokeTextureResourcePath));
        return particles;
    }

    private ParticleSystem CreatePalmSparkParticles()
    {
        GameObject particleObject = new GameObject("BeamPalmSparks");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.16f, 0.3f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.35f, 0.9f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.045f, 0.09f);
        main.maxParticles = 200;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = palmSparkRate;
        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.055f;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(impactSparkTextureResourcePath));
        return particles;
    }

    private ParticleSystem CreateImpactSparks()
    {
        GameObject particleObject = new GameObject("BeamImpactSparks");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = false;
        main.startLifetime = 0.18f;
        main.startSpeed = 1.35f;
        main.startSize = 0.1f;
        main.maxParticles = 80;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 22) });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.035f;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(impactSparkTextureResourcePath));
        return particles;
    }

    private ParticleSystem CreateImpactSmoke()
    {
        GameObject particleObject = new GameObject("BeamImpactSmoke");
        ParticleSystem particles = particleObject.AddComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particles.main;
        main.loop = false;
        main.startLifetime = 0.34f;
        main.startSpeed = 0.32f;
        main.startSize = 0.18f;
        main.maxParticles = 28;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 8) });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.045f;

        ConfigureParticleRenderer(particles, Resources.Load<Texture2D>(impactSmokeTextureResourcePath));
        return particles;
    }

    private void ConfigureParticleRenderer(ParticleSystem particles, Texture2D texture)
    {
        ParticleSystemRenderer renderer = particles.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material = CreateBeamMaterial($"{particles.name}Material", texture);
    }

    private static Color WithAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    private static void PlayParticles(ParticleSystem particle)
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
            if (particle != null && !particle.isPlaying)
            {
                particle.Play();
            }
        }
    }

    private static void StopParticles(ParticleSystem particle)
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
            if (particle != null)
            {
                particle.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    private static void ScrollBeamMaterial(Material material, float speed)
    {
        if (material == null)
        {
            return;
        }

        Vector2 offset = material.mainTextureOffset;
        offset.x = -Time.time * speed;
        material.mainTextureOffset = offset;
    }
}
