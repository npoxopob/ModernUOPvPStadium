using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Amazon.Clothing;

/// <summary>Level 4 (Elite Amazon) skirt. +15 DEX, reflects Clumsy + Weaken + Curse immunity.</summary>
[SerializationGenerator(0)]
public partial class EliteAmazonSkirt : BaseAmazonSkirt
{
    public override int RequiredAmazonLevel => 4;
    public override int DexBonus => 15;
    public override bool ClumsyReflect => true;
    public override bool WeakenReflect => true;

    [Constructible]
    public EliteAmazonSkirt() : base(0x0489)
    {
        Name = "Elite Amazon Skirt";
    }
}
