using Server;
using Server.Mobiles;
using PvPStadium.Races;

namespace PvPStadium.Items.Human;

public static class HumanItemHelper
{
    public static int GetHumanLevel(Mobile m)
    {
        if (m is not PlayerMobile)
            return 0;

        if (!RaceStateStore.TryGet(m.Serial, out var st) || st == null)
            return 0;

        return st.RaceKey == "human" ? st.Level : 0;
    }

    public static bool IsHuman(Mobile m, int minLevel = 1) => GetHumanLevel(m) >= minLevel;

    /// <summary>Returns max crystal sockets for the given human level.</summary>
    public static int MaxSockets(int level) => level switch
    {
        1 => 1,
        2 => 2,
        3 => 3,
        >= 4 => 3,
        _ => 0
    };
}
