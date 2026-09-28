# Thermal Game Demo

Two-player Quest 3 thermal combat demo. The current build uses Mirror on a local Wi-Fi router for the match and sends combat signals by UDP to a Raspberry Pi. The router can run without an Internet connection.

To run the demo without Unity, download `ThermalGameDemo-LAN-auto-install-kit.zip` from the [latest release](https://github.com/danielhonrales/ThermalGameDemo/releases/latest) and follow the [APK install guide](docs/RUN_WITHOUT_UNITY.md). Both headsets use the same APK and choose host/client automatically. To rebuild or edit the game, use the [setup and deployment guide](docs/SETUP.md) and [arena editing guide](docs/ARENA_EDITING.md). See [external assets](docs/EXTERNAL_ASSETS.md) before opening the Unity project on a new computer.

The current Quest/Pi setup has been checked for APK installation, Mirror host startup, and live Pi receipt of fire, ice, and unshielded-hit events. A shield-block event, a complete two-Quest round on this exact build, and a WAN-disconnected round still need live verification. The Pi receiver reports events; it does not drive GPIO hardware.
