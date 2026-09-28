using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>Two shared, glowing refuges appear for five seconds before an arena-wide blast.</summary>
[DisallowMultipleComponent]
public sealed class SafeZoneHazardView : MonoBehaviour
{
    public const float WarningAt = 20f;
    public const float BlastAt = 25f;
    public const float BlastEndsAt = 28f;
    public const float HealAt = 29f;
    public const float HalfWidth = 0.45f;
    public const float HalfDepth = 0.45f;
    public const float MinimumMove = 0.7f;
    public const float Midline = 0.275f;
    private const float Height = 2.15f;
    private static readonly Color Yellow = new Color(1f, 0.89f, 0.38f, 1f);

    // Sample only the authored play footprint; reject cover and both players' starting footprints.
    public static bool SelectZones(Vector3[] heads, System.Func<Vector2, bool> clear, int seed,
        out Vector2 first, out Vector2 second)
    {
        var random = new System.Random(seed);
        var candidates = new System.Collections.Generic.List<Vector2>();
        for (float x = -2f; x <= 2.5f; x += 0.2f)
            for (float z = -1.4f; z <= 2.9f; z += 0.2f)
            {
                var point = new Vector2(x, z);
                bool away = heads.Length > 0;
                foreach (var head in heads)
                    if (DistanceToZone(point, head) < MinimumMove) away = false;
                if (away && clear(point)) candidates.Add(point);
            }
        first = second = default;
        float best = float.NegativeInfinity;
        for (int i = 0; i < candidates.Count; i++)
            for (int j = i + 1; j < candidates.Count; j++)
            {
                // Keep each complete refuge in its own half, not merely its centre.
                if (candidates[i].x + HalfWidth > Midline || candidates[j].x - HalfWidth < Midline) continue;
                if (Vector2.Distance(candidates[i], candidates[j]) < 1.25f) continue;
                float score = 0f;
                bool reachable = true;
                foreach (var head in heads)
                {
                    float distance = DistanceToZone(head.x < Midline ? candidates[i] : candidates[j], head);
                    if (distance > 2.5f) reachable = false;
                    score += 1.5f - Mathf.Abs(distance - 1.1f);
                }
                score += (float)random.NextDouble() * 0.6f;
                if (reachable && score > best) { best = score; first = candidates[i]; second = candidates[j]; }
            }
        return !float.IsNegativeInfinity(best);
    }

    // Test the actual collision geometry, not broad world AABBs that fill mesh openings.
    // Start above the floor so thin deck meshes do not count as standing-space obstacles.
    public static bool HasClearance(Vector3 center, Quaternion rotation)
        => !Physics.CheckBox(center, new Vector3(0.56f, 1.025f, 0.56f), rotation,
            LayerMask.GetMask(CombatLayers.GameplayCover), QueryTriggerInteraction.Ignore);

    public static float DistanceToZone(Vector2 center, Vector3 head)
        => new Vector2(Mathf.Max(0f, Mathf.Abs(head.x - center.x) - HalfWidth),
            Mathf.Max(0f, Mathf.Abs(head.z - center.y) - HalfDepth)).magnitude;

    private sealed class Zone
    {
        public Transform root;
        public LineRenderer[] edges;
        public Renderer floor;
    }

    private readonly Zone[] zones = new Zone[2];
    private Material edgeMaterial, wallMaterial, smokeMaterial;
    private ParticleSystem smoke;
    private LineRenderer blastRing;
    private RectTransform blastVeil;
    private RawImage veil;
    private Texture2D veilTexture;
    private readonly System.Collections.Generic.List<GameObject> explosions = new System.Collections.Generic.List<GameObject>();
    private AudioSource hazardAudio;
    private bool blastShown;
    private bool wasActive;

    public static bool IsExploding(float elapsed) => elapsed >= BlastAt && elapsed < BlastEndsAt;
    public static bool Contains(Vector2 first, Vector2 second, Vector3 head)
        => DistanceToZone(first, head) <= 0f || DistanceToZone(second, head) <= 0f;

    private void Awake()
    {
        edgeMaterial = CombatVfxStyle.CreateMaterial("Safe zone edges", Color.white);
        wallMaterial = CombatVfxStyle.CreateMaterial("Safe zone walls", new Color(1f, 0.88f, 0.4f, 0.08f));
        for (int i = 0; i < 2; i++) zones[i] = BuildZone(i);
        blastRing = CombatVfxStyle.CreateLine(transform, "Blast front", edgeMaterial, true, 0.13f);
        BuildSmoke();
        BuildBlastVeil();
        hazardAudio = gameObject.AddComponent<AudioSource>();
        hazardAudio.playOnAwake = false;
        hazardAudio.spatialBlend = 0f;
        hazardAudio.dopplerLevel = 0f;
    }

    private Zone BuildZone(int index)
    {
        var zone = new Zone();
        zone.root = new GameObject("Safe zone " + (index + 1)).transform;
        zone.root.SetParent(transform, false);
        zone.edges = new LineRenderer[12];
        Vector3[] corners =
        {
            new Vector3(-HalfWidth, 0f, -HalfDepth), new Vector3(HalfWidth, 0f, -HalfDepth),
            new Vector3(HalfWidth, 0f, HalfDepth), new Vector3(-HalfWidth, 0f, HalfDepth)
        };
        for (int edge = 0; edge < 12; edge++)
        {
            var line = CombatVfxStyle.CreateLine(zone.root, "Glowing edge " + edge, edgeMaterial, false, 0.035f);
            line.positionCount = 2;
            Vector3 a = corners[edge < 8 ? edge % 4 : edge - 8];
            Vector3 b = edge < 8 ? corners[(edge + 1) % 4] : a + Vector3.up * Height;
            if (edge >= 4 && edge < 8) { a += Vector3.up * Height; b += Vector3.up * Height; }
            line.SetPosition(0, a);
            line.SetPosition(1, b);
            line.enabled = true;
            zone.edges[edge] = line;
        }
        zone.floor = Panel(zone.root, "Glowing floor", new Vector3(0f, 0.012f, 0f),
            new Vector3(HalfWidth * 2f, 0.015f, HalfDepth * 2f));
        Panel(zone.root, "Left glow", new Vector3(-HalfWidth, Height * 0.5f, 0f),
            new Vector3(0.012f, Height, HalfDepth * 2f));
        Panel(zone.root, "Right glow", new Vector3(HalfWidth, Height * 0.5f, 0f),
            new Vector3(0.012f, Height, HalfDepth * 2f));
        zone.root.gameObject.SetActive(false);
        return zone;
    }

    private Renderer Panel(Transform parent, string name, Vector3 position, Vector3 scale)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = position;
        go.transform.localScale = scale;
        go.AddComponent<MeshFilter>().sharedMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        var renderer = go.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = wallMaterial;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        return renderer;
    }

    private void BuildSmoke()
    {
        var go = new GameObject("Arena blast smoke");
        go.transform.SetParent(transform, false);
        smoke = go.AddComponent<ParticleSystem>();
        var main = smoke.main;
        main.playOnAwake = false;
        main.loop = false;
        main.duration = 2f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(1.2f, 2.2f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.5f, 1.6f);
        main.startSize = new ParticleSystem.MinMaxCurve(1.8f, 2.8f);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(0.8f, 0.72f, 0.55f, 0.9f),
            new Color(0.4f, 0.43f, 0.45f, 0.85f));
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 340;
        var color = smoke.colorOverLifetime;
        color.enabled = true;
        var fade = new Gradient();
        fade.SetKeys(new[] { new GradientColorKey(new Color(1f, 0.6f, 0.2f), 0f),
            new GradientColorKey(Color.white, 0.25f) },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0.9f, 0.4f), new GradientAlphaKey(0f, 1f) });
        color.color = fade;
        var emission = smoke.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 170) });
        var shape = smoke.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(5.2f, 1.8f, 5.2f);
        smokeMaterial = CombatVfxStyle.CreateMaterial("Blast smoke", Color.white);
        smokeMaterial.mainTexture = HudSprites.Dot().texture;
        var renderer = go.GetComponent<ParticleSystemRenderer>();
        renderer.sharedMaterial = smokeMaterial;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        smoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void BuildBlastVeil()
    {
        // Close smoke fills both eyes briefly; world-space fire and plumes provide depth around it.
        blastVeil = HudKit.Canvas("Blast smoke immersion", transform, new Vector2(1800f, 1800f), 90);
        blastVeil.localScale = Vector3.one * 0.001f;
        var panel = new GameObject("Rolling smoke", typeof(RectTransform), typeof(RawImage));
        panel.transform.SetParent(blastVeil, false);
        veil = panel.GetComponent<RawImage>();
        veil.rectTransform.sizeDelta = new Vector2(1800f, 1800f);
        veil.raycastTarget = false;
        veilTexture = new Texture2D(128, 128, TextureFormat.RGB24, false);
        var pixels = new Color[128 * 128];
        for (int y = 0; y < 128; y++) for (int x = 0; x < 128; x++)
        {
            float noise = Mathf.PerlinNoise(x * 0.04f, y * 0.04f) * 0.65f
                + Mathf.PerlinNoise(x * 0.12f, y * 0.12f) * 0.35f;
            pixels[y * 128 + x] = Color.Lerp(new Color(0.14f, 0.15f, 0.16f), new Color(0.72f, 0.73f, 0.74f), noise);
        }
        veilTexture.SetPixels(pixels); veilTexture.Apply();
        veil.texture = veilTexture;
        blastVeil.gameObject.SetActive(false);
    }

    private void ShowBlastVeil(float age)
    {
        var eye = Camera.main;
        if (eye == null) return;
        blastVeil.gameObject.SetActive(age >= 0f && age < 2.8f);
        blastVeil.SetPositionAndRotation(eye.transform.position + eye.transform.forward * 0.45f, eye.transform.rotation);
        float alpha = age < 0.12f ? Mathf.Clamp01(age / 0.12f) : 1f - Mathf.SmoothStep(0f, 1f, (age - 0.9f) / 1.7f);
        veil.color = new Color(1f, 1f, 1f, alpha);
        veil.uvRect = new Rect(age * 0.06f, age * 0.08f, 1f + age * 0.08f, 1f + age * 0.08f);
    }

    private void MushroomPlume(Vector3 position)
    {
        // A stem and widening crown, emitted into the existing bounded smoke system.
        for (int i = 0; i < 18; i++)
        {
            float height = i / 17f * 2.6f;
            float angle = i * 2.4f;
            float radius = height < 1.5f ? 0.16f : 0.65f + (height - 1.5f) * 0.6f;
            var particle = new ParticleSystem.EmitParams {
                position = position + new Vector3(Mathf.Cos(angle) * radius, height, Mathf.Sin(angle) * radius),
                velocity = new Vector3(Mathf.Cos(angle) * 0.2f, 0.35f, Mathf.Sin(angle) * 0.2f),
                startLifetime = 2.6f, startSize = height < 1.5f ? 0.65f : 1.35f,
                startColor = new Color(0.45f, 0.38f, 0.3f, 0.95f) };
            smoke.Emit(particle, 1);
        }
    }

    public void Show(FusionRoundDirector round)
    {
        float elapsed = round.FightElapsed;
        bool active = round.IsFighting && round.SafeZonesReady && elapsed >= WarningAt && elapsed < BlastEndsAt;
        if (!active)
        {
            if (wasActive)
            {
                blastVeil.gameObject.SetActive(false);
                hazardAudio.Stop();
                foreach (var explosion in explosions) if (explosion != null) Destroy(explosion);
                explosions.Clear();
                foreach (Zone zone in zones) zone.root.gameObject.SetActive(false);
                smoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                blastRing.enabled = false;
                blastShown = false;
            }
            wasActive = false;
            return;
        }
        if (!wasActive && elapsed < BlastAt)
        {
            hazardAudio.clip = SynthAudio.BlastAlarm();
            hazardAudio.loop = true;
            hazardAudio.volume = 0.16f;
            hazardAudio.Play();
        }
        wasActive = true;
        float pulse = 0.5f + 0.5f * Mathf.Sin(Time.time * 7f);
        for (int i = 0; i < 2; i++)
        {
            Zone zone = zones[i];
            zone.root.gameObject.SetActive(true);
            Vector2 center = i == 0 ? round.SafeZoneA : round.SafeZoneB;
            zone.root.position = NetworkPlayerAlignment.TransformPoint(new Vector3(center.x, 0f, center.y));
            zone.root.rotation = NetworkPlayerAlignment.TransformRotation(Quaternion.identity);
            Color edgeColor = CombatVfxStyle.WithAlpha(Yellow, 0.55f + 0.4f * pulse);
            foreach (LineRenderer edge in zone.edges) edge.startColor = edge.endColor = edgeColor;
        }
        if (elapsed >= BlastAt && !blastShown)
        {
            blastShown = true;
            hazardAudio.Stop();
            hazardAudio.clip = SynthAudio.BlastRinging();
            hazardAudio.loop = false;
            hazardAudio.volume = 0.25f;
            hazardAudio.time = Mathf.Clamp(elapsed - BlastAt, 0f, hazardAudio.clip.length - 0.01f);
            hazardAudio.Play();
            Vector3 center = NetworkPlayerAlignment.TransformPoint(new Vector3(0.275f, 0f, 0.71f));
            smoke.transform.position = center + Vector3.up * 1f;
            smoke.Play();
            var fx = ThermalFxLibrary.Instance;
            for (int x = 0; x < 3; x++) for (int z = 0; z < 3; z++)
            {
                Vector3 point = NetworkPlayerAlignment.TransformPoint(new Vector3(-1.8f + x * 2f, 0f, -1.25f + z * 2f));
                var explosion = ThermalFxLibrary.Spawn(fx?.bigExplosion, point, 1.65f, 3f);
                explosions.Add(explosion);
                var fire = ThermalFxLibrary.Spawn(fx?.fireMedium, point + Vector3.up * 0.35f, 1.25f, 2.6f);
                explosions.Add(fire);
                if (fire != null) LimitBlastEffect(fire);
                if (explosion != null)
                {
                    LimitBlastEffect(explosion);
                }
                MushroomPlume(point);
            }
            SynthAudio.Play2D(SynthAudio.Explosion(), 0.6f, 0.7f);
        }
        if (blastShown) ShowBlastVeil(elapsed - BlastAt);
        if (blastShown && elapsed < BlastAt + 1.3f)
        {
            Vector3 center = NetworkPlayerAlignment.TransformPoint(new Vector3(0.275f, 0.08f, 0.71f));
            float age = elapsed - BlastAt;
            CombatVfxStyle.SetRing(blastRing, center, Quaternion.Euler(90f, 0f, 0f), 0.3f + 3.5f * age, 64);
            blastRing.startColor = blastRing.endColor = CombatVfxStyle.WithAlpha(Yellow, 1f - age / 1.3f);
        }
        else blastRing.enabled = false;
    }

    private static void LimitBlastEffect(GameObject effect)
    {
        foreach (var system in effect.GetComponentsInChildren<ParticleSystem>())
        {
            var main = system.main;
            main.maxParticles = Mathf.Min(main.maxParticles, 24);
        }
        foreach (var source in effect.GetComponentsInChildren<AudioSource>()) { source.Stop(); source.enabled = false; }
        foreach (var light in effect.GetComponentsInChildren<Light>()) light.enabled = false;
    }

    private void OnDisable()
    {
        if (hazardAudio != null) hazardAudio.Stop();
    }

    private void OnDestroy()
    {
        foreach (var explosion in explosions) if (explosion != null) Destroy(explosion);
        if (veilTexture != null) Destroy(veilTexture);
        if (edgeMaterial != null) Destroy(edgeMaterial);
        if (wallMaterial != null) Destroy(wallMaterial);
        if (smokeMaterial != null) Destroy(smokeMaterial);
    }
}
