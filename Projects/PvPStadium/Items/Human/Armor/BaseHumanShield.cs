using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Human.Armor;

/// <summary>
/// Base class for human shields.
/// Features: defense bonus, physical reflect, Parry skill bonus at higher levels.
/// </summary>
[SerializationGenerator(0, false)]
public abstract partial class BaseHumanShield : BaseShield
{
    public virtual int RequiredHumanLevel => 1;

    /// <summary>Percentage of physical damage reflected (0-15).</summary>
    public virtual int ReflectPhysical => 0;

    protected BaseHumanShield(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;

        if (ReflectPhysical > 0)
            Attributes.ReflectPhysical = ReflectPhysical;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!HumanItemHelper.IsHuman(from, RequiredHumanLevel))
        {
            from.SendMessage(0x22, "Only a human can equip this shield.");
            return false;
        }

        return base.CanEquip(from);
    }
}
