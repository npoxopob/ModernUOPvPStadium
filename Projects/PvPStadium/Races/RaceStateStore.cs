using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Server;
using Server.Logging;
using Server.Mobiles;
using PvPStadium.Mechanics;

namespace PvPStadium.Races;

internal sealed class RaceChoice
{
    // legacy: RaceIndex; new: RaceKey + Level
    public int? RaceIndex { get; set; }
    public string? RaceKey { get; set; }
    public int Level { get; set; }
    public bool NormalizedDone { get; set; }
    public long LastLifeDrainAtTicks { get; set; }
    public bool HasLifeDrainAbility { get; set; }
    public long HolyAuraActiveUntilTicks { get; set; }
    public long LastHolyAuraAtTicks { get; set; }
}

internal static class RaceStateStore
{
    private static readonly object _sync = new();
    private static readonly ConcurrentDictionary<int, RaceChoice> _bySerial = new();

    private static readonly string _folder = Path.Combine(Core.BaseDirectory, "Configuration", "PvPStadium");
    private static readonly string _filePath = Path.Combine(_folder, "races-state.json");

    private static readonly JsonSerializerOptions _jsonOpts = new(JsonSerializerOptions.Default)
    {
        WriteIndented = true
    };

    public static void Load()
    {
        try
        {
            Directory.CreateDirectory(_folder);
            if (!File.Exists(_filePath))
            {
                Save();
                return;
            }

            var json = File.ReadAllText(_filePath);
            _bySerial.Clear();
            // Попытка загрузить новый формат
            Dictionary<int, RaceChoice>? dataNew = null;
            try { dataNew = JsonSerializer.Deserialize<Dictionary<int, RaceChoice>>(json); } catch { }
            if (dataNew != null)
            {
                foreach (var kv in dataNew)
                {
                    _bySerial[kv.Key] = kv.Value ?? new RaceChoice();
                }
            }
            else
            {
                // Миграция со старого формата (serial -> raceIndex)
                var dataOld = JsonSerializer.Deserialize<Dictionary<int, int>>(json) ?? new();
                foreach (var kv in dataOld)
                {
                    _bySerial[kv.Key] = new RaceChoice { RaceIndex = kv.Value, NormalizedDone = true };
                }
                Save();
            }

            // Post-migration: convert RaceIndex -> RaceKey when possible
            foreach (var kv in _bySerial)
            {
                var c = kv.Value;
                if (c.RaceKey == null && c.RaceIndex is int idx)
                {
                    // map core indices to keys from defaults (best-effort)
                    c.RaceKey = idx switch { 1 => "elf", 3 => "gargoyle", _ => "human" };
                    c.Level = 1;
                }
            }
        }
        catch (Exception ex)
        {
            LogFactory.GetLogger(typeof(RaceStateStore)).Error(ex, "[PvP Stadium] Failed to load race state.");
        }
    }

    public static void Save()
    {
        try
        {
            Directory.CreateDirectory(_folder);
            var snap = new Dictionary<int, RaceChoice>(_bySerial);
            var json = JsonSerializer.Serialize(snap, _jsonOpts);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            LogFactory.GetLogger(typeof(RaceStateStore)).Error(ex, "[PvP Stadium] Failed to save race state.");
        }
    }

    public static bool TryGet(Serial serial, out RaceChoice? choice)
    {
        return _bySerial.TryGetValue(serial.ToInt32(), out choice);
    }

    public static RaceChoice GetOrCreate(Serial serial)
    {
        return _bySerial.GetOrAdd(serial.ToInt32(), _ => new RaceChoice());
    }

    public static void Set(Serial serial, int raceIndex)
    {
        var c = GetOrCreate(serial);
        c.RaceIndex = raceIndex;
    }

    public static void MarkNormalized(Serial serial)
    {
        var c = GetOrCreate(serial);
        c.NormalizedDone = true;
    }

    public static bool TryGetLifeDrainRemaining(PlayerMobile pm, out double remaining)
    {
        remaining = 0;
        if (!TryGet(pm.Serial, out var c) || c == null || c.LastLifeDrainAtTicks <= 0)
            return true;
        var last = new DateTime(c.LastLifeDrainAtTicks, DateTimeKind.Utc);
        var next = last.AddSeconds(VampireLifeDrain.Config.CooldownSec);
        var now = DateTime.UtcNow;
        if (now < next)
        {
            remaining = (next - now).TotalSeconds;
            return false;
        }
        return true;
    }

    public static void Remove(Serial serial)
    {
        _bySerial.TryRemove(serial.ToInt32(), out _);
    }
}
