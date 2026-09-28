using System;
using System.Reflection;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ArenaMechanicsChecks
{
    public static void Run()
    {
        CheckWood();
        CheckCargo();
        CheckOverheadFlight();
        var roof = GameObject.Find("ArenaRoot/Environment/VirtualCeiloingAlwaysVisible");
        if (roof == null)
            throw new Exception("Authored roof is missing or inactive.");
        for (int i = 0; i < 4; i++)
            if (Resources.Load<GameObject>("ThermalArena/ShipCargo" + i) == null)
                throw new Exception("Missing diverse cargo variant " + i);
        Debug.Log("ARENA MECHANICS PASSED: wood chars, opens a physical hole, resets; cargo drops to authored pose without rigidbodies; four cargo variants.");
    }

    public static void CaptureArena()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
        foreach (var camera in UnityEngine.Object.FindObjectsByType<Camera>(FindObjectsSortMode.None)) camera.enabled = false;
        var ceiling = GameObject.Find("ArenaRoot/Environment/VirtualCeiloingAlwaysVisible");
        if (ceiling != null) ceiling.SetActive(false);
        foreach (var wood in UnityEngine.Object.FindObjectsByType<BurnableWood>(FindObjectsSortMode.None))
            typeof(BurnableWood).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(wood, null);
        var eye = new GameObject("Arena review camera").AddComponent<Camera>();
        eye.transform.position = new Vector3(5f, 5.8f, -5.8f);
        eye.transform.LookAt(new Vector3(0.275f, 0.5f, 0.71f));
        eye.clearFlags = CameraClearFlags.SolidColor;
        eye.backgroundColor = new Color(0.08f, 0.10f, 0.14f);
        eye.fieldOfView = 55f;
        var target = new RenderTexture(1400, 1000, 24);
        eye.targetTexture = target; eye.Render();
        RenderTexture.active = target;
        var image = new Texture2D(1400, 1000, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, 1400, 1000), 0, 0); image.Apply();
        System.IO.File.WriteAllBytes("/tmp/thermal-r12-arena.png", image.EncodeToPNG());
        eye.targetTexture = null; RenderTexture.active = null;
        UnityEngine.Object.DestroyImmediate(image); UnityEngine.Object.DestroyImmediate(target);
        UnityEngine.Object.DestroyImmediate(eye.gameObject);
        if (ceiling != null) ceiling.SetActive(true);
        Debug.Log("ARENA PREVIEW: /tmp/thermal-r12-arena.png");
    }

    private static void CheckWood()
    {
        var root = new GameObject("Wood regression");
        root.SetActive(false);
        root.transform.position = Vector3.one * 1000f;
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        try
        {
            var wood = root.AddComponent<BurnableWood>();
            typeof(BurnableWood).GetMethod("Awake", flags).Invoke(wood, null);
            var collider = root.GetComponent<MeshCollider>();
            Vector3 spot = new Vector3(0.045f, 0.5729f, 0f);
            int cell = BurnableWood.CellAt(spot);
            if (cell < 0 || BurnableWood.CellAt(new Vector3(5f, 0f, 0f)) != -1)
                throw new Exception("Wood hit cell mapping failed.");
            var ray = new Ray(root.transform.TransformPoint(spot) - Vector3.forward, Vector3.forward);
            root.SetActive(true); Physics.SyncTransforms();
            if (!collider.Raycast(ray, out _, 2f)) throw new Exception("Unburned wood did not stop a shot.");
            var heat = (byte[])typeof(BurnableWood).GetField("heat", flags).GetValue(wood);
            for (int pulse = 0; pulse < 5; pulse++) BurnableWood.SpreadHeat(heat, 0, cell);
            int holes = 0;
            foreach (byte value in heat) if (value >= BurnableWood.BurnThrough) holes++;
            if (holes < 9) throw new Exception("Burn spread stayed confined to a single cell.");
            typeof(BurnableWood).GetMethod("Rebuild", flags).Invoke(wood, null);
            Physics.SyncTransforms();
            if (collider.Raycast(ray, out _, 2f)) throw new Exception("Burned hole still blocks shots.");
            System.Array.Clear(heat, 0, heat.Length);
            typeof(BurnableWood).GetMethod("Rebuild", flags).Invoke(wood, null);
            Physics.SyncTransforms();
            if (!collider.Raycast(ray, out _, 2f)) throw new Exception("Wood reset did not restore cover.");
        }
        finally { UnityEngine.Object.DestroyImmediate(root); }
    }

    private static void CheckOverheadFlight()
    {
        var root = new GameObject("Overhead flight regression");
        var load = new GameObject("Return load");
        try
        {
            var drone = root.AddComponent<CoverDrone>();
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            void Set(string field, Vector3 value) => typeof(CoverDrone).GetField(field, flags).SetValue(drone, value);
            Set("entry", new Vector3(0f, 2.7f, -4.5f)); Set("hover", new Vector3(0f, 2.35f, 0f));
            Set("low", new Vector3(0f, 2.35f, 0f)); Set("exit", new Vector3(4f, 2.9f, 5f));
            drone.SetReturnCargo(load.transform);
            var pose = typeof(CoverDrone).GetMethod("Pose", flags);
            for (float t = 0f; t <= CoverDrone.ExchangeGone; t += 0.02f)
                if (((Vector3)pose.Invoke(drone, new object[] { t })).y < 2.25f)
                    throw new Exception("Grapple drone descended into the player space.");
        }
        finally { UnityEngine.Object.DestroyImmediate(root); UnityEngine.Object.DestroyImmediate(load); }
    }

    private static void CheckCargo()
    {
        var cargo = GameObject.CreatePrimitive(PrimitiveType.Cube);
        try
        {
            cargo.transform.position = new Vector3(1000f, 0.5f, 1000f);
            cargo.AddComponent<Rigidbody>();
            Vector3 rest = cargo.transform.position;
            var motion = ArenaCargoPhysics.Register(cargo.transform, 99);
            if (cargo.GetComponent<Rigidbody>() != null) throw new Exception("Cargo still has a rigidbody.");
            cargo.transform.position += Vector3.up * 2f;
            motion.Release(Vector3.right * 4f, Vector3.one);
            motion.TickDrop(ArenaCargoPhysics.DropDuration / 2f);
            if (cargo.transform.position.y <= rest.y || cargo.transform.position.y >= rest.y + 2f)
                throw new Exception("Scripted cargo drop did not animate.");
            motion.TickDrop(ArenaCargoPhysics.DropDuration);
            if (Vector3.Distance(cargo.transform.position, rest) > 0.001f)
                throw new Exception("Dropped cover missed its authored pose.");
            Physics.SyncTransforms();
            var ray = new Ray(rest + Vector3.forward * 2f, Vector3.back);
            if (!cargo.GetComponent<Collider>().Raycast(ray, out _, 3f))
                throw new Exception("Fixed cargo no longer blocks combat shots.");
        }
        finally { UnityEngine.Object.DestroyImmediate(cargo); }
        ArenaLayoutAuthoring.Validate(UnityEngine.Object.FindFirstObjectByType<ArenaLayouts>());
    }
}
