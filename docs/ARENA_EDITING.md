# Sculpting the arena in Unity

Open `Assets/Scenes/Game.unity` and **stop Play Mode** before editing. Both layouts are stored in this scene; normal builds preserve your positions, rotations, scales, and prefab choices.

## Starting arena

1. Choose **Thermal → Arena → Edit Starting Arena**.
2. Expand `ArenaRoot → GameplayRoot → GameplayCover` in the Hierarchy.
3. Move, rotate, scale, duplicate, or replace the objects with Unity's W/E/R tools. Select an object and press F to frame it in Scene view.
4. Keep each movable cover assembly under one direct child named `Obstacle …` (or `Crate …`). Drones remove these at the transition. All other children, including the numbered samples and wood shields, stay in both arenas.
5. Keep colliders on cover so it blocks shots. Do not add Rigidbody components: the build/runtime removes them.

## Sudden-death arena

1. Choose **Thermal → Arena → Edit Sudden Death Arena**.
2. Edit the direct children of `ArenaRoot → GameplayRoot → SuddenDeathLayout`.
3. Every direct child is one drone delivery. Put all parts of an assembly underneath that child. Place it exactly where and how it should rest: height, tilt, scale and rotation are preserved.
4. Drag in different prefab assets, duplicate or delete pieces as needed. Existing options are in `Assets/Resources/ThermalArena` and the imported sci-fi/industrial asset folders. Keep 1–31 deliveries and no more than 31 starting drone loads.
5. Include a BoxCollider or MeshCollider for shot blocking. Put `Burning` in a delivery object's name if you want its small decorative flame after delivery.

The sudden-death preview hides starting drone loads and shows the new layout plus persistent dressing. It does not run drones, ground fire, or atmospheric effects. The game always starts with the starting layout, regardless of which preview you last saved. Avoid disabling individual children to remove them; delete or move them outside these layout roots instead.

## Save and build

- Use **Thermal → Arena → Sync Colliders To Meshes** after editing geometry. This replaces old parent boxes with non-convex mesh colliders attached to the visual children, preserving positions and mesh openings. The batch build runs it automatically, including additional `Deck details` groups directly under `GameplayRoot`. Burnable shields keep their special hole colliders.
- Save **Game.unity** with Ctrl+S.
- Use the normal LAN APK build (`LanDemoSetup.BuildLanApk`), or the existing `ThermalBatch.RebuildCheckAndBuild` automation. Despite its legacy name, that automation now preserves authored layouts.
- **Do not use Thermal → Build Default Arena Layout** after sculpting: that explicit reset command regenerates the starting obstacles and samples.
- Install the same APK on both headsets. Positions are authored relative to the shared arena and follow each headset's calibration.
- Keep clear space for a 0.9 × 0.9 m refuge in each half. The build checks safe-zone placement against the starting cover and fails if the layout blocks the tested placements.

Cover uses a short scripted landing, then remains fixed. It does not bounce, ricochet, or respond to players. A shot-down drone's load returns to its authored landing location.

Ground-fire locations are still defined by `SuddenDeathDirector.GroundFires`; moving a cover object does not move those hazard spots. The roof is under `ArenaRoot → Environment → VirtualCeiloingAlwaysVisible` and can also be positioned in Scene view.
