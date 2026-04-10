using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Human.Clothing;

/// <summary>Level 1 human cloak. +5 DEX.</summary>
[SerializationGenerator(0, false)]
public partial class MilitiaCloak : BaseHumanCloak
{
    public override int RequiredHumanLevel => 1;
    public override int DexBonus => 5;

    [Constructible]
    public MilitiaCloak() : base(0x0835)
    {
        Name = "Militia Cloak";
    }
}
