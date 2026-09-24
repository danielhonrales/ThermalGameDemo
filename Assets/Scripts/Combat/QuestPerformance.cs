using UnityEngine;

/// <summary>
/// Quest 3 performance defaults: sustained-high CPU/GPU clocks and dynamic high fixed foveated
/// rendering, which keep the arena, fire and particle effects at the display's frame rate.
/// </summary>
public static class QuestPerformance
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Apply()
    {
        if (Application.isEditor) return;
        OVRManager.suggestedCpuPerfLevel = OVRManager.ProcessorPerformanceLevel.SustainedHigh;
        OVRManager.suggestedGpuPerfLevel = OVRManager.ProcessorPerformanceLevel.SustainedHigh;
        OVRManager.foveatedRenderingLevel = OVRManager.FoveatedRenderingLevel.High;
        OVRManager.useDynamicFoveatedRendering = true;
        Debug.Log("Quest performance: sustained-high CPU/GPU, dynamic high foveation.");
    }
}
