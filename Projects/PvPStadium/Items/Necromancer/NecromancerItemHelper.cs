using Server;
using Server.Mobiles;
using PvPStadium.Races;

namespace PvPStadium.Items.Necromancer;

public static class NecromancerItemHelper
{
    public static int GetNecromancerLevel(Mobile m)
    {
        if (m is not PlayerMobile)
            return 0;

        if (!RaceStateStore.TryGet(m.Serial, out var st) || st == null)
            return 0;

        return st.RaceKey == "necromancer" ? st.Level : 0;
    }

    public static bool IsNecromancer(Mobile m, int minLevel = 1) => GetNecromancerLevel(m) >= minLevel;

    /// <summary>Checks if target is a "light" class (paladin — natural enemy of necromancer).</summary>
    public static bool IsLightClass(Mobile m)
    {
        if (m is not PlayerMobile)
            return false;

        if (!RaceStateStore.TryGet(m.Serial, out var st) || st == null)
            return false;

        return st.RaceKey == "paladin";
    }
}
