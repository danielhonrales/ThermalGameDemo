using Fusion;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkObject))]
public sealed class NetworkArenaPlacementManager : NetworkBehaviour
{
    [SerializeField] private Transform arenaRoot;
    [SerializeField] private Transform localHead;
    [SerializeField] private string localHeadObjectName = "CenterEyeAnchor";
    [SerializeField] private bool placeFromAuthorityOnSpawn = true;
    [SerializeField] private float floorY = 0f;
    [SerializeField, Min(0f)] private float estimatedEyeHeightMeters = 1.6f;
    [SerializeField] private bool placeYFromHeadHeight = false;
    [SerializeField] private float verticalOffsetMeters = 0f;
    [SerializeField] private bool yawOnly = true;
    [SerializeField] private bool localOnlyPlacement = true;
    [SerializeField] private bool logPlacement = true;

    [Networked] private NetworkBool IsPlaced { get; set; }
    [Networked] private Vector3 ArenaPosition { get; set; }
    [Networked] private Quaternion ArenaRotation { get; set; }

    private bool attemptedAuthorityPlacement;

    public bool HasStateAuthorityForPlacement => Object == null || Runner == null || !Runner.IsRunning || Object.HasStateAuthority;

    public override void Spawned()
    {
        FindLocalHead();

        if (Object.HasStateAuthority && placeFromAuthorityOnSpawn)
        {
            TryPlaceFromLocalHead();
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && placeFromAuthorityOnSpawn && !IsPlaced && !attemptedAuthorityPlacement)
        {
            TryPlaceFromLocalHead();
        }
    }

    private void LateUpdate()
    {
        ApplyNetworkedArenaPose();
    }

    [ContextMenu("Place Arena From This Device")]
    public void PlaceArenaFromThisDevice()
    {
        FindLocalHead();

        if (localHead == null)
        {
            Debug.LogWarning("Cannot place arena because no local head transform was found.", this);
            return;
        }

        Vector3 position = localHead.position;
        position.y = placeYFromHeadHeight ? localHead.position.y - estimatedEyeHeightMeters : floorY;
        position.y += verticalOffsetMeters;

        Quaternion rotation = localHead.rotation;
        if (yawOnly)
        {
            Vector3 forward = Vector3.ProjectOnPlane(localHead.forward, Vector3.up).normalized;
            if (forward.sqrMagnitude < 0.001f)
            {
                forward = Vector3.forward;
            }

            rotation = Quaternion.LookRotation(forward, Vector3.up);
        }

        if (localOnlyPlacement)
        {
            ApplyLocalArenaPose(position, rotation);

            if (logPlacement)
            {
                Debug.Log($"ArenaRoot locally placed at {position} rotation {rotation.eulerAngles}.", this);
            }
        }
        else if (Object != null && Object.HasStateAuthority)
        {
            SetArenaPose(position, rotation);
        }
        else if (Object != null && Runner != null && Runner.IsRunning)
        {
            RPC_RequestArenaPlacement(position, rotation);
        }
        else
        {
            if (logPlacement)
            {
                Debug.Log("ArenaPlacementManager is not running as a Fusion network object, applying arena placement locally.", this);
            }

            ApplyLocalArenaPose(position, rotation);
        }
    }

    public void PlaceArenaAtWorldPose(Vector3 position, Quaternion rotation)
    {
        if (yawOnly)
        {
            Vector3 forward = Vector3.ProjectOnPlane(rotation * Vector3.forward, Vector3.up).normalized;
            if (forward.sqrMagnitude < 0.001f)
            {
                forward = Vector3.forward;
            }

            rotation = Quaternion.LookRotation(forward, Vector3.up);
        }

        position.y += verticalOffsetMeters;
        ApplyLocalArenaPose(position, rotation);

        if (logPlacement)
        {
            Debug.Log($"ArenaRoot placed from shared colocation pose at {position} rotation {rotation.eulerAngles}.", this);
        }
    }

    /// <summary>
    /// Establishes a local coordinate frame used only for networked player poses.
    /// This deliberately leaves ArenaRoot and the visual environment untouched.
    /// </summary>
    public void CalibrateNetworkPlayersFromManualReference(Vector3 physicalReferencePoint, Vector3 referencePointInArena)
    {
        FindLocalHead();

        Vector3 forward = localHead != null
            ? Vector3.ProjectOnPlane(localHead.forward, Vector3.up)
            : Vector3.forward;
        if (forward.sqrMagnitude < 0.001f)
        {
            forward = Vector3.forward;
        }

        Quaternion rotation = yawOnly
            ? Quaternion.LookRotation(forward.normalized, Vector3.up)
            : (localHead != null ? localHead.rotation : Quaternion.identity);

        Vector3 position = physicalReferencePoint - rotation * referencePointInArena;
        NetworkPlayerAlignment.SetCalibration(position, rotation);

        if (logPlacement)
        {
            Debug.Log($"Manual player calibration placed reference {referencePointInArena} at physical point {physicalReferencePoint}; network player frame is {position}, {rotation.eulerAngles}. ArenaRoot was not moved.", this);
        }
    }

    /// <summary>
    /// Calibrates only the coordinate frame used by networked player bodies.
    /// Stand on the shared physical marker and face the shared direction before
    /// calling this on each headset. ArenaRoot is never moved.
    /// </summary>
    public void CalibrateNetworkPlayersFromThisDevice()
    {
        FindLocalHead();
        if (localHead == null)
        {
            Debug.LogWarning("Cannot calibrate networked player bodies because no local head transform was found.", this);
            return;
        }

        Vector3 forward = Vector3.ProjectOnPlane(localHead.forward, Vector3.up).normalized;
        if (forward.sqrMagnitude < 0.001f)
        {
            forward = Vector3.forward;
        }

        Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
        Vector3 origin = localHead.position;

        // The headset supplies X/Z from the shared standing marker. Reuse the
        // already-calibrated arena floor only for height so player calibration
        // cannot disturb the arena itself.
        if (arenaRoot == null)
        {
            GameObject foundArena = GameObject.Find("ArenaRoot");
            if (foundArena != null)
            {
                arenaRoot = foundArena.transform;
            }
        }

        origin.y = arenaRoot != null ? arenaRoot.position.y : floorY;
        NetworkPlayerAlignment.SetCalibration(origin, rotation);

        if (logPlacement)
        {
            Debug.Log($"Player-body frame calibrated at {origin}, yaw {rotation.eulerAngles.y:0.0}. ArenaRoot was not moved.", this);
        }
    }

    private void TryPlaceFromLocalHead()
    {
        attemptedAuthorityPlacement = true;
        PlaceArenaFromThisDevice();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestArenaPlacement(Vector3 position, Quaternion rotation)
    {
        SetArenaPose(position, rotation);
    }

    private void SetArenaPose(Vector3 position, Quaternion rotation)
    {
        ArenaPosition = position;
        ArenaRotation = rotation;
        IsPlaced = true;
        ApplyLocalArenaPose(position, rotation);
        if (logPlacement)
        {
            Debug.Log($"ArenaRoot placed at {position} rotation {rotation.eulerAngles}.", this);
        }
    }

    private void ApplyNetworkedArenaPose()
    {
        if (localOnlyPlacement)
        {
            return;
        }

        if (!IsPlaced)
        {
            return;
        }

        ApplyLocalArenaPose(ArenaPosition, ArenaRotation);
    }

    private void ApplyLocalArenaPose(Vector3 position, Quaternion rotation)
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
            arenaRoot.SetPositionAndRotation(position, rotation);
        }
        else if (logPlacement)
        {
            Debug.LogWarning("Cannot apply arena pose because ArenaRoot is not assigned and no object named ArenaRoot was found.", this);
        }
    }

    private void FindLocalHead()
    {
        if (localHead != null)
        {
            return;
        }

        GameObject found = GameObject.Find(localHeadObjectName);
        if (found != null)
        {
            localHead = found.transform;
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            localHead = mainCamera.transform;
        }
    }
}
