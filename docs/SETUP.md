# Quest and Raspberry Pi setup

This guide describes the current router-only build. Both Quests install the **same APK** and select host/client automatically: the first one running on an empty LAN hosts after three seconds, and the other joins. Quest A sends its own events to the Pi at `192.168.1.5`; Quest B can send its events to a second Pi by setting that Pi's address in its output config. The two Pi outputs are separate from the Quest-to-Quest match. One Quest can start a solo practice round; a duel requires two Quests.

If you have the compiled APK install bundle and only want to run the demo, read [RUN_WITHOUT_UNITY.md](RUN_WITHOUT_UNITY.md) and skip section 2 below.

## 1. Prepare the router and devices

1. Flash Raspberry Pi OS to a Pi SD card with Raspberry Pi Imager. In Imager's advanced settings, set username `milabpi`, set a password, enable SSH, and configure the Pi's Wi-Fi or Ethernet for this router. Boot it and verify `ssh milabpi@192.168.1.5` works. If you choose a different username, edit `User=` and both `/home/milabpi/` paths in `tools/pi/thermal-game-receiver.service`, plus the install commands below. Use your own password; no credential is stored in this repo.
2. Put the Pi and each Quest on the same router LAN. Keep the router powered even when its WAN/Internet cable is disconnected. Disable guest-network/client/AP isolation so Wi-Fi devices can contact each other. First-time Quest activation, Developer Mode setup, and software downloads may require Internet; the installed demo does not.
3. Reserve `192.168.1.5` for the current Pi in the router's DHCP settings, or assign that address statically. If its address changes, update `combat-output.json` on the headset and restart the app. Check the actual Pi IP with `hostname -I` on the Pi.
4. Enable Developer Mode on each Quest, install Android platform tools (`adb`) on the computer, connect each headset by USB, put it on, and accept its USB debugging prompt. Run `adb devices`; every headset being configured must show `device`, not `unauthorized`. With two attached, use `adb -s SERIAL` for **every** headset command.
5. Allow local UDP traffic: Mirror match port `7777` on whichever Quest hosts, discovery port `47777` and host election port `47778` between Quests, and combat output port `7779` on each Pi. No Internet, Photon login, Quest Link, or router port forwarding is needed at runtime.

The Pi service listens on all Pi network interfaces. A firewall on the Pi, if enabled, must allow incoming UDP `7779` from the Quest LAN.

## 2. Clone and build the Unity project

Install Git LFS, Unity **6000.3.14f1**, the Android Build Support module with SDK/NDK and OpenJDK, and Android `adb`. Use a Unity account with a working Editor license. Then:

```bash
git clone https://github.com/danielhonrales/ThermalGameDemo.git
cd ThermalGameDemo
git lfs install
git lfs pull
```

Restore the licensed asset folders listed in [EXTERNAL_ASSETS.md](EXTERNAL_ASSETS.md) at their exact paths. They are intentionally absent from GitHub. Open the project in Unity once to import assets and resolve packages. `Assets/Scenes/Game.unity` is the build scene; `Assets/Scene.unity` is an extra working scene and is not in the APK build. From the project root, run the Editor in batch mode:

```bash
~/Unity/Hub/Editor/6000.3.14f1/Editor/Unity -batchmode -quit \
  -projectPath "$PWD" -executeMethod ThermalBatch.RebuildCheckAndBuild \
  -logFile /tmp/thermal-lan-build.log
```

Adjust the Editor path if Unity Hub installed it elsewhere. If this computer has the `unity` CLI helper, the equivalent command is:

```bash
unity run "$PWD" --editor-version 6000.3.14f1 -- \
  -executeMethod ThermalBatch.RebuildCheckAndBuild -logFile /tmp/thermal-lan-build.log
```

The method syncs colliders to the authored arena layouts, runs project regression checks, configures the Mirror scene/prefab, and writes `/tmp/ThermalGameDemo-2min-LAN.apk`. Confirm the log contains `LAN Quest APK built at /tmp/ThermalGameDemo-2min-LAN.apk` and that the APK exists. An APK is a build artifact; it is distributed in GitHub Releases rather than Git history. Keep the same APK for both Quests. To make the install bundle from a rebuilt APK, run `python3 tools/make_install_kit.py --apk /tmp/ThermalGameDemo-2min-LAN.apk --output /tmp/ThermalGameDemo-LAN-auto-install-kit.zip`.

## 3. Install the Pi receiver

From the project root, copy the Python receiver and systemd service to the current Pi. SSH and sudo will prompt for the Pi credentials; do not put passwords in scripts or Git.

```bash
scp tools/pi/receiver.py tools/pi/thermal-game-receiver.service milabpi@192.168.1.5:/home/milabpi/
ssh -t milabpi@192.168.1.5 'mkdir -p /home/milabpi/thermal-game && mv /home/milabpi/receiver.py /home/milabpi/thermal-game/receiver.py && sudo install -m 644 /home/milabpi/thermal-game-receiver.service /etc/systemd/system/thermal-game-receiver.service && sudo systemctl daemon-reload && sudo systemctl enable --now thermal-game-receiver && sudo systemctl restart thermal-game-receiver'
ssh milabpi@192.168.1.5 'systemctl is-active thermal-game-receiver && systemctl is-enabled thermal-game-receiver'
```

Both status lines should read `active` and `enabled`. The service starts after Pi reboots. To watch signals:

```bash
ssh -t milabpi@192.168.1.5 'journalctl -u thermal-game-receiver -f -o cat'
```

The receiver prints JSON signals only; it does **not** control GPIO, heat, motors, or other hardware. See [the packet contract](RASPBERRY_PI_OUTPUT.md) before attaching any hardware behavior.

## 4. Install and configure Quest A

The Android package ID is `com.UnityTechnologies.com.unity.template.urpblank`. The app reads optional JSON config files **at launch** from `/sdcard/Android/data/com.UnityTechnologies.com.unity.template.urpblank/files/`. APK updates with `install -r` retain these files; uninstalling the app removes them. No host/client role file is needed, including on older installations with an old `role` setting.

Replace `QUEST_A_SERIAL` with the value shown by `adb devices`. First install and launch the app once so Android creates its app data directory. Confirm that directory exists before pushing configs:

```bash
export QUEST_A=QUEST_A_SERIAL
export APP_FILES=/sdcard/Android/data/com.UnityTechnologies.com.unity.template.urpblank/files
export APK_PATH=${APK_PATH:-/tmp/ThermalGameDemo-2min-LAN.apk}
adb -s "$QUEST_A" install -r "$APK_PATH"
adb -s "$QUEST_A" shell am start -n com.UnityTechnologies.com.unity.template.urpblank/com.unity3d.player.UnityPlayerGameActivity
adb -s "$QUEST_A" shell ls "$APP_FILES"
```

If the `ls` command says the directory does not exist yet, wait for the first launch to finish and run it again. To give Quest A a readable Pi output label, push its output config:

```bash
adb -s "$QUEST_A" shell am force-stop com.UnityTechnologies.com.unity.template.urpblank
adb -s "$QUEST_A" push tools/pi/quest-a-combat-output.json "$APP_FILES/combat-output.json"
adb -s "$QUEST_A" shell am start -n com.UnityTechnologies.com.unity.template.urpblank/com.unity3d.player.UnityPlayerGameActivity
```

The Quest automatically hosts if it finds no other host. Its Pi output file sets device label `quest-a`. On launch, press **PLAYER 1** in the floating **Select Pi** menu to send to `192.168.1.4:7779`; restart the app to choose again. The menu's Pi IPs are set on the `Pi Selection Menu` object in `Game.unity`. Check startup with:

```bash
adb -s "$QUEST_A" logcat -d -s Unity:D | grep -E 'LAN: hosting|CombatOutput|Exception'
```

Expected lines include `LAN: hosting` and a `CombatOutput` `player_ready` event. After calibration, one or both players enter the practice sandbox. Either player holds a thumbs-up for three seconds to start a solo round or two-player duel.

## 5. Add Quest B later

Give Quest B the **same APK**. There is no host/client choice to make. If Quest A is already running, Quest B finds it and joins. If both start together, host beacons break the tie. Connect Quest B by USB, use its own serial in `adb -s`, install and launch once, then stop the app only if you need to push its own device label. On launch, press **PLAYER 2** in the **Select Pi** menu to send to the second Pi (`192.168.1.248`).

To label Quest B's output, push a `combat-output.json` with its own label (the menu choice overrides `host`):

```json
{"udpEnabled":true,"host":"192.168.1.5","port":7779,"deviceLabel":"quest-b"}
```

Run the same receiver service on the second Pi, substituting its user, address, and home directory in the install commands. If both headsets should temporarily use the current Pi, press the same button on both; the Pi receiver tracks their sessions separately.

```bash
export QUEST_B=QUEST_B_SERIAL
adb -s "$QUEST_B" install -r "$APK_PATH"
adb -s "$QUEST_B" shell am start -n com.UnityTechnologies.com.unity.template.urpblank/com.unity3d.player.UnityPlayerGameActivity
adb -s "$QUEST_B" shell ls "$APP_FILES"
adb -s "$QUEST_B" shell am force-stop com.UnityTechnologies.com.unity.template.urpblank
adb -s "$QUEST_B" push /path/to/quest-b-combat-output.json "$APP_FILES/combat-output.json"
adb -s "$QUEST_B" shell am start -n com.UnityTechnologies.com.unity.template.urpblank/com.unity3d.player.UnityPlayerGameActivity
```

Either headset can start first. The other searches by LAN discovery and retries. Automatic selection needs Wi-Fi peer broadcasts. If the router blocks broadcast and cannot be changed, start one headset first, reserve its address on the router, and put that address in the second headset's optional `lan-match.json` `fallbackHost` field before restarting it. See [LAN_MATCH.md](LAN_MATCH.md).

## 6. Run and verify the demo

Stand at the same real-world reference point one headset at a time, face the same direction, and hold the **left middle-finger pinch** to calibrate. Both calibrated players enter the practice sandbox; either holds a thumbs-up on either hand for three seconds to begin the synchronized countdown. One calibrated player can start a solo round the same way. The right hand uses a finger gun (the index finger, or index and middle, extended) for the fire beam (each burst lasts up to 3 seconds, then a 3-second cooldown follows), an open palm turned away from the body for the ice bomb, and a fist with the palm turned toward the chest for the shield. After a result, practice resumes and another thumbs-up starts the next round. See [the participant flow](TWO_MINUTE_DEMO.md).

Watch the Pi journal while playing. The receiver prints `ice_shot` when the bomb is thrown, `fire_start`/`fire_stop` as the beam turns on/off, `hit_received` for a confirmed unshielded hit, and `shield_block` for a confirmed block. Practice hits emit `hit_received` while health stays full. It may also print `output_timeout` when the app closes, pauses, or stops sending. The defender's Pi gets hit/block events; a shield pose alone is not a block.

For a full offline check, disconnect only the router's WAN, keep its LAN/Wi-Fi running, start both Quests, calibrate, practice, exercise all four signals, and finish a round. Current checks confirm APK launch, automatic host startup, Pi service startup, and live Quest-to-Pi fire, ice, and unshielded-hit events. A shield-block event, a complete two-Quest round on this exact build, and a WAN-disconnected round remain to be checked.

## Troubleshooting

| Symptom | Check |
|---|---|
| `adb devices` shows `unauthorized` | Put on that headset and accept the USB debugging prompt. |
| Quest says the app is not responding on its first launch just after an APK install | Close the app and launch it again. This occurred during the 2026-09-25 sideload; the next launch reached the LAN host and logged `player_ready`. |
| APK installs but config push fails | Launch the app once, then check the package-specific `files/` directory exists. |
| Second Quest does not join | Same Wi-Fi/subnet, no client isolation, UDP `7777`/`47777`/`47778`; set its `fallbackHost` if broadcast discovery fails. |
| Pi journal shows no attack signals | Check Pi service is `active`, Quest output JSON has `udpEnabled:true` and the Pi's current IP, restart the app after edits, then perform an attack. |
| No `hit_received` or `shield_block` | These are confirmed defender events. Sudden-death ground fire can cause `hit_received` in solo mode; `shield_block` requires an opponent. Firing at empty space and merely raising a shield do not count. |
| Build from a fresh clone has missing assets | Run `git lfs pull` and restore the excluded licensed folders in [EXTERNAL_ASSETS.md](EXTERNAL_ASSETS.md). |

The headset's local event history is `combat-events.jsonl` in its app data directory. Pull it with `adb -s "$QUEST_A" pull "$APP_FILES/combat-events.jsonl" .` for diagnosis.
