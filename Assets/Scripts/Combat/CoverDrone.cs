using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Sudden-death cargo drone. Its flight is a pure function of the shared match clock, so both
/// headsets see the same choreography: it streaks in from far outside the room, snatches or drops
/// cover, and blasts away. Shooting it (beam or ice blast) knocks it out of the sky: it sparks,
/// trails smoke and fire, and explodes; any cargo it carries falls with physics and stays as cover.
/// </summary>
[DisallowMultipleComponent]
public sealed class CoverDrone : MonoBehaviour
{
    public enum Job { Pickup, Delivery }

    // Timeline (seconds from this drone's start): fast swoop in, quick grab, fast exit.
    public const float Arrive = 0.95f, GrabEnd = 1.45f, Gone = 2.35f;
    private const float GrabMid = (Arrive + GrabEnd) * 0.5f;

    public int Index { get; private set; }
    public bool IsDown { get; private set; }
    public bool Finished { get; private set; }
    public Transform Cargo => cargo;
    public Job Kind => job;

    private Job job;
    private float startAt;
    private Vector3 entry, hover, low, exit;
    private Quaternion flightRotation;
    private Transform cargo;
    private Vector3 cargoOffset;
    private bool cargoAttached, cargoReleased;
    private System.Action<CoverDrone> onRelease;
    private Vector3 deliveryRest;
    private Quaternion deliveryRotation = Quaternion.identity;
    private Quaternion cargoRotation = Quaternion.identity;

    private Transform model;
    private Transform clawLeft, clawRight;
    private LineRenderer thruster, jetLeft, jetRight, searchlight, tractor;
    private TrailRenderer trail, smokeTrail;
    private AudioSource hum;
    private Rigidbody body;
    private Collider hitbox;
    private Material lineMaterial, smokeMaterial;
    private Vector3 lastPosition, velocity;
    private float downAt = -1f, crashedAt = -1f, lastShotFxAt = -1f;
    private float seed;
    private GameObject fireFx;
    private bool whooshedIn, whooshedOut;

    internal static readonly Color Red = new Color(1f, 0.12f, 0.08f, 1f);
    private static readonly Color Flame = new Color(1f, 0.55f, 0.2f, 1f);

    public static CoverDrone Create(Transform parent, int index, Job job, float startAt,
        Vector3 entry, Vector3 hover, Vector3 low, Vector3 exit, Transform cargo, System.Action<CoverDrone> onRelease)
    {
        var go = new GameObject($"Cover drone {index} ({job})");
        go.transform.SetParent(parent, false);
        var drone = go.AddComponent<CoverDrone>();
        drone.Index = index;
        drone.job = job;
        drone.startAt = startAt;
        drone.entry = entry;
        drone.hover = hover;
        drone.low = low;
        drone.exit = exit;
        drone.cargo = cargo;
        drone.cargoRotation = cargo != null ? cargo.rotation : Quaternion.identity;
        Vector3 heading = Vector3.ProjectOnPlane(hover - entry, Vector3.up);
        drone.flightRotation = Quaternion.LookRotation(heading.sqrMagnitude > 0.0001f ? heading : Vector3.forward);
        drone.onRelease = onRelease;
        drone.seed = index * 1.618f;
        drone.Build();
        drone.transform.position = entry;
        drone.transform.rotation = drone.flightRotation;
        drone.lastPosition = entry;
        go.SetActive(false);
        return drone;
    }

    public void SetDeliveryPose(Vector3 rest, Quaternion rotation)
    {
        deliveryRest = rest;
        deliveryRotation = rotation;
        cargoRotation = rotation;
    }

    // ---------------- Model ----------------

    private void Build()
    {
        ThermalFxLibrary fx = ThermalFxLibrary.Instance;
        lineMaterial = CombatVfxStyle.CreateMaterial("Drone lines", Color.white);
        smokeMaterial = CombatVfxStyle.CreateMaterial("Drone smoke", Color.white);
        smokeMaterial.mainTexture = HudSprites.Dot().texture;

        model = new GameObject("Model").transform;
        model.SetParent(transform, false);

        // Textured gunmetal hull with red running lights (nose along +Z).
        var hull = new GameObject("Hull");
        hull.transform.SetParent(model, false);
        Mesh mesh = fx != null ? fx.droneHullMesh : null;
        if (mesh != null)
        {
            hull.AddComponent<MeshFilter>().sharedMesh = mesh;
            var renderer = hull.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = fx.droneHull;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            float longest = Mathf.Max(mesh.bounds.size.x, mesh.bounds.size.z);
            hull.transform.localScale = Vector3.one * (0.5f / longest);
            hull.transform.localPosition = -Vector3.Scale(mesh.bounds.center, hull.transform.localScale);
        }

        Material dark = fx != null ? fx.droneBody : null;
        clawLeft = Part(PrimitiveType.Cube, "Claw L", dark, new Vector3(-0.05f, -0.16f, 0.02f), new Vector3(0.014f, 0.08f, 0.05f)).transform;
        clawRight = Part(PrimitiveType.Cube, "Claw R", dark, new Vector3(0.05f, -0.16f, 0.02f), new Vector3(0.014f, 0.08f, 0.05f)).transform;

        thruster = Flame3(transform, "Thruster", 0.07f);
        jetLeft = Flame3(transform, "Hover jet L", 0.045f);
        jetRight = Flame3(transform, "Hover jet R", 0.045f);
        searchlight = CombatVfxStyle.CreateLine(transform, "Searchlight", lineMaterial, true, 0.22f);
        searchlight.positionCount = 2;
        searchlight.widthCurve = new AnimationCurve(new Keyframe(0f, 0.03f), new Keyframe(1f, 1f));
        searchlight.enabled = true;
        tractor = CombatVfxStyle.CreateLine(transform, "Tractor beam", lineMaterial, true, 0.34f);
        tractor.positionCount = 2;
        tractor.widthCurve = new AnimationCurve(new Keyframe(0f, 0.15f), new Keyframe(1f, 1f));

        trail = gameObject.AddComponent<TrailRenderer>();
        trail.sharedMaterial = lineMaterial;
        trail.time = 0.25f;
        trail.widthCurve = new AnimationCurve(new Keyframe(0f, 0.05f), new Keyframe(1f, 0f));
        trail.startColor = CombatVfxStyle.WithAlpha(Flame, 0.7f);
        trail.endColor = CombatVfxStyle.WithAlpha(Red, 0f);
        trail.shadowCastingMode = ShadowCastingMode.Off;
        trail.minVertexDistance = 0.03f;

        var smokeObject = new GameObject("Exhaust smoke");
        smokeObject.transform.SetParent(transform, false);
        smokeObject.transform.localPosition = new Vector3(0f, 0f, -0.25f);
        smokeTrail = smokeObject.AddComponent<TrailRenderer>();
        smokeTrail.sharedMaterial = smokeMaterial;
        smokeTrail.time = 0.6f;
        smokeTrail.widthCurve = new AnimationCurve(new Keyframe(0f, 0.04f), new Keyframe(1f, 0.22f));
        smokeTrail.startColor = new Color(0.25f, 0.22f, 0.22f, 0.35f);
        smokeTrail.endColor = new Color(0.1f, 0.1f, 0.1f, 0f);
        smokeTrail.shadowCastingMode = ShadowCastingMode.Off;
        smokeTrail.minVertexDistance = 0.05f;

        var sphere = gameObject.AddComponent<SphereCollider>();
        sphere.radius = 0.3f;
        hitbox = sphere;
        body = gameObject.AddComponent<Rigidbody>();
        body.isKinematic = true;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        CombatLayers.SetLayerRecursively(gameObject, CombatLayers.GameplayCoverLayer);

        hum = gameObject.AddComponent<AudioSource>();
        hum.clip = SynthAudio.DroneHum();
        hum.loop = true;
        hum.spatialBlend = 1f;
        hum.minDistance = 0.5f;
        hum.maxDistance = 12f;
        hum.rolloffMode = AudioRolloffMode.Linear;
        hum.dopplerLevel = 0.8f;
        hum.volume = 0.6f;
        hum.pitch = 0.9f + (Index % 4) * 0.05f;
    }

    private LineRenderer Flame3(Transform parent, string name, float width)
    {
        LineRenderer line = CombatVfxStyle.CreateLine(parent, name, lineMaterial, true, width);
        line.positionCount = 3;
        line.widthCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.35f, 0.8f), new Keyframe(1f, 0f));
        var gradient = new Gradient();
        gradient.SetKeys(
            new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Flame, 0.3f), new GradientColorKey(Red, 1f) },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0.8f, 0.4f), new GradientAlphaKey(0f, 1f) });
        line.colorGradient = gradient;
        line.enabled = true;
        return line;
    }

    private Renderer Part(PrimitiveType type, string name, Material material, Vector3 position, Vector3 scale)
    {
        GameObject part = GameObject.CreatePrimitive(type);
        part.name = name;
        Destroy(part.GetComponent<Collider>());
        part.transform.SetParent(model, false);
        part.transform.localPosition = position;
        part.transform.localScale = scale;
        var renderer = part.GetComponent<Renderer>();
        if (material != null) renderer.sharedMaterial = material;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        return renderer;
    }

    // ---------------- Flight ----------------

    /// <summary>Drives the drone from the shared clock (seconds since sudden death began).</summary>
    public void Tick(float clock)
    {
        if (IsDown) { TickDown(); return; }
        float t = clock - startAt;
        if (t < 0f) { if (gameObject.activeSelf) gameObject.SetActive(false); return; }
        if (t > Gone)
        {
            Finished = true;
            if (gameObject.activeSelf) gameObject.SetActive(false);
            if (job == Job.Pickup && cargo != null && cargoAttached) cargo.gameObject.SetActive(false);
            return;
        }
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            trail.Clear();
            smokeTrail.Clear();
            hum.Play();
        }
        if (!whooshedIn && t > Arrive - 0.45f) { whooshedIn = true; SynthAudio.PlayAt(SynthAudio.Whoosh(), hover, 0.8f, 1.1f); }
        if (!whooshedOut && t > GrabEnd) { whooshedOut = true; SynthAudio.PlayAt(SynthAudio.Whoosh(), transform.position, 0.7f, 1.35f); }

        Vector3 position = Pose(t);
        velocity = (position - lastPosition) / Mathf.Max(Time.deltaTime, 0.0001f);
        lastPosition = position;
        body.MovePosition(position);
        transform.position = position;

        // Hold the nose at its world-space approach bearing throughout the flight.
        transform.rotation = flightRotation;

        AnimateParts(t);
        UpdateCargo(t);
    }

    private static float EaseOut(float x) { x = Mathf.Clamp01(x); return 1f - (1f - x) * (1f - x) * (1f - x); }
    private static float EaseIn(float x) { x = Mathf.Clamp01(x); return x * x * x; }

    private static Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, float k)
        => Vector3.Lerp(Vector3.Lerp(a, b, k), Vector3.Lerp(b, c, k), k);

    private Vector3 Pose(float t)
    {
        float bob = Mathf.Sin((t + seed) * 5f) * 0.02f;
        if (t < Arrive)
        {
            // High-speed swoop: dive from far outside, flare into a hover.
            Vector3 control = Vector3.Lerp(entry, hover, 0.6f) + Vector3.up * 0.9f;
            return Bezier(entry, control, hover, EaseOut(t / Arrive));
        }
        if (t < GrabEnd)
        {
            float k = (t - Arrive) / (GrabEnd - Arrive);
            return Vector3.Lerp(hover, low, Mathf.Sin(Mathf.PI * k)) + Vector3.up * bob;
        }
        Vector3 climb = hover + Vector3.up * 0.8f;
        return Bezier(hover, climb, exit, EaseIn((t - GrabEnd) / (Gone - GrabEnd)));
    }

    private void AnimateParts(float t)
    {
        float speed = velocity.magnitude;
        float flicker = 0.85f + 0.3f * Mathf.PerlinNoise(Time.time * 30f, seed);
        Vector3 back = -transform.forward;
        Vector3 rear = transform.position + back * 0.22f + transform.up * 0.02f;
        SetFlame(thruster, rear, back, (0.12f + Mathf.Min(speed, 8f) * 0.05f) * flicker);
        Vector3 belly = transform.position - transform.up * 0.08f;
        float jet = (0.08f + 0.06f * (1f - Mathf.Clamp01(speed / 4f))) * flicker;
        SetFlame(jetLeft, belly - transform.right * 0.1f, -transform.up, jet);
        SetFlame(jetRight, belly + transform.right * 0.1f, -transform.up, jet);

        // Sweeping red scan light while working, off at speed.
        float scan = Mathf.Clamp01(1f - speed / 3f);
        Vector3 scanDir = (transform.forward * 0.6f - Vector3.up + transform.right * Mathf.Sin(Time.time * 2.4f + seed) * 0.5f).normalized;
        searchlight.SetPosition(0, transform.position + transform.forward * 0.2f);
        searchlight.SetPosition(1, transform.position + scanDir * 1.6f);
        searchlight.startColor = CombatVfxStyle.WithAlpha(Red, 0f * scan);
        searchlight.enabled = false; // removed for clarity: too much visual noise with many drones
        searchlight.endColor = CombatVfxStyle.WithAlpha(Red, 0f);

        float open = job == Job.Pickup ? (t < GrabMid ? 1f : 0f) : (t < GrabMid ? 0f : 1f);
        float jaw = Mathf.Lerp(0.035f, 0.08f, open);
        clawLeft.localPosition = new Vector3(-jaw, -0.16f, 0.02f);
        clawRight.localPosition = new Vector3(jaw, -0.16f, 0.02f);

        bool beam = t > Arrive - 0.15f && t < GrabEnd + 0.1f;
        tractor.enabled = beam;
        if (beam)
        {
            float pulse = 0.35f + 0.2f * Mathf.Sin(Time.time * 45f);
            tractor.SetPosition(0, transform.position - Vector3.up * 0.15f);
            tractor.SetPosition(1, new Vector3(transform.position.x, low.y - 0.3f, transform.position.z));
            tractor.startColor = CombatVfxStyle.WithAlpha(Color.Lerp(Red, Color.white, 0.2f), pulse);
            tractor.endColor = CombatVfxStyle.WithAlpha(Red, 0.03f);
        }
    }

    private static void SetFlame(LineRenderer line, Vector3 start, Vector3 direction, float length)
    {
        line.SetPosition(0, start);
        line.SetPosition(1, start + direction * length * 0.35f);
        line.SetPosition(2, start + direction * length);
    }

    private void UpdateCargo(float t)
    {
        if (cargo == null || cargoReleased) return;
        if (job == Job.Pickup)
        {
            if (!cargoAttached && t >= GrabMid)
            {
                cargoAttached = true;
                cargoOffset = cargo.position - transform.position;
                SynthAudio.PlayAt(SynthAudio.Clunk(), cargo.position, 0.8f, 1.3f);
                ThermalFxLibrary.DustBurst(cargo.position, 0.8f);
            }
            if (!cargoAttached)
            {
                return;
            }
        }
        else if (!cargoAttached)
        {
            cargoAttached = true;
            cargo.gameObject.SetActive(true);
        }

        if (job == Job.Delivery && t >= GrabMid)
        {
            Release(false);
            return;
        }
        Vector3 anchor = job == Job.Pickup ? transform.position + cargoOffset : transform.position + (deliveryRest - low);
        cargo.SetPositionAndRotation(anchor, cargoRotation);
    }

    private void Release(bool falling)
    {
        if (cargo == null || cargoReleased) return;
        cargoReleased = true;
        if (falling)
        {
            cargo.gameObject.SetActive(true);
            if (!cargo.TryGetComponent(out Rigidbody rb)) rb = cargo.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.mass = 40f;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.linearVelocity = Vector3.ClampMagnitude(velocity, 5f);
            rb.angularVelocity = Random.insideUnitSphere * 3f;
            if (!cargo.TryGetComponent(out CargoImpact _)) cargo.gameObject.AddComponent<CargoImpact>();
        }
        else
        {
            cargo.SetPositionAndRotation(deliveryRest, deliveryRotation);
        }
        onRelease?.Invoke(this);
    }

    // ---------------- Shot down ----------------

    /// <summary>Local shooter hit this drone; the shot is confirmed by the host.</summary>
    public void ReportShot(Vector3 point)
    {
        if (IsDown || !gameObject.activeSelf) return;
        if (Time.time - lastShotFxAt > 0.15f)
        {
            lastShotFxAt = Time.time;
            ThermalFxLibrary.Spawn(ThermalFxLibrary.Instance?.metalSparks, point, 0.6f, 2f);
        }
        var local = Mirror.NetworkClient.localPlayer;
        if (local != null && local.TryGetComponent(out FusionRoundDirector director))
            director.RequestDroneDown(Index);
    }

    public void KnockDown()
    {
        if (IsDown) return;
        IsDown = true;
        downAt = Time.time;
        gameObject.SetActive(true);
        tractor.enabled = false;
        searchlight.enabled = false;
        jetLeft.enabled = jetRight.enabled = false;
        ThermalFxLibrary fx = ThermalFxLibrary.Instance;
        ThermalFxLibrary.Spawn(fx?.electroHit, transform.position, 0.6f, 2f);
        ThermalFxLibrary.Spawn(fx?.hitSparks, transform.position, 0.8f, 2f);
        ThermalFxLibrary.Spawn(fx?.hitExplosion, transform.position, 0.18f, 2f);
        SynthAudio.PlayAt(SynthAudio.PowerDown(), transform.position, 1f);
        SynthAudio.PlayAt(SynthAudio.Explosion(), transform.position, 0.5f, 1.4f);
        FusionRoundHud.Current?.Toast("DRONE DOWN", CombatVfxStyle.Heat, 1.8f);
        if (cargo != null && !cargoReleased && (job == Job.Delivery || cargoAttached))
            Release(true);
        body.isKinematic = false;
        body.mass = 4f;
        body.linearVelocity = Vector3.ClampMagnitude(velocity, 4f) + Vector3.up * 0.8f;
        body.angularVelocity = new Vector3(Random.Range(-5f, 5f), Random.Range(9f, 15f), Random.Range(-5f, 5f));
        hitbox.gameObject.layer = 0;
        smokeTrail.time = 2.5f;
        smokeTrail.startColor = new Color(0.12f, 0.1f, 0.1f, 0.8f);
        smokeTrail.widthCurve = new AnimationCurve(new Keyframe(0f, 0.12f), new Keyframe(1f, 0.9f));
        if (fx != null && fx.fireMedium != null)
        {
            fireFx = ThermalFxLibrary.Spawn(fx.fireMedium, transform.position, 0.25f, 6f);
            if (fireFx != null) fireFx.transform.SetParent(transform, true);
        }
    }

    private void TickDown()
    {
        float age = Time.time - downAt;
        float sputter = Random.value > 0.5f ? 1f : 0.2f;
        SetFlame(thruster, transform.position - transform.forward * 0.22f, -transform.forward, 0.25f * sputter);
        hum.pitch = Mathf.Lerp(hum.pitch, 0.3f, Time.deltaTime * 2f);
        hum.volume = Mathf.Lerp(hum.volume, 0f, Time.deltaTime * 1.5f);
        if (crashedAt < 0f && age > 2.2f) Explode();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsDown && crashedAt < 0f && Time.time - downAt > 0.15f) Explode();
    }

    private void Explode()
    {
        crashedAt = Time.time;
        ThermalFxLibrary fx = ThermalFxLibrary.Instance;
        GameObject blast = fx == null ? null : fx.bigExplosion != null ? fx.bigExplosion : fx.droneExplosion;
        ThermalFxLibrary.Spawn(blast, transform.position, 0.3f, 5f);
        ThermalFxLibrary.DustBurst(transform.position, 1.2f);
        ThermalFxLibrary.Spawn(fx?.fireMedium, transform.position, 0.22f, 5f);
        SynthAudio.PlayAt(SynthAudio.Explosion(), transform.position, 1f);
        Finished = true;
        if (fireFx != null) fireFx.transform.SetParent(null, true);
        smokeTrail.transform.SetParent(null, true);
        smokeTrail.autodestruct = true;
        smokeTrail.emitting = false;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (lineMaterial != null) Destroy(lineMaterial);
    }
}

/// <summary>Dust, thud and a spark burst when falling cargo hits the floor.</summary>
public sealed class CargoImpact : MonoBehaviour
{
    private float lastImpact = -1f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude < 1.2f || Time.time - lastImpact < 0.25f) return;
        lastImpact = Time.time;
        Vector3 point = collision.GetContact(0).point;
        ThermalFxLibrary.DustBurst(point, 1f);
        ThermalFxLibrary.Spawn(ThermalFxLibrary.Instance?.metalSparks, point, 0.8f, 2f);
        SynthAudio.PlayAt(SynthAudio.Clunk(), point, Mathf.Clamp01(collision.relativeVelocity.magnitude / 5f), 0.8f);
    }
}
