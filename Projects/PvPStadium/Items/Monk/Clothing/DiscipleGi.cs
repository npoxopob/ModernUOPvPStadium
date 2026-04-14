using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Monk.Clothing;

/// <summary>Level 1 monk gi. +5 DEX, +5% Evasion.</summary>
[SerializationGenerator(0)]
public partial class DiscipleGi : BaseMonkGi
{
    public override int RequiredMonkLevel => 1;
    public override int DexBonus => 5;
    public override int EvasionBonus => 5;

    [Constructible]
    public DiscipleGi() : base(0x0835)
    {
        Name = "Disciple Gi";
    }
}
