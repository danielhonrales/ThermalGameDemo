using System.Threading.Tasks;
using Fusion;
using Meta.XR.MultiplayerBlocks.Shared;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class QuestOnlyLocalMatchmakingLauncher : MonoBehaviour
{
    public enum LaunchMode
    {
        AutoHostOrJoin,
        ForceHost,
        ForceGuest
    }

    [SerializeField] private LaunchMode launchMode = LaunchMode.AutoHostOrJoin;
    [SerializeField] private LocalMatchmaking localMatchmaking;
    [SerializeField] private NetworkRunner runner;
    [SerializeField, Min(0f)] private float startDelaySeconds = 1f;
    [SerializeField, Min(1f)] private float hostFallbackDelaySeconds = 7f;
    [SerializeField] private bool logStatus = true;

    private async void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        await StartQuestLocalMatchmaking();
#else
        Log("Skipping Meta local matchmaking outside Quest player.");
#endif
    }

    private async Task StartQuestLocalMatchmaking()
    {
        if (startDelaySeconds > 0f)
        {
            await Task.Delay(Mathf.RoundToInt(startDelaySeconds * 1000f));
        }

        FindReferences();

        if (localMatchmaking == null)
        {
            Debug.LogError("QuestOnlyLocalMatchmakingLauncher could not find LocalMatchmaking.", this);
            return;
        }

        if (launchMode == LaunchMode.ForceHost)
        {
            Log("Starting as forced host.");
            await localMatchmaking.StartAsHost();
            return;
        }

        Log("Starting Quest local session discovery.");
        await localMatchmaking.StartAsGuest(launchMode != LaunchMode.ForceGuest);

        if (launchMode == LaunchMode.ForceGuest)
        {
            Log("Forced guest discovery finished. Not falling back to host.");
            return;
        }

        if (hostFallbackDelaySeconds > 0f)
        {
            await Task.Delay(Mathf.RoundToInt(hostFallbackDelaySeconds * 1000f));
        }

        FindReferences();
        if (runner != null && runner.IsRunning)
        {
            Log("NetworkRunner is already running after discovery. Not starting host.");
            return;
        }

        Log("No running NetworkRunner after discovery. Starting as host.");
        await localMatchmaking.StartAsHost();
    }

    private void FindReferences()
    {
        if (localMatchmaking == null)
        {
            localMatchmaking = FindFirstObjectByType<LocalMatchmaking>();
        }

        if (runner == null)
        {
            runner = FindFirstObjectByType<NetworkRunner>();
        }
    }

    private void Log(string message)
    {
        if (logStatus)
        {
            Debug.Log($"QuestOnlyLocalMatchmakingLauncher: {message}", this);
        }
    }
}
