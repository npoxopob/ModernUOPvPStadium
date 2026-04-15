using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Berserker.Clothing;

/// <summary>
/// Base class for berserker kilts (BaseOuterLegs clothing).
/// Features: DEX bonus, spell immunity (Weaken or Curse).
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseBerserkerKilt : BaseOuterLegs
{
    /// <summary>Minimum berserker level required to equip.</summary>
    public virtual int RequiredBerserkerLevel => 2;

    /// <summary>DEX bonus when worn.</summary>
    public virtual int DexBonus => 5;

    /// <summary>If true, provides immunity to Weaken spell.</summary>
    public virtual bool WeakenImmunity => false;

    /// <summary>If true, provides immunity to Curse spell.</summary>
    public virtual bool CurseImmunity => false;

    protected BaseBerserkerKilt(int hue) : base(0x1537, hue) // kilt graphic
    {
        LootType = LootType.Blessed;
        Weight = 3.0;

        // Use built-in AosAttributes so BaseClothing handles add/remove correctly
        Attributes.BonusDex = DexBonus;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!BerserkerItemHelper.IsBerserker(from, RequiredBerserkerLevel))
        {
            from.SendMessage(0x22, "Only a berserker can wear this kilt.");
            return false;
        }

        return base.CanEquip(from);
    }

    /// <summary>
    /// Checks if any worn berserker kilt grants Weaken immunity.
    /// Called from SpellHooks.
    /// </summary>
    public static bool HasWeakenImmunity(Mobile m)
    {
        if (m is not PlayerMobile)
            return false;

        var kilt = m.FindItemOnLayer<BaseBerserkerKilt>(Layer.OuterLegs);
        return kilt != null && kilt.WeakenImmunity;
    }

    /// <summary>
    /// Checks if any worn berserker kilt grants Curse immunity.
    /// Called from SpellHooks.
    /// </summary>
    public static bool HasCurseImmunity(Mobile m)
    {
        if (m is not PlayerMobile)
            return false;

        var kilt = m.FindItemOnLayer<BaseBerserkerKilt>(Layer.OuterLegs);
        return kilt != null && kilt.CurseImmunity;
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (WeakenImmunity)
        {
            list.Add(1042971, $"{"Immune to Weaken"}");
        }

        if (CurseImmunity)
        {
            list.Add(1042971, $"{"Immune to Curse"}");
        }
    }
}
