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

The LAN connection shares player head pose, combat visuals, health, shield blocks, sandbox/round phases, countdown, and win state. It does not solve physical room alignment: stand at the same real-world reference point one at a time, face the same direction, and hold the left middle-finger pinch on each headset. Once both players have calibrated, they enter the practice sandbox. Either player holds a thumbs-up on either hand for three seconds to start the duel. With one calibrated headset, the same gesture starts solo. After each result, players return to practice. Opening either app first is fine; the host must remain open during the duel.

For an offline acceptance test, disconnect the router's WAN, connect both Quests to its Wi-Fi, and verify that both players join, calibrate, see the practice cards, and can start the same countdown with either player's thumbs-up. Land beam and ice hits, block with shields, and see the same result after one minute. Confirm both return to practice. One headset should log `LAN: hosting`, the other `LAN: found host Quest`, and the host `2/2`. The old Photon launcher is inactive in the scene; its source and plugin remain in the repository as a backup.

The optional Raspberry Pi event output is separate from the match connection. It sends JSON UDP to the Pi's own LAN IP on port **7779** when enabled; see [RASPBERRY_PI_OUTPUT.md](RASPBERRY_PI_OUTPUT.md).

## Spectator PC

A PC can watch the match as a third connection without playing. It never hosts, never spawns a player, and sends nothing to the Pi. The Quests must run a build with LAN protocol 17 or later, because older hosts accept only two connections.

1. Connect the PC to the same router as the Quests. The first time, allow the Unity Editor through Windows Firewall for private networks, or it cannot hear the host's discovery broadcasts.
2. In Unity, open `Assets/Scenes/Game.unity` and tick **Thermal Demo > Play As Spectator**. The setting is saved per PC; untick it to play normally again.
3. Open a second Game view (**Window > General > Game**). Set the first Game view's display to **Display 1** and the second to **Display 2**, then drag the second onto the other monitor. Use **Maximize On Play** or a 16:9 aspect for each.
4. Press Play. The PC connects when it finds the host Quest, before or after the players join.

Display 1 shows both players' head views side by side: Player 1 has the lower network ID, usually the host. Display 2 cycles through fixed arena cameras every 8 seconds. The Right arrow or Space key shows the next camera, the Left arrow the previous one, and P pauses the cycle; click the Display 2 Game view first so it has keyboard focus. Both screens show the round clock, phase, score, and each player's health.

The PC has no passthrough, so it shows only the virtual arena, effects, and a simple stand-in figure for each player. Player views are rebuilt from the head pose the headset shares 20 times per second and run about 0.1 s behind so motion stays smooth. They show what is in front of the player, not the real room or their hands.

Without authored cameras, the gallery places seven cameras around the cover layout. To frame your own, move the Scene view to a good angle and choose **Thermal Demo > Add Spectator Viewpoint From Scene View**. This adds a `SpectatorViewpoint` under `ArenaRoot/SpectatorViewpoints`; each one sets a label, field of view, and whether to hide the virtual ceiling. Once any exist, the gallery uses only those, in name order. Save the scene to keep them.

Instead of the Editor, use the Windows build: **Thermal Demo > Build Windows Spectator** writes `Builds/Windows/ThermalGameDemo.exe` from the same project and scenes as the Quest APK. A Windows build is always a spectator, so it needs no command-line argument; on first launch, allow it through Windows Firewall for private networks. It opens Display 2 automatically when a second monitor is connected. The build leaves XR off on the PC.

### Mirroring the real headset view

The spectator rebuilds each view from head pose. To show what a headset actually displays, mirror it with [scrcpy](https://github.com/Genymobile/scrcpy) (`winget install Genymobile.scrcpy`). This runs over the same router and is independent of the game. The PC connects to each headset by IP, so the headsets don't need the PC's address.

1. Join the Quest to the match router, plug it into the PC by USB and run `powershell -ExecutionPolicy Bypass -File tools\quest-mirror.ps1`. This switches the headset to network ADB, prints its IP and opens a window with its left-eye view. Unplug it and repeat for the other headset.
2. Network ADB stays on until the headset reboots. Later, reopen the windows without USB: `tools\quest-mirror.ps1 -Ip <quest1-ip>,<quest2-ip>`. Reserve each Quest's IP on the router (DHCP reservation) so it stays the same.

A headset that isn't being worn sleeps and sends no frames, and its window can stay blank after it wakes. Start the mirror while the headset is worn, or close the window and run the script again. The mirror includes passthrough. The headset's picture is pre-warped for its lenses and ringed in black, so the window shows a 16:9 crop of the flatter middle of the left eye; `-Zoom` sets how much of the eye's width it keeps (default 0.9, about the widest before the black ring shows at the edge; 1 for the whole warped eye). Full lens correction would need a video tool such as OBS with a lens-correction filter on the scrcpy window. Each mirror uses about 8 Mbit/s of Wi-Fi at 1280 px and 30 fps; lower it with `-MaxSize 960 -BitRate 4M` if the match lags.
