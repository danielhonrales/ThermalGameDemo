using Mirror;
using UnityEngine;

/// <summary>
/// Slides its object back and forth along a direction in its own frame, either side of where it was
/// placed. The position comes from the match clock (Mirror's shared network time once connected), so
/// both headsets and the spectator see the wall in the same place without sending it over the network.
/// </summary>
[DisallowMultipleComponent]
public sealed class SlidingWall : MonoBehaviour
{
    [Tooltip("Metres the wall travels either side of its placed position.")]
    [SerializeField, Min(0f)] private float travel = 1f;
    [Tooltip("Seconds for one full trip there and back.")]
    [SerializeField, Min(1f)] private float period = 12f;
    [Tooltip("Direction of travel in the wall's own frame: x slides it sideways, z moves it face first.")]
    [SerializeField] private Vector3 axis = Vector3.right;

    private Vector3 home;
    private Transform homeParent;
    private bool homed;

    private float travelScale = 1f;

    public float Travel => travel * travelScale;
    private Vector3 Axis => axis.sqrMagnitude > 0.0001f ? axis.normalized : Vector3.right;

    /// <summary>The middle of the wall's track in world space.</summary>
    public Vector3 HomeWorld => !homed ? transform.position : homeParent != null ? homeParent.TransformPoint(home) : home;

    /// <summary>Moves the middle of the track and scales the travel (for <see cref="ArenaSize"/>).</summary>
    public void SetHome(Vector3 world, float travelFactor)
    {
        travelScale = travelFactor;
        if (!homed) { transform.position = world; return; }   // not playing: the placed position is the home
        home = homeParent != null ? homeParent.InverseTransformPoint(world) : world;
    }

    /// <summary>Offset along the travel direction at a clock time: a smooth back-and-forth that starts centred.</summary>
    public static float Offset(double time, float travel, float period)
        => travel * Mathf.Sin((float)(2.0 * System.Math.PI * (time % period) / period));

    private void Awake()
    {
        home = transform.localPosition;
        homeParent = transform.parent;
        homed = true;
    }

    private void Update()
    {
        // A drone carrying the wall off at sudden death moves it; it slides again once put back.
        if (transform.parent != homeParent) return;
        double time = NetworkClient.active || NetworkServer.active ? NetworkTime.time : Time.timeAsDouble;
        // In world metres, whatever the scale of the cover it sits in.
        Vector3 origin = homeParent != null ? homeParent.TransformPoint(home) : home;
        transform.position = origin + transform.TransformDirection(Axis) * Offset(time, Travel, period);
    }

    private void OnDrawGizmosSelected()
    {
        // The path the wall's centre covers while playing.
        Vector3 origin = Application.isPlaying && transform.parent != null ? transform.parent.TransformPoint(home) : transform.position;
        Vector3 direction = transform.TransformDirection(Axis);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin - direction * Travel, origin + direction * Travel);
    }
}
