using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Human.Armor;

/// <summary>Level 4 human shield. +15% reflect physical, high resistance.</summary>
[SerializationGenerator(0)]
public partial class CommanderShield : BaseHumanShield
{
    public override int RequiredHumanLevel => 4;
    public override int ReflectPhysical => 15;
    public override int BasePhysicalResistance => 12;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public CommanderShield() : base(0x1B76)
    {
        Name = "Commander's Shield";
        Hue = 0x0A09;
        Weight = 6.0;
    }
}
