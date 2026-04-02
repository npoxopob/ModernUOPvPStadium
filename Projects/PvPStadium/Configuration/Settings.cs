using System;
using System.IO;
using System.Text.Json;
using Server;
using Server.Logging;

namespace PvPStadium.Configuration;

public sealed class Settings
{
    public bool NormalizeOnFirstLogin { get; set; } = true;
    public bool ShowRaceGumpOnFirstLogin { get; set; } = false;

    private static readonly string Folder = Path.Combine(Core.BaseDirectory, "Configuration", "PvPStadium");
    private static readonly string FilePath = Path.Combine(Folder, "settings.json");

    public static Settings Current { get; private set; } = new();

    public static void Load()
    {
        try
        {
            Directory.CreateDirectory(Folder);
            if (!File.Exists(FilePath))
            {
                // write defaults
                var jsonDefault = JsonSerializer.Serialize(new Settings(), new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, jsonDefault);
                Current = new Settings();
                return;
            }

            var json = File.ReadAllText(FilePath);
            var loaded = JsonSerializer.Deserialize<Settings>(json);
            Current = loaded ?? new Settings();
        }
        catch (Exception ex)
        {
            LogFactory.GetLogger(typeof(Settings)).Error(ex, "[PvP Stadium] Failed to load settings, using defaults.");
            Current = new Settings();
        }
    }
}
