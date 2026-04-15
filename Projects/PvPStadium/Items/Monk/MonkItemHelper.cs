using Server;
using Server.Mobiles;
using PvPStadium.Races;

namespace PvPStadium.Items.Monk;

public static class MonkItemHelper
{
    public static int GetMonkLevel(Mobile m)
    {
        if (m is not PlayerMobile)
        {
            return 0;
        }

        if (!RaceStateStore.TryGet(m.Serial, out var st) || st == null)
        {
            return 0;
        }

        return st.RaceKey == "monk" ? st.Level : 0;
    }

    public static bool IsMonk(Mobile m, int minLevel = 1) => GetMonkLevel(m) >= minLevel;

    /// <summary>Max Chi charges for this monk level (0 if not monk).</summary>
    public static int MaxChi(int level) => level switch
    {
        1 => 3,
        2 => 5,
        3 => 7,
        _ => level >= 4 ? 10 : 0,
    };
}
