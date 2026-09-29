using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Always-on double helix wrapped around the tracked right forearm (elbow to wrist), where the
/// Peltiers and vibros sit. Idle it breathes softly; <see cref="ArmActivationSignal"/> drives
/// its colour, pulse travelling along the arm, build-up tightening, peak flash and vibro jitter.
/// Each activity keeps its own look: heat shows the helix, cold (ice) fades it to the collar and
/// flow rings, and the shield replaces everything with a pulsing purple aura.
/// </summary>
[DisallowMultipleComponent]
public sealed class ForearmPulseSpiral : MonoBehaviour
{
    [SerializeField] private HandPoseRouter poseRouter;
    [SerializeField] private Transform headset;
    [SerializeField, Min(0.1f)] private float forearmLength = 0.25f;
    [SerializeField, Min(0.01f)] private float armRadius = 0.048f;
    [Tooltip("Opacity with nothing active. Zero keeps the arm completely clean at rest.")]
    [SerializeField, Range(0f, 1f)] private float idleOpacity = 0f;
    [SerializeField, Min(1f)] private float turns = 4.5f;

    private const int Segments = 72;
    private readonly LineRenderer[] strands = new LineRenderer[3];
    private LineRenderer wristCollar, elbowCollar, flowRing;
    private Material material;
    private Gradient gradient;
    private readonly GradientColorKey[] colorKeys = new GradientColorKey[2];
    private readonly GradientAlphaKey[] alphaKeys = new GradientAlphaKey[5];
    private float phase, flow, visible;
    private Vector3 smoothedWrist;
    private Quaternion smoothedRotation = Quaternion.identity;

    // Shield aura: a steady glow shell plus two shells that swell outward and fade, half a pulse apart.
    private const int AuraLayers = 3, AuraSides = 24, AuraRows = 12;
    private readonly Vector3[] auraNormals = new Vector3[AuraSides];
    private readonly Vector3[] auraVertices = new Vector3[AuraLayers * AuraRows * AuraSides];
    private readonly Color32[] auraColors = new Color32[AuraLayers * AuraRows * AuraSides];
    private Transform auraRoot;
    private Mesh auraMesh;
    private Material auraMaterial;
    private MeshRenderer auraRenderer;
    private float auraPhase;

    /// <summary>Smoothed forearm pose shared with other arm effects.</summary>
    public bool HasPose => visible > 0.5f;
    public Vector3 Wrist => smoothedWrist;
    public Quaternion ArmRotation => smoothedRotation;
    public Vector3 Elbow => smoothedWrist - smoothedRotation * Vector3.forward * forearmLength;
    public float ArmRadius => armRadius;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AttachToLocalRig()
    {
        foreach (HandPoseRouter router in Object.FindObjectsByType<HandPoseRouter>(FindObjectsSortMode.None))
        {
            if (router.GetComponent<ForearmPulseSpiral>() == null)
                router.gameObject.AddComponent<ForearmPulseSpiral>();
            if (router.GetComponent<ForearmBursts>() == null)
                router.gameObject.AddComponent<ForearmBursts>();
        }
    }

    private void Awake()
    {
        if (poseRouter == null) poseRouter = GetComponent<HandPoseRouter>();
        material = CombatVfxStyle.CreateMaterial("Forearm pulse", Color.white);
        var root = new GameObject("ForearmPulseVFX").transform;
        root.SetParent(transform, false);
        float[] widths = { 0.009f, 0.0065f, 0.003f };
        for (int i = 0; i < strands.Length; i++)
        {
            strands[i] = CombatVfxStyle.CreateLine(root, "ForearmStrand" + i, material, true, widths[i]);
            strands[i].positionCount = Segments + 1;
        }
        wristCollar = CombatVfxStyle.CreateLine(root, "WristCollar", material, true, 0.005f);
        elbowCollar = CombatVfxStyle.CreateLine(root, "ElbowCollar", material, true, 0.004f);
        flowRing = CombatVfxStyle.CreateLine(root, "FlowRing", material, true, 0.007f);
        gradient = new Gradient();
        CreateAura(root);
    }

    private void CreateAura(Transform parent)
    {
        auraRoot = new GameObject("ShieldAura").transform;
        auraRoot.SetParent(parent, false);
        for (int side = 0; side < AuraSides; side++)
        {
            float angle = side * Mathf.PI * 2f / AuraSides;
            auraNormals[side] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
        }
        var triangles = new int[AuraLayers * (AuraRows - 1) * AuraSides * 6];
        int n = 0;
        for (int layer = 0; layer < AuraLayers; layer++)
            for (int row = 0; row < AuraRows - 1; row++)
                for (int side = 0; side < AuraSides; side++)
                {
                    int next = (side + 1) % AuraSides;
                    int a = AuraIndex(layer, row, side), b = AuraIndex(layer, row, next);
                    int c = AuraIndex(layer, row + 1, side), d = AuraIndex(layer, row + 1, next);
                    triangles[n++] = a; triangles[n++] = c; triangles[n++] = b;
                    triangles[n++] = b; triangles[n++] = c; triangles[n++] = d;
                }
        auraMesh = new Mesh { name = "Shield aura" };
        auraMesh.MarkDynamic();
        auraMesh.vertices = auraVertices;
        auraMesh.colors32 = auraColors;
        auraMesh.triangles = triangles;
        // Fixed bounds cover the widest ripple; vertices move every frame without recalculating them.
        auraMesh.bounds = new Bounds(new Vector3(0f, 0f, forearmLength * 0.5f),
            new Vector3(0.45f, 0.45f, forearmLength + 0.3f));
        auraRoot.gameObject.AddComponent<MeshFilter>().sharedMesh = auraMesh;
        auraMaterial = CombatVfxStyle.CreateMaterial("Shield aura", Color.white);
        auraRenderer = auraRoot.gameObject.AddComponent<MeshRenderer>();
        auraRenderer.sharedMaterial = auraMaterial;
        auraRenderer.shadowCastingMode = ShadowCastingMode.Off;
        auraRenderer.receiveShadows = false;
        auraRenderer.enabled = false;
    }

    private static int AuraIndex(int layer, int row, int side) => (layer * AuraRows + row) * AuraSides + side;

    private void LateUpdate()
    {
        if (headset == null)
            headset = Camera.main != null ? Camera.main.transform : null;
        Vector3 wrist = smoothedWrist;
        Quaternion rotation = smoothedRotation;
        bool tracked = poseRouter != null
            && poseRouter.TryGetForearmPose(headset, out wrist, out rotation);
        if (!tracked) { wrist = smoothedWrist; rotation = smoothedRotation; }
        visible = Mathf.MoveTowards(visible, tracked ? 1f : 0f, Time.deltaTime * (tracked ? 6f : 3f));
        if (visible <= 0.001f) { SetEnabled(false); return; }

        // Light smoothing hides hand-tracking jitter without visible lag.
        float follow = 1f - Mathf.Exp(-Time.deltaTime * 30f);
        smoothedWrist = tracked && smoothedWrist == Vector3.zero ? wrist : Vector3.Lerp(smoothedWrist, wrist, follow);
        smoothedRotation = Quaternion.Slerp(smoothedRotation, rotation, follow);

        ArmActivationSignal.Reading signal = ArmActivationSignal.Sample(Time.time);
        float energy = Mathf.Clamp01(signal.Intensity);
        // Completely clean arm at rest: nothing drawn until a weapon, shield or hit is active.
        if (idleOpacity <= 0f && energy < 0.01f && signal.Anticipation <= 0f) { SetEnabled(false); return; }
        float breathing = 0.5f + 0.5f * Mathf.Sin(Time.time * 1.7f);
        // Build-up spins faster and cinches the helix; the peak releases it outward.
        float spin = 1.4f + energy * 9f + signal.Anticipation * 14f;
        phase += Time.deltaTime * spin;
        flow = Mathf.Repeat(flow + Time.deltaTime * (0.35f + energy * 1.6f), 1f);
        float cinch = 1f - 0.28f * signal.Anticipation * signal.Anticipation;
        float swell = 1f + 0.35f * Mathf.Max(0f, signal.Intensity - 1f) + 0.12f * energy;

        Vector3 axis = smoothedRotation * Vector3.forward;
        Vector3 up = smoothedRotation * Vector3.up;
        Vector3 side = smoothedRotation * Vector3.right;
        Vector3 elbow = smoothedWrist - axis * forearmLength;

        // Ice reads as rings and the shield as an aura: the helix fades out for both.
        float spiral = Mathf.Clamp01(1f - signal.Cold - signal.Shield);
        float lines = 1f - signal.Shield;
        for (int s = 0; s < strands.Length; s++)
        {
            LineRenderer line = strands[s];
            line.enabled = spiral > 0.02f;
            if (!line.enabled) continue;
            float offset = s * Mathf.PI * (s == 2 ? 0.5f : 1f);
            float direction = s == 2 ? -1.6f : 1f;
            for (int i = 0; i <= Segments; i++)
            {
                float t = i / (float)Segments;
                // Taper: fuller near the elbow, snug at the wrist.
                float radius = armRadius * Mathf.Lerp(1.12f, 0.78f, t) * cinch * swell
                    * (s == 2 ? 1.18f : 1f);
                radius += Mathf.PerlinNoise(t * 9f + s * 3f, Time.time * 38f) * signal.Vibration * 0.012f;
                float angle = t * turns * Mathf.PI * 2f * direction + phase * direction + offset;
                Vector3 radial = side * Mathf.Cos(angle) + up * Mathf.Sin(angle);
                line.SetPosition(i, elbow + axis * (forearmLength * t) + radial * radius);
            }
            float strandAlpha = (s == 2 ? 0.55f : s == 1 ? 0.8f : 1f) * spiral;
            ApplyGradient(line, signal, energy, breathing, strandAlpha);
            line.widthMultiplier = (s == 0 ? 0.009f : s == 1 ? 0.0065f : 0.003f)
                * (1f + energy * 0.9f + signal.Vibration * 0.4f);
        }

        Quaternion ringPlane = Quaternion.LookRotation(axis, up);
        Color tint = signal.Color;
        float collarAlpha = visible * lines * Mathf.Lerp(idleOpacity * 0.8f, 1f, energy);
        CombatVfxStyle.SetRing(wristCollar, smoothedWrist, ringPlane,
            armRadius * 0.95f * swell, 36, phase * 40f, 300f);
        CombatVfxStyle.SetRing(elbowCollar, elbow, ringPlane,
            armRadius * 1.2f * swell, 36, -phase * 30f, 260f);
        wristCollar.startColor = CombatVfxStyle.WithAlpha(tint, collarAlpha);
        wristCollar.endColor = CombatVfxStyle.WithAlpha(tint, 0.02f);
        elbowCollar.startColor = CombatVfxStyle.WithAlpha(tint, collarAlpha * 0.7f);
        elbowCollar.endColor = CombatVfxStyle.WithAlpha(tint, 0.02f);
        wristCollar.enabled = elbowCollar.enabled = lines > 0.02f;

        // A bright ring sweeps along the arm while active, like heat moving through the pads.
        float ringT = FlowPosition(signal);
        CombatVfxStyle.SetRing(flowRing, elbow + axis * (forearmLength * ringT), ringPlane,
            armRadius * Mathf.Lerp(1.15f, 0.82f, ringT) * swell * 1.08f, 40);
        Color ringColor = Color.Lerp(tint, Color.white, 0.35f * energy);
        flowRing.startColor = flowRing.endColor = CombatVfxStyle.WithAlpha(ringColor,
            visible * lines * Mathf.Clamp01(energy * 1.2f + signal.Anticipation * 0.6f));
        flowRing.widthMultiplier = 0.004f + 0.006f * energy;
        flowRing.enabled = (energy > 0.02f || signal.Anticipation > 0f) && lines > 0.02f;

        UpdateAura(signal, energy, elbow, swell);
    }

    // Soft purple glow around the forearm: fades toward elbow and wrist, brightest around the arm's
    // silhouette, and pulses outward. A block flares it wider and brighter.
    private void UpdateAura(ArmActivationSignal.Reading signal, float energy, Vector3 elbow, float swell)
    {
        float strength = signal.Shield * visible * Mathf.Clamp01(energy * 1.8f + signal.Anticipation * 0.6f);
        auraRenderer.enabled = strength > 0.01f;
        if (!auraRenderer.enabled) return;
        auraPhase = Mathf.Repeat(auraPhase + Time.deltaTime * (1.1f + signal.Vibration * 1.5f), 1f);
        float flare = Mathf.Clamp01((signal.Intensity - 0.7f) * 1.5f);
        float beat = 0.5f + 0.5f * Mathf.Cos(auraPhase * Mathf.PI * 2f);
        auraRoot.SetPositionAndRotation(elbow, smoothedRotation);
        Vector3 eye = headset != null ? auraRoot.InverseTransformPoint(headset.position) : Vector3.up;
        const float start = -0.03f;
        float length = forearmLength + 0.07f;
        Color shield = CombatVfxStyle.Shield;
        int v = 0;
        for (int layer = 0; layer < AuraLayers; layer++)
        {
            float ripple = layer == 0 ? 0f : Mathf.Repeat(auraPhase + (layer - 1) * 0.5f, 1f);
            float fade = (1f - ripple) * (1f - ripple);
            float scale = layer == 0 ? 1.15f + 0.08f * beat : Mathf.Lerp(1.2f, 2.1f, 1f - fade);
            float layerAlpha = layer == 0 ? 0.35f + 0.2f * beat : 0.4f * fade;
            Color tint = layer == 0 ? Color.Lerp(shield, Color.white, 0.2f + 0.3f * flare) : shield;
            for (int row = 0; row < AuraRows; row++)
            {
                float t = row / (AuraRows - 1f);
                float z = start + length * t;
                float radius = armRadius * Mathf.Lerp(1.12f, 0.8f, Mathf.Clamp01(z / forearmLength))
                    * swell * scale * (1f + 0.35f * flare);
                float ends = Mathf.Sin(Mathf.PI * t);
                for (int side = 0; side < AuraSides; side++)
                {
                    Vector3 normal = auraNormals[side];
                    Vector3 point = normal * radius + new Vector3(0f, 0f, z);
                    float rim = 1f - Mathf.Abs(Vector3.Dot(normal, (eye - point).normalized));
                    tint.a = Mathf.Clamp01(strength * layerAlpha * ends * Mathf.Lerp(0.3f, 1f, rim * rim) * (1f + flare));
                    auraVertices[v] = point;
                    auraColors[v] = tint;
                    v++;
                }
            }
        }
        auraMesh.SetVertices(auraVertices, 0, auraVertices.Length, MeshUpdateFlags.DontRecalculateBounds);
        auraMesh.colors32 = auraColors;
    }

    // Build-up draws energy from both ends toward the centre; active pulses flow elbow to wrist.
    private float FlowPosition(ArmActivationSignal.Reading signal)
        => signal.Anticipation > 0f && signal.Intensity < 0.6f
            ? Mathf.Lerp(0.05f, 0.5f, signal.Anticipation) : flow;

    private void ApplyGradient(LineRenderer line, ArmActivationSignal.Reading signal,
        float energy, float breathing, float strandAlpha)
    {
        float baseAlpha = visible * strandAlpha
            * Mathf.Lerp(idleOpacity * (0.75f + 0.25f * breathing), 0.85f, energy);
        float band = FlowPosition(signal);
        float bandAlpha = Mathf.Clamp01(baseAlpha + visible * strandAlpha
            * (energy + signal.Anticipation * 0.5f));
        colorKeys[0] = new GradientColorKey(signal.Color, 0f);
        colorKeys[1] = new GradientColorKey(Color.Lerp(signal.Color, Color.white, 0.3f * energy), 1f);
        alphaKeys[0] = new GradientAlphaKey(baseAlpha * 0.25f, 0f);
        alphaKeys[1] = new GradientAlphaKey(baseAlpha, Mathf.Clamp(band - 0.18f, 0.02f, 0.96f));
        alphaKeys[2] = new GradientAlphaKey(bandAlpha, Mathf.Clamp(band, 0.03f, 0.97f));
        alphaKeys[3] = new GradientAlphaKey(baseAlpha, Mathf.Clamp(band + 0.18f, 0.04f, 0.98f));
        alphaKeys[4] = new GradientAlphaKey(baseAlpha * 0.6f, 1f);
        gradient.SetKeys(colorKeys, alphaKeys);
        line.colorGradient = gradient;
    }

    private void SetEnabled(bool enabled)
    {
        foreach (LineRenderer line in strands) if (line != null) line.enabled = enabled;
        if (wristCollar != null) wristCollar.enabled = enabled;
        if (elbowCollar != null) elbowCollar.enabled = enabled;
        if (flowRing != null) flowRing.enabled = enabled;
        if (auraRenderer != null) auraRenderer.enabled = enabled;
    }

    private void OnDisable() => SetEnabled(false);

    private void OnDestroy()
    {
        if (material != null) Destroy(material);
        if (auraMaterial != null) Destroy(auraMaterial);
        if (auraMesh != null) Destroy(auraMesh);
    }
}
