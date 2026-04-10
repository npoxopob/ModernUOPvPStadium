using Server;
using Server.Items;
using Server.Mobiles;
using PvPStadium.Items.Vampire.Clothing;
using PvPStadium.Races;

namespace PvPStadium.Items.Vampire;

public static class VampireItemHelper
{
    /// <summary>
    /// Returns the vampire level (1-4) or 0 if the mobile is not a vampire.
    /// </summary>
    public static int GetVampireLevel(Mobile m)
    {
        if (m is not PlayerMobile)
            return 0;

        if (!RaceStateStore.TryGet(m.Serial, out var st) || st == null)
            return 0;

        return st.RaceKey == "vampire" ? st.Level : 0;
    }

    /// <summary>
    /// Checks if the mobile is a vampire of at least the given level.
    /// </summary>
    public static bool IsVampire(Mobile m, int minLevel = 1) => GetVampireLevel(m) >= minLevel;

    /// <summary>
    /// Returns the race key ("paladin", "vampire", etc.) or null.
    /// </summary>
    public static string? GetRaceKey(Mobile m)
    {
        if (m is not PlayerMobile)
            return null;

        return RaceStateStore.TryGet(m.Serial, out var st) && st != null ? st.RaceKey : null;
    }

    /// <summary>
    /// Checks if the mobile is wearing a vampire shroud with poison resistance.
    /// </summary>
    public static bool HasPoisonResist(Mobile m)
    {
        var shroud = m.FindItemOnLayer<BaseVampireShroud>(Layer.OuterTorso);
        return shroud != null && shroud.PoisonResist;
    }

    /// <summary>
    /// Checks if the mobile is wearing a vampire shroud with poison reflect.
    /// </summary>
    public static bool HasPoisonReflect(Mobile m)
    {
        var shroud = m.FindItemOnLayer<BaseVampireShroud>(Layer.OuterTorso);
        return shroud != null && shroud.PoisonReflect;
    }
}
