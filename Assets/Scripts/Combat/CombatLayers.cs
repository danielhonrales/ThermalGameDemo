using UnityEngine;

public static class CombatLayers
{
    public const string GameplayCover = "GameplayCover";
    public const string HeadTarget = "HeadTarget";
    public const string Beam = "Beam";
    public const string BackgroundEnvironment = "BackgroundEnvironment";
    public const string Debug = "Debug";

    public static int GameplayCoverLayer => LayerMask.NameToLayer(GameplayCover);
    public static int HeadTargetLayer => LayerMask.NameToLayer(HeadTarget);
    public static int BeamLayer => LayerMask.NameToLayer(Beam);
    public static int BackgroundEnvironmentLayer => LayerMask.NameToLayer(BackgroundEnvironment);
    public static int DebugLayer => LayerMask.NameToLayer(Debug);

    public static LayerMask CombatHitMask => LayerMask.GetMask(GameplayCover, HeadTarget);

    public static bool HasRequiredLayers()
    {
        return GameplayCoverLayer >= 0
            && HeadTargetLayer >= 0
            && BeamLayer >= 0
            && BackgroundEnvironmentLayer >= 0
            && DebugLayer >= 0;
    }

    public static void SetLayerRecursively(GameObject root, int layer)
    {
        if (root == null || layer < 0)
        {
            return;
        }

        root.layer = layer;

        foreach (Transform child in root.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
