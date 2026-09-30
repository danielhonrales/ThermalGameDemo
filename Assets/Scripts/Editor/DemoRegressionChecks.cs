using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class DemoRegressionChecks
{
    public static void RunAll()
    {
        RunLanElectionCheck();
        RunBoneLookup();
        RunSkeletonProvider();
        RunPoseFixtures();
        RunInteractionChecks();
        RunTrajectoryChecks();
        RunHudFollowCheck();
        RunFeedbackChecks();
        RunOutputCheck();
        RunSafeZoneCheck();
        RunArenaSequenceCheck();
        RunSoloCheck();
        RunRepeatedIceVisualCheck();
        RunArmEffectsCheck();
        RunSpectatorCheck();
        CaptureHud();
        CaptureSandboxGuide();
    }

    public static void RunLanElectionCheck()
    {
        if (!LanMatchManager.ShouldYieldTo("b", "a", 1)
            || LanMatchManager.ShouldYieldTo("a", "b", 1)
            || LanMatchManager.ShouldYieldTo("b", "a", 2))
            throw new Exception("LAN host collision must keep the smaller ID and preserve a full match.");
        Debug.Log("LAN ELECTION PASSED: host ID tie-break preserves a full match.");
    }

    public static void RunSafeZoneCheck()
    {
        if (SafeZoneHazardView.WarningAt != 20f
            || SafeZoneHazardView.BlastAt - SafeZoneHazardView.WarningAt != 5f
            || SafeZoneHazardView.HealAt <= SafeZoneHazardView.BlastEndsAt
            || SafeZoneHazardView.IsExploding(24.99f)
            || !SafeZoneHazardView.IsExploding(25f)
            || SafeZoneHazardView.IsExploding(28f))
            throw new Exception("Safe-zone warning, blast, or heal timing is wrong.");
        for (int seed = 0; seed < 12; seed++)
        {
            var heads = new[] { new Vector3(-0.8f, 1.6f, 0f), new Vector3(1.3f, 1.6f, 1.4f) };
            if (!SafeZoneHazardView.SelectZones(heads, _ => true, seed, out var first, out var second))
                throw new Exception("Procedural zones missing.");
            foreach (var head in heads)
                if (SafeZoneHazardView.DistanceToZone(first, head) < SafeZoneHazardView.MinimumMove
                    || SafeZoneHazardView.DistanceToZone(second, head) < SafeZoneHazardView.MinimumMove)
                    throw new Exception("A safe zone did not require movement.");
            if (first.x + SafeZoneHazardView.HalfWidth > SafeZoneHazardView.Midline
                || second.x - SafeZoneHazardView.HalfWidth < SafeZoneHazardView.Midline
                || Vector2.Distance(first, second) < 1.25f
                || !SafeZoneHazardView.Contains(first, second, new Vector3(first.x, 1.6f, first.y)))
                throw new Exception("Safe-zone footprints overlap or reject their center.");
        }
        if (SafeZoneHazardView.SelectZones(new[] { Vector3.zero }, _ => false, 1, out _, out _))
            throw new Exception("Blocked arena must not spawn unreachable refuges.");
        Debug.Log("SAFE ZONE PASSED: procedural refuges require movement, respect blocked space, five-second warning.");
    }

    public static void RunArenaSequenceCheck()
    {
        float coverDeliveredAt = SuddenDeathDirector.DeliveryStart + CoverDrone.GrabMid / CoverDrone.DeliverySpeed;
        float dronesGoneAt = SuddenDeathDirector.DeliveryStart + CoverDrone.ExchangeGone / CoverDrone.DeliverySpeed;
        if (SuddenDeathDirector.PickupStart != SuddenDeathDirector.DeliveryStart
            || coverDeliveredAt >= 0f || dronesGoneAt >= 0f
            || SafeZoneHazardView.BlastEndsAt >= 40f + SuddenDeathDirector.PickupStart)
            throw new Exception("Sudden-death cover must settle before 40 seconds after the safe-zone blast.");
        Debug.Log("ARENA SEQUENCE PASSED: safe-zone blast ends before cover moves; drones leave before 40 seconds.");
    }

    public static void RunSoloCheck()
    {
        if (!HandPoseRouter.ClassifyThumbsUp(0.8f, 0.8f, 0.2f, 0.2f, 0.3f, 0.3f)
            || HandPoseRouter.ClassifyThumbsUp(0.8f, -0.8f, 0.2f, 0.2f, 0.3f, 0.3f)
            || HandPoseRouter.ClassifyThumbsUp(0.8f, 0.8f, 0.9f, 0.2f, 0.3f, 0.3f)
            || FusionRoundDirector.CanStartRound(1, true, false)
            || !FusionRoundDirector.CanStartRound(1, true, true)
            || FusionRoundDirector.CanStartRound(1, false, true)
            || !FusionRoundDirector.CanStartRound(2, true, false))
            throw new Exception("Solo gesture or round admission failed.");
        if (FusionRoundDirector.CanEnterSandbox(2, false)
            || !FusionRoundDirector.CanEnterSandbox(2, true)
            || !FusionRoundDirector.CanEnterSandbox(1, true)
            || FusionRoundDirector.CanEnterSandbox(0, true))
            throw new Exception("Practice sandbox admission failed.");
        Debug.Log("SANDBOX PASSED: calibrated players practice until a valid thumbs-up starts the round.");
    }

    public static void RunRepeatedIceVisualCheck()
    {
        var root = new GameObject("Repeated ice visual regression");
        try
        {
            var visual = root.AddComponent<NetworkPlayerGrenadeVisual>();
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var type = typeof(NetworkPlayerGrenadeVisual);
            type.GetMethod("SetThrow", flags).Invoke(visual,
                new object[] { Vector3.zero, Vector3.forward, 0f, true });
            type.GetMethod("SetCharge", flags).Invoke(visual,
                new object[] { Vector3.zero, Quaternion.identity, 0.25f, true });
            if (!(bool)type.GetField("GrenadeVisible", flags).GetValue(visual)
                || !(bool)type.GetField("ChargeVisible", flags).GetValue(visual))
                throw new Exception("Starting the next ice charge hid the previous bomb in flight.");
            type.GetMethod("SetExplosion", flags).Invoke(visual,
                new object[] { Vector3.zero, 0, Vector3.zero, true });
            if (!(bool)type.GetField("ChargeVisible", flags).GetValue(visual))
                throw new Exception("The previous ice explosion hid the next charge.");
        }
        finally { UnityEngine.Object.DestroyImmediate(root); }
        Debug.Log("REPEATED ICE PASSED: new charge stays visible during previous flight and explosion.");
    }

    public static void RunArmEffectsCheck()
    {
        var root = new GameObject("Arm effects regression");
        root.SetActive(false);
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var effects = root.AddComponent<IceGrenadeEffects>();
        var onSignal = typeof(ArmActivationSignal).GetMethod("OnSignal", BindingFlags.Static | BindingFlags.NonPublic);
        try
        {
            var rings = (LineRenderer[])typeof(IceGrenadeEffects).GetField("chargeRings", flags).GetValue(effects);
            foreach (var (progress, expected) in new[] { (0.1f, 1), (0.5f, 3), (1f, 5) })
            {
                effects.ShowCharge(Vector3.zero, Quaternion.identity, progress, false);
                int lit = 0;
                float previousZ = float.NegativeInfinity;
                foreach (LineRenderer ring in rings)
                {
                    if (!ring.enabled) continue;
                    lit++;
                    float z = ring.GetPosition(0).z;
                    for (int i = 1; i < ring.positionCount; i++)
                        if (Mathf.Abs(ring.GetPosition(i).z - z) > 0.0001f)
                            throw new Exception("Ice forearm ring is not a flat ring around the arm.");
                    if (z <= previousZ) throw new Exception("Ice forearm rings must land from elbow to wrist.");
                    previousZ = z;
                }
                if (lit != expected) throw new Exception($"Ice charge at {progress:P0} lit {lit} forearm rings, expected {expected}.");
            }
            ArmActivationSignal.Sample(Time.time); // First sample subscribes and resets outside play mode.
            onSignal.Invoke(null, new object[] { "session_pause", "" });
            onSignal.Invoke(null, new object[] { "ice_charge_start", "ice" });
            if (ArmActivationSignal.Sample(Time.time + 0.1f).Cold < 0.999f)
                throw new Exception("An ice charge must fade the forearm spiral.");
            onSignal.Invoke(null, new object[] { "fire_start", "fire" });
            var firing = ArmActivationSignal.Sample(Time.time + 0.1f);
            if (firing.Cold > 0.5f || firing.Shield > 0f)
                throw new Exception("Firing must keep the forearm spiral.");
            onSignal.Invoke(null, new object[] { "session_pause", "" });
            onSignal.Invoke(null, new object[] { "shield_start", "shield" });
            if (ArmActivationSignal.Sample(Time.time + 0.1f).Shield < 0.999f)
                throw new Exception("Raising the shield must swap the forearm spiral for its aura.");

            var arm = root.AddComponent<ForearmPulseSpiral>();
            var armType = typeof(ForearmPulseSpiral);
            armType.GetMethod("Awake", flags).Invoke(arm, null);
            var eye = new GameObject("Aura eye").transform;
            eye.SetParent(root.transform, false);
            eye.position = new Vector3(0f, 1f, 0.12f); // Above a forearm lying along +Z from the origin.
            armType.GetField("headset", flags).SetValue(arm, eye);
            armType.GetField("visible", flags).SetValue(arm, 1f);
            var updateAura = armType.GetMethod("UpdateAura", flags);
            var aura = (MeshRenderer)armType.GetField("auraRenderer", flags).GetValue(arm);
            var colors = (Color32[])armType.GetField("auraColors", flags).GetValue(arm);
            int sides = (int)armType.GetField("AuraSides", BindingFlags.Static | BindingFlags.NonPublic).GetRawConstantValue();
            int rows = (int)armType.GetField("AuraRows", BindingFlags.Static | BindingFlags.NonPublic).GetRawConstantValue();
            updateAura.Invoke(arm, new object[] { new ArmActivationSignal.Reading { Shield = 1f, Intensity = 0.55f }, 0.55f, Vector3.zero, 1f });
            int middle = rows / 2 * sides; // Inner glow, halfway along the arm; side 0 is edge-on, side/4 faces the eye.
            if (!aura.enabled || colors[0].a != 0 || colors[(rows - 1) * sides].a != 0
                || colors[middle].a <= colors[middle + sides / 4].a)
                throw new Exception("Shield aura must fade toward elbow and wrist and glow brightest around the arm's outline.");
            updateAura.Invoke(arm, new object[] { new ArmActivationSignal.Reading { Intensity = 0.55f }, 0.55f, Vector3.zero, 1f });
            if (aura.enabled) throw new Exception("Shield aura must stay hidden without a shield.");
        }
        finally
        {
            onSignal.Invoke(null, new object[] { "session_pause", "" });
            foreach (string name in new[] { "impactRoot", "hitRoot" })
                if (typeof(IceGrenadeEffects).GetField(name, flags).GetValue(effects) is Transform leftover)
                    UnityEngine.Object.DestroyImmediate(leftover.gameObject);
            UnityEngine.Object.DestroyImmediate(root);
        }
        Debug.Log("ARM EFFECTS PASSED: ice rings land elbow to wrist; ice fades the spiral, the shield swaps it for a fading edge-lit aura, fire keeps it.");
    }

    public static void RunSpectatorCheck()
    {
        // Head poses arrive at the 20 Hz send rate; spectator views interpolate between them.
        var samples = new List<(float time, Pose pose)>();
        SpectatorDirector.AddSample(samples, 10f, new Pose(Vector3.zero, Quaternion.identity));
        SpectatorDirector.AddSample(samples, 10.05f, new Pose(Vector3.right, Quaternion.Euler(0f, 90f, 0f)));
        Pose middle = SpectatorDirector.Interpolate(samples, 10.025f);
        if (Vector3.Distance(middle.position, Vector3.right * 0.5f) > 0.001f
            || Quaternion.Angle(middle.rotation, Quaternion.Euler(0f, 45f, 0f)) > 0.1f)
            throw new Exception("Spectator head views must interpolate between network samples.");
        // After standing still, movement resumes from the resting pose instead of jumping ahead.
        SpectatorDirector.AddSample(samples, 12f, new Pose(Vector3.right * 2f, Quaternion.identity));
        if (Vector3.Distance(SpectatorDirector.Interpolate(samples, 11.9f).position, Vector3.right) > 0.001f)
            throw new Exception("Spectator head view jumped when a still player moved again.");
        int count = samples.Count;
        SpectatorDirector.AddSample(samples, 12.01f, samples[count - 1].pose);
        if (samples.Count != count) throw new Exception("An unchanged head pose must not add a sample.");

        var layout = new Bounds(new Vector3(0.275f, 0.6f, 0.71f), new Vector3(5.2f, 1.2f, 4f));
        List<SpectatorDirector.Shot> shots = SpectatorDirector.GenerateShots(layout);
        Vector3 focus = new Vector3(layout.center.x, 0.9f, layout.center.z);
        foreach (SpectatorDirector.Shot shot in shots)
            if (Mathf.Abs(shot.LocalPosition.x) > SpectatorDirector.PlayfieldHalf + 0.001f
                || Mathf.Abs(shot.LocalPosition.z) > SpectatorDirector.PlayfieldHalf + 0.001f
                || Vector3.Dot(shot.LocalRotation * Vector3.forward, focus - shot.LocalPosition) <= 0f)
                throw new Exception($"Spectator camera {shot.Label} is outside the playfield or faces away from the arena.");
        if (shots.Count < 5) throw new Exception("Spectator gallery needs several arena cameras.");
        Debug.Log($"SPECTATOR PASSED: head views interpolate 20 Hz poses and resume smoothly; {shots.Count} generated arena cameras stay in the playfield facing the layout.");
    }

    public static void RunSkeletonProvider()
    {
        var root = new GameObject("Provider regression");
        root.SetActive(false);
        try
        {
            var hand = root.AddComponent<OVRHand>();
            root.AddComponent<OVRSkeleton>(); // Matches the broken, unconfigured runtime component.
            var resolved = CombatHandSkeleton.For(hand);
            typeof(CombatHandSkeleton).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(resolved, null);
            var expected = ((OVRSkeleton.IOVRSkeletonDataProvider)hand).GetSkeletonType();
            var provider = typeof(OVRSkeleton).GetField("_dataProvider", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(resolved);
            if (resolved.GetSkeletonType() != expected || !ReferenceEquals(provider, hand))
                throw new Exception("Runtime skeleton must select the hand format and bind its actual provider before initialization.");
            var tracking = new GameObject("Tracking space fixture");
            try
            {
                tracking.transform.SetPositionAndRotation(new Vector3(2f, 1f, -3f), Quaternion.Euler(0f, 70f, 0f));
                root.transform.SetPositionAndRotation(new Vector3(-4f, 0f, 1f), Quaternion.Euler(0f, -30f, 0f));
                typeof(CombatHandSkeleton).GetField("trackingSpace", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(resolved, tracking.transform);
                Vector3 localWrist = new Vector3(0.3f, 1.1f, 0.6f);
                Quaternion localRotation = Quaternion.Euler(20f, 35f, 10f);
                typeof(CombatHandSkeleton).GetMethod("ApplyTrackingPose", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(resolved, new object[] { localWrist, localRotation, 1.1f });
                if (Vector3.Distance(resolved.transform.position, tracking.transform.TransformPoint(localWrist)) > 0.001f
                    || Quaternion.Angle(resolved.transform.rotation, tracking.transform.rotation * localRotation) > 0.01f
                    || Mathf.Abs(resolved.transform.lossyScale.x - 1.1f) > 0.001f)
                    throw new Exception("Hand world pose must follow tracking space, not the data-source parent.");
            }
            finally { UnityEngine.Object.DestroyImmediate(tracking); }
            Debug.Log("SKELETON PROVIDER PASSED: unconfigured skeleton bypassed; actual hand provider bound.");
            Debug.Log("HAND WORLD POSE PASSED: translated/rotated tracking space and independent data-source parent.");
        }
        finally { UnityEngine.Object.DestroyImmediate(root); }
    }

    public static void RunPoseFixtures()
    {
        Vector3 Direction(float degrees) => Quaternion.Euler(degrees, 0f, 0f) * Vector3.forward * 0.035f;
        float straight = HandPoseRouter.MeasureExtension(Direction(0), Direction(8), Direction(12), Direction(15));
        float curled = HandPoseRouter.MeasureExtension(Direction(0), Direction(50), Direction(120), Direction(160));
        if (straight < 0.85f || curled > 0.35f) throw new Exception("Finger bend fixture failed.");
        var cases = new[] {
            (straight, curled, curled, curled, false, HandPoseRouter.PoseKind.Neutral),
            (curled, straight, curled, curled, false, HandPoseRouter.PoseKind.Neutral),
            (straight, straight, curled, curled, true, HandPoseRouter.PoseKind.Fire),
            (straight, straight, straight, straight, false, HandPoseRouter.PoseKind.Ice),
            (straight, straight, -1f, straight, false, HandPoseRouter.PoseKind.Ice),
            (straight, curled, -1f, curled, false, HandPoseRouter.PoseKind.Neutral),
            (straight, curled, -1f, -1f, false, HandPoseRouter.PoseKind.Neutral),
            (curled, curled, -1f, -1f, false, HandPoseRouter.PoseKind.Shield),
            (-1f, -1f, -1f, -1f, false, HandPoseRouter.PoseKind.Neutral),
            (0.57f, 0.57f, 0.2f, 0.2f, false, HandPoseRouter.PoseKind.Neutral),
            (0.4f, 0.4f, 0.2f, 0.2f, false, HandPoseRouter.PoseKind.Neutral),
            (straight, straight, 0.75f, 0.35f, false, HandPoseRouter.PoseKind.Neutral),
            // Relaxed resting hand: loosely curled everywhere must not raise a shield or fire.
            (0.45f, 0.40f, 0.50f, 0.55f, false, HandPoseRouter.PoseKind.Neutral),
            (0.30f, 0.30f, 0.60f, 0.60f, false, HandPoseRouter.PoseKind.Neutral),
            (0.80f, 0.30f, 0.45f, 0.45f, false, HandPoseRouter.PoseKind.Neutral)
        };
        foreach (var c in cases)
        {
            var actual = HandPoseRouter.Classify(c.Item1, c.Item2, c.Item3, c.Item4, c.Item5, HandPoseRouter.PoseKind.Neutral);
            if (actual != c.Item6) throw new Exception($"Pose fixture expected {c.Item6}, got {actual}.");
        }
        if (HandPoseRouter.Classify(straight, straight, straight, straight, false, HandPoseRouter.PoseKind.Shield)
            != HandPoseRouter.PoseKind.Ice) throw new Exception("Opening fist must release shield.");
        Quaternion rotation = Quaternion.Euler(70f, 105f, 33f);
        float rotated = HandPoseRouter.MeasureExtension(rotation * Direction(0), rotation * Direction(8),
            rotation * Direction(12), rotation * Direction(15));
        if (Mathf.Abs(rotated - straight) > 0.001f) throw new Exception("Hand orientation changed finger classification.");
        Debug.Log("POSE FIXTURES PASSED: two-finger gun, open palm, relaxed fist, occluded outer fingers, invalid tracking, and rotated hand.");
    }

    public static void RunHudFollowCheck()
    {
        var root = new GameObject("HUD follow regression");
        var cameraRoot = new GameObject("Tracked eye regression");
        try
        {
            var camera = cameraRoot.AddComponent<Camera>();
            var hud = root.AddComponent<FusionRoundHud>();
            hud.Preview(camera);
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var hudRoot = (RectTransform)typeof(FusionRoundHud).GetField("root", flags).GetValue(hud);
            var status = (RectTransform)typeof(FusionRoundHud).GetField("statusRoot", flags).GetValue(hud);
            Vector3 expectedLocal = camera.transform.InverseTransformPoint(hudRoot.position);
            // Calibration/network updates can stop while tracking keeps moving.
            camera.transform.SetPositionAndRotation(new Vector3(3f, 1.6f, -2f), Quaternion.Euler(10f, 85f, 0f));
            if (Vector3.Distance(hudRoot.position, camera.transform.TransformPoint(expectedLocal)) > 0.001f)
                throw new Exception("Head-locked banners freeze in world space between round updates/calibration.");
            // Health and clock hold eye height in the facing direction whatever the head pitch or tilt.
            Vector3 facing = Quaternion.Euler(0f, 85f, 0f) * Vector3.forward;
            typeof(FusionRoundHud).GetField("smoothedHeading", flags).SetValue(hud, facing);
            var position = typeof(FusionRoundHud).GetMethod("Position", flags);
            foreach (Vector3 look in new[] { new Vector3(-35f, 85f, 0f), new Vector3(0f, 85f, 0f),
                new Vector3(45f, 85f, 25f), new Vector3(85f, 85f, 0f) })
            {
                camera.transform.rotation = Quaternion.Euler(look);
                position.Invoke(hud, new object[] { camera });
                if (Vector3.Distance(status.position, camera.transform.position + facing * 1.5f) > 0.001f
                    || Vector3.Dot(status.up, Vector3.up) < 0.999f)
                    throw new Exception($"Health and clock moved with head pitch/tilt {look}.");
            }
            Debug.Log("HUD FOLLOW PASSED: banners stay eye-relative without a network refresh; health and clock keep eye height at any head pitch.");
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(root);
            UnityEngine.Object.DestroyImmediate(cameraRoot);
        }
    }

    public static void RunFeedbackChecks()
    {
        var root = new GameObject("Immediate feedback regression");
        root.transform.position = Vector3.one * 1000f;
        root.SetActive(false);
        var cover = GameObject.CreatePrimitive(PrimitiveType.Cube);
        try
        {
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var router = root.AddComponent<HandPoseRouter>();
            void SetRouter(string name, object value) => typeof(HandPoseRouter).GetField(name, flags).SetValue(router, value);
            foreach (var pose in new[] { HandPoseRouter.PoseKind.Fire, HandPoseRouter.PoseKind.Ice, HandPoseRouter.PoseKind.Shield })
            {
                SetRouter("evaluatedFrame", Time.frameCount);
                SetRouter("candidate", pose);
                SetRouter("current", HandPoseRouter.PoseKind.Neutral);
                SetRouter("candidateSince", Time.unscaledTime);
                if (router.FeedbackPose != HandPoseRouter.PoseKind.Neutral)
                    throw new Exception("A pose glimpsed for one frame (resting hand) must not show weapon feedback.");
                SetRouter("candidateSince", Time.unscaledTime - HandPoseRouter.FeedbackIntentSeconds - 0.01f);
                if (router.FeedbackPose != pose || router.Current != HandPoseRouter.PoseKind.Neutral)
                    throw new Exception("Intended-pose feedback must appear without prematurely confirming gameplay.");
            }
            SetRouter("candidate", HandPoseRouter.PoseKind.Fire);
            SetRouter("candidateSince", Time.unscaledTime - 1f);
            SetRouter("hasFireRay", true);
            SetRouter("fireRay", new Ray(root.transform.position, Vector3.forward));
            var effects = root.AddComponent<ThermalBeamEffects>();
            var shooter = root.AddComponent<PalmBeamShooter>();
            typeof(PalmBeamShooter).GetMethod("Awake", flags).Invoke(shooter, null);
            void SetShooter(string name, object value) => typeof(PalmBeamShooter).GetField(name, flags).SetValue(shooter, value);
            SetShooter("palmOrigin", root.transform);
            SetShooter("showAimGuideWhileCharging", true);
            SetShooter("showAimGuide", true);
            cover.transform.position = root.transform.position + Vector3.forward * 3f;
            cover.layer = CombatLayers.GameplayCoverLayer;
            Physics.SyncTransforms();
            shooter.FireBeam();
            var line = (LineRenderer)typeof(PalmBeamShooter).GetField("aimGuideLine", flags).GetValue(shooter);
            if (!line.enabled || Mathf.Abs(line.GetPosition(1).z - (root.transform.position.z + 2.5f)) > 0.001f)
                throw new Exception("First-frame dotted beam preview must end at the actual cover raycast.");
            float burst = (float)typeof(PalmBeamShooter).GetField("beamBurstStartTime", flags).GetValue(shooter);
            if (burst >= 0f) throw new Exception("Unconfirmed pose must not fire the beam.");
            Vector3 head = new Vector3(0f, 1.7f, 0f), wrist = new Vector3(0.3f, 1.1f, 0.35f);
            Vector3 elbow = HandPoseRouter.EstimateElbow(head, Quaternion.identity, wrist);
            if (Mathf.Abs(Vector3.Distance(elbow, wrist) - 0.25f) > 0.001f)
                throw new Exception("Arm estimate does not preserve forearm length in reachable workspace.");
            Quaternion yaw = Quaternion.Euler(0f, 100f, 0f);
            Vector3 translated = new Vector3(2f, 0f, -3f);
            Vector3 rotatedElbow = HandPoseRouter.EstimateElbow(yaw * head + translated, yaw, yaw * wrist + translated);
            if (Vector3.Distance(rotatedElbow, yaw * elbow + translated) > 0.001f)
                throw new Exception("Arm estimate changes under arena yaw/translation.");
            Vector3 cuffAtLimit = HandPoseRouter.ForearmDirection(head, Quaternion.identity,
                new Vector3(0.1f, 1.1f, 0.35f)).normalized;
            Vector3 cuffAcrossChest = HandPoseRouter.ForearmDirection(head, Quaternion.identity,
                new Vector3(-0.2f, 1.1f, 0.35f)).normalized;
            if (Vector3.Dot(cuffAtLimit, cuffAcrossChest) < 0.999f)
                throw new Exception("Forearm cuff swivels further inward after the wrist crosses the chest.");
            Vector3 outwardWrist = new Vector3(0.3f, 1.1f, 0.35f);
            Vector3 normalCuff = HandPoseRouter.ForearmDirection(head, Quaternion.identity, outwardWrist);
            if (Vector3.Distance(normalCuff, outwardWrist - elbow) > 0.001f)
                throw new Exception("Forearm cuff changed the normal right-hand pose.");
            Quaternion arm = Quaternion.LookRotation(normalCuff, Vector3.up);
            ForearmShieldController.GetForearmMount(outwardWrist, arm, 0.125f, 0.075f,
                out Vector3 shieldCentre, out Quaternion shieldRotation);
            if (Vector3.Distance(shieldCentre, Vector3.Lerp(elbow, outwardWrist, 0.5f) + arm * Vector3.up * 0.075f) > 0.001f
                || Mathf.Abs(Vector3.Dot(shieldRotation * Vector3.forward, normalCuff.normalized)) > 0.001f
                || Vector3.Dot(shieldRotation * Vector3.up, normalCuff.normalized) < 0.999f)
                throw new Exception("Shield must lie along the back of the forearm, centred over the arm-effect cuff.");
            Mesh shield = (Mesh)typeof(ForearmShieldEffects).Assembly.GetType("CombatVfxStyle")
                .GetMethod("CreateHexField", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { 0.31f });
            foreach (Color color in shield.colors)
                if (color.a < 0.19f) throw new Exception("Shield fill is too transparent.");
            UnityEngine.Object.DestroyImmediate(shield);
            Debug.Log("FEEDBACK PASSED: immediate pose preview, charge raycast hits cover without firing, arm estimate follows calibrated frame, shield along the forearm, visible shield fill.");
        }
        finally { UnityEngine.Object.DestroyImmediate(root); UnityEngine.Object.DestroyImmediate(cover); }
    }

    public static void RunOutputCheck()
    {
        var root = new GameObject("Output regression");
        root.SetActive(false);
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var staticFlags = BindingFlags.Static | BindingFlags.NonPublic;
        using var receiver = new System.Net.Sockets.UdpClient(new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 0));
        receiver.Client.ReceiveTimeout = 1000;
        var sender = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
            System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
        try
        {
            var output = root.AddComponent<CombatEventOutput>();
            typeof(CombatEventOutput).GetField("instance", staticFlags).SetValue(null, output);
            typeof(CombatEventOutput).GetField("socket", flags).SetValue(output, sender);
            typeof(CombatEventOutput).GetField("destination", flags).SetValue(output, receiver.Client.LocalEndPoint);
            var message = (CombatEventOutput.Message)typeof(CombatEventOutput).GetField("message", flags).GetValue(output);
            message.session = "fixture";
            message.device = "fixture-quest";
            CombatEventOutput.Message Read()
            {
                System.Net.IPEndPoint from = null;
                return JsonUtility.FromJson<CombatEventOutput.Message>(System.Text.Encoding.UTF8.GetString(receiver.Receive(ref from)));
            }
            CombatEventOutput.State("shield", true);
            var first = Read();
            CombatEventOutput.State("shield", true); // Duplicate state must not produce another event.
            CombatEventOutput.Emit("shield_block", "ice", 20, 300);
            var block = Read();
            if (first.@event != "shield_start" || block.@event != "shield_block" || block.blocks != 1
                || block.health != 300 || block.seq != first.seq + 1 || block.active.Length != 1)
                throw new Exception("UDP state/event sequencing or block accounting failed.");
            CombatEventOutput.State("shield", false);
            var stopped = Read();
            if (stopped.active.Length != 0 || stopped.@event != "shield_stop")
                throw new Exception("Shield stop did not clear output state.");
            CombatEventOutput.Emit("hit_received", "hazard", 12, 288);
            var hit = Read();
            if (hit.source != "hazard" || hit.hits != 1 || hit.health != 288)
                throw new Exception("Damage source/health missing from output.");
            Debug.Log("OUTPUT PASSED: real UDP loopback, ordered state transitions, duplicate suppression, shield blocks and hazard damage payloads.");
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(root);
            typeof(CombatEventOutput).GetField("instance", staticFlags).SetValue(null, null);
            sender.Dispose();
        }
    }

    public static void RunInteractionChecks()
    {
        var active = HandPoseRouter.PoseKind.Neutral;
        var pending = active;
        float since = 0f, departure = -1f;
        void Step(HandPoseRouter.PoseKind observed, float time, HandPoseRouter.PoseKind expected)
        {
            HandPoseRouter.AdvancePose(observed, time, 0.1f, ref active, ref pending, ref since, ref departure);
            if (active != expected) throw new Exception($"Pose transition at {time}: expected {expected}, got {active}.");
        }
        Step(HandPoseRouter.PoseKind.Shield, 0f, HandPoseRouter.PoseKind.Neutral);
        Step(HandPoseRouter.PoseKind.Fire, 0.08f, HandPoseRouter.PoseKind.Neutral);
        Step(HandPoseRouter.PoseKind.Ice, 0.16f, HandPoseRouter.PoseKind.Neutral);
        Step(HandPoseRouter.PoseKind.Fire, 0.30f, HandPoseRouter.PoseKind.Neutral);
        Step(HandPoseRouter.PoseKind.Fire, 0.55f, HandPoseRouter.PoseKind.Neutral);
        Step(HandPoseRouter.PoseKind.Fire, 0.60f, HandPoseRouter.PoseKind.Fire);
        Step(HandPoseRouter.PoseKind.Neutral, 0.62f, HandPoseRouter.PoseKind.Fire);
        Step(HandPoseRouter.PoseKind.Fire, 0.65f, HandPoseRouter.PoseKind.Fire);
        Step(HandPoseRouter.PoseKind.Shield, 0.74f, HandPoseRouter.PoseKind.Fire);
        Step(HandPoseRouter.PoseKind.Shield, 0.85f, HandPoseRouter.PoseKind.Neutral);
        Step(HandPoseRouter.PoseKind.Ice, 0.88f, HandPoseRouter.PoseKind.Neutral);
        Step(HandPoseRouter.PoseKind.Ice, 1.17f, HandPoseRouter.PoseKind.Ice);
        Step(HandPoseRouter.PoseKind.Neutral, 1.19f, HandPoseRouter.PoseKind.Ice);
        Step(HandPoseRouter.PoseKind.Neutral, 1.30f, HandPoseRouter.PoseKind.Neutral);

        active = pending = HandPoseRouter.PoseKind.Neutral;
        since = 0f; departure = -1f;
        Step(HandPoseRouter.PoseKind.Shield, 2f, HandPoseRouter.PoseKind.Neutral);
        Step(HandPoseRouter.PoseKind.Shield, 2.12f, HandPoseRouter.PoseKind.Neutral);
        Step(HandPoseRouter.PoseKind.Shield, 2.19f, HandPoseRouter.PoseKind.Shield);

        Vector3 origin = new Vector3(0.3f, 1f, 0.6f);
        foreach (float yaw in new[] { 0f, 90f, 180f, 270f })
        {
            Vector3 heading = Quaternion.Euler(0f, yaw, 0f) * Vector3.forward;
            Vector3 target = IceGrenadeLauncher.ConstrainForwardTarget(origin, origin - heading * 0.5f,
                heading, 0.75f, 5f, 0f);
            if (Vector3.Dot(target - origin, heading) < 0.74f || Mathf.Abs(target.y) > 0.001f)
                throw new Exception("Nearby gaze hit reversed ice launch direction.");
        }
        var root = new GameObject("Audio parent regression");
        try
        {
            root.transform.position = new Vector3(1f, 2f, 3f);
            var source = CombatAudioVoice.Create(root.transform, "Effect audio", null, false);
            source.transform.position = new Vector3(-3f, 1f, 4f);
            if (root.transform.position != new Vector3(1f, 2f, 3f))
                throw new Exception("Positioning an effect sound moved its combat rig.");
        }
        finally { UnityEngine.Object.DestroyImmediate(root); }
        Debug.Log("INTERACTION CHECKS PASSED: transient gestures rejected, stable poses accepted, prompt release, forward ice targets, isolated audio emitters.");
    }

    public static void RunTrajectoryChecks()
    {
        var rangeRoot = new GameObject("Hand range regression");
        rangeRoot.SetActive(false);
        try
        {
            var launcher = rangeRoot.AddComponent<IceGrenadeLauncher>();
            var head = new GameObject("Head").transform;
            var hand = new GameObject("Hand").transform;
            head.SetParent(rangeRoot.transform);
            hand.SetParent(rangeRoot.transform);
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            void Set(string name, object value) => typeof(IceGrenadeLauncher).GetField(name, flags).SetValue(launcher, value);
            Set("headset", head);
            Set("palmOrigin", hand);
            Set("lowHandHeightOffsetFromHeadset", -0.55f);
            Set("highHandHeightOffsetFromHeadset", -0.20f);
            Set("throwHeightExponent", 1.6f);
            Set("minThrowDistance", 0.50f);
            Set("maxThrowDistance", 5f);
            var distanceMethod = typeof(IceGrenadeLauncher).GetMethod("GetThrowDistance", flags);
            foreach (float headHeight in new[] { 1.4f, 1.8f })
            {
                head.position = Vector3.up * headHeight;
                float previous = 0f;
                foreach (float offset in new[] { -0.65f, -0.55f, -0.50f, -0.40f, -0.30f, -0.20f })
                {
                    hand.position = head.position + Vector3.up * offset;
                    float distance = (float)distanceMethod.Invoke(launcher, null);
                    if (distance < previous || (offset == -0.55f && Mathf.Abs(distance - 0.50f) > 0.001f)
                        || (offset == -0.40f && distance > 1.8f)
                        || (offset == -0.20f && Mathf.Abs(distance - 5f) > 0.001f))
                        throw new Exception("Hand height must cover the full grenade range below eye level.");
                    previous = distance;
                }
            }
        }
        finally { UnityEngine.Object.DestroyImmediate(rangeRoot); }
        foreach (var test in new[] {
            (Quaternion.identity, 0f),
            (Quaternion.Euler(0f, 0f, -1f), 0f),
            (Quaternion.Euler(0f, 0f, -10f), 12.8f),
            (Quaternion.Euler(0f, 0f, 10f), -12.8f),
            (Quaternion.Euler(0f, 10f, 0f), 12.8f),
            (Quaternion.Euler(12f, 0f, 0f), 0f),
            (Quaternion.Euler(0f, 0f, -60f), 35f)
        })
        {
            float angle = IceGrenadeLauncher.WristSteeringAngle(Quaternion.identity, test.Item1);
            if (Mathf.Abs(angle - test.Item2) > 0.02f)
                throw new Exception($"Wrist steering expected {test.Item2}, got {angle}.");
        }
        Debug.Log("ICE STEERING PASSED: short low casts, full raised range, left/right wrist steering, dead band and bounded deflection.");
        var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.layer = 30;
        wall.transform.position = new Vector3(1000f, 2f, 1001.5f);
        wall.transform.localScale = new Vector3(2f, 4f, 0.2f);
        var projectileObject = new GameObject("Flight regression");
        var projectile = projectileObject.AddComponent<IceGrenadeProjectile>();
        var points = new Vector3[512];
        Vector3 origin = new Vector3(1000f, 1.2f, 1000f);
        Vector3 velocity = new Vector3(0f, 2f, 5f);
        try
        {
            foreach (bool withCover in new[] { true, false })
            {
                wall.SetActive(withCover);
                Physics.SyncTransforms();
                int count = IceGrenadeTrajectory.Predict(origin, velocity, Physics.gravity, 0f,
                    projectile.CollisionRadius, projectile.MinimumFlightTime, projectile.MaximumFlightTime,
                    1 << 30, points, out var predicted);
                if (count < 2 || (withCover && predicted.Collider != wall.GetComponent<Collider>())
                    || (!withCover && (predicted.Collider != null || Mathf.Abs(predicted.Point.y) > 0.001f)))
                    throw new Exception("Trajectory preview did not stop at the first cover/floor contact.");
                foreach (int rate in new[] { 36, 72, 90 })
                {
                    bool hit = false;
                    Vector3 actual = Vector3.zero;
                    Collider actualCollider = null;
                    projectile.Launch(origin, velocity, 1 << 30, null,
                        (point, collider) => { hit = true; actual = point; actualCollider = collider; }, 1f, 0f);
                    var advance = typeof(IceGrenadeProjectile).GetMethod("AdvanceTo", BindingFlags.Instance | BindingFlags.NonPublic);
                    for (int frame = 1; frame <= rate * 5 && !hit; frame++)
                        advance.Invoke(projectile, new object[] { frame / (float)rate });
                    if (!hit || Vector3.Distance(actual, predicted.Point) > 0.001f || actualCollider != predicted.Collider)
                        throw new Exception($"Preview and live projectile differ at {rate} fps.");
                }
            }
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(projectileObject);
            UnityEngine.Object.DestroyImmediate(wall);
        }
        Debug.Log("TRAJECTORY CHECKS PASSED: preview equals live grenade for cover/floor impacts at 36, 72 and 90 fps.");

        foreach (float distance in new[] { 0.5f, 2f, 5f })
        foreach (float handHeight in new[] { 0.9f, 1.6f })
        {
            // Arena-scale coordinates; an empty mask keeps scene colliders out of the flight.
            Vector3 start = new Vector3(0.3f, handHeight, 0.6f);
            Vector3 aim = start + Quaternion.Euler(0f, distance * 40f, 0f) * Vector3.forward * distance;
            aim.y = 0f;
            Vector3 launch = IceGrenadeLauncher.SolveTimedLaunchVelocity(start, aim, Physics.gravity, 1f);
            int steps = IceGrenadeTrajectory.Predict(start, launch, Physics.gravity, 0f, 0.1f, 0.08f, 4f,
                0, points, out var landing) - 1;
            if (Vector3.Distance(landing.Point, aim) > 0.001f
                || Mathf.Abs(steps * IceGrenadeTrajectory.StepSeconds - 1f) > IceGrenadeTrajectory.StepSeconds + 0.0001f)
                throw new Exception($"Ice bomb aimed {distance} m away from {handHeight} m did not land on target after one second.");
        }
        var cooldownRoot = new GameObject("Ice cooldown regression");
        cooldownRoot.SetActive(false);
        GameObject thrown = null;
        try
        {
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var type = typeof(IceGrenadeLauncher);
            var launcher = cooldownRoot.AddComponent<IceGrenadeLauncher>();
            type.GetMethod("ThrowGrenade", flags).Invoke(launcher, new object[] { cooldownRoot.transform.position, Vector3.up });
            thrown = ((Component)type.GetField("activeProjectile", flags).GetValue(launcher)).gameObject;
            if ((float)type.GetField("nextThrowAllowedTime", flags).GetValue(launcher) - Time.time < 2.999f)
                throw new Exception("An ice throw must start a 3-second cooldown.");
            foreach (var placed in UnityEngine.Object.FindObjectsByType<IceGrenadeLauncher>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                if ((float)type.GetField("throwCooldownSeconds", flags).GetValue(placed) < 3f)
                    throw new Exception($"{placed.name} ice cooldown is shorter than 3 seconds.");
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(thrown);
            UnityEngine.Object.DestroyImmediate(cooldownRoot);
        }
        Debug.Log("ICE TIMING PASSED: 0.5-5 m throws land on target after one second; each throw starts a 3-second cooldown.");
    }

    public static void CaptureHud()
    {
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null) return;
        UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene);
        var originalPipeline = UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline;
        var originalQuality = QualitySettings.renderPipeline;
        RenderTexture target = null;
        Texture2D image = null;
        try
        {
            UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline = null;
            QualitySettings.renderPipeline = null;
            var camera = new GameObject("HUD preview camera").AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.fieldOfView = 64f;
            camera.nearClipPlane = 0.05f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.045f, 0.065f, 0.085f);
            var hud = new GameObject("HUD preview").AddComponent<FusionRoundHud>();
            hud.Preview(camera);
            foreach (var text in UnityEngine.Object.FindObjectsByType<TMPro.TextMeshProUGUI>(FindObjectsSortMode.None))
            {
                text.ForceMeshUpdate(true);
                if (text.isTextOverflowing) throw new Exception("HUD text overflows: " + text.name);
            }
            Canvas.ForceUpdateCanvases();
            target = new RenderTexture(1600, 1000, 24);
            camera.targetTexture = target;
            camera.Render();
            RenderTexture.active = target;
            image = new Texture2D(1600, 1000, TextureFormat.RGB24, false);
            image.ReadPixels(new Rect(0, 0, 1600, 1000), 0, 0);
            image.Apply();
            System.IO.File.WriteAllBytes("/tmp/thermal-hud-preview.png", image.EncodeToPNG());
            Debug.Log("HUD PREVIEW PASSED: no text overflow; /tmp/thermal-hud-preview.png");
        }
        finally
        {
            RenderTexture.active = null;
            if (image != null) UnityEngine.Object.DestroyImmediate(image);
            if (target != null) UnityEngine.Object.DestroyImmediate(target);
            UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline = originalPipeline;
            QualitySettings.renderPipeline = originalQuality;
        }
    }

    public static void CaptureSandboxGuide()
    {
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null) return;
        UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene);
        var originalPipeline = UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline;
        var originalQuality = QualitySettings.renderPipeline;
        RenderTexture target = null;
        Texture2D image = null;
        try
        {
            UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline = null;
            QualitySettings.renderPipeline = null;
            var camera = new GameObject("Practice guide preview camera").AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.transform.position = new Vector3(0.275f, 1.75f, -1.25f);
            camera.transform.LookAt(new Vector3(0.275f, 2.12f, 0.71f));
            camera.fieldOfView = 64f;
            camera.nearClipPlane = 0.05f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.045f, 0.065f, 0.085f);
            var guide = new GameObject("Practice guide preview").AddComponent<SandboxGuideView>();
            guide.Preview(camera);
            foreach (var text in UnityEngine.Object.FindObjectsByType<TMPro.TextMeshProUGUI>(FindObjectsSortMode.None))
            {
                text.ForceMeshUpdate(true);
                if (text.isTextOverflowing) throw new Exception("Practice guide text overflows: " + text.name);
            }
            Canvas.ForceUpdateCanvases();
            target = new RenderTexture(1600, 1000, 24);
            camera.targetTexture = target;
            camera.Render();
            RenderTexture.active = target;
            image = new Texture2D(1600, 1000, TextureFormat.RGB24, false);
            image.ReadPixels(new Rect(0, 0, 1600, 1000), 0, 0);
            image.Apply();
            System.IO.File.WriteAllBytes("/tmp/thermal-sandbox-preview.png", image.EncodeToPNG());
            Debug.Log("SANDBOX GUIDE PREVIEW PASSED: no text overflow; /tmp/thermal-sandbox-preview.png");
        }
        finally
        {
            RenderTexture.active = null;
            if (image != null) UnityEngine.Object.DestroyImmediate(image);
            if (target != null) UnityEngine.Object.DestroyImmediate(target);
            UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline = originalPipeline;
            QualitySettings.renderPipeline = originalQuality;
        }
    }

    public static void InspectSafeZoneClearance()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
        Transform arena = GameObject.Find("ArenaRoot")?.transform;
        Transform cover = GameObject.Find("ArenaRoot/GameplayRoot/GameplayCover")?.transform;
        if (arena == null || cover == null) throw new Exception("Arena or starting cover missing.");
        Collider[] obstacles = cover.GetComponentsInChildren<Collider>(true);
        for (int sample = 0; sample < 20; sample++)
        {
            var heads = new[] { new Vector3(-1.5f + sample * 0.1f, 1.6f, -0.4f),
                new Vector3(1.8f - sample * 0.1f, 1.6f, 1.8f) };
            Physics.SyncTransforms();
            bool Clear(Vector2 point) => SafeZoneHazardView.HasClearance(
                arena.TransformPoint(new Vector3(point.x, 1.125f, point.y)), arena.rotation);
            if (!SafeZoneHazardView.SelectZones(heads, Clear, sample, out var a, out var b) || !Clear(a) || !Clear(b))
                throw new Exception("Procedural refuges failed starting-cover clearance sample " + sample);
        }
        Debug.Log($"SAFE ZONE CLEARANCE PASSED: procedural placement across 20 player pairs and {obstacles.Length} colliders.");
    }

    public static void RunBoneLookup()
    {
        var errors = new List<string>();
        foreach (bool xr in new[] { true, false })
        {
            GameObject root = new GameObject("Hand lookup regression");
            root.SetActive(false);
            try
            {
                var skeleton = root.AddComponent<OVRSkeleton>();
                typeof(OVRSkeleton).GetField("_skeletonType", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(skeleton, xr ? OVRSkeleton.SkeletonType.XRHandRight : OVRSkeleton.SkeletonType.HandRight);
                var bones = new List<OVRBone>();
                for (int i = 0; i < (xr ? 26 : 24); i++)
                {
                    var bone = new GameObject("Joint " + i).transform;
                    bone.SetParent(root.transform);
                    bones.Add(new OVRBone((OVRSkeleton.BoneId)i, 0, bone));
                }
                typeof(OVRSkeleton).GetProperty("Bones").SetValue(skeleton, bones);
                var router = root.AddComponent<HandPoseRouter>();
                typeof(HandPoseRouter).GetField("skeleton", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(router, skeleton);
                var lookup = typeof(HandPoseRouter).GetMethod("Bone", BindingFlags.Instance | BindingFlags.NonPublic);
                var pairs = new[] {
                    (OVRSkeleton.BoneId.XRHand_IndexProximal, OVRSkeleton.BoneId.Hand_Index1),
                    (OVRSkeleton.BoneId.XRHand_IndexTip, OVRSkeleton.BoneId.Hand_IndexTip),
                    (OVRSkeleton.BoneId.XRHand_MiddleProximal, OVRSkeleton.BoneId.Hand_Middle1),
                    (OVRSkeleton.BoneId.XRHand_MiddleTip, OVRSkeleton.BoneId.Hand_MiddleTip),
                    (OVRSkeleton.BoneId.XRHand_RingTip, OVRSkeleton.BoneId.Hand_RingTip),
                    (OVRSkeleton.BoneId.XRHand_LittleTip, OVRSkeleton.BoneId.Hand_PinkyTip)
                };
                foreach (var pair in pairs)
                {
                    var found = (Transform)lookup.Invoke(router, new object[] {pair.Item1, pair.Item2});
                    int expected = (int)(xr ? pair.Item1 : pair.Item2);
                    if (found != bones[expected].Transform)
                        errors.Add($"{(xr ? "OpenXR" : "Legacy")} expected joint {expected}, got {found?.name}");
                }
            }
            finally { UnityEngine.Object.DestroyImmediate(root); }
        }
        if (errors.Count > 0) throw new Exception("HAND LOOKUP FAILED: " + string.Join("; ", errors));
        Debug.Log("HAND LOOKUP PASSED: all finger joints map correctly in OpenXR and legacy skeletons.");
    }
}
