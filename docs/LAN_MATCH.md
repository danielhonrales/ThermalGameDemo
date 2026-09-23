# Two-Quest LAN match

See [SETUP.md](SETUP.md) for the exact APK install and per-headset config commands.

The current Quest build uses Mirror with KCP over UDP. One Quest hosts the game on UDP port **7777**; the other looks for it using LAN discovery on UDP port **47777**. No Photon account, Internet, or Quest Link is used at runtime. Both headsets must join the same router Wi-Fi with client isolation disabled. The router can have no WAN connection.

Both headsets install the same APK. Each one reads `lan-match.json` from its app data directory on launch:

```text
/sdcard/Android/data/com.UnityTechnologies.com.unity.template.urpblank/files/lan-match.json
```

The repository includes `tools/pi/quest-a-lan-match.json` for the host and `tools/pi/quest-b-lan-match.json` for the client. Their contents are:

```json
{"role":"host","fallbackHost":"","port":7777}
```

```json
{"role":"client","fallbackHost":"","port":7777}
```

Push the host file to the first headset and the client file to the second, then restart the app on both. The client discovers the host automatically and retries if the host is started later or disconnects. If the router blocks broadcast, set `fallbackHost` on the client to the host Quest's reserved LAN IP, such as `192.168.8.100`; the discovery attempt runs first, then the client connects to that IP. Reserve the host IP on the router for repeatable demos.

```bash
adb -s HOST_SERIAL push tools/pi/quest-a-lan-match.json /sdcard/Android/data/com.UnityTechnologies.com.unity.template.urpblank/files/lan-match.json
adb -s CLIENT_SERIAL push tools/pi/quest-b-lan-match.json /sdcard/Android/data/com.UnityTechnologies.com.unity.template.urpblank/files/lan-match.json
```

The LAN connection shares player head pose, combat visuals, health, shield blocks, countdown, win state, and fire zones. It does not solve physical room alignment: stand at the same real-world reference point one at a time, face the same direction, and hold the left middle-finger pinch on each headset. Once both players have calibrated, the five-second countdown starts. Opening either app first is fine; the host must remain open during the duel.

For an offline acceptance test, disconnect the router's WAN, connect both Quests to its Wi-Fi, and verify that both players join, calibrate, see the same countdown, land beam and ice damage, block with shields, and see the same result after two minutes. Confirm the host logs `LAN: hosting`, the client logs `LAN: found host Quest`, and the host logs `2/2`. The old Photon launcher is inactive in the scene; its source and plugin remain in the repository as a backup.

The optional Raspberry Pi event output is separate from the match connection. It sends JSON UDP to the Pi's own LAN IP on port **7779** when enabled; see [RASPBERRY_PI_OUTPUT.md](RASPBERRY_PI_OUTPUT.md).
