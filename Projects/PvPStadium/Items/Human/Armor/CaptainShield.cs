using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Human.Armor;

/// <summary>Level 3 human shield. +10% reflect physical.</summary>
[SerializationGenerator(0, false)]
public partial class CaptainShield : BaseHumanShield
{
    public override int RequiredHumanLevel => 3;
    public override int ReflectPhysical => 10;
    public override int BasePhysicalResistance => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public CaptainShield() : base(0x1B76)
    {
        Name = "Captain's Shield";
        Hue = 0x08B0;
        Weight = 6.0;
    }
}
