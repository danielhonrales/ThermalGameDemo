using Fusion;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkObject))]
public sealed class NetworkHeadTracker : NetworkBehaviour
{
    [Header("Local Tracking")]
    [SerializeField] private Transform localHead;
    [SerializeField] private string localHeadObjectName = "CenterEyeAnchor";
    [SerializeField] private Transform arenaRoot;
    [SerializeField] private string arenaRootObjectName = "ArenaRoot";
    [SerializeField] private bool useArenaRelativeCoordinates = true;

    [Header("Networked Hitbox")]
    [SerializeField] private Transform headHitbox;
    [Tooltip("Uses the existing HeadHitbox object as a compact full-body target below the tracked head.")]
    [SerializeField] private bool useFullBodyCapsule = true;
    [SerializeField, Min(0.05f)] private float headRadiusMeters = 0.18f;
    [SerializeField, Min(0.1f)] private float headCapsuleHeightMeters = 0.34f;
    [SerializeField] private Vector3 headCapsuleCenter = Vector3.zero;
    [SerializeField, Min(0.1f)] private float bodyCapsuleRadiusMeters = 0.28f;
    [SerializeField, Min(0.2f)] private float bodyCapsuleHeightMeters = 1.7f;
    [SerializeField, Min(0f)] private float bodyCenterBelowHeadMeters = 0.85f;

    [Networked] private Vector3 NetworkHeadPosition { get; set; }
    [Networked] private Quaternion NetworkHeadRotation { get; set; }

    public Vector3 HeadWorldPosition => GetHeadWorldPose().position;

    public override void Spawned()
    {
        EnsureHeadHitbox();
        ApplyHeadHitboxSetup();
        FindArenaRoot();

        if (Object.HasInputAuthority)
        {
            FindLocalHead();
            PushLocalHeadToNetwork();
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority)
        {
            if (localHead == null)
            {
                FindLocalHead();
            }

            PushLocalHeadToNetwork();
        }
    }

    private void LateUpdate()
    {
        if (headHitbox == null)
        {
            return;
        }

        FindArenaRoot();

        Pose headPose = GetHeadWorldPose();
        if (useFullBodyCapsule)
        {
            Vector3 bodyPosition = headPose.position + Vector3.down * bodyCenterBelowHeadMeters;
            Vector3 bodyForward = Vector3.ProjectOnPlane(headPose.rotation * Vector3.forward, Vector3.up);
            if (bodyForward.sqrMagnitude < 0.0001f)
            {
                bodyForward = Vector3.forward;
            }

            headHitbox.SetPositionAndRotation(bodyPosition, Quaternion.LookRotation(bodyForward.normalized, Vector3.up));
            return;
        }

        headHitbox.SetPositionAndRotation(headPose.position, headPose.rotation);
    }

    private void PushLocalHeadToNetwork()
    {
        if (localHead == null)
        {
            return;
        }

        FindArenaRoot();

        if (NetworkPlayerAlignment.HasCalibration)
        {
            NetworkHeadPosition = NetworkPlayerAlignment.InverseTransformPoint(localHead.position);
            NetworkHeadRotation = NetworkPlayerAlignment.InverseTransformRotation(localHead.rotation);
            return;
        }

        if (useArenaRelativeCoordinates && arenaRoot != null)
        {
            NetworkHeadPosition = arenaRoot.InverseTransformPoint(localHead.position);
            NetworkHeadRotation = Quaternion.Inverse(arenaRoot.rotation) * localHead.rotation;
            return;
        }

        NetworkHeadPosition = localHead.position;
        NetworkHeadRotation = localHead.rotation;
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

    private void FindArenaRoot()
    {
        if (arenaRoot != null)
        {
            return;
        }

        GameObject found = GameObject.Find(arenaRootObjectName);
        if (found != null)
        {
            arenaRoot = found.transform;
        }
    }

    private void EnsureHeadHitbox()
    {
        if (headHitbox != null)
        {
            return;
        }

        Transform existing = transform.Find("HeadHitbox");
        if (existing != null)
        {
            headHitbox = existing;
            return;
        }

        GameObject hitboxObject = new GameObject("HeadHitbox");
        hitboxObject.transform.SetParent(transform, false);
        headHitbox = hitboxObject.transform;
    }

    private void ApplyHeadHitboxSetup()
    {
        if (headHitbox == null)
        {
            return;
        }

        int headTargetLayer = CombatLayers.HeadTargetLayer;
        if (headTargetLayer >= 0)
        {
            headHitbox.gameObject.layer = headTargetLayer;
        }

        // The hitbox transform is placed at the tracked head. Keep the capsule compact
        // and locally centered so it cannot drift below or away from the player.
        foreach (SphereCollider sphere in headHitbox.GetComponents<SphereCollider>())
        {
            sphere.enabled = false;
            Destroy(sphere);
        }

        CapsuleCollider capsule = headHitbox.GetComponent<CapsuleCollider>();
        if (capsule == null)
        {
            capsule = headHitbox.gameObject.AddComponent<CapsuleCollider>();
        }

        float radius = useFullBodyCapsule ? bodyCapsuleRadiusMeters : headRadiusMeters;
        float height = useFullBodyCapsule ? bodyCapsuleHeightMeters : headCapsuleHeightMeters;
        capsule.radius = radius;
        capsule.height = Mathf.Max(height, radius * 2f);
        capsule.center = useFullBodyCapsule ? Vector3.zero : headCapsuleCenter;
        capsule.direction = 1;
        capsule.isTrigger = false;
        capsule.enabled = true;

        Renderer[] renderers = headHitbox.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer hitboxRenderer in renderers)
        {
            Destroy(hitboxRenderer);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (headHitbox == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(headHitbox.position, headRadiusMeters);
    }

    private Pose GetHeadWorldPose()
    {
        FindArenaRoot();
        Pose pose;
        if (NetworkPlayerAlignment.HasCalibration)
        {
            pose = new Pose(
                NetworkPlayerAlignment.TransformPoint(NetworkHeadPosition),
                NetworkPlayerAlignment.TransformRotation(NetworkHeadRotation));
        }
        else if (useArenaRelativeCoordinates && arenaRoot != null)
        {
            pose = new Pose(
                arenaRoot.TransformPoint(NetworkHeadPosition),
                arenaRoot.rotation * NetworkHeadRotation);
        }
        else
        {
            pose = new Pose(NetworkHeadPosition, NetworkHeadRotation);
        }

        if (Object != null && !Object.HasInputAuthority)
        {
            pose.position = RemotePlayerCorrection.Apply(pose.position);
        }

        return pose;
    }
}

/// <summary>
/// Per-headset mapping between a manually registered physical frame and the
/// shared coordinates transmitted for player tracking. It never moves scene art.
/// </summary>
public static class NetworkPlayerAlignment
{
    public static bool HasCalibration { get; private set; }

    private static Vector3 origin;
    private static Quaternion rotation = Quaternion.identity;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay()
    {
        HasCalibration = false;
        origin = Vector3.zero;
        rotation = Quaternion.identity;
    }

    public static void SetCalibration(Vector3 worldOrigin, Quaternion worldRotation)
    {
        origin = worldOrigin;
        rotation = worldRotation;
        HasCalibration = true;
    }

    public static Vector3 InverseTransformPoint(Vector3 worldPoint)
    {
        return Quaternion.Inverse(rotation) * (worldPoint - origin);
    }

    public static Quaternion InverseTransformRotation(Quaternion worldRotation)
    {
        return Quaternion.Inverse(rotation) * worldRotation;
    }

    public static Vector3 TransformPoint(Vector3 alignedPoint)
    {
        return origin + rotation * alignedPoint;
    }

    public static Quaternion TransformRotation(Quaternion alignedRotation)
    {
        return rotation * alignedRotation;
    }

    public static Vector3 InverseTransformDirection(Vector3 worldDirection)
    {
        return Quaternion.Inverse(rotation) * worldDirection;
    }

    public static Vector3 TransformDirection(Vector3 alignedDirection)
    {
        return rotation * alignedDirection;
    }
}

public static class RemotePlayerCorrection
{
    public static Vector3 WorldOffset { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay() => Reset();

    public static void SetWorldOffset(Vector3 offset) => WorldOffset = offset;
    public static Vector3 Apply(Vector3 worldPosition) => worldPosition + WorldOffset;
    public static void Reset() => WorldOffset = Vector3.zero;
}
