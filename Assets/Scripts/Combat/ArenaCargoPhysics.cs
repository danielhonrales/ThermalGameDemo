using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>Scripted delivery to the authored pose. Cargo never has a rigidbody.</summary>
public sealed class ArenaCargoPhysics : MonoBehaviour
{
    // Kept under the existing name so scene/script references remain valid.
    public static readonly Dictionary<int, ArenaCargoPhysics> Bodies = new Dictionary<int, ArenaCargoPhysics>();
    public int Id;
    public bool Released { get; private set; }
    private Vector3 rest, start, target;
    private Quaternion restRotation, startRotation, rotation;
    private float releasedAt;
    private bool landed, hasPose;
    public const float DropDuration = 0.35f;

    public static ArenaCargoPhysics Register(Transform cargo, int id)
    {
        ArenaLayouts.RemoveRigidbodies(cargo);
        var motion = cargo.GetComponent<ArenaCargoPhysics>();
        if (motion == null) motion = cargo.gameObject.AddComponent<ArenaCargoPhysics>();
        motion.Id = id; Bodies[id] = motion;
        motion.rest = cargo.position;
        motion.restRotation = cargo.rotation;
        return motion;
    }

    public void Release(Vector3 velocity, Vector3 spin)
    {
        if (Released) return;
        Released = true;
        gameObject.SetActive(true);
        start = transform.position;
        startRotation = transform.rotation;
        releasedAt = Time.time;
    }

    public void Receive(Vector3 position, Quaternion orientation)
    {
        if (NetworkServer.active) return;
        target = NetworkPlayerAlignment.TransformPoint(position);
        rotation = NetworkPlayerAlignment.TransformRotation(orientation);
        Released = true;
        if (!hasPose) transform.SetPositionAndRotation(target, rotation);
        hasPose = true;
        gameObject.SetActive(true);
    }

    public void TickDrop(float age)
    {
        if (!Released || landed) return;
        float t = Mathf.Clamp01(age / DropDuration);
        transform.SetPositionAndRotation(Vector3.Lerp(start, rest, t * t), Quaternion.Slerp(startRotation, restRotation, t));
        if (t < 1f) return;
        landed = true;
        if (NetworkServer.active)
            FusionRoundDirector.Active()?.SendCargoImpact(rest);
    }

    private void Update()
    {
        if (NetworkServer.active) TickDrop(Time.time - releasedAt);
        else if (hasPose)
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, target, 1f - Mathf.Exp(-25f * Time.deltaTime)),
                Quaternion.Slerp(transform.rotation, rotation, 1f - Mathf.Exp(-25f * Time.deltaTime)));
    }

    public static void Impact(Vector3 point)
    {
        ThermalFxLibrary.DustBurst(point, 1.2f);
        SynthAudio.PlayAt(SynthAudio.Clunk(), point, 0.35f, 0.8f);
    }

    private void OnDestroy()
    {
        if (Bodies.TryGetValue(Id, out var value) && value == this) Bodies.Remove(Id);
    }
}
