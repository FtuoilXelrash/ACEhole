# ACEhole

![Version](https://img.shields.io/badge/version-0.0.1-blue.svg)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![License](https://img.shields.io/badge/license-GPLv3-green.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)](https://www.microsoft.com/windows)

A Windows Forms application for managing and supervising a locally-hosted ACE (Asheron's Call
Emulator) Dedicated Server. Replaces the batch-file restart loop with a protected GUI that launches,
monitors, and auto-restarts the server — with live console output and process supervision.

**Current Version:** 0.0.1

## Features

- **Server Console** — Live output from `ACE.Server.exe` read directly from the ACE log file,
  color-coded by message type (joins, leaves, chat, errors, warnings, saves, mods).
  - **Console Commands** bar — Send any server console command via stdin.
    Responses appear in the console. In-memory history (last 50) in the dropdown.
  - **Global Chat** bar — Send a global broadcast message to all in-game players via stdin.
    In-memory history (last 50).
  - Consecutive identical lines are collapsed into a suppression notice to keep the console
    readable. Toggleable in Settings → General.
  - Console trim prevents memory growth over days of uptime (circular buffer, 5 000-line cap).

- **Process Supervision** — Launch, stop, and restart `ACE.Server.exe` from the toolbar.
  - Crash detection via `Process.Exited`; configurable auto-restart delay (replaces the batch
    `goto :start` loop entirely).
  - Close prompt when the server is running: **Hide to Tray · Stop Server and Exit · Cancel**
  - No command-line arguments are passed — ACE reads all configuration from its own config files.

- **Server Status Tab** — Process-level metrics and future console-parsed data:
  - **Server Status** — server name, log file status
  - **Process Monitor** — server state, PID, RAM, CPU
  - **Population Data** — Players Online, Max Players
  - **World Data** — In-Game Time (future: from console parsing)
  - **Server Data** — Memory, Network, Uptime
  - **Player Database** — Total Players (future)
  - **MODs** — future: total, loaded, in error
  - Manual **Refresh Stats** button.

- **Always-visible Status Bar** — 2-row panel at the bottom of every tab:
  - Row 1: `Players Online`
  - Row 2: `State · Uptime · Memory` — version right-aligned
  - Values green when populated, orange when zero, gray when server is stopped.

- **System Tray Integration** — Minimize to tray. Right-click context menu: Open / Exit.
  Double-click icon to restore.

- **Window Settings** — Per-window always-on-top and show-in-taskbar options in Settings →
  Windows tab. Settings dialog remembers its last position.

- **Window Position Memory** — Saves and restores position and size. Multi-monitor safe with
  automatic centering if position is off-screen.

- **Dark Mode Support** — Light / Dark / System theme (follows Windows setting). Configure in
  View → Settings → General.

- **JSON Configuration** — All settings in `data/config.json`.
  - Export Config and Import Config for full backup and restore.
  - Auto-migration adds new settings on upgrade without losing existing values.

- **Log Colors** — Configurable color swatches for every console line type (Joins/Leaves, Chat,
  Errors, Warnings, Saves, MODs, Default) plus App log level colors (INFO/WARN/ERROR/DEBUG).

- **Notifications** — Tray balloon notifications (server start/stop) and sound alerts
  (`server_start.wav` / `server_stop.wav`). Configure in View → Settings → Notifications.

- **Log Files** — App log (global, always active) and Console log (per-session server output).
  - Rotation by size (configurable), cleanup by file count, optional debug logging.
  - Floating Log Viewer window (**Log Files** toolbar button or **View → Log Files**): browse App
    and Console logs by date with **live tail** (real-time updates, color-coded, smart auto-scroll).

## Quick Start

1. Open **View → Settings → Server Settings** and verify:
   - **ACE Executable** — path to `ACE.Server.exe` (default: `Z:\ACE TEST\ACE.Server.exe`)
   - **Log File Path** — path to ACE log file (default: `Z:\ACE TEST\Logs\FULL\ACE_Log.txt`)
2. Click **▶ Start Server**.
3. The console tab shows all server output. The status bar updates with process metrics.

## Configuration — Settings Dialog

**Server Settings tab (Paths / Cycles sub-tabs):**

| Setting | Default | Description |
|---|---|---|
| ACE Executable | `Z:\ACE TEST\ACE.Server.exe` | Full path to `ACE.Server.exe` |
| Log File Path | `Z:\ACE TEST\Logs\FULL\ACE_Log.txt` | ACE server log file to tail |
| Auto-restart on crash | On | Restart after unexpected process exit |
| Restart delay (sec) | 5 | Wait time before auto-restart |

**UI Settings tab (General / Windows sub-tabs):**
- General: Theme (Light / Dark / System), time format, minimize to tray, auto-refresh Server
  Status interval, suppress repeated console lines, Export/Import Config
- Windows: per-window always-on-top and show-in-taskbar toggles + remembered position for Main
  Window, Log Viewer, and Settings dialog

**Logs tab (Log Files / Log Colors sub-tabs):**
- Log Files: Max log file size, files to retain, debug logging, auto-delete
- Log Colors: color swatch buttons for each console and app log line type

## Requirements

- **Operating System:** Windows 10 or higher
- **Framework:** .NET 8.0 Runtime
- **ACE Server:** `ACE.Server.exe` with a valid configuration

## Building from Source

### Prerequisites

- Visual Studio 2022 or later
- .NET 8.0 SDK

### Build Steps

1. Open `ACEhole.sln` in Visual Studio
2. Build the solution:
   ```bash
   msbuild ACEhole.sln /p:Configuration=Release
   ```
3. Compiled executable: `bin\Release\net8.0-windows10.0.17763.0\ACEhole.exe`

## Scripts (all under `scripts\`)

| Script | Purpose |
|---|---|
| `test-cert_and_build.ps1` | Build signed MSIX for local test install (run as Admin) |
| `release-build.ps1` | Build unsigned MSIX for MS Store submission |
| `installed_app-certification.ps1` | Run WACK against installed package |
| `packaged_app-certification.ps1` | Run WACK against MSIX package file |
| `backup_project.ps1` | Timestamped project backup |
| `increment_version.ps1` | Auto-increment AssemblyInfo version |

## Directory Structure

```
ACEhole/
├── data/                           # Runtime data (release: %LocalAppData%\ACEhole\)
│   └── config.json                 # Application configuration
├── logs/
│   ├── App/                        # Global app log (always active)
│   │   └── AppLog_YYYY-MM-DD.log
│   └── Console/                    # Per-session server console log
│       └── ConsoleLog_YYYY-MM-DD.log
├── sounds/                         # Sound notification files
│   ├── server_start.wav
│   └── server_stop.wav
└── docs/
    ├── README.txt                  # This file (plain text)
    └── ACEhole.html                # User guide (HTML)
```

## Author

**Ftuoil Xelrash**

## Acknowledgments

Built for ACE server administrators who want protection from accidental console closes and a
clean, supervised replacement for the batch-file restart loop.

---

**Note:** ACEhole requires a locally-installed ACE Dedicated Server. It is intended for server
owners/administrators running the server on the same machine as the application.
