# Demo validation

For the current deployment procedure and latest verification limits, see [SETUP.md](SETUP.md). The notes below preserve earlier iteration results.

## 2026-09-23 Mirror LAN and Pi deployment

The Mirror/KCP LAN APK was built at `/tmp/ThermalGameDemo-2min-LAN.apk` and installed on Quest A. Unity's output regression passed and the Android app launched, logged `LAN: hosting` and `player_ready`, and used device label `quest-a`. The Pi receiver service is `active` and `enabled` at `192.168.1.5:7779`. A UDP packet sent from the Quest Wi-Fi address `192.168.1.208` appeared in the Pi journal. After the game app was stopped, the Pi reported an `output_timeout` for the actual app session, confirming its snapshot stream arrived. Python receiver tests passed. The Unity CLI reported a shutdown/licensing error after writing the APK, but the APK installed and launched successfully.

These checks establish transport and startup, not live gameplay. A two-Quest round, all four gesture/hit signals, and operation with the router WAN disconnected remain to be verified.

## Gesture regression

The failed pose build mixed legacy and OpenXR bone identifiers. These enum values overlap numerically, so accepting either value selected unrelated joints. An Editor regression that invokes the actual resolver reproduced incorrect joint selection in both formats. The corrected resolver chooses the skeleton format before looking up an ID.

`DemoRegressionChecks.RunAll` verifies both joint mappings, anatomically bent finger samples, one- and two-finger fire poses, open-palm ice, relaxed fist, missing outer-finger samples, invalid samples, and rotation invariance. It also renders `/tmp/thermal-hud-preview.png` and rejects overflowing HUD text. The Android build runs the mapping and pose checks before building.

Development APKs write `pose-trace.csv` in the app's persistent data directory at 10 Hz for up to ten minutes. This records tracking validity, skeleton format, four bend scores, candidate pose, and selected pose. It allows on-headset failures to be investigated without relying on a short logcat buffer. It is not an in-game diagnostic overlay.

## Runtime provider failure

The next headset capture contained 1,970 samples: 1,760 tracked-hand samples, but zero valid skeleton samples. Every row reported skeleton type `None`. Runtime `AddComponent<OVRSkeleton>()` left the default type unconfigured; SDK `Awake` could not bind a matching provider and `ShouldInitialize` rejected `None`.

`CombatHandSkeleton` now obtains the actual parent OVRHand provider's format before calling SDK Awake. Gesture routing, shield positioning, and left-hand alignment use this shared resolver. `RunSkeletonProvider` exercises the real provider binding with an unconfigured skeleton present; the build runs this check too. The rebuilt APK was installed on Quest 3 serial ending 04LX. During the next test, telemetry reported valid XRHandRight joints and all three poses (fire, ice, shield), including repeated exits from shield. The initial post-fix capture contained 231 valid tracked samples versus zero in the failed build. Visual effect correctness and usability still require recording/user review.

## Hand placement follow-up

The first provider fix recovered gesture classification, but the user reported the beam and shield attached incorrectly. The OVRHand component lives on a data-source object whose transform is not the tracked wrist. The runtime skeleton now applies the provider's RootPose through OVRCameraRig.trackingSpace, including hand scale. A regression invokes the actual transform method with independently positioned/rotated data-source and tracking-space objects.

Shield placement now uses the wrist and index/middle/pinky knuckles to derive the back-of-hand plane. Curled fingertip direction and the legacy extra 90-degree rotation no longer determine the skeletal mount. The follow-up APK was installed on 04LX. A device recording shows the beam originating at the visible pointing finger and the purple shield moving with the hand. Telemetry still recognizes all three poses. Subjective aim/rotation approval is pending; two-headset verification of this build remains outstanding.

## HUD

The clock and local health have separate world-space canvases, with no combined surrounding panel. Health has a subtle dark backing and green edge accent. It is a green segmented bar with HP BAR above its left edge. The opponent's health uses the same style above their head and is hidden by gameplay cover. Text uses the bundled TextMesh Pro SDF font resources.

## Stability and audio refinement

The user approved the individual visual effects and hand placement, then reported excessive gesture switching, occasional reversed ice direction, and lingering sounds. The accepted visuals are retained.

- Pose selection now needs 240 ms of consistent evidence, with a 100 ms release and separate entry/retention thresholds. Ambiguous outer-finger shapes remain neutral; one occluded outer finger is tolerated when the other provides evidence. A fully unknown outer pair cannot initiate a gun pose.
- Actual recorded deliberate gestures had median extension scores around 0.91/0.87 for the pointing fingers, 0.92/0.83 for open outer fingers, and 0/0 for a fist. Threshold changes preserve these clear poses while rejecting intermediate shapes.
- Gaze targets are smoothed and constrained ahead of the grenade's origin. Looking at nearby floor or the palm cannot reverse the launch. Heading remains defined when looking nearly straight down.
- Shield source clips were 6.984 and 9.720 seconds. Deployment/impact now use bounded 0.4/0.3-second voices, with no stacking and immediate stop when hidden. Beam contact is bounded to 0.3 seconds; ice impact to 1.3 seconds. Round and hazard cues are bounded too. Short releases fade over the final 100 ms.
- Beam/ice audio emitters now have their own child transforms. Positioning a sound no longer moves the effects rig.

`RunInteractionChecks` exercises actual pose-transition logic, reversed nearby gaze targets at four headings, and audio-emitter transform isolation. All checks passed before the Android build. The final refinement APK was installed and launched on Quest 3 ending 04LX. Live telemetry reports all three poses, with neutral gaps between selections. Sampled recording frames show the three effects and forward ice trajectories; no exceptions appeared in the captured Unity log. The user confirmed that both gesture switching and lingering audio improved; these settings are retained during the ice landing fix.

## Trustworthy ice landing preview

The user reported an unreliable, flickering landing indication. The old gaze ray alternated between arbitrary scene surfaces and a floor fallback, while its preview ignored collisions and the live grenade used sphere casts. The landing ring also pulsed and disappeared during flight.

Target selection now uses a continuous ground plane, a 6 cm dead band, damped movement, and a target lock for the final 16% of charging. Scene cover no longer changes the target-selection ray. Instead, preview and live flight share `IceGrenadeTrajectory` at 90 simulation steps per second, including identical sphere radius, first collision, and floor crossing.

The cyan path stops at the first predicted impact. A steady center/ring and blast-radius outline mark that contact, turn white when the aim locks, and remain visible until detonation. `RunTrajectoryChecks` drives the actual projectile against cover and floor at 36, 72, and 90 simulated render fps and verifies its impact agrees with the preview within 1 mm; all passed. Development builds record predicted/actual impact error in `ice-landing-trace.csv`. Moving colliders can naturally change the impact after launch.

The landing refinement was installed on 04LX. Seven recorded throws matched their predicted positions exactly at four decimal places, including cover contact. The user confirmed ice was no longer finicky, but requested a nearer default and more responsive movement. The follow-up maps a level gaze to 2.25 m, 25 degrees downward to 0.75 m, and 15 degrees upward to 5 m, with monotonic interpolation independent of participant height. Its damping/dead band were slightly reduced; the final distance adjustment passed the build checks and was installed/launched on Quest 3 ending 04LX; user confirmation is pending.

## Final hand-controlled ice range

The user found the pitch-based range insufficiently dynamic and requested the old hand-controlled movement. Range now comes from casting-hand height relative to the headset: 60 cm below eye level gives 0.75 m, and 20 cm below eye level gives 5 m, with a mild exponential response. Head heading continues to steer sideways. The full range is accessible over a 40 cm hand movement below the face.

The early target lock was removed so range remains adjustable throughout charging. Damping is lighter, with a 2.5 cm positional dead band. The oversized blast-radius preview outline was removed; a compact 32 cm diameter landing ring and center dot remain. The actual grenade blast radius/effects are unchanged. The marker turns white only when launched and remains through flight. Tests exercise actual GetThrowDistance at two participant heights and the full low/high hand range, alongside the existing preview/live collision checks. The hand-controlled build passed all pre-build checks, was installed/launched on Quest 3 ending 04LX, and produced no exceptions in the captured startup log. The latest test has not yet produced any new throws; user confirmation of range control remains pending.

## Final radius and close-range tuning

The user approved hand-controlled distance as “very good” and requested a moderately larger bottom radius and closer default shots. The landing ring grows from 0.16 m to 0.26 m radius. The actual ice blast/damage radius grows from 1.4 m to 1.65 m (about 18% more radius), with the dynamically created remote effects using the same default. The minimum throw distance is now 0.5 m; the low-hand threshold moves from 60 cm to 55 cm below the headset, biasing normal low casts closer. Maximum distance stays 5 m and maximum range still occurs 20 cm below eye level. Existing hand-range fixtures use the new bounds. All build checks passed. The parameter refinement was built, installed, and launched successfully on Quest 3 ending 04LX. The other headset still needs this matching APK.

## Wrist steering and progressive range

The user requested fine lateral steering without turning the whole head/arm, plus a lower default range. Ice now records the tracked wrist orientation at the start of each charge, then uses wrist yaw/rightward banking for fine steering. A 2-degree dead band ignores small tremors; 10 degrees of deliberate tilt produces about 12.8 degrees of aim change, capped at 35 degrees either way. The reference heading remains fixed during that charge so a head turn alone does not create an opposing wrist input. Missing wrist data retains the last steering value until normal pose release cancels charging.

Height-to-range now uses a 1.6 power curve rather than the previous early distance boost. At 40 cm below the headset, range is about 1.66 m rather than 2.79 m; the endpoints remain 0.5 m at 55 cm below the headset and 5 m at 20 cm below. Lateral steering preserves the selected radial distance. The accepted blast radius, marker, sound and pose timings are unchanged. Wrist direction/dead-band/cap and lower-default/full-range fixtures passed alongside the gesture and preview/live collision checks. The APK was installed and launched on Quest 3 ending 04LX, and copied to the durable Downloads APK. The same APK was subsequently installed and launched successfully on the second Quest 3 ending 0H55. Both headsets now have this build; in-headset wrist-steering feedback and current-build two-player verification remain pending.

## Verification boundary

Editor checks and the HUD preview pass. Real headset gesture reliability, both-player round progression, and the final appearance still require headset verification of the revised APK. The earlier user's successful two-player test predates these changes and does not validate the new round or pose implementation.


## Immediate feedback, charge aiming, HUD follow, and Pi dummy output

A new HUD regression reproduced the reported freeze before the fix: after one HUD refresh, moving/rotating the tracked eye without another round update left the local HP bar fixed in world space. The local timer, announcements and health now parent to the eye and refresh independently in LateUpdate. The same test now passes, including a translated/rotated camera.

Pose feedback now begins on the first detected candidate frame. Fire/ice gameplay still requires the stable 240 ms pose; shield protection confirms after 120 ms. Candidate-only visuals are silent and local. Beam and ice charge durations begin with their visible charge, removing the preceding blank wait. Firing still requires confirmation. A dotted red guide and facing reticle use the beam collision ray during charge; a miss draws a short guide without a false hit marker. Beam hit queries no longer apply damage during charging.

Shield surface vertex opacity increased from 1.5–7% to 20–42%, retaining purple coloring. Ice charge spirals now use tracked wrist position and a two-bone estimated arm direction, independent of the launch trajectory, with a shorter 22 cm cuff. OpenXR hand joints do not provide a measured elbow here, so exact forearm fit remains a device-feel check, not a claim of measured full-body IK.

Editor feedback fixtures pass: immediate visual pose feedback with unconfirmed gameplay, actual charging raycast endpoint at cover without a beam burst, arm estimate length and calibration-frame invariance, and shield fill opacity. Existing joint/gesture, audio, ice steering and live-projectile prediction checks also pass. The HUD preview has no text overflow.

CombatEventOutput adds local rotating JSONL event logs and optional nonblocking UDP. Its real loopback test passed for state transitions, duplicate suppression, shield block counter, and hazard source/health. Python receiver tests passed for two independent headsets, reordered/duplicate packets, malformed payloads, timeout and recovery. See RASPBERRY_PI_OUTPUT.md. Physical Pi/router delivery and the latest device visual/gesture changes remain pending.

The feedback/output APK built successfully, was copied to Downloads/ThermalGameDemo-Quest-2min-Photon.apk, and was installed/launched on Quest 3 ending 0H55. Startup log capture contained no exceptions. The device JSONL log recorded session_start, session_resume, player_ready and round_phase. Quest 04LX still needs this APK before current-build multiplayer testing because damage RPCs now carry the source category. Human visual/gesture/calibration feedback remains pending. The Python receiver also passed a real socket smoke test with two headset sessions and independent timeout clearing.

The user tested the feedback build and described it as "not bad", requesting a longer red charge and less transparent effects. Device events recorded fire bursts, ice throws/impacts, and shield start/stop transitions returning to idle. The recording shows the cuff following the forearm and the more visible purple shield; no manual calibration event was captured, so that specific in-headset follow check remains outstanding. The next refinement raises beam charge from 0.52 to 0.75 seconds, guide opacity to 90–95%, and shared combat VFX alpha by 18% (clamped, preserving zero-alpha fades).

The longer-charge/opacity refinement passed all APK pre-build checks and built successfully. Serialized Game scene values were verified at 0.75 s beam charge and 0.90/0.95 guide alpha. The latest APK was copied to Downloads, installed and launched on 0H55. Quest 04LX still needs the matching update.
