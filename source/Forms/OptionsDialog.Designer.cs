namespace ACEhole
{
    partial class OptionsDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ── Tab control ─────────────────────────────────────────────
            this.optionsTabs      = new System.Windows.Forms.TabControl();
            this.tabServer        = new System.Windows.Forms.TabPage();
            this.tabGeneral       = new System.Windows.Forms.TabPage();
            this.tabLog           = new System.Windows.Forms.TabPage();
            this.tabNotifications = new System.Windows.Forms.TabPage();
            this.tabWindows       = new System.Windows.Forms.TabPage();

            // Inner TabControl for Server Settings tab
            this.serverTabControl = new System.Windows.Forms.TabControl();
            this.pathsSubTab      = new System.Windows.Forms.TabPage();
            this.cyclesSubTab     = new System.Windows.Forms.TabPage();

            // Inner TabControl for UI Settings tab
            this.uiTabControl    = new System.Windows.Forms.TabControl();
            this.uiGeneralSubTab = new System.Windows.Forms.TabPage();

            // Inner TabControl for Logs tab
            this.logsTabControl  = new System.Windows.Forms.TabControl();
            this.logFilesSubTab  = new System.Windows.Forms.TabPage();
            this.logColorsSubTab = new System.Windows.Forms.TabPage();

            // Inner TabControl for Notifications tab
            this.notificationsTabControl         = new System.Windows.Forms.TabControl();
            this.traySubTab                      = new System.Windows.Forms.TabPage();
            this.soundSubTab                     = new System.Windows.Forms.TabPage();
            this.enableTrayNotificationsCheckBox = new System.Windows.Forms.CheckBox();
            this.trayNotificationsGroupBox       = new System.Windows.Forms.GroupBox();
            this.showServerStartCheckBox         = new System.Windows.Forms.CheckBox();
            this.showServerStopCheckBox          = new System.Windows.Forms.CheckBox();
            this.soundNotificationsGroupBox      = new System.Windows.Forms.GroupBox();
            this.enableSoundServerStartCheckBox  = new System.Windows.Forms.CheckBox();
            this.enableSoundServerStopCheckBox   = new System.Windows.Forms.CheckBox();

            // ── GroupBoxes ──────────────────────────────────────────────
            this.serverPathsGB = new System.Windows.Forms.GroupBox();
            this.cyclesGB      = new System.Windows.Forms.GroupBox();
            this.generalGB     = new System.Windows.Forms.GroupBox();
            this.logSettingsGB = new System.Windows.Forms.GroupBox();

            // ── Paths group controls ────────────────────────────────────
            this.lbl_AceExe         = new System.Windows.Forms.Label();
            this.aceExeTextBox      = new System.Windows.Forms.TextBox();
            this.browseAceExeButton = new System.Windows.Forms.Button();
            this.lbl_LogFile        = new System.Windows.Forms.Label();
            this.logFileTextBox     = new System.Windows.Forms.TextBox();
            this.browseLogFileButton = new System.Windows.Forms.Button();

            // ── Cycles group controls ───────────────────────────────────
            this.autoRestartCheckBox = new System.Windows.Forms.CheckBox();
            this.lbl_RestartDelay    = new System.Windows.Forms.Label();
            this.restartDelayNumeric = new System.Windows.Forms.NumericUpDown();

            // ── History size controls ───────────────────────────────────
            this.lbl_CommandHistorySize  = new System.Windows.Forms.Label();
            this.commandHistorySizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.lbl_ChatHistorySize     = new System.Windows.Forms.Label();
            this.chatHistorySizeNumeric  = new System.Windows.Forms.NumericUpDown();

            // ── General tab controls ────────────────────────────────────
            this.lbl_Theme              = new System.Windows.Forms.Label();
            this.themeCombo             = new System.Windows.Forms.ComboBox();
            this.timeFormatCheckBox     = new System.Windows.Forms.CheckBox();
            this.minimizeToTrayCheckBox = new System.Windows.Forms.CheckBox();
            this.autoUpdateCheckBox     = new System.Windows.Forms.CheckBox();
            this.lbl_UpdateSec          = new System.Windows.Forms.Label();
            this.updateIntervalNumeric  = new System.Windows.Forms.NumericUpDown();
            this.suppressRepeatCheckBox = new System.Windows.Forms.CheckBox();
            this.exportConfigButton     = new System.Windows.Forms.Button();
            this.importConfigButton     = new System.Windows.Forms.Button();

            // ── Log tab controls ────────────────────────────────────────
            this.lbl_MaxLogSize       = new System.Windows.Forms.Label();
            this.maxLogSizeNumeric    = new System.Windows.Forms.NumericUpDown();
            this.lbl_RetainFiles      = new System.Windows.Forms.Label();
            this.maxLogFilesNumeric   = new System.Windows.Forms.NumericUpDown();
            this.debugLoggingCheckBox = new System.Windows.Forms.CheckBox();
            this.autoDeleteCheckBox   = new System.Windows.Forms.CheckBox();

            // ── Dialog buttons ──────────────────────────────────────────
            this.saveButton   = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.restartDelayNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updateIntervalNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxLogSizeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxLogFilesNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandHistorySizeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chatHistorySizeNumeric)).BeginInit();
            this.optionsTabs.SuspendLayout();
            this.tabServer.SuspendLayout();
            this.serverTabControl.SuspendLayout();
            this.pathsSubTab.SuspendLayout();
            this.cyclesSubTab.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.uiTabControl.SuspendLayout();
            this.uiGeneralSubTab.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.logsTabControl.SuspendLayout();
            this.logFilesSubTab.SuspendLayout();
            this.tabNotifications.SuspendLayout();
            this.notificationsTabControl.SuspendLayout();
            this.traySubTab.SuspendLayout();
            this.soundSubTab.SuspendLayout();
            this.trayNotificationsGroupBox.SuspendLayout();
            this.soundNotificationsGroupBox.SuspendLayout();
            this.SuspendLayout();

            var font = new System.Drawing.Font("Segoe UI", 9f);

            // ══════════════════════════════════════════════════════════
            // SERVER PATHS GroupBox
            // ══════════════════════════════════════════════════════════
            this.serverPathsGB.Text     = "Paths";
            this.serverPathsGB.Font     = font;
            this.serverPathsGB.Location = new System.Drawing.Point(6, 6);
            this.serverPathsGB.Size     = new System.Drawing.Size(616, 98);

            this.lbl_AceExe.Text      = "ACE Executable:";
            this.lbl_AceExe.Location  = new System.Drawing.Point(8, 24);
            this.lbl_AceExe.Size      = new System.Drawing.Size(108, 20);
            this.lbl_AceExe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.aceExeTextBox.Location = new System.Drawing.Point(118, 22);
            this.aceExeTextBox.Size     = new System.Drawing.Size(410, 22);

            this.browseAceExeButton.Text     = "Browse...";
            this.browseAceExeButton.Location = new System.Drawing.Point(532, 21);
            this.browseAceExeButton.Size     = new System.Drawing.Size(76, 24);
            this.browseAceExeButton.Click   += new System.EventHandler(this.browseAceExeButton_Click);

            this.lbl_LogFile.Text      = "Log File Path:";
            this.lbl_LogFile.Location  = new System.Drawing.Point(8, 56);
            this.lbl_LogFile.Size      = new System.Drawing.Size(108, 20);
            this.lbl_LogFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.logFileTextBox.Location = new System.Drawing.Point(118, 54);
            this.logFileTextBox.Size     = new System.Drawing.Size(410, 22);

            this.browseLogFileButton.Text     = "Browse...";
            this.browseLogFileButton.Location = new System.Drawing.Point(532, 53);
            this.browseLogFileButton.Size     = new System.Drawing.Size(76, 24);
            this.browseLogFileButton.Click   += new System.EventHandler(this.browseLogFileButton_Click);

            this.serverPathsGB.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lbl_AceExe, this.aceExeTextBox, this.browseAceExeButton,
                this.lbl_LogFile, this.logFileTextBox, this.browseLogFileButton });

            // ══════════════════════════════════════════════════════════
            // CYCLES GroupBox
            // ══════════════════════════════════════════════════════════
            this.cyclesGB.Text     = "Crash Restart Settings";
            this.cyclesGB.Font     = font;
            this.cyclesGB.Location = new System.Drawing.Point(6, 6);
            this.cyclesGB.Size     = new System.Drawing.Size(616, 86);

            this.autoRestartCheckBox.Text     = "Auto-restart on crash";
            this.autoRestartCheckBox.Location = new System.Drawing.Point(8, 24);
            this.autoRestartCheckBox.Size     = new System.Drawing.Size(260, 22);

            this.lbl_RestartDelay.Text      = "Crash restart delay (sec):";
            this.lbl_RestartDelay.Location  = new System.Drawing.Point(8, 56);
            this.lbl_RestartDelay.Size      = new System.Drawing.Size(164, 20);
            this.lbl_RestartDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.restartDelayNumeric.Location = new System.Drawing.Point(176, 54);
            this.restartDelayNumeric.Size     = new System.Drawing.Size(65, 22);
            this.restartDelayNumeric.Minimum  = 1; this.restartDelayNumeric.Maximum = 300;

            this.cyclesGB.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.autoRestartCheckBox, this.lbl_RestartDelay, this.restartDelayNumeric });

            // ── Server inner TabControl ────────────────────────────────
            this.pathsSubTab.Text      = "Paths";
            this.pathsSubTab.Font      = font;
            this.pathsSubTab.Padding   = new System.Windows.Forms.Padding(4);
            this.pathsSubTab.AutoScroll = true;
            this.pathsSubTab.Controls.Add(this.serverPathsGB);

            this.cyclesSubTab.Text    = "Cycles";
            this.cyclesSubTab.Font    = font;
            this.cyclesSubTab.Padding = new System.Windows.Forms.Padding(4);
            this.cyclesSubTab.Controls.Add(this.cyclesGB);

            this.serverTabControl.Controls.AddRange(new System.Windows.Forms.TabPage[] {
                this.pathsSubTab, this.cyclesSubTab });
            this.serverTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverTabControl.Font = font;

            this.tabServer.Text    = "Server Settings";
            this.tabServer.Font    = font;
            this.tabServer.Padding = new System.Windows.Forms.Padding(3);
            this.tabServer.Controls.Add(this.serverTabControl);

            // ══════════════════════════════════════════════════════════
            // GENERAL Tab
            // ══════════════════════════════════════════════════════════
            this.generalGB.Text     = "UI Settings";
            this.generalGB.Font     = font;
            this.generalGB.Location = new System.Drawing.Point(6, 6);
            this.generalGB.Size     = new System.Drawing.Size(646, 290);

            this.lbl_Theme.Text      = "Theme:";
            this.lbl_Theme.Location  = new System.Drawing.Point(8, 28);
            this.lbl_Theme.Size      = new System.Drawing.Size(80, 22);
            this.lbl_Theme.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.themeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.themeCombo.Items.AddRange(new object[] { "Light", "Dark", "System" });
            this.themeCombo.Location = new System.Drawing.Point(90, 26);
            this.themeCombo.Size     = new System.Drawing.Size(160, 24);

            this.timeFormatCheckBox.Text     = "Use 24-hour time format";
            this.timeFormatCheckBox.Location = new System.Drawing.Point(8, 62);
            this.timeFormatCheckBox.Size     = new System.Drawing.Size(250, 22);

            this.minimizeToTrayCheckBox.Text     = "Minimize to tray on close  (shows close prompt when server is running)";
            this.minimizeToTrayCheckBox.Location = new System.Drawing.Point(8, 90);
            this.minimizeToTrayCheckBox.Size     = new System.Drawing.Size(500, 22);

            this.autoUpdateCheckBox.Text     = "Auto-refresh Server Status tab";
            this.autoUpdateCheckBox.Location = new System.Drawing.Point(8, 120);
            this.autoUpdateCheckBox.Size     = new System.Drawing.Size(230, 22);

            this.lbl_UpdateSec.Text      = "every (sec):";
            this.lbl_UpdateSec.Location  = new System.Drawing.Point(246, 122);
            this.lbl_UpdateSec.Size      = new System.Drawing.Size(80, 20);
            this.lbl_UpdateSec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.updateIntervalNumeric.Location = new System.Drawing.Point(328, 120);
            this.updateIntervalNumeric.Size     = new System.Drawing.Size(70, 22);
            this.updateIntervalNumeric.Minimum  = 5; this.updateIntervalNumeric.Maximum = 3600;

            this.suppressRepeatCheckBox.Text     = "Suppress repeated console lines  (collapses flood messages into a count)";
            this.suppressRepeatCheckBox.Location = new System.Drawing.Point(8, 150);
            this.suppressRepeatCheckBox.Size     = new System.Drawing.Size(530, 22);

            var lbl_ConfigBackup = new System.Windows.Forms.Label
            {
                Text = "Config Backup:", Location = new System.Drawing.Point(8, 186),
                Size = new System.Drawing.Size(100, 22), TextAlign = System.Drawing.ContentAlignment.MiddleLeft, Font = font
            };

            this.exportConfigButton.Text     = "Export Config...";
            this.exportConfigButton.Location = new System.Drawing.Point(110, 184);
            this.exportConfigButton.Size     = new System.Drawing.Size(120, 26);
            this.exportConfigButton.Click   += new System.EventHandler(this.exportConfigButton_Click);

            this.importConfigButton.Text     = "Import Config...";
            this.importConfigButton.Location = new System.Drawing.Point(238, 184);
            this.importConfigButton.Size     = new System.Drawing.Size(120, 26);
            this.importConfigButton.Click   += new System.EventHandler(this.importConfigButton_Click);

            this.lbl_CommandHistorySize.Text      = "Console Cmd history (max):";
            this.lbl_CommandHistorySize.Location  = new System.Drawing.Point(8, 222);
            this.lbl_CommandHistorySize.Size      = new System.Drawing.Size(165, 20);
            this.lbl_CommandHistorySize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.commandHistorySizeNumeric.Location = new System.Drawing.Point(176, 220);
            this.commandHistorySizeNumeric.Size     = new System.Drawing.Size(60, 22);
            this.commandHistorySizeNumeric.Minimum  = 1; this.commandHistorySizeNumeric.Maximum = 100;

            this.lbl_ChatHistorySize.Text      = "Chat history (max):";
            this.lbl_ChatHistorySize.Location  = new System.Drawing.Point(252, 222);
            this.lbl_ChatHistorySize.Size      = new System.Drawing.Size(120, 20);
            this.lbl_ChatHistorySize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.chatHistorySizeNumeric.Location = new System.Drawing.Point(376, 220);
            this.chatHistorySizeNumeric.Size     = new System.Drawing.Size(60, 22);
            this.chatHistorySizeNumeric.Minimum  = 1; this.chatHistorySizeNumeric.Maximum = 100;

            this.generalGB.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lbl_Theme, this.themeCombo,
                this.timeFormatCheckBox, this.minimizeToTrayCheckBox,
                this.autoUpdateCheckBox, this.lbl_UpdateSec, this.updateIntervalNumeric,
                this.suppressRepeatCheckBox,
                lbl_ConfigBackup, this.exportConfigButton, this.importConfigButton,
                this.lbl_CommandHistorySize, this.commandHistorySizeNumeric,
                this.lbl_ChatHistorySize, this.chatHistorySizeNumeric });

            // ── UI Settings inner TabControl ───────────────────────────
            this.uiGeneralSubTab.Text    = "General";
            this.uiGeneralSubTab.Font    = font;
            this.uiGeneralSubTab.Padding = new System.Windows.Forms.Padding(4);
            this.uiGeneralSubTab.Controls.Add(this.generalGB);

            this.uiTabControl.Controls.AddRange(new System.Windows.Forms.TabPage[] {
                this.uiGeneralSubTab, this.tabWindows });
            this.uiTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTabControl.Font = font;

            this.tabGeneral.Text    = "UI Settings";
            this.tabGeneral.Font    = font;
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Controls.Add(this.uiTabControl);

            // ══════════════════════════════════════════════════════════
            // LOG SETTINGS Tab
            // ══════════════════════════════════════════════════════════
            this.logSettingsGB.Text     = "Log File Settings";
            this.logSettingsGB.Font     = font;
            this.logSettingsGB.Location = new System.Drawing.Point(6, 6);
            this.logSettingsGB.Size     = new System.Drawing.Size(646, 170);

            this.lbl_MaxLogSize.Text      = "Max log file size (MB):";
            this.lbl_MaxLogSize.Location  = new System.Drawing.Point(8, 28);
            this.lbl_MaxLogSize.Size      = new System.Drawing.Size(160, 22);
            this.lbl_MaxLogSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.maxLogSizeNumeric.Location = new System.Drawing.Point(172, 26);
            this.maxLogSizeNumeric.Size     = new System.Drawing.Size(80, 22);
            this.maxLogSizeNumeric.Minimum  = 10; this.maxLogSizeNumeric.Maximum = 500;

            this.lbl_RetainFiles.Text      = "Log files to retain (per type):";
            this.lbl_RetainFiles.Location  = new System.Drawing.Point(8, 62);
            this.lbl_RetainFiles.Size      = new System.Drawing.Size(200, 22);
            this.lbl_RetainFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.maxLogFilesNumeric.Location = new System.Drawing.Point(212, 60);
            this.maxLogFilesNumeric.Size     = new System.Drawing.Size(80, 22);
            this.maxLogFilesNumeric.Minimum  = 1; this.maxLogFilesNumeric.Maximum = 1000;

            this.debugLoggingCheckBox.Text     = "Enable debug logging in App log";
            this.debugLoggingCheckBox.Location = new System.Drawing.Point(8, 96);
            this.debugLoggingCheckBox.Size     = new System.Drawing.Size(300, 22);

            this.autoDeleteCheckBox.Text     = "Auto-delete old log files (keep only the newest N files per type)";
            this.autoDeleteCheckBox.Location = new System.Drawing.Point(8, 124);
            this.autoDeleteCheckBox.Size     = new System.Drawing.Size(500, 22);

            this.logSettingsGB.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lbl_MaxLogSize, this.maxLogSizeNumeric,
                this.lbl_RetainFiles, this.maxLogFilesNumeric,
                this.debugLoggingCheckBox, this.autoDeleteCheckBox });

            this.logsTabControl.Controls.AddRange(new System.Windows.Forms.TabPage[] {
                this.logFilesSubTab, this.logColorsSubTab });
            this.logsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsTabControl.Font = font;

            this.logFilesSubTab.Text    = "Log Files";
            this.logFilesSubTab.Font    = font;
            this.logFilesSubTab.Padding = new System.Windows.Forms.Padding(4);
            this.logFilesSubTab.Controls.Add(this.logSettingsGB);

            this.logColorsSubTab.Text    = "Log Colors";
            this.logColorsSubTab.Font    = font;
            this.logColorsSubTab.Padding = new System.Windows.Forms.Padding(4);

            this.tabLog.Text    = "Logs";
            this.tabLog.Font    = font;
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Controls.Add(this.logsTabControl);

            // ── Notifications tab ─────────────────────────────────────
            this.enableTrayNotificationsCheckBox.Text      = "Enable tray notifications";
            this.enableTrayNotificationsCheckBox.Font      = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            this.enableTrayNotificationsCheckBox.Location  = new System.Drawing.Point(15, 15);
            this.enableTrayNotificationsCheckBox.AutoSize  = true;
            this.enableTrayNotificationsCheckBox.CheckedChanged += new System.EventHandler(this.enableTrayNotificationsCheckBox_CheckedChanged);

            this.showServerStartCheckBox.Text     = "Show notification on server start";
            this.showServerStartCheckBox.Location = new System.Drawing.Point(15, 25);
            this.showServerStartCheckBox.AutoSize = true;

            this.showServerStopCheckBox.Text     = "Show notification on server stop";
            this.showServerStopCheckBox.Location = new System.Drawing.Point(15, 48);
            this.showServerStopCheckBox.AutoSize = true;

            this.trayNotificationsGroupBox.Text     = "Tray Notification Settings";
            this.trayNotificationsGroupBox.Location = new System.Drawing.Point(6, 45);
            this.trayNotificationsGroupBox.Size     = new System.Drawing.Size(500, 100);
            this.trayNotificationsGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.showServerStartCheckBox, this.showServerStopCheckBox });

            this.traySubTab.Text                   = "Tray";
            this.traySubTab.UseVisualStyleBackColor = true;
            this.traySubTab.Controls.Add(this.enableTrayNotificationsCheckBox);
            this.traySubTab.Controls.Add(this.trayNotificationsGroupBox);

            this.enableSoundServerStartCheckBox.Text     = "Play sound on server start  (server_start.wav)";
            this.enableSoundServerStartCheckBox.Location = new System.Drawing.Point(15, 25);
            this.enableSoundServerStartCheckBox.AutoSize = true;

            this.enableSoundServerStopCheckBox.Text     = "Play sound on server stop  (server_stop.wav)";
            this.enableSoundServerStopCheckBox.Location = new System.Drawing.Point(15, 48);
            this.enableSoundServerStopCheckBox.AutoSize = true;

            this.soundNotificationsGroupBox.Text     = "Sound Notification Settings";
            this.soundNotificationsGroupBox.Location = new System.Drawing.Point(6, 6);
            this.soundNotificationsGroupBox.Size     = new System.Drawing.Size(500, 100);
            this.soundNotificationsGroupBox.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.enableSoundServerStartCheckBox, this.enableSoundServerStopCheckBox });

            this.soundSubTab.Text                   = "Sound";
            this.soundSubTab.UseVisualStyleBackColor = true;
            this.soundSubTab.Controls.Add(this.soundNotificationsGroupBox);

            this.notificationsTabControl.Controls.AddRange(new System.Windows.Forms.TabPage[] {
                this.traySubTab, this.soundSubTab });
            this.notificationsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notificationsTabControl.Font = font;

            this.tabNotifications.Text    = "Notifications";
            this.tabNotifications.Font    = font;
            this.tabNotifications.Padding = new System.Windows.Forms.Padding(3);
            this.tabNotifications.Controls.Add(this.notificationsTabControl);

            // ── Windows / main tab ────────────────────────────────────
            this.tabWindows.Text    = "Windows";
            this.tabWindows.Font    = font;
            this.tabWindows.Padding = new System.Windows.Forms.Padding(4);

            this.optionsTabs.Controls.AddRange(new System.Windows.Forms.TabPage[] {
                this.tabServer, this.tabGeneral, this.tabLog, this.tabNotifications });
            this.optionsTabs.Font     = font;
            this.optionsTabs.Location = new System.Drawing.Point(8, 8);
            this.optionsTabs.Size     = new System.Drawing.Size(664, 558);

            // ── Save / Cancel ──────────────────────────────────────────
            this.saveButton.Text     = "Save";
            this.saveButton.Font     = font;
            this.saveButton.Location = new System.Drawing.Point(510, 572);
            this.saveButton.Size     = new System.Drawing.Size(80, 30);
            this.saveButton.Click   += new System.EventHandler(this.saveButton_Click);

            this.cancelButton.Text     = "Cancel";
            this.cancelButton.Font     = font;
            this.cancelButton.Location = new System.Drawing.Point(596, 572);
            this.cancelButton.Size     = new System.Drawing.Size(80, 30);
            this.cancelButton.Click   += new System.EventHandler(this.cancelButton_Click);

            // ── OptionsDialog ──────────────────────────────────────────
            this.ClientSize      = new System.Drawing.Size(684, 612);
            this.Text            = "ACEhole - Settings";
            this.Font            = font;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.AcceptButton    = this.saveButton;
            this.CancelButton    = this.cancelButton;

            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.optionsTabs, this.saveButton, this.cancelButton });

            ((System.ComponentModel.ISupportInitialize)(this.restartDelayNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updateIntervalNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxLogSizeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxLogFilesNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandHistorySizeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chatHistorySizeNumeric)).EndInit();
            this.optionsTabs.ResumeLayout(false);
            this.tabServer.ResumeLayout(false);
            this.serverTabControl.ResumeLayout(false);
            this.pathsSubTab.ResumeLayout(false);
            this.cyclesSubTab.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.uiTabControl.ResumeLayout(false);
            this.uiGeneralSubTab.ResumeLayout(false);
            this.tabLog.ResumeLayout(false);
            this.logsTabControl.ResumeLayout(false);
            this.logFilesSubTab.ResumeLayout(false);
            this.tabNotifications.ResumeLayout(false);
            this.notificationsTabControl.ResumeLayout(false);
            this.traySubTab.ResumeLayout(false);
            this.traySubTab.PerformLayout();
            this.trayNotificationsGroupBox.ResumeLayout(false);
            this.trayNotificationsGroupBox.PerformLayout();
            this.soundSubTab.ResumeLayout(false);
            this.soundNotificationsGroupBox.ResumeLayout(false);
            this.soundNotificationsGroupBox.PerformLayout();
            this.ResumeLayout(false);
        }

        // ── Field declarations ─────────────────────────────────────────
        private System.Windows.Forms.TabControl  optionsTabs;
        private System.Windows.Forms.TabPage     tabServer;
        private System.Windows.Forms.TabControl  serverTabControl;
        private System.Windows.Forms.TabPage     pathsSubTab;
        private System.Windows.Forms.TabPage     cyclesSubTab;
        private System.Windows.Forms.TabPage     tabGeneral;
        private System.Windows.Forms.TabControl  uiTabControl;
        private System.Windows.Forms.TabPage     uiGeneralSubTab;
        private System.Windows.Forms.TabPage     tabLog;
        private System.Windows.Forms.TabPage     tabNotifications;
        private System.Windows.Forms.TabControl  logsTabControl;
        private System.Windows.Forms.TabPage     logFilesSubTab;
        private System.Windows.Forms.TabPage     logColorsSubTab;
        private System.Windows.Forms.TabControl  notificationsTabControl;
        private System.Windows.Forms.TabPage     traySubTab;
        private System.Windows.Forms.TabPage     soundSubTab;
        private System.Windows.Forms.CheckBox    enableTrayNotificationsCheckBox;
        private System.Windows.Forms.GroupBox    trayNotificationsGroupBox;
        private System.Windows.Forms.CheckBox    showServerStartCheckBox;
        private System.Windows.Forms.CheckBox    showServerStopCheckBox;
        private System.Windows.Forms.GroupBox    soundNotificationsGroupBox;
        private System.Windows.Forms.CheckBox    enableSoundServerStartCheckBox;
        private System.Windows.Forms.CheckBox    enableSoundServerStopCheckBox;
        private System.Windows.Forms.TabPage     tabWindows;
        private System.Windows.Forms.Button      saveButton;
        private System.Windows.Forms.Button      cancelButton;

        private System.Windows.Forms.GroupBox  serverPathsGB;
        private System.Windows.Forms.Label     lbl_AceExe;
        private System.Windows.Forms.TextBox   aceExeTextBox;
        private System.Windows.Forms.Button    browseAceExeButton;
        private System.Windows.Forms.Label     lbl_LogFile;
        private System.Windows.Forms.TextBox   logFileTextBox;
        private System.Windows.Forms.Button    browseLogFileButton;

        private System.Windows.Forms.GroupBox      cyclesGB;
        private System.Windows.Forms.CheckBox      autoRestartCheckBox;
        private System.Windows.Forms.Label         lbl_RestartDelay;
        private System.Windows.Forms.NumericUpDown restartDelayNumeric;

        private System.Windows.Forms.GroupBox      generalGB;
        private System.Windows.Forms.Label         lbl_Theme;
        private System.Windows.Forms.ComboBox      themeCombo;
        private System.Windows.Forms.CheckBox      timeFormatCheckBox;
        private System.Windows.Forms.CheckBox      minimizeToTrayCheckBox;
        private System.Windows.Forms.CheckBox      autoUpdateCheckBox;
        private System.Windows.Forms.Label         lbl_UpdateSec;
        private System.Windows.Forms.NumericUpDown updateIntervalNumeric;
        private System.Windows.Forms.CheckBox      suppressRepeatCheckBox;
        private System.Windows.Forms.Button        exportConfigButton;
        private System.Windows.Forms.Button        importConfigButton;

        private System.Windows.Forms.GroupBox      logSettingsGB;
        private System.Windows.Forms.Label         lbl_MaxLogSize;
        private System.Windows.Forms.NumericUpDown maxLogSizeNumeric;
        private System.Windows.Forms.Label         lbl_RetainFiles;
        private System.Windows.Forms.NumericUpDown maxLogFilesNumeric;
        private System.Windows.Forms.CheckBox      debugLoggingCheckBox;
        private System.Windows.Forms.CheckBox      autoDeleteCheckBox;

        private System.Windows.Forms.Label         lbl_CommandHistorySize;
        private System.Windows.Forms.NumericUpDown commandHistorySizeNumeric;
        private System.Windows.Forms.Label         lbl_ChatHistorySize;
        private System.Windows.Forms.NumericUpDown chatHistorySizeNumeric;
    }
}
