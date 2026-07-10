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

    private bool colocated;

    [ContextMenu("Apply Colocated Arena Pose")]
    public void OnColocationReady()
    {
        colocated = true;

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
}
