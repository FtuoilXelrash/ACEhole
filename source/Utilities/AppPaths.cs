using System;
using System.IO;

namespace ACEhole
{
    /// <summary>
    /// Centralized path resolver.
    /// Debug builds write alongside the executable.
    /// Release builds write to %LocalAppData%\ACEhole (required for MSIX/Store).
    /// </summary>
    public static class AppPaths
    {
        public static readonly string Root;
        public static readonly string DataDirectory;
        public static readonly string LogsDirectory;

        static AppPaths()
        {
            Root            = ResolveRoot();
            DataDirectory   = Path.Combine(Root, "data");
            LogsDirectory   = Path.Combine(Root, "logs");
        }

        public static string GetConsoleLogDirectory()
            => Path.Combine(LogsDirectory, "Console");

        private static string ResolveRoot()
        {
#if DEBUG
            return AppDomain.CurrentDomain.BaseDirectory;
#else
            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ACEhole");
            Directory.CreateDirectory(dir);
            return dir;
#endif
        }
    }
}
