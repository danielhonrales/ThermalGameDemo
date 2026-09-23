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
    private IceGrenadeTrajectory.State flight;

    public Vector3 Velocity => velocity;
    public bool IsAlive => enabled && gameObject.activeInHierarchy;
    public float CollisionRadius => collisionRadius;
    public float MinimumFlightTime => minFlightTime;
    public float MaximumFlightTime => maxLifetime;

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
        flight = new IceGrenadeTrajectory.State { Position = startPosition, Velocity = initialVelocity };

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

    /// <summary>Stops an in-flight grenade without creating an explosion when its weapon is switched away.</summary>
    public void Cancel()
    {
        enabled = false;

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

    private void Update() => AdvanceTo(Time.time - spawnTime);

    private void AdvanceTo(float elapsed)
    {
        Vector3 gravity = Physics.gravity * gravityMultiplier;
        while (flight.Elapsed + IceGrenadeTrajectory.StepSeconds <= elapsed)
        {
            if (IceGrenadeTrajectory.Step(ref flight, gravity, floorWorldY, collisionRadius,
                minFlightTime, collisionMask, out var contact))
            {
                Explode(contact.Point, contact.Collider);
                return;
            }
            if (flight.Elapsed >= maxLifetime)
            {
                Explode(new Vector3(flight.Position.x, floorWorldY, flight.Position.z), null);
                return;
            }
        }
        transform.position = flight.Position;
        velocity = flight.Velocity;
        if (visualRoot != null && velocity.sqrMagnitude > 0.01f)
            visualRoot.rotation = Quaternion.LookRotation(velocity.normalized, Vector3.up);
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
