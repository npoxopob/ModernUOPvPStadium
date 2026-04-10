using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Amazon.Clothing;

/// <summary>Level 1 (Amazon Girl) skirt. +10 DEX, Clumsy resist.</summary>
[SerializationGenerator(0, false)]
public partial class SkirtOfDexterity : BaseAmazonSkirt
{
    public override int RequiredAmazonLevel => 1;
    public override int DexBonus => 10;
    public override bool ClumsyResist => true;

    [Constructible]
    public SkirtOfDexterity() : base(0x0A4E)
    {
        Name = "Skirt of Dexterity";
    }
}
