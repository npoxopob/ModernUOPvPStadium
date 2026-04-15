using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Human.Jewelry;

/// <summary>
/// Human ring. STR bonus + Defense Chance Increase via AosAttributes.
/// Level 1: +5 STR, +5% DCI
/// Level 2: +5 STR, +10% DCI
/// Level 3: +10 STR, +10% DCI
/// Level 4: +10 STR, +15% DCI
/// </summary>
[SerializationGenerator(0)]
public partial class HumanRing : BaseRing
{
    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _requiredLevel;

    [Constructible]
    public HumanRing() : this(1) { }

    [Constructible]
    public HumanRing(int level) : base(0x108A) // gold ring
    {
        _requiredLevel = level;
        LootType = LootType.Blessed;

        switch (level)
        {
            case 1:
                Name = "Militia Ring";
                Hue = 0x0835;
                Attributes.BonusStr = 5;
                Attributes.DefendChance = 5;
                break;
            case 2:
                Name = "Veteran Ring";
                Hue = 0x08AB;
                Attributes.BonusStr = 5;
                Attributes.DefendChance = 10;
                break;
            case 3:
                Name = "Captain's Ring";
                Hue = 0x08B0;
                Attributes.BonusStr = 10;
                Attributes.DefendChance = 10;
                break;
            default: // 4+
                Name = "Commander's Ring";
                Hue = 0x0A09;
                Attributes.BonusStr = 10;
                Attributes.DefendChance = 15;
                break;
        }
    }

    public override double DefaultWeight => 0.1;

    public override bool CanEquip(Mobile from)
    {
        if (!HumanItemHelper.IsHuman(from, _requiredLevel))
        {
            from.SendMessage(0x22, "Only a human can wear this ring.");
            return false;
        }

        return base.CanEquip(from);
    }
}
