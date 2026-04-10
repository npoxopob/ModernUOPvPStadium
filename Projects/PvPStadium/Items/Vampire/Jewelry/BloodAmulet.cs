using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Vampire.Jewelry;

/// <summary>
/// Level 4 (Nosferatu) vampire necklace.
/// Caps incoming damage at a threshold (default 75).
/// Bypassed if attacker is a paladin.
/// </summary>
[SerializationGenerator(0, false)]
public partial class BloodAmulet : BaseNecklace
{
    /// <summary>Maximum damage allowed through per hit.</summary>
    public static int DamageCap => 75;

    [Constructible]
    public BloodAmulet() : base(0x1088) // gold necklace graphic
    {
        Name = "Blood Amulet";
        Hue = 0x0A11;
        LootType = LootType.Blessed;
    }

    public override double DefaultWeight => 0.1;

    public override bool CanEquip(Mobile from)
    {
        if (!VampireItemHelper.IsVampire(from, 4))
        {
            from.SendMessage(0x22, "Only a Nosferatu Vampire can wear this amulet.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void AddNameProperties(IPropertyList list)
    {
        base.AddNameProperties(list);
        list.Add(1042971, $"Absorbs damage above {DamageCap}");
        list.Add(1042971, "Bypassed by Paladins");
    }

    /// <summary>
    /// Called externally (from a damage hook or helper) to apply the damage cap.
    /// Returns the capped damage value.
    /// </summary>
    public static int ApplyDamageCap(Mobile defender, Mobile? attacker, int damage)
    {
        if (defender is not PlayerMobile pm)
            return damage;

        // Check if wearing Blood Amulet
        var amulet = pm.FindItemOnLayer<BloodAmulet>(Layer.Neck);
        if (amulet == null)
            return damage;

        // Bypassed if attacker is a paladin
        if (attacker != null && VampireItemHelper.GetRaceKey(attacker) == "paladin")
            return damage;

        if (damage > DamageCap)
            return DamageCap;

        return damage;
    }
}
