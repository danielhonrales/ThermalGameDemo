using UnityEngine;

[DisallowMultipleComponent]
public sealed class WeaponTransitionEffects : MonoBehaviour
{
    [Header("Wrist sweep")]
    [SerializeField, Min(0.05f)] private float ringRadius = 0.1f;
    [SerializeField, Min(0.001f)] private float ringWidth = 0.008f;
    [SerializeField, Min(0.08f)] private float completionSeconds = 0.26f;

    private Transform effectRoot;
    private LineRenderer sweep;
    private LineRenderer trace;
    private LineRenderer spiralA;
    private LineRenderer spiralB;
    private LineRenderer nearCollar;
    private LineRenderer farCollar;
    private Material lineMaterial;
    private float completionStartedAt = -1f;
    private bool showingProgress;
    private Color activeColor;
    private Vector3 forearmDirection = Vector3.down;

    private void Update()
    {
        if (effectRoot == null || completionStartedAt < 0f)
        {
            return;
        }

        float progress = (Time.time - completionStartedAt) / completionSeconds;
        if (progress >= 1f)
        {
            completionStartedAt = -1f;
            if (!showingProgress)
            {
                effectRoot.gameObject.SetActive(false);
            }
            return;
        }

        float eased = 1f - Mathf.Pow(1f - progress, 2f);
        float radius = Mathf.Lerp(ringRadius * 1.08f, ringRadius * 0.66f, eased);
        DrawArc(sweep, radius, 320f, Time.time * 100f, 0.6f * (1f - eased));
        DrawArc(trace, radius * 0.77f, 160f, -Time.time * 125f, 0.22f * (1f - eased));
        DrawForearm(1f - eased);
    }

    public void ShowTransition(
        Vector3 worldPosition,
        Quaternion worldRotation,
        float progress,
        CombatWeaponMode.WeaponMode targetMode,
        Vector3 armDirection)
    {
        EnsureEffects();
        showingProgress = true;
        completionStartedAt = -1f;
        activeColor = ColorFor(targetMode);
        forearmDirection = armDirection.sqrMagnitude > 0.1f
            ? armDirection.normalized : -(worldRotation * Vector3.up);
        effectRoot.gameObject.SetActive(true);
        effectRoot.SetPositionAndRotation(worldPosition, worldRotation);

        float eased = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(progress));
        DrawArc(sweep, ringRadius, Mathf.Lerp(12f, 290f, eased), -90f, 0.18f + eased * 0.52f);
        DrawArc(trace, ringRadius * 0.76f, Mathf.Lerp(5f, 155f, eased), 90f, 0.12f + eased * 0.16f);
        DrawForearm(0.35f + eased * 0.65f);
    }

    public void HideTransition()
    {
        showingProgress = false;
        if (completionStartedAt < 0f && effectRoot != null)
        {
            effectRoot.gameObject.SetActive(false);
        }
    }

    public void PlayCompletion(
        Vector3 worldPosition,
        Quaternion worldRotation,
        CombatWeaponMode.WeaponMode targetMode,
        Vector3 armDirection)
    {
        EnsureEffects();
        showingProgress = false;
        activeColor = ColorFor(targetMode);
        forearmDirection = armDirection.sqrMagnitude > 0.1f
            ? armDirection.normalized : -(worldRotation * Vector3.up);
        effectRoot.gameObject.SetActive(true);
        effectRoot.SetPositionAndRotation(worldPosition, worldRotation);
        completionStartedAt = Time.time;
        DrawArc(sweep, ringRadius, 320f, -90f, 0.6f);
        DrawArc(trace, ringRadius * 0.77f, 160f, 90f, 0.22f);
        DrawForearm(1f);
    }

    private void EnsureEffects()
    {
        if (effectRoot != null)
        {
            return;
        }

        GameObject rootObject = new GameObject("WeaponTransitionVFX");
        rootObject.transform.SetParent(transform, false);
        effectRoot = rootObject.transform;
        lineMaterial = CombatVfxStyle.CreateMaterial("Wrist sweep", Color.white);
        sweep = CombatVfxStyle.CreateLine(effectRoot, "ModeSweep", lineMaterial, false, ringWidth * 0.6f);
        trace = CombatVfxStyle.CreateLine(effectRoot, "ModeTrace", lineMaterial, false, ringWidth * 0.35f);
        spiralA = CombatVfxStyle.CreateLine(effectRoot, "ForearmSpiralA", lineMaterial, false, 0.01f);
        spiralB = CombatVfxStyle.CreateLine(effectRoot, "ForearmSpiralB", lineMaterial, false, 0.007f);
        nearCollar = CombatVfxStyle.CreateLine(effectRoot, "ForearmCollarNear", lineMaterial, false, 0.007f);
        farCollar = CombatVfxStyle.CreateLine(effectRoot, "ForearmCollarFar", lineMaterial, false, 0.006f);
        effectRoot.gameObject.SetActive(false);
    }

    private void DrawArc(LineRenderer line, float radius, float degrees, float startDegrees, float opacity)
    {
        CombatVfxStyle.SetRing(line, Vector3.zero, Quaternion.identity, radius, 40, startDegrees, degrees);
        line.startColor = CombatVfxStyle.WithAlpha(activeColor, opacity);
        line.endColor = CombatVfxStyle.WithAlpha(activeColor, opacity * 0.08f);
    }

    private static Color ColorFor(CombatWeaponMode.WeaponMode mode)
    {
        return mode == CombatWeaponMode.WeaponMode.IceGrenade
            ? CombatVfxStyle.Cold
            : CombatVfxStyle.Heat;
    }

    private void DrawForearm(float opacity)
    {
        Vector3 axis = Quaternion.Inverse(effectRoot.rotation) * forearmDirection;
        axis.Normalize();
        Vector3 tangent = Vector3.Cross(axis, Vector3.up);
        if (tangent.sqrMagnitude < 0.01f)
            tangent = Vector3.Cross(axis, Vector3.right);
        tangent.Normalize();
        Vector3 bitangent = Vector3.Cross(axis, tangent).normalized;
        const int segments = 56;
        spiralA.positionCount = spiralB.positionCount = segments + 1;
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float radius = 0.052f + 0.025f * Mathf.Sin(t * Mathf.PI);
            float angle = t * Mathf.PI * 5f - Time.time * 11f;
            Vector3 radial = (tangent * Mathf.Cos(angle)
                + bitangent * Mathf.Sin(angle)) * radius;
            Vector3 center = axis * (0.015f + 0.25f * t);
            spiralA.SetPosition(i, center + radial);
            spiralB.SetPosition(i, center - radial);
        }
        spiralA.startColor = CombatVfxStyle.WithAlpha(activeColor, 0.8f * opacity);
        spiralA.endColor = CombatVfxStyle.WithAlpha(activeColor, 0.08f * opacity);
        spiralB.startColor = CombatVfxStyle.WithAlpha(activeColor, 0.18f * opacity);
        spiralB.endColor = CombatVfxStyle.WithAlpha(activeColor, 0.75f * opacity);
        spiralA.enabled = spiralB.enabled = true;

        Quaternion plane = Quaternion.LookRotation(axis);
        CombatVfxStyle.SetRing(nearCollar, axis * 0.045f,
            plane, 0.065f, 32, Time.time * 75f, 285f);
        CombatVfxStyle.SetRing(farCollar, axis * 0.24f,
            plane, 0.07f, 32, -Time.time * 95f, 240f);
        nearCollar.startColor = CombatVfxStyle.WithAlpha(activeColor, 0.68f * opacity);
        nearCollar.endColor = CombatVfxStyle.WithAlpha(activeColor, 0.03f);
        farCollar.startColor = CombatVfxStyle.WithAlpha(activeColor, 0.46f * opacity);
        farCollar.endColor = CombatVfxStyle.WithAlpha(activeColor, 0.02f);
    }

    private void OnDestroy()
    {
        if (lineMaterial != null)
        {
            Destroy(lineMaterial);
        }
    }
}
