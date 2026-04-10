using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Amazon.Clothing;

/// <summary>Level 3 (Amazon Queen) skirt. +10 DEX, reflects Clumsy + Weaken.</summary>
[SerializationGenerator(0, false)]
public partial class SuperiorAmazonSkirt : BaseAmazonSkirt
{
    public override int RequiredAmazonLevel => 3;
    public override int DexBonus => 10;
    public override bool ClumsyReflect => true;
    public override bool WeakenReflect => true;

    [Constructible]
    public SuperiorAmazonSkirt() : base(0x0A4E)
    {
        Name = "Superior Amazon Skirt";
    }
}
