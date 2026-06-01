using System;

namespace ACEhole
{
    public class ModInfo
    {
        public int    Number       { get; set; }
        public string Name         { get; set; }
        public string Version      { get; set; }
        public string Author       { get; set; }
        public string LoadTime     { get; set; }
        public string Memory       { get; set; }
        public string Filename     { get; set; }
        public bool   IsLoaded     { get; set; }
        public bool   HasError     { get; set; }
        public string ErrorMessage { get; set; }
        public long   MemoryBytes  { get; set; }

        public ModInfo()
        {
            Name = ""; Version = ""; Author = ""; LoadTime = "";
            Memory = ""; Filename = ""; IsLoaded = true;
            HasError = false; ErrorMessage = ""; MemoryBytes = 0;
        }

        public void ParseMemory()
        {
            if (string.IsNullOrEmpty(Memory)) { MemoryBytes = 0; return; }
            try
            {
                var parts = Memory.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2 || !double.TryParse(parts[0], out double value)) { MemoryBytes = 0; return; }
                MemoryBytes = parts[1].ToUpperInvariant() switch
                {
                    "B"  => (long)value,
                    "KB" => (long)(value * 1024),
                    "MB" => (long)(value * 1024 * 1024),
                    "GB" => (long)(value * 1024 * 1024 * 1024),
                    _    => 0
                };
            }
            catch { MemoryBytes = 0; }
        }
    }
}
