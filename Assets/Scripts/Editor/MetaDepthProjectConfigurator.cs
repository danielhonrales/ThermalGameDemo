#if UNITY_EDITOR && OPEN_XR_META_2_1_OR_NEWER
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features.Meta;

[InitializeOnLoad]
internal static class MetaDepthProjectConfigurator
{
    static MetaDepthProjectConfigurator()
    {
        EditorApplication.delayCall += ConfigureDepthFeatures;
    }

    private static void ConfigureDepthFeatures()
    {
        OpenXRSettings settings = OpenXRSettings.GetSettingsForBuildTargetGroup(BuildTargetGroup.Android);
        if (settings == null)
        {
            return;
        }

        bool changed = false;
        AROcclusionFeature occlusion = settings.GetFeature<AROcclusionFeature>();
        if (occlusion != null && !occlusion.enabled)
        {
            occlusion.enabled = true;
            EditorUtility.SetDirty(occlusion);
            changed = true;
        }

        ARSessionFeature session = settings.GetFeature<ARSessionFeature>();
        if (session != null && !session.enabled)
        {
            session.enabled = true;
            EditorUtility.SetDirty(session);
            changed = true;
        }

        if (settings.renderMode != OpenXRSettings.RenderMode.SinglePassInstanced)
        {
            settings.renderMode = OpenXRSettings.RenderMode.SinglePassInstanced;
            EditorUtility.SetDirty(settings);
            changed = true;
        }

        if (changed)
        {
            AssetDatabase.SaveAssets();
            Debug.Log("Configured Meta Quest OpenXR Session and Occlusion features for Environment Depth.");
        }
    }
}
#endif
