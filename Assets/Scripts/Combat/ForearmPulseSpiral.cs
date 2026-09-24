using UnityEngine;

/// <summary>
/// Always-on double helix wrapped around the tracked right forearm (elbow to wrist), where the
/// Peltiers and vibros sit. Idle it breathes softly; <see cref="ArmActivationSignal"/> drives
/// its colour, pulse travelling along the arm, build-up tightening, peak flash and vibro jitter.
/// </summary>
[DisallowMultipleComponent]
public sealed class ForearmPulseSpiral : MonoBehaviour
{
    [SerializeField] private HandPoseRouter poseRouter;
    [SerializeField] private Transform headset;
    [SerializeField, Min(0.1f)] private float forearmLength = 0.25f;
    [SerializeField, Min(0.01f)] private float armRadius = 0.048f;
    [SerializeField, Range(0f, 1f)] private float idleOpacity = 0.28f;
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
    }

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

        for (int s = 0; s < strands.Length; s++)
        {
            LineRenderer line = strands[s];
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
            float strandAlpha = s == 2 ? 0.55f : s == 1 ? 0.8f : 1f;
            ApplyGradient(line, signal, energy, breathing, strandAlpha);
            line.widthMultiplier = (s == 0 ? 0.009f : s == 1 ? 0.0065f : 0.003f)
                * (1f + energy * 0.9f + signal.Vibration * 0.4f);
            line.enabled = true;
        }

        Quaternion ringPlane = Quaternion.LookRotation(axis, up);
        Color tint = signal.Color;
        float collarAlpha = visible * Mathf.Lerp(idleOpacity * 0.8f, 1f, energy);
        CombatVfxStyle.SetRing(wristCollar, smoothedWrist, ringPlane,
            armRadius * 0.95f * swell, 36, phase * 40f, 300f);
        CombatVfxStyle.SetRing(elbowCollar, elbow, ringPlane,
            armRadius * 1.2f * swell, 36, -phase * 30f, 260f);
        wristCollar.startColor = CombatVfxStyle.WithAlpha(tint, collarAlpha);
        wristCollar.endColor = CombatVfxStyle.WithAlpha(tint, 0.02f);
        elbowCollar.startColor = CombatVfxStyle.WithAlpha(tint, collarAlpha * 0.7f);
        elbowCollar.endColor = CombatVfxStyle.WithAlpha(tint, 0.02f);
        wristCollar.enabled = elbowCollar.enabled = true;

        // A bright ring sweeps along the arm while active, like heat moving through the pads.
        float ringT = FlowPosition(signal);
        CombatVfxStyle.SetRing(flowRing, elbow + axis * (forearmLength * ringT), ringPlane,
            armRadius * Mathf.Lerp(1.15f, 0.82f, ringT) * swell * 1.08f, 40);
        Color ringColor = Color.Lerp(tint, Color.white, 0.35f * energy);
        flowRing.startColor = flowRing.endColor = CombatVfxStyle.WithAlpha(ringColor,
            visible * Mathf.Clamp01(energy * 1.2f + signal.Anticipation * 0.6f));
        flowRing.widthMultiplier = 0.004f + 0.006f * energy;
        flowRing.enabled = energy > 0.02f || signal.Anticipation > 0f;
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
            * (0.2f + energy * 0.8f + signal.Anticipation * 0.5f));
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
    }

    private void OnDisable() => SetEnabled(false);

    private void OnDestroy()
    {
        if (material != null) Destroy(material);
    }
}
