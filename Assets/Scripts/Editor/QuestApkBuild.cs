using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>Builds the single Quest APK every headset uses; each headset picks its Pi in the startup menu.
/// Batch: -executeMethod QuestApkBuild.Build, or QuestApkBuild.BuildAndRun to install on the connected headset.</summary>
public static class QuestApkBuild
{
    private const string ApkPath = "Builds/ThermalGameDemo.apk";

    [MenuItem("Thermal Demo/Build Quest APK")]
    public static void Build() => Build(BuildOptions.None);

    [MenuItem("Thermal Demo/Build and Run on Connected Quest")]
    public static void BuildAndRun() => Build(BuildOptions.AutoRunPlayer);

    private static void Build(BuildOptions options)
    {
        if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        Directory.CreateDirectory(Path.GetDirectoryName(ApkPath));
        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
            locationPathName = ApkPath,
            target = BuildTarget.Android,
            targetGroup = BuildTargetGroup.Android,
            options = options
        });
        if (report.summary.result != BuildResult.Succeeded)
            throw new InvalidOperationException($"Quest APK failed: {report.summary.result} ({report.summary.totalErrors} errors).");
        Debug.Log($"Quest APK built at {Path.GetFullPath(ApkPath)}");
    }
}
