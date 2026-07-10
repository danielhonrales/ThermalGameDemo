using UnityEngine;

[DisallowMultipleComponent]
public sealed class VisualOnlyEnvironment : MonoBehaviour
{
    [SerializeField] private bool applyLayerToChildren = true;
    [SerializeField] private bool disableColliders = true;

    private void Reset()
    {
        ApplySetup();
    }

    private void Awake()
    {
        ApplySetup();
    }

    [ContextMenu("Apply Visual Only Setup")]
    public void ApplySetup()
    {
        int layer = CombatLayers.BackgroundEnvironmentLayer;
        if (layer >= 0)
        {
            if (applyLayerToChildren)
            {
                CombatLayers.SetLayerRecursively(gameObject, layer);
            }
            else
            {
                gameObject.layer = layer;
            }
        }

        if (!disableColliders)
        {
            return;
        }

        Collider[] colliders = GetComponentsInChildren<Collider>(true);
        foreach (Collider environmentCollider in colliders)
        {
            environmentCollider.enabled = false;
        }
    }
}
