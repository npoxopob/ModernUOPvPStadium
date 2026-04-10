using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Paladin.Jewelry;

/// <summary>
/// Level 2 (Knight of Justice) paladin ring.
/// 33% chance to resist paralyze spell.
/// </summary>
[SerializationGenerator(0, false)]
public partial class HolyRing : BaseRing
{
    [Constructible]
    public HolyRing() : base(0x108A) // silver ring graphic
    {
        Name = "Holy Ring";
        Hue = 0x0920;
        LootType = LootType.Blessed;
    }

    public override double DefaultWeight => 0.1;

    public override bool CanEquip(Mobile from)
    {
        if (!PaladinItemHelper.IsPaladin(from, 2))
        {
            from.SendMessage(0x22, "Only a Knight of Justice or higher can wear this ring.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void AddNameProperties(IPropertyList list)
    {
        base.AddNameProperties(list);
        list.Add(1042971, "33% Paralyze Resistance");
        list.Add(1042971, "Knight of Justice");
    }

    /// <summary>
    /// Checks if wearing this ring resists the paralyze.
    /// Returns true if paralyze is resisted.
    /// </summary>
    public static bool TryResistParalyze(Mobile target)
    {
        if (target is not PlayerMobile pm)
            return false;

        var ring = pm.FindItemOnLayer<HolyRing>(Layer.Ring);
        if (ring == null)
            return false;

        // 33% chance to resist
        if (Utility.RandomDouble() < 0.33)
        {
            pm.SendMessage(0x3B2, "Your Holy Ring resists the paralyze!");
            pm.FixedParticles(0x375A, 10, 15, 5037, 0x480, 0, EffectLayer.Waist);
            pm.PlaySound(0x1F9);
            return true;
        }

        return false;
    }
}
