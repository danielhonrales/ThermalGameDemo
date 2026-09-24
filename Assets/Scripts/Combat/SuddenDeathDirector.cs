using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Client-side sudden-death presentation, driven entirely by the shared match clock.
/// The arena turns hellish: red passthrough grade, a charred metal floor with glowing seams
/// erupting underfoot, a scorched red roof, perimeter fires, rolling ground smoke, falling
/// sparks and embers — while drones rip out the old cover and drop new, scattered cover.
/// The real room and the opponent stay visible throughout.
/// </summary>
[DisallowMultipleComponent]
public sealed class SuddenDeathDirector : MonoBehaviour
{
    private const float WarningLead = 5f;
    private const float PickupStart = -1.3f, PickupStagger = 0.12f;
    private const float DeliveryStart = 0.8f, DeliveryStagger = 0.13f;
    private const float MirrorX = 0.275f;

    private static readonly Color Red = new Color(1f, 0.1f, 0.06f, 1f);
    private static readonly Color Ember = new Color(1f, 0.32f, 0.08f, 1f);

    private enum CoverKind { Concrete, Metal, BurningCrate }
    private struct Slot { public Vector3 position; public float yaw; public CoverKind kind; }

    // Scattered sudden-death cover, mirror-symmetric about the plane between the start pads.
    private static readonly Slot[] Layout =
    {
        new Slot { position = new Vector3(MirrorX, 0f, 0.3f), yaw = 0f, kind = CoverKind.Concrete },
        new Slot { position = new Vector3(-0.55f, 0f, 1.55f), yaw = 25f, kind = CoverKind.Metal },
        new Slot { position = new Vector3(1.1f, 0f, 1.55f), yaw = -25f, kind = CoverKind.Metal },
        new Slot { position = new Vector3(-0.85f, 0f, -1.15f), yaw = -30f, kind = CoverKind.Concrete },
        new Slot { position = new Vector3(1.4f, 0f, -1.15f), yaw = 30f, kind = CoverKind.Concrete },
        new Slot { position = new Vector3(MirrorX, 0f, -2.55f), yaw = 90f, kind = CoverKind.BurningCrate },
        new Slot { position = new Vector3(-1.65f, 0f, -2.45f), yaw = 60f, kind = CoverKind.Concrete },
        new Slot { position = new Vector3(2.2f, 0f, -2.45f), yaw = -60f, kind = CoverKind.Concrete },
        new Slot { position = new Vector3(-1.55f, 0f, 1.75f), yaw = 10f, kind = CoverKind.BurningCrate },
        new Slot { position = new Vector3(2.1f, 0f, 1.75f), yaw = -10f, kind = CoverKind.BurningCrate },
    };

    private sealed class Cluster
    {
        public Transform carrier;
        public readonly List<Transform> items = new List<Transform>();
        public readonly List<Transform> parents = new List<Transform>();
        public readonly List<Vector3> positions = new List<Vector3>();
        public readonly List<Quaternion> rotations = new List<Quaternion>();
        public readonly List<Vector3> scales = new List<Vector3>();
    }

    private readonly List<Cluster> clusters = new List<Cluster>();
    private readonly List<CoverDrone> drones = new List<CoverDrone>();
    private readonly List<GameObject> newCover = new List<GameObject>();
    private readonly List<Renderer> coverStrips = new List<Renderer>();
    private readonly List<float> coverLandedAt = new List<float>();
    private readonly List<GameObject> coverFires = new List<GameObject>();
    private Transform gameplayRoot;
    private float floorY;
    private Vector3 centre;
    private Bounds arenaBounds;
    private bool built, swapping, erupted;
    private int seenDownMask;
    private float intensity, eruptedAt = -10f;

    // Atmosphere.
    private OVRPassthroughLayer passthrough;
    private OVRPassthroughColorLut redLut;
    private float appliedLutWeight = -1f;
    private Light sun;
    private Color sunColor;
    private float sunIntensity;
    private Color ambient;
    private readonly List<Renderer> gradedRenderers = new List<Renderer>();
    private readonly List<Color> gradedEmission = new List<Color>();
    private MaterialPropertyBlock block;
    private GameObject hellFloor;
    private Renderer hellFloorRenderer;
    private LineRenderer eruptionRing;
    private readonly List<LineRenderer> beacons = new List<LineRenderer>();
    private readonly List<Vector3> beaconPositions = new List<Vector3>();
    private readonly List<GameObject> hellFx = new List<GameObject>();
    private readonly List<Vector3> firePoints = new List<Vector3>();
    private readonly List<Vector3> smokePoints = new List<Vector3>();
    private readonly List<Vector3> sparkPoints = new List<Vector3>();
    private ParticleSystem embers, motes;
    private LineRenderer boundary;
    private Material lineMaterial, emberMaterial;
    private AudioSource siren, heartbeat, fireLoop;

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private static readonly int EmissionId = Shader.PropertyToID("_EmissionColor");

    public static SuddenDeathDirector Ensure()
    {
        var existing = FindFirstObjectByType<SuddenDeathDirector>();
        return existing != null ? existing : new GameObject("Sudden Death").AddComponent<SuddenDeathDirector>();
    }

    // ---------------- Setup ----------------

    private void Build()
    {
        if (built) return;
        built = true;
        block = new MaterialPropertyBlock();
        lineMaterial = CombatVfxStyle.CreateMaterial("Sudden death lines", Color.white);
        Transform arena = GameObject.Find("ArenaRoot")?.transform;
        gameplayRoot = GameObject.Find("ArenaRoot/GameplayRoot")?.transform;
        Transform cover = GameObject.Find("ArenaRoot/GameplayRoot/GameplayCover")?.transform;
        Transform ceiling = GameObject.Find("ArenaRoot/Environment/VirtualCeiloingAlwaysVisible")?.transform;

        // Calibration places the floor at the arena root height.
        floorY = arena != null ? arena.position.y : 0f;
        centre = new Vector3(MirrorX, floorY, -0.4f);
        arenaBounds = new Bounds(centre, new Vector3(6.2f, 0.1f, 6.6f));

        // Legacy built-in particle shaders don't render in URP (they show as white domes).
        if (arena != null)
            foreach (ParticleSystemRenderer legacy in arena.GetComponentsInChildren<ParticleSystemRenderer>(true))
                if (legacy.sharedMaterial != null && legacy.sharedMaterial.shader.name.StartsWith("Legacy"))
                    legacy.enabled = false;

        // Existing arena art is graded (tint + emission) rather than relit.
        if (ceiling != null) AddGraded(ceiling);
        if (cover != null) AddGraded(cover);

        BuildClusters(cover);
        BuildFloorCollider();
        BuildNewCover();
        BuildAtmosphere();

        foreach (Light light in FindObjectsByType<Light>(FindObjectsSortMode.None))
            if (light.type == LightType.Directional) { sun = light; break; }
        if (sun != null) { sunColor = sun.color; sunIntensity = sun.intensity; }
        ambient = RenderSettings.ambientLight;
        passthrough = FindFirstObjectByType<OVRPassthroughLayer>();
    }

    private void AddGraded(Transform root)
    {
        foreach (Renderer r in root.GetComponentsInChildren<Renderer>(true))
        {
            if (r is ParticleSystemRenderer || r is LineRenderer) continue;
            Material m = r.sharedMaterial;
            gradedRenderers.Add(r);
            gradedEmission.Add(m != null && m.HasProperty(EmissionId) ? m.GetColor(EmissionId) : Color.black);
        }
    }

    private void BuildClusters(Transform cover)
    {
        if (cover == null) return;
        // Each authored obstacle is one drone load; legacy loose crates are grouped by footprint.
        var loose = new List<Transform>();
        foreach (Transform child in cover)
        {
            if (child.name.StartsWith("Obstacle")) AddCluster(new List<Transform> { child });
            else if (child.name.StartsWith("Crate")) loose.Add(child);
        }
        var assigned = new HashSet<Transform>();
        foreach (Transform crate in loose)
        {
            if (assigned.Contains(crate)) continue;
            var group = new List<Transform>();
            var queue = new Queue<Transform>();
            queue.Enqueue(crate);
            assigned.Add(crate);
            while (queue.Count > 0)
            {
                Transform item = queue.Dequeue();
                group.Add(item);
                foreach (Transform other in loose)
                    if (!assigned.Contains(other) && Flat(other.position - item.position).magnitude < 0.75f)
                    {
                        assigned.Add(other);
                        queue.Enqueue(other);
                    }
            }
            AddCluster(group);
        }
    }

    private void AddCluster(List<Transform> items)
    {
        var cluster = new Cluster();
        Vector3 sum = Vector3.zero;
        foreach (Transform item in items)
        {
            cluster.items.Add(item);
            cluster.parents.Add(item.parent);
            cluster.positions.Add(item.localPosition);
            cluster.rotations.Add(item.localRotation);
            cluster.scales.Add(item.localScale);
            sum += item.position;
        }
        cluster.carrier = new GameObject("Cover cluster carrier").transform;
        cluster.carrier.SetParent(transform, false);
        cluster.carrier.position = sum / items.Count;
        clusters.Add(cluster);
    }

    private void BuildFloorCollider()
    {
        var floor = new GameObject("Sudden death floor collider");
        floor.transform.SetParent(transform, false);
        floor.transform.position = new Vector3(centre.x, floorY - 0.05f, centre.z);
        floor.AddComponent<BoxCollider>().size = new Vector3(16f, 0.1f, 16f);
    }

    private void BuildNewCover()
    {
        ThermalFxLibrary fx = ThermalFxLibrary.Instance;
        for (int i = 0; i < Layout.Length; i++)
        {
            Slot slot = Layout[i];
            GameObject piece = CreateCoverPiece(slot.kind, fx);
            piece.name = "Sudden death cover " + i;
            piece.transform.SetParent(gameplayRoot != null ? gameplayRoot : transform, true);

            // Measure upright, then face the slot yaw and sit on the floor.
            piece.transform.rotation = Quaternion.identity;
            Bounds upright = RenderBounds(piece);
            Vector3 topLocal = piece.transform.InverseTransformPoint(new Vector3(upright.center.x, upright.max.y, upright.center.z));
            bool alongZ = upright.size.z >= upright.size.x;
            float length = Mathf.Max(upright.size.x, upright.size.z) * 0.9f;
            piece.transform.rotation = Quaternion.Euler(0f, slot.yaw, 0f);
            Bounds b = RenderBounds(piece);
            Vector3 target = new Vector3(slot.position.x, floorY, slot.position.z);
            piece.transform.position += target - new Vector3(b.center.x, b.min.y, b.center.z);
            b = RenderBounds(piece);
            if (piece.GetComponentInChildren<Collider>() == null)
            {
                var box = piece.AddComponent<BoxCollider>();
                box.center = piece.transform.InverseTransformPoint(b.center);
                Vector3 local = Quaternion.Inverse(piece.transform.rotation) * b.size;
                Vector3 lossy = piece.transform.lossyScale;
                box.size = new Vector3(Mathf.Abs(local.x) / lossy.x, Mathf.Abs(local.y) / lossy.y, Mathf.Abs(local.z) / lossy.z);
            }

            // Thin neon crown strip marks new cover once it lands.
            GameObject strip = GameObject.CreatePrimitive(PrimitiveType.Cube);
            strip.name = "Crown light";
            Destroy(strip.GetComponent<Collider>());
            strip.transform.SetParent(piece.transform, false);
            strip.transform.localPosition = topLocal;
            Vector3 size = alongZ ? new Vector3(0.03f, 0.015f, length) : new Vector3(length, 0.015f, 0.03f);
            Vector3 scale = piece.transform.lossyScale;
            strip.transform.localScale = new Vector3(size.x / scale.x, size.y / scale.y, size.z / scale.z);
            var stripRenderer = strip.GetComponent<Renderer>();
            if (fx != null && fx.coverEmissive != null) stripRenderer.sharedMaterial = fx.coverEmissive;
            stripRenderer.shadowCastingMode = ShadowCastingMode.Off;
            coverStrips.Add(stripRenderer);
            coverLandedAt.Add(-1f);
            coverFires.Add(null);

            piece.AddComponent<GameplayCoverMarker>();
            CombatLayers.SetLayerRecursively(piece, CombatLayers.GameplayCoverLayer);
            piece.SetActive(false);
            newCover.Add(piece);
        }
    }

    private static GameObject CreateCoverPiece(CoverKind kind, ThermalFxLibrary fx)
    {
        string path = kind == CoverKind.Metal ? "ThermalFX/Cover/Metal_Barrier_1"
            : kind == CoverKind.Concrete ? "ThermalFX/Cover/Concrete_Barrier_2" : "ThermalFX/Cover/Crate";
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject piece = prefab != null ? Instantiate(prefab) : GameObject.CreatePrimitive(PrimitiveType.Cube);
        if (kind != CoverKind.BurningCrate && fx != null && fx.barrier != null)
            foreach (Renderer r in piece.GetComponentsInChildren<Renderer>()) r.sharedMaterial = fx.barrier;
        // Concrete stays crouch-high (~1 m) so it never reads as a wall.
        if (kind == CoverKind.Concrete) piece.transform.localScale *= 0.78f;
        if (kind == CoverKind.BurningCrate) piece.transform.localScale *= 0.5f;
        return piece;
    }

    private static Bounds RenderBounds(GameObject go)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(go.transform.position, Vector3.one * 0.5f);
        Bounds b = renderers[0].bounds;
        foreach (Renderer r in renderers) if (!(r is ParticleSystemRenderer)) b.Encapsulate(r.bounds);
        return b;
    }

    private static Vector3 Flat(Vector3 v) => new Vector3(v.x, 0f, v.z);

    private void BuildAtmosphere()
    {
        ThermalFxLibrary fx = ThermalFxLibrary.Instance;

        // VR floor overlay: charred sci-fi deck plating with molten seams. Erupts at sudden death.
        hellFloor = new GameObject("Hell floor");
        hellFloor.transform.SetParent(transform, false);
        hellFloor.transform.position = new Vector3(centre.x, floorY + 0.004f, centre.z);
        if (fx != null && fx.floorTileMesh != null)
        {
            hellFloor.AddComponent<MeshFilter>().sharedMesh = fx.floorTileMesh;
            hellFloorRenderer = hellFloor.AddComponent<MeshRenderer>();
            hellFloorRenderer.sharedMaterial = fx.hellFloor;
            hellFloorRenderer.shadowCastingMode = ShadowCastingMode.Off;
            Bounds mb = fx.floorTileMesh.bounds;
            // The kit tile lies in its XY plane facing +Z with a corner pivot; lay it face-up.
            bool flatXY = mb.size.z < mb.size.y;
            hellFloor.transform.rotation = flatXY ? Quaternion.Euler(-90f, 0f, 0f) : Quaternion.identity;
        }
        hellFloor.SetActive(false);
        eruptionRing = CombatVfxStyle.CreateLine(transform, "Eruption ring", lineMaterial, true, 0.12f);

        float minX = arenaBounds.min.x, maxX = arenaBounds.max.x, minZ = arenaBounds.min.z, maxZ = arenaBounds.max.z;
        // Fires line the arena edge (outside the play lanes), smoke rolls in from the sides.
        firePoints.AddRange(new[]
        {
            new Vector3(minX, floorY, minZ), new Vector3(maxX, floorY, minZ), new Vector3(maxX, floorY, maxZ), new Vector3(minX, floorY, maxZ),
            new Vector3(centre.x, floorY, minZ - 0.1f), new Vector3(centre.x, floorY, maxZ + 0.1f),
        });
        smokePoints.AddRange(new[]
        {
            new Vector3(minX + 0.4f, floorY, centre.z), new Vector3(maxX - 0.4f, floorY, centre.z),
            new Vector3(centre.x, floorY, minZ + 0.4f), new Vector3(centre.x, floorY, maxZ - 0.4f),
        });
        float ceilingY = floorY + 2.6f;
        sparkPoints.AddRange(new[]
        {
            new Vector3(minX + 1.1f, ceilingY, minZ + 1.2f), new Vector3(maxX - 1.1f, ceilingY, maxZ - 1.2f),
            new Vector3(maxX - 1.3f, ceilingY, minZ + 1.6f), new Vector3(minX + 1.3f, ceilingY, maxZ - 1.6f),
        });

        // Rotating siren beacons hang from the virtual ceiling corners.
        foreach (Vector3 corner in new[]
        {
            new Vector3(minX + 0.5f, ceilingY, minZ + 0.5f), new Vector3(maxX - 0.5f, ceilingY, minZ + 0.5f),
            new Vector3(maxX - 0.5f, ceilingY, maxZ - 0.5f), new Vector3(minX + 0.5f, ceilingY, maxZ - 0.5f)
        })
        {
            LineRenderer sweep = CombatVfxStyle.CreateLine(transform, "Beacon sweep", lineMaterial, true, 0.7f);
            sweep.positionCount = 2;
            sweep.widthCurve = new AnimationCurve(new Keyframe(0f, 0.04f), new Keyframe(1f, 1f));
            beacons.Add(sweep);
            beaconPositions.Add(corner);
        }

        // Dense rising embers and ash across the whole arena.
        var emberObject = new GameObject("Embers");
        emberObject.transform.SetParent(transform, false);
        emberObject.transform.position = new Vector3(centre.x, floorY + 0.1f, centre.z);
        embers = emberObject.AddComponent<ParticleSystem>();
        embers.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var main = embers.main;
        main.loop = true;
        main.startLifetime = new ParticleSystem.MinMaxCurve(3f, 7f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.1f, 0.45f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.006f, 0.03f);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 0.55f, 0.15f, 1f), new Color(1f, 0.12f, 0.04f, 1f));
        main.maxParticles = 500;
        main.gravityModifier = -0.03f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        var emission = embers.emission;
        emission.rateOverTime = 0f;
        var shape = embers.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(arenaBounds.size.x, 0.2f, arenaBounds.size.z);
        var noise = embers.noise;
        noise.enabled = true;
        noise.strength = 0.35f;
        noise.frequency = 0.7f;
        var colour = embers.colorOverLifetime;
        colour.enabled = true;
        var fade = new Gradient();
        fade.SetKeys(new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(new Color(1f, 0.3f, 0.2f), 1f) },
            new[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(1f, 0.15f), new GradientAlphaKey(0f, 1f) });
        colour.color = fade;
        var emberRenderer = emberObject.GetComponent<ParticleSystemRenderer>();
        emberMaterial = CombatVfxStyle.CreateMaterial("Embers", Color.white);
        emberMaterial.mainTexture = HudSprites.Dot().texture;
        emberRenderer.sharedMaterial = emberMaterial;
        emberRenderer.renderMode = ParticleSystemRenderMode.Stretch;
        emberRenderer.velocityScale = 0.08f;
        emberRenderer.shadowCastingMode = ShadowCastingMode.Off;

        // Minute-one ambience: cool drifting motes and a faint holographic arena boundary.
        motes = Object.Instantiate(embers, embers.transform.parent);
        motes.name = "Calm motes";
        var moteMain = motes.main;
        moteMain.startColor = new ParticleSystem.MinMaxGradient(new Color(0.4f, 0.9f, 1f, 0.5f), new Color(0.7f, 0.95f, 1f, 0.25f));
        moteMain.startSpeed = new ParticleSystem.MinMaxCurve(0.01f, 0.05f);
        moteMain.startSize = new ParticleSystem.MinMaxCurve(0.004f, 0.012f);
        moteMain.gravityModifier = -0.002f;
        var moteShape = motes.shape;
        moteShape.scale = new Vector3(arenaBounds.size.x, 2.2f, arenaBounds.size.z);
        moteShape.position = new Vector3(0f, 1.1f, 0f);
        motes.GetComponent<ParticleSystemRenderer>().renderMode = ParticleSystemRenderMode.Billboard;
        var moteEmission = motes.emission;
        moteEmission.rateOverTime = 14f;
        motes.Play();
        boundary = CombatVfxStyle.CreateLine(transform, "Arena boundary", lineMaterial, true, 0.012f);
        boundary.positionCount = 5;
        boundary.SetPositions(new[]
        {
            new Vector3(minX, floorY + 0.01f, minZ), new Vector3(maxX, floorY + 0.01f, minZ),
            new Vector3(maxX, floorY + 0.01f, maxZ), new Vector3(minX, floorY + 0.01f, maxZ),
            new Vector3(minX, floorY + 0.01f, minZ)
        });

        siren = Loop(SynthAudio.Siren());
        heartbeat = Loop(SynthAudio.Heartbeat());
        fireLoop = Loop(SynthAudio.FireRoar());
    }

    private AudioSource Loop(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.spatialBlend = 0f;
        source.volume = 0f;
        return source;
    }

    /// <summary>Thin, low-lying smoke that drifts across the floor without hiding the room.</summary>
    private GameObject GroundHaze(Vector3 position)
    {
        var go = new GameObject("Ground haze");
        go.transform.SetParent(transform, false);
        go.transform.position = position + Vector3.up * 0.05f;
        var system = go.AddComponent<ParticleSystem>();
        system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var main = system.main;
        main.loop = true;
        main.startLifetime = new ParticleSystem.MinMaxCurve(5f, 8f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.05f, 0.2f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.8f, 1.6f);
        main.startRotation = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(0.35f, 0.1f, 0.08f, 0.09f), new Color(0.18f, 0.08f, 0.07f, 0.14f));
        main.maxParticles = 40;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        var emission = system.emission;
        emission.rateOverTime = 6f;
        var shape = system.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(1.6f, 0.05f, 1.6f);
        var velocity = system.velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.World;
        Vector3 inward = Flat(centre - position).normalized * 0.12f;
        velocity.x = new ParticleSystem.MinMaxCurve(inward.x);
        velocity.y = new ParticleSystem.MinMaxCurve(0.01f);
        velocity.z = new ParticleSystem.MinMaxCurve(inward.z);
        var colour = system.colorOverLifetime;
        colour.enabled = true;
        var fade = new Gradient();
        fade.SetKeys(new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(1f, 0.3f), new GradientAlphaKey(0f, 1f) });
        colour.color = fade;
        var size = system.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 0.7f, 1f, 1.4f));
        var renderer = go.GetComponent<ParticleSystemRenderer>();
        renderer.sharedMaterial = emberMaterial;
        renderer.renderMode = ParticleSystemRenderMode.HorizontalBillboard;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        system.Play();
        return go;
    }

    private static GameObject SpawnLoop(GameObject prefab, Vector3 position, Quaternion rotation, float scale, Transform parent)
    {
        if (prefab == null) return null;
        GameObject fx = Instantiate(prefab, position, rotation * prefab.transform.localRotation, parent);
        fx.transform.localScale = prefab.transform.localScale * scale;
        ThermalFxLibrary.FixParticleMaterials(fx);
        foreach (ParticleSystem system in fx.GetComponentsInChildren<ParticleSystem>())
        {
            ParticleSystem.MainModule main = system.main;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            main.loop = true;
            system.Play();
        }
        return fx;
    }

    // ---------------- Per frame ----------------

    public void Show(FusionRoundDirector round)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        // While an editor preview runs, live match updates are ignored.
        if (FindAnyObjectByType<SuddenDeathPreview>() != null) return;
#endif
        Drive(round.Phase, round.SuddenDeathClock, round.DroneDownMask);
    }

    /// <summary>Advances the presentation from the shared match clock (also used by the editor preview).</summary>
    public void Drive(FusionRoundDirector.RoundPhase phase, float clock, int droneDownMask)
    {
        Build();
        bool fighting = phase == FusionRoundDirector.RoundPhase.Fighting;
        if (!fighting)
        {
            // Keep the sudden-death look through the result screen, reset for the next round.
            if (phase == FusionRoundDirector.RoundPhase.Result) { Atmosphere(intensity, 0f); return; }
            if (swapping || intensity > 0f) ResetArena();
            return;
        }

        float warning = Mathf.Clamp01((clock + WarningLead) / WarningLead);
        float target = clock >= 0f ? 1f : warning * 0.3f;
        intensity = Mathf.MoveTowards(intensity, target, Time.deltaTime * (clock >= 0f ? 3f : 0.5f));
        if (clock >= 0f && !erupted) Erupt();
        Atmosphere(intensity, clock);

        if (clock >= PickupStart - 0.1f && !swapping) BeginSwap();
        if (swapping)
        {
            if (droneDownMask != seenDownMask)
            {
                int fresh = droneDownMask & ~seenDownMask;
                seenDownMask = droneDownMask;
                foreach (CoverDrone drone in drones)
                    if (drone != null && (fresh & (1 << drone.Index)) != 0) drone.KnockDown();
            }
            foreach (CoverDrone drone in drones)
                if (drone != null) drone.Tick(clock);
        }
        UpdateCoverLights();
    }

    private void Erupt()
    {
        erupted = true;
        eruptedAt = Time.time;
        ThermalFxLibrary fx = ThermalFxLibrary.Instance;
        hellFloor.SetActive(true);
        if (fx == null) return;
        foreach (Vector3 point in firePoints)
        {
            ThermalFxLibrary.Spawn(fx.bigExplosion, point + Vector3.up * 0.2f, 0.25f, 4f);
            hellFx.Add(SpawnLoop(fx.fireLarge, point, Quaternion.identity, 0.45f, transform));
        }
        foreach (Vector3 point in smokePoints)
            hellFx.Add(GroundHaze(point));
        foreach (Vector3 point in sparkPoints)
            hellFx.Add(SpawnLoop(fx.ceilingSparks, point, Quaternion.Euler(90f, 0f, 0f), 0.45f, transform));
        SynthAudio.Play2D(SynthAudio.Explosion(), 0.8f, 0.6f);
    }

    private void BeginSwap()
    {
        swapping = true;
        seenDownMask = 0;
        drones.Clear();
        int index = 0;
        for (int i = 0; i < clusters.Count; i++)
        {
            Cluster cluster = clusters[i];
            Bounds b = ClusterBounds(cluster);
            cluster.carrier.position = new Vector3(b.center.x, b.max.y, b.center.z);
            foreach (Transform item in cluster.items) item.SetParent(cluster.carrier, true);
            Vector3 top = cluster.carrier.position;
            FlightPath(top, 0.55f, 0.16f, index, out Vector3 entry, out Vector3 hover, out Vector3 low, out Vector3 exit);
            drones.Add(CoverDrone.Create(transform, index++, CoverDrone.Job.Pickup, PickupStart + i * PickupStagger,
                entry, hover, low, exit, cluster.carrier, null));
        }
        for (int i = 0; i < newCover.Count; i++)
        {
            GameObject piece = newCover[i];
            Bounds b = RenderBounds(piece);
            Vector3 top = new Vector3(b.center.x, b.max.y, b.center.z);
            FlightPath(top, 0.6f, 0.14f, index, out Vector3 entry, out Vector3 hover, out Vector3 low, out Vector3 exit);
            int slot = i;
            var drone = CoverDrone.Create(transform, index++, CoverDrone.Job.Delivery, DeliveryStart + i * DeliveryStagger,
                entry, hover, low, exit, piece.transform, d => OnCoverLanded(slot, d));
            drone.SetDeliveryPose(piece.transform.position, piece.transform.rotation);
            piece.transform.position += Vector3.up * 6f;
            drones.Add(drone);
        }
    }

    /// <summary>Drones arrive from far outside the room along the target's outward bearing and leave on another.</summary>
    private void FlightPath(Vector3 top, float hoverHeight, float lowHeight, int index,
        out Vector3 entry, out Vector3 hover, out Vector3 low, out Vector3 exit)
    {
        Vector3 outward = Flat(top - centre);
        if (outward.sqrMagnitude < 0.01f) outward = Quaternion.Euler(0f, index * 137f, 0f) * Vector3.forward;
        outward.Normalize();
        hover = top + Vector3.up * hoverHeight;
        low = top + Vector3.up * lowHeight;
        entry = hover + Quaternion.Euler(0f, -35f, 0f) * outward * 7f + Vector3.up * 1.6f;
        exit = hover + Quaternion.Euler(0f, 55f, 0f) * outward * 8f + Vector3.up * 2.2f;
    }

    private void OnCoverLanded(int slot, CoverDrone drone)
    {
        coverLandedAt[slot] = Time.time;
        GameObject piece = newCover[slot];
        piece.SetActive(true);
        Vector3 foot = new Vector3(piece.transform.position.x, floorY, piece.transform.position.z);
        ThermalFxLibrary fx = ThermalFxLibrary.Instance;
        if (!drone.IsDown)
        {
            ThermalFxLibrary.DustBurst(foot, 1.3f);
            ThermalFxLibrary.Spawn(fx?.metalSparks, foot + Vector3.up * 0.1f, 1f, 2f);
            SynthAudio.PlayAt(SynthAudio.Clunk(), foot, 1f, 0.7f);
        }
        if (Layout[slot].kind == CoverKind.BurningCrate && fx != null)
        {
            Bounds b = RenderBounds(piece);
            coverFires[slot] = SpawnLoop(fx.fireMedium, new Vector3(b.center.x, b.max.y, b.center.z), Quaternion.identity, 0.3f, piece.transform);
        }
    }

    private static Bounds ClusterBounds(Cluster cluster)
    {
        bool any = false;
        Bounds b = new Bounds(cluster.carrier.position, Vector3.zero);
        foreach (Transform item in cluster.items)
            foreach (Renderer r in item.GetComponentsInChildren<Renderer>())
            {
                if (!any) { b = r.bounds; any = true; } else b.Encapsulate(r.bounds);
            }
        return b;
    }

    private void UpdateCoverLights()
    {
        float beat = FusionRoundHud.Current != null ? FusionRoundHud.Current.HeartbeatPulse() : 0.5f;
        for (int i = 0; i < coverStrips.Count; i++)
        {
            if (coverStrips[i] == null) continue;
            float landed = coverLandedAt[i];
            float on = landed < 0f ? 0f : Mathf.Clamp01((Time.time - landed) / 0.25f);
            float flicker = landed >= 0f && Time.time - landed < 0.5f && Random.value > 0.6f ? 0.2f : 1f;
            coverStrips[i].GetPropertyBlock(block);
            block.SetColor(EmissionId, Red * (on * flicker * (2.5f + 3f * beat)));
            coverStrips[i].SetPropertyBlock(block);
        }
    }

    private void Atmosphere(float amount, float clock)
    {
        float beat = FusionRoundHud.Current != null ? FusionRoundHud.Current.HeartbeatPulse() : 0f;
        bool active = amount > 0.001f;
        float hell = erupted ? Mathf.Clamp01((Time.time - eruptedAt) / 0.8f) * amount : 0f;

        // Real world takes a red grade that breathes with the heartbeat.
        ApplyPassthroughGrade(active ? amount * (0.6f + 0.12f * beat) : 0f);
        if (passthrough != null)
        {
            passthrough.edgeRenderingEnabled = hell > 0.3f;
            passthrough.edgeColor = new Color(1f, 0.15f, 0.05f, 0.3f * hell * (0.6f + 0.4f * beat));
        }

        if (sun != null)
        {
            sun.color = Color.Lerp(sunColor, new Color(1f, 0.28f, 0.12f), amount);
            sun.intensity = Mathf.Lerp(sunIntensity, sunIntensity * (0.7f + 0.25f * beat), amount);
        }
        RenderSettings.ambientLight = Color.Lerp(ambient, new Color(0.35f, 0.04f, 0.03f), amount);

        // Roof and existing cover scorch: darker, redder, with every light strip burning red.
        for (int i = 0; i < gradedRenderers.Count; i++)
        {
            Renderer r = gradedRenderers[i];
            if (r == null) continue;
            r.GetPropertyBlock(block);
            if (active)
            {
                block.SetColor(BaseColorId, Color.Lerp(Color.white, new Color(0.5f, 0.2f, 0.17f), amount * 0.5f + hell * 0.5f));
                Color original = gradedEmission[i];
                if (original.maxColorComponent > 0.01f)
                    block.SetColor(EmissionId, Color.Lerp(original, Red * (3f + 2.5f * beat), amount));
            }
            else block.Clear();
            r.SetPropertyBlock(block);
        }

        // Floor erupts outward from the centre with a molten shockwave ring.
        if (hellFloor != null)
        {
            hellFloor.SetActive(hell > 0f);
            if (hellFloorRenderer != null && hell > 0f)
            {
                float grow = 1f - Mathf.Pow(1f - Mathf.Clamp01((Time.time - eruptedAt) / 0.7f), 3f);
                float scale = FloorScale() * Mathf.Max(0.02f, grow);
                hellFloor.transform.localScale = Vector3.one * scale;
                // Keep the tile centred on the arena as it grows (mesh pivot is at a corner).
                Vector3 meshCentre = hellFloor.GetComponent<MeshFilter>().sharedMesh.bounds.center;
                hellFloor.transform.position = new Vector3(centre.x, floorY + 0.004f, centre.z)
                    - hellFloor.transform.rotation * (meshCentre * scale);
                hellFloorRenderer.GetPropertyBlock(block);
                float surge = Mathf.Exp(-(Time.time - eruptedAt) * 2.5f);
                // Molten cracks (colour lives in the emission texture) throb with the heartbeat.
                block.SetColor(EmissionId, Color.white * (1.1f + 1.2f * beat + 5f * surge) * hell);
                hellFloorRenderer.SetPropertyBlock(block);
            }
        }
        float ringAge = Time.time - eruptedAt;
        eruptionRing.enabled = erupted && ringAge < 1.2f;
        if (eruptionRing.enabled)
        {
            CombatVfxStyle.SetRing(eruptionRing, new Vector3(centre.x, floorY + 0.02f, centre.z), Quaternion.Euler(90f, 0f, 0f), 0.3f + ringAge * 4.5f, 96);
            eruptionRing.widthMultiplier = 0.25f * (1f - ringAge / 1.2f) + 0.02f;
            eruptionRing.startColor = eruptionRing.endColor = CombatVfxStyle.WithAlpha(Color.Lerp(Color.white, Ember, ringAge * 2f), 1f - ringAge / 1.2f);
        }

        for (int i = 0; i < beacons.Count; i++)
        {
            LineRenderer sweep = beacons[i];
            sweep.enabled = active;
            if (!active) continue;
            float angle = Time.time * 240f + i * 90f;
            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * new Vector3(0f, -0.6f, 1f).normalized;
            sweep.SetPosition(0, beaconPositions[i]);
            sweep.SetPosition(1, beaconPositions[i] + dir * 2.2f);
            sweep.startColor = CombatVfxStyle.WithAlpha(Color.Lerp(Red, Color.white, 0.3f), 0.4f * amount);
            sweep.endColor = CombatVfxStyle.WithAlpha(Red, 0f);
        }

        if (active && !embers.isPlaying) embers.Play();
        else if (!active && embers.isPlaying) embers.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        var emission = embers.emission;
        emission.rateOverTime = 25f * amount + 90f * hell;
        if (motes != null)
        {
            var calm = motes.emission;
            calm.rateOverTime = 14f * (1f - amount);
            float scan = 0.5f + 0.5f * Mathf.Sin(Time.time * 1.3f);
            boundary.enabled = amount < 0.99f;
            Color edge = Color.Lerp(new Color(0.3f, 0.85f, 1f), Red, amount);
            boundary.startColor = boundary.endColor = CombatVfxStyle.WithAlpha(edge, (0.18f + 0.12f * scan) * (1f - amount));
        }

        siren.volume = clock < 0f ? Mathf.Clamp01(amount * 2f) * 0.3f : Mathf.MoveTowards(siren.volume, 0.04f, Time.deltaTime * 0.15f);
        Toggle(siren, active);
        heartbeat.volume = clock >= 0f ? 0.5f * amount : 0f;
        heartbeat.pitch = 1.6f;
        Toggle(heartbeat, clock >= 0f && active);
        fireLoop.volume = 0.3f * hell;
        Toggle(fireLoop, hell > 0f);
    }

    private float FloorScale()
    {
        ThermalFxLibrary fx = ThermalFxLibrary.Instance;
        if (fx == null || fx.floorTileMesh == null) return 1f;
        Bounds mb = fx.floorTileMesh.bounds;
        float span = Mathf.Max(mb.size.x, Mathf.Max(mb.size.y, mb.size.z));
        return Mathf.Max(arenaBounds.size.x, arenaBounds.size.z) / span;
    }

    private static void Toggle(AudioSource source, bool on)
    {
        if (on && !source.isPlaying) source.Play();
        else if (!on && source.isPlaying) source.Stop();
    }

    private void ApplyPassthroughGrade(float weight)
    {
        if (passthrough == null) return;
        weight = Mathf.Clamp01(weight);
        if (Mathf.Abs(weight - appliedLutWeight) < 0.01f) return;
        appliedLutWeight = weight;
        if (weight <= 0.001f) { passthrough.DisableColorMap(); return; }
        if (redLut == null) redLut = BuildRedLut();
        if (redLut != null && redLut.IsValid) passthrough.SetColorLut(redLut, weight);
    }

    private static OVRPassthroughColorLut BuildRedLut()
    {
        const int res = 16;
        var colors = new Color[res * res * res];
        for (int b = 0; b < res; b++)
            for (int g = 0; g < res; g++)
                for (int r = 0; r < res; r++)
                {
                    Color c = new Color(r / (res - 1f), g / (res - 1f), b / (res - 1f));
                    float lum = c.r * 0.3f + c.g * 0.59f + c.b * 0.11f;
                    // Crushed, blood-red grade that keeps shapes and people readable.
                    Color red = new Color(Mathf.Pow(lum, 0.85f) * 1.1f + 0.03f, lum * 0.26f, lum * 0.2f);
                    colors[r + g * res + b * res * res] = Color.Lerp(c * 0.7f, red, 0.85f);
                }
        try { return new OVRPassthroughColorLut(colors, OVRPassthroughColorLut.ColorChannels.Rgb); }
        catch (System.Exception e) { Debug.LogWarning("Sudden death LUT unavailable: " + e.Message); return null; }
    }

    // ---------------- Reset ----------------

    public void ResetArena()
    {
        if (!built) return;
        foreach (CoverDrone drone in drones)
            if (drone != null) Destroy(drone.gameObject);
        drones.Clear();
        foreach (Cluster cluster in clusters)
        {
            for (int i = 0; i < cluster.items.Count; i++)
            {
                Transform item = cluster.items[i];
                if (item == null) continue;
                item.SetParent(cluster.parents[i], false);
                item.localPosition = cluster.positions[i];
                item.localRotation = cluster.rotations[i];
                item.localScale = cluster.scales[i];
                item.gameObject.SetActive(true);
            }
            cluster.carrier.gameObject.SetActive(true);
            if (cluster.carrier.TryGetComponent(out Rigidbody rb)) Destroy(rb);
            if (cluster.carrier.TryGetComponent(out CargoImpact impact)) Destroy(impact);
        }
        for (int i = 0; i < newCover.Count; i++)
        {
            GameObject piece = newCover[i];
            if (piece.TryGetComponent(out Rigidbody rb)) Destroy(rb);
            if (piece.TryGetComponent(out CargoImpact impact)) Destroy(impact);
            if (coverFires[i] != null) Destroy(coverFires[i]);
            coverFires[i] = null;
            piece.SetActive(false);
            coverLandedAt[i] = -1f;
        }
        foreach (GameObject fx in hellFx) if (fx != null) Destroy(fx);
        hellFx.Clear();
        swapping = false;
        erupted = false;
        seenDownMask = 0;
        intensity = 0f;
        Atmosphere(0f, -999f);
    }

    private void OnDestroy()
    {
        if (built) Atmosphere(0f, -999f);
        if (lineMaterial != null) Destroy(lineMaterial);
        if (emberMaterial != null) Destroy(emberMaterial);
        redLut?.Dispose();
    }
}
