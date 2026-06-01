using System;
using System.Reflection;
using System.Windows.Forms;

namespace ACEhole
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Logger.Initialize();

            var ver = Assembly.GetExecutingAssembly().GetName().Version;
            Logger.Info($"ACEhole v{ver.Major}.{ver.Minor}.{ver.Build} starting");
            Logger.Info($"OS: {Environment.OSVersion}");
            Logger.Info($"CLR: {Environment.Version}");

            AppConfig config = ConfigManager.LoadConfig();
            Logger.SetConfig(config);
            Logger.Info($"Config loaded from: {ConfigManager.GetConfigPath()}");

            Logger.Debug($"  AceExe:      {config.Server.AceExePath}");
            Logger.Debug($"  LogFile:     {config.Server.LogFilePath}");
            Logger.Debug($"  AutoRestart: {config.Server.AutoRestartOnCrash}");
            Logger.Debug($"  Theme:       {config.Ui.Theme}");

            if (config.Logging.EnableAutoDelete)
                Logger.CleanupOldLogFiles(config.Logging.RetainLogCount);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            Logger.Info("Starting main window.");
            Application.Run(new Main_Win());
            Logger.Info("Application exited.");
        }
    }
}
