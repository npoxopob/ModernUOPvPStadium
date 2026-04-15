using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Human.Armor;

/// <summary>Level 1 human shield. Basic defense.</summary>
[SerializationGenerator(0)]
public partial class MilitiaShield : BaseHumanShield
{
    public override int RequiredHumanLevel => 1;
    public override int BasePhysicalResistance => 5;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public MilitiaShield() : base(0x1B76) // metal kite shield
    {
        Name = "Militia Shield";
        Hue = 0x0835;
        Weight = 6.0;
    }
}
