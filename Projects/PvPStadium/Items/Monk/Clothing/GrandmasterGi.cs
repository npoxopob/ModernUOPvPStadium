using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Monk.Clothing;

/// <summary>Level 4 monk gi. +15 DEX, +20% Evasion, 20% Physical Resist, Paralyze Immunity.</summary>
[SerializationGenerator(0, false)]
public partial class GrandmasterGi : BaseMonkGi
{
    public override int RequiredMonkLevel => 4;
    public override int DexBonus => 15;
    public override int EvasionBonus => 20;
    public override int PhysicalResistBonus => 20;
    public override bool ParalyzeImmunity => true;

    [Constructible]
    public GrandmasterGi() : base(0x0A09)
    {
        Name = "Grandmaster Gi";
    }
}
