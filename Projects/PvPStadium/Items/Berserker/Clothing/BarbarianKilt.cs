using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Berserker.Clothing;

/// <summary>Level 2 (Barbarian) kilt. +5 DEX, Weaken immunity.</summary>
[SerializationGenerator(0, false)]
public partial class BarbarianKilt : BaseBerserkerKilt
{
    public override int RequiredBerserkerLevel => 2;
    public override int DexBonus => 5;
    public override bool WeakenImmunity => true;

    [Constructible]
    public BarbarianKilt() : base(0x047F)
    {
        Name = "Barbarian Kilt";
    }
}
