@echo off
rem Launches tools\quest-mirror.ps1: switches each USB-connected Quest to network ADB and opens its mirror window.
rem   quest-mirror.cmd                                     Quest(s) plugged in by USB, or reconnect to the last headsets
rem   quest-mirror.cmd -Ip 192.168.1.106,192.168.1.107     pick headsets by Wi-Fi IP (ip:port also works)
powershell -NoProfile -ExecutionPolicy Bypass -Command "& '%~dp0tools\quest-mirror.ps1' %*"
if errorlevel 1 pause
