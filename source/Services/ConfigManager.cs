using System;
using System.IO;
using Newtonsoft.Json;

namespace ACEhole
{
    public static class ConfigManager
    {
        private static readonly string ConfigDir  = AppPaths.DataDirectory;
        private static readonly string ConfigPath = Path.Combine(ConfigDir, "config.json");

        public static AppConfig LoadConfig()
        {
            try
            {
                if (!File.Exists(ConfigPath))
                    return CreateDefault();

                string json = File.ReadAllText(ConfigPath);
                var settings = new JsonSerializerSettings
                {
                    ObjectCreationHandling = ObjectCreationHandling.Replace
                };
                AppConfig cfg = JsonConvert.DeserializeObject<AppConfig>(json, settings);
                if (cfg == null) return CreateDefault();

                bool modified = Migrate(cfg);
                if (modified) SaveConfig(cfg);
                return cfg;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Config load error: {ex.Message}");
                return CreateDefault();
            }
        }

        public static bool SaveConfig(AppConfig cfg)
        {
            try
            {
                Directory.CreateDirectory(ConfigDir);
                string json = JsonConvert.SerializeObject(cfg, Formatting.Indented);
                File.WriteAllText(ConfigPath, json);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Config save error: {ex.Message}");
                return false;
            }
        }

        public static bool ExportConfig(string path, AppConfig cfg)
        {
            try
            {
                string json = JsonConvert.SerializeObject(cfg, Formatting.Indented);
                File.WriteAllText(path, json);
                return true;
            }
            catch { return false; }
        }

        public static AppConfig ImportConfig(string path)
        {
            try
            {
                string json = File.ReadAllText(path);
                var settings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
                return JsonConvert.DeserializeObject<AppConfig>(json, settings) ?? CreateDefault();
            }
            catch { return null; }
        }

        public static string GetConfigPath() => ConfigPath;

        private static AppConfig CreateDefault()
        {
            AppConfig cfg = new AppConfig();
            SaveConfig(cfg);
            return cfg;
        }

        private static bool Migrate(AppConfig cfg)
        {
            bool changed = false;
            if (cfg.Server        == null) { cfg.Server        = new ServerSettings();       changed = true; }
            if (cfg.Ui            == null) { cfg.Ui            = new UiSettings();           changed = true; }
            if (cfg.Logging       == null) { cfg.Logging       = new LogSettings();          changed = true; }
            if (cfg.Colors        == null) { cfg.Colors        = new ConsoleColors();        changed = true; }
            if (cfg.LogViewer     == null) { cfg.LogViewer     = new LogViewerSettings();    changed = true; }
            if (cfg.Notifications == null) { cfg.Notifications = new NotificationSettings(); changed = true; }
            if (cfg.Ui.CommandHistory == null) { cfg.Ui.CommandHistory = new System.Collections.Generic.List<string>(); changed = true; }
            if (cfg.Ui.ChatHistory    == null) { cfg.Ui.ChatHistory    = new System.Collections.Generic.List<string>(); changed = true; }
            return changed;
        }
    }
}
