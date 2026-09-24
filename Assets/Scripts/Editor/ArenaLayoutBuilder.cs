using UnityEditor;
using UnityEngine;

/// <summary>
/// Builds the default 1v1 cover layout under ArenaRoot/GameplayRoot/GameplayCover from the
/// sci-fi kit. Mirror-symmetric about the plane between the two start pads, all cover low
/// enough (≤ ~1.3 m) to keep the room and the opponent visible. Each "Obstacle" child is one
/// unit the sudden-death drones can lift away.
/// </summary>
public static class ArenaLayoutBuilder
{
    private const float MirrorX = 0.275f; // midpoint between PlayerAStart and PlayerBStart
    private const string Kit = "Assets/Creepy_Cat/3D Scifi Kit Starter Kit_HD/Prefabs/";

    private struct Piece
    {
        public string name, prefab;
        public Vector3 position; // floor contact point (x, 0, z); y is extra lift
        public float yaw, scale;
        public bool mirror;
        public Piece[] stack;
    }

    [MenuItem("Thermal/Build Default Arena Layout")]
    public static void Build()
    {
        Transform cover = GameObject.Find("ArenaRoot/GameplayRoot/GameplayCover")?.transform;
        if (cover == null) { Debug.LogError("GameplayCover not found."); return; }
        Undo.RegisterFullObjectHierarchyUndo(cover.gameObject, "Build arena layout");

        // Retire the old loose crates; keep the structural posts.
        for (int i = cover.childCount - 1; i >= 0; i--)
        {
            Transform child = cover.GetChild(i);
            if (child.name.StartsWith("Crate") || child.name.StartsWith("Obstacle"))
                Undo.DestroyObjectImmediate(child.gameObject);
        }

        // Spread across the 5 x 5 m ceiling footprint, centred at z = 0.71 (ceiling centre).
        Piece[] layout =
        {
            // Centre: coolant stack breaks the direct line between the players.
            new Piece { name = "Coolant stack", prefab = "Stuff/Pipes_02", position = new Vector3(MirrorX, 0f, 0.71f), yaw = 45f, scale = 0.34f },
            // Forward crate stacks to duck behind.
            new Piece { name = "Crate stack", prefab = "Props/Crate_01", position = new Vector3(-0.7f, 0f, 2.2f), yaw = 18f, scale = 0.5f, mirror = true,
                stack = new[]
                {
                    new Piece { prefab = "Props/Crate_01", position = new Vector3(0.05f, 0.375f, -0.08f), yaw = -12f, scale = 0.5f },
                    new Piece { prefab = "Props/Crate_01", position = new Vector3(-0.02f, 0.75f, 0.1f), yaw = 7f, scale = 0.5f },
                } },
            // Low pipe runs you crouch behind.
            new Piece { name = "Pipe bundle", prefab = "Stuff/Pipes_01", position = new Vector3(-0.75f, 0f, -0.9f), yaw = 10f, scale = 0.34f, mirror = true },
            // See-through railings on the north and south edges of the lane.
            new Piece { name = "Railing north", prefab = "Fences/Fence_Short_01", position = new Vector3(MirrorX, 0f, 3.0f), yaw = 90f, scale = 0.65f },
            new Piece { name = "Railing south", prefab = "Fences/Fence_Short_01", position = new Vector3(MirrorX, 0f, -1.6f), yaw = 90f, scale = 0.65f },
            // Consoles behind each start pad, angled to open peeking lanes.
            new Piece { name = "Console", prefab = "Walls/Wall_Table_01", position = new Vector3(-1.7f, 0f, -1.3f), yaw = 35f, scale = 0.36f, mirror = true },
            // Raised deck plates in the far corners for height variety.
            new Piece { name = "Deck plate", prefab = "Stairways/Stairway_Plateform_01", position = new Vector3(-1.7f, 0f, 2.75f), yaw = 0f, scale = 0.2f, mirror = true },
        };

        int count = 0;
        foreach (Piece piece in layout)
        {
            count += Place(cover, piece, false);
            if (piece.mirror) count += Place(cover, piece, true);
        }
        EditorUtility.SetDirty(cover.gameObject);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(cover.gameObject.scene);
        Debug.Log($"Arena layout built: {count} obstacles.");
    }

    private static int Place(Transform cover, Piece piece, bool mirrored)
    {
        Vector3 position = piece.position;
        float yaw = piece.yaw;
        if (mirrored) { position.x = 2f * MirrorX - position.x; yaw = -yaw; }
        var root = new GameObject("Obstacle " + piece.name + (mirrored ? " B" : piece.mirror ? " A" : ""));
        Undo.RegisterCreatedObjectUndo(root, "Obstacle");
        root.transform.SetParent(cover, false);
        root.transform.position = new Vector3(position.x, cover.parent.position.y, position.z);
        root.transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        root.transform.localScale = Vector3.one;
        // Cover parent is non-uniformly scaled; keep obstacles at true world scale.
        Vector3 parentScale = cover.lossyScale;
        root.transform.localScale = new Vector3(1f / parentScale.x, 1f / parentScale.y, 1f / parentScale.z);

        AddPart(root.transform, piece, Vector3.zero, 0f);
        if (piece.stack != null)
            foreach (Piece extra in piece.stack) AddPart(root.transform, extra, extra.position, extra.yaw);

        // One box collider per obstacle, from its rendered bounds, so beams and drones interact.
        Bounds world = RenderBounds(root);
        var box = root.AddComponent<BoxCollider>();
        Quaternion inverse = Quaternion.Inverse(root.transform.rotation);
        box.center = root.transform.InverseTransformPoint(world.center);
        Vector3 localSize = inverse * world.size;
        Vector3 rootScale = root.transform.lossyScale;
        box.size = new Vector3(Mathf.Abs(localSize.x) / rootScale.x, Mathf.Abs(localSize.y) / rootScale.y,
            Mathf.Abs(localSize.z) / rootScale.z) * 0.92f;
        CombatLayers.SetLayerRecursively(root, LayerMask.NameToLayer(CombatLayers.GameplayCover));
        return 1;
    }

    private static void AddPart(Transform root, Piece piece, Vector3 offset, float yaw)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(Kit + piece.prefab + ".prefab");
        if (prefab == null) { Debug.LogWarning("Missing kit prefab " + piece.prefab); return; }
        var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, root);
        instance.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
        instance.transform.localScale = Vector3.one * piece.scale;
        foreach (Collider collider in instance.GetComponentsInChildren<Collider>()) Object.DestroyImmediate(collider);
        // Sit the part on the floor (plus offset) centred on the obstacle root.
        instance.transform.localPosition = Vector3.zero;
        Bounds b = RenderBounds(instance);
        Vector3 floorAnchor = root.position + root.rotation * offset;
        instance.transform.position += new Vector3(floorAnchor.x - b.center.x, floorAnchor.y - b.min.y, floorAnchor.z - b.center.z);
    }

    private static Bounds RenderBounds(GameObject go)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(go.transform.position, Vector3.zero);
        Bounds b = renderers[0].bounds;
        foreach (Renderer r in renderers) b.Encapsulate(r.bounds);
        return b;
    }
}
