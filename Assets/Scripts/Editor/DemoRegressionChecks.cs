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
        CaptureHud();
    }

    public static void RunLanElectionCheck()
    {
        if (!LanMatchManager.ShouldYieldTo("b", "a", 1)
            || LanMatchManager.ShouldYieldTo("a", "b", 1)
            || LanMatchManager.ShouldYieldTo("b", "a", 2))
            throw new Exception("LAN host collision must keep the smaller ID and preserve a full match.");
        Debug.Log("LAN ELECTION PASSED: host ID tie-break preserves a full match.");
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
            (straight, curled, curled, curled, false, HandPoseRouter.PoseKind.Fire),
            (curled, straight, curled, curled, false, HandPoseRouter.PoseKind.Fire),
            (straight, straight, curled, curled, true, HandPoseRouter.PoseKind.Fire),
            (straight, straight, straight, straight, false, HandPoseRouter.PoseKind.Ice),
            (straight, straight, -1f, straight, false, HandPoseRouter.PoseKind.Ice),
            (straight, curled, -1f, curled, false, HandPoseRouter.PoseKind.Fire),
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
        Debug.Log("POSE FIXTURES PASSED: index/middle gun, open palm, relaxed fist, occluded outer fingers, invalid tracking, and rotated hand.");
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
            var field = typeof(FusionRoundHud).GetField("root", BindingFlags.Instance | BindingFlags.NonPublic);
            var hudRoot = (RectTransform)field.GetValue(hud);
            Vector3 expectedLocal = camera.transform.InverseTransformPoint(hudRoot.position);
            // Calibration/network updates can stop while tracking keeps moving.
            camera.transform.SetPositionAndRotation(new Vector3(3f, 1.6f, -2f), Quaternion.Euler(10f, 85f, 0f));
            if (Vector3.Distance(hudRoot.position, camera.transform.TransformPoint(expectedLocal)) > 0.001f)
                throw new Exception("Local HP bar freezes in world space between round updates/calibration.");
            Debug.Log("HUD FOLLOW PASSED: health remains eye-relative without a network HUD refresh.");
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
                if (router.FeedbackPose != pose || router.Current != HandPoseRouter.PoseKind.Neutral)
                    throw new Exception("First-frame feedback must appear without prematurely confirming gameplay.");
            }
            SetRouter("candidate", HandPoseRouter.PoseKind.Fire);
            SetRouter("hasFireRay", true);
            SetRouter("fireRay", new Ray(Vector3.zero, Vector3.forward));
            var effects = root.AddComponent<ThermalBeamEffects>();
            var shooter = root.AddComponent<PalmBeamShooter>();
            typeof(PalmBeamShooter).GetMethod("Awake", flags).Invoke(shooter, null);
            void SetShooter(string name, object value) => typeof(PalmBeamShooter).GetField(name, flags).SetValue(shooter, value);
            SetShooter("palmOrigin", root.transform);
            SetShooter("showAimGuideWhileCharging", true);
            SetShooter("showAimGuide", true);
            cover.transform.position = Vector3.forward * 3f;
            cover.layer = CombatLayers.GameplayCoverLayer;
            Physics.SyncTransforms();
            shooter.FireBeam();
            var line = (LineRenderer)typeof(PalmBeamShooter).GetField("aimGuideLine", flags).GetValue(shooter);
            if (!line.enabled || Mathf.Abs(line.GetPosition(1).z - 2.5f) > 0.001f)
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
            Mesh shield = (Mesh)typeof(ForearmShieldEffects).Assembly.GetType("CombatVfxStyle")
                .GetMethod("CreateHexField", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { 0.31f });
            foreach (Color color in shield.colors)
                if (color.a < 0.19f) throw new Exception("Shield fill is too transparent.");
            UnityEngine.Object.DestroyImmediate(shield);
            Debug.Log("FEEDBACK PASSED: immediate pose preview, charge raycast hits cover without firing, arm estimate follows calibrated frame, visible shield fill.");
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
