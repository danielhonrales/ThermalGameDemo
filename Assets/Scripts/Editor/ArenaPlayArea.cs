using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>Editor tools for fitting the arena into the real play space marked by <see cref="PlayAreaOutline"/>.</summary>
public static class ArenaPlayArea
{
    private const string ScenePath = "Assets/Scenes/Game.unity";

    /// <summary>Adds the floor outline, centred on the starting cover and at its floor height, if the scene has none.</summary>
    [MenuItem("Thermal/Arena/Add Play Area Outline")]
    public static void AddOutline()
    {
        if (Application.isPlaying) throw new Exception("Stop Play Mode before adding the play area outline.");
        if (UnityEngine.Object.FindFirstObjectByType<PlayAreaOutline>() != null)
        {
            Debug.Log("The scene already has a play area outline; move or resize the existing one.");
            return;
        }
        var layouts = Layouts();
        Bounds cover = RendererBounds(layouts.initialCover);
        var outline = new GameObject("Play Area Outline");
        Undo.RegisterCreatedObjectUndo(outline, "Add play area outline");
        outline.transform.SetParent(layouts.transform, false);
        // Cover rests on the floor, so its lowest point gives the floor height.
        Vector3 centre = layouts.transform.InverseTransformPoint(new Vector3(cover.center.x, cover.min.y, cover.center.z));
        outline.transform.localPosition = centre;
        outline.AddComponent<PlayAreaOutline>();
        EditorSceneManager.MarkSceneDirty(outline.scene);
        Selection.activeGameObject = outline;
        Debug.Log($"Added a play area outline under {layouts.name} at {centre}; save the scene to keep it.");
    }

    /// <summary>Logs every arena piece with whether it fits inside the outlined play area.</summary>
    [MenuItem("Thermal/Arena/List Objects Outside Play Area")]
    public static void ListObjects() => Debug.Log(Report());

    /// <summary>Batch: adds the outline if missing, saves the scene, and writes the report to -reportPath (or Temp/).</summary>
    public static void AddOutlineAndReport()
    {
        EditorSceneManager.OpenScene(ScenePath);
        AddOutline();
        EditorSceneManager.SaveOpenScenes();
        string report = Report();
        string[] args = Environment.GetCommandLineArgs();
        int i = Array.IndexOf(args, "-reportPath");
        string path = i >= 0 && i + 1 < args.Length ? args[i + 1] : "Temp/arena-play-area.txt";
        File.WriteAllText(path, report);
        Debug.Log(report);
    }

    private static ArenaLayouts Layouts()
    {
        var layouts = UnityEngine.Object.FindFirstObjectByType<ArenaLayouts>();
        if (layouts == null) throw new Exception("Open the authored Game scene first.");
        return layouts;
    }

    private static string Report()
    {
        var outline = UnityEngine.Object.FindFirstObjectByType<PlayAreaOutline>();
        if (outline == null) throw new Exception("Add a play area outline first (Thermal → Arena → Add Play Area Outline).");
        var layouts = Layouts();
        Transform area = outline.transform;
        var text = new StringBuilder();
        text.AppendLine($"Play area {outline.Size.x:0.##} m x {outline.Size.y:0.##} m, centred at {layouts.transform.InverseTransformPoint(area.position)} under {layouts.name}.");
        text.AppendLine("Positions and sizes are metres in the play area's frame: x across, z along, y up from its floor.");
        foreach (var environment in UnityEngine.Object.FindObjectsByType<VisualOnlyEnvironment>(FindObjectsSortMode.None))
        {
            // The virtual room is hidden inside this box so the real room shows through.
            var serialized = new SerializedObject(environment);
            var root = serialized.FindProperty("playfieldRoot").objectReferenceValue as Transform;
            Vector3 centre = serialized.FindProperty("playfieldLocalCenter").vector3Value;
            Vector3 size = serialized.FindProperty("playfieldSize").vector3Value;
            Vector3 inArea = area.InverseTransformPoint(root != null ? root.TransformPoint(centre) : centre);
            text.AppendLine($"Passthrough cutout ({environment.name}): {size.x:0.##} x {size.z:0.##} m, centre ({inArea.x:+0.00;-0.00}, {inArea.z:+0.00;-0.00}).");
        }

        var groups = new List<(string title, IEnumerable<Transform> items)>
        {
            ("Starting cover (drones carry these off at sudden death)", Children(layouts.initialCover)),
            ("Sudden-death cover (drones deliver these)", Children(layouts.suddenDeathCover)),
        };
        foreach (Transform child in layouts.transform)
            if (child.name.StartsWith("Deck details", StringComparison.OrdinalIgnoreCase))
                groups.Add(($"{Path(child, layouts.transform.parent)} (stay in both arenas)", Children(child)));
        groups.Add(("Spawn points", layouts.transform.Cast<Transform>()
            .Where(t => t.name == "SpawnPoints").SelectMany(Children)));

        foreach (var (title, items) in groups)
        {
            text.AppendLine().AppendLine(title + ":");
            foreach (Transform item in items)
            {
                Describe(text, outline, item, 1);
                // Groups whose parts are placed separately get their parts listed too.
                if (item.childCount > 1 && BoundsIn(area, item).size.magnitude > 2.5f)
                    foreach (Transform part in item)
                        Describe(text, outline, part, 2);
            }
        }
        return text.ToString();
    }

    private static void Describe(StringBuilder text, PlayAreaOutline outline, Transform item, int depth)
    {
        Bounds local = BoundsIn(outline.transform, item);
        string fit = Fit(outline, local);
        string name = new string(' ', 2 * (depth - 1)) + item.name + (item.gameObject.activeInHierarchy ? "" : " (inactive)");
        text.AppendLine($"  [{fit,-7}] {name,-44} centre ({local.center.x:+0.00;-0.00}, {local.center.z:+0.00;-0.00})"
            + (local.size == Vector3.zero ? "  no mesh" : $"  size {local.size.x:0.00} x {local.size.z:0.00} x {local.size.y:0.00} high"));
    }

    private static IEnumerable<Transform> Children(Transform parent)
        => parent == null ? Enumerable.Empty<Transform>() : parent.Cast<Transform>();

    private static string Fit(PlayAreaOutline outline, Bounds local)
    {
        var corners = new[] { local.min, local.max, new Vector3(local.min.x, 0f, local.max.z), new Vector3(local.max.x, 0f, local.min.z) };
        int inside = corners.Count(outline.Contains);
        return inside == corners.Length ? "inside" : outline.Contains(local.center) || inside > 0 ? "partly" : "outside";
    }

    /// <summary>The renderers' bounds in another transform's space (just the position for an empty object).</summary>
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

    private static Bounds RendererBounds(Transform root)
    {
        var renderers = root.GetComponentsInChildren<Renderer>(true).Where(r => !(r is ParticleSystemRenderer)).ToArray();
        if (renderers.Length == 0) return new Bounds(root.position, Vector3.zero);
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers) bounds.Encapsulate(renderer.bounds);
        return bounds;
    }

    private static string Path(Transform t, Transform stopAt)
    {
        string path = t.name;
        for (Transform p = t.parent; p != null && p != stopAt; p = p.parent) path = p.name + "/" + path;
        return path;
    }
}
