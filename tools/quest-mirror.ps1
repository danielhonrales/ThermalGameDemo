<#
.SYNOPSIS
Mirrors each Quest's view to this PC with scrcpy over the LAN; no internet is needed.

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
    # Share of the left eye's width shown. 1 (the default) shows the whole left-eye view as the headset
    # draws it: pre-warped for its lenses and ringed in black. Below 1, a 16:9 crop around the eye's
    # centre keeps the flatter middle.
    [ValidateRange(0.3, 1.0)][double]$Zoom = 1.0,
    # Pixels along the captured picture's long side, before the window squeezes it to shape. The capture
    # is stretched three times wide, so 4096 (the encoder's limit) keeps about 1430 of the eye's 1920 rows.
    [int]$MaxSize = 4096,
    # Window height in pixels; the width follows from the view's shape.
    [int]$WindowHeight = 900,
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
$usbFor = @{}   # Wi-Fi target -> USB serial of the same headset, while it is plugged in
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
    $usbFor["${address}:5555"] = $serial
    if (-not $targets.Contains("${address}:5555")) { $targets.Add("${address}:5555") }
}
if ($usb) { Start-Sleep -Seconds 2 }  # adbd restarts in network mode

if ($targets.Count -eq 0) { throw 'No headset found. Plug one in by USB, or pass -Ip with each Quest''s Wi-Fi IP.' }

foreach ($target in $targets) {
    if ($target -match ':') {
        $result = & $adb connect $target
        # A failure also says "connected" ("the connected party did not respond").
        if ($result -notmatch '^(already )?connected to') {
            Write-Warning "${target}: $result"
            if (-not $usbFor[$target]) { continue }
            # Plugged in but unreachable (e.g. this PC is on another network): mirror over the cable.
            $target = $usbFor[$target]
            Write-Warning "Mirroring $target over USB instead; keep it plugged in."
        }
    }
    # The panel holds both eyes side by side (on a Quest 3S, 3664 x 1920: two 1832 x 1920 views), but
    # the headset overrides the display size (10992 x 1920), and scrcpy crops in those stretched
    # units. Crop the left half of the override, or a part of it, and let the window squeeze it back to
    # the eye's true shape.
    $sizes = & $adb -s $target shell wm size
    $physical = $sizes | Select-String 'Physical size: (\d+)x(\d+)'
    $override = $sizes | Select-String 'Override size: (\d+)x(\d+)'
    $width = [int]$physical.Matches[0].Groups[1].Value
    $height = [int]$physical.Matches[0].Groups[2].Value
    $logical = if ($override) { $override } else { $physical }
    $logicalWidth = [int]$logical.Matches[0].Groups[1].Value
    $logicalHeight = [int]$logical.Matches[0].Groups[2].Value
    $sx = $logicalWidth / $width
    $sy = $logicalHeight / $height
    $eyeWidth = [int]($logicalWidth / 2)
    if ($Zoom -ge 1)
    {
        $crop = "${eyeWidth}:${logicalHeight}:0:0"
        $aspect = ($width / 2) / $height
    }
    else {
        # 16:9 in true pixels around the left eye's centre, converted to the stretched units.
        $trueWidth = $width / 2 * $Zoom
        $trueHeight = [Math]::Min($height, $trueWidth * 9 / 16)
        $w = [int]($trueWidth * $sx / 2) * 2
        $h = [int]($trueHeight * $sy / 2) * 2
        $x = [int](($eyeWidth - $w) / 2)
        $y = [int](($logicalHeight - $h) / 2)
        $crop = "${w}:${h}:${x}:${y}"
        $aspect = $trueWidth / $trueHeight
    }
    Start-Process $scrcpy -ArgumentList @(
        # Start-Process joins arguments with spaces, so quote the title's.
        '-s', $target, "--window-title=`"Quest $target`"",
        '--no-audio', '--no-control', '--crop', $crop,
        '--render-fit=stretched', '--no-window-aspect-ratio-lock',
        '--window-height', $WindowHeight, '--window-width', [int]($WindowHeight * $aspect),
        '--max-size', $MaxSize, '--max-fps', $Fps, '--video-bit-rate', $BitRate)
    Write-Host "Mirroring $target"
}
