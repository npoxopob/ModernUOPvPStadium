using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Logging;

namespace PvPStadium.Mechanics;

public static class PaladinHolyAura
{
    public struct AuraConfig
    {
        public bool Enabled;
        public Dictionary<int,double> CooldownByLevel;   // seconds per level
        public Dictionary<int,double> DurationByLevel;   // seconds per level
        public Dictionary<int,int> RadiusByLevel;        // tiles per level
        public bool BlocksLifeDrain;
        public double Multiplier;
        public Dictionary<int,double> DamageReductionByLevel; // incoming damage scalar per level
    }

    public static AuraConfig Config = new AuraConfig
    {
        Enabled = true,
        CooldownByLevel = new Dictionary<int,double> { {1,180},{2,150},{3,120},{4,90} },
        DurationByLevel = new Dictionary<int,double> { {1,15},{2,18},{3,21},{4,24} },
        RadiusByLevel = new Dictionary<int,int> { {1,6},{2,7},{3,8},{4,9} },
        BlocksLifeDrain = true,
        Multiplier = 0.5,
        DamageReductionByLevel = new Dictionary<int,double> { {1,0.90},{2,0.85},{3,0.80},{4,0.75} }
    };

    private static readonly ILogger _log = LogFactory.GetLogger(typeof(PaladinHolyAura));

    public static void RegisterCommands()
    {
        CommandSystem.Register("HolyAura", AccessLevel.Player, e =>
        {
            if (e.Mobile is not PlayerMobile pm)
            {
                return;
            }
            if (!IsPaladin(pm))
            {
                pm.SendMessage(0x22, "Эта способность доступна только паладину.");
                return;
            }
            if (!Config.Enabled)
            {
                pm.SendMessage(0x22, "Святая аура временно недоступна.");
                return;
            }

            if (!CheckCooldown(pm, out var remaining))
            {
                pm.SendMessage(0x22, $"Перезарядка: {Math.Ceiling(remaining)} сек.");
                return;
            }

            var st = PvPStadium.Races.RaceStateStore.GetOrCreate(pm.Serial);
            var lvl = st.Level > 0 ? st.Level : 1;
            var dur = (Config.DurationByLevel != null && Config.DurationByLevel.TryGetValue(lvl, out var d)) ? d : 10.0;

            var until = DateTime.UtcNow.AddSeconds(dur).Ticks;
            st.HolyAuraActiveUntilTicks = until;
            st.LastHolyAuraAtTicks = DateTime.UtcNow.Ticks;
            PvPStadium.Races.RaceStateStore.Save();

            pm.SendMessage(0x44, $"Святая аура активна на {dur} сек.");
        });
    }

    private static bool IsPaladin(PlayerMobile pm)
    {
        return PvPStadium.Races.RaceStateStore.TryGet(pm.Serial, out var st) && st != null && st.RaceKey == "paladin";
    }

    private static bool CheckCooldown(PlayerMobile pm, out double remainingSec)
    {
        remainingSec = 0;
        var st = PvPStadium.Races.RaceStateStore.GetOrCreate(pm.Serial);
        var now = DateTime.UtcNow;
        var lastTicks = st.LastHolyAuraAtTicks;
        // resolve cooldown by paladin level
        var lvl = st.Level > 0 ? st.Level : 1;
        var cd = (Config.CooldownByLevel != null && Config.CooldownByLevel.TryGetValue(lvl, out var c)) ? c : 120.0;
        if (lastTicks > 0)
        {
            var last = new DateTime(lastTicks, DateTimeKind.Utc);
            var next = last.AddSeconds(cd);
            if (now < next)
            {
                remainingSec = (next - now).TotalSeconds;
                return false;
            }
        }
        return true;
    }

    public static bool IsAuraActive(Mobile m)
    {
        if (m is not PlayerMobile pm)
        {
            return false;
        }

        if (!IsPaladin(pm))
        {
            return false;
        }

        if (!PvPStadium.Races.RaceStateStore.TryGet(pm.Serial, out var st) || st == null)
        {
            return false;
        }
        return st.HolyAuraActiveUntilTicks > DateTime.UtcNow.Ticks;
    }
}
