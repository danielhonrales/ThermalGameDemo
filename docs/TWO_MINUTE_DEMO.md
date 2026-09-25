# One-minute Quest 3 duel

The current build uses Mirror over a private LAN. Both Quests install the same APK and join the same router Wi-Fi, but the router does not need Internet. They automatically choose one host and one client; no role selection is needed. See [LAN_MATCH.md](LAN_MATCH.md) for setup and validation. The Photon version remains a backup.

## Participant flow

1. Open the game on both Quest 3 headsets.
2. Stand one at a time at the same physical reference point, facing the same direction, and hold the **left middle-finger pinch** for calibration.
3. When both players are present and calibrated, the synchronized five-second countdown starts. The round lasts **1:00**.
   With one headset, calibrate and hold a thumbs-up on either hand for three seconds while waiting to start a solo round. A second headset joining switches the next countdown back to a duel.
4. On the right hand, point the index or middle finger for the **red fire beam**, hold an open palm for the **cyan ice grenade**, and make a relaxed fist for the **purple shield**. Charge visuals acknowledge the shape immediately. Hold it steady to confirm an attack (about a quarter-second); a fist confirms shield protection in 0.12 seconds. The red beam visibly charges for 0.75 seconds before firing, with a dotted aiming guide. Moving between shapes should not fire another weapon. There is no weapon-switch gesture.
5. **WATCH OUT FOR LASERS** appears before the first sweep at 10 seconds. A second head-height beam arrives at 25 seconds on the perpendicular axis, then two more at 40 and 55 seconds. A low hum grows as a sweep approaches. Duck under every beam. A laser hit removes health even while shielding.
6. The player with more health at the end wins, or the round ends early at zero HP. The next round starts automatically after the result screen.

The first 20 seconds have half damage so first-time participants can learn the poses. Health is 300 HP; beam and laser hits are 20 damage before the early reduction and temporary invulnerability. The heal appears at 20 seconds. The red **ARENA CHANGE** countdown runs from 20 to 25 seconds. At 25 seconds, quieter pickup drones arrive together; faster delivery drones arrive about one second after the pickup grabs. The final 20 seconds are sudden death with a lighter warm tint. Your corner health bar changes from green through yellow to red; the opponent's red health bar appears above them.

## Build

`LanDemoSetup.BuildLanApk` applies scene/prefab settings through the Unity Editor API and builds `/tmp/ThermalGameDemo-2min-LAN.apk`. The last known working Photon APK remains at `/home/mason/Downloads/ThermalGameDemo-Quest-Photon-working.apk`.

For the router-only test, both headsets must join the same private router with client isolation disabled. Disconnect the router's WAN and verify discovery, calibration, combat, and the full round on both headsets.

Ice landing control: lower the casting hand for a nearby throw and raise it for a farther throw; head direction gives broad aim, and a small wrist tilt or turn during charging adjusts it left/right. The full 0.5–5 m range is available while the hand stays below eye level. The compact marker remains adjustable throughout charging, turns white on launch, and stays visible until detonation.
