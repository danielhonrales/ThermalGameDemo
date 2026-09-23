using UnityEngine;

[DisallowMultipleComponent]
public sealed class ForearmShieldController : MonoBehaviour
{
    [Header("Hand Mount")]
    [SerializeField] private Transform handOrigin;
    [SerializeField] private Transform headset;
    [SerializeField] private Vector3 shieldLocalOffset = Vector3.zero;
    [SerializeField, Min(0.02f)] private float shieldForwardDistance = 0.1f;
    [Tooltip("Hand-local axis used only to push the shield off the hand. Up is the back-of-hand side for this rig; Down is the palm/beam side.")]
    [SerializeField] private PalmBeamShooter.LocalAxis shieldMountOffsetAxis = PalmBeamShooter.LocalAxis.Up;
    [SerializeField] private PalmBeamShooter.LocalAxis shieldFacingAxis = PalmBeamShooter.LocalAxis.Forward;
    [Tooltip("Places the shield on the back of the hand instead of in front of a closed fist.")]
    [SerializeField] private bool mountOnBackOfHand = true;
    [Tooltip("Local rotation applied only for the back-of-hand mount. Adjust the signed X value if the shield faces the wrong way on-device.")]
    [SerializeField] private Vector3 backHandRotationOffset = new Vector3(90f, 0f, 0f);

    [Header("Fist Gate")]
    [SerializeField] private bool requireTrackedHand = true;
    [SerializeField] private OVRHand handTrackingSource;
    [SerializeField] private OVRSkeleton handSkeleton;
    [SerializeField, Range(0.06f, 0.2f)] private float fingertipNearPalmDistance = 0.13f;
    [SerializeField, Range(1, 4)] private int requiredFingertipsNearPalm = 2;
    [SerializeField, Min(0f)] private float shieldDurationSeconds = 3f;

    [Header("Debug")]
    [SerializeField] private bool logShieldState;

    private ForearmShieldEffects shieldEffects;
    private NetworkPlayerShieldVisual localNetworkShield;
    private PalmBeamShooter beamShooter;
    private bool lastShieldActive;
    private bool lastPublishedShieldActive;
    private bool wasFistActive;
    private float shieldActiveUntilTime;
    private HandPoseRouter poseRouter;

    public bool IsShieldActive => poseRouter != null
        ? poseRouter.IsShieldPose : Time.time < shieldActiveUntilTime;

    public void CancelForAttack()
    {
        shieldActiveUntilTime = 0f;
    }

    private void OnDisable()
    {
        shieldActiveUntilTime = 0f;
        CombatEventOutput.State("shield", false);
        shieldEffects?.HideShield();
        PublishShield(false, Vector3.zero, Quaternion.identity);
    }

    private void Awake()
    {
        shieldEffects = GetComponent<ForearmShieldEffects>();
        beamShooter = GetComponent<PalmBeamShooter>();
        poseRouter = GetComponent<HandPoseRouter>();
        if (shieldEffects == null)
        {
            shieldEffects = gameObject.AddComponent<ForearmShieldEffects>();
        }

        FindSceneReferences();

        if (handOrigin != null)
        {
            shieldEffects.ConfigureHandMount(handOrigin);
        }

        EnsureHandSkeleton();

        int layer = CombatLayers.BeamLayer;
        if (layer >= 0)
        {
            CombatLayers.SetLayerRecursively(gameObject, layer);
        }
    }

    private void Update()
    {
        FindSceneReferences();

        if (handOrigin == null)
        {
            return;
        }

        if (shieldEffects != null)
        {
            shieldEffects.ConfigureHandMount(handOrigin);
        }

        bool fistActive = IsFistActive();
        if (fistActive)
        {
            if (!wasFistActive)
            {
                beamShooter?.CancelForShield();
            }
            shieldActiveUntilTime = Time.time + shieldDurationSeconds;
        }
        else if (poseRouter != null
            || (beamShooter != null && beamShooter.IsAttackPoseActive()))
        {
            shieldActiveUntilTime = 0f;
        }

        wasFistActive = fistActive;

        bool shieldActive = IsShieldActive;
        CombatEventOutput.State("shield", shieldActive);

        if (!TryGetShieldPose(out Vector3 worldPosition, out Quaternion worldRotation))
        {
            if (shieldActive)
            {
                shieldActiveUntilTime = 0f;
                shieldActive = false;
            }

            CombatEventOutput.State("shield", false);
            shieldEffects.HideShield();
            PublishShield(false, worldPosition, worldRotation);
            LogShieldIfChanged(false);
            return;
        }

        Vector3 localPosition = handOrigin.InverseTransformPoint(worldPosition);
        Quaternion localRotation = Quaternion.Inverse(handOrigin.rotation) * worldRotation;

        if (shieldActive || (poseRouter != null && poseRouter.FeedbackPose == HandPoseRouter.PoseKind.Shield))
        {
            shieldEffects.ShowShieldLocal(localPosition, localRotation, shieldActive);
        }
        else
        {
            shieldEffects.HideShield();
        }

        PublishShield(shieldActive, worldPosition, worldRotation);
        LogShieldIfChanged(shieldActive);
    }

    public void SetHandOrigin(Transform newHandOrigin)
    {
        handOrigin = newHandOrigin;
        shieldEffects?.ConfigureHandMount(handOrigin);
    }

    public void SetPalmOrigin(Transform newPalmOrigin)
    {
        SetHandOrigin(newPalmOrigin);
    }

    public void SetHeadset(Transform newHeadset)
    {
        headset = newHeadset;
    }

    [ContextMenu("Find Scene References")]
    public void FindSceneReferences()
    {
        if (handOrigin == null)
        {
            PalmBeamShooter beamShooter = GetComponent<PalmBeamShooter>();
            if (beamShooter != null)
            {
                handOrigin = FindTransformByName("RightHandAnchor");
            }

            if (handOrigin == null)
            {
                handOrigin = FindTransformByName("RightHandAnchor");
            }
        }

        if (headset == null)
        {
            headset = FindTransformByName("CenterEyeAnchor");
            if (headset == null)
            {
                Camera mainCamera = Camera.main;
                if (mainCamera != null)
                {
                    headset = mainCamera.transform;
                }
            }
        }

        if (handTrackingSource == null)
        {
            handTrackingSource = FindRightHand();
        }

        EnsureHandSkeleton();
    }

    private static Transform FindTransformByName(string objectName)
    {
        GameObject found = GameObject.Find(objectName);
        return found != null ? found.transform : null;
    }

    private static OVRHand FindRightHand()
    {
        OVRHand[] hands = FindObjectsByType<OVRHand>(FindObjectsSortMode.None);
        foreach (OVRHand hand in hands)
        {
            if (hand == null)
            {
                continue;
            }

            if (hand.gameObject.name.IndexOf("Right", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return hand;
            }
        }

        foreach (OVRHand hand in hands)
        {
            Transform anchor = FindTransformByName("RightHandAnchor");
            if (anchor != null && hand.transform.IsChildOf(anchor))
            {
                return hand;
            }
        }

        return hands.Length > 0 ? hands[0] : null;
    }

    private bool IsFistActive()
    {
        if (poseRouter != null) return poseRouter.IsShieldPose;
        if (requireTrackedHand && (handTrackingSource == null || !handTrackingSource.IsTracked))
        {
            return false;
        }

        if (TryIsFistFromSkeleton())
        {
            return true;
        }

        return TryIsFistFallback();
    }

    private bool TryIsFistFromSkeleton()
    {
        EnsureHandSkeleton();
        if (handSkeleton == null || !handSkeleton.IsInitialized || handSkeleton.Bones == null)
        {
            return false;
        }

        Transform palm = FindBone(
            OVRSkeleton.BoneId.XRHand_Palm,
            OVRSkeleton.BoneId.Hand_WristRoot);
        if (palm == null)
        {
            palm = FindBone(
                OVRSkeleton.BoneId.XRHand_Wrist,
                OVRSkeleton.BoneId.Hand_WristRoot);
        }

        if (palm == null)
        {
            return false;
        }

        int nearPalmCount = 0;
        nearPalmCount += IsFingertipNearPalm(palm, FindBone(OVRSkeleton.BoneId.XRHand_IndexTip, OVRSkeleton.BoneId.Hand_IndexTip)) ? 1 : 0;
        nearPalmCount += IsFingertipNearPalm(palm, FindBone(OVRSkeleton.BoneId.XRHand_MiddleTip, OVRSkeleton.BoneId.Hand_MiddleTip)) ? 1 : 0;
        nearPalmCount += IsFingertipNearPalm(palm, FindBone(OVRSkeleton.BoneId.XRHand_RingTip, OVRSkeleton.BoneId.Hand_RingTip)) ? 1 : 0;
        nearPalmCount += IsFingertipNearPalm(palm, FindBone(OVRSkeleton.BoneId.XRHand_LittleTip, OVRSkeleton.BoneId.Hand_PinkyTip)) ? 1 : 0;

        return nearPalmCount >= requiredFingertipsNearPalm;
    }

    private bool TryIsFistFallback()
    {
        if (handTrackingSource == null)
        {
            return false;
        }

        int pinchingFingers = 0;
        if (handTrackingSource.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            pinchingFingers++;
        }

        if (handTrackingSource.GetFingerIsPinching(OVRHand.HandFinger.Middle))
        {
            pinchingFingers++;
        }

        if (handTrackingSource.GetFingerIsPinching(OVRHand.HandFinger.Ring))
        {
            pinchingFingers++;
        }

        if (handTrackingSource.GetFingerIsPinching(OVRHand.HandFinger.Pinky))
        {
            pinchingFingers++;
        }

        return pinchingFingers >= requiredFingertipsNearPalm;
    }

    private bool IsFingertipNearPalm(Transform palm, Transform fingertip)
    {
        return fingertip != null
            && Vector3.Distance(fingertip.position, palm.position) <= fingertipNearPalmDistance;
    }

    private void EnsureHandSkeleton()
    {
        if (handTrackingSource == null && handOrigin != null)
        {
            handTrackingSource = handOrigin.GetComponentInChildren<OVRHand>();
        }

        if (handSkeleton != null && handSkeleton.GetSkeletonType() != OVRSkeleton.SkeletonType.None) return;
        handSkeleton = CombatHandSkeleton.For(handTrackingSource);
    }

    private Transform FindBone(params OVRSkeleton.BoneId[] candidates)
    {
        if (handSkeleton == null || handSkeleton.Bones == null)
        {
            return null;
        }

        var type = handSkeleton.GetSkeletonType();
        bool xr = type == OVRSkeleton.SkeletonType.XRHandRight || type == OVRSkeleton.SkeletonType.XRHandLeft;
        OVRSkeleton.BoneId candidate = candidates[xr || candidates.Length == 1 ? 0 : 1];
        foreach (OVRBone bone in handSkeleton.Bones)
        {
            if (bone.Id == candidate && bone.Transform != null) return bone.Transform;
        }

        return null;
    }

    private bool TryGetShieldPose(out Vector3 worldPosition, out Quaternion worldRotation)
    {
        worldPosition = handOrigin.position;
        worldRotation = handOrigin.rotation;

        EnsureHandSkeleton();
        if (handSkeleton != null && handSkeleton.IsInitialized && TryGetKnuckleShieldPose(out worldPosition, out worldRotation))
        {
            return true;
        }

        Vector3 handForward = GetWorldAxisDirection(handOrigin, shieldFacingAxis);
        if (handForward.sqrMagnitude <= 0.0001f)
        {
            return false;
        }

        Vector3 forward = mountOnBackOfHand ? -handForward : handForward;
        worldPosition = handOrigin.position + handOrigin.TransformVector(shieldLocalOffset) + GetMountOffsetDirection() * shieldForwardDistance;
        worldRotation = BuildShieldRotation(worldPosition, forward);
        worldRotation = ApplyBackHandRotation(worldRotation);
        return true;
    }

    private bool TryGetKnuckleShieldPose(out Vector3 worldPosition, out Quaternion worldRotation)
    {
        worldPosition = handOrigin.position;
        worldRotation = handOrigin.rotation;

        Transform wrist = FindBone(OVRSkeleton.BoneId.XRHand_Wrist, OVRSkeleton.BoneId.Hand_WristRoot);
        Transform index = FindBone(OVRSkeleton.BoneId.XRHand_IndexProximal, OVRSkeleton.BoneId.Hand_Index1);
        Transform middle = FindBone(OVRSkeleton.BoneId.XRHand_MiddleProximal, OVRSkeleton.BoneId.Hand_Middle1);
        Transform pinky = FindBone(OVRSkeleton.BoneId.XRHand_LittleProximal, OVRSkeleton.BoneId.Hand_Pinky1);
        if (wrist == null || index == null || middle == null || pinky == null) return false;
        // Stable palm plane: curled fingertips must not steer a fist-mounted shield.
        Vector3 alongHand = middle.position - wrist.position;
        Vector3 acrossHand = index.position - pinky.position;
        Vector3 backOfHand = Vector3.Cross(acrossHand, alongHand);
        if (alongHand.sqrMagnitude < 0.0001f || backOfHand.sqrMagnitude < 0.0000001f) return false;
        backOfHand.Normalize();
        Vector3 normal = mountOnBackOfHand ? backOfHand : alongHand.normalized;
        worldPosition = Vector3.Lerp(wrist.position, middle.position, 0.65f)
            + normal * shieldForwardDistance;
        worldRotation = Quaternion.LookRotation(normal, mountOnBackOfHand ? alongHand.normalized : backOfHand);
        return true;
    }

    private Quaternion ApplyBackHandRotation(Quaternion rotation)
    {
        return mountOnBackOfHand
            ? rotation * Quaternion.Euler(backHandRotationOffset)
            : rotation;
    }

    private Vector3 GetMountOffsetDirection()
    {
        Vector3 offsetDirection = GetWorldAxisDirection(handOrigin, shieldMountOffsetAxis);
        return offsetDirection.sqrMagnitude > 0.0001f ? offsetDirection.normalized : Vector3.up;
    }

    private Quaternion BuildShieldRotation(Vector3 shieldAnchor, Vector3 forward)
    {
        Vector3 up = handOrigin.up;
        if (headset != null)
        {
            Vector3 faceForward = Vector3.ProjectOnPlane(headset.position - shieldAnchor, Vector3.up);
            if (faceForward.sqrMagnitude > 0.0001f)
            {
                up = Vector3.Cross(Vector3.Cross(faceForward.normalized, forward), forward);
                if (up.sqrMagnitude <= 0.0001f)
                {
                    up = handOrigin.up;
                }
                else
                {
                    up.Normalize();
                }
            }
        }

        return Quaternion.LookRotation(forward, up);
    }

    private void PublishShield(bool active, Vector3 position, Quaternion rotation)
    {
        if (!active && !lastPublishedShieldActive)
        {
            return;
        }

        NetworkPlayerShieldVisual shieldVisual = FindLocalNetworkShield();
        shieldVisual?.SubmitShield(active, position, rotation);
        lastPublishedShieldActive = active;
    }

    private NetworkPlayerShieldVisual FindLocalNetworkShield()
    {
        if (localNetworkShield != null && localNetworkShield.IsLocalPlayer)
        {
            return localNetworkShield;
        }

        NetworkPlayerShieldVisual[] shieldVisuals = FindObjectsByType<NetworkPlayerShieldVisual>(FindObjectsSortMode.None);
        foreach (NetworkPlayerShieldVisual shieldVisual in shieldVisuals)
        {
            if (shieldVisual.IsLocalPlayer)
            {
                localNetworkShield = shieldVisual;
                return localNetworkShield;
            }
        }

        return null;
    }

    private void LogShieldIfChanged(bool shieldActive)
    {
        if (!logShieldState || shieldActive == lastShieldActive)
        {
            return;
        }

        lastShieldActive = shieldActive;
        Debug.Log(shieldActive ? "Forearm shield activated." : "Forearm shield deactivated.", this);
    }

    private static Vector3 GetWorldAxisDirection(Transform source, PalmBeamShooter.LocalAxis axis)
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
}
