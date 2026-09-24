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
    private Material lineMaterial;
    private float completionStartedAt = -1f;
    private bool showingProgress;
    private Color activeColor;

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
        effectRoot.gameObject.SetActive(true);
        effectRoot.SetPositionAndRotation(worldPosition, worldRotation);

        float eased = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(progress));
        DrawArc(sweep, ringRadius, Mathf.Lerp(12f, 290f, eased), -90f, 0.18f + eased * 0.52f);
        DrawArc(trace, ringRadius * 0.76f, Mathf.Lerp(5f, 155f, eased), 90f, 0.12f + eased * 0.16f);
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
        effectRoot.gameObject.SetActive(true);
        effectRoot.SetPositionAndRotation(worldPosition, worldRotation);
        completionStartedAt = Time.time;
        DrawArc(sweep, ringRadius, 320f, -90f, 0.6f);
        DrawArc(trace, ringRadius * 0.77f, 160f, 90f, 0.22f);
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

    private void OnDestroy()
    {
        if (lineMaterial != null)
        {
            Destroy(lineMaterial);
        }
    }
}
