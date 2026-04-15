using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Human.Clothing;

/// <summary>Level 2 human cloak. +5 DEX, +2 HP regen.</summary>
[SerializationGenerator(0)]
public partial class VeteranCloak : BaseHumanCloak
{
    public override int RequiredHumanLevel => 2;
    public override int DexBonus => 5;
    public override int HpRegen => 2;

    [Constructible]
    public VeteranCloak() : base(0x08AB)
    {
        Name = "Veteran Cloak";
    }
}
