<#
.SYNOPSIS
Mirrors each Quest's view to this PC with scrcpy over the LAN; no internet is needed.

.DESCRIPTION
Plug a headset in by USB and run this once: it switches the headset to network ADB on port 5555
(this lasts until the headset reboots), prints its Wi-Fi IP and opens its mirror window. Repeat for
the second headset. The addresses are remembered, so later, with nothing plugged in, just run it again
to reconnect to the same headsets. To pick headsets yourself, pass their IPs (ip:port also works, e.g.
for Quest wireless debugging's own port):

    tools\quest-mirror.ps1 -Ip 192.168.1.106,192.168.1.107

Give each Quest a DHCP reservation on the router so its IP never changes. The PC connects to the
headsets; the game is not involved and the headsets never need the PC's IP. A sleeping headset
(not worn) sends no frames, and its window can stay blank after it wakes: start the mirror while
the headset is on someone's head, or close the window and run this again.

After a Wi-Fi stall the window skips the late video and jumps back to live, rather than playing it
fast to catch up (quest-mirror-relay.ps1, one hidden helper per window; -NoSkip turns it off).
#>
param(
    [string[]]$Ip = @(),
    # Network ADB port set on plugged-in headsets, and used for -Ip entries given without one.
    [int]$Port = 5555,
    # Share of the left eye's width shown. 1 (the default) shows the whole left-eye view as the headset
    # draws it: pre-warped for its lenses and ringed in black. Below 1, a 16:9 crop around the eye's
    # centre keeps the flatter middle.
    [ValidateRange(0.3, 1.0)][double]$Zoom = 1.0,
    # Pixels along the captured picture's long side, before the window squeezes it to shape. The capture
    # is stretched three times wide, so 2560 keeps about 900 of the eye's 1920 rows, as many as the
    # default window shows. Raise it with -WindowHeight for a bigger window; 4096 (about 1430 rows) is
    # the encoder's limit, and loads the headset enough to stutter while the game runs.
    [int]$MaxSize = 2560,
    # Window height in pixels; the width follows from the view's shape.
    [int]$WindowHeight = 900,
    [int]$Fps = 24,
    [string]$BitRate = '5M',
    # h265 looks better than h264 at the same bit rate, which matters on a crowded Wi-Fi.
    [ValidateSet('h264', 'h265')][string]$Codec = 'h265',
    # Milliseconds of video held back on this PC to ride out Wi-Fi hiccups: smoother, but that much
    # later. 0 shows each frame as soon as it arrives.
    [ValidateRange(0, 1000)][int]$Buffer = 0,
    # After a stall, video more than this many milliseconds late is skipped (the window holds its last
    # picture, then jumps back to live) rather than played fast to catch up. See quest-mirror-relay.ps1.
    [ValidateRange(50, 5000)][int]$SkipLateMs = 200,
    # Shows every frame, catching up fast after a stall, as scrcpy does on its own.
    [switch]$NoSkip
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

$relay = Join-Path $PSScriptRoot 'quest-mirror-relay.ps1'
$shell = (Get-Command pwsh.exe -ErrorAction SilentlyContinue).Source
if (-not $shell) { $shell = (Get-Command powershell.exe).Source }

# The first local TCP port from $from up that nothing uses, including windows still open from an
# earlier run, and that this run hasn't handed out yet.
$takenPorts = [System.Collections.Generic.HashSet[int]]::new()
function Get-FreePort([int]$from) {
    $used = @(Get-NetTCPConnection -ErrorAction SilentlyContinue | ForEach-Object LocalPort)
    for ($p = $from; $p -lt 65535; $p++) {
        if ($used -notcontains $p -and $takenPorts.Add($p)) { return $p }
    }
    throw 'No free local port for the mirror.'
}

function Get-QuestIp([string]$serial) {
    $line = & $adb -s $serial shell 'ip -o -4 addr show wlan0' 2>$null
    if ($line -match 'inet (\d+\.\d+\.\d+\.\d+)') { $Matches[1] }
}

$targets = [System.Collections.Generic.List[string]]::new()
$usbFor = @{}   # Wi-Fi target -> USB serial of the same headset, while it is plugged in
# Headsets that connected last time, so a plain rerun finds them without retyping IPs.
$savedFile = Join-Path $env:LOCALAPPDATA 'quest-mirror-targets.txt'
foreach ($address in $Ip -split ',' | Where-Object { $_ }) {
    $address = $address.Trim()
    $targets.Add($(if ($address -match ':\d+$') { $address } else { "${address}:$Port" }))
}

$usb = & $adb devices | Select-String '^(\S+)\s+device$' | ForEach-Object { $_.Matches[0].Groups[1].Value } |
    Where-Object { $_ -notmatch ':' }
foreach ($serial in $usb) {
    $address = Get-QuestIp $serial
    if (-not $address) {
        Write-Warning "$serial is not on Wi-Fi; mirroring it over USB instead. Join it to the router to mirror wirelessly."
        $targets.Add($serial)
        continue
    }
    & $adb -s $serial tcpip $Port | Out-Null
    Write-Host "$serial is at $address; network ADB is on until it reboots. You can unplug it."
    $usbFor["${address}:$Port"] = $serial
    if (-not $targets.Contains("${address}:$Port")) { $targets.Add("${address}:$Port") }
}
if ($usb) { Start-Sleep -Seconds 2 }  # adbd restarts in network mode

if ($targets.Count -eq 0 -and (Test-Path $savedFile)) {
    foreach ($saved in Get-Content $savedFile | Where-Object { $_ }) { $targets.Add($saved.Trim()) }
    if ($targets.Count) { Write-Host "Reconnecting to last headsets: $($targets -join ', ')" }
}
if ($targets.Count -eq 0) { throw 'No headset found. Plug one in by USB, or pass -Ip with each Quest''s Wi-Fi IP.' }

$connected = [System.Collections.Generic.List[string]]::new()
$mirrored = 0
foreach ($target in $targets) {
    if ($target -match ':') {
        # A connection left "offline" (headset slept or changed network) blocks a fresh connect; drop it first.
        # (Windows PowerShell turns its "no such device" reply into a terminating error under Stop.)
        try { & $adb disconnect $target 2>&1 | Out-Null } catch { }
        $result = & $adb connect $target
        # A failure also says "connected" ("the connected party did not respond").
        # Remember connected headsets, and plugged-in ones whose Wi-Fi IP was just read even if this PC can't reach it yet.
        if ($result -match '^(already )?connected to' -or $usbFor[$target]) { $connected.Add($target) }
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
    $arguments = @(
        # Start-Process joins arguments with spaces, so quote the title's.
        '-s', $target, "--window-title=`"Quest $target`"",
        '--no-audio', '--no-control', '--crop', $crop,
        '--render-fit=stretched', '--no-window-aspect-ratio-lock',
        '--window-height', $WindowHeight, '--window-width', [int]($WindowHeight * $aspect),
        '--max-size', $MaxSize, '--max-fps', $Fps, '--video-bit-rate', $BitRate,
        '--video-codec', $Codec, '--video-buffer', $Buffer)
    if (-not $NoSkip) {
        # scrcpy forwards its own port to the headset and reads the video from the relay's port instead;
        # the relay passes it on, skipping late frames. A key frame every second keeps skips short.
        $forwardPort = Get-FreePort 27183
        $relayPort = Get-FreePort ($forwardPort + 1)
        Start-Process $shell -WindowStyle Hidden -ArgumentList @(
            '-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', "`"$relay`"",
            '-ListenPort', $relayPort, '-TargetPort', $forwardPort, '-LateMs', $SkipLateMs)
        $deadline = (Get-Date).AddSeconds(15)
        while (-not (Get-NetTCPConnection -LocalPort $relayPort -State Listen -ErrorAction SilentlyContinue)) {
            if ((Get-Date) -gt $deadline) { throw "The frame-skipping relay didn't start; rerun with -NoSkip to mirror without it." }
            Start-Sleep -Milliseconds 200
        }
        $arguments += @('--port', $forwardPort, '--tunnel-port', $relayPort, '--video-codec-options', 'i-frame-interval=1')
    }
    Start-Process $scrcpy -ArgumentList $arguments
    Write-Host "Mirroring $target"
    $mirrored++
}
if ($connected.Count) { Set-Content -Path $savedFile -Value $connected }
if (-not $mirrored) { throw 'No headset could be mirrored. Is this PC on the same Wi-Fi as the headsets? Plug one in by USB and rerun.' }
