using UnityEditor;
using UnityEngine;

/// <summary>Editor controls for running this PC as the LAN match spectator in Play mode.</summary>
public static class SpectatorModeMenu
{
    private const string ToggleItem = "Thermal Demo/Play As Spectator";

    [MenuItem(ToggleItem, priority = 100)]
    private static void Toggle()
    {
        bool enabled = !EditorPrefs.GetBool(SpectatorSession.EditorPrefKey, false);
        EditorPrefs.SetBool(SpectatorSession.EditorPrefKey, enabled);
        Debug.Log(enabled
            ? "Spectator mode ON: Play mode joins the Quest match without a player. Show Display 1 (players) and Display 2 (arena cameras) in two Game views."
            : "Spectator mode OFF: Play mode runs as a normal player.");
    }

    [MenuItem(ToggleItem, true)]
    private static bool ToggleAvailable()
    {
        Menu.SetChecked(ToggleItem, EditorPrefs.GetBool(SpectatorSession.EditorPrefKey, false));
        return !EditorApplication.isPlayingOrWillChangePlaymode;
    }

    [MenuItem("Thermal Demo/Add Spectator Viewpoint From Scene View", priority = 101)]
    private static void AddViewpoint()
    {
        SceneView view = SceneView.lastActiveSceneView;
        if (view == null)
        {
            Debug.LogWarning("Open a Scene view and frame the shot first.");
            return;
        }

        Transform arena = GameObject.Find("ArenaRoot")?.transform;
        Transform group = arena != null ? arena.Find("SpectatorViewpoints") : null;
        if (arena != null && group == null)
        {
            group = new GameObject("SpectatorViewpoints").transform;
            group.SetParent(arena, false);
            Undo.RegisterCreatedObjectUndo(group.gameObject, "Add spectator viewpoint");
        }

        int number = group != null ? group.childCount + 1 : 1;
        var viewpoint = new GameObject($"Viewpoint {number:00}");
        Undo.RegisterCreatedObjectUndo(viewpoint, "Add spectator viewpoint");
        Transform eye = view.camera.transform;
        viewpoint.transform.SetPositionAndRotation(eye.position, eye.rotation);
        if (group != null) viewpoint.transform.SetParent(group, true);
        var marker = viewpoint.AddComponent<SpectatorViewpoint>();
        var serialized = new SerializedObject(marker);
        serialized.FindProperty("fieldOfView").floatValue = Mathf.Clamp(view.cameraSettings.fieldOfView, 20f, 110f);
        serialized.ApplyModifiedPropertiesWithoutUndo();
        Selection.activeGameObject = viewpoint;
        Debug.Log($"Added {viewpoint.name}. Once the scene has any viewpoints, the spectator gallery uses them instead of its generated cameras; save the scene to keep it.");
    }
}
