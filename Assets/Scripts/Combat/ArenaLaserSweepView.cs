using Mirror;
using UnityEngine;

/// <summary>Side-mounted beams driven by the shared fight clock; the same geometry feeds server damage.</summary>
[DisallowMultipleComponent]
public sealed class ArenaLaserSweepView : MonoBehaviour
{
    public const int Count = 4;
    public const float FirstSeconds = 10f;
    private const float AddEverySeconds = 15f;
    private const float HalfArena = 2.7f;
    private const float SweepSpeed = 0.52f;
    private const float HitWidth = 0.15f;
    private static readonly Vector2 Centre = new Vector2(0.275f, 0.71f);

    private readonly LineRenderer[] glows = new LineRenderer[Count];
    private readonly LineRenderer[] cores = new LineRenderer[Count];
    private Material material;
    private AudioSource proximityHum;

    public static float StartsAt(int index) => FirstSeconds + index * AddEverySeconds;

    public static bool TryGetBeam(int index, float elapsed, out Vector3 from, out Vector3 to)
    {
        from = to = Vector3.zero;
        if (index < 0 || index >= Count || elapsed < StartsAt(index)) return false;
        float travel = Mathf.PingPong((elapsed - StartsAt(index)) * SweepSpeed, HalfArena * 2f);
        if (index >= 2) travel = HalfArena * 2f - travel;
        float lane = travel - HalfArena;
        float height = (index & 1) == 0 ? 1.48f : 0.14f;
        if ((index & 1) == 0)
        {
            float x = Centre.x + lane;
            from = new Vector3(x, height, Centre.y - HalfArena);
            to = new Vector3(x, height, Centre.y + HalfArena);
        }
        else
        {
            float z = Centre.y + lane;
            from = new Vector3(Centre.x - HalfArena, height, z);
            to = new Vector3(Centre.x + HalfArena, height, z);
        }
        return true;
    }

    public static bool Hits(int index, float elapsed, Vector3 head, float standingEyeHeight)
    {
        if (!TryGetBeam(index, elapsed, out Vector3 from, out Vector3 to)) return false;
        bool alongZ = (index & 1) == 0;
        float lateral = alongZ ? Mathf.Abs(head.x - from.x) : Mathf.Abs(head.z - from.z);
        float along = alongZ ? head.z : head.x;
        float middle = alongZ ? Centre.y : Centre.x;
        if (lateral > HitWidth || Mathf.Abs(along - middle) > HalfArena) return false;
        // Head tracking detects a duck directly. A small rise in headset height is the
        // available proxy for stepping over a shin-high beam; Quest has no foot tracking.
        return alongZ ? head.y >= 1.28f : head.y <= standingEyeHeight + 0.07f;
    }

    private void Awake()
    {
        material = CombatVfxStyle.CreateMaterial("Arena lasers", Color.white);
        for (int i = 0; i < Count; i++)
        {
            glows[i] = CombatVfxStyle.CreateLine(transform, "Laser glow " + i, material, true, 0.09f);
            cores[i] = CombatVfxStyle.CreateLine(transform, "Laser core " + i, material, true, 0.024f);
            glows[i].positionCount = cores[i].positionCount = 2;
        }
        proximityHum = gameObject.AddComponent<AudioSource>();
        proximityHum.clip = SynthAudio.LaserProximityHum();
        proximityHum.loop = true;
        proximityHum.playOnAwake = false;
        proximityHum.spatialBlend = 0f;
        proximityHum.volume = 0f;
        proximityHum.Play();
    }

    public void Show(bool fighting, float elapsed)
    {
        Vector3 head = Vector3.zero;
        bool hasHead = fighting && NetworkClient.localPlayer != null;
        if (hasHead)
        {
            NetworkHeadTracker tracker = NetworkClient.localPlayer.GetComponent<NetworkHeadTracker>();
            hasHead = tracker != null;
            if (hasHead) head = tracker.CanonicalHeadPosition;
        }
        float closeness = 0f;
        for (int i = 0; i < Count; i++)
        {
            Vector3 from = Vector3.zero, to = Vector3.zero;
            bool visible = fighting && TryGetBeam(i, elapsed, out from, out to);
            glows[i].enabled = cores[i].enabled = visible;
            if (!visible) continue;
            Vector3 worldFrom = NetworkPlayerAlignment.HasCalibration
                ? NetworkPlayerAlignment.TransformPoint(from) : from;
            Vector3 worldTo = NetworkPlayerAlignment.HasCalibration
                ? NetworkPlayerAlignment.TransformPoint(to) : to;
            glows[i].SetPosition(0, worldFrom);
            glows[i].SetPosition(1, worldTo);
            cores[i].SetPosition(0, worldFrom);
            cores[i].SetPosition(1, worldTo);
            float pulse = 0.5f + 0.5f * Mathf.Sin(Time.time * 15f + i * 1.7f);
            glows[i].widthMultiplier = 0.09f * (0.9f + 0.1f * pulse);
            cores[i].widthMultiplier = 0.024f * (0.94f + 0.06f * pulse);
            glows[i].startColor = glows[i].endColor = new Color(1f, 0.08f, 0.02f, 0.23f + 0.13f * pulse);
            cores[i].startColor = cores[i].endColor = new Color(1f, 0.74f + 0.16f * pulse, 0.57f, 1f);
            if (hasHead)
            {
                Vector2 a = new Vector2(from.x, from.z);
                Vector2 b = new Vector2(to.x, to.z);
                Vector2 p = new Vector2(head.x, head.z);
                float projection = Mathf.Clamp01(Vector2.Dot(p - a, b - a) / (b - a).sqrMagnitude);
                float distance = Vector2.Distance(p, Vector2.Lerp(a, b, projection));
                closeness = Mathf.Max(closeness,
                    1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.2f, 1.6f, distance)));
            }
        }
        if (proximityHum != null)
            proximityHum.volume = Mathf.MoveTowards(proximityHum.volume, 0.3f * closeness, Time.deltaTime * 0.9f);
    }

    private void OnDestroy()
    {
        if (material != null) Destroy(material);
    }
}
