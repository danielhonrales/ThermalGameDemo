using UnityEditor;
using UnityEngine;

/// <summary>
/// Builds the default 1v1 cover layout under ArenaRoot/GameplayRoot/GameplayCover from the
/// sci-fi kit. Mirror-symmetric about the plane between the two start pads. Each "Obstacle" child is one
/// unit the sudden-death drones can lift away.
/// </summary>
public static class ArenaLayoutBuilder
{
    private const float MirrorX = 0.275f; // midpoint between PlayerAStart and PlayerBStart
    private const string Kit = "Assets/Creepy_Cat/3D Scifi Kit Starter Kit_HD/Prefabs/";

    private struct Piece
    {
        public string name, prefab;
        public Vector3 size;
        public int variant;
        public Vector3 position; // floor contact point (x, 0, z); y is extra lift
        public float yaw, scale, heightMultiplier;
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
            if (child.name.StartsWith("Crate") || child.name.StartsWith("Obstacle") || child.name.StartsWith("Wood"))
                Undo.DestroyObjectImmediate(child.gameObject);
        }

        // Spread across the 5 x 5 m ceiling footprint, centred at z = 0.71 (ceiling centre).
        Piece[] layout =
        {
            new Piece { name = "Reactor", prefab = "Walls/Column_01_Big", position = new Vector3(MirrorX, 0f, 0.71f), size = new Vector3(0.8f, 1.4f, 0.7f), variant = 0 },
            new Piece { name = "Cargo locker", prefab = "Props/Crate_01", position = new Vector3(-1.25f, 0f, 2.3f), yaw = 12f, size = new Vector3(0.9f, 1.2f, 0.65f), variant = 1, mirror = true },
            new Piece { name = "Navigation console", prefab = "Walls/Wall_Console_01_Half", position = new Vector3(-1.2f, 0f, -0.8f), yaw = -12f, size = new Vector3(0.9f, 1.15f, 0.55f), variant = 2, mirror = true },
            new Piece { name = "Bulkhead module", prefab = "Walls/Wall_Cube_01_Flat", position = new Vector3(-1.9f, 0f, 0.7f), size = new Vector3(0.8f, 1.3f, 0.5f), variant = 3, mirror = true },
        };

        int count = 0;
        foreach (Piece piece in layout)
        {
            count += Place(cover, piece, false);
            if (piece.mirror) count += Place(cover, piece, true);
        }
        BuildWood(cover, 0, new Vector3(-2f, 0f, -0.95f));
        BuildWood(cover, 1, new Vector3(2.55f, 0f, -0.95f));
        BuildDressing(cover);
        MakeSceneTwoSided();
        var ceiling = GameObject.Find("ArenaRoot/Environment/VirtualCeiloingAlwaysVisible");
        if (ceiling != null)
        {
            // Anchor authored ceiling geometry deterministically. Renderer bounds can be stale
            // during batch scene loading and previously accumulated metres on every build.
            Vector3 position = ceiling.transform.localPosition;
            position.y = 0.73f;
            ceiling.transform.localPosition = position;
            ceiling.SetActive(true);
        }
        EditorUtility.SetDirty(cover.gameObject);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(cover.gameObject.scene);
        Debug.Log($"Arena layout built: {count} obstacles.");
    }

    private static void BuildWood(Transform cover, int id, Vector3 position)
    {
        var root = new GameObject("Wood shipping panel " + id);
        root.transform.SetParent(cover, false);
        root.transform.position = position;
        root.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        Vector3 scale = cover.lossyScale;
        root.transform.localScale = new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z);
        root.AddComponent<BurnableWood>().PanelId = id;
        Material rim = ThermalFxLibrary.Instance != null ? ThermalFxLibrary.Instance.droneHull : null;
        void Frame(string name, Vector3 at, Vector3 size, float tilt = 0f)
        {
            var part = GameObject.CreatePrimitive(PrimitiveType.Cube);
            part.name = name;
            part.transform.SetParent(root.transform, false);
            part.transform.localPosition = at;
            part.transform.localRotation = Quaternion.Euler(0f, 0f, tilt);
            part.transform.localScale = size;
            if (rim != null) part.GetComponent<Renderer>().sharedMaterial = rim;
        }
        Frame("Left armored edge", new Vector3(-0.48f, 0.56f, 0.02f), new Vector3(0.085f, 1.12f, 0.16f));
        Frame("Right armored edge", new Vector3(0.48f, 0.56f, 0.02f), new Vector3(0.085f, 1.12f, 0.16f));
        Frame("Upper shield crown", new Vector3(0f, 1.30f, 0.02f), new Vector3(0.78f, 0.1f, 0.16f));
        Frame("Left bevel", new Vector3(-0.425f, 1.19f, 0.02f), new Vector3(0.09f, 0.23f, 0.16f), -35f);
        Frame("Right bevel", new Vector3(0.425f, 1.19f, 0.02f), new Vector3(0.09f, 0.23f, 0.16f), 35f);
        Frame("Armored sill", new Vector3(0f, 0.035f, 0.02f), new Vector3(1.04f, 0.1f, 0.2f));
        var standPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Abandoned World/Metal and Concrete Barrier/Prefabs/Metal_Element_4.prefab");
        if (standPrefab != null)
        {
            var stand = (GameObject)PrefabUtility.InstantiatePrefab(standPrefab, root.transform);
            StripAmbientComponents(stand);
            stand.transform.rotation = Quaternion.identity;
            stand.transform.localScale = Vector3.one;
            Bounds b = RenderBounds(stand);
            stand.transform.localScale = new Vector3(0.95f / b.size.x, 0.18f / b.size.y, 0.42f / b.size.z);
            stand.transform.localRotation = Quaternion.identity;
            b = RenderBounds(stand);
            stand.transform.position += root.transform.position - new Vector3(b.center.x, b.min.y, b.center.z);
            foreach (var collider in stand.GetComponentsInChildren<Collider>()) Object.DestroyImmediate(collider);
        }
        var box = root.AddComponent<BoxCollider>();
        box.center = Vector3.up * BurnableWood.Height / 2f;
        box.size = new Vector3(BurnableWood.Width, BurnableWood.Height, 0.075f);
        var preview = GameObject.CreatePrimitive(PrimitiveType.Cube);
        preview.name = "Wood preview";
        Object.DestroyImmediate(preview.GetComponent<Collider>());
        preview.transform.SetParent(root.transform, false);
        preview.transform.localPosition = box.center;
        preview.transform.localScale = box.size;
        CombatLayers.SetLayerRecursively(root, CombatLayers.GameplayCoverLayer);
    }

    private static void BuildDressing(Transform parent)
    {
        var old = parent.Find("Deck details");
        if (old != null) Object.DestroyImmediate(old.gameObject);
        var details = new GameObject("Deck details").transform;
        details.SetParent(parent, false);
        Vector3 scale = parent.lossyScale;
        details.localScale = new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z);
        string[] samples =
        {
            "Walls/Wall_Cube_01_Round", "Walls/Wall_Cube_02_Flat", "Walls/Wall_Console_01_Large",
            "Walls/Wall_Cube_01_Invert", "Walls/Wall_Cube_01_Coin", "Props/Airing_01",
            "Stuff/Pipes_03", "Stuff/Pipes_01", "Stuff/Intercom_01", "Stuff/Intercom_02",
            "Fences/Fence_Angle_02", "Fences/Fence_Turn_90_Short_01",
            "Metal_Barrier_1", "Metal_Barrier_2", "Concrete_Barrier_1", "Concrete_Barrier_3",
            "Metal_Element_1", "Metal_Element_3", "Stuff/Beam_04", "Stuff/Air_Grid_01"
        };
        var inventory = new System.Text.StringBuilder("# Arena asset samples\n\nNumbers match the plaques in the scene.\n\n");
        for (int i = 0; i < samples.Length; i++)
        {
            string path = samples[i].Contains("/") ? Kit + samples[i] + ".prefab"
                : "Assets/Abandoned World/Metal and Concrete Barrier/Prefabs/" + samples[i] + ".prefab";
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) throw new System.Exception("Missing asset sample " + path);
            var prop = (GameObject)PrefabUtility.InstantiatePrefab(prefab, details);
            prop.name = (i + 1).ToString("00") + " " + samples[i];
            StripAmbientComponents(prop);
            prop.transform.rotation = Quaternion.Euler(0f, i % 2 == 0 ? 25f : -25f, 0f);
            prop.transform.localScale = Vector3.one;
            Bounds measured = RenderBounds(prop);
            float fit = Mathf.Min(0.65f / Mathf.Max(measured.size.x, measured.size.z), 1.1f / measured.size.y);
            prop.transform.localScale = Vector3.one * fit;
            Bounds bounds = RenderBounds(prop);
            // Six per side, then four along each end: a broad selection surrounding the playable lanes.
            Vector3 point;
            if (i < 12) point = new Vector3(i % 2 == 0 ? -2.65f : 3.2f, 0.012f, -1.45f + (i / 2) * 0.8f);
            else point = new Vector3(-1.55f + ((i - 12) % 4) * 1.2f, 0.012f, i < 16 ? -2.1f : 3.55f);
            prop.transform.position += point - new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
            foreach (var collider in prop.GetComponentsInChildren<Collider>()) Object.DestroyImmediate(collider);
            foreach (var mesh in prop.GetComponentsInChildren<MeshFilter>())
                if (mesh.sharedMesh != null) mesh.gameObject.AddComponent<MeshCollider>().sharedMesh = mesh.sharedMesh;
            CombatLayers.SetLayerRecursively(prop, CombatLayers.GameplayCoverLayer);
            var plaque = new GameObject("Sample number").AddComponent<TMPro.TextMeshPro>();
            plaque.transform.SetParent(details, false);
            plaque.transform.position = point + Vector3.up * 0.1f + (new Vector3(0.275f, 0f, 0.71f) - point).normalized * 0.38f;
            Vector3 facing = plaque.transform.position - new Vector3(0.275f, 0.1f, 0.71f);
            facing.y = 0f;
            plaque.transform.rotation = Quaternion.LookRotation(facing);
            plaque.font = Resources.Load<TMPro.TMP_FontAsset>("ThermalFX/Fonts/Roboto-Black SDF"); plaque.text = (i + 1).ToString("00");
            plaque.fontSize = 1.5f; plaque.alignment = TMPro.TextAlignmentOptions.Center;
            plaque.rectTransform.sizeDelta = new Vector2(0.3f, 0.15f);
            plaque.color = new Color(0.6f, 0.9f, 1f);
            inventory.AppendLine($"- **{i + 1:00}** — {samples[i]}");
        }
        System.IO.File.WriteAllText("docs/ARENA_ASSET_SAMPLES.md", inventory.ToString());
    }

    private static void MakeSceneTwoSided()
    {
        var materials = new System.Collections.Generic.HashSet<Material>();
        foreach (var renderer in Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            foreach (var material in renderer.sharedMaterials) if (material != null) materials.Add(material);
        foreach (var material in Resources.LoadAll<Material>("ThermalFX")) materials.Add(material);
        foreach (var material in materials)
        {
            if (material.shader.name == "Standard")
            {
                Texture texture = material.mainTexture; Color color = material.color;
                material.shader = Shader.Find("Universal Render Pipeline/Lit");
                material.SetTexture("_BaseMap", texture); material.SetColor("_BaseColor", color);
            }
            if (material.HasProperty("_Cull")) material.SetFloat("_Cull", 0f);
            if (material.HasProperty("_CullMode")) material.SetFloat("_CullMode", 0f);
            if (material.HasProperty("_CullModeForward")) material.SetFloat("_CullModeForward", 0f);
            material.doubleSidedGI = true;
            EditorUtility.SetDirty(material);
        }
        AssetDatabase.SaveAssets();
    }

    private static int Place(Transform cover, Piece piece, bool mirrored)
    {
        Vector3 position = piece.position;
        float yaw = piece.yaw;
        if (mirrored) { position.x = 2f * MirrorX - position.x; yaw = -yaw; }
        var root = new GameObject("Obstacle " + piece.name + (mirrored ? " B" : piece.mirror ? " A" : ""));
        Undo.RegisterCreatedObjectUndo(root, "Obstacle");
        root.transform.SetParent(cover, false);
        root.transform.position = new Vector3(position.x, cover.parent.position.y + position.y, position.z);
        root.transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        root.transform.localScale = Vector3.one;
        // Cover parent is non-uniformly scaled; keep obstacles at true world scale.
        Vector3 parentScale = cover.lossyScale;
        root.transform.localScale = new Vector3(1f / parentScale.x, 1f / parentScale.y, 1f / parentScale.z);

        AddPart(root.transform, piece, Vector3.zero, 0f);
        if (piece.variant == 0)
        {
            var core = GameObject.CreatePrimitive(PrimitiveType.Cube);
            core.name = "Solid reactor core";
            core.transform.SetParent(root.transform, false);
            core.transform.localPosition = new Vector3(0f, 0.68f, 0f);
            core.transform.localScale = new Vector3(0.65f, 1.22f, 0.6f);
            Object.DestroyImmediate(core.GetComponent<Collider>());
            if (ThermalFxLibrary.Instance != null)
                core.GetComponent<Renderer>().sharedMaterial = ThermalFxLibrary.Instance.droneBody;
        }
        if (piece.stack != null)
            foreach (Piece extra in piece.stack) AddPart(root.transform, extra, extra.position, extra.yaw);

        // Collider in obstacle-local space, fitted to the solid cargo mesh.
        Bounds local = new Bounds();
        bool first = true;
        foreach (MeshFilter filter in root.GetComponentsInChildren<MeshFilter>())
        {
            Bounds mesh = filter.sharedMesh.bounds;
            for (int corner = 0; corner < 8; corner++)
            {
                Vector3 point = mesh.center + Vector3.Scale(mesh.extents,
                    new Vector3((corner & 1) == 0 ? -1 : 1, (corner & 2) == 0 ? -1 : 1, (corner & 4) == 0 ? -1 : 1));
                point = root.transform.InverseTransformPoint(filter.transform.TransformPoint(point));
                if (first) { local = new Bounds(point, Vector3.zero); first = false; } else local.Encapsulate(point);
            }
        }
        var box = root.AddComponent<BoxCollider>();
        box.center = local.center;
        box.size = local.size;
        // The same textured cargo becomes damaged, tilted cover in sudden death.
        if (!mirrored)
        {
            System.IO.Directory.CreateDirectory("Assets/Resources/ThermalArena");
            PrefabUtility.SaveAsPrefabAsset(root, "Assets/Resources/ThermalArena/ShipCargo" + piece.variant + ".prefab");
        }
        CombatLayers.SetLayerRecursively(root, LayerMask.NameToLayer(CombatLayers.GameplayCover));
        return 1;
    }

    private static void AddPart(Transform root, Piece piece, Vector3 offset, float yaw)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(Kit + piece.prefab + ".prefab");
        if (prefab == null) { Debug.LogWarning("Missing kit prefab " + piece.prefab); return; }
        var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, root);
        StripAmbientComponents(instance);
        instance.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
        instance.transform.localScale = Vector3.one;
        foreach (Collider collider in instance.GetComponentsInChildren<Collider>()) Object.DestroyImmediate(collider);
        // Sit the part on the floor (plus offset) centred on the obstacle root.
        instance.transform.localPosition = Vector3.zero;
        Bounds measured = RenderBounds(instance);
        instance.transform.localScale = Vector3.Scale(instance.transform.localScale,
            new Vector3(piece.size.x / measured.size.x, piece.size.y / measured.size.y, piece.size.z / measured.size.z));
        Bounds b = RenderBounds(instance);
        Vector3 floorAnchor = root.position + root.rotation * offset;
        instance.transform.position += new Vector3(floorAnchor.x - b.center.x, floorAnchor.y - b.min.y, floorAnchor.z - b.center.z);
    }

    private static void StripAmbientComponents(GameObject go)
    {
        foreach (var source in go.GetComponentsInChildren<AudioSource>(true)) Object.DestroyImmediate(source);
        foreach (var light in go.GetComponentsInChildren<Light>(true)) Object.DestroyImmediate(light);
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
