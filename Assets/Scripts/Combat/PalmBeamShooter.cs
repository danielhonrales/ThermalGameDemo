using UnityEngine;

[DisallowMultipleComponent]
public sealed class PalmBeamShooter : MonoBehaviour
{
    public enum LocalAxis
    {
        Forward,
        Back,
        Up,
        Down,
        Right,
        Left
    }

    [Header("Aim")]
    [SerializeField] private Transform palmOrigin;
    [SerializeField] private Transform forearmMount;
    [SerializeField] private LocalAxis rayAxis = LocalAxis.Forward;
    [SerializeField] private Vector3 localOriginOffset;
    [SerializeField] private float worldUpOriginOffset = 0.08f;
    [SerializeField, Min(0.1f)] private float maxDistance = 100f;
    [SerializeField, Min(1f)] private float visualRayDistance = 500f;
    [SerializeField] private bool fireContinuously = true;

    [Header("Aim Guide")]
    [SerializeField] private bool showAimGuide = true;
    [Tooltip("Keeps the targeting line and impact marker hidden while the palm is charging. The actual beam is always hidden until firing begins.")]
    [SerializeField] private bool showAimGuideWhileCharging;
    [SerializeField] private LineRenderer aimGuideLine;
    [SerializeField, Min(0.001f)] private float aimGuideWidth = 0.014f;
    [SerializeField] private Color aimGuideColor = new Color(1f, 0.55f, 0.12f, 0.38f);
    [SerializeField] private Color aimGuideHitColor = new Color(1f, 0.88f, 0.45f, 0.92f);

    [Header("Optional Iron Man Pose Gate")]
    [SerializeField] private bool requireIronManPose = true;
    [SerializeField] private bool requirePalmFacingAwayFromHead = true;
    [SerializeField] private bool requireFingersPointUp = true;
    [SerializeField] private bool requireTrackedHand = true;
    [SerializeField] private bool requireOpenFingers = true;
    [SerializeField] private OVRHand handTrackingSource;
    [SerializeField] private Transform headset;
    [SerializeField] private LocalAxis palmFacingAxis = LocalAxis.Back;
    [SerializeField] private LocalAxis fingerUpAxis = LocalAxis.Forward;
    [SerializeField, Range(-1f, 1f)] private float awayFromHeadDotThreshold = 0.05f;
    [SerializeField, Range(-1f, 1f)] private float fingersUpDotThreshold = 0.3f;
    [SerializeField, Range(0f, 1f)] private float maxFingerPinchForOpenPalm = 0.35f;

    [Header("Charge")]
    [SerializeField] private bool requireChargeBeforeFire = true;
    [SerializeField, Min(0f)] private float chargeSeconds = 0.75f;
    [SerializeField] private Color chargeColor = new Color(1f, 0.25f, 0.05f, 1f);

    [Header("Beam Burst")]
    [SerializeField, Min(0f)] private float maxBeamDurationSeconds = 3f;
    [SerializeField, Min(0f)] private float beamCooldownSeconds = 2f;

    [Header("Visuals")]
    [SerializeField] private bool showSimpleBeamLine;
    [SerializeField] private LineRenderer beamLine;
    [SerializeField] private float lineWidth = 0.025f;
    [SerializeField] private Color missColor = new Color(1f, 0.48f, 0.08f, 1f);
    [SerializeField] private Color blockedColor = new Color(1f, 0.85f, 0.1f, 1f);
    [SerializeField] private Color shieldBlockedColor = new Color(0.72f, 0.2f, 1f, 1f);
    [SerializeField] private Color headshotColor = new Color(1f, 0.12f, 0.02f, 1f);

    [Header("Debug")]
    [SerializeField] private bool logHits = true;

    private LayerMask combatMask;
    private int lastHitInstanceId;
    private string lastResult;
    private NetworkPlayerBeamVisual localNetworkBeam;
    private ThermalBeamEffects beamEffects;
    private float poseChargeStartTime = -1f;
    private float beamBurstStartTime = -1f;
    private float nextAllowedFireTime;
    private ForearmShieldController shieldController;
    private bool wasAttackPoseActive;

    private void Reset()
    {
        combatMask = CombatLayers.CombatHitMask;
    }

    private void Awake()
    {
        combatMask = CombatLayers.CombatHitMask;
        beamEffects = GetComponent<ThermalBeamEffects>();
        shieldController = GetComponent<ForearmShieldController>();
        EnsureLineRenderer();
        EnsureAimGuideLine();

        int layer = CombatLayers.BeamLayer;
        if (layer >= 0)
        {
            CombatLayers.SetLayerRecursively(gameObject, layer);
        }
    }

    private void Update()
    {
        if (!fireContinuously)
        {
            StopBeamBurst(false);
            return;
        }

        FireBeam();
    }

    [ContextMenu("Fire Beam Once")]
    public void FireBeam()
    {
        if (palmOrigin == null)
        {
            StopBeamBurst(false);
            return;
        }

        bool attackPoseActive = !requireIronManPose || IsIronManPoseActive();
        if (attackPoseActive && !wasAttackPoseActive)
        {
            shieldController?.CancelForAttack();
        }

        wasAttackPoseActive = attackPoseActive;

        if (Time.time < nextAllowedFireTime)
        {
            StopBeamBurst(false);
            return;
        }

        if (shieldController != null && shieldController.IsShieldActive)
        {
            StopBeamBurst(false);
            return;
        }

        if (requireIronManPose && !attackPoseActive)
        {
            StopBeamBurst(beamBurstStartTime >= 0f);
            return;
        }

        Vector3 origin = GetBeamOrigin();
        Vector3 direction = GetAxisDirection(palmOrigin, rayAxis);
        if (direction.sqrMagnitude <= 0.0001f)
        {
            StopBeamBurst(false);
            return;
        }

        direction.Normalize();
        ResolveBeamHits(origin, direction, out Vector3 beamEnd, out bool hitSomething, out Color beamColor, out string result, out Collider hitCollider);

        if (requireChargeBeforeFire && !IsChargeComplete())
        {
            SetRayVisualOnly(false);
            PublishNetworkBeamHidden();
            if (showAimGuideWhileCharging)
            {
                ShowAimGuide(origin, beamEnd, hitSomething ? beamColor : aimGuideHitColor);
            }
            else
            {
                HideAimGuide();
            }

            if (beamEffects != null)
            {
                Vector3 chargeOrigin = GetBeamOrigin();
                beamEffects.ShowCharge(chargeOrigin, chargeColor, GetChargeProgress(), chargeOrigin, GetChargeRotation());
            }

            PublishNetworkCharge(GetBeamOrigin(), GetChargeRotation(), chargeColor, GetChargeProgress());
            return;
        }

        if (beamBurstStartTime < 0f)
        {
            beamBurstStartTime = Time.time;
        }

        if (Time.time - beamBurstStartTime >= maxBeamDurationSeconds)
        {
            StopBeamBurst(true);
            return;
        }

        ShowAimGuide(origin, beamEnd, hitSomething ? beamColor : aimGuideHitColor);
        DrawBeam(origin, beamEnd, beamColor);
        if (beamEffects != null)
        {
            beamEffects.ShowBeam(origin, beamEnd, beamColor, hitSomething, beamEnd, GetEffectMountPosition(), GetEffectMountRotation());
        }

        PublishNetworkBeam(origin, beamEnd, beamColor, hitSomething, beamEnd, GetEffectMountPosition(), GetEffectMountRotation());
        LogResultIfChanged(result, hitCollider);
    }

    private void ResolveBeamHits(
        Vector3 origin,
        Vector3 direction,
        out Vector3 beamEnd,
        out bool hitSomething,
        out Color beamColor,
        out string result,
        out Collider hitCollider)
    {
        float rayDistance = GetRayDistance();
        hitSomething = false;
        hitCollider = null;
        result = "Miss";
        beamColor = missColor;
        beamEnd = origin + direction * rayDistance;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, rayDistance, combatMask, QueryTriggerInteraction.Ignore))
        {
            hitSomething = true;
            beamEnd = hit.point;
            hitCollider = hit.collider;
            beamColor = ClassifyHit(hit.collider, out result);
            ApplyCombatResult(hit.collider, result);
        }
    }

    private float GetRayDistance()
    {
        return Mathf.Max(visualRayDistance, maxDistance);
    }

    private Color ClassifyHit(Collider collider, out string result)
    {
        NetworkPlayerHealth health = collider.GetComponentInParent<NetworkPlayerHealth>();
        bool isHeadTarget = collider.GetComponentInParent<HeadshotTarget>() != null
            || collider.gameObject.layer == CombatLayers.HeadTargetLayer;

        if (isHeadTarget && health != null && health.IsShieldActive)
        {
            result = "Shield";
            return shieldBlockedColor;
        }

        if (isHeadTarget)
        {
            result = "Headshot";
            return headshotColor;
        }

        if (collider.GetComponentInParent<GameplayCoverMarker>() != null || collider.gameObject.layer == CombatLayers.GameplayCoverLayer)
        {
            result = "Blocked";
            return blockedColor;
        }

        result = "Hit";
        return missColor;
    }

    private void ApplyCombatResult(Collider hitCollider, string result)
    {
        if (result != "Headshot")
        {
            return;
        }

        ApplyHeadshotDamage(hitCollider);
    }

    public void SetPalmOrigin(Transform newPalmOrigin)
    {
        palmOrigin = newPalmOrigin;
    }

    public void SetHeadset(Transform newHeadset)
    {
        headset = newHeadset;
    }

    public void SetForearmMount(Transform newForearmMount)
    {
        forearmMount = newForearmMount;
    }

    public bool IsAttackPoseActive()
    {
        return !requireIronManPose || IsIronManPoseActive();
    }

    public bool IsAttackEngaged => beamBurstStartTime >= 0f || poseChargeStartTime >= 0f;

    public void CancelForShield()
    {
        StopBeamBurst(false);
        wasAttackPoseActive = false;
    }

    /// <summary>Immediately clears the beam/charge visuals when another weapon takes over.</summary>
    public void CancelWeaponVisuals()
    {
        StopBeamBurst(false);
        SetRayVisualOnly(false);
        beamEffects?.HideBeam();
        wasAttackPoseActive = false;
    }

    private void OnDisable()
    {
        CancelWeaponVisuals();
    }

    private void ApplyHeadshotDamage(Collider hitCollider)
    {
        NetworkPlayerHealth health = hitCollider.GetComponentInParent<NetworkPlayerHealth>();
        if (health == null || health.IsLocalPlayer)
        {
            return;
        }

        health.RequestHeadshotDamage();
    }

    private void PublishNetworkBeam(Vector3 start, Vector3 end, Color color, bool hitSomething, Vector3 hitPoint, Vector3 mountPosition, Quaternion mountRotation)
    {
        NetworkPlayerBeamVisual beamVisual = FindLocalNetworkBeam();
        if (beamVisual != null)
        {
            beamVisual.SubmitBeam(start, end, color, hitSomething, hitPoint, mountPosition, mountRotation);
        }
    }

    private void PublishNetworkCharge(Vector3 position, Quaternion rotation, Color color, float progress)
    {
        NetworkPlayerBeamVisual beamVisual = FindLocalNetworkBeam();
        if (beamVisual != null)
        {
            beamVisual.SubmitCharge(position, rotation, color, progress);
        }
    }

    private void PublishNetworkBeamHidden()
    {
        NetworkPlayerBeamVisual beamVisual = FindLocalNetworkBeam();
        if (beamVisual != null)
        {
            beamVisual.HideBeam();
        }
    }

    private NetworkPlayerBeamVisual FindLocalNetworkBeam()
    {
        if (localNetworkBeam != null && localNetworkBeam.IsLocalPlayer)
        {
            return localNetworkBeam;
        }

        NetworkPlayerBeamVisual[] beamVisuals = FindObjectsByType<NetworkPlayerBeamVisual>(FindObjectsSortMode.None);
        foreach (NetworkPlayerBeamVisual beamVisual in beamVisuals)
        {
            if (beamVisual.IsLocalPlayer)
            {
                localNetworkBeam = beamVisual;
                return localNetworkBeam;
            }
        }

        return null;
    }

    private bool IsChargeComplete()
    {
        if (chargeSeconds <= 0f)
        {
            return true;
        }

        if (poseChargeStartTime < 0f)
        {
            poseChargeStartTime = Time.time;
            if (logHits)
            {
                Debug.Log($"Palm beam charging for {chargeSeconds:0.00}s.", this);
            }
        }

        return Time.time - poseChargeStartTime >= chargeSeconds;
    }

    private float GetChargeProgress()
    {
        if (chargeSeconds <= 0f || poseChargeStartTime < 0f)
        {
            return 1f;
        }

        return Mathf.Clamp01((Time.time - poseChargeStartTime) / chargeSeconds);
    }

    private void ResetCharge()
    {
        poseChargeStartTime = -1f;
    }

    private void StopBeamBurst(bool startCooldown)
    {
        SetBeamVisible(false);
        HideAimGuide();
        PublishNetworkBeamHidden();
        ResetCharge();
        beamBurstStartTime = -1f;

        if (startCooldown)
        {
            nextAllowedFireTime = Time.time + beamCooldownSeconds;
        }
    }

    private void ShowAimGuide(Vector3 origin, Vector3 beamEnd, Color endMarkerColor)
    {
        if (!showAimGuide)
        {
            HideAimGuide();
            return;
        }

        EnsureAimGuideLine();
        aimGuideLine.enabled = true;
        aimGuideLine.startColor = aimGuideColor;
        aimGuideLine.endColor = aimGuideHitColor;
        aimGuideLine.SetPosition(0, origin);
        aimGuideLine.SetPosition(1, beamEnd);
        beamEffects?.ShowAimMarker(beamEnd, endMarkerColor);
        beamEffects?.HideRangeLimitMarker();
    }

    private void HideAimGuide()
    {
        if (aimGuideLine != null)
        {
            aimGuideLine.enabled = false;
        }

        beamEffects?.HideAimMarker();
        beamEffects?.HideRangeLimitMarker();
    }

    private void EnsureAimGuideLine()
    {
        if (aimGuideLine != null)
        {
            ConfigureAimGuideLine();
            return;
        }

        GameObject lineObject = new GameObject("AimGuideLine");
        lineObject.transform.SetParent(transform, false);
        aimGuideLine = lineObject.AddComponent<LineRenderer>();
        ConfigureAimGuideLine();
    }

    private void ConfigureAimGuideLine()
    {
        aimGuideLine.positionCount = 2;
        aimGuideLine.useWorldSpace = true;
        aimGuideLine.startWidth = aimGuideWidth;
        aimGuideLine.endWidth = aimGuideWidth * 1.35f;
        aimGuideLine.numCapVertices = 6;
        aimGuideLine.alignment = LineAlignment.View;
        aimGuideLine.material = new Material(Shader.Find("Sprites/Default"));
        aimGuideLine.enabled = false;
    }

    private bool IsIronManPoseActive()
    {
        if (requireTrackedHand && handTrackingSource != null && !handTrackingSource.IsTracked)
        {
            return false;
        }

        if (requirePalmFacingAwayFromHead && !IsPalmFacingAwayFromHead())
        {
            return false;
        }

        if (requireFingersPointUp && !AreFingersPointingUp())
        {
            return false;
        }

        if (requireOpenFingers && handTrackingSource != null && !IsOpenPalm())
        {
            return false;
        }

        return true;
    }

    private bool IsPalmFacingAwayFromHead()
    {
        if (headset == null)
        {
            return true;
        }

        Vector3 palmDirection = GetAxisDirection(palmOrigin, palmFacingAxis);
        Vector3 awayFromHead = (palmOrigin.position - headset.position).normalized;
        return Vector3.Dot(palmDirection, awayFromHead) >= awayFromHeadDotThreshold;
    }

    private bool AreFingersPointingUp()
    {
        Vector3 fingerDirection = GetAxisDirection(palmOrigin, fingerUpAxis);
        return Vector3.Dot(fingerDirection, Vector3.up) >= fingersUpDotThreshold;
    }

    private bool IsOpenPalm()
    {
        return handTrackingSource.GetFingerPinchStrength(OVRHand.HandFinger.Index) <= maxFingerPinchForOpenPalm
            && handTrackingSource.GetFingerPinchStrength(OVRHand.HandFinger.Middle) <= maxFingerPinchForOpenPalm
            && handTrackingSource.GetFingerPinchStrength(OVRHand.HandFinger.Ring) <= maxFingerPinchForOpenPalm
            && handTrackingSource.GetFingerPinchStrength(OVRHand.HandFinger.Pinky) <= maxFingerPinchForOpenPalm;
    }

    private static Vector3 GetAxisDirection(Transform source, LocalAxis axis)
    {
        return axis switch
        {
            LocalAxis.Forward => source.forward,
            LocalAxis.Back => -source.forward,
            LocalAxis.Up => source.up,
            LocalAxis.Down => -source.up,
            LocalAxis.Right => source.right,
            LocalAxis.Left => -source.right,
            _ => source.forward
        };
    }

    private Vector3 GetBeamOrigin()
    {
        return palmOrigin.position
            + palmOrigin.TransformVector(localOriginOffset)
            + Vector3.up * worldUpOriginOffset;
    }

    private Vector3 GetEffectMountPosition()
    {
        Transform mount = forearmMount != null ? forearmMount : palmOrigin;
        return mount != null ? mount.position : GetBeamOrigin();
    }

    private Quaternion GetEffectMountRotation()
    {
        Transform mount = forearmMount != null ? forearmMount : palmOrigin;
        return mount != null ? mount.rotation : Quaternion.identity;
    }

    private Quaternion GetChargeRotation()
    {
        if (palmOrigin == null)
        {
            return Quaternion.identity;
        }

        Vector3 direction = GetAxisDirection(palmOrigin, rayAxis);
        Vector3 up = GetAxisDirection(palmOrigin, fingerUpAxis);
        if (direction.sqrMagnitude <= 0.0001f)
        {
            return palmOrigin.rotation;
        }

        return Quaternion.LookRotation(direction, up);
    }

    private void EnsureLineRenderer()
    {
        if (beamLine != null)
        {
            ConfigureLineRenderer();
            return;
        }

        GameObject lineObject = new GameObject("BeamLine");
        lineObject.transform.SetParent(transform, false);
        beamLine = lineObject.AddComponent<LineRenderer>();
        ConfigureLineRenderer();
    }

    private void ConfigureLineRenderer()
    {
        beamLine.positionCount = 2;
        beamLine.useWorldSpace = true;
        beamLine.startWidth = lineWidth;
        beamLine.endWidth = lineWidth;
        beamLine.material = new Material(Shader.Find("Sprites/Default"));
        SetBeamVisible(false);
    }

    private void DrawBeam(Vector3 start, Vector3 end, Color color)
    {
        EnsureLineRenderer();
        beamLine.enabled = showSimpleBeamLine;
        beamLine.startColor = color;
        beamLine.endColor = color;
        beamLine.SetPosition(0, start);
        beamLine.SetPosition(1, end);
    }

    private void SetBeamVisible(bool visible)
    {
        if (beamLine != null)
        {
            beamLine.enabled = visible;
        }

        if (!visible && beamEffects != null)
        {
            beamEffects.HideBeam();
        }
    }

    private void SetRayVisualOnly(bool visible)
    {
        if (beamLine != null)
        {
            beamLine.enabled = visible;
        }
    }

    private void LogResultIfChanged(string result, Collider hitCollider)
    {
        if (!logHits)
        {
            return;
        }

        int hitInstanceId = hitCollider != null ? hitCollider.GetInstanceID() : 0;
        if (result == lastResult && hitInstanceId == lastHitInstanceId)
        {
            return;
        }

        lastResult = result;
        lastHitInstanceId = hitInstanceId;

        string targetName = hitCollider != null ? hitCollider.name : "none";
        Debug.Log($"Palm beam result: {result} ({targetName})", hitCollider);
    }

    private void OnDrawGizmosSelected()
    {
        if (palmOrigin == null)
        {
            return;
        }

        Gizmos.color = Color.cyan;
        Vector3 origin = GetBeamOrigin();
        Gizmos.DrawRay(origin, GetAxisDirection(palmOrigin, rayAxis) * Mathf.Min(maxDistance, 2f));

        Gizmos.color = Color.green;
        Gizmos.DrawRay(origin, GetAxisDirection(palmOrigin, fingerUpAxis) * 0.35f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(origin, GetAxisDirection(palmOrigin, palmFacingAxis) * 0.35f);
    }
}
