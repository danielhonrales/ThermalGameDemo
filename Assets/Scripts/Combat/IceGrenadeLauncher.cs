using UnityEngine;

[DisallowMultipleComponent]
public sealed class IceGrenadeLauncher : MonoBehaviour
{
    [Header("Aim")]
    [SerializeField] private Transform palmOrigin;
    [SerializeField] private Transform headset;
    [SerializeField] private PalmBeamShooter.LocalAxis throwAxis = PalmBeamShooter.LocalAxis.Forward;
    [SerializeField] private PalmBeamShooter.LocalAxis aimUpAxis = PalmBeamShooter.LocalAxis.Up;
    [SerializeField] private Vector3 localOriginOffset;
    [SerializeField] private float worldUpOriginOffset = 0.08f;
    [SerializeField, Range(0f, 1f)] private float headAimBlend = 0.65f;

    [Header("Throw")]
    [SerializeField] private Transform floorReference;
    [SerializeField] private float floorLocalHeight = -0.4f;
    [SerializeField, Min(0.25f)] private float minThrowDistance = 0.75f;
    [SerializeField, Min(0.5f)] private float maxThrowDistance = 4f;
    [SerializeField] private float lowHandHeightOffsetFromHeadset = -0.45f;
    [SerializeField] private float highHandHeightOffsetFromHeadset = 0.4f;
    [SerializeField] private float lowHandHeightAboveFloor = 0.85f;
    [SerializeField] private float highHandHeightAboveFloor = 1.75f;
    [SerializeField, Min(0.1f)] private float gravityMultiplier = 1f;
    [SerializeField, Min(0.1f)] private float arcPeakHeight = 0.75f;
    [SerializeField, Min(0.05f)] private float minArcPeakHeight = 0.25f;

    [Header("Pose Gate")]
    [SerializeField] private bool requireIronManPose = true;
    [SerializeField] private bool requireTrackedHand = true;
    [SerializeField] private bool requireFingersPointUp = true;
    [SerializeField] private bool requireOpenFingers = true;
    [SerializeField] private OVRHand handTrackingSource;
    [SerializeField] private PalmBeamShooter.LocalAxis fingerUpAxis = PalmBeamShooter.LocalAxis.Forward;
    [SerializeField, Range(-1f, 1f)] private float fingersUpDotThreshold = 0.3f;
    [SerializeField, Range(0f, 1f)] private float maxFingerPinchForOpenPalm = 0.35f;

    [Header("Charge")]
    [SerializeField] private bool requireChargeBeforeThrow = true;
    [SerializeField, Min(0f)] private float chargeSeconds = 0.9f;
    [SerializeField, Min(0f)] private float throwCooldownSeconds = 0.35f;

    [Header("Arc Preview")]
    [SerializeField] private bool showArcPreview = true;
    [SerializeField] private int arcPreviewSteps = 24;
    [SerializeField, Min(0.001f)] private float arcPreviewWidth = 0.018f;
    [SerializeField] private Color arcPreviewColor = new Color(0.55f, 0.78f, 1f, 0.55f);

    [Header("Debug")]
    [SerializeField] private bool logThrows = true;

    private LayerMask collisionMask;
    private IceGrenadeEffects grenadeEffects;
    private IceGrenadeProjectile activeProjectile;
    private LineRenderer arcPreviewLine;
    private NetworkPlayerGrenadeVisual localNetworkGrenade;
    private float poseChargeStartTime = -1f;
    private float nextThrowAllowedTime;
    private bool waitingForPoseReset;

    private void Reset()
    {
        collisionMask = CombatLayers.CombatHitMask;
    }

    private void Awake()
    {
        collisionMask = CombatLayers.CombatHitMask;
        grenadeEffects = GetComponent<IceGrenadeEffects>();
        EnsureProjectile();
        EnsureArcPreview();

        int layer = CombatLayers.BeamLayer;
        if (layer >= 0)
        {
            CombatLayers.SetLayerRecursively(gameObject, layer);
        }
    }

    private void Update()
    {
        if (activeProjectile != null && activeProjectile.IsAlive)
        {
            HideArcPreview();
            return;
        }

        if (palmOrigin == null)
        {
            ResetCharge();
            grenadeEffects?.HideAll();
            HideArcPreview();
            PublishHidden();
            return;
        }

        if (Time.time < nextThrowAllowedTime)
        {
            grenadeEffects?.HideCharge();
            HideArcPreview();
            return;
        }

        if (requireIronManPose && !IsIronManPoseActive())
        {
            waitingForPoseReset = false;
            ResetCharge();
            grenadeEffects?.HideAll();
            HideArcPreview();
            PublishHidden();
            return;
        }

        if (waitingForPoseReset)
        {
            grenadeEffects?.HideCharge();
            HideArcPreview();
            return;
        }

        Vector3 origin = GetThrowOrigin();
        Vector3 launchVelocity = GetLaunchVelocity(origin);

        if (requireChargeBeforeThrow && !IsChargeComplete())
        {
            float progress = GetChargeProgress();
            grenadeEffects?.ShowCharge(origin, GetThrowRotation(launchVelocity), progress);
            UpdateArcPreview(origin, launchVelocity);
            PublishCharge(origin, GetThrowRotation(launchVelocity), progress);
            return;
        }

        ThrowGrenade(origin, launchVelocity);
    }

    public void SetPalmOrigin(Transform newPalmOrigin)
    {
        palmOrigin = newPalmOrigin;
    }

    public void SetHeadset(Transform newHeadset)
    {
        headset = newHeadset;
    }

    private void ThrowGrenade(Vector3 origin, Vector3 launchVelocity)
    {
        grenadeEffects?.HideCharge();
        HideArcPreview();
        PublishHidden();

        EnsureProjectile();
        Transform visual = grenadeEffects != null ? grenadeEffects.CreateGrenadeVisual(activeProjectile.transform) : null;
        float floorY = GetFloorWorldY();
        activeProjectile.Launch(origin, launchVelocity, collisionMask, visual, OnGrenadeExploded, gravityMultiplier, floorY);
        grenadeEffects?.StartFlightTrail(activeProjectile.transform);

        PublishThrow(origin, launchVelocity, floorY);
        ResetCharge();
        waitingForPoseReset = true;
        nextThrowAllowedTime = Time.time + throwCooldownSeconds;

        if (logThrows)
        {
            Debug.Log($"Ice grenade launched from {origin} with velocity {launchVelocity}.", this);
        }
    }

    private void OnGrenadeExploded(Vector3 position, Collider hitCollider)
    {
        grenadeEffects?.StopFlightTrail();
        grenadeEffects?.PlayExplosion(position);
        ApplyExplosionDamage(position);
        PublishExplosion(position);

        if (logThrows)
        {
            string targetName = hitCollider != null ? hitCollider.name : "none";
            Debug.Log($"Ice grenade exploded at {position} (hit: {targetName}).", this);
        }
    }

    private void ApplyExplosionDamage(Vector3 position)
    {
        if (grenadeEffects == null)
        {
            return;
        }

        float radius = grenadeEffects.ExplosionRadius;
        Collider[] hits = Physics.OverlapSphere(position, radius, collisionMask, QueryTriggerInteraction.Ignore);
        foreach (Collider hit in hits)
        {
            if (hit == null)
            {
                continue;
            }

            bool isHeadTarget = hit.GetComponentInParent<HeadshotTarget>() != null
                || hit.gameObject.layer == CombatLayers.HeadTargetLayer;
            if (!isHeadTarget)
            {
                continue;
            }

            NetworkPlayerHealth health = hit.GetComponentInParent<NetworkPlayerHealth>();
            if (health == null || health.IsLocalPlayer)
            {
                continue;
            }

            health.RequestHeadshotDamage();
        }
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

    private bool IsIronManPoseActive()
    {
        if (requireTrackedHand && handTrackingSource != null && !handTrackingSource.IsTracked)
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

    private Vector3 GetThrowOrigin()
    {
        return palmOrigin.position
            + palmOrigin.TransformVector(localOriginOffset)
            + Vector3.up * worldUpOriginOffset;
    }

    private Vector3 GetAimDirection()
    {
        Vector3 palmDirection = GetAxisDirection(palmOrigin, throwAxis);
        Vector3 lookDirection = headset != null ? headset.forward : palmDirection;
        Vector3 blended = Vector3.Slerp(palmDirection.normalized, lookDirection.normalized, headAimBlend);
        if (blended.sqrMagnitude <= 0.0001f)
        {
            return palmDirection.normalized;
        }

        return blended.normalized;
    }

    private Vector3 GetLaunchVelocity(Vector3 origin)
    {
        float throwDistance = GetThrowDistance();
        Vector3 target = GetTargetPoint(origin, throwDistance);
        float gravity = Mathf.Abs(Physics.gravity.y) * gravityMultiplier;
        float distanceT = Mathf.InverseLerp(minThrowDistance, maxThrowDistance, throwDistance);
        float peakHeight = Mathf.Lerp(minArcPeakHeight, arcPeakHeight, distanceT);
        return SolveBallisticVelocity(origin, target, gravity, peakHeight);
    }

    private float GetThrowDistance()
    {
        if (palmOrigin == null)
        {
            return minThrowDistance;
        }

        float handY = palmOrigin.position.y;
        float lowHandY;
        float highHandY;
        if (headset != null)
        {
            float headY = headset.position.y;
            lowHandY = headY + lowHandHeightOffsetFromHeadset;
            highHandY = Mathf.Max(lowHandY + 0.05f, headY + highHandHeightOffsetFromHeadset);
        }
        else
        {
            float floorY = GetFloorWorldY();
            lowHandY = floorY + lowHandHeightAboveFloor;
            highHandY = Mathf.Max(lowHandY + 0.05f, floorY + highHandHeightAboveFloor);
        }

        float heightT = Mathf.InverseLerp(lowHandY, highHandY, handY);
        return Mathf.Lerp(minThrowDistance, maxThrowDistance, heightT);
    }

    private Vector3 GetTargetPoint(Vector3 origin)
    {
        return GetTargetPoint(origin, GetThrowDistance());
    }

    private Vector3 GetTargetPoint(Vector3 origin, float throwDistance)
    {
        Vector3 flatAim = Vector3.ProjectOnPlane(GetAimDirection(), Vector3.up);
        if (flatAim.sqrMagnitude <= 0.0001f)
        {
            flatAim = Vector3.forward;
        }
        else
        {
            flatAim.Normalize();
        }

        float floorY = GetFloorWorldY();
        Vector3 target = origin + flatAim * throwDistance;
        target.y = floorY;
        return target;
    }

    private float GetFloorWorldY()
    {
        Transform reference = floorReference != null ? floorReference : transform;
        return reference.TransformPoint(new Vector3(0f, floorLocalHeight, 0f)).y;
    }

    private static Vector3 SolveBallisticVelocity(Vector3 origin, Vector3 target, float gravity, float peakHeightOffset)
    {
        Vector3 displacement = target - origin;
        Vector3 displacementXZ = new Vector3(displacement.x, 0f, displacement.z);
        float horizontalDistance = displacementXZ.magnitude;
        float peakHeight = Mathf.Max(origin.y + peakHeightOffset, target.y + 0.2f);

        if (horizontalDistance <= 0.05f)
        {
            float upwardSpeed = Mathf.Sqrt(2f * gravity * Mathf.Max(0.15f, peakHeight - origin.y));
            return Vector3.up * upwardSpeed;
        }

        float rise = Mathf.Max(0.05f, peakHeight - origin.y);
        float fall = Mathf.Max(0.05f, peakHeight - target.y);
        float upwardSpeedComponent = Mathf.Sqrt(2f * gravity * rise);
        float totalTime = upwardSpeedComponent / gravity + Mathf.Sqrt(2f * fall / gravity);
        Vector3 horizontalVelocity = displacementXZ / totalTime;
        return horizontalVelocity + Vector3.up * upwardSpeedComponent;
    }

    private static Quaternion GetThrowRotation(Vector3 launchVelocity)
    {
        if (launchVelocity.sqrMagnitude <= 0.0001f)
        {
            return Quaternion.identity;
        }

        return Quaternion.LookRotation(launchVelocity.normalized, Vector3.up);
    }

    private void UpdateArcPreview(Vector3 origin, Vector3 launchVelocity)
    {
        if (!showArcPreview)
        {
            HideArcPreview();
            return;
        }

        EnsureArcPreview();
        arcPreviewLine.enabled = true;

        Vector3 gravity = Physics.gravity * gravityMultiplier;
        float floorY = GetFloorWorldY();
        float timeStep = 0.06f;
        Vector3 position = origin;
        Vector3 velocity = launchVelocity;
        int pointIndex = 0;

        for (int i = 0; i < arcPreviewSteps; i++)
        {
            arcPreviewLine.SetPosition(pointIndex++, position);

            velocity += gravity * timeStep;
            Vector3 nextPosition = position + velocity * timeStep;
            if (nextPosition.y <= floorY)
            {
                Vector3 landingPoint = nextPosition;
                landingPoint.y = floorY;
                arcPreviewLine.SetPosition(pointIndex++, landingPoint);
                break;
            }

            position = nextPosition;
        }

        arcPreviewLine.positionCount = pointIndex;
    }

    private void HideArcPreview()
    {
        if (arcPreviewLine != null)
        {
            arcPreviewLine.enabled = false;
        }
    }

    private void EnsureProjectile()
    {
        if (activeProjectile != null)
        {
            return;
        }

        GameObject projectileObject = new GameObject("IceGrenadeProjectile");
        projectileObject.transform.SetParent(transform, false);
        activeProjectile = projectileObject.AddComponent<IceGrenadeProjectile>();
        projectileObject.SetActive(false);
    }

    private void EnsureArcPreview()
    {
        if (arcPreviewLine != null)
        {
            return;
        }

        GameObject lineObject = new GameObject("IceArcPreview");
        lineObject.transform.SetParent(transform, false);
        arcPreviewLine = lineObject.AddComponent<LineRenderer>();
        arcPreviewLine.useWorldSpace = true;
        arcPreviewLine.widthMultiplier = arcPreviewWidth;
        arcPreviewLine.numCapVertices = 6;
        arcPreviewLine.material = new Material(Shader.Find("Sprites/Default"));
        arcPreviewLine.startColor = arcPreviewColor;
        arcPreviewLine.endColor = WithAlpha(arcPreviewColor, 0.08f);
        arcPreviewLine.enabled = false;
    }

    private void PublishCharge(Vector3 position, Quaternion rotation, float progress)
    {
        NetworkPlayerGrenadeVisual grenadeVisual = FindLocalNetworkGrenade();
        grenadeVisual?.SubmitCharge(position, rotation, progress);
    }

    private void PublishThrow(Vector3 origin, Vector3 launchVelocity, float floorWorldY)
    {
        NetworkPlayerGrenadeVisual grenadeVisual = FindLocalNetworkGrenade();
        grenadeVisual?.SubmitThrow(origin, launchVelocity, floorWorldY);
    }

    private void PublishExplosion(Vector3 position)
    {
        NetworkPlayerGrenadeVisual grenadeVisual = FindLocalNetworkGrenade();
        grenadeVisual?.SubmitExplosion(position);
    }

    private void PublishHidden()
    {
        NetworkPlayerGrenadeVisual grenadeVisual = FindLocalNetworkGrenade();
        grenadeVisual?.HideGrenade();
    }

    private NetworkPlayerGrenadeVisual FindLocalNetworkGrenade()
    {
        if (localNetworkGrenade != null && localNetworkGrenade.IsLocalPlayer)
        {
            return localNetworkGrenade;
        }

        NetworkPlayerGrenadeVisual[] grenadeVisuals = FindObjectsByType<NetworkPlayerGrenadeVisual>(FindObjectsSortMode.None);
        foreach (NetworkPlayerGrenadeVisual grenadeVisual in grenadeVisuals)
        {
            if (grenadeVisual.IsLocalPlayer)
            {
                localNetworkGrenade = grenadeVisual;
                return localNetworkGrenade;
            }
        }

        return null;
    }

    private static Vector3 GetAxisDirection(Transform source, PalmBeamShooter.LocalAxis axis)
    {
        return axis switch
        {
            PalmBeamShooter.LocalAxis.Forward => source.forward,
            PalmBeamShooter.LocalAxis.Back => -source.forward,
            PalmBeamShooter.LocalAxis.Up => source.up,
            PalmBeamShooter.LocalAxis.Down => -source.up,
            PalmBeamShooter.LocalAxis.Right => source.right,
            PalmBeamShooter.LocalAxis.Left => -source.right,
            _ => source.forward
        };
    }

    private static Color WithAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}
