using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Necromancer.Clothing;

/// <summary>
/// Base class for necromancer robes (BaseOuterTorso).
/// Features: INT bonus via AosAttributes, Holy damage reduction.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseNecroRobe : BaseOuterTorso
{
    public virtual int RequiredNecromancerLevel => 1;
    public virtual int IntBonus => 10;

    /// <summary>Percentage of Holy (paladin) bonus damage absorbed (0–100).</summary>
    public virtual int HolyDamageReduction => 0;

    protected BaseNecroRobe(int hue) : base(0x2684, hue) // hooded shroud of shadows
    {
        LootType = LootType.Blessed;
        Weight = 3.0;

        Attributes.BonusInt = IntBonus;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!NecromancerItemHelper.IsNecromancer(from, RequiredNecromancerLevel))
        {
            from.SendMessage(0x22, "Only a necromancer can wear this robe.");
            return false;
        }

        return base.CanEquip(from);
    }

    /// <summary>Checks if the target is wearing a necro robe and returns the Holy damage reduction %.</summary>
    public static int GetHolyReduction(Mobile m)
    {
        var robe = m.FindItemOnLayer<BaseNecroRobe>(Layer.OuterTorso);
        return robe?.HolyDamageReduction ?? 0;
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (HolyDamageReduction > 0)
        {
            list.Add(1042971, $"{"Holy Damage Reduction"}\t{HolyDamageReduction}%");
        }
    }
}
