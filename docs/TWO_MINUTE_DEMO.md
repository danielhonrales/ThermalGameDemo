# One-minute Quest 3 duel

The current build uses Mirror over a private LAN. Both Quests install the same APK and join the same router Wi-Fi, but the router does not need Internet. They automatically choose one host and one client; no role selection is needed. See [LAN_MATCH.md](LAN_MATCH.md) for setup and validation. The Photon version remains a backup.

## Participant flow

1. Open the game on both Quest 3 headsets.
2. Stand one at a time at the same physical reference point, facing the same direction, and hold the **left middle-finger pinch** for calibration.
3. Once calibrated, players enter a shared **practice sandbox**. A three-card guide in the middle of the playfield turns toward each headset and shows the fire, ice, and shield poses with their effects. Both players can practice attacks and see hit/block effects without losing health. With one headset, practice works as a solo sandbox too.
4. Either player holds a **thumbs-up for three seconds** to start the synchronized five-second countdown. The duel lasts **1:00**. One calibrated player can start a solo round in the same way.
5. On the right hand, point the index or middle finger for the **red fire beam**, hold an open palm for the **cyan ice bomb**, and make a relaxed fist for the **purple shield**. Fire continues while held after its short charge. Ice recharges visibly and throws again while the palm stays open, with a brief gap after each throw. Shield remains active while the fist is held.
6. At 20 seconds, **DANGER** appears and two flashing yellow safe zones mark places to stand. Players have five seconds to move into either zone. From 25 to 28 seconds, the arena blast repeatedly damages anyone outside both zones, even with a shield. A heal pickup appears at 29 seconds.
7. The player with more health at the end wins, or the round ends early at zero HP. After the result, both players return to the sandbox and can start another round with a thumbs-up. Holding the left-hand finger gun for three seconds resets the shared match and returns to practice.

The first 20 seconds have half damage. Health is 300 HP; ordinary head hits are 20 damage before that reduction and temporary invulnerability. The safe-zone blast is 80 damage per accepted hit, with repeated checks during its three-second window. At 35 seconds, an amber **SUDDEN DEATH** countdown announces the cover move. Drones start at 36 seconds, and the new cover is stable before 40 seconds. At 40 seconds the floor changes and the final 20-second sudden-death phase begins. Your corner health bar changes from green through yellow to red; the opponent's red health bar appears above them. Any actual hit flashes the victim's view and HUD red.

## Build

`LanDemoSetup.BuildLanApk` applies scene/prefab settings through the Unity Editor API and builds `/tmp/ThermalGameDemo-2min-LAN.apk`. The last known working Photon APK remains at `/home/mason/Downloads/ThermalGameDemo-Quest-Photon-working.apk`.

For the router-only test, both headsets must join the same private router with client isolation disabled. Disconnect the router's WAN and verify discovery, calibration, combat, and the full round on both headsets.

Ice landing control: lower the casting hand for a nearby throw and raise it for a farther throw; head direction gives broad aim, and a small wrist tilt or turn during charging adjusts it left/right. The full 0.5–5 m range is available while the hand stays below eye level. The compact marker remains adjustable throughout charging, turns white on launch, and stays visible until detonation.
