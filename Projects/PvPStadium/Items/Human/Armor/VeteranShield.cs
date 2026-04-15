using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Human.Armor;

/// <summary>Level 2 human shield. +5% reflect physical.</summary>
[SerializationGenerator(0)]
public partial class VeteranShield : BaseHumanShield
{
    public override int RequiredHumanLevel => 2;
    public override int ReflectPhysical => 5;
    public override int BasePhysicalResistance => 8;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public VeteranShield() : base(0x1B76)
    {
        Name = "Veteran Shield";
        Hue = 0x08AB;
        Weight = 6.0;
    }
}
