using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.BuildingBlocks;
using Meta.XR.MultiplayerBlocks.Shared;
using Meta.XR.MRUtilityKit;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class ColocationArenaBinder : MonoBehaviour
{
    [SerializeField] private NetworkArenaPlacementManager arenaPlacementManager;
    [SerializeField] private Transform arenaRoot;
    [SerializeField] private Vector3 sharedArenaPosition = Vector3.zero;
    [SerializeField] private Vector3 sharedArenaEulerAngles = Vector3.zero;
    [SerializeField] private bool disableManualCalibrationAfterColocation = false;
    [SerializeField] private LeftHandCalibratePose manualCalibrationPose;
    [SerializeField] private bool logColocation = true;

    [Header("Guest Anchor Recovery")]
    [SerializeField, Min(1)] private int guestLoadRetryCount = 8;
    [SerializeField, Min(0.25f)] private float guestLoadRetryDelaySeconds = 2f;
    [SerializeField, Min(1f)] private float guestLoadAttemptTimeoutSeconds = 6f;

    private bool colocated;
    private SharedSpatialAnchorCore sharedAnchorCore;
    private Coroutine retryCoroutine;
    private bool loadAttemptCompleted;
    private bool anchorLoadSucceeded;

    private void OnEnable()
    {
        LocalMatchmaking.OnSessionDiscoverSucceeded.AddListener(OnGuestSessionDiscovered);
        FindSharedAnchorCore();
    }

    private void OnDisable()
    {
        LocalMatchmaking.OnSessionDiscoverSucceeded.RemoveListener(OnGuestSessionDiscovered);
        if (sharedAnchorCore != null)
        {
            sharedAnchorCore.OnSharedSpatialAnchorsLoadCompleted.RemoveListener(OnSharedAnchorsLoaded);
        }

        if (retryCoroutine != null)
        {
            StopCoroutine(retryCoroutine);
            retryCoroutine = null;
        }
    }

    [ContextMenu("Apply Colocated Arena Pose")]
    public void OnColocationReady()
    {
        colocated = true;
        RemotePlayerCorrection.Reset();

        if (arenaPlacementManager == null)
        {
            arenaPlacementManager = FindFirstObjectByType<NetworkArenaPlacementManager>();
        }

        Quaternion rotation = Quaternion.Euler(sharedArenaEulerAngles);

        if (arenaPlacementManager != null)
        {
            arenaPlacementManager.PlaceArenaAtWorldPose(sharedArenaPosition, rotation);
        }
        else
        {
            if (arenaRoot == null)
            {
                GameObject foundArena = GameObject.Find("ArenaRoot");
                if (foundArena != null)
                {
                    arenaRoot = foundArena.transform;
                }
            }

            if (arenaRoot != null)
            {
                arenaRoot.SetPositionAndRotation(sharedArenaPosition, rotation);
            }
        }

        if (disableManualCalibrationAfterColocation && manualCalibrationPose != null)
        {
            manualCalibrationPose.enabled = false;
        }

        if (logColocation)
        {
            Debug.Log($"Colocation ready. ArenaRoot set to shared pose {sharedArenaPosition}, {sharedArenaEulerAngles}.", this);
        }
    }

    public bool IsColocated => colocated;

    private void OnGuestSessionDiscovered(Guid groupUuid)
    {
        FindSharedAnchorCore();
        if (sharedAnchorCore == null)
        {
            Debug.LogError("Colocation guest discovered the host, but SharedSpatialAnchorCore is missing.", this);
            return;
        }

        anchorLoadSucceeded = false;
        loadAttemptCompleted = false;
        if (retryCoroutine != null)
        {
            StopCoroutine(retryCoroutine);
        }

        retryCoroutine = StartCoroutine(RetryGuestAnchorLoad(groupUuid));
    }

    private IEnumerator RetryGuestAnchorLoad(Guid groupUuid)
    {
        // Give the host's asynchronous create/save/share chain time to finish.
        yield return new WaitForSeconds(guestLoadRetryDelaySeconds);

        for (int attempt = 1; attempt <= guestLoadRetryCount && !anchorLoadSucceeded; attempt++)
        {
            loadAttemptCompleted = false;
            if (logColocation)
            {
                Debug.Log($"Loading shared alignment anchor, attempt {attempt}/{guestLoadRetryCount} for group {groupUuid}.", this);
            }

            sharedAnchorCore.LoadAndInstantiateAnchorsFromGroup(null, groupUuid);

            float deadline = Time.unscaledTime + guestLoadAttemptTimeoutSeconds;
            while (!loadAttemptCompleted && !anchorLoadSucceeded && Time.unscaledTime < deadline)
            {
                yield return null;
            }

            if (!anchorLoadSucceeded)
            {
                yield return new WaitForSeconds(guestLoadRetryDelaySeconds);
            }
        }

        if (!anchorLoadSucceeded)
        {
            Debug.LogError($"Shared alignment anchor was not available after {guestLoadRetryCount} retries. Check Enhanced Spatial Services, clear Physical Space History, and verify the host completed cloud sharing.", this);
        }

        retryCoroutine = null;
    }

    private void OnSharedAnchorsLoaded(List<OVRSpatialAnchor> anchors, OVRSpatialAnchor.OperationResult result)
    {
        loadAttemptCompleted = true;
        if (result != OVRSpatialAnchor.OperationResult.Success || anchors == null || anchors.Count == 0)
        {
            if (logColocation)
            {
                Debug.LogWarning($"Shared alignment anchor load returned {result} with {anchors?.Count ?? 0} anchors; retrying.", this);
            }

            return;
        }

        anchorLoadSucceeded = true;

        // Meta's installed handler waits for this same event, but explicitly
        // applying the anchor here also covers an early failed query that left
        // that handler awaiting forever.
        if (MRUK.Instance != null)
        {
            MRUK.Instance.SetCustomWorldLockAnchor(anchors[0], Pose.identity);
        }

        OnColocationReady();
    }

    private void FindSharedAnchorCore()
    {
        if (sharedAnchorCore == null)
        {
            sharedAnchorCore = FindFirstObjectByType<SharedSpatialAnchorCore>();
        }

        if (sharedAnchorCore != null)
        {
            sharedAnchorCore.OnSharedSpatialAnchorsLoadCompleted.RemoveListener(OnSharedAnchorsLoaded);
            sharedAnchorCore.OnSharedSpatialAnchorsLoadCompleted.AddListener(OnSharedAnchorsLoaded);
        }
    }
}
