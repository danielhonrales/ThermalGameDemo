using UnityEngine;

[DisallowMultipleComponent]
public sealed class LeftFistRemotePlayerAdjuster : MonoBehaviour
{
    [SerializeField] private OVRHand leftHand;
    [SerializeField] private OVRSkeleton leftHandSkeleton;
    [SerializeField, Range(0.06f, 0.2f)] private float fingertipNearPalmDistance = 0.13f;
    [SerializeField, Range(1, 4)] private int requiredFingertipsNearPalm = 2;
    [SerializeField, Min(0f)] private float holdBeforeDragSeconds = 0.5f;
    [SerializeField, Min(0.01f)] private float movementScale = 1f;
    [SerializeField] private bool allowVerticalMovement = true;
    [SerializeField] private bool logAdjustment = true;

    private float fistHeldTime;
    private bool dragging;
    private Vector3 dragStartHandPosition;
    private Vector3 dragStartOffset;

    private void Update()
    {
        if (!IsClosedFist() || !TryGetHandPosition(out Vector3 handPosition))
        {
            if (dragging && logAdjustment)
            {
                Debug.Log($"Remote player manual offset saved as {RemotePlayerCorrection.WorldOffset}.", this);
            }

            fistHeldTime = 0f;
            dragging = false;
            return;
        }

        if (!dragging)
        {
            fistHeldTime += Time.deltaTime;
            if (fistHeldTime < holdBeforeDragSeconds)
            {
                return;
            }

            dragging = true;
            dragStartHandPosition = handPosition;
            dragStartOffset = RemotePlayerCorrection.WorldOffset;

            if (logAdjustment)
            {
                Debug.Log("Left-fist remote player adjustment started.", this);
            }
        }

        Vector3 movement = (handPosition - dragStartHandPosition) * movementScale;
        if (!allowVerticalMovement)
        {
            movement.y = 0f;
        }

        RemotePlayerCorrection.SetWorldOffset(dragStartOffset + movement);
    }

    [ContextMenu("Reset Remote Player Offset")]
    public void ResetRemotePlayerOffset()
    {
        RemotePlayerCorrection.Reset();
    }

    private bool IsClosedFist()
    {
        if (leftHand == null || !leftHand.IsTracked)
        {
            return false;
        }

        EnsureSkeleton();
        Transform palm = FindBone(OVRSkeleton.BoneId.XRHand_Palm, OVRSkeleton.BoneId.Hand_WristRoot);
        if (palm == null)
        {
            palm = FindBone(OVRSkeleton.BoneId.XRHand_Wrist, OVRSkeleton.BoneId.Hand_WristRoot);
        }

        if (palm != null)
        {
            int nearPalmCount = 0;
            nearPalmCount += IsNearPalm(palm, FindBone(OVRSkeleton.BoneId.XRHand_IndexTip, OVRSkeleton.BoneId.Hand_IndexTip)) ? 1 : 0;
            nearPalmCount += IsNearPalm(palm, FindBone(OVRSkeleton.BoneId.XRHand_MiddleTip, OVRSkeleton.BoneId.Hand_MiddleTip)) ? 1 : 0;
            nearPalmCount += IsNearPalm(palm, FindBone(OVRSkeleton.BoneId.XRHand_RingTip, OVRSkeleton.BoneId.Hand_RingTip)) ? 1 : 0;
            nearPalmCount += IsNearPalm(palm, FindBone(OVRSkeleton.BoneId.XRHand_LittleTip, OVRSkeleton.BoneId.Hand_PinkyTip)) ? 1 : 0;
            if (nearPalmCount >= requiredFingertipsNearPalm)
            {
                return true;
            }
        }

        int pinchingFingers = 0;
        pinchingFingers += leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index) ? 1 : 0;
        pinchingFingers += leftHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) ? 1 : 0;
        pinchingFingers += leftHand.GetFingerIsPinching(OVRHand.HandFinger.Ring) ? 1 : 0;
        pinchingFingers += leftHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky) ? 1 : 0;
        return pinchingFingers >= requiredFingertipsNearPalm;
    }

    private bool IsNearPalm(Transform palm, Transform fingertip)
    {
        return fingertip != null
            && Vector3.Distance(fingertip.position, palm.position) <= fingertipNearPalmDistance;
    }

    private Transform FindBone(params OVRSkeleton.BoneId[] candidates)
    {
        if (leftHandSkeleton == null || !leftHandSkeleton.IsInitialized || leftHandSkeleton.Bones == null)
        {
            return null;
        }

        foreach (OVRSkeleton.BoneId candidate in candidates)
        {
            foreach (OVRBone bone in leftHandSkeleton.Bones)
            {
                if (bone.Id == candidate && bone.Transform != null)
                {
                    return bone.Transform;
                }
            }
        }

        return null;
    }

    private bool TryGetHandPosition(out Vector3 position)
    {
        position = default;
        EnsureSkeleton();

        if (leftHandSkeleton != null && leftHandSkeleton.IsInitialized && leftHandSkeleton.Bones != null)
        {
            foreach (OVRBone bone in leftHandSkeleton.Bones)
            {
                if ((bone.Id == OVRSkeleton.BoneId.XRHand_Wrist || bone.Id == OVRSkeleton.BoneId.Hand_WristRoot)
                    && bone.Transform != null)
                {
                    position = bone.Transform.position;
                    return true;
                }
            }
        }

        if (leftHand != null && leftHand.IsPointerPoseValid && leftHand.PointerPose != null)
        {
            position = leftHand.PointerPose.position;
            return true;
        }

        return false;
    }

    private void EnsureSkeleton()
    {
        if (leftHandSkeleton != null || leftHand == null)
        {
            return;
        }

        leftHandSkeleton = leftHand.GetComponent<OVRSkeleton>();
        if (leftHandSkeleton == null)
        {
            leftHandSkeleton = leftHand.GetComponentInChildren<OVRSkeleton>(true);
        }

        if (leftHandSkeleton == null)
        {
            leftHandSkeleton = leftHand.GetComponentInParent<OVRSkeleton>();
        }

        if (leftHandSkeleton == null)
        {
            leftHandSkeleton = leftHand.gameObject.AddComponent<OVRSkeleton>();
        }
    }
}
