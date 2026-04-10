using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Berserker.Clothing;

/// <summary>Level 4 (Child of Ragnar) kilt. +10 DEX, Curse immunity.</summary>
[SerializationGenerator(0, false)]
public partial class KiltOfAncestors : BaseBerserkerKilt
{
    public override int RequiredBerserkerLevel => 4;
    public override int DexBonus => 10;
    public override bool CurseImmunity => true;
    public override bool WeakenImmunity => true; // Elite has both

    [Constructible]
    public KiltOfAncestors() : base(0x0A2C)
    {
        Name = "Kilt of Ancestors";
    }
}
