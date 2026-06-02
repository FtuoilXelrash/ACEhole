using System;
using System.Collections.Generic;

namespace ACEhole
{
    public enum ThemeMode { Light, Dark, System }

    public class AppConfig
    {
        public ServerSettings       Server        { get; set; } = new ServerSettings();
        public UiSettings           Ui            { get; set; } = new UiSettings();
        public LogSettings          Logging       { get; set; } = new LogSettings();
        public ConsoleColors        Colors        { get; set; } = new ConsoleColors();
        public LogViewerSettings    LogViewer     { get; set; } = new LogViewerSettings();
        public NotificationSettings Notifications { get; set; } = new NotificationSettings();
    }

    public class ServerSettings
    {
        // Paths
        public string AceExePath  { get; set; } = @"Z:\ACE TEST\ACE.Server.exe";
        public string LogFilePath { get; set; } = @"Z:\ACE TEST\Logs\FULL\ACE_Log.txt";

        // Auto-restart
        public bool AutoRestartOnCrash { get; set; } = true;
        public int  RestartDelaySecs   { get; set; } = 5;
    }

    public class UiSettings
    {
        public ThemeMode Theme          { get; set; } = ThemeMode.Light;
        public bool      Use24HourTime  { get; set; } = true;
        public bool      MinimizeToTray { get; set; } = true;

        // Status auto-update
        public bool AutoUpdateStatus          { get; set; } = true;
        public int  StatusUpdateIntervalSecs  { get; set; } = 30;

        // Window position and size (-1 = default/center)
        public int WindowX      { get; set; } = -1;
        public int WindowY      { get; set; } = -1;
        public int WindowWidth  { get; set; } = 1100;
        public int WindowHeight { get; set; } = 700;

        // Options dialog position
        public int OptionsDialogX { get; set; } = -1;
        public int OptionsDialogY { get; set; } = -1;

        // Last active tab
        public string LastActiveTab { get; set; } = "";

        // Command/chat history persistence
        public int          CommandHistoryMaxSize { get; set; } = 20;
        public int          ChatHistoryMaxSize    { get; set; } = 20;
        public List<string> CommandHistory        { get; set; } = new List<string>();
        public List<string> ChatHistory           { get; set; } = new List<string>();

        // Collapse consecutive identical console lines into a suppression notice.
        public bool SuppressRepeatedConsoleLines { get; set; } = true;

        // Window behavior
        public bool MainWindowAlwaysOnTop   { get; set; } = false;
        public bool MainWindowShowInTaskbar { get; set; } = true;
        public bool SettingsAlwaysOnTop    { get; set; } = false;
        public bool SettingsShowInTaskbar  { get; set; } = true;
    }

    public class LogSettings
    {
        public int  MaxLogFileSizeMB   { get; set; } = 100;
        public int  RetainLogCount     { get; set; } = 100;
        public bool EnableDebugLogging { get; set; } = false;
        public bool EnableAutoDelete   { get; set; } = false;
    }

    public class NotificationSettings
    {
        public bool EnableTrayNotifications      { get; set; } = false;
        public bool TrayNotifyOnServerStart      { get; set; } = true;
        public bool TrayNotifyOnServerStop       { get; set; } = true;
        public bool EnableSoundOnServerStart     { get; set; } = true;
        public bool EnableSoundOnServerStop      { get; set; } = true;
    }

    public class ConsoleColors
    {
        public bool EnableConsoleColors   { get; set; } = true;
        public bool EnableLogViewerColors { get; set; } = true;

        // Console output line type colors (hex strings)
        public string JoinLeaveColor { get; set; } = "#66BB6A";   // green
        public string ChatColor      { get; set; } = "#00BCD4";   // cyan
        public string ErrorColor     { get; set; } = "#EF5350";   // red
        public string WarningColor   { get; set; } = "#FF9800";   // orange
        public string ModColor       { get; set; } = "#AB47BC";   // purple
        public string SaveColor      { get; set; } = "#4DB6AC";   // teal
        public string NetworkColor   { get; set; } = "#546E7A";   // blue-gray (muted)
        public string DefaultColor   { get; set; } = "#9E9E9E";   // gray

        // App log level colors (Log Viewer — Application log type)
        public string AppInfoColor  { get; set; } = "#64B5F6";   // light blue
        public string AppWarnColor  { get; set; } = "#FF9800";   // orange
        public string AppErrorColor { get; set; } = "#EF5350";   // red
        public string AppDebugColor { get; set; } = "#737373";   // gray
    }

    public class LogViewerSettings
    {
        public int  WindowX          { get; set; } = -1;
        public int  WindowY          { get; set; } = -1;
        public bool AlwaysOnTop      { get; set; } = false;
        public bool ShowInTaskbar    { get; set; } = true;
        public int  LastLogTypeIndex { get; set; } = 0;
    }
}
