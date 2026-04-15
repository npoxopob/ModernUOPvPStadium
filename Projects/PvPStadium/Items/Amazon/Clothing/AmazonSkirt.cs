using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Amazon.Clothing;

/// <summary>Level 2 (Amazon) skirt. +10 DEX, Clumsy reflect.</summary>
[SerializationGenerator(0)]
public partial class AmazonSkirt : BaseAmazonSkirt
{
    public override int RequiredAmazonLevel => 2;
    public override int DexBonus => 10;
    public override bool ClumsyReflect => true;

    [Constructible]
    public AmazonSkirt() : base(0x0A4E)
    {
        Name = "Amazon Skirt";
    }
}
