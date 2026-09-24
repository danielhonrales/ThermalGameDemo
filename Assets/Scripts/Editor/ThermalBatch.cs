using UnityEditor.SceneManagement;

/// <summary>Headless iteration pipeline: rebuild the arena layout, run every regression check, build the Quest APK.</summary>
public static class ThermalBatch
{
    public static void RebuildCheckAndBuild()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
        ArenaLayoutBuilder.Build();
        EditorSceneManager.SaveOpenScenes();
        DemoRegressionChecks.RunAll();
        LanDemoSetup.BuildLanApk();
    }
}
