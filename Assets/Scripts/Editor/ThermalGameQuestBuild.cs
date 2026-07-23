using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class ThermalGameQuestBuild
{
    public const string TestApkPath = "/tmp/ThermalGameDemo-depth-test.apk";

    public static void BuildDepthTestApk()
    {
        string[] scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            throw new InvalidOperationException("No enabled scenes are configured in Build Settings.");
        }

        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = TestApkPath,
            target = BuildTarget.Android,
            options = BuildOptions.Development
        });

        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new InvalidOperationException($"Quest APK build failed: {report.summary.result} ({report.summary.totalErrors} errors).");
        }
    }
}
