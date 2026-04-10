using Server;
using Server.Mobiles;
using PvPStadium.Races;

namespace PvPStadium.Items.Paladin;

public static class PaladinItemHelper
{
    /// <summary>
    /// Returns the paladin level (1-4) or 0 if not a paladin.
    /// </summary>
    public static int GetPaladinLevel(Mobile m)
    {
        if (m is not PlayerMobile)
            return 0;

        if (!RaceStateStore.TryGet(m.Serial, out var st) || st == null)
            return 0;

        return st.RaceKey == "paladin" ? st.Level : 0;
    }

    /// <summary>
    /// Checks if the mobile is a paladin of at least the given level.
    /// </summary>
    public static bool IsPaladin(Mobile m, int minLevel = 1) => GetPaladinLevel(m) >= minLevel;

    /// <summary>
    /// Checks if the target is a "chaos" class (necromancer or vampire — natural enemies of paladin).
    /// </summary>
    public static bool IsChaosClass(Mobile m)
    {
        if (m is not PlayerMobile)
            return false;

        if (!RaceStateStore.TryGet(m.Serial, out var st) || st == null)
            return false;

        return st.RaceKey is "necromancer" or "vampire";
    }
}
