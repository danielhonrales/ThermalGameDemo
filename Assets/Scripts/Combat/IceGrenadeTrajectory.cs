using UnityEngine;

/// <summary>The shared fixed-step flight and collision calculation for aiming and live grenades.</summary>
public static class IceGrenadeTrajectory
{
    public const float StepSeconds = 1f / 90f;

    public struct State
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public float Elapsed;
    }

    public struct Contact
    {
        public Vector3 Point;
        public Vector3 Normal;
        public Collider Collider;
    }

    public static bool Step(ref State state, Vector3 gravity, float floorY, float radius,
        float minimumFlightTime, LayerMask mask, out Contact contact)
    {
        Vector3 nextVelocity = state.Velocity + gravity * StepSeconds;
        Vector3 displacement = (state.Velocity + nextVelocity) * (0.5f * StepSeconds);
        Vector3 next = state.Position + displacement;
        float distance = displacement.magnitude;
        bool reachesFloor = next.y <= floorY;
        float floorFraction = reachesFloor && Mathf.Abs(displacement.y) > 0.000001f
            ? Mathf.Clamp01((floorY - state.Position.y) / displacement.y) : 1f;
        float castDistance = distance * floorFraction;
        state.Elapsed += StepSeconds;
        if (state.Elapsed >= minimumFlightTime && castDistance > 0.0001f
            && Physics.SphereCast(state.Position, radius, displacement / distance, out RaycastHit hit,
                castDistance, mask, QueryTriggerInteraction.Ignore))
        {
            contact = new Contact { Point = hit.point, Normal = hit.normal, Collider = hit.collider };
            state.Position = hit.point;
            return true;
        }
        if (reachesFloor)
        {
            Vector3 point = state.Position + displacement * floorFraction;
            point.y = floorY;
            contact = new Contact { Point = point, Normal = Vector3.up };
            state.Position = point;
            return true;
        }
        state.Position = next;
        state.Velocity = nextVelocity;
        contact = default;
        return false;
    }

    public static int Predict(Vector3 origin, Vector3 velocity, Vector3 gravity, float floorY,
        float radius, float minimumFlightTime, float maximumFlightTime, LayerMask mask,
        Vector3[] points, out Contact contact)
    {
        var state = new State { Position = origin, Velocity = velocity };
        int count = 0;
        points[count++] = origin;
        while (state.Elapsed < maximumFlightTime && count < points.Length)
        {
            bool hit = Step(ref state, gravity, floorY, radius, minimumFlightTime, mask, out contact);
            points[count++] = state.Position;
            if (hit) return count;
        }
        contact = new Contact { Point = new Vector3(state.Position.x, floorY, state.Position.z), Normal = Vector3.up };
        points[count - 1] = contact.Point;
        return count;
    }
}
