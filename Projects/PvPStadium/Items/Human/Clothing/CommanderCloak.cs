using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Human.Clothing;

/// <summary>Level 4 human cloak. +15 DEX, +5 HP regen.</summary>
[SerializationGenerator(0, false)]
public partial class CommanderCloak : BaseHumanCloak
{
    public override int RequiredHumanLevel => 4;
    public override int DexBonus => 15;
    public override int HpRegen => 5;

    [Constructible]
    public CommanderCloak() : base(0x0A09)
    {
        Name = "Commander's Cloak";
    }
}
