using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>Builds one Quest APK per player, each sending to its own Pi by default.
/// Batch: -executeMethod PlayerApkBuilds.BuildBoth</summary>
public static class PlayerApkBuilds
{
    private const string Player1PiHost = "192.168.1.4";
    private const string Player2PiHost = "192.168.1.248";
    private const string OutputFolder = "Builds";
    private const string BakedHostPath = "Assets/Resources/" + CombatEventOutput.BakedPiHostResource + ".txt";

    [MenuItem("Thermal Demo/Build Player 1 and 2 APKs")]
    public static void BuildBoth()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        Build("player1", Player1PiHost);
        Build("player2", Player2PiHost);
    }

    private static void Build(string player, string piHost)
    {
        Directory.CreateDirectory(OutputFolder);
        string apkPath = Path.Combine(OutputFolder, $"ThermalGameDemo-{player}.apk");
        File.WriteAllText(BakedHostPath, piHost);
        AssetDatabase.ImportAsset(BakedHostPath);
        try
        {
            BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
                locationPathName = apkPath,
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
                options = BuildOptions.None
            });
            if (report.summary.result != BuildResult.Succeeded)
                throw new InvalidOperationException($"{player} APK failed: {report.summary.result} ({report.summary.totalErrors} errors).");
            Debug.Log($"{player} APK (Pi {piHost}) built at {Path.GetFullPath(apkPath)}");
        }
        finally
        {
            // Ordinary builds must not inherit a player's Pi IP.
            AssetDatabase.DeleteAsset(BakedHostPath);
        }
    }
}
