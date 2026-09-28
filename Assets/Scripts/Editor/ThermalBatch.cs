using UnityEditor.SceneManagement;

/// <summary>Headless iteration pipeline: preserve the authored arena layouts, run every regression check, build the Quest APK.</summary>
public static class ThermalBatch
{
    public static void RebuildCheckAndBuild()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
        ArenaLayoutAuthoring.Prepare();
        ArenaColliderSetup.Sync();
        EditorSceneManager.SaveOpenScenes();
        ArenaMechanicsChecks.Run();
        DemoRegressionChecks.RunAll();
        if (UnityEngine.SystemInfo.graphicsDeviceType != UnityEngine.Rendering.GraphicsDeviceType.Null)
            ArenaMechanicsChecks.CaptureArena();
        LanDemoSetup.BuildLanApk();
    }
}
