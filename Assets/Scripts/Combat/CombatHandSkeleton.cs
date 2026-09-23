using UnityEngine;

/// <summary>Configures the provider's joint format before the SDK searches for its provider.</summary>
public sealed class CombatHandSkeleton : OVRSkeleton
{
    private IOVRSkeletonDataProvider handProvider;
    private Transform trackingSpace;
    protected override void Awake()
    {
        var source = GetComponentInParent<OVRHand>();
        if (source != null)
        {
            handProvider = (IOVRSkeletonDataProvider)source;
            _skeletonType = handProvider.GetSkeletonType();
        }
        var rig = FindFirstObjectByType<OVRCameraRig>();
        if (rig != null) trackingSpace = rig.trackingSpace;
        base.Awake();
    }

    protected override void Update()
    {
        if (trackingSpace == null)
        {
            var rig = FindFirstObjectByType<OVRCameraRig>();
            if (rig != null) trackingSpace = rig.trackingSpace;
        }
        if (handProvider != null && trackingSpace != null)
        {
            var data = handProvider.GetSkeletonPoseData();
            if (data.IsDataValid)
                ApplyTrackingPose(data.RootPose.Position.FromFlippedZVector3f(),
                    data.RootPose.Orientation.FromFlippedZQuatf(), data.RootScale);
        }
        base.Update();
    }

    private void ApplyTrackingPose(Vector3 position, Quaternion rotation, float scale)
    {
        // The OVRHand data-source object is not a tracked hand transform.
        // RootPose is expressed in camera-rig tracking space, independent of that object's parent.
        transform.SetPositionAndRotation(trackingSpace.TransformPoint(position), trackingSpace.rotation * rotation);
        Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;
        Vector3 worldScale = trackingSpace.lossyScale * scale;
        transform.localScale = new Vector3(worldScale.x / parentScale.x,
            worldScale.y / parentScale.y, worldScale.z / parentScale.z);
    }

    public static OVRSkeleton For(OVRHand hand)
    {
        if (hand == null) return null;
        var expected = ((IOVRSkeletonDataProvider)hand).GetSkeletonType();
        foreach (var existing in hand.GetComponentsInChildren<OVRSkeleton>(true))
            if (existing.GetSkeletonType() == expected && existing.isActiveAndEnabled)
                return existing;
        var root = new GameObject("Combat hand joints");
        root.transform.SetParent(hand.transform, false);
        return root.AddComponent<CombatHandSkeleton>();
    }
}
