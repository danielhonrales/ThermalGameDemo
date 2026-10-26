using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Builds the play area's fixed walls under the starting cover, where they stay in both arenas: two
/// waist-high curved walls in opposite corners and a full-height wall with a window that slides
/// across the middle (<see cref="SlidingWall"/>). Their meshes are shaped here and textured from the
/// sci-fi kit's own trim sheets, so they match the rest of the arena. They are ordinary scene objects
/// afterwards, so they can be moved, rotated or resized; running this again rebuilds them at the
/// default places.
/// </summary>
public static class ArenaWalls
{
    private const string ScenePath = "Assets/Scenes/Game.unity";
    private const string AssetFolder = "Assets/Resources/ThermalArena";
    private const string KitMaterials = "Assets/Creepy_Cat/3D Scifi Kit Starter Kit_HD/Materials/";
    private static readonly string[] WallNames = { "Wall Curved A", "Wall Curved B", "Wall Sliding" };

    // Metres. Curved walls: waist high, a 60-degree arc facing the centre from each corner.
    private const float CurvedHeight = 1f, CurvedRadius = 1.1f, CurvedThickness = 0.18f;
    private const float CurvedArcDegrees = 60f, CornerInset = 0.15f;
    // Sliding wall: full height, top corners cut off, on a vented footing, with a tall window at shooting height.
    private const float SlidingWidth = 1.3f, SlidingHeight = 2.4f, SlidingThickness = 0.18f, Chamfer = 0.3f;
    private const float WindowWidth = 0.5f, WindowHeight = 0.75f, WindowCentre = 1.3f;
    private const float FootWidth = 1.5f, FootHeight = 0.16f, FootDepth = 0.36f;
    private const float SlidingPeriod = 12f;

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

        foreach (string name in WallNames)
        {
            Transform old = cover.Find(name);
            if (old != null) Undo.DestroyObjectImmediate(old.gameObject);
        }

        Material elevation = KitMaterial("HD_Wall_01_Norm");
        Material panels = KitMaterial("HD_Wall_02_Norm");
        Material glow = GlowMaterial();
        Mesh curved = SaveMesh(CurvedWallMesh(), "ArenaCurvedWall");
        Mesh sliding = SaveMesh(SlidingWallMesh(), "ArenaSlidingWall");

        float cornerX = area.Size.x * 0.5f - CornerInset, cornerZ = area.Size.y * 0.5f - CornerInset;
        AddWall(WallNames[0], curved, new[] { panels }, area.transform, cover, new Vector3(cornerX, 0f, cornerZ), 0f);
        AddWall(WallNames[1], curved, new[] { panels }, area.transform, cover, new Vector3(-cornerX, 0f, -cornerZ), 180f);
        // Its x (width) runs across the arena, between the players' lines of fire.
        GameObject wall = AddWall(WallNames[2], sliding, new[] { elevation, panels, glow }, area.transform, cover, Vector3.zero, 90f);
        float travel = Mathf.Max(0f, Mathf.Min(area.Size.x, area.Size.y) * 0.5f - CornerInset - FootWidth * 0.5f);
        var slide = wall.AddComponent<SlidingWall>();
        var serialized = new SerializedObject(slide);
        serialized.FindProperty("travel").floatValue = travel;
        serialized.FindProperty("period").floatValue = SlidingPeriod;
        serialized.ApplyModifiedPropertiesWithoutUndo();

        EditorSceneManager.MarkSceneDirty(cover.gameObject.scene);
        Selection.activeGameObject = wall;
        string overlaps = PathOverlaps(layouts, area, wall, travel);
        Debug.Log($"Added the arena walls under {cover.name}; save the scene to keep them. The sliding wall travels {travel:0.##} m either side of the centre."
            + (overlaps.Length > 0 ? "\nThese pieces sit in the sliding wall's path; move them clear:\n" + overlaps : ""));
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

    /// <summary>
    /// A 60-degree wall around the origin, centred on the -x -z diagonal so it curves toward the arena
    /// from a corner placed at the origin. Faces show the kit's banded panel; the top and ends its trim strips.
    /// </summary>
    public static Mesh CurvedWallMesh()
    {
        const int segments = 16;
        var mesh = new MeshBuilder(1);
        float inner = CurvedRadius - CurvedThickness * 0.5f, outer = CurvedRadius + CurvedThickness * 0.5f;
        float half = CurvedArcDegrees * 0.5f * Mathf.Deg2Rad, mid = 1.25f * Mathf.PI;
        Vector3 At(float radius, float angle, float y) => new Vector3(radius * Mathf.Cos(angle), y, radius * Mathf.Sin(angle));
        Vector3 Out(float angle) => new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

        for (int i = 0; i < segments; i++)
        {
            float a0 = mid - half + 2f * half * i / segments, a1 = mid - half + 2f * half * (i + 1) / segments;
            float s0 = (float)i / segments, s1 = (float)(i + 1) / segments;
            Vector3 n = Out((a0 + a1) * 0.5f);
            // The convex face looks into the arena; the inner one into the corner. Both read left to right
            // from in front: from the arena the lower angle is on the left, from the corner on the right.
            mesh.Face(0, n, new[] { At(outer, a0, 0f), At(outer, a1, 0f), At(outer, a1, CurvedHeight), At(outer, a0, CurvedHeight) },
                new[] { BandPanel.At(s0, 0), BandPanel.At(s1, 0), BandPanel.At(s1, 1), BandPanel.At(s0, 1) });
            mesh.Face(0, -n, new[] { At(inner, a0, 0f), At(inner, a1, 0f), At(inner, a1, CurvedHeight), At(inner, a0, CurvedHeight) },
                new[] { BandPanel.At(1 - s0, 0), BandPanel.At(1 - s1, 0), BandPanel.At(1 - s1, 1), BandPanel.At(1 - s0, 1) });
            mesh.Face(0, Vector3.up, new[] { At(inner, a0, CurvedHeight), At(inner, a1, CurvedHeight), At(outer, a1, CurvedHeight), At(outer, a0, CurvedHeight) },
                new[] { CapStrip.At(s0, 0), CapStrip.At(s1, 0), CapStrip.At(s1, 1), CapStrip.At(s0, 1) });
        }
        float first = mid - half, last = mid + half;
        Vector3 Tangent(float angle) => new Vector3(-Mathf.Sin(angle), 0f, Mathf.Cos(angle));
        mesh.Quad(0, -Tangent(first), At(inner, first, 0f), At(outer, first, 0f), At(outer, first, CurvedHeight), At(inner, first, CurvedHeight), SideStrip);
        mesh.Quad(0, Tangent(last), At(outer, last, 0f), At(inner, last, 0f), At(inner, last, CurvedHeight), At(outer, last, CurvedHeight), SideStrip);
        return mesh.Build("ArenaCurvedWall");
    }

    /// <summary>
    /// The sliding wall in its own frame: x across, y up, z through it. A slab with its top corners cut
    /// off and a window, on a vented footing. Submeshes: 0 the kit wall elevation on both faces, 1 kit
    /// trim on edges and footing, 2 the glowing window reveal.
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

        // Window reveal: the four faces lining the opening glow.
        Region full = new Region(0, 0, 2048, 2048);
        mesh.Quad(2, Vector3.up, new Vector3(-wx, sill, -t), new Vector3(wx, sill, -t), new Vector3(wx, sill, t), new Vector3(-wx, sill, t), full);
        mesh.Quad(2, Vector3.down, new Vector3(-wx, head, -t), new Vector3(wx, head, -t), new Vector3(wx, head, t), new Vector3(-wx, head, t), full);
        mesh.Quad(2, Vector3.right, new Vector3(-wx, sill, -t), new Vector3(-wx, head, -t), new Vector3(-wx, head, t), new Vector3(-wx, sill, t), full);
        mesh.Quad(2, Vector3.left, new Vector3(wx, sill, -t), new Vector3(wx, head, -t), new Vector3(wx, head, t), new Vector3(wx, sill, t), full);

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

    /// <summary>Cover pieces that the sliding wall would pass through, one per line.</summary>
    private static string PathOverlaps(ArenaLayouts layouts, PlayAreaOutline area, GameObject sliding, float travel)
    {
        Bounds wall = BoundsIn(area.transform, sliding.transform);
        Vector3 axis = area.transform.InverseTransformDirection(sliding.transform.right);
        Vector3 sweep = new Vector3(Mathf.Abs(axis.x), 0f, Mathf.Abs(axis.z)) * travel * 2f;
        var path = new Bounds(wall.center, wall.size + sweep);
        var pieces = layouts.initialCover.Cast<Transform>().Where(t => !WallNames.Contains(t.name))
            .Concat(layouts.suddenDeathCover.Cast<Transform>())
            .SelectMany(t => t.name.StartsWith("Deck details") ? t.Cast<Transform>() : new[] { t });
        var hits = new List<string>();
        foreach (Transform piece in pieces)
        {
            // Each part separately: a group's parts may be spread around the arena with nothing in the path.
            foreach (Renderer part in piece.GetComponentsInChildren<Renderer>(true).Where(r => !(r is ParticleSystemRenderer)))
            {
                Bounds b = BoundsIn(area.transform, part.transform);
                bool overlap = b.min.x < path.max.x && b.max.x > path.min.x && b.min.z < path.max.z && b.max.z > path.min.z;
                if (!overlap) continue;
                string name = part.transform == piece ? piece.name : $"{piece.name}/{part.name}";
                hits.Add($"  {name} at ({b.center.x:+0.00;-0.00}, {b.center.z:+0.00;-0.00}), {b.size.y:0.00} m high");
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
