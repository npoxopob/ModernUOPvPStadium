using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Human.Clothing;

/// <summary>Level 3 human cloak. +10 DEX, +3 HP regen.</summary>
[SerializationGenerator(0, false)]
public partial class CaptainCloak : BaseHumanCloak
{
    public override int RequiredHumanLevel => 3;
    public override int DexBonus => 10;
    public override int HpRegen => 3;

    [Constructible]
    public CaptainCloak() : base(0x08B0)
    {
        Name = "Captain's Cloak";
    }
}
