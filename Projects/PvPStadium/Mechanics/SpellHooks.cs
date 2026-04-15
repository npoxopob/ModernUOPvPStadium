using Server;
using Server.Mobiles;
using PvPStadium.Items.Berserker.Clothing;
using PvPStadium.Items.Amazon.Clothing;

namespace PvPStadium.Mechanics;

public static class SpellHooks
{
    /// <summary>
    /// Called via reflection before Weaken spell is applied.
    /// Returns true if the spell should be blocked.
    /// </summary>
    public static bool PvPStadium_OnWeaken(Mobile target, Mobile caster)
    {
        if (target is not PlayerMobile)
        {
            return false;
        }

        // Berserker kilt immunity
        if (BaseBerserkerKilt.HasWeakenImmunity(target))
        {
            target.SendMessage(0x26, "Your kilt protects you from the Weaken spell!");
            target.FixedParticles(0x376A, 9, 32, 5005, 0x26, 0, EffectLayer.Waist);
            target.PlaySound(0x1E6);
            return true;
        }

        // Amazon skirt Weaken reflect
        if (BaseAmazonSkirt.TryWeakenReflect(target, caster))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Called via reflection before Clumsy spell is applied.
    /// Returns true if the spell should be blocked.
    /// </summary>
    public static bool PvPStadium_OnClumsy(Mobile target, Mobile caster)
    {
        if (target is not PlayerMobile)
        {
            return false;
        }

        // Amazon skirt Clumsy resist/reflect
        if (BaseAmazonSkirt.TryClumsyReflect(target, caster))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Called via reflection before Curse spell is applied.
    /// Returns true if the spell should be blocked.
    /// </summary>
    public static bool PvPStadium_OnCurse(Mobile target, Mobile caster)
    {
        if (target is not PlayerMobile)
        {
            return false;
        }

        if (BaseBerserkerKilt.HasCurseImmunity(target))
        {
            target.SendMessage(0x26, "Your kilt protects you from the Curse spell!");
            target.FixedParticles(0x376A, 9, 32, 5005, 0x26, 0, EffectLayer.Waist);
            target.PlaySound(0x1E1);
            return true;
        }

        return false;
    }
}
