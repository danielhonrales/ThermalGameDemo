using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ArenaLayoutAuthoring
{
    [MenuItem("Thermal/Arena/Prepare Editable Layouts")]
    public static void Prepare()
    {
        var arena = GameObject.Find("ArenaRoot");
        if (arena == null) throw new Exception("Open Assets/Scenes/Game.unity first.");
        Transform gameplay = arena.transform.Find("GameplayRoot");
        var layouts = gameplay.GetComponent<ArenaLayouts>();
        if (layouts == null) layouts = Undo.AddComponent<ArenaLayouts>(gameplay.gameObject);
        layouts.initialCover = gameplay.Find("GameplayCover");
        if (layouts.suddenDeathCover == null)
        {
            var root = new GameObject("SuddenDeathLayout");
            Undo.RegisterCreatedObjectUndo(root, "Create sudden death layout");
            root.transform.SetParent(gameplay, false);
            layouts.suddenDeathCover = root.transform;
            Vector3[] positions = {
                new Vector3(-0.55f,0f,1.65f), new Vector3(-1.65f,0f,2.65f), new Vector3(1.35f,0f,1.9f),
                new Vector3(-0.55f,0f,-1.05f), new Vector3(1.35f,0f,-0.55f),
                new Vector3(-1.65f,0f,0.35f), new Vector3(2.3f,0f,1.45f)
            };
            float[] yaw = {40f,25f,-25f,-25f,25f,65f,-65f};
            for (int i = 0; i < positions.Length; i++)
            {
                var prefab = Resources.Load<GameObject>("ThermalArena/ShipCargo" + i % 4);
                if (prefab == null) throw new Exception("Missing ShipCargo prefab " + i % 4);
                var piece = (GameObject)PrefabUtility.InstantiatePrefab(prefab, root.transform);
                piece.name = (i >= 5 ? "Burning " : "Cover ") + (i + 1);
                piece.transform.localRotation = Quaternion.Euler(i % 2 == 0 ? 9f : -7f, yaw[i], i % 3 == 0 ? 16f : -11f);
                piece.transform.localScale = Vector3.one;
                Bounds bounds = MeshBounds(piece);
                Vector3 target = arena.transform.TransformPoint(positions[i]) + Vector3.up * 0.03f;
                piece.transform.position += target - new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
                ArenaLayouts.RemoveRigidbodies(piece.transform);
                CombatLayers.SetLayerRecursively(piece, CombatLayers.GameplayCoverLayer);
            }
        }
        ArenaLayouts.RemoveRigidbodies(layouts.initialCover);
        ArenaLayouts.RemoveRigidbodies(layouts.suddenDeathCover);
        layouts.initialCover.gameObject.SetActive(true);
        foreach (Transform child in layouts.initialCover) child.gameObject.SetActive(true);
        layouts.suddenDeathCover.gameObject.SetActive(false);
        EditorUtility.SetDirty(layouts);
        EditorSceneManager.MarkSceneDirty(gameplay.gameObject.scene);
        Validate(layouts);
    }

    private static Bounds MeshBounds(GameObject go)
    {
        bool any = false;
        Bounds result = default;
        foreach (var filter in go.GetComponentsInChildren<MeshFilter>(true))
        {
            if (filter.sharedMesh == null) continue;
            Bounds b = filter.sharedMesh.bounds;
            for (int i = 0; i < 8; i++)
            {
                Vector3 p = filter.transform.TransformPoint(b.center + Vector3.Scale(b.extents,
                    new Vector3((i & 1) == 0 ? -1 : 1, (i & 2) == 0 ? -1 : 1, (i & 4) == 0 ? -1 : 1)));
                if (!any) { result = new Bounds(p, Vector3.zero); any = true; } else result.Encapsulate(p);
            }
        }
        if (!any) throw new Exception("Cover has no mesh: " + go.name);
        return result;
    }

    public static void Validate(ArenaLayouts layouts)
    {
        if (layouts == null || layouts.initialCover == null || layouts.suddenDeathCover == null)
            throw new Exception("Both authored arena layouts are required.");
        if (layouts.suddenDeathCover.childCount == 0 || layouts.suddenDeathCover.childCount > 31)
            throw new Exception("SuddenDeathLayout requires 1–31 direct child obstacles.");
        int pickups = 0;
        foreach (Transform child in layouts.initialCover)
            if (child.name.StartsWith("Obstacle") || child.name.StartsWith("Crate")) pickups++;
        if (pickups > 31) throw new Exception("At most 31 initial drone loads are supported.");
        foreach (Transform root in new[] { layouts.initialCover, layouts.suddenDeathCover })
            if (root.GetComponentsInChildren<Rigidbody>(true).Length != 0)
                throw new Exception("Arena cover must not have rigidbodies.");
        Debug.Log("AUTHORED ARENA PASSED: both layouts present; no cover rigidbodies; drone count valid.");
    }

    [MenuItem("Thermal/Arena/Edit Starting Arena")]
    public static void EditInitial() => Preview(false);

    [MenuItem("Thermal/Arena/Edit Sudden Death Arena")]
    public static void EditSuddenDeath() => Preview(true);

    private static void Preview(bool sudden)
    {
        if (Application.isPlaying) throw new Exception("Stop Play Mode before editing a layout.");
        Prepare();
        var layouts = UnityEngine.Object.FindFirstObjectByType<ArenaLayouts>();
        Undo.RecordObjects(new UnityEngine.Object[] { layouts.initialCover.gameObject, layouts.suddenDeathCover.gameObject }, "Preview arena layout");
        // Persistent dressing stays visible in both previews, just as it does in the game.
        foreach (Transform child in layouts.initialCover)
        {
            bool movable = child.name.StartsWith("Obstacle") || child.name.StartsWith("Crate");
            Undo.RecordObject(child.gameObject, "Preview arena cover");
            child.gameObject.SetActive(!sudden || !movable);
        }
        layouts.suddenDeathCover.gameObject.SetActive(sudden);
        Selection.activeGameObject = (sudden ? layouts.suddenDeathCover : layouts.initialCover).gameObject;
        EditorSceneManager.MarkSceneDirty(layouts.gameObject.scene);
    }
}
