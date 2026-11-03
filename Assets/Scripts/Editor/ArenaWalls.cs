using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Builds the play area's walls in the starting cover: two stepped 1.5 m curved walls in opposite corners, square
/// pillars toward the other two corners, and a wall with a window that slides sideways toward those pillars,
/// stopping short of them (<see cref="SlidingWall"/>). Named as
/// obstacles, so drones carry them off when sudden death begins. Their meshes are shaped here and
/// textured from the sci-fi kit's own trim sheets, so they match the rest of the arena. They are
/// ordinary scene objects afterwards, so they can be moved, rotated or resized; running this again
/// rebuilds them at the default places.
/// </summary>
public static class ArenaWalls
{
    private const string ScenePath = "Assets/Scenes/Game.unity";
    private const string AssetFolder = "Assets/Resources/ThermalArena";
    private const string KitMaterials = "Assets/Creepy_Cat/3D Scifi Kit Starter Kit_HD/Materials/";
    private static readonly string[] WallNames =
        { "Obstacle Wall Curved A", "Obstacle Wall Curved B", "Obstacle Wall Sliding", "Obstacle Pillar C", "Obstacle Pillar D" };
    // Earlier builds' names, removed on rebuild.
    private static readonly string[] OldWallNames =
        { "Wall Curved A", "Wall Curved B", "Wall Sliding", "Obstacle Wall Curved C", "Obstacle Wall Curved D" };

    // Metres. Curved walls: a 45-degree arc facing the centre from each corner, 1.5 m tall over its left
    // half (as seen from the arena) and half that over its right half.
    private const float CurvedHeight = 1.5f, CurvedRadius = 1.1f, CurvedThickness = 0.09f;
    private const float CurvedArcDegrees = 45f, CornerInset = 0.15f;
    // The other two corners: square pillars as tall as the curved walls, a face toward the centre, standing as far
    // in from their corners as the middles of the curved walls' arcs.
    public const float PillarWidth = 0.45f, PillarHeight = CurvedHeight;
    // Sliding wall: a person's height with its top corners cut off, on a vented footing, with a window at shoulder height.
    private const float SlidingWidth = 0.86f, SlidingHeight = 1.75f, SlidingThickness = 0.09f, Chamfer = 0.12f;
    private const float WindowWidth = 0.56f, WindowHeight = 0.525f, WindowCentre = 1.32f;
    private const float FootWidth = 1f, FootHeight = 0.16f, FootDepth = 0.18f;
    // Gap left between the sliding wall's footing and a corner wall at the end of its travel.
    private const float SlidingClearance = 0.05f;
    // Seconds for one trip from one empty corner to the other and back.
    private const float SlidingPeriod = 36f;

    /// <summary>A region of a 2048-pixel trim sheet, in pixels from its top-left corner.</summary>
    private readonly struct Region
    {
        private readonly float x0, y0, x1, y1;
        public Region(float x0, float y0, float x1, float y1) { this.x0 = x0; this.y0 = y0; this.x1 = x1; this.y1 = y1; }
        /// <summary>The UV at (s, t) across the region, s left to right and t bottom to top.</summary>
        public Vector2 At(float s, float t) => new Vector2((x0 + s * (x1 - x0)) / 2048f, 1f - (y1 - t * (y1 - y0)) / 2048f);
    }

    // HD_Wall_01: a whole wall elevation (red header, panel with a red band, console row, light boxes); its left half.
    private static readonly Region WallElevation = new Region(0, 0, 1024, 2048);
    // HD_Wall_02 regions.
    private static readonly Region BandPanel = new Region(36, 220, 948, 732);       // white panel with a red band
    private static readonly Region CapStrip = new Region(496, 12, 1272, 204);       // rounded strip with a red band
    private static readonly Region SideStrip = new Region(568, 1072, 760, 1884);    // red upright with a lamp
    private static readonly Region VentStrip = new Region(100, 744, 960, 888);      // louvred vent

    [MenuItem("Thermal/Arena/Add Arena Walls")]
    public static void Add()
    {
        if (Application.isPlaying) throw new Exception("Stop Play Mode before adding the arena walls.");
        var layouts = UnityEngine.Object.FindFirstObjectByType<ArenaLayouts>();
        var area = UnityEngine.Object.FindFirstObjectByType<PlayAreaOutline>();
        if (layouts == null || area == null)
            throw new Exception("Open the Game scene with its play area outline (Thermal → Arena → Add Play Area Outline) first.");
        Transform cover = layouts.initialCover;

        foreach (string name in WallNames.Concat(OldWallNames))
        {
            Transform old = cover.Find(name);
            if (old != null) Undo.DestroyObjectImmediate(old.gameObject);
        }

        Material elevation = KitMaterial("HD_Wall_01_Norm");
        Material panels = KitMaterial("HD_Wall_02_Norm");
        Material glow = GlowMaterial();
        Mesh curved = SaveMesh(CurvedWallMesh(), "ArenaCurvedWall");
        Mesh pillar = SaveMesh(PillarMesh(), "ArenaPillar");
        Mesh sliding = SaveMesh(SlidingWallMesh(), "ArenaSlidingWall");

        var corners = new List<GameObject>();
        foreach (var (name, at, cornerYaw, isPillar) in CornerWalls(area.Size))
            corners.Add(AddWall(name, isPillar ? pillar : curved, new[] { panels }, area.transform, cover, at, cornerYaw));

        // The sliding wall crosses the space between the tall curved walls: it travels on the line through
        // the middle perpendicular to the line joining their centres, sideways, its faces toward them,
        // toward the pillars.
        Vector3 across = SlidingAcross(area.Size, out float yaw);
        GameObject wall = AddWall(WallNames[2], sliding, new[] { elevation, panels, glow }, area.transform, cover, Vector3.zero, yaw);
        float travel = WallTravel(area.Size, across);
        var slide = wall.AddComponent<SlidingWall>();
        var serialized = new SerializedObject(slide);
        serialized.FindProperty("travel").floatValue = travel;
        serialized.FindProperty("period").floatValue = SlidingPeriod;
        serialized.FindProperty("axis").vector3Value = Vector3.right;
        serialized.ApplyModifiedPropertiesWithoutUndo();

        EditorSceneManager.MarkSceneDirty(cover.gameObject.scene);
        Selection.activeGameObject = wall;
        string overlaps = PathOverlaps(layouts, area, wall, Vector3.right, travel);
        // Report anything the pillars stand on.
        string underPillars = string.Join("\n", corners.Skip(2).Select(c => PathOverlaps(layouts, area, c, Vector3.right, 0f)).Where(s => s.Length > 0));
        Debug.Log($"Added the arena walls under {cover.name}; save the scene to keep them. The sliding wall travels {travel:0.##} m either side of the centre."
            + (overlaps.Length > 0 ? "\nThese pieces sit in the sliding wall's path; move them clear:\n" + overlaps : "")
            + (underPillars.Length > 0 ? "\nThese pieces overlap the pillars:\n" + underPillars : ""));
    }

    /// <summary>
    /// Each corner piece's position (in the play area's frame), its turn, and whether it is a pillar: a curved
    /// wall's corner point, or a pillar's centre.
    /// </summary>
    public static (string name, Vector3 at, float yaw, bool pillar)[] CornerWalls(Vector2 areaSize)
    {
        float x = areaSize.x * 0.5f - CornerInset, z = areaSize.y * 0.5f - CornerInset;
        float inward = CurvedRadius * Mathf.Sqrt(0.5f);
        float px = x - inward, pz = z - inward;
        // The mesh curves toward the arena from a corner at +x +z; turning it by 90 degrees at a time faces each other corner.
        // The pillars are turned 45 degrees so a face looks at the centre.
        return new[]
        {
            (WallNames[0], new Vector3(x, 0f, z), 0f, false),
            (WallNames[1], new Vector3(-x, 0f, -z), 180f, false),
            (WallNames[3], new Vector3(px, 0f, -pz), 45f, true),
            (WallNames[4], new Vector3(-px, 0f, pz), 45f, true),
        };
    }

    /// <summary>
    /// Points around a corner piece's foot in the play area's frame, close enough together that nothing a
    /// centimetre wide fits between them: a curved wall's vertices, or the edges of a pillar.
    /// </summary>
    public static IEnumerable<Vector3> Outline(Vector3 at, float yaw, bool pillar, Mesh curved)
    {
        Quaternion turn = Quaternion.Euler(0f, yaw, 0f);
        // The arc's faces are flat between vertices and bulge toward the arena, so the vertices are its nearest points.
        if (!pillar) return curved.vertices.Select(v => at + turn * v).ToArray();
        float h = PillarWidth * 0.5f;
        int steps = Mathf.CeilToInt(PillarWidth / 0.01f);
        var corners = new[] { new Vector3(-h, 0f, -h), new Vector3(h, 0f, -h), new Vector3(h, 0f, h), new Vector3(-h, 0f, h) };
        return Enumerable.Range(0, 4).SelectMany(side => Enumerable.Range(0, steps)
            .Select(i => at + turn * Vector3.Lerp(corners[side], corners[(side + 1) % 4], (float)i / steps))).ToArray();
    }

    /// <summary>The sliding wall's direction of travel in the play area's frame, and its yaw (width along that direction).</summary>
    public static Vector3 SlidingAcross(Vector2 areaSize, out float yaw)
    {
        Vector3 cornerA = CornerWalls(areaSize)[0].at;
        Vector3 inward = new Vector3(-1f, 0f, -1f).normalized * CurvedRadius;
        Vector3 between = ((cornerA + inward) - (-cornerA - inward)).normalized;
        yaw = Mathf.Atan2(between.x, between.z) * Mathf.Rad2Deg;
        return Vector3.Cross(Vector3.up, between);
    }

    /// <summary>
    /// How far the sliding wall can travel either way along <paramref name="across"/>: its footing stays inside
    /// the play area and stops <see cref="SlidingClearance"/> short of every corner piece.
    /// </summary>
    public static float WallTravel(Vector2 areaSize, Vector3 across)
    {
        float travel = SlidingTravel(areaSize, across, across);
        Vector3 depth = Vector3.Cross(Vector3.up, across).normalized;
        Mesh curved = CurvedWallMesh();
        try
        {
            foreach (var (_, at, yaw, isPillar) in CornerWalls(areaSize))
                foreach (Vector3 p in Outline(at, yaw, isPillar, curved))
                {
                    if (Mathf.Abs(Vector3.Dot(p, depth)) > FootDepth * 0.5f + SlidingClearance) continue;
                    travel = Mathf.Min(travel, Mathf.Abs(Vector3.Dot(p, across)) - FootWidth * 0.5f - SlidingClearance);
                }
        }
        finally { UnityEngine.Object.DestroyImmediate(curved); }
        return Mathf.Max(0f, travel);
    }

    /// <summary>Batch: builds the walls in the Game scene and saves it.</summary>
    public static void AddAndSave()
    {
        EditorSceneManager.OpenScene(ScenePath);
        Add();
        EditorSceneManager.SaveOpenScenes();
    }

    private static GameObject AddWall(string name, Mesh mesh, Material[] materials, Transform area, Transform parent,
        Vector3 local, float yaw)
    {
        var wall = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(wall, "Add arena walls");
        wall.AddComponent<MeshFilter>().sharedMesh = mesh;
        wall.AddComponent<MeshRenderer>().sharedMaterials = materials;
        wall.AddComponent<MeshCollider>().sharedMesh = mesh;
        wall.transform.SetPositionAndRotation(area.TransformPoint(local), area.rotation * Quaternion.Euler(0f, yaw, 0f));
        wall.transform.SetParent(parent, true);
        wall.layer = CombatLayers.GameplayCoverLayer;
        return wall;
    }

    /// <summary>Collects faces into a mesh with one submesh per material; every face is wound to point along the normal it is given.</summary>
    private sealed class MeshBuilder
    {
        private readonly List<Vector3> vertices = new List<Vector3>();
        private readonly List<Vector2> uvs = new List<Vector2>();
        private readonly List<int>[] submeshes;

        public MeshBuilder(int materials) => submeshes = Enumerable.Range(0, materials).Select(_ => new List<int>()).ToArray();

        /// <summary>A convex polygon, fanned from its first corner.</summary>
        public void Face(int submesh, Vector3 normal, IReadOnlyList<Vector3> corners, IReadOnlyList<Vector2> uv)
        {
            int start = vertices.Count;
            vertices.AddRange(corners);
            uvs.AddRange(uv);
            for (int i = 1; i + 1 < corners.Count; i++)
            {
                int a = start, b = start + i, c = start + i + 1;
                // Unity draws the side that cross(b - a, c - a) points to.
                if (Vector3.Dot(Vector3.Cross(corners[i] - corners[0], corners[i + 1] - corners[0]), normal) < 0f) (b, c) = (c, b);
                submeshes[submesh].AddRange(new[] { a, b, c });
            }
        }

        /// <summary>A rectangle a-b-c-d (in order around it) showing a whole region, s along a-b and t along a-d.</summary>
        public void Quad(int submesh, Vector3 normal, Vector3 a, Vector3 b, Vector3 c, Vector3 d, Region region)
            => Face(submesh, normal, new[] { a, b, c, d },
                new[] { region.At(0, 0), region.At(1, 0), region.At(1, 1), region.At(0, 1) });

        public Mesh Build(string name)
        {
            var mesh = new Mesh { name = name };
            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.subMeshCount = submeshes.Length;
            for (int i = 0; i < submeshes.Length; i++) mesh.SetTriangles(submeshes[i], i);
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();
            return mesh;
        }
    }

    /// <summary>A 1.5 m, 45-degree curved wall: <see cref="CurvedWallMesh(float, float)"/> at the default size.</summary>
    public static Mesh CurvedWallMesh() => CurvedWallMesh(CurvedHeight, CurvedArcDegrees);

    /// <summary>
    /// A wall of <paramref name="arcDegrees"/> around the origin, centred on the -x -z diagonal so it curves toward the arena
    /// from a corner placed at the origin, full height over its left half and half height over its right.
    /// Faces show the kit's banded panel once per half-height row; the tops, ends and step show its trim strips.
    /// </summary>
    public static Mesh CurvedWallMesh(float height, float arcDegrees)
    {
        // About 3.75 degrees a segment, and an even count so the step falls between two of them.
        int segments = Mathf.Max(2, 2 * Mathf.RoundToInt(arcDegrees / 7.5f));
        int stepAt = segments / 2;      // the upper half runs over segments 0 .. stepAt-1 only
        float lowTop = height * 0.5f;
        var mesh = new MeshBuilder(1);
        float inner = CurvedRadius - CurvedThickness * 0.5f, outer = CurvedRadius + CurvedThickness * 0.5f;
        float halfArc = arcDegrees * 0.5f * Mathf.Deg2Rad, mid = 1.25f * Mathf.PI;
        Vector3 At(float radius, float angle, float y) => new Vector3(radius * Mathf.Cos(angle), y, radius * Mathf.Sin(angle));
        Vector3 Out(float angle) => new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        float Angle(int i) => mid - halfArc + 2f * halfArc * i / segments;

        for (int i = 0; i < segments; i++)
        {
            float a0 = Angle(i), a1 = Angle(i + 1);
            float s0 = (float)i / segments, s1 = (float)(i + 1) / segments;
            Vector3 n = Out((a0 + a1) * 0.5f);
            // From the arena the lower angle is on the left: the lower half runs the whole arc and the
            // upper half only its left half, stepping down in the middle.
            bool tall = i < stepAt;
            // The convex face looks into the arena; the inner one into the corner. Both read left to right
            // from in front: from the arena the lower angle is on the left, from the corner on the right.
            for (int row = 0; row < (tall ? 2 : 1); row++)
            {
                float y0 = row * lowTop, y1 = y0 + lowTop;
                mesh.Face(0, n, new[] { At(outer, a0, y0), At(outer, a1, y0), At(outer, a1, y1), At(outer, a0, y1) },
                    new[] { BandPanel.At(s0, 0), BandPanel.At(s1, 0), BandPanel.At(s1, 1), BandPanel.At(s0, 1) });
                mesh.Face(0, -n, new[] { At(inner, a0, y0), At(inner, a1, y0), At(inner, a1, y1), At(inner, a0, y1) },
                    new[] { BandPanel.At(1 - s0, 0), BandPanel.At(1 - s1, 0), BandPanel.At(1 - s1, 1), BandPanel.At(1 - s0, 1) });
            }
            float top = tall ? height : lowTop;
            mesh.Face(0, Vector3.up, new[] { At(inner, a0, top), At(inner, a1, top), At(outer, a1, top), At(outer, a0, top) },
                new[] { CapStrip.At(s0, 0), CapStrip.At(s1, 0), CapStrip.At(s1, 1), CapStrip.At(s0, 1) });
        }
        float first = Angle(0), step = Angle(stepAt), last = Angle(segments);
        Vector3 Tangent(float angle) => new Vector3(-Mathf.Sin(angle), 0f, Mathf.Cos(angle));
        // Ends: full height on the left, half height on the right, and the step between the two halves.
        mesh.Quad(0, -Tangent(first), At(inner, first, 0f), At(outer, first, 0f), At(outer, first, height), At(inner, first, height), SideStrip);
        mesh.Quad(0, Tangent(last), At(outer, last, 0f), At(inner, last, 0f), At(inner, last, lowTop), At(outer, last, lowTop), SideStrip);
        mesh.Quad(0, Tangent(step), At(outer, step, lowTop), At(inner, step, lowTop), At(inner, step, height), At(outer, step, height), SideStrip);
        return mesh.Build("ArenaCurvedWall");
    }

    /// <summary>
    /// A square pillar standing on the origin, <see cref="PillarWidth"/> across and <see cref="PillarHeight"/> tall.
    /// Each side shows the kit's banded panel once per half-height row between red uprights on its corners;
    /// a trim strip caps it.
    /// </summary>
    public static Mesh PillarMesh()
    {
        var mesh = new MeshBuilder(1);
        float h = PillarWidth * 0.5f, half = PillarHeight * 0.5f;
        // The corner uprights take this much of each side.
        const float upright = 0.06f;
        Vector3 top = Vector3.up * PillarHeight;
        var corners = new[] { new Vector3(-h, 0f, -h), new Vector3(h, 0f, -h), new Vector3(h, 0f, h), new Vector3(-h, 0f, h) };
        for (int side = 0; side < 4; side++)
        {
            Vector3 a = corners[side], b = corners[(side + 1) % 4];
            Vector3 normal = (a + b).normalized, along = (b - a).normalized;
            Vector3 a1 = a + along * upright, b1 = b - along * upright;
            for (int row = 0; row < 2; row++)
            {
                Vector3 y0 = Vector3.up * (row * half), y1 = Vector3.up * ((row + 1) * half);
                mesh.Quad(0, normal, a1 + y0, b1 + y0, b1 + y1, a1 + y1, BandPanel);
            }
            mesh.Quad(0, normal, a, a1, a1 + top, a + top, SideStrip);
            mesh.Quad(0, normal, b1, b, b + top, b1 + top, SideStrip);
        }
        mesh.Quad(0, Vector3.up, corners[0] + top, corners[1] + top, corners[2] + top, corners[3] + top, CapStrip);
        return mesh.Build("ArenaPillar");
    }

    /// <summary>
    /// The sliding wall in its own frame: x across, y up, z through it. A slab with its top corners cut
    /// off and a window, on a vented footing. Submeshes: 0 the kit wall elevation on both faces, 1 kit
    /// trim on edges, window reveal and footing, 2 the glowing lip around the window.
    /// </summary>
    public static Mesh SlidingWallMesh()
    {
        var mesh = new MeshBuilder(3);
        float w = SlidingWidth * 0.5f, h = SlidingHeight, t = SlidingThickness * 0.5f;
        float wx = WindowWidth * 0.5f, sill = WindowCentre - WindowHeight * 0.5f, head = WindowCentre + WindowHeight * 0.5f;
        Vector2 Elevation(Vector3 p, bool back) => WallElevation.At(back ? (w - p.x) / SlidingWidth : (p.x + w) / SlidingWidth, p.y / h);

        // The slab's face, split into convex pieces around the window: below, beside, and the cut-off top.
        var pieces = new List<Vector2[]>
        {
            new[] { new Vector2(-w, 0), new Vector2(w, 0), new Vector2(w, sill), new Vector2(-w, sill) },
            new[] { new Vector2(-w, sill), new Vector2(-wx, sill), new Vector2(-wx, head), new Vector2(-w, head) },
            new[] { new Vector2(wx, sill), new Vector2(w, sill), new Vector2(w, head), new Vector2(wx, head) },
            new[] { new Vector2(-w, head), new Vector2(w, head), new Vector2(w, h - Chamfer), new Vector2(w - Chamfer, h),
                    new Vector2(-w + Chamfer, h), new Vector2(-w, h - Chamfer) },
        };
        foreach (Vector2[] piece in pieces)
        {
            Vector3[] front = piece.Select(p => new Vector3(p.x, p.y, -t)).ToArray();
            Vector3[] back = piece.Select(p => new Vector3(p.x, p.y, t)).ToArray();
            mesh.Face(0, Vector3.back, front, front.Select(p => Elevation(p, false)).ToArray());
            mesh.Face(0, Vector3.forward, back, back.Select(p => Elevation(p, true)).ToArray());
        }

        // Outer edges, around the outline: trim strips running along each edge.
        var outline = new[] { new Vector2(w, 0), new Vector2(w, h - Chamfer), new Vector2(w - Chamfer, h),
                              new Vector2(-w + Chamfer, h), new Vector2(-w, h - Chamfer), new Vector2(-w, 0) };
        for (int i = 0; i + 1 < outline.Length; i++)
        {
            Vector2 a = outline[i], b = outline[i + 1];
            Vector2 edge = b - a;
            Vector3 normal = new Vector3(edge.y, -edge.x, 0f).normalized;
            if (Vector3.Dot(normal, new Vector3((a.x + b.x) * 0.5f, (a.y + b.y) * 0.5f - h * 0.5f, 0f)) < 0f) normal = -normal;
            var corners = new[] { new Vector3(a.x, a.y, -t), new Vector3(b.x, b.y, -t), new Vector3(b.x, b.y, t), new Vector3(a.x, a.y, t) };
            if (Mathf.Abs(edge.y) > Mathf.Abs(edge.x) * 2f)
                // Upright sides: the red upright runs up the edge, its lamp near the top.
                mesh.Face(1, normal, corners, corners.Select(p => SideStrip.At(p.z < 0 ? 0 : 1, p.y / (h - Chamfer))).ToArray());
            else
                mesh.Quad(1, normal, corners[0], corners[1], corners[2], corners[3], CapStrip);
        }

        // Window reveal: the four faces lining the opening carry the kit's rounded red-band trim, running
        // along each edge, so the opening reads as a fitted frame.
        mesh.Quad(1, Vector3.up, new Vector3(-wx, sill, -t), new Vector3(wx, sill, -t), new Vector3(wx, sill, t), new Vector3(-wx, sill, t), CapStrip);
        mesh.Quad(1, Vector3.down, new Vector3(-wx, head, -t), new Vector3(wx, head, -t), new Vector3(wx, head, t), new Vector3(-wx, head, t), CapStrip);
        mesh.Quad(1, Vector3.right, new Vector3(-wx, sill, -t), new Vector3(-wx, head, -t), new Vector3(-wx, head, t), new Vector3(-wx, sill, t), CapStrip);
        mesh.Quad(1, Vector3.left, new Vector3(wx, sill, -t), new Vector3(wx, head, -t), new Vector3(wx, head, t), new Vector3(wx, sill, t), CapStrip);

        // A thin glowing lip frames the opening on both faces, just proud of the surface.
        Region full = new Region(0, 0, 2048, 2048);
        const float lip = 0.025f, proud = 0.003f;
        foreach (float side in new[] { -1f, 1f })
        {
            float z = side * (t + proud);
            Vector3 facing = side < 0 ? Vector3.back : Vector3.forward;
            mesh.Quad(2, facing, new Vector3(-wx - lip, head, z), new Vector3(wx + lip, head, z), new Vector3(wx + lip, head + lip, z), new Vector3(-wx - lip, head + lip, z), full);
            mesh.Quad(2, facing, new Vector3(-wx - lip, sill - lip, z), new Vector3(wx + lip, sill - lip, z), new Vector3(wx + lip, sill, z), new Vector3(-wx - lip, sill, z), full);
            mesh.Quad(2, facing, new Vector3(-wx - lip, sill, z), new Vector3(-wx, sill, z), new Vector3(-wx, head, z), new Vector3(-wx - lip, head, z), full);
            mesh.Quad(2, facing, new Vector3(wx, sill, z), new Vector3(wx + lip, sill, z), new Vector3(wx + lip, head, z), new Vector3(wx, head, z), full);
        }

        // Footing: a vented plinth wider and deeper than the slab.
        float fx = FootWidth * 0.5f, fz = FootDepth * 0.5f, fy = FootHeight;
        mesh.Quad(1, Vector3.back, new Vector3(-fx, 0, -fz), new Vector3(fx, 0, -fz), new Vector3(fx, fy, -fz), new Vector3(-fx, fy, -fz), VentStrip);
        mesh.Quad(1, Vector3.forward, new Vector3(fx, 0, fz), new Vector3(-fx, 0, fz), new Vector3(-fx, fy, fz), new Vector3(fx, fy, fz), VentStrip);
        mesh.Quad(1, Vector3.left, new Vector3(-fx, 0, fz), new Vector3(-fx, 0, -fz), new Vector3(-fx, fy, -fz), new Vector3(-fx, fy, fz), CapStrip);
        mesh.Quad(1, Vector3.right, new Vector3(fx, 0, -fz), new Vector3(fx, 0, fz), new Vector3(fx, fy, fz), new Vector3(fx, fy, -fz), CapStrip);
        mesh.Quad(1, Vector3.up, new Vector3(-fx, fy, -fz), new Vector3(fx, fy, -fz), new Vector3(fx, fy, fz), new Vector3(-fx, fy, fz), CapStrip);
        return mesh.Build("ArenaSlidingWall");
    }

    private static Material KitMaterial(string name)
    {
        var material = AssetDatabase.LoadAssetAtPath<Material>(KitMaterials + name + ".mat");
        if (material == null) throw new Exception($"Missing sci-fi kit material {name}.");
        return material;
    }

    private static Material GlowMaterial()
    {
        string path = $"{AssetFolder}/ArenaWallGlow.mat";
        var material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material != null) return material;
        // The kit's cyan light-box colour.
        var cyan = new Color(0.62f, 0.98f, 1f);
        material = new Material(Shader.Find("Universal Render Pipeline/Lit")) { name = "ArenaWallGlow" };
        material.SetColor("_BaseColor", cyan * 0.5f);
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", cyan * 1.4f);
        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
        AssetDatabase.CreateAsset(material, path);
        return material;
    }

    private static Mesh SaveMesh(Mesh mesh, string name)
    {
        string path = $"{AssetFolder}/{name}.asset";
        var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if (existing == null) { AssetDatabase.CreateAsset(mesh, path); return mesh; }
        existing.Clear();
        EditorUtility.CopySerialized(mesh, existing);
        AssetDatabase.SaveAssets();
        return existing;
    }

    /// <summary>
    /// How far the sliding wall's centre can travel either way along <paramref name="across"/> (in the play
    /// area's frame), with its width along <paramref name="width"/>, while all of its footing stays at least
    /// the corner inset inside the play area.
    /// </summary>
    public static float SlidingTravel(Vector2 areaSize, Vector3 across, Vector3 width)
    {
        float hx = areaSize.x * 0.5f - CornerInset, hz = areaSize.y * 0.5f - CornerInset;
        Vector3 depth = Vector3.Cross(Vector3.up, width).normalized;
        var footprint = new List<Vector3>();
        foreach (float s in new[] { -1f, 1f })
            foreach (float u in new[] { -1f, 1f })
                footprint.Add(width * (s * FootWidth * 0.5f) + depth * (u * FootDepth * 0.5f));
        bool Inside(float d) => footprint.All(c =>
        {
            Vector3 p = across * d + c;
            return Mathf.Abs(p.x) <= hx && Mathf.Abs(p.z) <= hz;
        });
        float travel = 0f;
        while (Inside(travel + 0.01f) && Inside(-travel - 0.01f)) travel += 0.01f;
        return travel;
    }

    /// <summary>Cover pieces that the sliding wall would pass through, one per line.</summary>
    private static string PathOverlaps(ArenaLayouts layouts, PlayAreaOutline area, GameObject sliding, Vector3 axis, float travel)
    {
        // Measured in the wall's own frame, where it travels along an axis.
        Bounds wall = sliding.GetComponent<MeshFilter>().sharedMesh.bounds;
        var path = new Bounds(wall.center, wall.size + Vector3.Scale(axis, axis) * travel * 2f);
        var pieces = layouts.initialCover.Cast<Transform>().Where(t => !WallNames.Contains(t.name))
            .Concat(layouts.suddenDeathCover.Cast<Transform>())
            .SelectMany(t => t.name.StartsWith("Deck details") ? t.Cast<Transform>() : new[] { t });
        var hits = new List<string>();
        foreach (Transform piece in pieces)
        {
            // Each part separately: a group's parts may be spread around the arena with nothing in the path.
            foreach (Renderer part in piece.GetComponentsInChildren<Renderer>(true).Where(r => !(r is ParticleSystemRenderer)))
            {
                Bounds b = BoundsIn(sliding.transform, part.transform);
                bool overlap = b.min.x < path.max.x && b.max.x > path.min.x && b.min.z < path.max.z && b.max.z > path.min.z;
                if (!overlap) continue;
                string name = part.transform == piece ? piece.name : $"{piece.name}/{part.name}";
                Bounds at = BoundsIn(area.transform, part.transform);
                hits.Add($"  {name} at ({at.center.x:+0.00;-0.00}, {at.center.z:+0.00;-0.00}), {at.size.y:0.00} m high");
            }
        }
        return string.Join("\n", hits.Distinct());
    }

    private static Bounds BoundsIn(Transform frame, Transform item)
    {
        var renderers = item.GetComponentsInChildren<Renderer>(true).Where(r => !(r is ParticleSystemRenderer)).ToArray();
        if (renderers.Length == 0) return new Bounds(frame.InverseTransformPoint(item.position), Vector3.zero);
        Bounds? result = null;
        foreach (Renderer renderer in renderers)
        {
            Bounds world = renderer.bounds;
            for (int c = 0; c < 8; c++)
            {
                Vector3 corner = world.center + Vector3.Scale(world.extents,
                    new Vector3((c & 1) == 0 ? -1 : 1, (c & 2) == 0 ? -1 : 1, (c & 4) == 0 ? -1 : 1));
                Vector3 local = frame.InverseTransformPoint(corner);
                if (result == null) result = new Bounds(local, Vector3.zero);
                else { var b = result.Value; b.Encapsulate(local); result = b; }
            }
        }
        return result.Value;
    }
}
