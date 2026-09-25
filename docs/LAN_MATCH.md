# Two-Quest LAN match

See [SETUP.md](SETUP.md) for the exact APK install and per-headset config commands.

The current Quest build uses Mirror with KCP over UDP. Both headsets install the **same APK** and automatically look for an existing host. If neither finds one after three seconds, a headset starts hosting on UDP port **7777**. The second joins via LAN discovery on UDP **47777** or the host beacon on UDP **47778**. If both start at the same time, the host beacons break the tie and one joins the other. No Photon account, Internet, Quest Link, or host/client selection is used at runtime. Both headsets must join the same router Wi-Fi with client isolation disabled. The router can have no WAN connection.

No `lan-match.json` file is needed for normal setup. An older installation may still have one: its old `role` field is ignored by this build. The optional file at the following path can set `fallbackHost` and `port` for troubleshooting:

```text
/sdcard/Android/data/com.UnityTechnologies.com.unity.template.urpblank/files/lan-match.json
```

Its default contents are:

```json
{"fallbackHost":"","port":7777}
```

The second headset discovers the host automatically and retries if it disconnects. If the router blocks broadcasts, automatic setup cannot find peers; enable peer broadcasts on the router. `fallbackHost` is an optional direct connection to a known Quest IP when a host is already running.

The LAN connection shares player head pose, combat visuals, health, shield blocks, countdown, win state, and laser timing. It does not solve physical room alignment: stand at the same real-world reference point one at a time, face the same direction, and hold the left middle-finger pinch on each headset. Once both players have calibrated, the five-second countdown starts. With one calibrated headset, hold a thumbs-up on either hand for three seconds while waiting to start solo. Opening either app first is fine; the host must remain open during the duel.

For an offline acceptance test, disconnect the router's WAN, connect both Quests to its Wi-Fi, and verify that both players join, calibrate, see the same countdown, land beam and ice damage, block with shields, and see the same result after one minute. Confirm one headset logs `LAN: hosting`, the other logs `LAN: found host Quest`, and the host logs `2/2`. The old Photon launcher is inactive in the scene; its source and plugin remain in the repository as a backup.

The optional Raspberry Pi event output is separate from the match connection. It sends JSON UDP to the Pi's own LAN IP on port **7779** when enabled; see [RASPBERRY_PI_OUTPUT.md](RASPBERRY_PI_OUTPUT.md).
