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
    [SerializeField, Min(0.05f)] private float headRadiusMeters = 0.18f;
    [SerializeField] private bool hideHeadRenderer = true;

    [Networked] private Vector3 NetworkHeadPosition { get; set; }
    [Networked] private Quaternion NetworkHeadRotation { get; set; }

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

        if (useArenaRelativeCoordinates && arenaRoot != null)
        {
            Vector3 worldPosition = arenaRoot.TransformPoint(NetworkHeadPosition);
            Quaternion worldRotation = arenaRoot.rotation * NetworkHeadRotation;
            headHitbox.SetPositionAndRotation(worldPosition, worldRotation);
            return;
        }

        headHitbox.SetPositionAndRotation(NetworkHeadPosition, NetworkHeadRotation);
    }

    private void PushLocalHeadToNetwork()
    {
        if (localHead == null)
        {
            return;
        }

        FindArenaRoot();

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

        SphereCollider sphere = headHitbox.GetComponent<SphereCollider>();
        if (sphere == null)
        {
            sphere = headHitbox.gameObject.AddComponent<SphereCollider>();
        }

        sphere.radius = headRadiusMeters;
        sphere.center = Vector3.zero;
        sphere.isTrigger = false;

        Renderer[] renderers = headHitbox.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer hitboxRenderer in renderers)
        {
            hitboxRenderer.enabled = !hideHeadRenderer;
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
}
