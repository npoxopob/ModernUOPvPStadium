using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Paladin.Jewelry;

/// <summary>
/// Level 3 (Paladin) ring.
/// 33% chance to reflect paralyze spell back to caster.
/// </summary>
[SerializationGenerator(0)]
public partial class RingOfHeaven : BaseRing
{
    [Constructible]
    public RingOfHeaven() : base(0x108A) // silver ring graphic
    {
        Name = "Ring of Heaven";
        Hue = 0x042E;
        LootType = LootType.Blessed;
    }

    public override double DefaultWeight => 0.1;

    public override bool CanEquip(Mobile from)
    {
        if (!PaladinItemHelper.IsPaladin(from, 3))
        {
            from.SendMessage(0x22, "Only a Paladin or higher can wear this ring.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);
        list.Add(1042971, $"{"33% Paralyze Reflection"}");
        list.Add(1042971, $"{"Paladin"}");
    }

    /// <summary>
    /// Checks if wearing this ring reflects the paralyze back to caster.
    /// Returns true if paralyze is reflected (and blocked).
    /// </summary>
    public static bool TryReflectParalyze(Mobile target, Mobile caster)
    {
        if (target is not PlayerMobile pm)
        {
            return false;
        }

        var ring = pm.FindItemOnLayer<RingOfHeaven>(Layer.Ring);
        if (ring == null)
        {
            return false;
        }

        // 33% chance to reflect
        if (Utility.RandomDouble() < 0.33)
        {
            pm.SendMessage(0x3B2, "Your Ring of Heaven reflects the paralyze!");
            pm.FixedParticles(0x375A, 10, 15, 5037, 0x480, 0, EffectLayer.Waist);
            pm.PlaySound(0x1F9);

            // Reflect paralyze to caster
            if (caster != null && caster != target && caster.Alive)
            {
                caster.Paralyze(TimeSpan.FromSeconds(5.0));
                caster.SendMessage(0x22, "The paralyze has been reflected back to you!");
                caster.FixedParticles(0x376A, 9, 32, 5005, 0x480, 0, EffectLayer.Waist);
            }

            return true;
        }

        return false;
    }
}
