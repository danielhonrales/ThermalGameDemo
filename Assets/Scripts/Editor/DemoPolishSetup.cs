using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>Applies the two-minute demo setup through Unity's scene/prefab APIs.</summary>
public static class DemoPolishSetup
{
    private const string ScenePath = "Assets/Scenes/Game.unity";
    private const string PlayerPrefabPath = "Assets/Prefabs/Network/NetworkPlayer.prefab";
    private const string ApkPath = "/tmp/ThermalGameDemo-2min-photon.apk";

    [MenuItem("Thermal Demo/Apply Two-Minute Round Setup")]
    public static void Apply()
    {
        GameObject prefab = PrefabUtility.LoadPrefabContents(PlayerPrefabPath);
        try
        {
            if (prefab.GetComponent<FusionRoundDirector>() == null)
                prefab.AddComponent<FusionRoundDirector>();
            NetworkPlayerHealth playerHealth = prefab.GetComponent<NetworkPlayerHealth>();
            if (playerHealth == null) throw new InvalidOperationException("NetworkPlayer is missing health.");
            SerializedObject health = new SerializedObject(playerHealth);
            health.FindProperty("maxHealth").intValue = 300;
            health.FindProperty("invulnerabilitySeconds").floatValue = 0.72f;
            health.FindProperty("resetToFullHealthOnZero").boolValue = false;
            health.FindProperty("barWidth").floatValue = 0.55f;
            health.FindProperty("barHeight").floatValue = 0.06f;
            health.FindProperty("healthyColor").colorValue = new Color(0.16f, 0.95f, 0.43f, 1f);
            health.FindProperty("lowHealthColor").colorValue = new Color(0.16f, 0.95f, 0.43f, 1f);
            health.ApplyModifiedPropertiesWithoutUndo();
            PrefabUtility.SaveAsPrefabAsset(prefab, PlayerPrefabPath);
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefab);
        }

        var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        PalmBeamShooter beam = UnityEngine.Object.FindFirstObjectByType<PalmBeamShooter>(FindObjectsInactive.Include);
        if (beam == null) throw new InvalidOperationException("Game scene is missing PalmBeamShooter.");
        GameObject rig = beam.gameObject;
        if (rig.GetComponent<HandPoseRouter>() == null)
            rig.AddComponent<HandPoseRouter>();
        SetFloat(rig.GetComponent<HandPoseRouter>(), "trackingGraceSeconds", 0.12f);
        SetFloat(rig.GetComponent<HandPoseRouter>(), "poseExitGraceSeconds", 0.10f);

        beam.enabled = true;
        SetBool(beam, "showAimGuide", true);
        SetBool(beam, "showAimGuideWhileCharging", true);
        SetFloat(beam, "chargeSeconds", 0.75f);
        SetColor(beam, "aimGuideColor", new Color(1f, 0.21f, 0.13f, 0.9f));
        SetColor(beam, "aimGuideHitColor", new Color(1f, 0.55f, 0.45f, 0.95f));
        SetFloat(beam, "maxBeamDurationSeconds", 2.5f);
        SetFloat(beam, "beamCooldownSeconds", 0f);

        IceGrenadeLauncher ice = rig.GetComponent<IceGrenadeLauncher>();
        if (ice == null) throw new InvalidOperationException("Game scene is missing IceGrenadeLauncher.");
        ice.enabled = true;
        SetFloat(ice, "chargeSeconds", 1.05f);
        SetFloat(ice, "throwCooldownSeconds", 0f);
        SetFloat(ice, "minThrowDistance", 0.50f);
        SetFloat(ice, "maxThrowDistance", 5f);
        SetFloat(ice, "lowHandHeightOffsetFromHeadset", -0.55f);
        SetFloat(ice, "highHandHeightOffsetFromHeadset", -0.20f);
        SetFloat(ice, "throwHeightExponent", 1.6f);
        SetBool(ice, "useGazeTargeting", true);

        IceGrenadeEffects coldEffects = rig.GetComponent<IceGrenadeEffects>();
        if (coldEffects != null)
            SetFloat(coldEffects, "explosionRadius", 1.65f);
        ForearmShieldController shield = rig.GetComponent<ForearmShieldController>();
        if (shield != null)
            SetFloat(shield, "shieldDurationSeconds", 2.3f);
        ForearmShieldEffects shieldEffects = rig.GetComponent<ForearmShieldEffects>();
        if (shieldEffects != null)
            SetFloat(shieldEffects, "shieldDiscDiameter", 0.62f);

        StartPadMatchManager oldCountdown = UnityEngine.Object.FindFirstObjectByType<StartPadMatchManager>(FindObjectsInactive.Include);
        if (oldCountdown != null) oldCountdown.enabled = false;
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        Debug.Log("Applied two-minute Fusion demo setup to Game scene and NetworkPlayer prefab.");
    }

    public static void BuildTwoMinuteApk()
    {
        DemoRegressionChecks.RunBoneLookup();
        DemoRegressionChecks.RunSkeletonProvider();
        DemoRegressionChecks.RunPoseFixtures();
        DemoRegressionChecks.RunInteractionChecks();
        DemoRegressionChecks.RunTrajectoryChecks();
        DemoRegressionChecks.RunHudFollowCheck();
        DemoRegressionChecks.RunFeedbackChecks();
        DemoRegressionChecks.RunOutputCheck();
        Apply();
        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = new[] { ScenePath },
            locationPathName = ApkPath,
            target = BuildTarget.Android,
            options = BuildOptions.Development
        });
        if (report.summary.result != BuildResult.Succeeded)
            throw new InvalidOperationException($"Two-minute Quest APK failed: {report.summary.result} ({report.summary.totalErrors} errors).");
        Debug.Log($"Two-minute Quest APK built at {ApkPath}.");
    }

    private static void SetFloat(Component component, string name, float value)
    {
        SerializedObject serialized = new SerializedObject(component);
        SerializedProperty property = serialized.FindProperty(name);
        if (property == null) throw new InvalidOperationException($"Missing {component.GetType().Name}.{name}");
        property.floatValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetColor(Component component, string name, Color value)
    {
        SerializedObject serialized = new SerializedObject(component);
        SerializedProperty property = serialized.FindProperty(name);
        if (property == null) throw new InvalidOperationException($"Missing {component.GetType().Name}.{name}");
        property.colorValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetBool(Component component, string name, bool value)
    {
        SerializedObject serialized = new SerializedObject(component);
        SerializedProperty property = serialized.FindProperty(name);
        if (property == null) throw new InvalidOperationException($"Missing {component.GetType().Name}.{name}");
        property.boolValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }
}
