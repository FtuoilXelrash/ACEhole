namespace ACEhole
{
    partial class Main_Win
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

            // ── Instantiate all controls ────────────────────────────────
            this.menuStrip1           = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem         = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem         = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuItem         = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem     = new System.Windows.Forms.ToolStripMenuItem();
            this.logFilesMenuItem     = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem         = new System.Windows.Forms.ToolStripMenuItem();
            this.releaseInfoMenuItem   = new System.Windows.Forms.ToolStripMenuItem();
            this.documentationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSep1         = new System.Windows.Forms.ToolStripSeparator();
            this.aboutMenuItem         = new System.Windows.Forms.ToolStripMenuItem();

            this.toolbarPanel        = new System.Windows.Forms.Panel();
            this.startServerButton   = new System.Windows.Forms.Button();
            this.stopServerButton    = new System.Windows.Forms.Button();
            this.restartServerButton = new System.Windows.Forms.Button();
            this.toolbarSep          = new System.Windows.Forms.Label();
            this.logFilesButton      = new System.Windows.Forms.Button();

            this.Main_Tabs   = new System.Windows.Forms.TabControl();
            this.tab_Console = new System.Windows.Forms.TabPage();
            this.tab_Status  = new System.Windows.Forms.TabPage();

            this.consoleRTB             = new System.Windows.Forms.RichTextBox();
            this.consoleChatPanel       = new System.Windows.Forms.Panel();
            this.consoleCommandLabel    = new System.Windows.Forms.Label();
            this.consoleCommandComboBox = new System.Windows.Forms.ComboBox();
            this.consoleChatLabel       = new System.Windows.Forms.Label();
            this.consoleChatComboBox    = new System.Windows.Forms.ComboBox();

            this.serverStatusGroupBox   = new System.Windows.Forms.GroupBox();
            this.procMonitorGroupBox    = new System.Windows.Forms.GroupBox();
            this.populationGroupBox     = new System.Windows.Forms.GroupBox();
            this.worldDataGroupBox      = new System.Windows.Forms.GroupBox();
            this.serverDataGroupBox     = new System.Windows.Forms.GroupBox();
            this.playerDatabaseGroupBox = new System.Windows.Forms.GroupBox();
            this.modsGroupBox           = new System.Windows.Forms.GroupBox();
            this.refreshStatsButton     = new System.Windows.Forms.Button();

            // Status bar
            this.statusInfoPanel  = new System.Windows.Forms.Panel();
            this.statusBarTopLine = new System.Windows.Forms.Panel();
            this.statVersionLabel = new System.Windows.Forms.Label();

            // Status tab value labels (configured in SetupStatusTab)
            this.serverNameValueLabel    = new System.Windows.Forms.Label();
            this.serverOnlineValueLabel  = new System.Windows.Forms.Label();
            this.procStateValueLabel     = new System.Windows.Forms.Label();
            this.procPidValueLabel       = new System.Windows.Forms.Label();
            this.procRamValueLabel       = new System.Windows.Forms.Label();
            this.procCpuValueLabel       = new System.Windows.Forms.Label();
            this.playersOnlineValueLabel = new System.Windows.Forms.Label();
            this.maxPlayersValueLabel    = new System.Windows.Forms.Label();
            this.worldTimeValueLabel     = new System.Windows.Forms.Label();
            this.memoryValueLabel        = new System.Windows.Forms.Label();
            this.networkInValueLabel     = new System.Windows.Forms.Label();
            this.networkOutValueLabel    = new System.Windows.Forms.Label();
            this.uptimeValueLabel        = new System.Windows.Forms.Label();
            this.totalPlayersValueLabel  = new System.Windows.Forms.Label();
            this.totalModsValueLabel     = new System.Windows.Forms.Label();
            this.loadedModsValueLabel    = new System.Windows.Forms.Label();
            this.errorModsValueLabel     = new System.Windows.Forms.Label();

            // Status bar value labels (configured in SetupStatusBar)
            this.statPlayersOnlineValue = new System.Windows.Forms.Label();
            this.statStateValue         = new System.Windows.Forms.Label();
            this.statUptimeValue        = new System.Windows.Forms.Label();
            this.statMemoryValue        = new System.Windows.Forms.Label();

            // Tray
            this.notifyIcon1          = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openTrayMenuItem     = new System.Windows.Forms.ToolStripMenuItem();
            this.trayMenuSeparator    = new System.Windows.Forms.ToolStripSeparator();
            this.exitTrayMenuItem     = new System.Windows.Forms.ToolStripMenuItem();

            this.menuStrip1.SuspendLayout();
            this.toolbarPanel.SuspendLayout();
            this.Main_Tabs.SuspendLayout();
            this.tab_Console.SuspendLayout();
            this.consoleChatPanel.SuspendLayout();
            this.tab_Status.SuspendLayout();
            this.statusInfoPanel.SuspendLayout();
            this.trayContextMenuStrip.SuspendLayout();
            this.SuspendLayout();

            // ── MenuStrip ──────────────────────────────────────────────
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.fileMenuItem, this.viewMenuItem, this.helpMenuItem });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Size     = new System.Drawing.Size(1100, 24);

            this.fileMenuItem.Text = "&File";
            this.fileMenuItem.DropDownItems.Add(this.exitMenuItem);
            this.exitMenuItem.Text   = "E&xit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);

            this.viewMenuItem.Text = "&View";
            this.viewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.settingsMenuItem, this.logFilesMenuItem });
            this.settingsMenuItem.Text   = "&Settings";
            this.settingsMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
            this.logFilesMenuItem.Text   = "&Log Files";
            this.logFilesMenuItem.Click += new System.EventHandler(this.logFilesMenuItem_Click);

            this.helpMenuItem.Text = "&Help";
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.releaseInfoMenuItem, this.documentationMenuItem, this.toolStripSep1, this.aboutMenuItem });
            this.releaseInfoMenuItem.Text    = "&Release Information";
            this.releaseInfoMenuItem.Click  += new System.EventHandler(this.releaseInfoMenuItem_Click);
            this.documentationMenuItem.Text  = "&Documentation";
            this.documentationMenuItem.Click += new System.EventHandler(this.documentationMenuItem_Click);
            this.aboutMenuItem.Text          = "&About ACEhole";
            this.aboutMenuItem.Click        += new System.EventHandler(this.aboutMenuItem_Click);

            // ── Toolbar Panel ──────────────────────────────────────────
            this.toolbarPanel.Dock      = System.Windows.Forms.DockStyle.Top;
            this.toolbarPanel.Height    = 38;
            this.toolbarPanel.BackColor = System.Drawing.Color.Transparent;

            this.startServerButton.Text      = "▶  Start Server";
            this.startServerButton.Location  = new System.Drawing.Point(6, 5);
            this.startServerButton.Size      = new System.Drawing.Size(118, 28);
            this.startServerButton.ForeColor = System.Drawing.Color.FromArgb(76, 175, 80);
            this.startServerButton.Font      = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            this.startServerButton.Click    += new System.EventHandler(this.startServerButton_Click);

            this.stopServerButton.Text     = "■  Stop Server";
            this.stopServerButton.Location = new System.Drawing.Point(130, 5);
            this.stopServerButton.Size     = new System.Drawing.Size(110, 28);
            this.stopServerButton.Enabled  = false;
            this.stopServerButton.Click   += new System.EventHandler(this.stopServerButton_Click);

            this.restartServerButton.Text     = "↺  Restart";
            this.restartServerButton.Location = new System.Drawing.Point(246, 5);
            this.restartServerButton.Size     = new System.Drawing.Size(90, 28);
            this.restartServerButton.Enabled  = false;
            this.restartServerButton.Click   += new System.EventHandler(this.restartServerButton_Click);

            this.toolbarSep.Text      = "|";
            this.toolbarSep.Location  = new System.Drawing.Point(342, 10);
            this.toolbarSep.Size      = new System.Drawing.Size(10, 20);
            this.toolbarSep.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);

            this.logFilesButton.Text     = "Log Files";
            this.logFilesButton.Location = new System.Drawing.Point(356, 5);
            this.logFilesButton.Size     = new System.Drawing.Size(80, 28);
            this.logFilesButton.Click   += new System.EventHandler(this.logFilesButton_Click);

            this.toolbarPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.startServerButton, this.stopServerButton, this.restartServerButton,
                this.toolbarSep, this.logFilesButton });

            // ── Console Tab ────────────────────────────────────────────
            this.tab_Console.Text    = "Server Console";
            this.tab_Console.Name    = "tab_Console";
            this.tab_Console.Padding = new System.Windows.Forms.Padding(0);

            this.consoleRTB.Dock        = System.Windows.Forms.DockStyle.Fill;
            this.consoleRTB.BackColor   = System.Drawing.Color.FromArgb(12, 12, 12);
            this.consoleRTB.ForeColor   = System.Drawing.Color.FromArgb(212, 212, 212);
            this.consoleRTB.Font        = new System.Drawing.Font("Consolas", 9f);
            this.consoleRTB.ReadOnly    = true;
            this.consoleRTB.ScrollBars  = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.consoleRTB.WordWrap    = true;
            this.consoleRTB.BorderStyle = System.Windows.Forms.BorderStyle.None;

            // "Console Commands" label
            this.consoleCommandLabel.Text      = "Console Commands";
            this.consoleCommandLabel.AutoSize  = true;
            this.consoleCommandLabel.Location  = new System.Drawing.Point(4, 4);

            // Console command ComboBox — Enter sends via stdin
            this.consoleCommandComboBox.Font             = new System.Drawing.Font("Consolas", 9f);
            this.consoleCommandComboBox.FormattingEnabled = true;
            this.consoleCommandComboBox.Location         = new System.Drawing.Point(0, 19);
            this.consoleCommandComboBox.Size             = new System.Drawing.Size(800, 21);
            this.consoleCommandComboBox.KeyDown         += new System.Windows.Forms.KeyEventHandler(this.consoleCommandComboBox_KeyDown);

            // "Global Chat" label
            this.consoleChatLabel.Text      = "Global Chat";
            this.consoleChatLabel.AutoSize  = true;
            this.consoleChatLabel.Location  = new System.Drawing.Point(4, 46);

            // Global Chat ComboBox — Enter sends via stdin
            this.consoleChatComboBox.Font             = new System.Drawing.Font("Consolas", 9f);
            this.consoleChatComboBox.FormattingEnabled = true;
            this.consoleChatComboBox.Location         = new System.Drawing.Point(0, 62);
            this.consoleChatComboBox.Size             = new System.Drawing.Size(800, 21);
            this.consoleChatComboBox.KeyDown         += new System.Windows.Forms.KeyEventHandler(this.consoleChatComboBox_KeyDown);

            this.consoleChatPanel.Dock   = System.Windows.Forms.DockStyle.Bottom;
            this.consoleChatPanel.Height = 88;
            this.consoleChatPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.consoleCommandLabel, this.consoleCommandComboBox,
                this.consoleChatLabel, this.consoleChatComboBox });

            this.tab_Console.Controls.Add(this.consoleRTB);
            this.tab_Console.Controls.Add(this.consoleChatPanel);

            // ── Status Tab ─────────────────────────────────────────────
            this.tab_Status.Text       = "Server Status";
            this.tab_Status.Name       = "tab_Status";
            this.tab_Status.Padding    = new System.Windows.Forms.Padding(6);
            this.tab_Status.AutoScroll = true;

            // GroupBox containers — contents wired in SetupStatusTab()
            this.serverStatusGroupBox.Text     = "Server Status";
            this.serverStatusGroupBox.Location = new System.Drawing.Point(8, 6);
            this.serverStatusGroupBox.Size     = new System.Drawing.Size(346, 72);

            this.procMonitorGroupBox.Text     = "Process Monitor";
            this.procMonitorGroupBox.Location = new System.Drawing.Point(8, 83);
            this.procMonitorGroupBox.Size     = new System.Drawing.Size(346, 96);

            this.populationGroupBox.Text     = "Population Data";
            this.populationGroupBox.Location = new System.Drawing.Point(8, 184);
            this.populationGroupBox.Size     = new System.Drawing.Size(346, 72);

            this.worldDataGroupBox.Text     = "World Data";
            this.worldDataGroupBox.Location = new System.Drawing.Point(362, 6);
            this.worldDataGroupBox.Size     = new System.Drawing.Size(290, 52);

            this.serverDataGroupBox.Text     = "Server Data";
            this.serverDataGroupBox.Location = new System.Drawing.Point(660, 6);
            this.serverDataGroupBox.Size     = new System.Drawing.Size(290, 130);

            this.playerDatabaseGroupBox.Text     = "Player Database";
            this.playerDatabaseGroupBox.Location = new System.Drawing.Point(362, 63);
            this.playerDatabaseGroupBox.Size     = new System.Drawing.Size(290, 52);

            this.modsGroupBox.Text     = "MODs";
            this.modsGroupBox.Location = new System.Drawing.Point(660, 140);
            this.modsGroupBox.Size     = new System.Drawing.Size(290, 92);

            this.refreshStatsButton.Text     = "Refresh Stats";
            this.refreshStatsButton.Location = new System.Drawing.Point(862, 6);
            this.refreshStatsButton.Size     = new System.Drawing.Size(90, 26);
            this.refreshStatsButton.Anchor   = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.refreshStatsButton.Click   += new System.EventHandler(this.refreshStatsButton_Click);

            this.tab_Status.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.serverStatusGroupBox, this.procMonitorGroupBox, this.populationGroupBox,
                this.worldDataGroupBox, this.serverDataGroupBox,
                this.playerDatabaseGroupBox, this.modsGroupBox,
                this.refreshStatsButton });

            // ── TabControl ──────────────────────────────────────────────
            this.Main_Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Main_Tabs.Controls.Add(this.tab_Console);
            this.Main_Tabs.Controls.Add(this.tab_Status);

            // ── Status Bar Panel ───────────────────────────────────────
            this.statusInfoPanel.Dock   = System.Windows.Forms.DockStyle.Bottom;
            this.statusInfoPanel.Height = 40;  // exact match to Fe2O3xH2O / rServed

            this.statusBarTopLine.Dock      = System.Windows.Forms.DockStyle.Top;
            this.statusBarTopLine.Height    = 1;
            this.statusBarTopLine.BackColor = System.Drawing.Color.FromArgb(210, 210, 210);

            // Version label — far right, row 2 (y=21), same as rServed
            this.statVersionLabel.AutoSize  = false;
            this.statVersionLabel.Size      = new System.Drawing.Size(180, 13);
            this.statVersionLabel.Anchor    = System.Windows.Forms.AnchorStyles.None;
            this.statVersionLabel.Location  = new System.Drawing.Point(650, 21);
            this.statVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.statVersionLabel.Font      = new System.Drawing.Font("Segoe UI", 8.25f);
            this.statVersionLabel.ForeColor = System.Drawing.Color.FromArgb(130, 130, 130);

            this.statusInfoPanel.Controls.Add(this.statusBarTopLine);
            this.statusInfoPanel.Controls.Add(this.statVersionLabel);

            // ── Tray ───────────────────────────────────────────────────
            this.trayContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.openTrayMenuItem, this.trayMenuSeparator, this.exitTrayMenuItem });
            this.openTrayMenuItem.Text   = "Open ACEhole";
            this.openTrayMenuItem.Click += new System.EventHandler(this.openTrayMenuItem_Click);
            this.exitTrayMenuItem.Text   = "Exit";
            this.exitTrayMenuItem.Click += new System.EventHandler(this.exitTrayMenuItem_Click);

            this.notifyIcon1.ContextMenuStrip = this.trayContextMenuStrip;
            this.notifyIcon1.Text             = "ACEhole";
            this.notifyIcon1.Visible          = true;
            this.notifyIcon1.DoubleClick     += new System.EventHandler(this.notifyIcon1_DoubleClick);

            // ── Form ───────────────────────────────────────────────────
            this.ClientSize      = new System.Drawing.Size(1100, 700);
            this.Text            = "ACEhole — Asheron's Call Server Manager";
            this.Font            = new System.Drawing.Font("Segoe UI", 9f);
            this.MinimumSize     = new System.Drawing.Size(900, 600);
            this.MaximizeBox     = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MainMenuStrip   = this.menuStrip1;

            this.Controls.Add(this.Main_Tabs);
            this.Controls.Add(this.statusInfoPanel);
            this.Controls.Add(this.toolbarPanel);
            this.Controls.Add(this.menuStrip1);

            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolbarPanel.ResumeLayout(false);
            this.Main_Tabs.ResumeLayout(false);
            this.tab_Console.ResumeLayout(false);
            this.consoleChatPanel.ResumeLayout(false);
            this.consoleChatPanel.PerformLayout();
            this.tab_Status.ResumeLayout(false);
            this.statusInfoPanel.ResumeLayout(false);
            this.trayContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ── Field declarations ─────────────────────────────────────────
        private System.Windows.Forms.MenuStrip         menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logFilesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem  releaseInfoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem  documentationMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSep1;
        private System.Windows.Forms.ToolStripMenuItem  aboutMenuItem;

        private System.Windows.Forms.Panel  toolbarPanel;
        private System.Windows.Forms.Button startServerButton;
        private System.Windows.Forms.Button stopServerButton;
        private System.Windows.Forms.Button restartServerButton;
        private System.Windows.Forms.Label  toolbarSep;
        private System.Windows.Forms.Button logFilesButton;

        private System.Windows.Forms.TabControl Main_Tabs;
        private System.Windows.Forms.TabPage    tab_Console;
        private System.Windows.Forms.TabPage    tab_Status;

        private System.Windows.Forms.RichTextBox consoleRTB;
        private System.Windows.Forms.Panel    consoleChatPanel;
        private System.Windows.Forms.Label    consoleCommandLabel;
        private System.Windows.Forms.ComboBox consoleCommandComboBox;
        private System.Windows.Forms.Label    consoleChatLabel;
        private System.Windows.Forms.ComboBox consoleChatComboBox;

        private System.Windows.Forms.GroupBox serverStatusGroupBox;
        private System.Windows.Forms.GroupBox procMonitorGroupBox;
        private System.Windows.Forms.GroupBox populationGroupBox;
        private System.Windows.Forms.GroupBox worldDataGroupBox;
        private System.Windows.Forms.GroupBox serverDataGroupBox;
        private System.Windows.Forms.GroupBox playerDatabaseGroupBox;
        private System.Windows.Forms.GroupBox modsGroupBox;
        private System.Windows.Forms.Button   refreshStatsButton;

        // Status tab value labels (populated in SetupStatusTab)
        private System.Windows.Forms.Label serverNameValueLabel;
        private System.Windows.Forms.Label serverOnlineValueLabel;
        private System.Windows.Forms.Label procStateValueLabel;
        private System.Windows.Forms.Label procPidValueLabel;
        private System.Windows.Forms.Label procRamValueLabel;
        private System.Windows.Forms.Label procCpuValueLabel;
        private System.Windows.Forms.Label playersOnlineValueLabel;
        private System.Windows.Forms.Label maxPlayersValueLabel;
        private System.Windows.Forms.Label worldTimeValueLabel;
        private System.Windows.Forms.Label memoryValueLabel;
        private System.Windows.Forms.Label networkInValueLabel;
        private System.Windows.Forms.Label networkOutValueLabel;
        private System.Windows.Forms.Label uptimeValueLabel;
        private System.Windows.Forms.Label totalPlayersValueLabel;
        private System.Windows.Forms.Label totalModsValueLabel;
        private System.Windows.Forms.Label loadedModsValueLabel;
        private System.Windows.Forms.Label errorModsValueLabel;

        // Status bar labels (populated in SetupStatusBar)
        private System.Windows.Forms.Label statPlayersOnlineValue;
        private System.Windows.Forms.Label statStateValue;
        private System.Windows.Forms.Label statUptimeValue;
        private System.Windows.Forms.Label statMemoryValue;
        private System.Windows.Forms.Label statVersionLabel;

        private System.Windows.Forms.Panel statusInfoPanel;
        private System.Windows.Forms.Panel statusBarTopLine;

        // ── MODs tab (created in SetupModsTab) ─────────────────────────
        private System.Windows.Forms.TabPage      tab_Mods;
        private System.Windows.Forms.TabControl   modsTabs;
        private System.Windows.Forms.TabPage      tab_ModMaintenance;
        private System.Windows.Forms.TabPage      tab_ModNotifications;
        private System.Windows.Forms.RichTextBox  modNotificationsRTB;

        // Tray
        private System.Windows.Forms.NotifyIcon          notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip     trayContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem    openTrayMenuItem;
        private System.Windows.Forms.ToolStripSeparator   trayMenuSeparator;
        private System.Windows.Forms.ToolStripMenuItem    exitTrayMenuItem;
    }
}
