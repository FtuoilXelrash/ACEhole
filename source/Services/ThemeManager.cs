using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ACEhole
{
    public static class ThemeManager
    {
        public static class LightTheme
        {
            public static readonly Color FormBackground        = Color.FromArgb(255, 255, 255);
            public static readonly Color ControlBackground     = Color.White;
            public static readonly Color TextForeground        = Color.FromArgb(33, 33, 33);
            public static readonly Color TextBoxBackground     = Color.White;
            public static readonly Color TextBoxForeground     = Color.FromArgb(33, 33, 33);
            public static readonly Color RichTextBoxBackground = Color.FromArgb(250, 250, 250);
            public static readonly Color RichTextBoxForeground = Color.FromArgb(33, 33, 33);
            public static readonly Color GroupBoxForeground    = Color.FromArgb(33, 33, 33);
            public static readonly Color LabelForeground       = Color.FromArgb(33, 33, 33);
            public static readonly Color ButtonBackground      = SystemColors.Control;
            public static readonly Color ButtonForeground      = Color.FromArgb(33, 33, 33);
        }

        public static class DarkTheme
        {
            public static readonly Color FormBackground        = Color.FromArgb(30, 30, 30);
            public static readonly Color ControlBackground     = Color.FromArgb(37, 37, 38);
            public static readonly Color TextForeground        = Color.FromArgb(212, 212, 212);
            public static readonly Color TextBoxBackground     = Color.FromArgb(45, 45, 48);
            public static readonly Color TextBoxForeground     = Color.FromArgb(212, 212, 212);
            public static readonly Color RichTextBoxBackground = Color.FromArgb(30, 30, 30);
            public static readonly Color RichTextBoxForeground = Color.FromArgb(212, 212, 212);
            public static readonly Color GroupBoxForeground    = Color.FromArgb(200, 200, 200);
            public static readonly Color LabelForeground       = Color.FromArgb(212, 212, 212);
            public static readonly Color ButtonBackground      = Color.FromArgb(60, 60, 60);
            public static readonly Color ButtonForeground      = Color.FromArgb(212, 212, 212);
        }

        public static bool IsWindowsInDarkMode()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key?.GetValue("AppsUseLightTheme") is int v) return v == 0;
                }
            }
            catch { }
            return false;
        }

        public static bool ShouldUseDarkMode(ThemeMode mode) => mode switch
        {
            ThemeMode.Dark   => true,
            ThemeMode.System => IsWindowsInDarkMode(),
            _                => false
        };

        public static void ApplyTheme(Form form, bool isDark)
        {
            try
            {
                form.BackColor = isDark ? DarkTheme.FormBackground : LightTheme.FormBackground;
                form.ForeColor = isDark ? DarkTheme.TextForeground : LightTheme.TextForeground;
                ApplyToControls(form.Controls, isDark);
                form.Refresh();
            }
            catch (Exception ex)
            {
                Logger.Warning($"Theme apply error: {ex.Message}");
            }
        }

        private static void ApplyToControls(Control.ControlCollection controls, bool isDark)
        {
            foreach (Control c in controls)
            {
                try
                {
                    if (IsStatusLabel(c)) { if (c.Controls.Count > 0) ApplyToControls(c.Controls, isDark); continue; }

                    if      (c is RichTextBox rtb) { rtb.BackColor = isDark ? DarkTheme.RichTextBoxBackground : LightTheme.RichTextBoxBackground; rtb.ForeColor = isDark ? DarkTheme.RichTextBoxForeground : LightTheme.RichTextBoxForeground; }
                    else if (c is TextBox tb)      { tb.BackColor  = isDark ? DarkTheme.TextBoxBackground     : LightTheme.TextBoxBackground;     tb.ForeColor  = isDark ? DarkTheme.TextBoxForeground     : LightTheme.TextBoxForeground; }
                    else if (c is Button btn)
                    {
                        btn.BackColor  = isDark ? DarkTheme.ButtonBackground : LightTheme.ButtonBackground;
                        btn.ForeColor  = isDark ? DarkTheme.ButtonForeground : LightTheme.ButtonForeground;
                        btn.FlatStyle  = isDark ? FlatStyle.Flat : FlatStyle.Standard;
                        if (isDark) btn.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
                    }
                    else if (c is Label lbl)       { lbl.ForeColor    = isDark ? DarkTheme.LabelForeground    : LightTheme.LabelForeground; }
                    else if (c is GroupBox gb)     { gb.ForeColor     = isDark ? DarkTheme.GroupBoxForeground : LightTheme.GroupBoxForeground; }
                    else if (c is TabControl tc)   { tc.BackColor     = isDark ? DarkTheme.ControlBackground  : LightTheme.ControlBackground; }
                    else if (c is TabPage tp)      { tp.BackColor     = isDark ? DarkTheme.FormBackground     : LightTheme.FormBackground; tp.ForeColor = isDark ? DarkTheme.TextForeground : LightTheme.TextForeground; }
                    else if (c is ComboBox cb)     { cb.BackColor     = isDark ? DarkTheme.TextBoxBackground  : LightTheme.TextBoxBackground; cb.ForeColor = isDark ? DarkTheme.TextBoxForeground : LightTheme.TextBoxForeground; }
                    else if (c is NumericUpDown n) { n.BackColor      = isDark ? DarkTheme.TextBoxBackground  : LightTheme.TextBoxBackground; n.ForeColor  = isDark ? DarkTheme.TextBoxForeground : LightTheme.TextBoxForeground; }
                    else if (c is CheckBox chk)    { chk.ForeColor    = isDark ? DarkTheme.LabelForeground    : LightTheme.LabelForeground; }
                    else if (c is Panel pnl)       { pnl.BackColor    = isDark ? DarkTheme.ControlBackground  : LightTheme.ControlBackground; }
                    else if (!(c is MenuStrip || c is ToolStrip || c is StatusStrip))
                    { c.ForeColor = isDark ? DarkTheme.TextForeground : LightTheme.TextForeground; }

                    if (c.Controls.Count > 0) ApplyToControls(c.Controls, isDark);
                }
                catch { }
            }
        }

        private static bool IsStatusLabel(Control c)
            => c is Label && c.Name != null &&
               (c.Name.StartsWith("stat") || c.Name.Contains("Value") || c.Name.Contains("Status"));
    }
}
