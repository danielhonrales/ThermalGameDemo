using UnityEngine;
using UnityEngine.Rendering;

/// <summary>Two shared, glowing refuges appear for five seconds before an arena-wide blast.</summary>
[DisallowMultipleComponent]
public sealed class SafeZoneHazardView : MonoBehaviour
{
    public const float WarningAt = 20f;
    public const float BlastAt = 25f;
    public const float BlastEndsAt = 28f;
    public const float HealAt = 29f;
    public const int LayoutCount = 4;
    private const float HalfWidth = 0.55f;
    private const float HalfDepth = 0.55f;
    private const float Height = 2.15f;
    private static readonly Color Yellow = new Color(1f, 0.89f, 0.38f, 1f);
    private static readonly Vector2[,] Layouts =
    {
        { new Vector2(0f, -0.5f), new Vector2(0f, 2f) },
        { new Vector2(0f, -0.5f), new Vector2(1.5f, 1f) },
        { new Vector2(0f, 1.5f), new Vector2(1.5f, 1f) },
        { new Vector2(0f, 2f), new Vector2(1.5f, 1f) }
    };

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
    private bool blastShown;
    private bool wasActive;

    public static Vector2 Center(int seed, int index) => Layouts[Mathf.Abs(seed) % LayoutCount, index];
    public static bool IsExploding(float elapsed) => elapsed >= BlastAt && elapsed < BlastEndsAt;
    public static bool Contains(int seed, Vector3 canonicalHead)
    {
        for (int i = 0; i < 2; i++)
        {
            Vector2 center = Center(seed, i);
            if (Mathf.Abs(canonicalHead.x - center.x) <= HalfWidth
                && Mathf.Abs(canonicalHead.z - center.y) <= HalfDepth) return true;
        }
        return false;
    }

    private void Awake()
    {
        edgeMaterial = CombatVfxStyle.CreateMaterial("Safe zone edges", Color.white);
        wallMaterial = CombatVfxStyle.CreateMaterial("Safe zone walls", new Color(1f, 0.88f, 0.4f, 0.08f));
        for (int i = 0; i < 2; i++) zones[i] = BuildZone(i);
        blastRing = CombatVfxStyle.CreateLine(transform, "Blast front", edgeMaterial, true, 0.13f);
        BuildSmoke();
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
        main.startSize = new ParticleSystem.MinMaxCurve(0.3f, 0.85f);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(0.8f, 0.72f, 0.55f, 0.35f),
            new Color(0.4f, 0.43f, 0.45f, 0.24f));
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 120;
        var emission = smoke.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 95) });
        var shape = smoke.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(5.2f, 0.15f, 5.2f);
        smokeMaterial = CombatVfxStyle.CreateMaterial("Blast smoke", Color.white);
        smokeMaterial.mainTexture = HudSprites.Dot().texture;
        var renderer = go.GetComponent<ParticleSystemRenderer>();
        renderer.sharedMaterial = smokeMaterial;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        smoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void Show(FusionRoundDirector round)
    {
        float elapsed = round.FightElapsed;
        bool active = round.IsFighting && elapsed >= WarningAt && elapsed < BlastEndsAt;
        if (!active)
        {
            if (wasActive)
            {
                foreach (Zone zone in zones) zone.root.gameObject.SetActive(false);
                smoke.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                blastRing.enabled = false;
                blastShown = false;
            }
            wasActive = false;
            return;
        }
        wasActive = true;
        float pulse = 0.5f + 0.5f * Mathf.Sin(Time.time * 7f);
        for (int i = 0; i < 2; i++)
        {
            Zone zone = zones[i];
            zone.root.gameObject.SetActive(true);
            Vector2 center = Center(round.HazardSeed, i);
            zone.root.position = NetworkPlayerAlignment.TransformPoint(new Vector3(center.x, 0f, center.y));
            zone.root.rotation = NetworkPlayerAlignment.TransformRotation(Quaternion.identity);
            Color edgeColor = CombatVfxStyle.WithAlpha(Yellow, 0.55f + 0.4f * pulse);
            foreach (LineRenderer edge in zone.edges) edge.startColor = edge.endColor = edgeColor;
        }
        if (elapsed >= BlastAt && !blastShown)
        {
            blastShown = true;
            Vector3 center = NetworkPlayerAlignment.TransformPoint(new Vector3(0.275f, 0f, 0.71f));
            smoke.transform.position = center + Vector3.up * 0.35f;
            smoke.Play();
            SynthAudio.Play2D(SynthAudio.Explosion(), 0.6f, 0.7f);
        }
        if (blastShown && elapsed < BlastAt + 1.3f)
        {
            Vector3 center = NetworkPlayerAlignment.TransformPoint(new Vector3(0.275f, 0.08f, 0.71f));
            float age = elapsed - BlastAt;
            CombatVfxStyle.SetRing(blastRing, center, Quaternion.Euler(90f, 0f, 0f), 0.3f + 3.5f * age, 64);
            blastRing.startColor = blastRing.endColor = CombatVfxStyle.WithAlpha(Yellow, 1f - age / 1.3f);
        }
        else blastRing.enabled = false;
    }

    private void OnDestroy()
    {
        if (edgeMaterial != null) Destroy(edgeMaterial);
        if (wallMaterial != null) Destroy(wallMaterial);
        if (smokeMaterial != null) Destroy(smokeMaterial);
    }
}
