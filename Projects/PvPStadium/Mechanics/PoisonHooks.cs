using Server;
using Server.Mobiles;
using PvPStadium.Items.Vampire;

namespace PvPStadium.Mechanics;

public static class PoisonHooks
{
    /// <summary>
    /// Checks if target is immune to poison via vampire shroud.
    /// Called via reflection from PlayerMobile.CheckPoisonImmunity().
    /// Returns true if the target should be immune.
    /// </summary>
    public static bool PvPStadium_CheckPoisonImmunity(Mobile target, Mobile from, Poison poison)
    {
        if (target is not PlayerMobile)
        {
            return false;
        }

        // Vampire shroud with poison resist blocks all poison
        if (VampireItemHelper.HasPoisonResist(target))
        {
            target.SendMessage(0x3B2, "Your shroud shields you from the poison!");
            return true;
        }

        return false;
    }

    /// <summary>
    /// Handles poison reflect after poison was successfully applied.
    /// Called via reflection from PlayerMobile.ApplyPoison().
    /// Returns true if the poison was reflected back to the attacker.
    /// </summary>
    public static bool PvPStadium_OnPoisonApplied(Mobile target, Mobile from, Poison poison)
    {
        if (target is not PlayerMobile || from == null || from == target)
        {
            return false;
        }

        // Vampire shroud with poison reflect: reflect poison back to attacker
        if (VampireItemHelper.HasPoisonReflect(target))
        {
            target.SendMessage(0x3B2, "Your shroud reflects the poison back!");
            from.ApplyPoison(target, poison);
            return true;
        }

        return false;
    }
}
