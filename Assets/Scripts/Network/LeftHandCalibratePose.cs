using UnityEngine;

[DisallowMultipleComponent]
public sealed class LeftHandCalibratePose : MonoBehaviour
{
    [SerializeField] private OVRHand leftHand;
    [SerializeField] private NetworkArenaPlacementManager arenaPlacementManager;

    [Header("Arena And Player Calibration")]
    [Tooltip("Hold a left middle-finger pinch while standing on the shared calibration mark and facing the shared calibration direction.")]
    [SerializeField, Range(0f, 1f)] private float pinchThreshold = 0.75f;
    [Tooltip("Keeps an index pinch from accidentally triggering calibration.")]
    [SerializeField, Range(0f, 1f)] private float indexPinchMax = 0.35f;
    [SerializeField, Min(0f)] private float holdSeconds = 1f;
    [SerializeField, Min(0f)] private float repeatCooldownSeconds = 1.5f;
    [SerializeField] private bool logCalibration = true;

    private float heldTime;
    private float nextAllowedCalibrationTime;
    private bool loggedPoseDetected;
    private bool waitForRelease;

    private void Update()
    {
        bool poseActive = IsCalibrationPoseActive();
        if (!poseActive)
        {
            heldTime = 0f;
            loggedPoseDetected = false;
            waitForRelease = false;
            return;
        }

        if (waitForRelease)
        {
            return;
        }

        if (Time.time < nextAllowedCalibrationTime)
        {
            return;
        }

        if (logCalibration && !loggedPoseDetected)
        {
            loggedPoseDetected = true;
            Debug.Log($"Arena and player calibration detected. Hold the left middle-finger pinch for {holdSeconds:0.00}s.", this);
        }

        heldTime += Time.deltaTime;
        if (heldTime < holdSeconds)
        {
            return;
        }

        heldTime = 0f;
        loggedPoseDetected = false;
        waitForRelease = true;
        nextAllowedCalibrationTime = Time.time + repeatCooldownSeconds;
        CalibrateArenaAndPlayer();
    }

    [ContextMenu("Calibrate Arena And Player")]
    public void CalibrateArenaAndPlayer()
    {
        if (arenaPlacementManager == null)
        {
            arenaPlacementManager = FindFirstObjectByType<NetworkArenaPlacementManager>();
        }

        if (arenaPlacementManager == null)
        {
            Debug.LogWarning("Cannot calibrate the arena and player because no NetworkArenaPlacementManager was found.", this);
            return;
        }

        // Both operations are intentionally local and independent of Fusion
        // authority and Meta colocation. Place the arena first so the player
        // body calibration can reuse its newly calibrated floor height.
        RemotePlayerCorrection.Reset();
        arenaPlacementManager.PlaceArenaFromThisDevice();
        arenaPlacementManager.CalibrateNetworkPlayersFromThisDevice();

        if (logCalibration)
        {
            Debug.Log("Arena, player capsule, and health-bar calibration applied locally.", this);
        }
    }

    private bool IsCalibrationPoseActive()
    {
        return leftHand != null
            && leftHand.IsTracked
            && leftHand.GetFingerPinchStrength(OVRHand.HandFinger.Middle) >= pinchThreshold
            && leftHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) <= indexPinchMax;
    }
}
