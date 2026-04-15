using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Human.Clothing;

/// <summary>
/// Base class for human cloaks.
/// Features: DEX bonus + HP Regeneration via AosAttributes.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseHumanCloak : BaseCloak
{
    public virtual int RequiredHumanLevel => 1;
    public virtual int DexBonus => 5;
    public virtual int HpRegen => 0;

    protected BaseHumanCloak(int hue) : base(0x1515, hue) // cloak
    {
        LootType = LootType.Blessed;
        Weight = 3.0;

        if (DexBonus > 0)
        {
            Attributes.BonusDex = DexBonus;
        }

        if (HpRegen > 0)
        {
            Attributes.RegenHits = HpRegen;
        }
    }

    public override bool CanEquip(Mobile from)
    {
        if (!HumanItemHelper.IsHuman(from, RequiredHumanLevel))
        {
            from.SendMessage(0x22, "Only a human can wear this cloak.");
            return false;
        }

        return base.CanEquip(from);
    }
}
