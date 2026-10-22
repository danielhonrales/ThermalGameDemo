using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;
using UnityEngine.XR.Management;

/// <summary>Builds the single Quest APK every headset uses; each headset picks its Pi in the startup menu.
/// Batch: -executeMethod QuestApkBuild.Build, or QuestApkBuild.BuildAndRun to install on the connected headset.
/// QuestApkBuild.BuildWindows builds the same game for a PC, which always runs as the match spectator.</summary>
public static class QuestApkBuild
{
    private const string ApkPath = "Builds/ThermalGameDemo.apk";
    private const string WindowsPath = "Builds/Windows/ThermalGameDemo.exe";
    private const string OpenXrLoader = "UnityEngine.XR.OpenXR.OpenXRLoader";

    [MenuItem("Thermal Demo/Build Windows Spectator")]
    public static void BuildWindows()
    {
        if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        Directory.CreateDirectory(Path.GetDirectoryName(WindowsPath));
        // The spectator never uses XR, and the Quest-only OpenXR features fail desktop validation.
        // Drop the desktop OpenXR loader for this build only; Editor Play mode keeps it.
        XRManagerSettings xr = XRGeneralSettingsPerBuildTarget
            .XRGeneralSettingsForBuildTarget(BuildTargetGroup.Standalone)?.Manager;
        bool removedOpenXr = xr != null
            && XRPackageMetadataStore.RemoveLoader(xr, OpenXrLoader, BuildTargetGroup.Standalone);
        BuildReport report;
        try
        {
            report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
                locationPathName = WindowsPath,
                target = BuildTarget.StandaloneWindows64,
                targetGroup = BuildTargetGroup.Standalone,
                options = BuildOptions.None
            });
        }
        finally
        {
            if (removedOpenXr) XRPackageMetadataStore.AssignLoader(xr, OpenXrLoader, BuildTargetGroup.Standalone);
        }
        if (report.summary.result != BuildResult.Succeeded)
            throw new InvalidOperationException($"Windows build failed: {report.summary.result} ({report.summary.totalErrors} errors).");
        Debug.Log($"Windows spectator built at {Path.GetFullPath(WindowsPath)}");
    }

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
