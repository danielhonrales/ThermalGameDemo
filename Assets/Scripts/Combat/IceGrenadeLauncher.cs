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
    [SerializeField] private bool useGazeTargeting = true;

    [Header("Throw")]
    [SerializeField] private Transform floorReference;
    [SerializeField] private float floorLocalHeight = -0.4f;
    [SerializeField, Min(0.25f)] private float minThrowDistance = 0.50f;
    [SerializeField, Min(0.5f)] private float maxThrowDistance = 4f;
    [SerializeField] private float lowHandHeightOffsetFromHeadset = -0.55f;
    [SerializeField] private float highHandHeightOffsetFromHeadset = -0.20f;
    [SerializeField] private float lowHandHeightAboveFloor = 0.85f;
    [SerializeField] private float highHandHeightAboveFloor = 1.75f;
    [SerializeField, Range(1f, 3f)] private float throwHeightExponent = 1.6f;
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
    [SerializeField, Min(0f)] private float chargeSeconds = 2f;
    [SerializeField, Min(0f)] private float throwCooldownSeconds = 0.35f;

    [Header("Arc Preview")]
    [SerializeField] private bool showArcPreview = true;
    [SerializeField] private int arcPreviewSteps = 48;
    [SerializeField, Min(0.001f)] private float arcPreviewWidth = 0.012f;
    [SerializeField] private Color arcPreviewColor = new Color(0.31f, 0.79f, 0.9f, 0.42f);

    [Header("Debug")]
    [SerializeField] private bool logThrows = true;

    private LayerMask collisionMask;
    private IceGrenadeEffects grenadeEffects;
    private IceGrenadeProjectile activeProjectile;
    private LineRenderer arcPreviewLine;
    private LineRenderer arcPreviewPulse;
    private LineRenderer landingMarker;
    private LineRenderer landingCenter;
    private readonly Vector3[] predictedPath = new Vector3[512];
    private NetworkPlayerGrenadeVisual localNetworkGrenade;
    private float poseChargeStartTime = -1f;
    private bool hardwareLeadSent;
    private float nextThrowAllowedTime;
    private HandPoseRouter poseRouter;
    private bool hasAimTarget;
    private Vector3 smoothedAimTarget;
    private bool hasWristReference;
    private Quaternion wristReferenceHeading;
    private Quaternion neutralWristRotation;
    private float wristSteeringDegrees;
    private IceGrenadeTrajectory.Contact predictedLanding;

    private void Reset()
    {
        collisionMask = CombatLayers.CombatHitMask;
    }

    private void Awake()
    {
        collisionMask = CombatLayers.CombatHitMask;
        grenadeEffects = GetComponent<IceGrenadeEffects>();
        poseRouter = GetComponent<HandPoseRouter>();
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
        // A held palm begins another visible charge after the short throw gap.
        bool projectileInFlight = activeProjectile != null && activeProjectile.IsAlive;

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
            ResetCharge();
            grenadeEffects?.HideAll();
            HideArcPreview();
            PublishHidden();
            return;
        }

        Vector3 origin = GetThrowOrigin();
        Vector3 launchVelocity = GetLaunchVelocity(origin);

        if ((requireChargeBeforeThrow && !IsChargeComplete()) || projectileInFlight
            || (poseRouter != null && !poseRouter.IsIcePose))
        {
            CombatEventOutput.State("ice_charge", poseRouter == null || poseRouter.IsIcePose);
            // Signal the Pi one hardware lead before release so the cold pulse peaks with the throw.
            if ((poseRouter == null || poseRouter.IsIcePose) && poseChargeStartTime >= 0f && !hardwareLeadSent
                && Time.time - poseChargeStartTime >= chargeSeconds - CombatEventOutput.HardwareLeadSeconds)
            {
                hardwareLeadSent = true;
                CombatEventOutput.Emit("ice_shot", "ice");
            }
            float progress = GetChargeProgress();
            Vector3 chargePosition = origin;
            Quaternion chargeRotation = GetThrowRotation(launchVelocity);
            if (poseRouter != null && poseRouter.TryGetForearmPose(headset, out Vector3 wrist, out Quaternion armRotation))
            { chargePosition = wrist; chargeRotation = armRotation; }
            grenadeEffects?.ShowCharge(chargePosition, chargeRotation, progress, poseRouter == null || poseRouter.IsIcePose);
            UpdateArcPreview(origin, launchVelocity);
            if (poseRouter == null || poseRouter.IsIcePose) PublishCharge(chargePosition, chargeRotation, progress);
            else PublishHidden();
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

    /// <summary>Immediately clears charge, arc, grenade, and explosion feedback on a weapon-mode change.</summary>
    public void CancelWeaponVisuals()
    {
        ResetCharge();
        HideArcPreview();
        grenadeEffects?.CancelAllWeaponVisuals();
        if (activeProjectile != null && activeProjectile.IsAlive)
        {
            activeProjectile.Cancel();
            CombatEventOutput.State("ice_flight", false);
        }

        PublishHidden();
    }

    private void OnDisable()
    {
        CancelWeaponVisuals();
    }

    private void ThrowGrenade(Vector3 origin, Vector3 launchVelocity)
    {
        grenadeEffects?.HideCharge();
        UpdateArcPreview(origin, launchVelocity);
        HideArcPath();
        PublishHidden();

        EnsureProjectile();
        Transform visual = grenadeEffects != null ? grenadeEffects.CreateGrenadeVisual(activeProjectile.transform) : null;
        float floorY = GetFloorWorldY();
        activeProjectile.Launch(origin, launchVelocity, collisionMask, visual, OnGrenadeExploded, gravityMultiplier, floorY);
        grenadeEffects?.StartFlightTrail(activeProjectile.transform);

        if (!hardwareLeadSent) CombatEventOutput.Emit("ice_shot", "ice");
        hardwareLeadSent = false;
        CombatEventOutput.State("ice_flight", true);
        PublishThrow(origin, launchVelocity, floorY);
        ResetCharge();
        if (landingMarker != null) landingMarker.startColor = landingMarker.endColor = Color.white;
        if (landingCenter != null) landingCenter.startColor = landingCenter.endColor = Color.white;
        nextThrowAllowedTime = Time.time + Mathf.Max(0.55f, throwCooldownSeconds);

        if (logThrows)
        {
            Debug.Log($"Ice grenade launched from {origin} with velocity {launchVelocity}.", this);
        }
    }

    private void OnGrenadeExploded(Vector3 position, Collider hitCollider)
    {
        CombatEventOutput.State("ice_flight", false);
        CombatEventOutput.Emit("ice_impact", hitCollider != null ? "collider" : "floor");
        TraceLanding(position, hitCollider);
        HideArcPreview();
        grenadeEffects?.StopFlightTrail();
        int contactKind = ApplyExplosionDamage(position, out Vector3 contactPoint);
        grenadeEffects?.PlayExplosion(position, contactKind, contactPoint);
        PublishExplosion(position, contactKind, contactPoint);

        if (logThrows)
        {
            string targetName = hitCollider != null ? hitCollider.name : "none";
            Debug.Log($"Ice grenade exploded at {position} (hit: {targetName}).", this);
        }
    }

    private int ApplyExplosionDamage(Vector3 position, out Vector3 contactPoint)
    {
        contactPoint = position;
        if (grenadeEffects == null)
        {
            return IceGrenadeEffects.NoPlayerContact;
        }

        float radius = grenadeEffects.ExplosionRadius;
        Collider[] hits = Physics.OverlapSphere(position, radius, collisionMask, QueryTriggerInteraction.Ignore);
        foreach (Collider hit in hits)
        {
            CoverDrone drone = hit != null ? hit.GetComponentInParent<CoverDrone>() : null;
            if (drone != null) drone.ReportShot(hit.ClosestPoint(position));
        }
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

            contactPoint = hit.bounds.center;
            if (health.IsShieldActive)
            {
                if (FusionRoundDirector.Active()?.AllowsCombat == true) health.RequestHeadshotDamage("ice");
                return IceGrenadeEffects.ShieldContact;
            }

            if (FusionRoundDirector.Active()?.AllowsCombat == true)
                health.RequestHeadshotDamage("ice");
            return IceGrenadeEffects.PlayerContact;
        }

        return IceGrenadeEffects.NoPlayerContact;
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
        CombatEventOutput.State("ice_charge", false);
        if (hardwareLeadSent) CombatEventOutput.Emit("ice_cancel", "ice");
        hardwareLeadSent = false;
        poseChargeStartTime = -1f;
        hasAimTarget = false;
        hasWristReference = false;
        wristSteeringDegrees = 0f;
    }

    private bool IsIronManPoseActive()
    {
        if (poseRouter != null) return poseRouter.FeedbackPose == HandPoseRouter.PoseKind.Ice;
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
        Vector3 target = GetTargetPoint(origin);
        if (useGazeTargeting && headset != null)
        {
            if (!hasAimTarget) smoothedAimTarget = target;
            else if (Vector3.Distance(smoothedAimTarget, target) > 0.025f)
                smoothedAimTarget = Vector3.MoveTowards(smoothedAimTarget,
                    Vector3.Lerp(smoothedAimTarget, target, 1f - Mathf.Exp(-12f * Time.deltaTime)),
                    10f * Time.deltaTime);
            hasAimTarget = true;
            target = smoothedAimTarget;
        }

        float throwDistance = Vector3.Distance(
            new Vector3(origin.x, 0f, origin.z), new Vector3(target.x, 0f, target.z));
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
        float distanceT = Mathf.Pow(heightT, throwHeightExponent);
        return Mathf.Lerp(minThrowDistance, maxThrowDistance, distanceT);
    }

    private Vector3 GetTargetPoint(Vector3 origin)
    {
        if (useGazeTargeting && headset != null)
        {
            float floorY = GetFloorWorldY();
            // Head heading gives coarse aim; a small wrist bank or turn supplies fine steering.
            float distance = GetThrowDistance();
            Vector3 heading = GetGazeHeading();
            if (poseRouter != null && poseRouter.TryGetTrackedWristRotation(out Quaternion wrist))
            {
                if (!hasWristReference)
                {
                    wristReferenceHeading = Quaternion.LookRotation(heading, Vector3.up);
                    neutralWristRotation = Quaternion.Inverse(wristReferenceHeading) * wrist;
                    hasWristReference = true;
                }
                wristSteeringDegrees = WristSteeringAngle(neutralWristRotation,
                    Quaternion.Inverse(wristReferenceHeading) * wrist);
            }
            Vector3 steeredHeading = Quaternion.AngleAxis(wristSteeringDegrees, Vector3.up) * heading;
            Vector3 target = origin + steeredHeading * distance;
            return ConstrainForwardTarget(origin, target, steeredHeading,
                minThrowDistance, maxThrowDistance, floorY);
        }
        return GetTargetPoint(origin, GetThrowDistance());
    }

    public static float WristSteeringAngle(Quaternion neutral, Quaternion current)
    {
        Quaternion delta = current * Quaternion.Inverse(neutral);
        delta.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180f) angle -= 360f;
        if (Mathf.Abs(angle) < 0.001f) return 0f;
        // Positive yaw or a rightward bank steers right; ignore pitch used for holding the palm.
        float deflection = angle * (axis.y - axis.z);
        float outsideDeadBand = Mathf.Max(0f, Mathf.Abs(deflection) - 2f);
        return Mathf.Clamp(Mathf.Sign(deflection) * outsideDeadBand * 1.6f, -35f, 35f);
    }

    private Vector3 GetGazeHeading()
    {
        // Camera right remains useful when looking almost straight down at the palm.
        Vector3 forward = Vector3.ProjectOnPlane(headset.forward, Vector3.up);
        if (forward.sqrMagnitude < 0.04f) forward = Vector3.Cross(headset.right, Vector3.up);
        return forward.normalized;
    }

    public static Vector3 ConstrainForwardTarget(Vector3 origin, Vector3 target, Vector3 gazeForward,
        float minimum, float maximum, float floorY)
    {
        Vector3 forward = Vector3.ProjectOnPlane(gazeForward, Vector3.up);
        if (forward.sqrMagnitude < 0.0001f) forward = Vector3.forward;
        forward.Normalize();
        Vector3 planar = Vector3.ProjectOnPlane(target - origin, Vector3.up);
        // Looking at the hand or nearby floor must never send the grenade back towards the player.
        float along = Vector3.Dot(planar, forward);
        if (along < minimum) planar += forward * (minimum - along);
        planar = Vector3.ClampMagnitude(planar, maximum);
        return new Vector3(origin.x + planar.x, floorY, origin.z + planar.z);
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

        EnsureProjectile();
        int count = IceGrenadeTrajectory.Predict(origin, launchVelocity, Physics.gravity * gravityMultiplier,
            GetFloorWorldY(), activeProjectile.CollisionRadius, activeProjectile.MinimumFlightTime,
            activeProjectile.MaximumFlightTime, collisionMask, predictedPath, out var landing);
        predictedLanding = landing;
        arcPreviewLine.positionCount = count;
        for (int i = 0; i < count; i++) arcPreviewLine.SetPosition(i, predictedPath[i]);

        float phase = Mathf.Repeat(Time.time * 0.72f, 1f);
        const int pulseSteps = 10;
        arcPreviewPulse.positionCount = pulseSteps + 1;
        for (int i = 0; i <= pulseSteps; i++)
        {
            float fraction = Mathf.Lerp(Mathf.Max(0f, phase - 0.13f), phase, i / (float)pulseSteps);
            float sample = fraction * (count - 1);
            int index = Mathf.Min(Mathf.FloorToInt(sample), count - 2);
            arcPreviewPulse.SetPosition(i, Vector3.Lerp(predictedPath[index], predictedPath[index + 1], sample - index));
        }
        arcPreviewPulse.enabled = true;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, landing.Normal);
        Vector3 center = landing.Point + landing.Normal * 0.035f;
        CombatVfxStyle.SetRing(landingMarker, center, rotation, 0.26f, 48);
        CombatVfxStyle.SetRing(landingCenter, center, rotation, 0.025f, 24);
        Color markerColor = CombatVfxStyle.ColdCore;
        landingMarker.startColor = landingMarker.endColor = markerColor;
        landingCenter.startColor = landingCenter.endColor = markerColor;
    }

    private void HideArcPath()
    {
        if (arcPreviewLine != null) arcPreviewLine.enabled = false;
        if (arcPreviewPulse != null) arcPreviewPulse.enabled = false;
    }

    private void TraceLanding(Vector3 actual, Collider actualCollider)
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        if (!Application.isPlaying || !showArcPreview) return;
        try
        {
            string path = System.IO.Path.Combine(Application.persistentDataPath, "ice-landing-trace.csv");
            if (!System.IO.File.Exists(path))
                System.IO.File.WriteAllText(path, "time,predictedX,predictedY,predictedZ,actualX,actualY,actualZ,error,sameCollider\n");
            Vector3 predicted = predictedLanding.Point;
            System.IO.File.AppendAllText(path, string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "{0:F3},{1:F3},{2:F3},{3:F3},{4:F3},{5:F3},{6:F3},{7:F4},{8}\n", Time.time,
                predicted.x, predicted.y, predicted.z, actual.x, actual.y, actual.z,
                Vector3.Distance(predicted, actual), predictedLanding.Collider == actualCollider));
        }
        catch (System.IO.IOException) { }
#endif
    }

    private void HideArcPreview()
    {
        HideArcPath();
        if (landingMarker != null) landingMarker.enabled = false;
        if (landingCenter != null) landingCenter.enabled = false;
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
        arcPreviewLine.widthCurve = new AnimationCurve(
            new Keyframe(0f, 0.08f), new Keyframe(0.08f, 1f),
            new Keyframe(0.9f, 1f), new Keyframe(1f, 0.5f));
        arcPreviewLine.numCapVertices = 4;
        arcPreviewLine.material = CombatVfxStyle.CreateMaterial("Cold aim path", Color.white);
        arcPreviewLine.startColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.Cold, 0.72f);
        arcPreviewLine.endColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.ColdCore, 0.8f);
        arcPreviewLine.enabled = false;

        GameObject pulseObject = new GameObject("IceArcTracer");
        pulseObject.transform.SetParent(transform, false);
        arcPreviewPulse = pulseObject.AddComponent<LineRenderer>();
        arcPreviewPulse.useWorldSpace = true;
        arcPreviewPulse.alignment = LineAlignment.View;
        arcPreviewPulse.widthMultiplier = arcPreviewWidth * 1.4f;
        arcPreviewPulse.numCapVertices = 4;
        arcPreviewPulse.sharedMaterial = arcPreviewLine.sharedMaterial;
        arcPreviewPulse.startColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.ColdCore, 0f);
        arcPreviewPulse.endColor = CombatVfxStyle.WithAlpha(CombatVfxStyle.ColdCore, 0.82f);
        arcPreviewPulse.enabled = false;

        landingMarker = CombatVfxStyle.CreateLine(transform, "Ice landing marker",
            arcPreviewLine.sharedMaterial, true, 0.024f);
        landingCenter = CombatVfxStyle.CreateLine(transform, "Ice landing center",
            arcPreviewLine.sharedMaterial, true, 0.045f);
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

    private void PublishExplosion(Vector3 position, int contactKind, Vector3 contactPoint)
    {
        NetworkPlayerGrenadeVisual grenadeVisual = FindLocalNetworkGrenade();
        grenadeVisual?.SubmitExplosion(position, contactKind, contactPoint);
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
