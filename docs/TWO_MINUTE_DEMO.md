# Two-minute Quest 3 duel

The current build uses Mirror over a private LAN. Both Quests install the same APK and join the same router Wi-Fi, but the router does not need Internet. They automatically choose one host and one client; no role selection is needed. See [LAN_MATCH.md](LAN_MATCH.md) for setup and validation. The Photon version remains a backup.

## Participant flow

1. Open the game on both Quest 3 headsets.
2. Stand one at a time at the same physical reference point, facing the same direction, and hold the **left middle-finger pinch** for calibration.
3. When both players are present and calibrated, the synchronized five-second countdown starts. The round lasts **2:00**.
4. On the right hand, point the index or middle finger for the **red fire beam**, hold an open palm for the **cyan ice grenade**, and make a relaxed fist for the **purple shield**. Charge visuals acknowledge the shape immediately. Hold it steady to confirm an attack (about a quarter-second); a fist confirms shield protection in 0.12 seconds. The red beam visibly charges for 0.75 seconds before firing, with a dotted aiming guide. Moving between shapes should not fire another weapon. There is no weapon-switch gesture.
5. A red ring with a growing center and **MOVE** warning marks an upcoming fire strike. Its center locks when the warning starts. Leave the ring before it fills; the burning area remains for three seconds.
6. The player with more health at the end wins, or the round ends early at zero HP. The next round starts automatically after the result screen.

The first 20 seconds have half damage and no hazards so first-time participants can learn the poses. Health is 300 HP; beam hits are 20 damage before the early reduction and temporary invulnerability. Hazard waves begin after 20 seconds and accelerate in the final 20 seconds.

## Build

`LanDemoSetup.BuildLanApk` applies scene/prefab settings through the Unity Editor API and builds `/tmp/ThermalGameDemo-2min-LAN.apk`. The last known working Photon APK remains at `/home/mason/Downloads/ThermalGameDemo-Quest-Photon-working.apk`.

For the router-only test, both headsets must join the same private router with client isolation disabled. Disconnect the router's WAN and verify discovery, calibration, combat, and the full round on both headsets.

Ice landing control: lower the casting hand for a nearby throw and raise it for a farther throw; head direction gives broad aim, and a small wrist tilt or turn during charging adjusts it left/right. The full 0.5–5 m range is available while the hand stays below eye level. The compact marker remains adjustable throughout charging, turns white on launch, and stays visible until detonation.
