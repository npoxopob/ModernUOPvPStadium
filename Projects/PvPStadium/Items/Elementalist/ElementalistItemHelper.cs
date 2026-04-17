using Server;
using Server.Mobiles;
using PvPStadium.Races;

namespace PvPStadium.Items.Elementalist;

public static class ElementalistItemHelper
{
    public static int GetElementalistLevel(Mobile m)
    {
        if (m is not PlayerMobile)
        {
            return 0;
        }

        if (!RaceStateStore.TryGet(m.Serial, out var st) || st == null)
        {
            return 0;
        }

        return st.RaceKey == "elementalist" ? st.Level : 0;
    }

    public static bool IsElementalist(Mobile m, int minLevel = 1) => GetElementalistLevel(m) >= minLevel;
}
