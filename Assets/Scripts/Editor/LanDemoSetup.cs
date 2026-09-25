#if UNITY_EDITOR
using System;
using Mirror;
using Mirror.Discovery;
using kcp2k;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class LanDemoSetup
{
    private const string ScenePath = "Assets/Scenes/Game.unity";
    private const string PlayerPath = "Assets/Prefabs/Network/NetworkPlayer.prefab";
    private const string ApkPath = "/tmp/ThermalGameDemo-2min-LAN.apk";

    public static void Apply()
    {
        DemoPolishSetup.Apply();
        GameObject prefab = PrefabUtility.LoadPrefabContents(PlayerPath);
        try
        {
            foreach (Fusion.NetworkTransform old in prefab.GetComponents<Fusion.NetworkTransform>())
                UnityEngine.Object.DestroyImmediate(old);
            foreach (Fusion.NetworkObject old in prefab.GetComponents<Fusion.NetworkObject>())
                UnityEngine.Object.DestroyImmediate(old);
            if (prefab.GetComponent<Mirror.NetworkIdentity>() == null) prefab.AddComponent<Mirror.NetworkIdentity>();
            if (prefab.GetComponent<NetworkHeadTracker>() == null || prefab.GetComponent<NetworkPlayerHealth>() == null
                || prefab.GetComponent<FusionRoundDirector>() == null)
                throw new InvalidOperationException("LAN player prefab lost head, health, or round behavior.");
            PrefabUtility.SaveAsPrefabAsset(prefab, PlayerPath);
        }
        finally { PrefabUtility.UnloadPrefabContents(prefab); }

        var scene = EditorSceneManager.OpenScene(ScenePath);
        FixedFusionRoomLauncher photon = UnityEngine.Object.FindFirstObjectByType<FixedFusionRoomLauncher>(FindObjectsInactive.Include);
        if (photon != null) photon.gameObject.SetActive(false);
        foreach (QuestOnlyLocalMatchmakingLauncher old in UnityEngine.Object.FindObjectsByType<QuestOnlyLocalMatchmakingLauncher>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            old.enabled = false;
        foreach (ColocationArenaBinder old in UnityEngine.Object.FindObjectsByType<ColocationArenaBinder>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            old.enabled = false;
        foreach (StartPadMatchManager old in UnityEngine.Object.FindObjectsByType<StartPadMatchManager>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            old.enabled = false;
        foreach (Fusion.NetworkRunner runner in UnityEngine.Object.FindObjectsByType<Fusion.NetworkRunner>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            runner.gameObject.SetActive(false);
        GameObject matchmaking = GameObject.Find("[BuildingBlock] Custom Matchmaking");
        if (matchmaking != null) matchmaking.SetActive(false);
        NetworkArenaPlacementManager placement = UnityEngine.Object.FindFirstObjectByType<NetworkArenaPlacementManager>(FindObjectsInactive.Include);
        if (placement != null)
            foreach (Fusion.NetworkObject old in placement.GetComponents<Fusion.NetworkObject>())
                UnityEngine.Object.DestroyImmediate(old);

        LanMatchManager manager = UnityEngine.Object.FindFirstObjectByType<LanMatchManager>(FindObjectsInactive.Include);
        if (manager == null)
        {
            var go = new GameObject("LAN Match");
            go.AddComponent<KcpTransport>();
            go.AddComponent<NetworkDiscovery>();
            manager = go.AddComponent<LanMatchManager>();
        }
        manager.gameObject.SetActive(true);
        var transport = manager.GetComponent<KcpTransport>();
        transport.port = 7777;
        manager.transport = transport;
        manager.maxConnections = 2;
        manager.sendRate = 20;
        manager.autoCreatePlayer = true;
        manager.playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPath);
        var discovery = manager.GetComponent<NetworkDiscovery>();
        discovery.transport = transport;
        discovery.secretHandshake = LanMatchManager.DiscoveryHandshake;
        if (manager.playerPrefab == null) throw new InvalidOperationException("LAN player prefab missing.");
        EditorUtility.SetDirty(manager);
        EditorUtility.SetDirty(discovery);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        VerifyLanWiring();
        Debug.Log("LAN demo configured: Mirror KCP host/client, Photon startup disabled.");
    }

    private static void VerifyLanWiring()
    {
        GameObject player = AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPath);
        LanMatchManager manager = UnityEngine.Object.FindFirstObjectByType<LanMatchManager>();
        if (player == null || player.GetComponent<Mirror.NetworkIdentity>() == null
            || player.GetComponent<Fusion.NetworkObject>() != null
            || player.GetComponent<Fusion.NetworkTransform>() != null
            || player.GetComponent<NetworkHeadTracker>() == null
            || player.GetComponent<NetworkPlayerHealth>() == null
            || player.GetComponent<FusionRoundDirector>() == null)
            throw new InvalidOperationException("LAN player prefab wiring is incomplete.");
        if (manager == null || manager.playerPrefab != player || manager.GetComponent<KcpTransport>()?.port != 7777
            || manager.GetComponent<NetworkDiscovery>() == null)
            throw new InvalidOperationException("LAN manager is not connected to the player prefab and UDP transport.");
        foreach (FixedFusionRoomLauncher old in UnityEngine.Object.FindObjectsByType<FixedFusionRoomLauncher>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            if (old.isActiveAndEnabled) throw new InvalidOperationException("Photon launcher is still active.");
        foreach (QuestOnlyLocalMatchmakingLauncher old in UnityEngine.Object.FindObjectsByType<QuestOnlyLocalMatchmakingLauncher>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            if (old.isActiveAndEnabled) throw new InvalidOperationException("Cloud matchmaking is still active.");
    }

    public static void BuildLanApk()
    {
        DemoRegressionChecks.RunLanElectionCheck();
        DemoRegressionChecks.RunBoneLookup();
        DemoRegressionChecks.RunSkeletonProvider();
        DemoRegressionChecks.RunPoseFixtures();
        DemoRegressionChecks.RunInteractionChecks();
        DemoRegressionChecks.RunTrajectoryChecks();
        DemoRegressionChecks.RunHudFollowCheck();
        DemoRegressionChecks.RunFeedbackChecks();
        DemoRegressionChecks.RunOutputCheck();
        DemoRegressionChecks.RunLaserCheck();
        DemoRegressionChecks.RunSoloCheck();
        Apply();
        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = new[] { ScenePath },
            locationPathName = ApkPath,
            target = BuildTarget.Android,
            options = BuildOptions.Development
        });
        if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            throw new InvalidOperationException($"LAN APK failed: {report.summary.result} ({report.summary.totalErrors} errors).");
        Debug.Log("LAN Quest APK built at " + ApkPath);
    }
}
#endif
