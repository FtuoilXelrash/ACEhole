using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACEhole
{
    public partial class Main_Win : Form
    {
        // ── Fields ────────────────────────────────────────────────────────
        private AppConfig      _config;
        private ProcessManager _processManager;
        private bool           _isExiting = false;
        private bool           _isDark    = false;

        private System.Windows.Forms.Timer _statusUpdateTimer;
        private System.Windows.Forms.Timer _procMonitorTimer;

        private LogViewerForm _logViewerForm;

        // Status bar cached values
        private string _statPlayers = "--";
        private string _statUptime  = "--";
        private string _statMemory  = "--";

        // Server start time for uptime calculation
        private DateTime _serverStartTime = DateTime.MinValue;

        // CPU measurement between timer samples
        private TimeSpan _lastCpuTime     = TimeSpan.Zero;
        private DateTime _lastCpuWallTime = DateTime.MinValue;

        // Server Configuration and MODs tabs (built in code)
        private TabPage tab_ServerConfig;

        // ── Constructor ───────────────────────────────────────────────────
        public Main_Win()
        {
            InitializeComponent();

            consoleChatPanel.Resize += (s, e) =>
            {
                int w = consoleChatPanel.ClientSize.Width;
                consoleCommandComboBox.Width = w;
                consoleChatComboBox.Width    = w;
            };

            SetupStatusTab();
            SetupStatusBar();
            SetupModsTab();
            SetupServerConfigTab();

            try
            {
                this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
                notifyIcon1.Icon = this.Icon;
            }
            catch { }

            _config = ConfigManager.LoadConfig();
            Logger.SetConfig(_config);

            // Restore command/chat history
            if (_config.Ui.CommandHistory?.Count > 0)
            {
                int max = Math.Max(1, _config.Ui.CommandHistoryMaxSize);
                foreach (var h in _config.Ui.CommandHistory.Take(max))
                {
                    _commandHistory.Add(h);
                    consoleCommandComboBox.Items.Add(h);
                }
            }
            if (_config.Ui.ChatHistory?.Count > 0)
            {
                int max = Math.Max(1, _config.Ui.ChatHistoryMaxSize);
                foreach (var h in _config.Ui.ChatHistory.Take(max))
                {
                    _chatHistory.Add(h);
                    consoleChatComboBox.Items.Add(h);
                }
            }

            _isDark = ThemeManager.ShouldUseDarkMode(_config.Ui.Theme);
            ThemeManager.ApplyTheme(this, _isDark);
            ApplyConsoleColors();

            this.TopMost       = _config.Ui.MainWindowAlwaysOnTop;
            this.ShowInTaskbar = _config.Ui.MainWindowShowInTaskbar;

            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            statVersionLabel.Text = $"v{ver.Major}.{ver.Minor}.{ver.Build}";

            RestoreWindowPosition();

            _statusUpdateTimer          = new System.Windows.Forms.Timer();
            _statusUpdateTimer.Interval = Math.Max(5, _config.Ui.StatusUpdateIntervalSecs) * 1000;
            _statusUpdateTimer.Tick    += StatusUpdateTimer_Tick;

            _procMonitorTimer          = new System.Windows.Forms.Timer();
            _procMonitorTimer.Interval = 2000;
            _procMonitorTimer.Tick    += ProcMonitorTimer_Tick;

            _processManager = new ProcessManager(_config);
            _processManager.StateChanged   += ProcessManager_StateChanged;
            _processManager.OutputReceived += ProcessManager_OutputReceived;
            _processManager.ServerStarted  += ProcessManager_ServerStarted;
            _processManager.ServerExited   += ProcessManager_ServerExited;

            UpdateStatusBar();
            UpdateServerStateLabel(ServerState.Stopped);

            if (!string.IsNullOrEmpty(_config.Ui.LastActiveTab))
                foreach (TabPage tp in Main_Tabs.TabPages)
                    if (tp.Name == _config.Ui.LastActiveTab) { Main_Tabs.SelectedTab = tp; break; }
        }

        // ── Status Tab Layout ─────────────────────────────────────────────
        private void SetupStatusTab()
        {
            var staticColor = Color.FromArgb(100, 100, 100);
            var valueColor  = Color.FromArgb(158, 158, 158);
            var font        = new Font("Segoe UI", 9f);

            // ── Server Status GroupBox ─────────────────────────────────
            AddStatRow(serverStatusGroupBox, font, staticColor, valueColor, 20,
                "Server Name:", serverNameValueLabel, 210);
            AddStatRow(serverStatusGroupBox, font, staticColor, valueColor, 40,
                "Log File:", serverOnlineValueLabel, 180);
            serverOnlineValueLabel.Text      = "● Stopped";
            serverOnlineValueLabel.ForeColor = Color.FromArgb(158, 158, 158);

            // ── Process Monitor GroupBox ───────────────────────────────
            AddStatRow(procMonitorGroupBox, font, staticColor, valueColor, 20, "State:", procStateValueLabel, 200);
            AddStatRow(procMonitorGroupBox, font, staticColor, valueColor, 40, "PID:",   procPidValueLabel,   150);
            AddStatRow(procMonitorGroupBox, font, staticColor, valueColor, 60, "CPU:",   procCpuValueLabel,   100);
            AddStatRow(procMonitorGroupBox, font, staticColor, valueColor, 80, "RAM:",   procRamValueLabel,   150);

            // ── Population GroupBox ────────────────────────────────────
            AddStatRow(populationGroupBox, font, staticColor, valueColor, 20, "Players Online:", playersOnlineValueLabel, 100);
            AddStatRow(populationGroupBox, font, staticColor, valueColor, 40, "Max Players:",    maxPlayersValueLabel,    100);

            // ── World Data GroupBox ────────────────────────────────────
            AddStatRow(worldDataGroupBox, font, staticColor, valueColor, 20, "In-Game Time:", worldTimeValueLabel, 130);

            // ── Server Data GroupBox ───────────────────────────────────
            AddStatRow(serverDataGroupBox, font, staticColor, valueColor,  20, "Memory:",      memoryValueLabel,     130);
            AddStatRow(serverDataGroupBox, font, staticColor, valueColor,  40, "Network In:",  networkInValueLabel,  130);
            AddStatRow(serverDataGroupBox, font, staticColor, valueColor,  60, "Network Out:", networkOutValueLabel, 130);
            AddStatRow(serverDataGroupBox, font, staticColor, valueColor,  80, "Uptime:",      uptimeValueLabel,     130);

            // ── Player Database GroupBox ───────────────────────────────
            AddStatRow(playerDatabaseGroupBox, font, staticColor, valueColor, 20, "Total Players:", totalPlayersValueLabel, 100);

            // ── MODs GroupBox ──────────────────────────────────────────
            AddStatRow(modsGroupBox, font, staticColor, valueColor, 20, "Total MODs:", totalModsValueLabel,  100);
            AddStatRow(modsGroupBox, font, staticColor, valueColor, 40, "Loaded:",     loadedModsValueLabel, 100);
            AddStatRow(modsGroupBox, font, staticColor, valueColor, 60, "In Error:",   errorModsValueLabel,  100);
        }

        private static void AddStatRow(GroupBox gb, Font font,
            Color staticColor, Color valueColor,
            int y, string labelText, Label valueLabel, int valueWidth)
        {
            var staticLabel = new Label
            {
                Text      = labelText,
                Location  = new Point(8, y),
                AutoSize  = true,
                Font      = font,
                ForeColor = staticColor
            };
            valueLabel.Text      = "--";
            valueLabel.Location  = new Point(140, y);
            valueLabel.Size      = new Size(valueWidth, 18);
            valueLabel.Font      = font;
            valueLabel.ForeColor = valueColor;
            gb.Controls.Add(staticLabel);
            gb.Controls.Add(valueLabel);
        }

        // ── Status Bar Layout ─────────────────────────────────────────────
        // EXACT positions/spacing from rServed — keep what applies, remove the rest.
        // Row 1: Players Online only (no Joining/Queued/Sleeping)
        // Row 2: State, Uptime, Memory (no FPS/Entities)
        private void SetupStatusBar()
        {
            var staticColor = Color.FromArgb(130, 130, 130);
            var dashColor   = Color.FromArgb(158, 158, 158);
            var font        = new Font("Segoe UI", 8.25f);   // matches Fe2O3xH2O AutoScaleDimensions

            // ── Row 1  y=2 ────────────────────────────────────────────
            AddBarFixed("Players Online:", 8,   2, 96, font, staticColor);
            statPlayersOnlineValue = AddBarFixed("--", 105, 2, 78, font, dashColor);

            // ── Row 2  y=21 ───────────────────────────────────────────
            AddBarFixed("State:",    8,  21, 96, font, staticColor);
            statStateValue   = AddBarFixed("● Stopped", 105, 21, 78, font, dashColor, autoEllipsis: true);
            AddBarFixed("Uptime:", 185,  21, 55, font, staticColor);
            statUptimeValue  = AddBarFixed("--",        241, 21, 62, font, dashColor);
            AddBarFixed("Memory:", 305,  21, 60, font, staticColor);
            statMemoryValue  = AddBarFixed("--",        366, 21, 52, font, dashColor);
            // statVersionLabel is already positioned in Designer.cs (x=650, Anchor=Right)
        }

        private Label AddBarFixed(string text, int x, int y, int w, Font font, Color color,
                                  bool autoEllipsis = false)
        {
            var lbl = new Label
            {
                Text         = text,
                Location     = new Point(x, y),
                Size         = new Size(w, 13),
                Font         = font,
                ForeColor    = color,
                AutoSize     = false,
                AutoEllipsis = autoEllipsis
            };
            statusInfoPanel.Controls.Add(lbl);
            return lbl;
        }

        // ── Process Manager Events ────────────────────────────────────────
        private void ProcessManager_StateChanged(object sender, ServerState state)
        {
            if (InvokeRequired) { Invoke(new Action(() => ProcessManager_StateChanged(sender, state))); return; }

            UpdateServerStateLabel(state);
            UpdateToolbarButtons(state);

            switch (state)
            {
                case ServerState.Running:
                    _serverStartTime = DateTime.Now;
                    AppendConsole($"[ACEhole] Server is running (PID {_processManager.Pid})", Color.FromArgb(76, 175, 80));
                    AppendConsole($"[ACEhole] Tailing log: {_config.Server.LogFilePath}", Color.FromArgb(130, 130, 130));
                    serverOnlineValueLabel.Text      = "● Tailing";
                    serverOnlineValueLabel.ForeColor = Color.FromArgb(76, 175, 80);
                    _procMonitorTimer.Start();
                    if (_config.Ui.AutoUpdateStatus)
                        _statusUpdateTimer.Start();
                    break;

                case ServerState.Stopped:
                case ServerState.Crashed:
                    _serverStartTime = DateTime.MinValue;
                    _procMonitorTimer.Stop();
                    _statusUpdateTimer.Stop();
                    ResetStatusBar();
                    ResetStatusTabValues();
                    UpdateProcMonitorGroup(null);
                    serverOnlineValueLabel.Text      = "● Stopped";
                    serverOnlineValueLabel.ForeColor = Color.FromArgb(158, 158, 158);
                    break;
            }
        }

        private void ProcessManager_OutputReceived(object sender, string line)
        {
            if (InvokeRequired) { Invoke(new Action(() => ProcessManager_OutputReceived(sender, line))); return; }
            var type  = ConsoleLineClassifier.Classify(line);
            var color = ConsoleLineClassifier.GetColor(type, _config);
            AppendConsole(line, color);

            if (IsModMessage(line))
                AppendModNotification($"({Logger.GetDisplayTimestamp()}) | {line}\r\n");
        }

        private void ProcessManager_ServerStarted(object sender, EventArgs e)
        {
            if (InvokeRequired) { Invoke(new Action(() => ProcessManager_ServerStarted(sender, e))); return; }
            Logger.Info("Server process started event received.");
            if (_config.Notifications.EnableTrayNotifications && _config.Notifications.TrayNotifyOnServerStart)
                notifyIcon1.ShowBalloonTip(3000, "ACEhole", "Server started", ToolTipIcon.Info);
            if (_config.Notifications.EnableSoundOnServerStart)
                PlayNotificationSound("server_start.wav");
        }

        private void ProcessManager_ServerExited(object sender, int exitCode)
        {
            if (InvokeRequired) { Invoke(new Action(() => ProcessManager_ServerExited(sender, exitCode))); return; }
            AppendConsole($"[ACEhole] Server process exited (code {exitCode})", Color.FromArgb(239, 83, 80));
            if (_config.Notifications.EnableTrayNotifications && _config.Notifications.TrayNotifyOnServerStop)
                notifyIcon1.ShowBalloonTip(3000, "ACEhole", "Server stopped", ToolTipIcon.Warning);
            if (_config.Notifications.EnableSoundOnServerStop)
                PlayNotificationSound("server_stop.wav");
        }

        // ── Timer Ticks ───────────────────────────────────────────────────
        private void StatusUpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatusBarUptime();
        }

        private void ProcMonitorTimer_Tick(object sender, EventArgs e)
        {
            UpdateProcMonitorGroup(_processManager);
            UpdateStatusBarUptime();
            UpdateStatusBarMemory();
        }

        // ── Status Bar ────────────────────────────────────────────────────
        private void UpdateStatusBarUptime()
        {
            if (_processManager.IsRunning && _serverStartTime != DateTime.MinValue)
            {
                _statUptime = FormatUptimeSpan(DateTime.Now - _serverStartTime);
                UpdateStatusBar();
            }
        }

        private void UpdateStatusBarMemory()
        {
            if (_processManager.IsRunning)
            {
                long ram = _processManager.GetWorkingSetBytes();
                _statMemory = ram >= 1024L * 1024 * 1024
                    ? $"{ram / (1024.0 * 1024.0 * 1024.0):0.##} GB"
                    : $"{ram / (1024 * 1024):N0} MB";
                UpdateStatusBar();
            }
        }

        private void ResetStatusBar()
        {
            _statPlayers = "--"; _statUptime = "--"; _statMemory = "--";
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            var green  = Color.FromArgb(76, 175, 80);
            var orange = Color.FromArgb(255, 152, 0);
            var gray   = Color.FromArgb(158, 158, 158);

            SetStatLabel(statPlayersOnlineValue, _statPlayers, green, orange, gray);
            SetStatLabel(statUptimeValue,        _statUptime,  green, orange, gray);
            SetStatLabel(statMemoryValue,        _statMemory,  green, orange, gray);
        }

        private static void SetStatLabel(Label lbl, string value, Color green, Color orange, Color gray)
        {
            lbl.Text      = value;
            lbl.ForeColor = value == "--" ? gray : value == "0" ? orange : green;
        }

        private void UpdateServerStateLabel(ServerState state)
        {
            string text = state switch
            {
                ServerState.Stopped    => "● Stopped",
                ServerState.Starting   => "● Starting...",
                ServerState.Running    => "● Running",
                ServerState.Stopping   => "● Stopping...",
                ServerState.Crashed    => "● Crashed!",
                ServerState.Restarting => "● Restarting...",
                _                      => "● Unknown"
            };

            Color color = state switch
            {
                ServerState.Running    => Color.FromArgb(76, 175, 80),
                ServerState.Crashed    => Color.FromArgb(239, 83, 80),
                ServerState.Starting   => Color.FromArgb(255, 193, 7),
                ServerState.Restarting => Color.FromArgb(255, 193, 7),
                _                      => Color.FromArgb(158, 158, 158)
            };

            statStateValue.Text        = text;
            statStateValue.ForeColor   = color;
            procStateValueLabel.Text   = text;
            procStateValueLabel.ForeColor = color;
        }

        private void UpdateToolbarButtons(ServerState state)
        {
            bool canStart   = state == ServerState.Stopped || state == ServerState.Crashed;
            bool canStop    = state == ServerState.Running  || state == ServerState.Starting;
            bool canRestart = state == ServerState.Running;
            startServerButton.Enabled   = canStart;
            stopServerButton.Enabled    = canStop;
            restartServerButton.Enabled = canRestart;
        }

        private void UpdateProcMonitorGroup(ProcessManager pm)
        {
            if (pm == null || !pm.IsRunning)
            {
                procPidValueLabel.Text = "--";
                procRamValueLabel.Text = "--";
                procCpuValueLabel.Text = "--";
                uptimeValueLabel.Text  = "--";
                memoryValueLabel.Text  = "--";
                _lastCpuWallTime = DateTime.MinValue;
                return;
            }
            procPidValueLabel.Text = pm.Pid.ToString();

            long ram = pm.GetWorkingSetBytes();
            string ramStr = ram >= 1024L * 1024 * 1024
                ? $"{ram / (1024.0 * 1024.0 * 1024.0):0.##} GB"
                : $"{ram / (1024 * 1024):N0} MB";
            procRamValueLabel.Text = ramStr;
            memoryValueLabel.Text  = ramStr;

            TimeSpan nowCpu  = pm.GetTotalProcessorTime();
            DateTime nowWall = DateTime.UtcNow;
            if (_lastCpuWallTime != DateTime.MinValue && nowCpu > TimeSpan.Zero)
            {
                double cpuDelta  = (nowCpu  - _lastCpuTime).TotalSeconds;
                double wallDelta = (nowWall - _lastCpuWallTime).TotalSeconds;
                if (wallDelta > 0 && cpuDelta >= 0)
                {
                    double pct = (cpuDelta / (wallDelta * Environment.ProcessorCount)) * 100.0;
                    procCpuValueLabel.Text = $"{Math.Min(pct, 100.0):F1}%";
                }
            }
            else
            {
                procCpuValueLabel.Text = "...";
            }
            _lastCpuTime     = nowCpu;
            _lastCpuWallTime = nowWall;

            if (_serverStartTime != DateTime.MinValue)
            {
                string uptime = FormatUptimeSpan(DateTime.Now - _serverStartTime);
                uptimeValueLabel.Text = uptime;
            }
        }

        // ── Server Status Tab ─────────────────────────────────────────────
        private void ResetStatusTabValues()
        {
            serverNameValueLabel.Text    = "--";
            serverOnlineValueLabel.Text  = "● Stopped";
            serverOnlineValueLabel.ForeColor = Color.FromArgb(158, 158, 158);

            Label[] dashLabels = {
                playersOnlineValueLabel, maxPlayersValueLabel,
                worldTimeValueLabel,
                memoryValueLabel, networkInValueLabel, networkOutValueLabel, uptimeValueLabel,
                totalPlayersValueLabel,
                totalModsValueLabel, loadedModsValueLabel, errorModsValueLabel
            };
            foreach (var lbl in dashLabels) lbl.Text = "--";
        }

        private void refreshStatsButton_Click(object sender, EventArgs e)
        {
            if (_processManager.IsRunning)
            {
                UpdateProcMonitorGroup(_processManager);
                UpdateStatusBarUptime();
                UpdateStatusBarMemory();
            }
        }

        // ── Console Tab ───────────────────────────────────────────────────

        private string _lastConsolePattern = null;
        private int    _suppressedCount    = 0;
        private bool   _pendingNotice      = false;

        private void AppendConsole(string line, Color color)
        {
            string pattern = ConsoleLineClassifier.ExtractMessagePattern(line);

            if (_config.Ui.SuppressRepeatedConsoleLines &&
                pattern != null && pattern == _lastConsolePattern)
            {
                _suppressedCount++;
                _pendingNotice = true;
                if (_suppressedCount % 50 == 0)
                    DoAppendLine(
                        $"    ↑  [{_suppressedCount} identical lines suppressed — {Logger.GetDisplayTimestamp()}]",
                        Color.FromArgb(80, 80, 80));
                return;
            }

            if (_pendingNotice && _suppressedCount > 0)
            {
                DoAppendLine(
                    $"    ↑  [{_suppressedCount} identical line{(_suppressedCount == 1 ? "" : "s")} suppressed]",
                    Color.FromArgb(80, 80, 80));
            }

            _lastConsolePattern = pattern;
            _suppressedCount    = 0;
            _pendingNotice      = false;

            DoAppendLine(line, color);
        }

        private void DoAppendLine(string line, Color color)
        {
            if (consoleRTB.Lines.Length > 5000)
            {
                int start = consoleRTB.GetFirstCharIndexFromLine(500);
                consoleRTB.ReadOnly = false;
                consoleRTB.Select(0, start);
                consoleRTB.SelectedText = "";
                consoleRTB.ReadOnly = true;
            }

            bool atEnd = consoleRTB.SelectionStart >= consoleRTB.TextLength - 1;

            consoleRTB.SelectionStart  = consoleRTB.TextLength;
            consoleRTB.SelectionLength = 0;
            consoleRTB.SelectionColor  = Color.FromArgb(100, 100, 100);
            consoleRTB.AppendText($"({Logger.GetDisplayTimestamp()}) | ");

            consoleRTB.SelectionStart  = consoleRTB.TextLength;
            consoleRTB.SelectionLength = 0;
            consoleRTB.SelectionColor  = color;
            consoleRTB.AppendText(line + Environment.NewLine);

            if (atEnd) { consoleRTB.SelectionStart = consoleRTB.TextLength; consoleRTB.ScrollToCaret(); }
        }

        private void ApplyConsoleColors()
        {
            consoleRTB.BackColor = Color.FromArgb(12, 12, 12);
            consoleRTB.ForeColor = Color.FromArgb(212, 212, 212);
        }

        // ── Console Command ComboBox + Global Chat ComboBox ───────────────

        private readonly List<string> _commandHistory = new List<string>();
        private readonly List<string> _chatHistory    = new List<string>();

        private void consoleCommandComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendConsoleCommand();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void consoleChatComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendGlobalChat();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void SendConsoleCommand()
        {
            string cmd = consoleCommandComboBox.Text.Trim();
            if (string.IsNullOrEmpty(cmd)) return;

            int cmdMax = Math.Max(1, _config.Ui.CommandHistoryMaxSize);
            _commandHistory.Remove(cmd);
            _commandHistory.Insert(0, cmd);
            if (_commandHistory.Count > cmdMax) _commandHistory.RemoveAt(cmdMax);

            consoleCommandComboBox.Items.Clear();
            foreach (var h in _commandHistory) consoleCommandComboBox.Items.Add(h);
            consoleCommandComboBox.Text = "";

            if (_processManager.IsRunning)
            {
                _processManager.SendCommand(cmd);
                AppendConsole($"> {cmd}", Color.FromArgb(100, 160, 220));
                Logger.Debug($"Console command sent: {cmd}");
            }
            else
            {
                AppendConsole("[ACEhole] Server is not running — command not sent.", Color.FromArgb(239, 83, 80));
            }
        }

        private void SendGlobalChat()
        {
            string message = consoleChatComboBox.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            int chatMax = Math.Max(1, _config.Ui.ChatHistoryMaxSize);
            _chatHistory.Remove(message);
            _chatHistory.Insert(0, message);
            if (_chatHistory.Count > chatMax) _chatHistory.RemoveAt(chatMax);

            consoleChatComboBox.Items.Clear();
            foreach (var h in _chatHistory) consoleChatComboBox.Items.Add(h);
            consoleChatComboBox.Text = "";

            if (!_processManager.IsRunning)
            {
                AppendConsole("[ACEhole] Server is not running — chat not sent.", Color.FromArgb(239, 83, 80));
                return;
            }

            // Placeholder: ACE global chat command TBD — update once console command is known
            string command = $"chat \"{message}\"";
            _processManager.SendCommand(command);
            AppendConsole($"> {command}", Color.FromArgb(100, 220, 160));
            Logger.Debug($"Global chat sent: {message}");
        }

        // ── MODs Tab ─────────────────────────────────────────────────────

        private void SetupModsTab()
        {
            var font = new Font("Segoe UI", 9f);

            tab_Mods = new TabPage { Text = "MODs", Name = "tab_Mods", Font = font };

            modsTabs = new TabControl { Dock = DockStyle.Fill, Font = font };
            tab_ModMaintenance   = new TabPage { Text = "Mod Maintenance",   Font = font };
            tab_ModNotifications = new TabPage { Text = "Mod Notifications", Font = font };

            // Mod Maintenance sub-tab (placeholder)
            var placeholderLabel = new Label
            {
                Text      = "MOD management coming in a future version.",
                AutoSize  = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock      = DockStyle.Fill,
                Font      = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(130, 130, 130)
            };
            tab_ModMaintenance.Controls.Add(placeholderLabel);

            // Mod Notifications sub-tab
            modNotificationsRTB = new RichTextBox
            {
                Dock        = DockStyle.Fill,
                BackColor   = Color.FromArgb(12, 12, 12),
                ForeColor   = Color.FromArgb(144, 238, 144),
                Font        = new Font("Consolas", 9f),
                ReadOnly    = true,
                ScrollBars  = RichTextBoxScrollBars.Vertical,
                WordWrap    = true,
                BorderStyle = BorderStyle.None
            };
            tab_ModNotifications.Controls.Add(modNotificationsRTB);

            modsTabs.Controls.Add(tab_ModMaintenance);
            modsTabs.Controls.Add(tab_ModNotifications);
            tab_Mods.Controls.Add(modsTabs);

            Main_Tabs.TabPages.Add(tab_Mods);

            var _ = tab_Mods.Handle;
        }

        private static bool IsModMessage(string line)
        {
            return line.Contains("[ACEmod]") ||
                   line.Contains("mod loaded") ||
                   line.Contains("mod unloaded") ||
                   line.Contains("Mod:");
        }

        private void AppendModNotification(string text)
        {
            if (modNotificationsRTB == null) return;
            if (modNotificationsRTB.InvokeRequired)
            {
                modNotificationsRTB.Invoke(new Action(() => AppendModNotification(text)));
                return;
            }
            bool atEnd = modNotificationsRTB.SelectionStart >= modNotificationsRTB.TextLength - 1;
            modNotificationsRTB.AppendText(text);
            if (atEnd) { modNotificationsRTB.SelectionStart = modNotificationsRTB.TextLength; modNotificationsRTB.ScrollToCaret(); }
        }

        // ── Server Configuration Tab (placeholder) ────────────────────────
        private void SetupServerConfigTab()
        {
            var font = new Font("Segoe UI", 9f);

            tab_ServerConfig = new TabPage { Text = "Server Configuration", Name = "tab_ServerConfig", Font = font };

            var serverConfigTabs = new TabControl { Dock = DockStyle.Fill, Font = font };
            var tab_Scheduled    = new TabPage { Text = "Scheduled",       Font = font };
            var tab_StaticOpts   = new TabPage { Text = "Static Options",  Font = font };
            var tab_DynamicOpts  = new TabPage { Text = "Dynamic Options", Font = font };

            var scheduledTabs = new TabControl { Dock = DockStyle.Fill, Font = font };
            var tab_SchedSub  = new TabPage { Text = "Scheduled", Font = font };
            var tab_Restarts  = new TabPage { Text = "Restarts",  Font = font };
            scheduledTabs.Controls.AddRange(new TabPage[] { tab_SchedSub, tab_Restarts });
            tab_Scheduled.Controls.Add(scheduledTabs);

            serverConfigTabs.Controls.AddRange(new TabPage[] { tab_Scheduled, tab_StaticOpts, tab_DynamicOpts });
            tab_ServerConfig.Controls.Add(serverConfigTabs);

            Main_Tabs.TabPages.Add(tab_ServerConfig);

            var _ = tab_ServerConfig.Handle;
        }

        // ── Toolbar Buttons ───────────────────────────────────────────────
        private void startServerButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_config.Server.AceExePath) ||
                !File.Exists(_config.Server.AceExePath))
            {
                MessageBox.Show(
                    "ACE.Server.exe was not found.\n\n" +
                    "Set the correct path in View → Settings → Server Settings → Paths.",
                    "ACE Server Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _ = _processManager.StartServerAsync();
        }

        private void stopServerButton_Click(object sender, EventArgs e)
        {
            _ = _processManager.StopServerAsync();
        }

        private void restartServerButton_Click(object sender, EventArgs e)
        {
            _ = _processManager.RestartServerAsync();
        }

        private void logFilesButton_Click(object sender, EventArgs e) => OpenLogViewer();

        // ── Menu Handlers ─────────────────────────────────────────────────
        private void exitMenuItem_Click(object sender, EventArgs e)     => DoExit();
        private void settingsMenuItem_Click(object sender, EventArgs e) => OpenSettings();
        private void logFilesMenuItem_Click(object sender, EventArgs e) => OpenLogViewer();

        private void releaseInfoMenuItem_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "docs", "README.txt");
            if (File.Exists(path)) System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path) { UseShellExecute = true });
            else MessageBox.Show("docs\\README.txt not found.", "Release Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void documentationMenuItem_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "docs", "ACEhole.html");
            if (File.Exists(path)) System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path) { UseShellExecute = true });
            else MessageBox.Show("docs\\ACEhole.html not found.", "Documentation", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void aboutMenuItem_Click(object sender, EventArgs e) => new AboutForm().ShowDialog(this);

        // ── Settings (modeless) ───────────────────────────────────────────
        private OptionsDialog _optionsDialog;

        private void OpenSettings()
        {
            if (_optionsDialog != null && !_optionsDialog.IsDisposed)
            {
                _optionsDialog.BringToFront();
                return;
            }
            _optionsDialog = new OptionsDialog(_config);
            _optionsDialog.SettingsSaved += OptionsDialog_SettingsSaved;
            _optionsDialog.Show();
        }

        private void OptionsDialog_SettingsSaved(object sender, EventArgs e)
        {
            Logger.SetConfig(_config);
            _processManager.UpdateConfig(_config);

            bool newDark = ThemeManager.ShouldUseDarkMode(_config.Ui.Theme);
            if (newDark != _isDark) { _isDark = newDark; ThemeManager.ApplyTheme(this, _isDark); ApplyConsoleColors(); }
            _statusUpdateTimer.Interval = Math.Max(5, _config.Ui.StatusUpdateIntervalSecs) * 1000;

            this.TopMost = _config.Ui.MainWindowAlwaysOnTop;

            if (_logViewerForm != null && !_logViewerForm.IsDisposed)
            {
                _logViewerForm.TopMost       = _config.LogViewer.AlwaysOnTop;
                _logViewerForm.ShowInTaskbar = _config.LogViewer.ShowInTaskbar;
            }

            _lastConsolePattern = null;
            _suppressedCount    = 0;
            _pendingNotice      = false;
        }

        // ── Log Viewer ────────────────────────────────────────────────────
        private void OpenLogViewer()
        {
            if (_logViewerForm != null && !_logViewerForm.IsDisposed) { _logViewerForm.BringToFront(); return; }
            _logViewerForm = new LogViewerForm(_config);
            _logViewerForm.Show();
        }

        // ── Tray ──────────────────────────────────────────────────────────
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show(); WindowState = FormWindowState.Normal; BringToFront(); Activate();
        }
        private void openTrayMenuItem_Click(object sender, EventArgs e)
        {
            Show(); WindowState = FormWindowState.Normal; BringToFront(); Activate();
        }
        private void exitTrayMenuItem_Click(object sender, EventArgs e) => DoExit();

        // ── Close / Exit ──────────────────────────────────────────────────
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_isExiting) { base.OnFormClosing(e); return; }

            if (_processManager != null && _processManager.IsRunning && _config.Ui.MinimizeToTray)
            {
                e.Cancel = true;
                ShowClosingDialog();
                return;
            }

            SaveAndClose();
            base.OnFormClosing(e);
        }

        private void ShowClosingDialog()
        {
            using var dlg = new Form
            {
                Text = "ACEhole", ClientSize = new Size(370, 130),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false, MinimizeBox = false,
                StartPosition = FormStartPosition.CenterParent,
                Font = new Font("Segoe UI", 9f)
            };
            var msg     = new Label { Text = "The ACE server is still running.", Location = new Point(16, 16), AutoSize = true };
            var hideBtn = new Button { Text = "Hide to Tray",         Location = new Point(16, 66),  Size = new Size(105, 28) };
            var stopBtn = new Button { Text = "Stop Server and Exit", Location = new Point(129, 66), Size = new Size(140, 28) };
            var canBtn  = new Button { Text = "Cancel",               Location = new Point(277, 66), Size = new Size(72, 28) };
            hideBtn.Click += (s, e) => { dlg.DialogResult = DialogResult.No;     dlg.Close(); };
            stopBtn.Click += (s, e) => { dlg.DialogResult = DialogResult.Yes;    dlg.Close(); };
            canBtn.Click  += (s, e) => { dlg.DialogResult = DialogResult.Cancel; dlg.Close(); };
            dlg.Controls.AddRange(new Control[] { msg, hideBtn, stopBtn, canBtn });
            ThemeManager.ApplyTheme(dlg, _isDark);

            var r = dlg.ShowDialog(this);
            if (r == DialogResult.Yes)
            {
                _ = Task.Run(async () =>
                {
                    await _processManager.StopServerAsync();
                    Invoke(new Action(() => { _isExiting = true; Application.Exit(); }));
                });
            }
            else if (r == DialogResult.No)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void DoExit()
        {
            _isExiting = true;
            SaveAndClose();
            Application.Exit();
        }

        private void SaveAndClose()
        {
            _config.Ui.WindowX       = this.Left;
            _config.Ui.WindowY       = this.Top;
            _config.Ui.WindowWidth   = this.Width;
            _config.Ui.WindowHeight  = this.Height;
            _config.Ui.LastActiveTab = Main_Tabs.SelectedTab?.Name ?? "";
            _config.Ui.CommandHistory = new List<string>(_commandHistory);
            _config.Ui.ChatHistory    = new List<string>(_chatHistory);
            ConfigManager.SaveConfig(_config);

            _statusUpdateTimer?.Stop();
            _procMonitorTimer?.Stop();
            _processManager?.Dispose();
            notifyIcon1.Visible = false;
            Logger.Info("ACEhole closing.");
        }

        // ── Window Position ───────────────────────────────────────────────
        private void RestoreWindowPosition()
        {
            if (_config.Ui.WindowWidth  > 0) this.Width  = _config.Ui.WindowWidth;
            if (_config.Ui.WindowHeight > 0) this.Height = _config.Ui.WindowHeight;

            if (_config.Ui.WindowX >= 0 && _config.Ui.WindowY >= 0)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location      = new Point(_config.Ui.WindowX, _config.Ui.WindowY);
                bool visible = false;
                foreach (var screen in Screen.AllScreens)
                    if (screen.WorkingArea.IntersectsWith(this.Bounds)) { visible = true; break; }
                if (!visible) this.CenterToScreen();
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────
        private static string FormatUptimeSpan(TimeSpan ts)
        {
            if (ts.TotalSeconds <= 0) return "--";
            if (ts.TotalDays  >= 1) return $"{(int)ts.TotalDays}d {ts.Hours:D2}h {ts.Minutes:D2}m";
            if (ts.TotalHours >= 1) return $"{(int)ts.TotalHours}h {ts.Minutes:D2}m";
            return $"{ts.Minutes}m {ts.Seconds:D2}s";
        }

        private void PlayNotificationSound(string soundFileName)
        {
            try
            {
                string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sounds", soundFileName);
                if (!File.Exists(soundPath)) { Logger.Warning($"Sound file not found: {soundPath}"); return; }
                using (var player = new System.Media.SoundPlayer(soundPath))
                    player.Play();
            }
            catch (Exception ex) { Logger.Warning($"Could not play sound '{soundFileName}': {ex.Message}"); }
        }

    }
}
