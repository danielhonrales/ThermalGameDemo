using UnityEngine;

/// <summary>
/// Whether this machine joins the LAN match as a non-playing spectator. A spectator never hosts,
/// never spawns a player and never sends hardware events; <see cref="SpectatorDirector"/> shows the
/// match. In the Editor it follows the "Thermal Demo/Play As Spectator" toggle; a desktop player
/// build can pass -spectator on the command line. Quest builds are never spectators.
/// </summary>
public static class SpectatorSession
{
    public const string EditorPrefKey = "ThermalDemo.PlayAsSpectator";
    private static bool? isSpectator;

    public static bool IsSpectator => isSpectator ??= Detect();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay() => isSpectator = null;

    private static bool Detect()
    {
        if (Application.platform == RuntimePlatform.Android || Application.isBatchMode) return false;
#if UNITY_EDITOR
        // Only interactive Play mode; builds and edit-mode checks on this PC stay normal.
        return Application.isPlaying && UnityEditor.EditorPrefs.GetBool(EditorPrefKey, false);
#else
        return System.Array.IndexOf(System.Environment.GetCommandLineArgs(), "-spectator") >= 0;
#endif
    }
}
