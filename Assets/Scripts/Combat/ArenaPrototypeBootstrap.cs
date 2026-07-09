using UnityEngine;

[DisallowMultipleComponent]
public sealed class ArenaPrototypeBootstrap : MonoBehaviour
{
    [Header("Scene Roots")]
    [SerializeField] private Transform arenaRoot;
    [SerializeField] private Transform gameplayRoot;
    [SerializeField] private Transform visualEnvironmentRoot;
    [SerializeField] private Transform debugRoot;

    [Header("Prototype Objects")]
    [SerializeField] private PalmBeamShooter palmBeamShooter;
    [SerializeField] private Transform palmOrigin;
    [SerializeField] private Transform headset;
    [SerializeField] private HeadshotTarget headshotTarget;

    private void Reset()
    {
        FindSceneReferences();
    }

    private void Awake()
    {
        FindSceneReferences();
        ApplyPrototypeSetup();
    }

    [ContextMenu("Find Scene References")]
    public void FindSceneReferences()
    {
        arenaRoot = arenaRoot != null ? arenaRoot : FindRoot("ArenaRoot");
        gameplayRoot = gameplayRoot != null ? gameplayRoot : FindRoot("GameplayRoot");
        visualEnvironmentRoot = visualEnvironmentRoot != null ? visualEnvironmentRoot : FindRoot("VisualEnvironmentRoot");
        debugRoot = debugRoot != null ? debugRoot : FindRoot("DebugRoot");

        palmBeamShooter = palmBeamShooter != null ? palmBeamShooter : FindFirstObjectByType<PalmBeamShooter>();
        headshotTarget = headshotTarget != null ? headshotTarget : FindFirstObjectByType<HeadshotTarget>();
    }

    [ContextMenu("Apply Prototype Setup")]
    public void ApplyPrototypeSetup()
    {
        if (!CombatLayers.HasRequiredLayers())
        {
            Debug.LogError("Combat layers are missing. Check ProjectSettings/TagManager.asset or create GameplayCover, HeadTarget, Beam, BackgroundEnvironment, and Debug layers.", this);
            return;
        }

        if (visualEnvironmentRoot != null)
        {
            VisualOnlyEnvironment visualOnly = visualEnvironmentRoot.GetComponent<VisualOnlyEnvironment>();
            if (visualOnly == null)
            {
                visualOnly = visualEnvironmentRoot.gameObject.AddComponent<VisualOnlyEnvironment>();
            }

            visualOnly.ApplySetup();
        }

        GameplayCoverMarker[] covers = FindObjectsByType<GameplayCoverMarker>(FindObjectsSortMode.None);
        foreach (GameplayCoverMarker cover in covers)
        {
            cover.ApplySetup();
        }

        if (headshotTarget != null)
        {
            headshotTarget.ApplySetup();
            if (headset != null)
            {
                headshotTarget.TrackedHead = headset;
            }
        }

        if (palmBeamShooter != null)
        {
            if (palmOrigin != null)
            {
                palmBeamShooter.SetPalmOrigin(palmOrigin);
            }

            if (headset != null)
            {
                palmBeamShooter.SetHeadset(headset);
            }
        }

        if (debugRoot != null && CombatLayers.DebugLayer >= 0)
        {
            CombatLayers.SetLayerRecursively(debugRoot.gameObject, CombatLayers.DebugLayer);
        }
    }

    private static Transform FindRoot(string rootName)
    {
        GameObject found = GameObject.Find(rootName);
        return found != null ? found.transform : null;
    }
}
