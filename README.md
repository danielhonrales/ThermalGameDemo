# Thermal Game Demo

Two-player Quest 3 thermal combat demo. The current build uses Mirror on a local Wi-Fi router for the match and sends combat signals by UDP to a Raspberry Pi. The router can run without an Internet connection.

Start with the [setup and deployment guide](docs/SETUP.md). It covers rebuilding the APK, installing and configuring each headset, setting up the Pi receiver, and validating an offline match. See [external assets](docs/EXTERNAL_ASSETS.md) before opening the project on a new computer.

The current single-Quest/Pi setup has been checked for APK installation, Mirror host startup, and Quest-to-Pi packet delivery. The four combat signals still need a live in-headset match test; confirmed hit and shield-block signals require an opponent or game hazard.
