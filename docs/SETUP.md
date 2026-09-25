# Quest and Raspberry Pi setup

This guide describes the current router-only build. Both Quests install the **same APK** and select host/client automatically: the first one running on an empty LAN hosts after three seconds, and the other joins. Quest A sends its own events to the Pi at `192.168.1.5`; Quest B can send its events to a second Pi by setting that Pi's address in its output config. The two Pi outputs are separate from the Quest-to-Quest match.

If you have the compiled APK install bundle and only want to run the demo, read [RUN_WITHOUT_UNITY.md](RUN_WITHOUT_UNITY.md) and skip section 2 below.

## 1. Prepare the router and devices

1. Put the Pi and each Quest on the same router LAN. Keep the router powered even when its WAN/Internet cable is disconnected. Disable guest-network/client/AP isolation so Wi-Fi devices can contact each other.
2. Reserve `192.168.1.5` for the current Pi in the router's DHCP settings, or assign that address statically. If its address changes, update `combat-output.json` on the headset and restart the app.
3. Use Quest Developer Mode, connect each headset to the build computer by USB, put it on, and accept its USB debugging prompt. Run `adb devices`; every headset being configured must show `device`, not `unauthorized`. With two attached, use `adb -s SERIAL` for **every** headset command.
4. Allow local UDP traffic: Mirror match port `7777` on whichever Quest hosts, discovery port `47777` and host election port `47778` between Quests, and combat output port `7779` on each Pi. No Internet, Photon login, Quest Link, or router port forwarding is needed at runtime.

The Pi service listens on all Pi network interfaces. A firewall on the Pi, if enabled, must allow incoming UDP `7779` from the Quest LAN.

## 2. Clone and build the Unity project

Install Git LFS, Unity **6000.3.14f1**, the Android Build Support module with SDK/NDK and OpenJDK, and Android `adb`. Use a Unity account with a working Editor license. Then:

```bash
git clone https://github.com/danielhonrales/ThermalGameDemo.git
cd ThermalGameDemo
git lfs install
git lfs pull
```

Restore the licensed asset folders listed in [EXTERNAL_ASSETS.md](EXTERNAL_ASSETS.md) at their exact paths. They are intentionally absent from GitHub. Open the project in Unity once to import assets and resolve packages. Build from the project root with this computer's `unity` helper:

```bash
unity run "$PWD" --editor-version 6000.3.14f1 -- \
  -executeMethod LanDemoSetup.BuildLanApk -logFile /tmp/thermal-lan-build.log
```

If `unity` is unavailable, run the Editor binary directly, adjusting its installed path:

```bash
~/Unity/Hub/Editor/6000.3.14f1/Editor/Unity -batchmode -quit \
  -projectPath "$PWD" -executeMethod LanDemoSetup.BuildLanApk \
  -logFile /tmp/thermal-lan-build.log
```

The method runs project regression checks, configures the Mirror scene/prefab, and writes `/tmp/ThermalGameDemo-2min-LAN.apk`. Confirm the log contains `LAN Quest APK built at /tmp/ThermalGameDemo-2min-LAN.apk` and that the APK exists. An APK is a build artifact; it is not stored in Git. Keep the same APK for both Quests.

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
adb -s "$QUEST_A" install -r /tmp/ThermalGameDemo-2min-LAN.apk
adb -s "$QUEST_A" shell am start -n com.UnityTechnologies.com.unity.template.urpblank/com.unity3d.player.UnityPlayerGameActivity
adb -s "$QUEST_A" shell ls "$APP_FILES"
```

If the `ls` command says the directory does not exist yet, wait for the first launch to finish and run it again. To give Quest A a readable Pi output label, push its output config:

```bash
adb -s "$QUEST_A" shell am force-stop com.UnityTechnologies.com.unity.template.urpblank
adb -s "$QUEST_A" push tools/pi/quest-a-combat-output.json "$APP_FILES/combat-output.json"
adb -s "$QUEST_A" shell am start -n com.UnityTechnologies.com.unity.template.urpblank/com.unity3d.player.UnityPlayerGameActivity
```

The Quest automatically hosts if it finds no other host. Its Pi output file selects `192.168.1.5:7779` and device label `quest-a`. To change the Pi IP or label, edit the local JSON, push it again, and restart the app. Check startup with:

```bash
adb -s "$QUEST_A" logcat -d -s Unity:D | grep -E 'LAN: hosting|CombatOutput|Exception'
```

Expected lines include `LAN: hosting` and a `CombatOutput` `player_ready` event. With one headset, calibrate, then hold a thumbs-up on either hand for three seconds while waiting to start a solo round. A two-player duel starts automatically after Quest B joins and both players calibrate.

## 5. Add Quest B later

Give Quest B the **same APK**. There is no host/client choice to make. If Quest A is already running, Quest B finds it and joins. If both start together, host beacons break the tie. Connect Quest B by USB, use its own serial in `adb -s`, install and launch once, then stop the app only if you need to push its separate Pi output config.

For Quest B's Pi output, create a separate `combat-output.json` with its own label and the **second Pi's actual LAN IP**:

```json
{"udpEnabled":true,"host":"SECOND_PI_IP","port":7779,"deviceLabel":"quest-b"}
```

Save that as a local file, replace `SECOND_PI_IP` with a numeric address, and push it to Quest B as `combat-output.json`. Run the same receiver service on that Pi, substituting its user, address, and home directory in the install commands. If both headsets should temporarily use the current Pi, use `192.168.1.5` for both and keep different device labels; the Pi receiver tracks their sessions separately.

```bash
export QUEST_B=QUEST_B_SERIAL
adb -s "$QUEST_B" install -r /tmp/ThermalGameDemo-2min-LAN.apk
adb -s "$QUEST_B" shell am start -n com.UnityTechnologies.com.unity.template.urpblank/com.unity3d.player.UnityPlayerGameActivity
adb -s "$QUEST_B" shell ls "$APP_FILES"
adb -s "$QUEST_B" shell am force-stop com.UnityTechnologies.com.unity.template.urpblank
adb -s "$QUEST_B" push /path/to/quest-b-combat-output.json "$APP_FILES/combat-output.json"
adb -s "$QUEST_B" shell am start -n com.UnityTechnologies.com.unity.template.urpblank/com.unity3d.player.UnityPlayerGameActivity
```

Either headset can start first. The other searches by LAN discovery and retries. Automatic selection needs Wi-Fi peer broadcasts. If the router blocks broadcast and cannot be changed, start one headset first, reserve its address on the router, and put that address in the second headset's optional `lan-match.json` `fallbackHost` field before restarting it. See [LAN_MATCH.md](LAN_MATCH.md).

## 6. Run and verify the demo

Stand at the same real-world reference point one headset at a time, face the same direction, and hold the **left middle-finger pinch** to calibrate. After both players calibrate, the synchronized countdown starts. For a solo round, calibrate one headset and hold a thumbs-up on either hand for three seconds while waiting. The right hand uses a pointing index or middle finger for the fire beam, an open palm for the ice grenade, and a relaxed fist for the shield. See [the participant flow](TWO_MINUTE_DEMO.md).

Watch the Pi journal while playing. The receiver prints `ice_shot` when the bomb is thrown, `fire_start`/`fire_stop` as the beam turns on/off, `hit_received` when this headset's player actually loses health, and `shield_block` when this player's active shield actually blocks a hit. It may also print `output_timeout` when the app closes, pauses, or stops sending. The defender's Pi gets hit/block events; a shield pose alone is not a block.

For a full offline check, disconnect only the router's WAN, keep its LAN/Wi-Fi running, start both Quests, calibrate, exercise all four signals, and finish a round. The current single-Quest check has confirmed APK launch, automatic host startup, host beacon transmission, Pi service startup, Quest-to-Pi UDP delivery, and an app-session timeout on the Pi. A test beacon from a laptop also made the Quest yield, try to join, then recover as host when the beacon stopped. A live four-signal, two-Quest match and a WAN-disconnected round remain to be checked.

## Troubleshooting

| Symptom | Check |
|---|---|
| `adb devices` shows `unauthorized` | Put on that headset and accept the USB debugging prompt. |
| Quest says the app is not responding on its first launch just after an APK install | Close the app and launch it again. This occurred during the 2026-09-25 sideload; the next launch reached the LAN host and logged `player_ready`. |
| APK installs but config push fails | Launch the app once, then check the package-specific `files/` directory exists. |
| Second Quest does not join | Same Wi-Fi/subnet, no client isolation, UDP `7777`/`47777`/`47778`; set its `fallbackHost` if broadcast discovery fails. |
| Pi journal shows no attack signals | Check Pi service is `active`, Quest output JSON has `udpEnabled:true` and the Pi's current IP, restart the app after edits, then perform an attack. |
| No `hit_received` or `shield_block` | These are confirmed defender events. Lasers can cause `hit_received` in solo mode; `shield_block` requires an opponent. Firing at empty space and merely raising a shield do not count. |
| Build from a fresh clone has missing assets | Run `git lfs pull` and restore the excluded licensed folders in [EXTERNAL_ASSETS.md](EXTERNAL_ASSETS.md). |

The headset's local event history is `combat-events.jsonl` in its app data directory. Pull it with `adb -s "$QUEST_A" pull "$APP_FILES/combat-events.jsonl" .` for diagnosis.
