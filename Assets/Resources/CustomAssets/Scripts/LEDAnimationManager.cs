using UnityEngine;

public sealed class LEDAnimationManager : MonoBehaviour
{
    public Renderer[] renderers;
    public Material idleMaterial;
    public Material activeMaterial;

    public void SetActiveVisual(bool active)
    {
        Material material = active ? activeMaterial : idleMaterial;
        if (material == null || renderers == null)
        {
            return;
        }

        foreach (Renderer targetRenderer in renderers)
        {
            if (targetRenderer != null)
            {
                targetRenderer.sharedMaterial = material;
            }
        }
    }
}
