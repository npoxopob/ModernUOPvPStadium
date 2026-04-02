using System;
using Server;
using Server.Mobiles;

namespace PvPStadium.Mechanics;

public static class DamageHooks
{
    // Universal incoming damage hook entry point. Called via AssemblyHandler.Invoke from UOContent.
    // Returns true if scalar was modified and should be applied; false to ignore.
    public static bool PvPStadium_OnIncomingDamage(Mobile target, Mobile from, ref double scalar)
    {
        if (target is not PlayerMobile pm)
        {
            return false;
        }

        // Paladin Holy Aura reduction (self only, first iteration)
        if (PaladinHolyAura.IsAuraActive(pm))
        {
            // Get paladin level from our race state
            if (PvPStadium.Races.RaceStateStore.TryGet(pm.Serial, out var st) && st != null && st.RaceKey == "paladin")
            {
                var lvl = st.Level > 0 ? st.Level : 1;
                var map = PaladinHolyAura.Config.DamageReductionByLevel;
                if (map != null && map.TryGetValue(lvl, out var mult))
                {
                    scalar *= mult; // multiply incoming damage by reduction multiplier
                    if (scalar < 0.0)
                        scalar = 0.0;
                    return true;
                }
            }
        }

        return false;
    }
}
