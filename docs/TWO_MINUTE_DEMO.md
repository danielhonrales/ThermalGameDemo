# One-minute Quest 3 duel

The current build uses Mirror over a private LAN. Both Quests install the same APK and join the same router Wi-Fi, but the router does not need Internet. They automatically choose one host and one client; no role selection is needed. See [LAN_MATCH.md](LAN_MATCH.md) for setup and validation. The Photon version remains a backup.

## Participant flow

1. Open the game on both Quest 3 headsets.
2. Stand one at a time at the same physical reference point, facing the same direction, and hold the **left middle-finger pinch** for calibration.
3. Once calibrated, players enter a shared **practice sandbox**. A three-card guide in the middle of the playfield turns toward each headset and shows the fire, ice, and shield poses with their effects. Both players can practice attacks and see hit/block effects without losing health. With one headset, practice works as a solo sandbox too.
4. Either player holds a **thumbs-up for three seconds** to start the synchronized five-second countdown. The duel lasts **1:00**. One calibrated player can start a solo round in the same way.
5. On the right hand, extend both index and middle fingers with the other fingers tucked for the **red fire beam**, hold an open palm for the **cyan ice bomb**, and make a relaxed fist for the **purple shield**. Fire continues while held after its short charge. Ice has a 3-second cooldown after each throw, then recharges visibly (cyan rings light up the forearm one after another, elbow to wrist) and throws again while the palm stays open. The shield lies along the back of the forearm, a purple aura pulses around the forearm, and both remain while the fist is held.
6. At 20 seconds, **DANGER** appears and two flashing yellow safe zones mark places to stand. Players have five seconds to move into either zone. At 25 seconds, one arena blast deals 90 damage to anyone outside both zones, even with a shield. The explosion and dense smoke clear over the next three seconds. Zones are 0.9 m squares selected procedurally when the warning starts: both are at least 0.7 m of movement from every player, clear of cover, with a refuge within 2.5 m. A heal pickup appears at 29 seconds.
7. The player with more health at the end wins, or the round ends early at zero HP. After the result, both players return to the sandbox and can start another round with a thumbs-up. Holding the left-hand two-finger gun for three seconds resets the shared match and returns to practice.

The first 20 seconds have half damage. Health is 300 HP; ordinary head hits are 20 damage before that reduction and temporary invulnerability. The safe-zone blast hits each exposed player once for 90 damage. At 35 seconds, an amber **SUDDEN DEATH** countdown announces the cover move. One drone fleet starts at 36 seconds. Drones cruise near 2.35 m with slower approach/departure and a fast cargo exchange, with visible grapple cables; suspended loads trail and swing. They drop varied machinery into a different layout, collect old cover, and leave before 40 seconds. Released cargo follows a short scripted drop to its authored pose and produces impact dust, then stays fixed. Shooting a drone releases its current load to its authored landing location. Cover has no rigidbodies; the host replicates delivery poses to the other headset. At 40 seconds the floor changes and the final 20-second sudden-death phase begins. Four marked fire patches deal 30 damage per accepted hit (including the sudden-death multiplier), ignoring shields. Your health bar and the round clock stay at eye level in the direction you face and do not move when you look up or down. The health bar changes from green through yellow to red; the opponent's red health bar appears above them. Any actual hit flashes the victim's view and HUD red.

## Build

`LanDemoSetup.BuildLanApk` applies scene/prefab settings through the Unity Editor API and builds `/tmp/ThermalGameDemo-2min-LAN.apk`. The last known working Photon APK remains at `/home/mason/Downloads/ThermalGameDemo-Quest-Photon-working.apk`.

For the router-only test, both headsets must join the same private router with client isolation disabled. Disconnect the router's WAN and verify discovery, calibration, combat, and the full round on both headsets.

Ice landing control: lower the casting hand for a nearby throw and raise it for a farther throw; head direction gives broad aim, and a small wrist tilt or turn during charging adjusts it left/right. The full 0.5–5 m range is available while the hand stays below eye level. The compact marker remains adjustable throughout charging, turns white on launch, and stays visible until detonation. Every bomb reaches its marker one second after release; launch speed scales with distance.

This revision uses LAN protocol 17. Install the same current APK on both headsets; a spectator PC must run the same project revision.

The normal arena includes reactor housings, cargo lockers, consoles, bulkheads, and small decorative deck equipment. A framed ablative shield on each side chars over a broad area at the beam impact point. About half a second of sustained fire opens the centre of a widening hole; beam shots then pass through it. Burn state is shared and resets for practice/new rounds. Wood remains in place during the drone exchange.

The safe-zone blast at 25 seconds includes arena-wide fireballs, mushroom-shaped plumes, and near-view smoke that briefly obscures vision, clearing by 28 seconds. It still applies only one 90-damage hit outside the refuges. There is no second large explosion at the sudden-death entrance.

Each safe zone stays fully in a different half of the field, offset from players, with reachability checked against the refuge on that player’s side. Twenty numbered asset samples surround the main arena; see [the sample list](ARENA_ASSET_SAMPLES.md). Scene and effect materials render both sides.

### Blast and cargo polish (r14)

Safe-zone countdown flashes red and plays a pulsing alarm during its five-second warning. The blast adds larger fireballs, flame fronts, and smoke columns; a quiet ringing sound fades out by the time smoke clears. Damage timing remains one 90 HP hit. Dropped cargo uses no rigidbodies and stays fixed after its scripted landing. The ceiling has been restored to its authored height; normal builds preserve its scene placement.

### Editable arena (r15)

See [Arena editing](ARENA_EDITING.md) for Scene-view editing of both layouts. Builds now preserve authored cover transforms. All cover rigidbodies are removed; deliveries use a short scripted landing and remain fixed. The headset smoke overlay stays neutral grey throughout the blast.
