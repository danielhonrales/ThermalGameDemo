using Mirror;
using UnityEngine;

/// <summary>
/// Slides its object back and forth along its own x axis, either side of where it was placed. The
/// position comes from the match clock (Mirror's shared network time once connected), so both
/// headsets and the spectator see the wall in the same place without sending it over the network.
/// </summary>
[DisallowMultipleComponent]
public sealed class SlidingWall : MonoBehaviour
{
    [Tooltip("Metres the wall travels either side of its placed position.")]
    [SerializeField, Min(0f)] private float travel = 1f;
    [Tooltip("Seconds for one full trip there and back.")]
    [SerializeField, Min(1f)] private float period = 12f;

    private Vector3 home;

    public float Travel => travel;

    /// <summary>Offset along the wall's x axis at a clock time: a smooth back-and-forth that starts centred.</summary>
    public static float Offset(double time, float travel, float period)
        => travel * Mathf.Sin((float)(2.0 * System.Math.PI * (time % period) / period));

    private void Awake() => home = transform.localPosition;

    private void Update()
    {
        double time = NetworkClient.active || NetworkServer.active ? NetworkTime.time : Time.timeAsDouble;
        transform.localPosition = home + transform.localRotation * Vector3.right * Offset(time, travel, period);
    }

    private void OnDrawGizmosSelected()
    {
        // The path the wall's centre covers while playing.
        Vector3 origin = Application.isPlaying && transform.parent != null ? transform.parent.TransformPoint(home) : transform.position;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin - transform.right * travel, origin + transform.right * travel);
    }
}
