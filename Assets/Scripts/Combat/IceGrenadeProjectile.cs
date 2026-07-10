using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class IceGrenadeProjectile : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float collisionRadius = 0.1f;
    [SerializeField, Min(0.1f)] private float maxLifetime = 4f;
    [SerializeField, Min(0f)] private float minFlightTime = 0.08f;

    private Vector3 velocity;
    private float spawnTime;
    private float gravityMultiplier = 1f;
    private float floorWorldY;
    private LayerMask collisionMask;
    private Transform visualRoot;
    private Light flightLight;
    private Action<Vector3, Collider> onExploded;

    public Vector3 Velocity => velocity;
    public bool IsAlive => enabled && gameObject.activeInHierarchy;

    public void Launch(
        Vector3 startPosition,
        Vector3 initialVelocity,
        LayerMask mask,
        Transform visual,
        Action<Vector3, Collider> explosionCallback,
        float gravityScale,
        float targetFloorWorldY)
    {
        collisionMask = mask;
        visualRoot = visual;
        velocity = initialVelocity;
        gravityMultiplier = gravityScale;
        floorWorldY = targetFloorWorldY;
        onExploded = explosionCallback;
        spawnTime = Time.time;

        transform.SetParent(null, true);
        transform.position = startPosition;
        gameObject.SetActive(true);
        enabled = true;

        if (visualRoot != null)
        {
            visualRoot.gameObject.SetActive(true);
            visualRoot.SetParent(transform, false);
            visualRoot.localPosition = Vector3.zero;
            visualRoot.localRotation = Quaternion.identity;
            flightLight = visualRoot.GetComponentInChildren<Light>(true);
            if (flightLight != null)
            {
                flightLight.enabled = true;
            }
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        float elapsed = Time.time - spawnTime;
        Vector3 gravity = Physics.gravity * gravityMultiplier;
        Vector3 nextVelocity = velocity + gravity * deltaTime;
        Vector3 displacement = (velocity + nextVelocity) * 0.5f * deltaTime;
        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = currentPosition + displacement;
        float distance = displacement.magnitude;

        if (nextPosition.y <= floorWorldY)
        {
            Vector3 landingPoint = nextPosition;
            landingPoint.y = floorWorldY;
            Explode(landingPoint, null);
            return;
        }

        if (elapsed >= minFlightTime && distance > 0.0001f && Physics.SphereCast(
                currentPosition,
                collisionRadius,
                displacement.normalized,
                out RaycastHit hit,
                distance,
                collisionMask,
                QueryTriggerInteraction.Ignore))
        {
            Vector3 impactPoint = hit.point;
            impactPoint.y = Mathf.Max(impactPoint.y, floorWorldY);
            Explode(impactPoint, hit.collider);
            return;
        }

        transform.position = nextPosition;
        velocity = nextVelocity;

        if (visualRoot != null && velocity.sqrMagnitude > 0.01f)
        {
            visualRoot.rotation = Quaternion.LookRotation(velocity.normalized, Vector3.up);
        }

        if (elapsed >= maxLifetime)
        {
            Vector3 timeoutPoint = transform.position;
            timeoutPoint.y = floorWorldY;
            Explode(timeoutPoint, null);
        }
    }

    private void Explode(Vector3 position, Collider hitCollider)
    {
        enabled = false;
        transform.position = position;
        onExploded?.Invoke(position, hitCollider);

        if (flightLight != null)
        {
            flightLight.enabled = false;
        }

        if (visualRoot != null)
        {
            Destroy(visualRoot.gameObject);
            visualRoot = null;
        }

        gameObject.SetActive(false);
    }
}
