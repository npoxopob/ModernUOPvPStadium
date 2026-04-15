using System;
using Server;
using Server.Logging;

namespace PvPStadium;

public static class Module
{
    // Вызывается на старте до загрузки мира (AssemblyHandler.Invoke("Configure"))
    public static void Configure()
    {
        // Загрузим конфигурации в начале
        PvPStadium.Configuration.Settings.Load();
        PvPStadium.Races.RaceRegistry.Load();
        PvPStadium.Races.RaceStateStore.Load();

        // Подписки на события/регистрации выполняем здесь, если нужно до World.Load
        EventSink.ServerStarted += OnServerStarted;
        // Подпишемся на события
        // Race selection gump now hooked via PlayerMobile.PlayerLoginEvent (see RaceLoginHook)
        // No EventSink.Connected subscription needed here
        EventSink.WorldSave += PvPStadium.Races.RaceStateStore.Save;
        // Passive vampire leech disabled per canon; no combat subscription.
    }

    // Вызывается после загрузки мира (AssemblyHandler.Invoke("Initialize"))
    public static void Initialize()
    {
        // Здесь можно прогреть кэш, зарегистрировать команды, и т.д.
        // Пример: простое сообщение в лог при инициализации модуля
        LogFactory.GetLogger(typeof(Module)).Information("[PvP Stadium] Module initialized.");
        // Регистрируем команды
        PvPStadium.Commands.RaceCommands.Initialize();
        PvPStadium.Commands.PvPRaceCommands.Initialize();

        // Загрузим конфиги механик Vampire/Paladin
        try
        {
            // Load Vampire mechanics
            var path = System.IO.Path.Combine(Core.BaseDirectory, "Configuration", "PvPStadium", "mechanics.vampire.json");
            if (System.IO.File.Exists(path))
            {
                var json = System.IO.File.ReadAllText(path);
                var anon = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);
                if (anon.TryGetProperty("ActiveLifeDrain", out var ald))
                {
                    PvPStadium.Mechanics.VampireLifeDrain.Config.Enabled = ald.TryGetProperty("Enabled", out var b) && b.ValueKind == System.Text.Json.JsonValueKind.True;
                    PvPStadium.Mechanics.VampireLifeDrain.Config.CooldownSec = ald.TryGetProperty("CooldownSec", out var c) && c.TryGetDouble(out var cd) ? cd : PvPStadium.Mechanics.VampireLifeDrain.Config.CooldownSec;
                    PvPStadium.Mechanics.VampireLifeDrain.Config.TickMs = ald.TryGetProperty("TickMs", out var t) && t.TryGetInt32(out var ti) ? ti : PvPStadium.Mechanics.VampireLifeDrain.Config.TickMs;
                    PvPStadium.Mechanics.VampireLifeDrain.Config.PerTickHP = ald.TryGetProperty("PerTickHP", out var h) && h.TryGetInt32(out var hp) ? hp : PvPStadium.Mechanics.VampireLifeDrain.Config.PerTickHP;
                    PvPStadium.Mechanics.VampireLifeDrain.Config.MaxDurationSec = ald.TryGetProperty("MaxDurationSec", out var ds) && ds.TryGetDouble(out var dur) ? dur : PvPStadium.Mechanics.VampireLifeDrain.Config.MaxDurationSec;
                    if (ald.TryGetProperty("PerTickHPByLevel", out var mapTick) && mapTick.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        var dict = new System.Collections.Generic.Dictionary<int,int>();
                        foreach (var prop in mapTick.EnumerateObject())
                        {
                            if (int.TryParse(prop.Name, out var lvl) && prop.Value.TryGetInt32(out var v))
                            {
                                dict[lvl] = v;
                            }
                        }

                        if (dict.Count > 0)
                        {
                            PvPStadium.Mechanics.VampireLifeDrain.Config.PerTickHPByLevel = dict;
                        }
                    }

                    if (ald.TryGetProperty("MaxDurationSecByLevel", out var mapDur) && mapDur.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        var dict = new System.Collections.Generic.Dictionary<int,double>();
                        foreach (var prop in mapDur.EnumerateObject())
                        {
                            if (int.TryParse(prop.Name, out var lvl) && prop.Value.TryGetDouble(out var v))
                            {
                                dict[lvl] = v;
                            }
                        }

                        if (dict.Count > 0)
                        {
                            PvPStadium.Mechanics.VampireLifeDrain.Config.MaxDurationSecByLevel = dict;
                        }
                    }
                    PvPStadium.Mechanics.VampireLifeDrain.Config.MaxRange = ald.TryGetProperty("MaxRange", out var mr) && mr.TryGetInt32(out var rng) ? rng : PvPStadium.Mechanics.VampireLifeDrain.Config.MaxRange;
                    if (ald.TryGetProperty("PaladinModifierEnabled", out var pme))
                    {
                        PvPStadium.Mechanics.VampireLifeDrain.Config.PaladinModifierEnabled = pme.ValueKind == System.Text.Json.JsonValueKind.True;
                    }
                    if (ald.TryGetProperty("PaladinPvpOnly", out var ppo))
                    {
                        PvPStadium.Mechanics.VampireLifeDrain.Config.PaladinPvpOnly = ppo.ValueKind == System.Text.Json.JsonValueKind.True;
                    }
                    if (ald.TryGetProperty("PaladinDefaultMultiplier", out var pdm) && pdm.TryGetDouble(out var dmult))
                    {
                        PvPStadium.Mechanics.VampireLifeDrain.Config.PaladinDefaultMultiplier = dmult;
                    }
                    if (ald.TryGetProperty("PaladinPerTickMultiplierByLevel", out var pmap) && pmap.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        var dictP = new System.Collections.Generic.Dictionary<int,double>();
                        foreach (var prop in pmap.EnumerateObject())
                        {
                            if (int.TryParse(prop.Name, out var lvl) && prop.Value.TryGetDouble(out var v))
                            {
                                dictP[lvl] = v;
                            }
                        }

                        if (dictP.Count > 0)
                        {
                            PvPStadium.Mechanics.VampireLifeDrain.Config.PaladinPerTickMultiplierByLevel = dictP;
                        }
                    }
                }
            }

            // Load Paladin mechanics
            var pathP = System.IO.Path.Combine(Core.BaseDirectory, "Configuration", "PvPStadium", "mechanics.paladin.json");
            if (System.IO.File.Exists(pathP))
            {
                var jsonP = System.IO.File.ReadAllText(pathP);
                var anonP = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(jsonP);
                if (anonP.TryGetProperty("ActiveHolyAura", out var aha))
                {
                    PvPStadium.Mechanics.PaladinHolyAura.Config.Enabled = aha.TryGetProperty("Enabled", out var b) && b.ValueKind == System.Text.Json.JsonValueKind.True;
                    if (aha.TryGetProperty("BlocksLifeDrain", out var bl) && bl.ValueKind == System.Text.Json.JsonValueKind.True)
                    {
                        PvPStadium.Mechanics.PaladinHolyAura.Config.BlocksLifeDrain = true;
                    }
                    else
                    {
                        PvPStadium.Mechanics.PaladinHolyAura.Config.BlocksLifeDrain = false;
                    }

                    if (aha.TryGetProperty("Multiplier", out var mul) && mul.TryGetDouble(out var mm))
                    {
                        PvPStadium.Mechanics.PaladinHolyAura.Config.Multiplier = mm;
                    }
                    if (aha.TryGetProperty("DurationByLevel", out var mapDurP) && mapDurP.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        var dict = new System.Collections.Generic.Dictionary<int,double>();
                        foreach (var prop in mapDurP.EnumerateObject())
                        {
                            if (int.TryParse(prop.Name, out var lvlP) && prop.Value.TryGetDouble(out var vP))
                            {
                                dict[lvlP] = vP;
                            }
                        }

                        if (dict.Count > 0)
                        {
                            PvPStadium.Mechanics.PaladinHolyAura.Config.DurationByLevel = dict;
                        }
                    }

                    if (aha.TryGetProperty("CooldownByLevel", out var mapCdP) && mapCdP.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        var dictCd = new System.Collections.Generic.Dictionary<int,double>();
                        foreach (var prop in mapCdP.EnumerateObject())
                        {
                            if (int.TryParse(prop.Name, out var lvlP) && prop.Value.TryGetDouble(out var vP))
                            {
                                dictCd[lvlP] = vP;
                            }
                        }

                        if (dictCd.Count > 0)
                        {
                            PvPStadium.Mechanics.PaladinHolyAura.Config.CooldownByLevel = dictCd;
                        }
                    }

                    if (aha.TryGetProperty("RadiusByLevel", out var mapRadP) && mapRadP.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        var dictRad = new System.Collections.Generic.Dictionary<int,int>();
                        foreach (var prop in mapRadP.EnumerateObject())
                        {
                            if (int.TryParse(prop.Name, out var lvlP) && prop.Value.TryGetInt32(out var vP))
                            {
                                dictRad[lvlP] = vP;
                            }
                        }

                        if (dictRad.Count > 0)
                        {
                            PvPStadium.Mechanics.PaladinHolyAura.Config.RadiusByLevel = dictRad;
                        }
                    }

                    if (aha.TryGetProperty("DamageReductionByLevel", out var mapRed) && mapRed.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        var dictRed = new System.Collections.Generic.Dictionary<int,double>();
                        foreach (var prop in mapRed.EnumerateObject())
                        {
                            if (int.TryParse(prop.Name, out var lvlP) && prop.Value.TryGetDouble(out var vP))
                            {
                                dictRed[lvlP] = vP;
                            }
                        }

                        if (dictRed.Count > 0)
                        {
                            PvPStadium.Mechanics.PaladinHolyAura.Config.DamageReductionByLevel = dictRed;
                        }
                    }
                }
            }
        }
        catch { }

        // Register ability commands
        PvPStadium.Mechanics.VampireLifeDrain.RegisterCommand();
        PvPStadium.Mechanics.PaladinHolyAura.RegisterCommands();

    }

    private static void OnServerStarted()
    {
        LogFactory.GetLogger(typeof(Module)).Information("[PvP Stadium] Server started. Module is active.");
    }
}
