using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Monk.Clothing;

/// <summary>Level 3 monk gi. +10 DEX, +15% Evasion, 15% Physical Resist.</summary>
[SerializationGenerator(0, false)]
public partial class MonkGi : BaseMonkGi
{
    public override int RequiredMonkLevel => 3;
    public override int DexBonus => 10;
    public override int EvasionBonus => 15;
    public override int PhysicalResistBonus => 15;

    [Constructible]
    public MonkGi() : base(0x08B0)
    {
        Name = "Monk Gi";
    }
}
