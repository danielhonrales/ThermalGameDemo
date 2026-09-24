using UnityEngine;

/// <summary>Reads the active right-hand skeleton once per frame and routes one pose to combat.</summary>
[DisallowMultipleComponent]
public sealed class HandPoseRouter : MonoBehaviour
{
    public enum PoseKind { Neutral, Fire, Ice, Shield }
    [SerializeField] private OVRHand hand;
    [SerializeField] private OVRSkeleton skeleton;
    [SerializeField] private Transform handAnchor;
    [SerializeField, Min(0f)] private float trackingGraceSeconds = 0.12f;
    [SerializeField, Min(0f)] private float poseExitGraceSeconds = 0.10f;
    private int evaluatedFrame = -1;
    private PoseKind current;
    private PoseKind candidate;
    private float candidateSince;
    private float departureSince = -1f;
    private float lastValidAt = -10f;
    private float nextReferenceSearch;
    private Ray fireRay;
    private bool hasFireRay;

    public PoseKind Current { get { Evaluate(); return current; } }
    // Visual acknowledgement once a pose has been intended briefly (so a resting hand stays clean);
    // gameplay still requires the full stable-pose confirmation.
    public const float FeedbackIntentSeconds = 0.12f;
    public PoseKind FeedbackPose
    {
        get
        {
            Evaluate();
            if (current != PoseKind.Neutral) return current;
            return candidate != PoseKind.Neutral && Time.unscaledTime - candidateSince >= FeedbackIntentSeconds
                ? candidate : PoseKind.Neutral;
        }
    }
    public bool IsFirePose => Current == PoseKind.Fire;
    public bool IsIcePose => Current == PoseKind.Ice;
    public bool IsShieldPose => Current == PoseKind.Shield;
    public bool TryGetFireRay(out Ray ray)
    {
        Evaluate();
        ray = fireRay;
        return hasFireRay && FeedbackPose == PoseKind.Fire;
    }

    public bool TryGetTrackedWristRotation(out Quaternion rotation)
    {
        Evaluate();
        rotation = Quaternion.identity;
        if (hand == null || !hand.IsTracked || skeleton == null || !skeleton.IsDataValid) return false;
        Transform wrist = Bone(OVRSkeleton.BoneId.XRHand_Wrist, OVRSkeleton.BoneId.Hand_WristRoot);
        if (wrist == null) return false;
        rotation = wrist.rotation;
        return true;
    }

    public bool TryGetForearmPose(Transform head, out Vector3 position, out Quaternion rotation)
    {
        Evaluate();
        position = Vector3.zero;
        rotation = Quaternion.identity;
        if (hand == null || !hand.IsTracked || skeleton == null || !skeleton.IsDataValid) return false;
        Transform wrist = Bone(OVRSkeleton.BoneId.XRHand_Wrist, OVRSkeleton.BoneId.Hand_WristRoot);
        Transform middle = Bone(OVRSkeleton.BoneId.XRHand_MiddleProximal, OVRSkeleton.BoneId.Hand_Middle1);
        Transform index = Bone(OVRSkeleton.BoneId.XRHand_IndexProximal, OVRSkeleton.BoneId.Hand_Index1);
        Transform pinky = Bone(OVRSkeleton.BoneId.XRHand_LittleProximal, OVRSkeleton.BoneId.Hand_Pinky1);
        if (wrist == null || middle == null || index == null || pinky == null) return false;
        position = wrist.position;
        Vector3 along = middle.position - position;
        Vector3 normal = Vector3.Cross(index.position - pinky.position, along).normalized;
        // Hand tracking supplies no OpenXR elbow. Use a short wrist cuff aligned toward
        // a two-bone arm estimate, independent of projectile elevation or wrist flexion.
        Vector3 elbow = head != null ? EstimateElbow(head.position, head.rotation, position)
            : position - along.normalized * 0.24f;
        Vector3 forward = position - elbow;
        if (forward.sqrMagnitude < 0.0001f) forward = along;
        if (Vector3.Cross(forward, normal).sqrMagnitude < 0.0001f) normal = Vector3.up;
        rotation = Quaternion.LookRotation(forward, normal);
        return true;
    }

    public static Vector3 EstimateElbow(Vector3 head, Quaternion headRotation, Vector3 wrist)
    {
        Vector3 forward = Vector3.ProjectOnPlane(headRotation * Vector3.forward, Vector3.up).normalized;
        if (forward.sqrMagnitude < 0.1f) forward = Vector3.forward;
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        Vector3 shoulder = head + right * 0.18f - Vector3.up * 0.22f;
        Vector3 axis = wrist - shoulder;
        float distance = Mathf.Max(0.001f, axis.magnitude);
        axis /= distance;
        const float upper = 0.29f, lower = 0.25f;
        float reach = Mathf.Clamp(distance, 0.05f, upper + lower - 0.001f);
        float along = (upper * upper - lower * lower + reach * reach) / (2f * reach);
        float bend = Mathf.Sqrt(Mathf.Max(0f, upper * upper - along * along));
        Vector3 pole = Vector3.ProjectOnPlane(-Vector3.up + right * 0.35f, axis).normalized;
        return shoulder + axis * along + pole * bend;
    }

    private void Awake() => FindReferences();

    public void FindReferences()
    {
        nextReferenceSearch = Time.unscaledTime + 0.75f;
        if (handAnchor == null) handAnchor = GameObject.Find("RightHandAnchor")?.transform;
        OVRHand best = null;
        int bestScore = -1;
        foreach (OVRHand found in FindObjectsByType<OVRHand>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (found.GetHand() != OVRPlugin.Hand.HandRight) continue;
            int score = (found.isActiveAndEnabled ? 4 : 0) + (found.IsTracked ? 8 : 0)
                + (handAnchor != null && found.transform.IsChildOf(handAnchor) ? 2 : 0);
            if (score > bestScore) { best = found; bestScore = score; }
        }
        if (best != null) hand = best;
        if (hand != null)
            skeleton = CombatHandSkeleton.For(hand);
    }

    private void Evaluate()
    {
        if (evaluatedFrame == Time.frameCount) return;
        evaluatedFrame = Time.frameCount;
        if ((hand == null || !hand.isActiveAndEnabled || skeleton == null || !hand.IsTracked)
            && Time.unscaledTime >= nextReferenceSearch) FindReferences();
        bool valid = hand != null && hand.IsTracked && skeleton != null
            && skeleton.IsInitialized && skeleton.IsDataValid && skeleton.Bones != null;
        if (!valid)
        {
            candidate = PoseKind.Neutral;
            candidateSince = Time.unscaledTime;
            if (Time.unscaledTime - lastValidAt > trackingGraceSeconds)
            {
                current = candidate = PoseKind.Neutral;
                hasFireRay = false;
                departureSince = -1f;
            }
            Trace(false, -1f, -1f, -1f, -1f, false, PoseKind.Neutral);
            return;
        }

        float index = Finger(0, out Vector3 indexBase, out Vector3 indexTip);
        float middle = Finger(1, out Vector3 middleBase, out Vector3 middleTip);
        float ring = Finger(2, out _, out _);
        float pinky = Finger(3, out _, out _);
        if (index >= 0f || middle >= 0f) lastValidAt = Time.unscaledTime;
        bool palmUp = handAnchor != null && Vector3.Dot(-handAnchor.up, Vector3.up) > 0.35f;
        PoseKind observed = Classify(index, middle, ring, pinky, palmUp, current);
        AdvancePose(observed, Time.unscaledTime, poseExitGraceSeconds,
            ref current, ref candidate, ref candidateSince, ref departureSince);

        bool useIndex = index >= 0.67f || index >= middle;
        Vector3 start = useIndex ? indexBase : middleBase;
        Vector3 end = useIndex ? indexTip : middleTip;
        Vector3 direction = end - start;
        hasFireRay = Mathf.Max(index, middle) >= 0f && direction.sqrMagnitude > 0.0001f;
        if (hasFireRay) fireRay = new Ray(end + direction.normalized * 0.015f, direction.normalized);
        Trace(true, index, middle, ring, pinky, palmUp, observed);
    }

    // Entry thresholds are stricter than hold thresholds so a relaxed, resting hand stays Neutral.
    public static PoseKind Classify(float index, float middle, float ring, float pinky,
        bool palmUp, PoseKind previous)
    {
        float straight = previous == PoseKind.Fire || previous == PoseKind.Ice ? 0.67f : 0.82f;
        bool indexOut = index >= straight;
        bool middleOut = middle >= straight;
        float open = previous == PoseKind.Ice ? 0.56f : 0.74f;
        bool outerOpen = (ring >= open && (pinky >= open || pinky < 0f))
            || (pinky >= open && ring < 0f);
        if (indexOut && middleOut && outerOpen) return PoseKind.Ice;
        float tucked = previous == PoseKind.Fire ? 0.62f : 0.42f;
        bool outerTucked = (ring < 0f || ring <= tucked) && (pinky < 0f || pinky <= tucked)
            && (ring >= 0f || pinky >= 0f);
        if ((indexOut || middleOut) && outerTucked) return PoseKind.Fire;
        // A shield needs a real fist: all tracked fingers curled, not just a loose resting hand.
        bool holding = previous == PoseKind.Shield;
        float curled = holding ? 0.40f : 0.26f;
        float outerCurled = holding ? 0.62f : 0.45f;
        if (index >= 0f && middle >= 0f && index <= curled && middle <= curled
            && ring <= outerCurled && pinky <= outerCurled) return PoseKind.Shield;
        return PoseKind.Neutral;
    }

    public const float ShieldConfirmSeconds = 0.18f;
    public const float AttackConfirmSeconds = 0.28f;

    public static void AdvancePose(PoseKind observed, float now, float releaseSeconds,
        ref PoseKind active, ref PoseKind pending, ref float pendingSince, ref float departure)
    {
        if (observed == active)
        {
            pending = observed;
            pendingSince = now;
            departure = -1f;
            return;
        }
        if (observed != pending) { pending = observed; pendingSince = now; }
        if (departure < 0f) departure = now;
        // Release promptly, but never fire the next weapon while merely passing through its pose.
        if (active != PoseKind.Neutral && now - departure >= releaseSeconds) active = PoseKind.Neutral;
        if (observed != PoseKind.Neutral && now - pendingSince >= (observed == PoseKind.Shield ? ShieldConfirmSeconds : AttackConfirmSeconds))
        {
            active = observed;
            departure = -1f;
        }
    }

    // Anatomical bend is independent of hand size and wrist position.
    public static float MeasureExtension(Vector3 knuckleDirection, Vector3 proximal,
        Vector3 intermediate, Vector3 distal)
    {
        if (knuckleDirection.sqrMagnitude < 0.000001f || proximal.sqrMagnitude < 0.000001f
            || intermediate.sqrMagnitude < 0.000001f || distal.sqrMagnitude < 0.000001f) return -1f;
        float bend = Vector3.Angle(knuckleDirection, proximal) * 0.6f
            + Vector3.Angle(proximal, intermediate)
            + Vector3.Angle(intermediate, distal) * 0.6f;
        return 1f - Mathf.Clamp01(bend / 160f);
    }

    private static readonly OVRSkeleton.BoneId[,] XrFingerBones = {
        { OVRSkeleton.BoneId.XRHand_IndexMetacarpal, OVRSkeleton.BoneId.XRHand_IndexProximal, OVRSkeleton.BoneId.XRHand_IndexIntermediate, OVRSkeleton.BoneId.XRHand_IndexDistal, OVRSkeleton.BoneId.XRHand_IndexTip },
        { OVRSkeleton.BoneId.XRHand_MiddleMetacarpal, OVRSkeleton.BoneId.XRHand_MiddleProximal, OVRSkeleton.BoneId.XRHand_MiddleIntermediate, OVRSkeleton.BoneId.XRHand_MiddleDistal, OVRSkeleton.BoneId.XRHand_MiddleTip },
        { OVRSkeleton.BoneId.XRHand_RingMetacarpal, OVRSkeleton.BoneId.XRHand_RingProximal, OVRSkeleton.BoneId.XRHand_RingIntermediate, OVRSkeleton.BoneId.XRHand_RingDistal, OVRSkeleton.BoneId.XRHand_RingTip },
        { OVRSkeleton.BoneId.XRHand_LittleMetacarpal, OVRSkeleton.BoneId.XRHand_LittleProximal, OVRSkeleton.BoneId.XRHand_LittleIntermediate, OVRSkeleton.BoneId.XRHand_LittleDistal, OVRSkeleton.BoneId.XRHand_LittleTip }
    };
    private static readonly OVRSkeleton.BoneId[,] LegacyFingerBones = {
        { OVRSkeleton.BoneId.Hand_WristRoot, OVRSkeleton.BoneId.Hand_Index1, OVRSkeleton.BoneId.Hand_Index2, OVRSkeleton.BoneId.Hand_Index3, OVRSkeleton.BoneId.Hand_IndexTip },
        { OVRSkeleton.BoneId.Hand_WristRoot, OVRSkeleton.BoneId.Hand_Middle1, OVRSkeleton.BoneId.Hand_Middle2, OVRSkeleton.BoneId.Hand_Middle3, OVRSkeleton.BoneId.Hand_MiddleTip },
        { OVRSkeleton.BoneId.Hand_WristRoot, OVRSkeleton.BoneId.Hand_Ring1, OVRSkeleton.BoneId.Hand_Ring2, OVRSkeleton.BoneId.Hand_Ring3, OVRSkeleton.BoneId.Hand_RingTip },
        { OVRSkeleton.BoneId.Hand_Pinky0, OVRSkeleton.BoneId.Hand_Pinky1, OVRSkeleton.BoneId.Hand_Pinky2, OVRSkeleton.BoneId.Hand_Pinky3, OVRSkeleton.BoneId.Hand_PinkyTip }
    };

    private float Finger(int finger, out Vector3 proximalPosition, out Vector3 tipPosition)
    {
        Transform metacarpal = Bone(XrFingerBones[finger, 0], LegacyFingerBones[finger, 0]);
        Transform proximal = Bone(XrFingerBones[finger, 1], LegacyFingerBones[finger, 1]);
        Transform intermediate = Bone(XrFingerBones[finger, 2], LegacyFingerBones[finger, 2]);
        Transform distal = Bone(XrFingerBones[finger, 3], LegacyFingerBones[finger, 3]);
        Transform tip = Bone(XrFingerBones[finger, 4], LegacyFingerBones[finger, 4]);
        proximalPosition = proximal != null ? proximal.position : Vector3.zero;
        tipPosition = tip != null ? tip.position : Vector3.zero;
        if (metacarpal == null || proximal == null || intermediate == null || distal == null || tip == null) return -1f;
        return MeasureExtension(proximal.position - metacarpal.position,
            intermediate.position - proximal.position, distal.position - intermediate.position, tip.position - distal.position);
    }

    private Transform Bone(OVRSkeleton.BoneId first, OVRSkeleton.BoneId second)
    {
        if (skeleton == null || skeleton.Bones == null) return null;
        var type = skeleton.GetSkeletonType();
        bool xr = type == OVRSkeleton.SkeletonType.XRHandRight || type == OVRSkeleton.SkeletonType.XRHandLeft;
        OVRSkeleton.BoneId selected = xr ? first : second;
        foreach (OVRBone bone in skeleton.Bones)
            if (bone.Id == selected && bone.Transform != null) return bone.Transform;
        return null;
    }

    private void OnDisable()
    {
        current = candidate = PoseKind.Neutral;
        departureSince = -1f;
        hasFireRay = false;
        CloseTrace();
    }

#if DEVELOPMENT_BUILD || UNITY_EDITOR
    private System.IO.StreamWriter trace;
    private float nextTraceAt;
    private int traceSamples;
#endif
    private void Trace(bool valid, float index, float middle, float ring, float pinky, bool palmUp, PoseKind observed)
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        if (!Application.isPlaying || Time.unscaledTime < nextTraceAt || traceSamples >= 6000) return;
        nextTraceAt = Time.unscaledTime + 0.1f;
        try
        {
            if (trace == null)
            {
                trace = new System.IO.StreamWriter(System.IO.Path.Combine(Application.persistentDataPath, "pose-trace.csv"), traceSamples > 0);
                if (traceSamples == 0) trace.WriteLine("time,tracked,valid,skeleton,index,middle,ring,pinky,palmUp,observed,pose");
            }
            trace.WriteLine(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "{0:F3},{1},{2},{3},{4:F3},{5:F3},{6:F3},{7:F3},{8},{9},{10}", Time.unscaledTime,
                hand != null && hand.IsTracked, valid, skeleton != null ? skeleton.GetSkeletonType().ToString() : "None",
                index, middle, ring, pinky, palmUp, observed, current));
            if (++traceSamples % 10 == 0) trace.Flush();
        }
        catch (System.IO.IOException) { CloseTrace(); }
#endif
    }
    private void CloseTrace()
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        trace?.Dispose(); trace = null;
#endif
    }
    private void OnApplicationPause(bool paused) { if (paused) CloseTrace(); }
}
