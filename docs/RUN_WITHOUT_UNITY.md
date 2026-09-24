# Install the Quest demo without Unity

Use the install bundle for the matching project version. It contains the compiled Quest APK, Pi receiver/service files, and the two Quest role configs. You do **not** need Unity or the licensed source asset packs to install the APK. You still need two developer-enabled Quest headsets, one or two Raspberry Pis, a local Wi-Fi router, and Android `adb` on a computer. The Pi uses Python 3 and systemd. Internet is needed to obtain tools and the bundle; the demo itself uses only the router LAN.

1. Download and unzip the install bundle from the GitHub release. From the unzipped folder, use the commands in [SETUP.md](SETUP.md), starting with **Prepare the router and devices**. Skip **Clone and build the Unity project**; the APK is already in the bundle as `ThermalGameDemo-Quest-LAN.apk`.
2. In the setup guide's APK install commands, replace `/tmp/ThermalGameDemo-2min-LAN.apk` with the full path to the bundled `ThermalGameDemo-Quest-LAN.apk`. Run the Pi setup commands from the bundle's root so `tools/pi/receiver.py` and the service file resolve.
3. Push the bundled Quest A host/output config to Quest A. For Quest B, push the bundled client config and create its output config with its own Pi address and `quest-b` label. Restart each app after pushing configs.
4. Watch the Pi journal while playing and follow the [offline acceptance steps](SETUP.md#6-run-and-verify-the-demo).

The APK is the executable game for the headset. A `.unitypackage` is for importing editable assets into the Unity Editor; it is not installable on Quest and would not replace the APK. Editable source remains in GitHub, while the licensed Asset Store source packs must be obtained by anyone rebuilding the project.

This is a prototype handoff: one Quest as host and Quest-to-Pi UDP delivery have been verified. A complete two-Quest match and all four live signal types remain to be tested before treating the bundle as production-ready.
