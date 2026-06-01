using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ACEhole
{
    public partial class LogViewerForm : Form
    {
        private AppConfig _config;

        // ── Log type / date state ─────────────────────────────────────────
        private static readonly string[] LogTypeNames  = { "Application", "Console" };
        private static readonly Func<string>[] LogDirs = {
            () => Logger.AppLogPath,
            () => Logger.ConsoleLogPath
        };
        private static readonly string[] LogPrefixes = { "AppLog", "ConsoleLog" };

        private List<DateTime> _availableDates = new List<DateTime>();
        private DateTime       _currentDate    = DateTime.Today;

        // ── Live-tail state ───────────────────────────────────────────────
        private System.Windows.Forms.Timer _liveTimer;
        private string  _liveFilePath     = null;
        private long    _liveFilePosition = 0;
        private bool    _autoScroll           = true;
        private bool    _suppressScrollEvents = false;

        private const int MaxInitialLines = 1000;

        // ── Constructor ───────────────────────────────────────────────────
        public LogViewerForm(AppConfig config)
        {
            InitializeComponent();
            _config = config;

            // Restore position
            if (_config.LogViewer.WindowX >= 0 && _config.LogViewer.WindowY >= 0)
            {
                StartPosition = FormStartPosition.Manual;
                var b = new Rectangle(_config.LogViewer.WindowX, _config.LogViewer.WindowY, Width, Height);
                bool onScreen = Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(b));
                Location = onScreen
                    ? new Point(_config.LogViewer.WindowX, _config.LogViewer.WindowY)
                    : new Point(100, 100);
            }
            this.TopMost       = _config.LogViewer.AlwaysOnTop;
            this.ShowInTaskbar = _config.LogViewer.ShowInTaskbar;

            _liveTimer          = new System.Windows.Forms.Timer();
            _liveTimer.Interval = 1000;
            _liveTimer.Tick    += LiveTimer_Tick;

            logRTB.VScroll    += LogRTB_VScroll;
            logRTB.MouseWheel += LogRTB_MouseWheel;

            int savedIdx = Math.Max(0, Math.Min(_config.LogViewer.LastLogTypeIndex, LogTypeNames.Length - 1));
            logTypeCombo.SelectedIndexChanged -= logTypeCombo_SelectedIndexChanged;
            logTypeCombo.SelectedIndex = savedIdx;
            logTypeCombo.SelectedIndexChanged += logTypeCombo_SelectedIndexChanged;
        }

        // ── Shown (first paint) ───────────────────────────────────────────
        private void LogViewerForm_Shown(object sender, EventArgs e)
        {
            RefreshAvailableDates();
            LoadCurrentLogAsync();
        }

        // ── Log type / date navigation ────────────────────────────────────
        private void logTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            _config.LogViewer.LastLogTypeIndex = logTypeCombo.SelectedIndex;
            RefreshAvailableDates();
            LoadCurrentLogAsync();
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            int idx = _availableDates.IndexOf(_currentDate);
            if (idx > 0) { _currentDate = _availableDates[idx - 1]; UpdateDateLabel(); LoadCurrentLogAsync(); }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            int idx = _availableDates.IndexOf(_currentDate);
            if (idx >= 0 && idx < _availableDates.Count - 1) { _currentDate = _availableDates[idx + 1]; UpdateDateLabel(); LoadCurrentLogAsync(); }
        }

        // ── Save / Delete ─────────────────────────────────────────────────
        private void saveLogButton_Click(object sender, EventArgs e)
        {
            string path = GetCurrentLogFilePath();
            if (path == null) { MessageBox.Show("No log file to save.", "Save Log", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            using var dlg = new SaveFileDialog { Filter = "Log Files|*.log|All Files|*.*", FileName = Path.GetFileName(path) };
            if (dlg.ShowDialog() == DialogResult.OK) { try { File.Copy(path, dlg.FileName, true); } catch (Exception ex) { MessageBox.Show(ex.Message, "Save Log", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
        }

        private void deleteLogButton_Click(object sender, EventArgs e)
        {
            string path = GetCurrentLogFilePath();
            if (path == null) { MessageBox.Show("No log file to delete.", "Delete Log", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            if (MessageBox.Show($"Delete '{Path.GetFileName(path)}'?", "Delete Log", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try { _liveTimer.Stop(); File.Delete(path); logRTB.Clear(); _liveFilePath = null; _liveFilePosition = 0; RefreshAvailableDates(); UpdateDateLabel(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Delete Log", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ── Position persistence ──────────────────────────────────────────
        private void LogViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _liveTimer.Stop();
            _liveTimer.Dispose();
            _config.LogViewer.WindowX          = this.Left;
            _config.LogViewer.WindowY          = this.Top;
            _config.LogViewer.LastLogTypeIndex = logTypeCombo.SelectedIndex;
            ConfigManager.SaveConfig(_config);
        }

        // ── Date helpers ──────────────────────────────────────────────────
        private void RefreshAvailableDates()
        {
            _availableDates.Clear();
            string dir = GetLogDir();
            if (dir == null || !Directory.Exists(dir)) { UpdateDateLabel(); return; }

            string prefix = LogPrefixes[logTypeCombo.SelectedIndex];
            var dateRx = new Regex(@"\d{4}-\d{2}-\d{2}");
            var dates = new HashSet<DateTime>();

            foreach (string f in Directory.GetFiles(dir, $"{prefix}_*.log"))
            {
                var m = dateRx.Match(Path.GetFileNameWithoutExtension(f));
                if (m.Success && DateTime.TryParse(m.Value, out DateTime d)) dates.Add(d.Date);
            }

            _availableDates = dates.OrderBy(d => d).ToList();
            if (_availableDates.Count > 0 && !_availableDates.Contains(_currentDate))
                _currentDate = _availableDates.Last();

            UpdateDateLabel();
        }

        private void UpdateDateLabel()
        {
            dateLabel.Text = _availableDates.Count == 0 ? "No logs" : _currentDate.ToString("yyyy-MM-dd");
            int idx = _availableDates.IndexOf(_currentDate);
            prevButton.Enabled = idx > 0;
            nextButton.Enabled = idx >= 0 && idx < _availableDates.Count - 1;
        }

        private string GetLogDir() => logTypeCombo.SelectedIndex >= 0 && logTypeCombo.SelectedIndex < LogDirs.Length
            ? LogDirs[logTypeCombo.SelectedIndex]() : null;

        private string GetCurrentLogFilePath()
        {
            string dir    = GetLogDir();
            string prefix = logTypeCombo.SelectedIndex >= 0 ? LogPrefixes[logTypeCombo.SelectedIndex] : null;
            if (dir == null || prefix == null || _availableDates.Count == 0) return null;
            string path = Path.Combine(dir, $"{prefix}_{_currentDate:yyyy-MM-dd}.log");
            return File.Exists(path) ? path : null;
        }

        private List<string> GetCurrentDateLogFiles()
        {
            string dir    = GetLogDir();
            string prefix = logTypeCombo.SelectedIndex >= 0 ? LogPrefixes[logTypeCombo.SelectedIndex] : null;
            if (dir == null || prefix == null || !Directory.Exists(dir)) return new List<string>();
            return Directory.GetFiles(dir, $"{prefix}_{_currentDate:yyyy-MM-dd}*.log").OrderBy(f => f).ToList();
        }

        // ── Async initial load + live-tail start ──────────────────────────
        private async void LoadCurrentLogAsync()
        {
            _liveTimer.Stop();
            logRTB.Clear();
            _autoScroll = true;

            var files = GetCurrentDateLogFiles();
            if (files.Count == 0)
            {
                logRTB.ForeColor = Color.Gray;
                logRTB.AppendText(_availableDates.Count == 0
                    ? "No log files found." : "No log file found for this date.");
                StartOrStopLiveTail();
                return;
            }

            logRTB.ForeColor = Color.Gray;
            logRTB.AppendText("Loading...");
            saveLogButton.Enabled = deleteLogButton.Enabled = false;

            int typeIdx = logTypeCombo.SelectedIndex;
            string rtf;
            try
            {
                rtf = await System.Threading.Tasks.Task.Run(() =>
                {
                    var lines = ReadColoredLines(files, typeIdx);
                    return lines.Count > 0 ? BuildRtf(lines) : null;
                });
            }
            catch (Exception ex)
            {
                logRTB.Clear(); logRTB.ForeColor = Color.Gray;
                logRTB.AppendText($"Error reading log: {ex.Message}");
                saveLogButton.Enabled = deleteLogButton.Enabled = true;
                StartOrStopLiveTail();
                return;
            }

            if (rtf == null)
            {
                logRTB.Clear(); logRTB.ForeColor = Color.Gray;
                logRTB.AppendText("Log file is empty.");
            }
            else
            {
                logRTB.Rtf = rtf;
                logRTB.SelectionStart = logRTB.TextLength;
                logRTB.ScrollToCaret();
            }

            saveLogButton.Enabled = deleteLogButton.Enabled = true;
            StartOrStopLiveTail();
        }

        // ── Live tail ─────────────────────────────────────────────────────
        private void StartOrStopLiveTail()
        {
            _liveFilePath     = GetCurrentLogFilePath();
            _liveFilePosition = 0;
            if (_liveFilePath != null && File.Exists(_liveFilePath))
                _liveFilePosition = new FileInfo(_liveFilePath).Length;

            if (_currentDate.Date == DateTime.Today)
                _liveTimer.Start();
        }

        private void LiveTimer_Tick(object sender, EventArgs e)
        {
            if (_currentDate.Date != DateTime.Today)
            {
                _currentDate = DateTime.Today;
                RefreshAvailableDates();
                UpdateDateLabel();
                LoadCurrentLogAsync();
                return;
            }

            string cur = GetCurrentLogFilePath();
            if (cur != _liveFilePath) { _liveFilePath = cur; _liveFilePosition = 0; }

            if (_liveFilePath == null || !File.Exists(_liveFilePath)) return;

            try
            {
                long len = new FileInfo(_liveFilePath).Length;
                if (len <= _liveFilePosition) return;

                string newText;
                using var fs = new FileStream(_liveFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fs.Seek(_liveFilePosition, SeekOrigin.Begin);
                using var rdr = new System.IO.StreamReader(fs);
                newText = rdr.ReadToEnd();
                _liveFilePosition = len;

                if (!string.IsNullOrEmpty(newText)) AppendLiveLines(newText);
            }
            catch { }
        }

        private void AppendLiveLines(string newText)
        {
            bool wasAtBottom = _autoScroll;
            int typeIdx = logTypeCombo.SelectedIndex;

            _suppressScrollEvents = true;
            try
            {
                foreach (string raw in newText.Split('\n'))
                {
                    string line = raw.TrimEnd('\r');
                    if (string.IsNullOrEmpty(line)) continue;

                    Color color = GetLineColor(line, typeIdx);
                    logRTB.SelectionStart  = logRTB.TextLength;
                    logRTB.SelectionLength = 0;
                    logRTB.SelectionColor  = color;
                    logRTB.AppendText(line + "\n");
                }
            }
            finally { _suppressScrollEvents = false; }

            if (wasAtBottom)
            {
                _suppressScrollEvents = true;
                logRTB.SelectionStart = logRTB.TextLength;
                logRTB.ScrollToCaret();
                _suppressScrollEvents = false;
            }
            else
            {
                _suppressScrollEvents = true;
                int visIdx = logRTB.GetCharIndexFromPosition(new Point(0, 0));
                logRTB.SelectionStart = visIdx; logRTB.SelectionLength = 0;
                _suppressScrollEvents = false;
            }
        }

        // ── Scroll tracking ───────────────────────────────────────────────
        private void LogRTB_VScroll(object sender, EventArgs e)
        {
            if (_suppressScrollEvents || logRTB.TextLength == 0) { _autoScroll = true; return; }
            var lastPos = logRTB.GetPositionFromCharIndex(logRTB.TextLength - 1);
            _autoScroll = lastPos.Y >= 0 && lastPos.Y < logRTB.ClientSize.Height;
            if (!_autoScroll) AnchorCaretToVisible();
        }

        private void LogRTB_MouseWheel(object sender, MouseEventArgs e)
        {
            if (_suppressScrollEvents || logRTB.TextLength == 0) { _autoScroll = true; return; }
            var lastPos = logRTB.GetPositionFromCharIndex(logRTB.TextLength - 1);
            _autoScroll = lastPos.Y >= 0 && lastPos.Y < logRTB.ClientSize.Height;
            if (!_autoScroll) AnchorCaretToVisible();
        }

        private void AnchorCaretToVisible()
        {
            _suppressScrollEvents = true;
            int visIdx = logRTB.GetCharIndexFromPosition(new Point(0, 0));
            logRTB.SelectionStart = visIdx; logRTB.SelectionLength = 0;
            _suppressScrollEvents = false;
        }

        // ── Color coding ──────────────────────────────────────────────────
        private Color GetLineColor(string line, int logTypeIndex)
        {
            if (!_config.Colors.EnableLogViewerColors) return Color.FromArgb(212, 212, 212);

            if (logTypeIndex == 0)
            {
                if (line.Contains("[INFO]"))  return ParseHex(_config.Colors.AppInfoColor);
                if (line.Contains("[WARN]"))  return ParseHex(_config.Colors.AppWarnColor);
                if (line.Contains("[ERROR]")) return ParseHex(_config.Colors.AppErrorColor);
                if (line.Contains("[DEBUG]")) return ParseHex(_config.Colors.AppDebugColor);
                return Color.FromArgb(180, 180, 180);
            }
            else
            {
                var type = ConsoleLineClassifier.Classify(line);
                return ConsoleLineClassifier.GetColor(type, _config);
            }
        }

        private static Color ParseHex(string hex)
        {
            try { return ColorTranslator.FromHtml(hex.StartsWith("#") ? hex : "#" + hex); }
            catch { return Color.Gray; }
        }

        // ── Bulk read for initial load ────────────────────────────────────
        private List<(string Text, Color LineColor)> ReadColoredLines(List<string> files, int typeIdx)
        {
            var rawLines = new List<string>();
            foreach (string f in files)
            {
                if (!File.Exists(f)) continue;
                try
                {
                    using var fs = new FileStream(f, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using var rdr = new System.IO.StreamReader(fs);
                    string line;
                    while ((line = rdr.ReadLine()) != null)
                        if (!string.IsNullOrEmpty(line)) rawLines.Add(line);
                }
                catch (IOException) { }
            }

            if (rawLines.Count > MaxInitialLines)
                rawLines = rawLines.GetRange(rawLines.Count - MaxInitialLines, MaxInitialLines);

            return rawLines.Select(l => (l, GetLineColor(l, typeIdx))).ToList();
        }

        // ── RTF builder (runs on background thread) ───────────────────────
        private static string BuildRtf(List<(string Text, Color LineColor)> lines)
        {
            var colorMap   = new Dictionary<Color, int>();
            var colorTable = new StringBuilder(@"{\colortbl ;");

            foreach (var (_, c) in lines)
            {
                if (!colorMap.ContainsKey(c))
                {
                    colorMap[c] = colorMap.Count + 1;
                    colorTable.Append($@"\red{c.R}\green{c.G}\blue{c.B};");
                }
            }
            colorTable.Append("}");

            var sb = new StringBuilder(lines.Count * 120);
            sb.Append(@"{\rtf1\ansi\deff0");
            sb.Append(colorTable);

            foreach (var (text, color) in lines)
            {
                sb.Append($@"\cf{colorMap[color]} ");
                sb.Append(EscapeRtf(text));
                sb.Append(@"\line");
            }
            sb.Append("}");
            return sb.ToString();
        }

        private static string EscapeRtf(string text)
        {
            var sb = new StringBuilder(text.Length + 8);
            foreach (char c in text)
            {
                if      (c == '\\') sb.Append(@"\\");
                else if (c == '{')  sb.Append(@"\{");
                else if (c == '}')  sb.Append(@"\}");
                else if (c > 127)   sb.Append($@"\u{(int)c}?");
                else                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
