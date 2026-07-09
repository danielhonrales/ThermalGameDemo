using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public sealed class NetworkPlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkObject networkPlayerPrefab;

    private NetworkRunner runner;

    private void Awake()
    {
        runner = GetComponent<NetworkRunner>();
        if (runner == null)
        {
            runner = GetComponentInChildren<NetworkRunner>();
        }
    }

    private void OnEnable()
    {
        RegisterCallbacks();
    }

    private void Start()
    {
        RegisterCallbacks();
    }

    private void OnDisable()
    {
        if (runner != null)
        {
            runner.RemoveCallbacks(this);
        }
    }

    private void RegisterCallbacks()
    {
        if (runner == null)
        {
            runner = FindFirstObjectByType<NetworkRunner>();
        }

        if (runner != null)
        {
            runner.AddCallbacks(this);
        }
    }

    public void OnPlayerJoined(NetworkRunner joinedRunner, PlayerRef player)
    {
        if (networkPlayerPrefab == null)
        {
            Debug.LogError("NetworkPlayerSpawner has no Network Player Prefab assigned.", this);
            return;
        }

        if (player != joinedRunner.LocalPlayer)
        {
            return;
        }

        NetworkObject existing = joinedRunner.GetPlayerObject(player);
        if (existing != null)
        {
            return;
        }

        NetworkObject spawned = joinedRunner.Spawn(
            networkPlayerPrefab,
            Vector3.zero,
            Quaternion.identity,
            player);

        joinedRunner.SetPlayerObject(player, spawned);
        Debug.Log($"Spawned local NetworkPlayer for {player}.", spawned);
    }

    public void OnPlayerLeft(NetworkRunner leftRunner, PlayerRef player)
    {
        NetworkObject playerObject = leftRunner.GetPlayerObject(player);
        if (playerObject != null && playerObject.HasStateAuthority)
        {
            leftRunner.Despawn(playerObject);
        }
    }

    public void OnInput(NetworkRunner inputRunner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner inputRunner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner shutdownRunner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner connectedRunner) { }
    public void OnDisconnectedFromServer(NetworkRunner disconnectedRunner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner requestRunner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner failedRunner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner messageRunner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner sessionRunner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner authRunner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner migrationRunner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner sceneRunner) { }
    public void OnSceneLoadStart(NetworkRunner sceneRunner) { }
    public void OnObjectEnterAOI(NetworkRunner aoiRunner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner aoiRunner, NetworkObject obj, PlayerRef player) { }
#if FUSION_2_1_OR_NEWER
    public void OnReliableDataReceived(NetworkRunner dataRunner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data) { }
#else
    public void OnReliableDataReceived(NetworkRunner dataRunner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
#endif
    public void OnReliableDataProgress(NetworkRunner dataRunner, PlayerRef player, ReliableKey key, float progress) { }
}
