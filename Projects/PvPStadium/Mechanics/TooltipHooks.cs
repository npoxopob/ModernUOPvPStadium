using Server;
using Server.Mobiles;
using PvPStadium.Races;

namespace PvPStadium.Mechanics;

public static class TooltipHooks
{
    /// <summary>
    /// Called via reflection from PlayerMobile.GetProperties to add PvP race title to tooltip.
    /// </summary>
    public static void PvPStadium_AddRaceTitle(Mobile m, IPropertyList list)
    {
        if (m is not PlayerMobile)
            return;

        if (!RaceStateStore.TryGet(m.Serial, out var st) || st == null)
            return;

        var key = st.RaceKey;
        var level = st.Level;

        if (string.IsNullOrEmpty(key) || level < 1)
            return;

        var levelName = RaceRegistry.GetLevelName(key, level);
        list.Add(1042971, $"{levelName}");
    }
}
