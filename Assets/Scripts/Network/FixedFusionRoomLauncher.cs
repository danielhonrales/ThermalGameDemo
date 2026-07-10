using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkRunner))]
public sealed class FixedFusionRoomLauncher : MonoBehaviour
{
    [SerializeField] private string sessionName = "thermal-demo-room";
    [SerializeField] private GameMode gameMode = GameMode.Shared;
    [SerializeField] private bool startOnAwake = true;
    [SerializeField] private bool includeActiveScene = false;

    private NetworkRunner runner;
    private bool isStarting;

    private async void Awake()
    {
        runner = GetComponent<NetworkRunner>();
        runner.ProvideInput = true;

        if (startOnAwake)
        {
            await StartFixedRoom();
        }
    }

    [ContextMenu("Start Fixed Room")]
    public async Task StartFixedRoom()
    {
        if (isStarting || runner == null || runner.IsRunning)
        {
            return;
        }

        isStarting = true;

        INetworkSceneManager sceneManager = GetComponent<INetworkSceneManager>();
        if (sceneManager == null)
        {
            sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        INetworkObjectProvider objectProvider = GetComponent<INetworkObjectProvider>();
        if (objectProvider == null)
        {
            objectProvider = gameObject.AddComponent<NetworkObjectProviderDefault>();
        }

        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        if (includeActiveScene)
        {
            Scene activeScene = SceneManager.GetActiveScene();
            if (activeScene.buildIndex >= 0)
            {
                sceneInfo.AddSceneRef(SceneRef.FromIndex(activeScene.buildIndex), LoadSceneMode.Additive);
            }
        }

        StartGameResult result = await runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            SessionName = sessionName,
            Scene = sceneInfo,
            SceneManager = sceneManager,
            ObjectProvider = objectProvider
        });

        isStarting = false;

        if (result.Ok)
        {
            Debug.Log($"Fusion fixed room started: {sessionName} ({gameMode}).", this);
        }
        else
        {
            Debug.LogError($"Fusion fixed room failed: {result.ShutdownReason}", this);
        }
    }
}
