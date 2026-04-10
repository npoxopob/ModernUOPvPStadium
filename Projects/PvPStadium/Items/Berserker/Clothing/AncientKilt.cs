using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Berserker.Clothing;

/// <summary>Level 3 (Berserker) kilt. +10 DEX, Weaken immunity.</summary>
[SerializationGenerator(0, false)]
public partial class AncientKilt : BaseBerserkerKilt
{
    public override int RequiredBerserkerLevel => 3;
    public override int DexBonus => 10;
    public override bool WeakenImmunity => true;

    [Constructible]
    public AncientKilt() : base(0x0492)
    {
        Name = "Ancient Kilt";
    }
}
