ACEhole - ACE Asheron's Call Server Manager
Version 0.0.2
Author: Ftuoil Xelrash
===========================================

A Windows application for managing and supervising a locally-hosted ACE (Asheron's Call
Emulator) Dedicated Server. Replaces the batch-file restart loop with a protected GUI that
launches, monitors, and auto-restarts the server with live console output.

QUICK START
-----------
1. Open View -> Settings -> Server Settings
2. Set the ACE Executable path (default: Z:\ACE TEST\ACE.Server.exe)
3. Set the Log File Path (default: Z:\ACE TEST\Logs\FULL\ACE_Log.txt)
4. Click Start Server

KEY FEATURES
------------
- Live console output from ACE.Server.exe via log file tailing
- Console Commands bar - send stdin commands to the server
- Global Chat bar - send broadcast messages via stdin
- Process supervision with crash detection and auto-restart
- Server Status tab with process metrics (PID, RAM, CPU, Uptime)
- 2-row status bar: Players Online | State, Uptime, Memory
- MODs tab placeholder (MOD management coming in future versions)
- Server Configuration tab placeholder (for future use)
- Settings dialog with position memory and dark mode support
- Log Viewer with live tail and date navigation
- System tray integration

SETTINGS
--------
Server Settings (Paths):
  ACE Executable  - Full path to ACE.Server.exe
  Log File Path   - Full path to ACE log file to tail

Server Settings (Cycles):
  Auto-restart on crash - Restart server after unexpected exit
  Crash restart delay   - Seconds to wait before restarting

For full documentation see docs\ACEhole.html

REQUIREMENTS
------------
- Windows 10 or higher
- .NET 8.0 Runtime
- ACE Dedicated Server installed and configured

BUILD FROM SOURCE
-----------------
- Visual Studio 2022 or later
- .NET 8.0 SDK
- Open ACEhole.sln and build
