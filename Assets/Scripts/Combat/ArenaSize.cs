using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Square arena sizes chosen at the Pi menu. The scene is authored at <see cref="AuthoredSide"/>; another size
/// spreads the arena out (or draws it in) about the play area's centre: cover, spawn pads, deck details and
/// sudden-death deliveries move so their distance from the centre scales with the side, without changing
/// their own size. The play area outline, the ceiling's footprint, the moving wall's travel, the clock
/// boards, the ground-fire paths and the heal corners follow. The round director keeps every headset on the
/// same size and only applies a change between rounds.
/// </summary>
public static class ArenaSize
{
    /// <summary>Side of the play area the scene was authored at, in metres.</summary>
    public const float AuthoredSide = 3.9f;
    public static readonly float[] Options = { 3f, 3.5f, 4f, 4.5f, 5f };

    /// <summary>Side currently applied to the scene.</summary>
    public static float Side { get; private set; } = AuthoredSide;
    /// <summary>Applied side relative to the authored arena.</summary>
    public static float Factor => Side / AuthoredSide;
    /// <summary>A side chosen on this headset that hasn't been sent to the round director yet; 0 when none.</summary>
    public static float PendingRequest { get; private set; }
    /// <summary>The last side chosen on this headset, if any (shown on the menu).</summary>
    public static float Chosen { get; private set; }

    private sealed class Placed
    {
        public Transform target;
        public Vector3 arenaPoint;      // authored, in ArenaRoot's frame
        public Vector3 localScale;      // authored, for the ceiling's footprint
        public SlidingWall wall;
        public bool footprint;
    }

    private static readonly List<Placed> placed = new List<Placed>();
    private static Transform arena;
    private static PlayAreaOutline outline;
    private static Vector3 centre;    // play area centre in ArenaRoot's frame

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        Side = AuthoredSide;
        PendingRequest = Chosen = 0f;
        placed.Clear();
        arena = null;
        outline = null;
    }

    public static bool IsOption(float side)
    {
        foreach (float option in Options)
            if (Mathf.Abs(option - side) < 0.01f) return true;
        return false;
    }

    /// <summary>Called by the Pi menu; the round director sends it on for every headset.</summary>
    public static void Request(float side)
    {
        if (!IsOption(side)) return;
        PendingRequest = Chosen = side;
    }

    public static void ClearRequest() => PendingRequest = 0f;

    /// <summary>Lays the scene out for a square play area of this side (0 means the authored size).</summary>
    public static void Apply(float side)
    {
        if (side <= 0f) side = AuthoredSide;
        if (Mathf.Abs(side - Side) < 0.001f) return;

        // Sudden death remembers cover positions when it builds; put the cover back (before the layout is
        // read) and let it rebuild next round.
        var suddenDeath = Object.FindFirstObjectByType<SuddenDeathDirector>();
        if (suddenDeath != null)
        {
            suddenDeath.ResetArena();
            Object.DestroyImmediate(suddenDeath.gameObject);
        }
        if (!Capture()) return;

        Side = side;
        float k = Factor;
        foreach (Placed item in placed)
        {
            if (item.target == null) continue;
            Vector3 point = Spread(item.arenaPoint, k);
            if (item.wall != null) item.wall.SetHome(arena.TransformPoint(point), k);
            else item.target.position = arena.TransformPoint(point);
            if (item.footprint)
                item.target.localScale = new Vector3(item.localScale.x * k, item.localScale.y, item.localScale.z * k);
        }
        if (outline != null) outline.SetSize(new Vector2(side, side));
        Debug.Log($"[ArenaSize] Arena laid out for {side:0.##} x {side:0.##} m (x{k:0.###} of the authored {AuthoredSide} m).");
    }

    /// <summary>An arena-frame point moved away from (or towards) the play area's centre; height is kept.</summary>
    private static Vector3 Spread(Vector3 point, float k)
        => new Vector3(centre.x + (point.x - centre.x) * k, point.y, centre.z + (point.z - centre.z) * k);

    /// <summary>Records the authored layout the first time a size is applied.</summary>
    private static bool Capture()
    {
        if (arena != null) return true;
        arena = GameObject.Find("ArenaRoot")?.transform;
        if (arena == null) { Debug.LogWarning("[ArenaSize] No ArenaRoot in the scene; the arena size is unchanged."); return false; }
        outline = Object.FindFirstObjectByType<PlayAreaOutline>();
        centre = outline != null ? arena.InverseTransformPoint(outline.transform.position)
            : new Vector3(SuddenDeathDirector.PlayAreaCentre.x, 0f, SuddenDeathDirector.PlayAreaCentre.y);

        Transform gameplay = arena.Find("GameplayRoot");
        if (gameplay != null)
            foreach (Transform group in gameplay)
            {
                // The weapon rig and the outline itself stay put; every other group's pieces spread out.
                if (group.GetComponent<PlayAreaOutline>() != null || group.GetComponentInChildren<PalmBeamShooter>(true) != null) continue;
                AddChildren(group);
            }
        Transform ceiling = arena.Find("Environment/VirtualCeiloingAlwaysVisible");
        if (ceiling != null) Add(ceiling, footprint: true);
        return true;
    }

    private static void AddChildren(Transform group)
    {
        foreach (Transform child in group)
        {
            // A grouping object (such as "Deck details" inside the cover) spreads its own pieces instead.
            if (child.name == "Deck details" && child.childCount > 0) { AddChildren(child); continue; }
            Add(child, footprint: false);
        }
    }

    private static void Add(Transform target, bool footprint)
    {
        var wall = target.GetComponent<SlidingWall>();
        Vector3 world = wall != null ? wall.HomeWorld : target.position;
        placed.Add(new Placed
        {
            target = target, wall = wall, footprint = footprint,
            arenaPoint = arena.InverseTransformPoint(world), localScale = target.localScale,
        });
    }
}
