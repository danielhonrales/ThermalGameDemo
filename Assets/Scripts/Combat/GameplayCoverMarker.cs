using UnityEngine;

[DisallowMultipleComponent]
public sealed class GameplayCoverMarker : MonoBehaviour
{
    [SerializeField] private bool applyLayerToChildren = true;
    [SerializeField] private bool requireCollider = true;

    private void Reset()
    {
        ApplySetup();
    }

    private void Awake()
    {
        ApplySetup();
    }

    [ContextMenu("Apply Cover Setup")]
    public void ApplySetup()
    {
        int layer = CombatLayers.GameplayCoverLayer;
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

        if (requireCollider && GetComponentInChildren<Collider>() == null)
        {
            Debug.LogWarning($"{name} is marked as gameplay cover but has no Collider. Add a BoxCollider or MeshCollider so beams can hit it.", this);
        }
    }
}
