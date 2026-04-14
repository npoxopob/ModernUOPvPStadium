using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Monk.Clothing;

/// <summary>Level 2 monk gi. +8 DEX, +10% Evasion.</summary>
[SerializationGenerator(0)]
public partial class AcolyteGi : BaseMonkGi
{
    public override int RequiredMonkLevel => 2;
    public override int DexBonus => 8;
    public override int EvasionBonus => 10;

    [Constructible]
    public AcolyteGi() : base(0x08AB)
    {
        Name = "Acolyte Gi";
    }
}
