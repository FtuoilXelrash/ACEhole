using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ACEhole
{
    public enum ServerState
    {
        Stopped,
        Starting,
        Running,
        Stopping,
        Crashed,
        Restarting
    }

    public class ProcessManager : IDisposable
    {
        public ServerState State     { get; private set; } = ServerState.Stopped;
        public bool IsRunning        => _process != null && !_process.HasExited;
        public int  Pid              => (_process != null && !_process.HasExited) ? _process.Id : 0;

        public event EventHandler<ServerState> StateChanged;
        public event EventHandler<string>      OutputReceived;
        public event EventHandler<int>         ServerExited;
        public event EventHandler              ServerStarted;

        private Process   _process;
        private AppConfig _config;
        private CancellationTokenSource _cts;
        private bool _intentionalStop = false;
        private bool _disposed        = false;

        // Strip ANSI/VT escape sequences
        private static readonly Regex _ansiRegex =
            new Regex(@"\x1B\[[\d;]*[A-Za-z]|\x1B.", RegexOptions.Compiled);

        public ProcessManager(AppConfig config) { _config = config; }
        public void UpdateConfig(AppConfig config) { _config = config; }

        // ── Public API ─────────────────────────────────────────────────

        public async Task StartServerAsync()
        {
            if (IsRunning) { Logger.Warning("StartServerAsync: already running."); return; }

            _intentionalStop = false;
            _cts = new CancellationTokenSource();

            Logger.Info("=== Server start sequence ===");
            Logger.Debug($"  Exe:     {_config.Server.AceExePath}");
            Logger.Debug($"  LogFile: {_config.Server.LogFilePath}");

            try
            {
                LaunchServer();
            }
            catch (OperationCanceledException)
            {
                Logger.Info("Start sequence cancelled.");
                SetState(ServerState.Stopped);
            }
            catch (Exception ex)
            {
                OnOutput($"[ACEhole] ERROR starting server: {ex.Message}");
                Logger.Error("StartServerAsync failed", ex);
                SetState(ServerState.Stopped);
            }

            await Task.CompletedTask;
        }

        public async Task StopServerAsync()
        {
            Logger.Info($"StopServerAsync — IsRunning={IsRunning}");
            _intentionalStop = true;
            _cts?.Cancel();

            if (!IsRunning) { SetState(ServerState.Stopped); return; }

            SetState(ServerState.Stopping);
            OnOutput("[ACEhole] Sending 'quit' to server...");
            try { SendCommand("quit"); } catch { }

            for (int i = 0; i < 20 && IsRunning; i++)
                await Task.Delay(500);

            if (IsRunning)
            {
                OnOutput("[ACEhole] Force-killing server process...");
                Logger.Warning("Force-killing server after 10s wait.");
                try { _process.Kill(); } catch { }
            }

            Logger.Info("Server stopped.");
            SetState(ServerState.Stopped);
        }

        public async Task RestartServerAsync()
        {
            Logger.Info("Restart requested.");
            SetState(ServerState.Restarting);
            await StopServerAsync();
            await Task.Delay(_config.Server.RestartDelaySecs * 1000);
            await StartServerAsync();
        }

        public void SendCommand(string command)
        {
            if (!IsRunning || _process.StandardInput == null)
            {
                Logger.Warning($"SendCommand ignored — server not running: {command}");
                return;
            }
            try
            {
                _process.StandardInput.WriteLine(command);
                Logger.Debug($"stdin > {command}");
            }
            catch (Exception ex) { Logger.Warning($"SendCommand failed: {ex.Message}"); }
        }

        public long GetWorkingSetBytes()
        {
            if (!IsRunning) return 0;
            try { _process.Refresh(); return _process.WorkingSet64; }
            catch { return 0; }
        }

        public TimeSpan GetTotalProcessorTime()
        {
            if (!IsRunning) return TimeSpan.Zero;
            try { _process.Refresh(); return _process.TotalProcessorTime; }
            catch { return TimeSpan.Zero; }
        }

        // ── Launch ─────────────────────────────────────────────────────

        private void LaunchServer()
        {
            SetState(ServerState.Starting);

            string aceDir = Path.GetDirectoryName(_config.Server.AceExePath);

            OnOutput("[ACEhole] ─────────────────────────────────────────────");
            OnOutput($"[ACEhole] Exe:     {_config.Server.AceExePath}");
            OnOutput($"[ACEhole] WorkDir: {aceDir}");
            OnOutput($"[ACEhole] Tailing: {_config.Server.LogFilePath}");
            OnOutput("[ACEhole] ─────────────────────────────────────────────");

            Logger.Info("Launching ACE.Server.exe");
            Logger.Debug($"WorkDir: {aceDir}");

            var psi = new ProcessStartInfo
            {
                FileName               = _config.Server.AceExePath,
                Arguments              = "",
                WorkingDirectory       = aceDir,   // CRITICAL: ACE loads config files from working dir
                UseShellExecute        = false,
                CreateNoWindow         = true,
                WindowStyle            = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = false,    // tailing log file instead
                RedirectStandardError  = true,
                RedirectStandardInput  = true
            };

            _process = new Process { StartInfo = psi, EnableRaisingEvents = true };
            _process.ErrorDataReceived += Process_Error;
            _process.Exited            += Process_Exited;

            _process.Start();
            _process.BeginErrorReadLine();

            Logger.StartConsoleLog();
            Logger.Info($"Process started — PID {_process.Id}");

            SetState(ServerState.Running);
            ServerStarted?.Invoke(this, EventArgs.Empty);

            // Tail the ACE log file for all console output
            _ = TailLogFileAsync(_config.Server.LogFilePath, _cts.Token);
        }

        // ── Log file tailer ────────────────────────────────────────────
        // Primary console source — immune to CLS/stdout-stall issues.

        private async Task TailLogFileAsync(string logPath, CancellationToken ct)
        {
            OnOutput($"[ACEhole] Waiting for log file: {logPath}");
            Logger.Debug($"Log tailer waiting for: {logPath}");

            int waited = 0;
            while (!File.Exists(logPath) && waited < 120_000 && !ct.IsCancellationRequested)
            {
                await Task.Delay(500, CancellationToken.None);
                waited += 500;
            }

            if (!File.Exists(logPath))
            {
                OnOutput("[ACEhole] Log file did not appear within 2 minutes — check log file path in Settings.");
                Logger.Warning($"Log file never appeared: {logPath}");
                return;
            }

            OnOutput("[ACEhole] Log file active.");
            Logger.Info($"Log tailer started: {logPath}");

            try
            {
                using var stream = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                using var reader = new System.IO.StreamReader(stream);

                // Seek to end so we only show new output, not existing log history
                stream.Seek(0, SeekOrigin.End);

                while (IsRunning && !ct.IsCancellationRequested)
                {
                    string line = reader.ReadLine();
                    if (line != null)
                    {
                        line = _ansiRegex.Replace(line, "");
                        if (string.IsNullOrEmpty(line)) continue;

                        OnOutput(line);
                        Logger.LogConsole(line);
                    }
                    else
                    {
                        await Task.Delay(80, CancellationToken.None);
                    }
                }
            }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                Logger.Warning($"Log tailer error: {ex.Message}");
            }

            Logger.Debug("Log tailer stopped.");
        }

        // ── Process events ─────────────────────────────────────────────

        private void Process_Error(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;
            string line = $"[STDERR] {e.Data}";
            OnOutput(line);
            Logger.Debug(line);
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            int code = 0;
            try { code = _process?.ExitCode ?? 0; } catch { }
            Logger.Info($"Server exited. Code={code}  Intentional={_intentionalStop}");
            ServerExited?.Invoke(this, code);

            if (!_intentionalStop && _config.Server.AutoRestartOnCrash)
            {
                SetState(ServerState.Crashed);
                int delay = _config.Server.RestartDelaySecs;
                OnOutput($"[ACEhole] Server crashed (exit {code}) — auto-restarting in {delay}s...");
                Logger.Warning($"Crash detected (code {code}). Restart in {delay}s.");
                _ = Task.Run(async () =>
                {
                    await Task.Delay(delay * 1000);
                    if (!_intentionalStop) { Logger.Info("Auto-restart."); await StartServerAsync(); }
                });
            }
            else if (!_intentionalStop)
            {
                SetState(ServerState.Crashed);
            }
        }

        private void OnOutput(string line) => OutputReceived?.Invoke(this, line);

        private void SetState(ServerState state)
        {
            var prev = State;
            State = state;
            Logger.Info($"State: {prev} → {state}");
            StateChanged?.Invoke(this, state);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _cts?.Cancel();
            try { if (IsRunning) _process?.Kill(); } catch { }
            _process?.Dispose();
            Logger.Debug("ProcessManager disposed.");
        }
    }
}
