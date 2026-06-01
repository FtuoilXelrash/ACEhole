using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace ACEhole
{
    public enum ConsoleLineType
    {
        JoinLeave,
        Chat,
        Error,
        Warning,
        Mod,
        Save,
        Network,
        Default
    }

    public static class ConsoleLineClassifier
    {
        // ACEhole timestamp wrapper: "(HH:mm:ss) | "
        private static readonly Regex _aceholePrefix =
            new Regex(@"^\(\d{2}:\d{2}:\d{2}\) \| ", RegexOptions.Compiled);

        // log4net prefix: "2026-05-31 12:34:56,789 [4] INFO  Namespace - "
        private static readonly Regex _log4netPrefix =
            new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},\d+ \[\d+\] \w+\s+[\w\.]+ - ", RegexOptions.Compiled);

        /// <summary>
        /// Strips timestamp/thread prefixes from a console line and returns the bare message.
        /// Used as the deduplication key in the throttler.
        /// Returns null for empty/whitespace-only lines.
        /// </summary>
        public static string ExtractMessagePattern(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;
            string s = _aceholePrefix.Replace(line, "");
            s = _log4netPrefix.Replace(s, "");
            s = s.Trim();
            return string.IsNullOrEmpty(s) ? null : s;
        }

        public static ConsoleLineType Classify(string line)
        {
            if (string.IsNullOrEmpty(line)) return ConsoleLineType.Default;

            string msg = ExtractMessagePattern(line) ?? line;

            // Network errors — check first to prevent misclassification as Error
            if (msg.Contains("SocketException") || msg.Contains("NetworkStream") ||
                msg.Contains("connection was forcibly closed") ||
                msg.Contains("An existing connection") ||
                (msg.Contains("failed") && msg.Contains("connect")))
                return ConsoleLineType.Network;

            // Join / Leave events
            if (msg.Contains("has logged in") || msg.Contains("has connected") ||
                msg.Contains("has logged out") || msg.Contains("has disconnected") ||
                msg.Contains("leaving") || msg.Contains("Logging off"))
                return ConsoleLineType.JoinLeave;

            // Chat messages
            if (msg.Contains("[GlobalChat]") || msg.Contains("[General]") ||
                msg.Contains("[Chat]") || msg.Contains("says:"))
                return ConsoleLineType.Chat;

            // Errors and exceptions
            if (msg.Contains("ERROR") || msg.Contains("Error") ||
                msg.Contains("Exception") || msg.Contains("NullReference") ||
                msg.Contains("FATAL") || msg.Contains("Fatal"))
                return ConsoleLineType.Error;

            // Warnings
            if (msg.Contains("WARNING") || msg.Contains("Warning") ||
                msg.Contains("WARN") || msg.Contains("Warn"))
                return ConsoleLineType.Warning;

            // MOD-related messages
            if (msg.Contains("[ACEmod]") || msg.Contains("mod loaded") ||
                msg.Contains("mod unloaded") || msg.Contains("Mod:"))
                return ConsoleLineType.Mod;

            // Save operations
            if (msg.Contains("Saving") || msg.Contains("saved") ||
                msg.Contains("Save ") || msg.Contains("Backup"))
                return ConsoleLineType.Save;

            return ConsoleLineType.Default;
        }

        public static Color GetColor(ConsoleLineType type, AppConfig config)
        {
            if (config?.Colors == null || !config.Colors.EnableConsoleColors)
                return Color.FromArgb(212, 212, 212);

            return type switch
            {
                ConsoleLineType.JoinLeave => ParseColor(config.Colors.JoinLeaveColor),
                ConsoleLineType.Chat      => ParseColor(config.Colors.ChatColor),
                ConsoleLineType.Error     => ParseColor(config.Colors.ErrorColor),
                ConsoleLineType.Warning   => ParseColor(config.Colors.WarningColor),
                ConsoleLineType.Mod       => ParseColor(config.Colors.ModColor),
                ConsoleLineType.Save      => ParseColor(config.Colors.SaveColor),
                ConsoleLineType.Network   => ParseColor(config.Colors.NetworkColor),
                _                         => ParseColor(config.Colors.DefaultColor)
            };
        }

        private static Color ParseColor(string hex)
        {
            try
            {
                if (hex?.StartsWith("#") == true) hex = hex.Substring(1);
                if (hex?.Length == 6)
                {
                    int r = Convert.ToInt32(hex.Substring(0, 2), 16);
                    int g = Convert.ToInt32(hex.Substring(2, 2), 16);
                    int b = Convert.ToInt32(hex.Substring(4, 2), 16);
                    return Color.FromArgb(r, g, b);
                }
            }
            catch { }
            return Color.FromArgb(212, 212, 212);
        }
    }
}
