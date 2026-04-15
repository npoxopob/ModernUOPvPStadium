using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Necromancer.Clothing;

/// <summary>Level 4 necromancer robe. +20 INT, 60% Holy reduction.</summary>
[SerializationGenerator(0)]
public partial class RobeOfTheVoid : BaseNecroRobe
{
    public override int RequiredNecromancerLevel => 4;
    public override int IntBonus => 20;
    public override int HolyDamageReduction => 60;

    [Constructible]
    public RobeOfTheVoid() : base(0x0386)
    {
        Name = "Robe of the Void";
    }
}
