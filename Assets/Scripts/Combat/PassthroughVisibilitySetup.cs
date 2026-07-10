using UnityEngine;

[DisallowMultipleComponent]
public sealed class PassthroughVisibilitySetup : MonoBehaviour
{
    [SerializeField] private OVRPassthroughLayer passthroughLayer;
    [SerializeField, Range(0f, 1f)] private float opacity = 1f;
    [SerializeField] private bool disableEdgeRendering = true;

    private void Reset()
    {
        passthroughLayer = FindFirstObjectByType<OVRPassthroughLayer>();
    }

    private void Awake()
    {
        Apply();
    }

    [ContextMenu("Apply Passthrough Visibility")]
    public void Apply()
    {
        if (passthroughLayer == null)
        {
            passthroughLayer = FindFirstObjectByType<OVRPassthroughLayer>();
        }

        if (passthroughLayer == null)
        {
            Debug.LogWarning("No OVRPassthroughLayer found in the scene.", this);
            return;
        }

        passthroughLayer.overlayType = OVROverlay.OverlayType.Underlay;
        passthroughLayer.textureOpacity = opacity;
        passthroughLayer.edgeRenderingEnabled = !disableEdgeRendering;
    }
}
