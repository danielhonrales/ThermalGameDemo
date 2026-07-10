# External Assets

The current scene expects these third-party art and VFX packages to be present locally, but the packages are not committed to this repository because they are large and/or redistributed under their original asset-store licenses.

Install or restore them into the exact paths below before opening `Assets/Scenes/Game.unity`:

| Local path | Asset/package |
| --- | --- |
| `Assets/Autarca/AlienShipsPack` | Autarca Alien Ships Pack |
| `Assets/GabrielAguiarProductions` | Gabriel Aguiar Productions magic orb / stylized beam VFX assets |
| `Assets/Hovl Studio/Magic effects pack` | Hovl Studio Magic Effects Pack |
| `Assets/Tomerinio/LEDLightBlocks` | Tomerinio LED Light Blocks |
| `Assets/UnityTechnologies/ParticlePack` | Unity Technologies Particle Pack |

Recommended restore flow:

1. Import the packages through Unity Package Manager, Unity Asset Store, or the team's shared asset archive.
2. Keep the imported folders at the paths listed above so existing prefab and scene GUID references continue to resolve.
3. Do not commit the imported package folders; `.gitignore` excludes them intentionally.

Small project-specific gameplay assets under `Assets/Resources/CustomAssets` are tracked with the project.
