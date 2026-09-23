# Raspberry Pi combat signals

For the complete Pi service installation and per-Quest APK/config sequence, use [SETUP.md](SETUP.md).

The game records local combat events independently of the match transport. The optional UDP sender can reach a Pi through your own router without Internet access. The active multiplayer build now uses Mirror on the LAN.

No hardware is driven. `tools/pi/receiver.py` prints the four requested signal types: `ice_shot` for the ice grenade attack, `fire_start` / `fire_stop` for the beam, `hit_received` for a confirmed unshielded hit, and `shield_block` for a confirmed shielded hit. It also prints `output_timeout` if a headset stops sending. Other telemetry and snapshots are received but not printed.

## Run the dummy receiver

On the Pi, with Python 3:

```bash
python3 tools/pi/receiver.py --port 7779
```

For the Pi at `192.168.1.5`, install `receiver.py` at `/home/milabpi/thermal-game/receiver.py` and `tools/pi/thermal-game-receiver.service` at `/etc/systemd/system/thermal-game-receiver.service`, then run `sudo systemctl daemon-reload && sudo systemctl enable --now thermal-game-receiver`. Watch signals with `journalctl -u thermal-game-receiver -f`.

Both headsets may send to the same Pi. Each installation has a persistent random `device` ID; each app run has a new `session` ID, plus the current Mirror `player` network ID. Optional `deviceLabel` can name the installations `quest-a` and `quest-b`.

The game writes `combat-output.json` and `combat-events.jsonl` in:

```text
/sdcard/Android/data/com.UnityTechnologies.com.unity.template.urpblank/files/
```

New installations default to UDP enabled at the Pi's LAN IP, `192.168.1.5:7779`. For an existing installation, update its existing config file:

```json
{"udpEnabled":true,"host":"192.168.1.5","port":7779,"deviceLabel":"quest-a"}
```

Push the configuration to each headset and restart the app:

```bash
adb -s QUEST_SERIAL push tools/pi/quest-a-combat-output.json /sdcard/Android/data/com.UnityTechnologies.com.unity.template.urpblank/files/combat-output.json
```

For the current single-Quest setup, configure that Quest as the Mirror host with `tools/pi/quest-a-lan-match.json` (same app data directory, file name `lan-match.json`). When the second Quest is added, configure it as a client per [LAN_MATCH.md](LAN_MATCH.md). The Pi output does not depend on the second Quest being present, but receiving a hit or shield block does require another player or a game hazard.

Keep the Pi at `192.168.1.5` with a router DHCP reservation or static address. The router must allow traffic between Wi-Fi clients; guest/client isolation can prevent delivery. Allow UDP port 7779 on the Pi. No discovery or Internet service is needed for this output.

## Message contract, schema 1

Every datagram is one UTF-8 JSON object under 1400 bytes. Fields:

| Field | Meaning |
|---|---|
| `session`, `device`, `player` | App run, headset label/ID, Mirror player network ID (-1 before joining) |
| `seq`, `time` | Increasing sequence per session; headset monotonic seconds |
| `event`, `source`, `amount` | Event name, source/category, damage amount where relevant |
| `health` | Latest local health (-1 before player initialization) |
| `active` | Complete set of currently active states |
| `hits`, `blocks`, `fireBursts`, `iceThrows`, `deaths` | Cumulative event counters for this app run |

States emit `_start` / `_stop` transitions: `fire_charge`, `fire`, `ice_charge`, `ice_flight`, `shield`, `hazard_warning`, `hazard`, `dead`.

Discrete events:

- `session_start`, `session_pause`, `session_resume`, `session_stop`
- `player_ready`, `calibrated`, `round_phase`, `round_disconnected`
- `fire_shot`, `fire_contact` (miss/hit/blocked/shield/headshot), `ice_shot`, `ice_impact` (collider/floor)
- `hit_received` (fire/ice/hazard), `shield_block` (fire/ice), `death`, `health_reset`

Only the local player's confirmed damage/block events are emitted. Shield visibility alone does not emit a block. A sustained beam produces at most four block notifications per second. Actual damage still uses the existing health/invulnerability rules. `fire_contact` reports aiming contact while firing, not proof that health changed. `hit_received` is the authoritative health change on the defender.

A `snapshot` repeats full state and counters every 250 ms when UDP is enabled. Local JSONL logs record changes/events (not repeated snapshots), flush once per second, and rotate at 1 MiB, retaining one previous log. The config is read at launch.

## Delivery behavior

UDP is best effort: one-time events can be lost. The next snapshot repairs continuous state and cumulative counts; it does not replay the missing event's timing, source or amount. The receiver rejects duplicate/out-of-order sequence numbers and prints `output_timeout` after one second without a fresh packet for a session. Restarted apps have new session IDs.

This is appropriate for the current dummy integration and replaceable continuous feedback. Before implementing hardware actions that require guaranteed commands, define their acknowledgement/retry and shutdown behavior or use a reliable transport. Do not interpret a single received `shield_start` as an indefinite instruction: the full state and timeout are part of the contract.

References: [IETF UDP Usage Guidelines](https://www.rfc-editor.org/info/rfc8085/), [Python socket API](https://docs.python.org/3/library/socket.html).

## Checks

```bash
python3 -m unittest discover -s tools/pi -p 'test_*.py'
```

Unity `DemoRegressionChecks.RunOutputCheck` sends real loopback UDP packets through the game output class and verifies state transitions, duplicate suppression, block counters, source and health fields. On 2026-09-23 the built app on Quest A at `192.168.1.208` launched as host and sent packets to the Pi service at `192.168.1.5`; the service reported that app session's `output_timeout` after the app was stopped. Live gesture-generated signals and a two-Quest match remain to be verified.
