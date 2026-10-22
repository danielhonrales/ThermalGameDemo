<#
.SYNOPSIS
Mirrors each Quest's left-eye view to this PC with scrcpy over the LAN; no internet is needed.

.DESCRIPTION
Plug a headset in by USB and run this once: it switches the headset to network ADB on port 5555
(this lasts until the headset reboots), prints its Wi-Fi IP and opens its mirror window. Repeat for
the second headset. Later, with nothing plugged in, pass the IPs to reopen the windows:

    tools\quest-mirror.ps1 -Ip 192.168.1.21,192.168.1.22

Give each Quest a DHCP reservation on the router so its IP never changes. The PC connects to the
headsets; the game is not involved and the headsets never need the PC's IP. A sleeping headset
(not worn) sends no frames, and its window can stay blank after it wakes: start the mirror while
the headset is on someone's head, or close the window and run this again.
#>
param(
    [string[]]$Ip = @(),
    # Share of the left eye's width shown, as a 16:9 crop around the lens centre. The headset's
    # picture is pre-warped for its lenses and ringed in black; cropping keeps the flatter middle.
    # 0.9 is about the widest without the black ring showing at the edge; 1 shows the whole eye.
    [ValidateRange(0.3, 1.0)][double]$Zoom = 0.9,
    [int]$MaxSize = 1280,
    [int]$Fps = 30,
    [string]$BitRate = '8M'
)
$ErrorActionPreference = 'Stop'

$scrcpy = (Get-Command scrcpy.exe -ErrorAction SilentlyContinue).Source
if (-not $scrcpy) {
    $scrcpy = Get-ChildItem "$env:LOCALAPPDATA\Microsoft\WinGet\Packages" -Recurse -Filter scrcpy.exe -ErrorAction SilentlyContinue |
        Select-Object -First 1 -ExpandProperty FullName
}
if (-not $scrcpy) { throw 'scrcpy not found. Install it with: winget install Genymobile.scrcpy' }
# Resolve the real exe behind the WinGet alias; scrcpy ships an adb beside it.
$link = (Get-Item $scrcpy).Target
if ($link) { $scrcpy = @($link)[0] }
# Two adb versions on one PC (e.g. Meta Quest Developer Hub's) keep killing each other's server and
# the headset drops off the list. Reuse whichever adb is already running.
$adb = Get-Process adb -ErrorAction SilentlyContinue | Where-Object Path | Select-Object -First 1 -ExpandProperty Path
if (-not $adb) { $adb = Join-Path (Split-Path $scrcpy) 'adb.exe' }
# scrcpy reads ADB; a value pointing at a folder makes it fail to start adb.
$env:ADB = $adb

function Get-QuestIp([string]$serial) {
    $line = & $adb -s $serial shell 'ip -o -4 addr show wlan0' 2>$null
    if ($line -match 'inet (\d+\.\d+\.\d+\.\d+)') { $Matches[1] }
}

$targets = [System.Collections.Generic.List[string]]::new()
foreach ($address in $Ip) { $targets.Add("${address}:5555") }

$usb = & $adb devices | Select-String '^(\S+)\s+device$' | ForEach-Object { $_.Matches[0].Groups[1].Value } |
    Where-Object { $_ -notmatch ':' }
foreach ($serial in $usb) {
    $address = Get-QuestIp $serial
    if (-not $address) {
        Write-Warning "$serial is not on Wi-Fi; mirroring it over USB instead. Join it to the router to mirror wirelessly."
        $targets.Add($serial)
        continue
    }
    & $adb -s $serial tcpip 5555 | Out-Null
    Write-Host "$serial is at $address; network ADB is on until it reboots. You can unplug it."
    if (-not $targets.Contains("${address}:5555")) { $targets.Add("${address}:5555") }
}
if ($usb) { Start-Sleep -Seconds 2 }  # adbd restarts in network mode

if ($targets.Count -eq 0) { throw 'No headset found. Plug one in by USB, or pass -Ip with each Quest''s Wi-Fi IP.' }

foreach ($target in $targets) {
    if ($target -match ':') {
        $result = & $adb connect $target
        if ($result -notmatch 'connected') { Write-Warning "${target}: $result"; continue }
    }
    # The display is both eyes side by side; mirror the left eye only.
    $size = & $adb -s $target shell wm size | Select-String 'Physical size: (\d+)x(\d+)'
    $eyeWidth = [int]$size.Matches[0].Groups[1].Value / 2
    $height = [int]$size.Matches[0].Groups[2].Value
    if ($Zoom -ge 1) { $crop = "${eyeWidth}:${height}:0:0" }
    else {
        # The left lens's visible circle sits nearer the nose, measured on a Quest 3S.
        $w = [int]($eyeWidth * $Zoom / 2) * 2
        $h = [int]($w * 9 / 16 / 2) * 2
        $x = [Math]::Min([Math]::Max(0, [int]($eyeWidth * 0.539 - $w / 2)), $eyeWidth - $w)
        $y = [Math]::Min([Math]::Max(0, [int]($height * 0.483 - $h / 2)), $height - $h)
        $crop = "${w}:${h}:${x}:${y}"
    }
    Start-Process $scrcpy -ArgumentList @(
        # Start-Process joins arguments with spaces, so quote the title's.
        '-s', $target, "--window-title=`"Quest $target`"",
        '--no-audio', '--no-control', '--crop', $crop,
        '--max-size', $MaxSize, '--max-fps', $Fps, '--video-bit-rate', $BitRate)
    Write-Host "Mirroring $target"
}
