# Install the Quest demo without Unity

Use the install bundle for the matching project version. It contains the compiled Quest APK, Pi receiver/service files, a sample Pi output config, and the setup documentation. You do **not** need Unity or the licensed source asset packs to install the APK. For one-player practice, one developer-enabled Quest and one Raspberry Pi are enough; a duel needs two Quests. You also need a local Wi-Fi router and Android `adb` on a computer. The Pi uses Python 3 and systemd. Internet is needed to obtain tools and the bundle; the demo itself uses only the router LAN.

1. Download `ThermalGameDemo-LAN-auto-install-kit.zip` from the [latest GitHub release](https://github.com/danielhonrales/ThermalGameDemo/releases/latest) and unzip it. From the unzipped folder, run `sha256sum -c SHA256SUMS` and `export APK_PATH="$PWD/ThermalGameDemo-Quest-LAN.apk"`, then use the commands in [SETUP.md](SETUP.md), starting with **Prepare the router and devices**. Skip **Clone and build the Unity project**; the APK is already in the bundle.
2. Run the Pi setup commands from the bundle's root so `tools/pi/receiver.py` and the service file resolve. The `APK_PATH` setting from step 1 makes the install commands use the bundled APK.
3. Install this same APK on both headsets; older LAN builds use a different match version and will not pair with it. The headsets choose host/client automatically. Push the bundled Pi output config to Quest A. For Quest B, create its output config with its own Pi address and `quest-b` label. Restart each app after pushing output configs.
4. Watch the Pi journal while playing and follow the [offline acceptance steps](SETUP.md#6-run-and-verify-the-demo).

The APK is the executable game for the headset. A `.unitypackage` is for importing editable assets into the Unity Editor; it is not installable on Quest and would not replace the APK. Editable source remains in GitHub, while the licensed Asset Store source packs must be obtained by anyone rebuilding the project.

This is a prototype handoff: one Quest as host and live Quest-to-Pi fire, ice, and unshielded-hit events have been verified. A complete two-Quest match on this exact build, a shield-block event, and a WAN-disconnected round remain to be verified.
