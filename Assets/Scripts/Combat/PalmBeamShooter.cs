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
    [SerializeField] private LocalAxis rayAxis = LocalAxis.Forward;
    [SerializeField] private Vector3 localOriginOffset;
    [SerializeField] private float worldUpOriginOffset = 0.08f;
    [SerializeField, Min(0.1f)] private float maxDistance = 20f;
    [SerializeField] private bool fireContinuously = true;

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
    [SerializeField, Min(0f)] private float chargeSeconds = 2f;
    [SerializeField] private Color chargeColor = new Color(1f, 0.25f, 0.05f, 1f);

    [Header("Visuals")]
    [SerializeField] private LineRenderer beamLine;
    [SerializeField] private float lineWidth = 0.025f;
    [SerializeField] private Color missColor = new Color(0.15f, 0.8f, 1f, 1f);
    [SerializeField] private Color blockedColor = new Color(1f, 0.85f, 0.1f, 1f);
    [SerializeField] private Color headshotColor = new Color(1f, 0.05f, 0.05f, 1f);

    [Header("Debug")]
    [SerializeField] private bool logHits = true;

    private LayerMask combatMask;
    private int lastHitInstanceId;
    private string lastResult;
    private NetworkPlayerBeamVisual localNetworkBeam;
    private ThermalBeamEffects beamEffects;
    private float poseChargeStartTime = -1f;

    private void Reset()
    {
        combatMask = CombatLayers.CombatHitMask;
    }

    private void Awake()
    {
        combatMask = CombatLayers.CombatHitMask;
        beamEffects = GetComponent<ThermalBeamEffects>();
        EnsureLineRenderer();

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
            SetBeamVisible(false);
            PublishNetworkBeamHidden();
            ResetCharge();
            return;
        }

        FireBeam();
    }

    [ContextMenu("Fire Beam Once")]
    public void FireBeam()
    {
        if (palmOrigin == null)
        {
            SetBeamVisible(false);
            PublishNetworkBeamHidden();
            ResetCharge();
            return;
        }

        if (requireIronManPose && !IsIronManPoseActive())
        {
            SetBeamVisible(false);
            PublishNetworkBeamHidden();
            ResetCharge();
            return;
        }

        Vector3 origin = GetBeamOrigin();
        Vector3 direction = GetAxisDirection(palmOrigin, rayAxis);

        if (requireChargeBeforeFire && !IsChargeComplete())
        {
            SetRayVisualOnly(false);
            PublishNetworkBeamHidden();

            if (beamEffects != null)
            {
                beamEffects.ShowCharge(origin, chargeColor);
            }

            PublishNetworkCharge(origin, chargeColor);
            return;
        }

        Vector3 end = origin + direction * maxDistance;
        Color beamColor = missColor;
        string result = "Miss";
        Collider hitCollider = null;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, combatMask, QueryTriggerInteraction.Ignore))
        {
            end = hit.point;
            hitCollider = hit.collider;

            if (hit.collider.GetComponentInParent<HeadshotTarget>() != null || hit.collider.gameObject.layer == CombatLayers.HeadTargetLayer)
            {
                result = "Headshot";
                beamColor = headshotColor;
                ApplyHeadshotDamage(hit.collider);
            }
            else if (hit.collider.GetComponentInParent<GameplayCoverMarker>() != null || hit.collider.gameObject.layer == CombatLayers.GameplayCoverLayer)
            {
                result = "Blocked";
                beamColor = blockedColor;
            }
        }

        DrawBeam(origin, end, beamColor);
        if (beamEffects != null)
        {
            beamEffects.ShowBeam(origin, end, beamColor, hitCollider != null);
        }
        PublishNetworkBeam(origin, end, beamColor);
        LogResultIfChanged(result, hitCollider);
    }

    public void SetPalmOrigin(Transform newPalmOrigin)
    {
        palmOrigin = newPalmOrigin;
    }

    public void SetHeadset(Transform newHeadset)
    {
        headset = newHeadset;
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

    private void PublishNetworkBeam(Vector3 start, Vector3 end, Color color)
    {
        NetworkPlayerBeamVisual beamVisual = FindLocalNetworkBeam();
        if (beamVisual != null)
        {
            beamVisual.SubmitBeam(start, end, color);
        }
    }

    private void PublishNetworkCharge(Vector3 position, Color color)
    {
        NetworkPlayerBeamVisual beamVisual = FindLocalNetworkBeam();
        if (beamVisual != null)
        {
            beamVisual.SubmitCharge(position, color);
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

    private void ResetCharge()
    {
        poseChargeStartTime = -1f;
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
        beamLine.enabled = true;
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
