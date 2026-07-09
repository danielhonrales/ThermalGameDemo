using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SphereCollider))]
public sealed class HeadshotTarget : MonoBehaviour
{
    [SerializeField] private Transform trackedHead;
    [SerializeField, Min(0.05f)] private float radiusMeters = 0.18f;
    [SerializeField] private bool followTrackedHead = true;
    [SerializeField] private bool colliderIsTrigger;

    public Transform TrackedHead
    {
        get => trackedHead;
        set => trackedHead = value;
    }

    public float RadiusMeters
    {
        get => radiusMeters;
        set
        {
            radiusMeters = Mathf.Max(0.05f, value);
            ConfigureCollider();
        }
    }

    private void Reset()
    {
        ApplySetup();
    }

    private void Awake()
    {
        ApplySetup();
    }

    private void LateUpdate()
    {
        if (followTrackedHead && trackedHead != null)
        {
            transform.SetPositionAndRotation(trackedHead.position, trackedHead.rotation);
        }
    }

    [ContextMenu("Apply Head Target Setup")]
    public void ApplySetup()
    {
        int layer = CombatLayers.HeadTargetLayer;
        if (layer >= 0)
        {
            gameObject.layer = layer;
        }

        ConfigureCollider();
    }

    private void ConfigureCollider()
    {
        SphereCollider sphere = GetComponent<SphereCollider>();
        sphere.radius = radiusMeters;
        sphere.center = Vector3.zero;
        sphere.isTrigger = colliderIsTrigger;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusMeters);
    }
}
