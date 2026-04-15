using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Jewelry;

/// <summary>
/// Amazon earrings. +10 STR bonus via AosAttributes (handled by BaseJewel).
/// </summary>
[SerializationGenerator(0)]
public partial class AmazonEarrings : BaseEarrings
{
    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _requiredLevel;

    [Constructible]
    public AmazonEarrings() : this(1)
    {
    }

    [Constructible]
    public AmazonEarrings(int level) : base(0x1087) // silver earrings
    {
        _requiredLevel = level;
        LootType = LootType.Blessed;

        // Use the built-in AosAttributes system so BaseJewel handles add/remove correctly
        Attributes.BonusStr = 10;

        if (level >= 4)
        {
            Name = "Elite Amazon Earrings";
            Hue = 0x0489;
        }
        else if (level >= 3)
        {
            Name = "Superior Amazon Earrings";
            Hue = 0x0A4E;
        }
        else if (level >= 2)
        {
            Name = "Amazon Earrings";
            Hue = 0x0A4C;
        }
        else
        {
            Name = "Amazon Girl Earrings";
            Hue = 0x0439;
        }
    }

    public override double DefaultWeight => 0.1;

    public override bool CanEquip(Mobile from)
    {
        if (!AmazonItemHelper.IsAmazon(from, _requiredLevel))
        {
            from.SendMessage(0x22, "Only an Amazon can wear these earrings.");
            return false;
        }

        return base.CanEquip(from);
    }
}
