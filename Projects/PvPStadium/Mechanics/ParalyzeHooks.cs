using Server;
using Server.Mobiles;
using PvPStadium.Items.Paladin.Jewelry;
using PvPStadium.Items.Necromancer.Jewelry;

namespace PvPStadium.Mechanics;

public static class ParalyzeHooks
{
    /// <summary>
    /// Called via reflection when a paralyze spell/effect is about to be applied to a target.
    /// Returns true if the paralyze should be blocked (resisted or reflected).
    /// </summary>
    public static bool PvPStadium_OnParalyze(Mobile target, Mobile caster)
    {
        if (target is not PlayerMobile)
        {
            return false;
        }

        // Berserker fury-based paralyze immunity
        if (FurySystem.HasFuryParalyzeImmunity(target))
        {
            target.SendMessage(0x26, "Your fury makes you immune to paralyze!");
            return true;
        }

        // Ring of Heaven (level 3 paladin) — reflects paralyze back
        if (RingOfHeaven.TryReflectParalyze(target, caster))
        {
            return true;
        }

        // Holy Ring (level 2 paladin) — resists paralyze
        if (HolyRing.TryResistParalyze(target))
        {
            return true;
        }

        // Necro Ring (level 4) — reflects paralyze back
        if (NecroRing.TryReflectParalyze(target, caster))
        {
            return true;
        }

        // Necro Ring (level 3) — resists paralyze
        if (NecroRing.TryResistParalyze(target))
        {
            return true;
        }

        return false;
    }
}
