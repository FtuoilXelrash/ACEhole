using System;
using System.IO;
using System.Linq;

namespace ACEhole
{
    public static class Logger
    {
        private static readonly string AppLogDirectory     = Path.Combine(AppPaths.LogsDirectory, "App");
        private static readonly string ConsoleLogDirectory = AppPaths.GetConsoleLogDirectory();

        private static string _currentAppLogFile;
        private static string _currentConsoleLogFile;
        private static readonly object _lock = new object();
        private static AppConfig _config;
        private static string _currentLogDate;

        public static void Initialize()
        {
            try
            {
                Directory.CreateDirectory(AppLogDirectory);
                _currentLogDate    = DateTime.Now.ToString("yyyy-MM-dd");
                _currentAppLogFile = FindOrCreateLogFile(AppLogDirectory, "AppLog");
                Log("INFO", "Logger initialized");
                Log("INFO", $"ACEhole started at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Logger init failed: {ex.Message}");
            }
        }

        public static void StartConsoleLog()
        {
            try
            {
                Directory.CreateDirectory(ConsoleLogDirectory);
                _currentConsoleLogFile = FindOrCreateLogFile(ConsoleLogDirectory, "ConsoleLog");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Console log init failed: {ex.Message}");
            }
        }

        public static void SetConfig(AppConfig config) => _config = config;

        public static void Info(string message)    => Log("INFO",  message);
        public static void Warning(string message) => Log("WARN",  message);
        public static void Error(string message)   => Log("ERROR", message);
        public static void Error(string message, Exception ex)
        {
            Log("ERROR", $"{message} — {ex.Message}");
            Log("ERROR", $"Stack: {ex.StackTrace}");
        }
        public static void Debug(string message)
        {
            if (_config != null && !_config.Logging.EnableDebugLogging) return;
            Log("DEBUG", message);
        }

        public static void LogConsole(string line)
        {
            if (string.IsNullOrEmpty(_currentConsoleLogFile)) return;
            lock (_lock)
            {
                try
                {
                    CheckRotate(ref _currentConsoleLogFile, ConsoleLogDirectory, "ConsoleLog");
                    File.AppendAllText(_currentConsoleLogFile,
                        $"({GetTimestamp()}) | {line}{Environment.NewLine}");
                }
                catch { }
            }
        }

        public static string AppLogPath     => AppLogDirectory;
        public static string ConsoleLogPath => ConsoleLogDirectory;
        public static string GetCurrentAppLogFile() => _currentAppLogFile;

        public static string GetDisplayTimestamp() => GetTimestamp();

        public static void CleanupOldLogFiles(int retain)
        {
            try { CleanDir(AppLogDirectory, retain, "App"); } catch { }
        }

        private static void Log(string level, string message)
        {
            if (string.IsNullOrEmpty(_currentAppLogFile)) return;
            lock (_lock)
            {
                try
                {
                    CheckRotate(ref _currentAppLogFile, AppLogDirectory, "AppLog");
                    string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}";
                    File.AppendAllText(_currentAppLogFile, entry + Environment.NewLine);
                    System.Diagnostics.Debug.WriteLine(entry);
                }
                catch { }
            }
        }

        private static string GetTimestamp()
        {
            bool use24 = (_config == null) || _config.Ui.Use24HourTime;
            return use24 ? DateTime.Now.ToString("HH:mm:ss") : DateTime.Now.ToString("hh:mm:ss tt");
        }

        private static string FindOrCreateLogFile(string dir, string prefix)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string path = Path.Combine(dir, $"{prefix}_{date}.log");
            if (File.Exists(path))
            {
                long max = (_config != null) ? (long)_config.Logging.MaxLogFileSizeMB * 1024 * 1024 : 100L * 1024 * 1024;
                if (new FileInfo(path).Length >= max)
                {
                    int seq = NextSeq(dir, prefix, date);
                    return Path.Combine(dir, $"{prefix}_{date}_{seq:D2}.log");
                }
            }
            return path;
        }

        private static int NextSeq(string dir, string prefix, string date)
        {
            var files = Directory.GetFiles(dir, $"{prefix}_{date}_*.log");
            int max = 0;
            foreach (var f in files)
            {
                var parts = Path.GetFileNameWithoutExtension(f).Split('_');
                if (parts.Length > 0 && int.TryParse(parts[^1], out int s) && s > max) max = s;
            }
            return max + 1;
        }

        private static void CheckRotate(ref string path, string dir, string prefix)
        {
            if (_config == null || !File.Exists(path)) return;
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            if (today != _currentLogDate)
            {
                _currentLogDate = today;
                path = Path.Combine(dir, $"{prefix}_{today}.log");
                return;
            }
            long max = (long)_config.Logging.MaxLogFileSizeMB * 1024 * 1024;
            if (new FileInfo(path).Length >= max)
            {
                int seq = NextSeq(dir, prefix, today);
                path = Path.Combine(dir, $"{prefix}_{today}_{seq:D2}.log");
            }
        }

        private static void CleanDir(string dir, int retain, string label)
        {
            if (!Directory.Exists(dir)) return;
            var files = Directory.GetFiles(dir, "*.log")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.LastWriteTime)
                .ToList();
            for (int i = retain; i < files.Count; i++)
            {
                try { files[i].Delete(); } catch { }
            }
        }
    }
}
