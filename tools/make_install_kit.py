#!/usr/bin/env python3
"""Package a built Quest APK with the Pi receiver and setup documentation."""

import argparse
import hashlib
from pathlib import Path
from zipfile import ZIP_DEFLATED, ZIP_STORED, ZipFile


ROOT = Path(__file__).resolve().parents[1]
PI_FILES = (
    Path("tools/pi/receiver.py"),
    Path("tools/pi/thermal-game-receiver.service"),
    Path("tools/pi/quest-a-combat-output.json"),
)


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--apk", required=True, type=Path, help="Built Quest APK")
    parser.add_argument("--output", required=True, type=Path, help="Output ZIP")
    args = parser.parse_args()

    apk = args.apk.resolve(strict=True)
    if apk.suffix.lower() != ".apk":
        parser.error("--apk must point to an APK file")
    output = args.output.resolve()
    if output == apk:
        parser.error("--output must differ from --apk")
    docs = sorted((ROOT / "docs").glob("*.md"))
    files = [ROOT / "README.md", *(ROOT / path for path in PI_FILES), *docs]
    for path in files:
        if not path.is_file():
            parser.error(f"Required bundle file is missing: {path}")

    output.parent.mkdir(parents=True, exist_ok=True)
    digest = hashlib.sha256()
    with apk.open("rb") as source:
        for chunk in iter(lambda: source.read(1024 * 1024), b""):
            digest.update(chunk)

    with ZipFile(output, "w") as bundle:
        bundle.write(apk, "ThermalGameDemo-Quest-LAN.apk", compress_type=ZIP_STORED)
        for path in files:
            bundle.write(path, path.relative_to(ROOT).as_posix(), compress_type=ZIP_DEFLATED)
        bundle.writestr(
            "SHA256SUMS",
            f"{digest.hexdigest()}  ThermalGameDemo-Quest-LAN.apk\n",
        )
    print(f"Wrote {output} ({output.stat().st_size} bytes)")
    print(f"APK SHA256 {digest.hexdigest()}")


if __name__ == "__main__":
    main()
