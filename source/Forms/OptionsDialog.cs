using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ACEhole
{
    public partial class OptionsDialog : Form
    {
        private AppConfig _config;
        public bool ConfigChanged { get; private set; }

        public event EventHandler SettingsSaved;

        private readonly List<(Button Swatch, Func<string> Get, Action<string> Set)> _colorSwatches
            = new List<(Button, Func<string>, Action<string>)>();

        private CheckBox _mainWindowAlwaysOnTopCB;
        private CheckBox _mainWindowShowInTaskbarCB;
        private CheckBox _logViewerAlwaysOnTopCB;
        private CheckBox _logViewerShowInTaskbarCB;
        private CheckBox _settingsAlwaysOnTopCB;
        private CheckBox _settingsShowInTaskbarCB;

        public OptionsDialog(AppConfig config)
        {
            InitializeComponent();
            _config = config;

            this.TopMost       = _config.Ui.SettingsAlwaysOnTop;
            this.ShowInTaskbar = _config.Ui.SettingsShowInTaskbar;

            LoadSettings();
            BuildConsoleColorsTab();
            BuildWindowsTab();
            LoadNotificationSettings();
        }

        // ── Load ──────────────────────────────────────────────────────────

        private void LoadSettings()
        {
            // Server Settings — Paths
            aceExeTextBox.Text  = _config.Server.AceExePath;
            logFileTextBox.Text = _config.Server.LogFilePath;

            // Server Settings — Cycles
            autoRestartCheckBox.Checked = _config.Server.AutoRestartOnCrash;
            restartDelayNumeric.Value   = _config.Server.RestartDelaySecs;

            // General
            themeCombo.SelectedIndex       = (int)_config.Ui.Theme;
            timeFormatCheckBox.Checked     = _config.Ui.Use24HourTime;
            minimizeToTrayCheckBox.Checked = _config.Ui.MinimizeToTray;
            autoUpdateCheckBox.Checked     = _config.Ui.AutoUpdateStatus;
            updateIntervalNumeric.Value    = Math.Max(5, Math.Min(3600, _config.Ui.StatusUpdateIntervalSecs));
            suppressRepeatCheckBox.Checked = _config.Ui.SuppressRepeatedConsoleLines;
            commandHistorySizeNumeric.Value = Math.Max(1, Math.Min(100, _config.Ui.CommandHistoryMaxSize));
            chatHistorySizeNumeric.Value    = Math.Max(1, Math.Min(100, _config.Ui.ChatHistoryMaxSize));

            // Log Settings
            maxLogSizeNumeric.Value    = Math.Max(10, Math.Min(500, _config.Logging.MaxLogFileSizeMB));
            maxLogFilesNumeric.Value   = Math.Max(1, Math.Min(1000, _config.Logging.RetainLogCount));
            debugLoggingCheckBox.Checked = _config.Logging.EnableDebugLogging;
            autoDeleteCheckBox.Checked   = _config.Logging.EnableAutoDelete;
        }

        // ── Save ──────────────────────────────────────────────────────────

        private void SaveSettings()
        {
            _config.Server.AceExePath  = aceExeTextBox.Text.Trim();
            _config.Server.LogFilePath = logFileTextBox.Text.Trim();

            _config.Server.AutoRestartOnCrash = autoRestartCheckBox.Checked;
            _config.Server.RestartDelaySecs   = (int)restartDelayNumeric.Value;

            _config.Ui.Theme                        = (ThemeMode)themeCombo.SelectedIndex;
            _config.Ui.Use24HourTime                = timeFormatCheckBox.Checked;
            _config.Ui.MinimizeToTray               = minimizeToTrayCheckBox.Checked;
            _config.Ui.AutoUpdateStatus             = autoUpdateCheckBox.Checked;
            _config.Ui.StatusUpdateIntervalSecs     = (int)updateIntervalNumeric.Value;
            _config.Ui.SuppressRepeatedConsoleLines = suppressRepeatCheckBox.Checked;
            _config.Ui.CommandHistoryMaxSize        = (int)commandHistorySizeNumeric.Value;
            _config.Ui.ChatHistoryMaxSize           = (int)chatHistorySizeNumeric.Value;

            _config.Ui.MainWindowAlwaysOnTop   = _mainWindowAlwaysOnTopCB.Checked;
            _config.Ui.MainWindowShowInTaskbar = _mainWindowShowInTaskbarCB.Checked;
            _config.LogViewer.AlwaysOnTop      = _logViewerAlwaysOnTopCB.Checked;
            _config.LogViewer.ShowInTaskbar    = _logViewerShowInTaskbarCB.Checked;
            _config.Ui.SettingsAlwaysOnTop     = _settingsAlwaysOnTopCB.Checked;
            _config.Ui.SettingsShowInTaskbar   = _settingsShowInTaskbarCB.Checked;

            this.TopMost       = _config.Ui.SettingsAlwaysOnTop;
            this.ShowInTaskbar = _config.Ui.SettingsShowInTaskbar;

            _config.Logging.MaxLogFileSizeMB   = (int)maxLogSizeNumeric.Value;
            _config.Logging.RetainLogCount     = (int)maxLogFilesNumeric.Value;
            _config.Logging.EnableDebugLogging = debugLoggingCheckBox.Checked;
            _config.Logging.EnableAutoDelete   = autoDeleteCheckBox.Checked;

            _config.Notifications.EnableTrayNotifications = enableTrayNotificationsCheckBox.Checked;
            _config.Notifications.TrayNotifyOnServerStart = showServerStartCheckBox.Checked;
            _config.Notifications.TrayNotifyOnServerStop  = showServerStopCheckBox.Checked;
            _config.Notifications.EnableSoundOnServerStart = enableSoundServerStartCheckBox.Checked;
            _config.Notifications.EnableSoundOnServerStop  = enableSoundServerStopCheckBox.Checked;

            ConfigManager.SaveConfig(_config);
            ConfigChanged = true;
        }

        // ── Browse buttons ────────────────────────────────────────────────

        private void browseAceExeButton_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog { Filter = "Executables|*.exe|All Files|*.*", Title = "Select ACE.Server.exe" };
            if (!string.IsNullOrEmpty(aceExeTextBox.Text) && File.Exists(aceExeTextBox.Text))
                dlg.InitialDirectory = Path.GetDirectoryName(aceExeTextBox.Text);
            if (dlg.ShowDialog() == DialogResult.OK) aceExeTextBox.Text = dlg.FileName;
        }

        private void browseLogFileButton_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog { Filter = "Log Files|*.log;*.txt|All Files|*.*", Title = "Select ACE Log File" };
            if (!string.IsNullOrEmpty(logFileTextBox.Text))
            {
                string dir = Path.GetDirectoryName(logFileTextBox.Text);
                if (Directory.Exists(dir)) dlg.InitialDirectory = dir;
            }
            if (dlg.ShowDialog() == DialogResult.OK) logFileTextBox.Text = dlg.FileName;
        }

        // ── Config import/export ──────────────────────────────────────────

        private void exportConfigButton_Click(object sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog { Filter = "JSON|*.json", FileName = "ACEhole_config_export.json",
                DefaultExt = "json", Title = "Export Config" };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            if (ConfigManager.ExportConfig(dlg.FileName, _config))
                MessageBox.Show("Config exported successfully.", "Export Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Export failed.", "Export Config", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void importConfigButton_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog { Filter = "JSON|*.json", Title = "Import Config" };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            var imported = ConfigManager.ImportConfig(dlg.FileName);
            if (imported == null) { MessageBox.Show("Import failed — invalid config file.", "Import Config", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            _config = imported;
            LoadSettings();
            MessageBox.Show("Config imported. Click Save to apply.", "Import Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── Dialog buttons ────────────────────────────────────────────────

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            ConfigChanged = true;
            SettingsSaved?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e) => this.Close();

        // ── Notification settings ─────────────────────────────────────────

        private void LoadNotificationSettings()
        {
            enableTrayNotificationsCheckBox.Checked = _config.Notifications.EnableTrayNotifications;
            showServerStartCheckBox.Checked         = _config.Notifications.TrayNotifyOnServerStart;
            showServerStopCheckBox.Checked          = _config.Notifications.TrayNotifyOnServerStop;
            enableSoundServerStartCheckBox.Checked  = _config.Notifications.EnableSoundOnServerStart;
            enableSoundServerStopCheckBox.Checked   = _config.Notifications.EnableSoundOnServerStop;
            UpdateNotificationControlsState();
        }

        private void enableTrayNotificationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNotificationControlsState();
        }

        private void UpdateNotificationControlsState()
        {
            bool enabled = enableTrayNotificationsCheckBox.Checked;
            showServerStartCheckBox.Enabled = enabled;
            showServerStopCheckBox.Enabled  = enabled;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_config.Ui.OptionsDialogX >= 0 && _config.Ui.OptionsDialogY >= 0)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location      = new System.Drawing.Point(_config.Ui.OptionsDialogX, _config.Ui.OptionsDialogY);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _config.Ui.OptionsDialogX = this.Left;
            _config.Ui.OptionsDialogY = this.Top;
            ConfigManager.SaveConfig(_config);
            base.OnFormClosing(e);
        }

        // ── Console Colors Tab ────────────────────────────────────────────

        private void BuildConsoleColorsTab()
        {
            _colorSwatches.Clear();
            logColorsSubTab.Controls.Clear();

            var font = new Font("Segoe UI", 9f);
            int leftX  = 10;
            int rightX = 220;
            int rowH   = 26;

            // ── Console Log section (left column) ─────────────────────────
            AddColorSection(logColorsSubTab, "Console / Server Log", leftX, 8, 180);
            int y = 32;
            AddColorRow(logColorsSubTab, "Joins / Leaves", leftX, y, font,
                () => _config.Colors.JoinLeaveColor, v => _config.Colors.JoinLeaveColor = v); y += rowH;
            AddColorRow(logColorsSubTab, "Chat",           leftX, y, font,
                () => _config.Colors.ChatColor,      v => _config.Colors.ChatColor = v); y += rowH;
            AddColorRow(logColorsSubTab, "Errors",         leftX, y, font,
                () => _config.Colors.ErrorColor,     v => _config.Colors.ErrorColor = v); y += rowH;
            AddColorRow(logColorsSubTab, "Warnings",       leftX, y, font,
                () => _config.Colors.WarningColor,   v => _config.Colors.WarningColor = v); y += rowH;
            AddColorRow(logColorsSubTab, "MODs",           leftX, y, font,
                () => _config.Colors.ModColor,       v => _config.Colors.ModColor = v); y += rowH;
            AddColorRow(logColorsSubTab, "Saves",          leftX, y, font,
                () => _config.Colors.SaveColor,      v => _config.Colors.SaveColor = v); y += rowH;
            AddColorRow(logColorsSubTab, "Network",        leftX, y, font,
                () => _config.Colors.NetworkColor,   v => _config.Colors.NetworkColor = v); y += rowH;
            AddColorRow(logColorsSubTab, "Default",        leftX, y, font,
                () => _config.Colors.DefaultColor,   v => _config.Colors.DefaultColor = v);

            // ── App Log section (right column) ────────────────────────────
            AddColorSection(logColorsSubTab, "Application Log", rightX, 8, 180);
            y = 32;
            AddColorRow(logColorsSubTab, "INFO",  rightX, y, font,
                () => _config.Colors.AppInfoColor,  v => _config.Colors.AppInfoColor = v); y += rowH;
            AddColorRow(logColorsSubTab, "WARN",  rightX, y, font,
                () => _config.Colors.AppWarnColor,  v => _config.Colors.AppWarnColor = v); y += rowH;
            AddColorRow(logColorsSubTab, "ERROR", rightX, y, font,
                () => _config.Colors.AppErrorColor, v => _config.Colors.AppErrorColor = v); y += rowH;
            AddColorRow(logColorsSubTab, "DEBUG", rightX, y, font,
                () => _config.Colors.AppDebugColor, v => _config.Colors.AppDebugColor = v);

            // ── Color Settings section ─────────────────────────────────────
            int bottomY = 305;
            AddColorSection(logColorsSubTab, "Color Settings", leftX, bottomY, 400);
            bottomY += 24;

            var enableConsoleCheck = new CheckBox
            {
                Text     = "Enable colors in console output",
                Location = new Point(leftX + 4, bottomY),
                Size     = new Size(400, 22),
                Font     = font,
                Checked  = _config.Colors.EnableConsoleColors
            };
            enableConsoleCheck.CheckedChanged += (s, e) => _config.Colors.EnableConsoleColors = enableConsoleCheck.Checked;
            logColorsSubTab.Controls.Add(enableConsoleCheck);
            bottomY += 24;

            var enableLogViewerCheck = new CheckBox
            {
                Text     = "Enable colors in Log Files viewer",
                Location = new Point(leftX + 4, bottomY),
                Size     = new Size(400, 22),
                Font     = font,
                Checked  = _config.Colors.EnableLogViewerColors
            };
            enableLogViewerCheck.CheckedChanged += (s, e) => _config.Colors.EnableLogViewerColors = enableLogViewerCheck.Checked;
            logColorsSubTab.Controls.Add(enableLogViewerCheck);

            bottomY += 30;
            var resetBtn = new Button
            {
                Text     = "Reset to Defaults",
                Location = new Point(leftX + 4, bottomY),
                Size     = new Size(130, 26),
                Font     = font
            };
            resetBtn.Click += (s, e) =>
            {
                _config.Colors = new ConsoleColors();
                BuildConsoleColorsTab();
            };
            logColorsSubTab.Controls.Add(resetBtn);
        }

        private void AddColorSection(Control parent, string title, int x, int y, int width)
        {
            var lbl = new Label
            {
                Text      = title,
                Location  = new Point(x, y + 2),
                Size      = new Size(width, 18),
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = SystemColors.ControlText
            };
            var sep = new Panel { Location = new Point(x, y + 20), Size = new Size(width, 1), BackColor = SystemColors.ControlDark };
            parent.Controls.Add(lbl);
            parent.Controls.Add(sep);
        }

        private void AddColorRow(Control parent, string label, int x, int y, Font font,
            Func<string> get, Action<string> set)
        {
            var lbl = new Label
            {
                Text     = label,
                Location = new Point(x, y + 3),
                AutoSize = true,
                Font     = font
            };

            var swatch = new Button
            {
                Location  = new Point(x + 130, y),
                Size      = new Size(60, 22),
                BackColor = HexToColor(get()),
                FlatStyle = FlatStyle.Flat,
                Text      = ""
            };
            swatch.FlatAppearance.BorderSize  = 1;
            swatch.FlatAppearance.BorderColor = Color.FromArgb(100, 100, 100);
            swatch.Click += (s, e) =>
            {
                using var dlg = new ColorDialog { Color = swatch.BackColor, FullOpen = true };
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    swatch.BackColor = dlg.Color;
                    set(ColorToHex(dlg.Color));
                }
            };

            _colorSwatches.Add((swatch, get, set));
            parent.Controls.Add(lbl);
            parent.Controls.Add(swatch);
        }

        private static Color HexToColor(string hex)
        {
            try { return ColorTranslator.FromHtml(hex.StartsWith("#") ? hex : "#" + hex); }
            catch { return Color.Gray; }
        }

        private static string ColorToHex(Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

        // ── Windows Tab ───────────────────────────────────────────────────

        private void BuildWindowsTab()
        {
            tabWindows.Controls.Clear();

            var innerTabs = new TabControl { Dock = DockStyle.Fill };
            var tabMain   = new TabPage { Text = "Main Window",  UseVisualStyleBackColor = true };
            var tabLog    = new TabPage { Text = "Log Viewer",   UseVisualStyleBackColor = true };
            var tabSets   = new TabPage { Text = "Settings",     UseVisualStyleBackColor = true };

            innerTabs.Controls.AddRange(new TabPage[] { tabMain, tabLog, tabSets });
            tabWindows.Controls.Add(innerTabs);

            BuildWindowsMainSubTab(tabMain);
            BuildWindowsLogViewerSubTab(tabLog);
            BuildWindowsSettingsSubTab(tabSets);
        }

        private void BuildWindowsMainSubTab(Control tab)
        {
            int x = 8, y = 8, w = 380;

            AddWinSectionHeader(tab, "Main Window", x, y);
            y += 20; AddWinSeparator(tab, x, y, w); y += 10;

            _mainWindowAlwaysOnTopCB   = AddWinCheckBox(tab, "Main window always on top",       x + 4, y); y += 22;
            _mainWindowShowInTaskbarCB = AddWinCheckBox(tab, "Show main window in the taskbar", x + 4, y); y += 32;

            _mainWindowAlwaysOnTopCB.Checked   = _config.Ui.MainWindowAlwaysOnTop;
            _mainWindowShowInTaskbarCB.Checked = _config.Ui.MainWindowShowInTaskbar;

            AddWinSectionHeader(tab, "Remembered Position", x, y);
            y += 20; AddWinSeparator(tab, x, y, w); y += 10;

            string pos = _config.Ui.WindowX >= 0
                ? $"X: {_config.Ui.WindowX},  Y: {_config.Ui.WindowY}  —  {_config.Ui.WindowWidth} × {_config.Ui.WindowHeight}"
                : "Default (centered on screen)";
            AddWinLabel(tab, pos, x + 4, y); y += 24;

            var resetBtn = new Button { Text = "Reset to Default", Location = new Point(x + 4, y), Size = new Size(130, 26), Font = this.Font };
            resetBtn.Click += (s, e) =>
            {
                _config.Ui.WindowX = -1; _config.Ui.WindowY = -1;
                _config.Ui.WindowWidth = 1100; _config.Ui.WindowHeight = 700;
                MessageBox.Show("Main window position will reset on next launch.", "Position Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            tab.Controls.Add(resetBtn);
        }

        private void BuildWindowsLogViewerSubTab(Control tab)
        {
            int x = 8, y = 8, w = 380;

            AddWinSectionHeader(tab, "Log Viewer Window", x, y);
            y += 20; AddWinSeparator(tab, x, y, w); y += 10;

            _logViewerAlwaysOnTopCB   = AddWinCheckBox(tab, "Log viewer always on top",         x + 4, y); y += 22;
            _logViewerShowInTaskbarCB = AddWinCheckBox(tab, "Show log viewer in the taskbar",   x + 4, y); y += 32;

            _logViewerAlwaysOnTopCB.Checked   = _config.LogViewer.AlwaysOnTop;
            _logViewerShowInTaskbarCB.Checked = _config.LogViewer.ShowInTaskbar;

            AddWinSectionHeader(tab, "Remembered Position", x, y);
            y += 20; AddWinSeparator(tab, x, y, w); y += 10;

            string pos = _config.LogViewer.WindowX >= 0
                ? $"X: {_config.LogViewer.WindowX},  Y: {_config.LogViewer.WindowY}"
                : "Default (Windows default location)";
            AddWinLabel(tab, pos, x + 4, y); y += 24;

            var resetBtn = new Button { Text = "Reset to Default", Location = new Point(x + 4, y), Size = new Size(130, 26), Font = this.Font };
            resetBtn.Click += (s, e) =>
            {
                _config.LogViewer.WindowX = -1; _config.LogViewer.WindowY = -1;
                MessageBox.Show("Log Viewer position will reset on next open.", "Position Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            tab.Controls.Add(resetBtn);
        }

        private void BuildWindowsSettingsSubTab(Control tab)
        {
            int x = 8, y = 8, w = 380;

            AddWinSectionHeader(tab, "Settings Window", x, y);
            y += 20; AddWinSeparator(tab, x, y, w); y += 10;

            _settingsAlwaysOnTopCB   = AddWinCheckBox(tab, "Settings window always on top",       x + 4, y); y += 22;
            _settingsShowInTaskbarCB = AddWinCheckBox(tab, "Show Settings window in the taskbar", x + 4, y); y += 32;

            _settingsAlwaysOnTopCB.Checked   = _config.Ui.SettingsAlwaysOnTop;
            _settingsShowInTaskbarCB.Checked = _config.Ui.SettingsShowInTaskbar;

            AddWinSectionHeader(tab, "Remembered Position", x, y);
            y += 20; AddWinSeparator(tab, x, y, w); y += 10;

            string pos = _config.Ui.OptionsDialogX >= 0
                ? $"X: {_config.Ui.OptionsDialogX},  Y: {_config.Ui.OptionsDialogY}"
                : "Default (centered on parent)";
            AddWinLabel(tab, pos, x + 4, y); y += 24;

            var resetBtn = new Button { Text = "Reset to Default", Location = new Point(x + 4, y), Size = new Size(130, 26), Font = this.Font };
            resetBtn.Click += (s, e) =>
            {
                _config.Ui.OptionsDialogX = -1; _config.Ui.OptionsDialogY = -1;
                MessageBox.Show("Settings window position will reset on next open.", "Position Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            tab.Controls.Add(resetBtn);
        }

        private void AddWinSectionHeader(Control parent, string text, int x, int y)
        {
            parent.Controls.Add(new Label
            {
                Text     = text,
                Location = new Point(x, y),
                Size     = new Size(380, 16),
                Font     = new Font(this.Font, FontStyle.Bold),
                AutoSize = false
            });
        }

        private void AddWinSeparator(Control parent, int x, int y, int width)
        {
            parent.Controls.Add(new Panel
            {
                Location  = new Point(x, y),
                Size      = new Size(width, 1),
                BackColor = SystemColors.ControlDark
            });
        }

        private CheckBox AddWinCheckBox(Control parent, string text, int x, int y)
        {
            var cb = new CheckBox
            {
                Text     = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font     = this.Font,
                UseVisualStyleBackColor = true
            };
            parent.Controls.Add(cb);
            return cb;
        }

        private void AddWinLabel(Control parent, string text, int x, int y)
        {
            parent.Controls.Add(new Label
            {
                Text      = text,
                Location  = new Point(x, y),
                AutoSize  = true,
                Font      = new Font("Consolas", 8.5f),
                ForeColor = SystemColors.GrayText
            });
        }
    }
}
