using UnityEngine;

[DisallowMultipleComponent]
public sealed class LeftHandCalibratePose : MonoBehaviour
{
    public enum CalibratePose
    {
        MiddleFingerPinch,
        ClosedFist
    }

    [SerializeField] private OVRHand leftHand;
    [SerializeField] private NetworkArenaPlacementManager arenaPlacementManager;
    [SerializeField] private CalibratePose pose = CalibratePose.MiddleFingerPinch;
    [SerializeField, Range(0f, 1f)] private float closedFingerThreshold = 0.75f;
    [SerializeField, Range(0f, 1f)] private float pinchThreshold = 0.75f;
    [SerializeField, Range(0f, 1f)] private float indexPinchMax = 0.35f;
    [SerializeField, Min(0f)] private float holdSeconds = 1f;
    [SerializeField, Min(0f)] private float repeatCooldownSeconds = 1.5f;
    [SerializeField] private bool requireHandTracked = true;
    [SerializeField] private bool requireStateAuthority = true;
    [SerializeField] private bool calibrateOnlyOnce = false;
    [SerializeField] private bool logCalibration = true;

    private float heldTime;
    private float nextAllowedCalibrationTime;
    private bool hasCalibrated;
    private bool loggedAuthorityBlock;
    private bool loggedPoseDetected;

    private void Update()
    {
        if (calibrateOnlyOnce && hasCalibrated)
        {
            return;
        }

        bool poseActive = IsCalibrationPoseActive();
        if (!poseActive)
        {
            heldTime = 0f;
            loggedPoseDetected = false;
            return;
        }

        if (Time.time < nextAllowedCalibrationTime)
        {
            return;
        }

        if (logCalibration && !loggedPoseDetected)
        {
            loggedPoseDetected = true;
            Debug.Log($"Left-hand {pose} calibration pose detected. Holding for {holdSeconds:0.00}s.", this);
        }

        heldTime += Time.deltaTime;
        if (heldTime < holdSeconds)
        {
            return;
        }

        hasCalibrated = true;
        heldTime = 0f;
        loggedPoseDetected = false;
        nextAllowedCalibrationTime = Time.time + repeatCooldownSeconds;
        CalibrateArena();
    }

    [ContextMenu("Calibrate Arena")]
    public void CalibrateArena()
    {
        if (arenaPlacementManager == null)
        {
            arenaPlacementManager = FindFirstObjectByType<NetworkArenaPlacementManager>();
        }

        if (arenaPlacementManager == null)
        {
            Debug.LogWarning("Cannot calibrate arena because no NetworkArenaPlacementManager was found.", this);
            return;
        }

        if (requireStateAuthority && !arenaPlacementManager.HasStateAuthorityForPlacement)
        {
            if (logCalibration && !loggedAuthorityBlock)
            {
                loggedAuthorityBlock = true;
                Debug.LogWarning("Calibration pose fired, but calibration was blocked because this device does not have arena state authority. Turn off Require State Authority for local testing, or trigger calibration from the host/state-authority headset.", this);
            }

            return;
        }

        arenaPlacementManager.PlaceArenaFromThisDevice();

        if (logCalibration)
        {
            Debug.Log($"Arena calibration requested from left-hand {pose} pose.", this);
        }
    }

    private bool IsCalibrationPoseActive()
    {
        if (leftHand == null)
        {
            return false;
        }

        if (requireHandTracked && !leftHand.IsTracked)
        {
            return false;
        }

        return pose switch
        {
            CalibratePose.MiddleFingerPinch => IsMiddleFingerPinchActive(),
            CalibratePose.ClosedFist => IsClosedFistLikePoseActive(),
            _ => false
        };
    }

    private bool IsMiddleFingerPinchActive()
    {
        return leftHand.GetFingerPinchStrength(OVRHand.HandFinger.Middle) >= pinchThreshold
            && leftHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) <= indexPinchMax;
    }

    private bool IsClosedFistLikePoseActive()
    {
        return leftHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) >= closedFingerThreshold
            && leftHand.GetFingerPinchStrength(OVRHand.HandFinger.Middle) >= closedFingerThreshold
            && leftHand.GetFingerPinchStrength(OVRHand.HandFinger.Ring) >= closedFingerThreshold
            && leftHand.GetFingerPinchStrength(OVRHand.HandFinger.Pinky) >= closedFingerThreshold;
    }
}
