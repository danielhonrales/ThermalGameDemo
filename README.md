# Thermal Game Demo

Two-player Quest 3 thermal combat demo. The current build uses Mirror on a local Wi-Fi router for the match and sends combat signals by UDP to a Raspberry Pi. The router can run without an Internet connection.

To run the demo without Unity, download the [latest install bundle](https://github.com/danielhonrales/ThermalGameDemo/releases/tag/quest-lan-ux-2026-09-24-r2) and follow the [APK install guide](docs/RUN_WITHOUT_UNITY.md). Both headsets use the same APK and choose host/client automatically. To rebuild or edit the game, use the [setup and deployment guide](docs/SETUP.md). See [external assets](docs/EXTERNAL_ASSETS.md) before opening the Unity project on a new computer.

The current single-Quest/Pi setup has been checked for APK installation, Mirror host startup, and Quest-to-Pi packet delivery. The four combat signals still need a live in-headset match test; confirmed hit and shield-block signals require an opponent or game hazard.
