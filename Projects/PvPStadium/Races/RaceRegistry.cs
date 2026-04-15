using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Server;

namespace PvPStadium.Races;

public sealed class RaceLevelSpec
{
    public int Level { get; set; }
    public string Name { get; set; } = "";
}

public sealed class RaceSpec
{
    public string Key { get; set; } = "";      // stable id (e.g., "vampire")
    public string Display { get; set; } = "";  // shown in lists
    public bool Enabled { get; set; } = true;
    public List<RaceLevelSpec> Levels { get; set; } = new();
}

internal static class RaceRegistry
{
    private static readonly string Folder = Path.Combine(Core.BaseDirectory, "Configuration", "PvPStadium");
    private static readonly string FilePath = Path.Combine(Folder, "races.json");

    private static readonly Dictionary<string, RaceSpec> _byKey = new(StringComparer.OrdinalIgnoreCase);
    public static IReadOnlyList<RaceSpec> Specs { get; private set; } = Array.Empty<RaceSpec>();

    public static void Load()
    {
        Directory.CreateDirectory(Folder);
        if (!File.Exists(FilePath))
        {
            var defaults = Array.Empty<RaceSpec>();
            File.WriteAllText(FilePath, JsonSerializer.Serialize(defaults, new JsonSerializerOptions { WriteIndented = true }));
            Specs = defaults;
            _byKey.Clear();
            return;
        }

        var json = File.ReadAllText(FilePath);
        var list = JsonSerializer.Deserialize<List<RaceSpec>>(json) ?? new();
        var enabled = list.Where(s => s.Enabled).ToArray();
        Specs = enabled;
        _byKey.Clear();
        foreach (var s in list)
        {
            _byKey[s.Key] = s; // keep all for lookup by key
        }
    }

    public static RaceSpec? GetSpec(string key)
        => key != null && _byKey.TryGetValue(key, out var spec) ? spec : null;

    public static string GetLevelName(string key, int level)
    {
        var spec = GetSpec(key);
        if (spec == null)
        {
            return key;
        }
        var lv = spec.Levels?.FirstOrDefault(l => l.Level == level);
        return lv?.Name ?? spec.Display;
    }

}
