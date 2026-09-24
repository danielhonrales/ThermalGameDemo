# Quest 3 build and launch

For the complete Pi, router, and per-headset procedure, use [SETUP.md](SETUP.md). Install the **same APK** on both headsets. Host/client roles are selected automatically; each headset's optional `combat-output.json` sets its Pi destination.

Open this project with Unity **6000.3.14f1** and Android Build Support (SDK/NDK and OpenJDK). Restore the licensed folders in [EXTERNAL_ASSETS.md](EXTERNAL_ASSETS.md) before opening `Assets/Scenes/Game.unity`. Git LFS must be installed when cloning so binary assets are available.

The game is a standalone Android app. Quest Link is not required on Linux.

Build the LAN development APK from the repository root:

```bash
unity run "$PWD" --editor-version 6000.3.14f1 -- \
  -executeMethod LanDemoSetup.BuildLanApk -logFile /tmp/thermal-lan-build.log
```

The build method writes `/tmp/ThermalGameDemo-2min-LAN.apk`. With Quest Developer Mode enabled and USB debugging accepted inside the headset, install and launch it:

```bash
adb devices
adb install -r /tmp/ThermalGameDemo-2min-LAN.apk
adb shell am start -n \
  com.UnityTechnologies.com.unity.template.urpblank/com.unity3d.player.UnityPlayerGameActivity
```

Repeat the install on the second Quest 3. Both apps search for a host; the first to start on an empty LAN becomes host after three seconds, and the other joins. No role file is required. To send to different Pis, push each headset's output JSON and relaunch; see [SETUP.md](SETUP.md). The host assigns distinct Mirror player IDs. In the same physical room, use the existing shared-reference calibration to align player positions. A single-headset launch confirms local startup; two-player alignment, hit registration, health, and match start require both headsets together.

The combat effects use red for the heat beam, blue/cyan for the grenade and cold impact, and purple for the shield. Their geometry is generated in code, so the old imported particle prefabs are no longer used for these combat effects.
