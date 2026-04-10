using Server;
using Server.Mobiles;
using PvPStadium.Races;

namespace PvPStadium.Items.Amazon;

public static class AmazonItemHelper
{
    public static int GetAmazonLevel(Mobile m)
    {
        if (m is not PlayerMobile)
            return 0;

        if (!RaceStateStore.TryGet(m.Serial, out var st) || st == null)
            return 0;

        return st.RaceKey == "amazon" ? st.Level : 0;
    }

    public static bool IsAmazon(Mobile m, int minLevel = 1) => GetAmazonLevel(m) >= minLevel;
}
