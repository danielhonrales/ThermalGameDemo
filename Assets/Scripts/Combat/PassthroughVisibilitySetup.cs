using System.Collections;
using Meta.XR.EnvironmentDepth;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.XR.Management;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

[DisallowMultipleComponent]
public sealed class PassthroughVisibilitySetup : MonoBehaviour
{
    private const string ScenePermission = "com.oculus.permission.USE_SCENE";

    [SerializeField] private OVRPassthroughLayer passthroughLayer;
    [SerializeField, Range(0f, 1f)] private float opacity = 1f;
    [SerializeField] private bool disableEdgeRendering = true;
    [Header("Dynamic Real-World Occlusion")]
    [SerializeField] private bool enableEnvironmentDepth = true;
    [SerializeField] private bool useSoftOcclusion = true;
    [SerializeField] private bool letRealHandsOccludeVirtualContent = true;
    [SerializeField] private Transform trackingSpace;
    [Header("Physical Hands Only")]
    [Tooltip("Disables only Meta/Interaction SDK hand mesh renderers. Hand tracking remains enabled for gestures and weapons.")]
    [SerializeField] private bool hideVirtualHandMeshes = true;

    private EnvironmentDepthManager environmentDepthManager;
#if UNITY_ANDROID
    private PermissionCallbacks scenePermissionCallbacks;
#endif

    private void Reset()
    {
        passthroughLayer = FindFirstObjectByType<OVRPassthroughLayer>();
    }

    private void Awake()
    {
        Apply();
    }

    private IEnumerator Start()
    {
        if (hideVirtualHandMeshes)
        {
            StartCoroutine(KeepVirtualHandMeshesHidden());
        }

        if (!enableEnvironmentDepth)
        {
            yield break;
        }

        // EnvironmentDepthManager caches the first provider it creates. Wait until
        // OpenXR has an active loader so an early Awake cannot cache an unsupported
        // provider for the remainder of the process.
        float loaderDeadline = Time.realtimeSinceStartup + 15f;
        while (!HasActiveXrLoader() && Time.realtimeSinceStartup < loaderDeadline)
        {
            yield return null;
        }

        if (!HasActiveXrLoader())
        {
            Debug.LogError("Environment Depth did not start because no active XR loader was available after 15 seconds.", this);
            yield break;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        RequestScenePermission();
#endif

        ConfigureEnvironmentDepth();

        float depthDeadline = Time.realtimeSinceStartup + 20f;
        while (environmentDepthManager != null
               && environmentDepthManager.enabled
               && !environmentDepthManager.IsDepthAvailable
               && Time.realtimeSinceStartup < depthDeadline)
        {
            yield return null;
        }

        if (environmentDepthManager == null || !environmentDepthManager.enabled)
        {
            Debug.LogError("Environment Depth manager disabled itself. Confirm Meta Quest: Occlusion is enabled and this is running on Quest 3/3S.", this);
        }
        else if (!environmentDepthManager.IsDepthAvailable)
        {
            Debug.LogError($"Environment Depth timed out. Spatial data permission granted: {HasScenePermission()}.", this);
        }
        else
        {
            Debug.Log("Environment Depth texture is available; real hands and people can occlude virtual environment geometry.", this);
        }
    }

    private IEnumerator KeepVirtualHandMeshesHidden()
    {
        // The hand rig can create or re-enable its renderers after XR startup, so this is
        // intentionally repeated. It does not touch OVRHand/OVRSkeleton tracking data.
        while (hideVirtualHandMeshes)
        {
            HideVirtualHandMeshes();
            yield return new WaitForSecondsRealtime(0.25f);
        }
    }

    private static void HideVirtualHandMeshes()
    {
        foreach (OVRMeshRenderer handMeshRenderer in FindObjectsByType<OVRMeshRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            handMeshRenderer.enabled = false;
            SkinnedMeshRenderer skinnedMesh = handMeshRenderer.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMesh != null)
            {
                skinnedMesh.enabled = false;
            }
        }

        foreach (OVRSkeletonRenderer skeletonRenderer in FindObjectsByType<OVRSkeletonRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            skeletonRenderer.enabled = false;
        }

        foreach (HandVisual handVisual in FindObjectsByType<HandVisual>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            handVisual.ForceOffVisibility = true;
        }
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

    private void ConfigureEnvironmentDepth()
    {
        if (!enableEnvironmentDepth)
        {
            return;
        }

        environmentDepthManager = FindFirstObjectByType<EnvironmentDepthManager>(FindObjectsInactive.Include);
        if (environmentDepthManager == null)
        {
            environmentDepthManager = gameObject.AddComponent<EnvironmentDepthManager>();
        }

        if (trackingSpace == null)
        {
            GameObject foundTrackingSpace = GameObject.Find("TrackingSpace");
            trackingSpace = foundTrackingSpace != null ? foundTrackingSpace.transform : null;
        }

        environmentDepthManager.CustomTrackingSpace = trackingSpace;
        environmentDepthManager.RemoveHands = !letRealHandsOccludeVirtualContent;
        environmentDepthManager.OcclusionShadersMode = useSoftOcclusion
            ? OcclusionShadersMode.SoftOcclusion
            : OcclusionShadersMode.HardOcclusion;
        environmentDepthManager.enabled = true;
    }

    private static bool HasActiveXrLoader()
    {
        XRGeneralSettings settings = XRGeneralSettings.Instance;
        return settings != null
            && settings.Manager != null
            && settings.Manager.activeLoader != null;
    }

    private static bool HasScenePermission()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return Permission.HasUserAuthorizedPermission(ScenePermission);
#else
        return true;
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void RequestScenePermission()
    {
        if (Permission.HasUserAuthorizedPermission(ScenePermission))
        {
            Debug.Log("Spatial data permission was already granted.", this);
            return;
        }

        scenePermissionCallbacks = new PermissionCallbacks();
        scenePermissionCallbacks.PermissionGranted += permission =>
            Debug.Log($"Spatial data permission granted: {permission}.", this);
        scenePermissionCallbacks.PermissionDenied += permission =>
            Debug.LogError($"Spatial data permission denied: {permission}. Environment Depth cannot start; re-enable Spatial Data for this app in Quest Settings if no prompt appears next time.", this);

        Debug.Log($"Requesting spatial data permission: {ScenePermission}.", this);
        Permission.RequestUserPermission(ScenePermission, scenePermissionCallbacks);
    }
#endif
}
