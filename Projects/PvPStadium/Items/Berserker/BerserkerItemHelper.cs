using Server;
using Server.Mobiles;
using PvPStadium.Races;

namespace PvPStadium.Items.Berserker;

public static class BerserkerItemHelper
{
    public static int GetBerserkerLevel(Mobile m)
    {
        if (m is not PlayerMobile)
            return 0;

        if (!RaceStateStore.TryGet(m.Serial, out var st) || st == null)
            return 0;

        return st.RaceKey == "berserker" ? st.Level : 0;
    }

    public static bool IsBerserker(Mobile m, int minLevel = 1) => GetBerserkerLevel(m) >= minLevel;
}
