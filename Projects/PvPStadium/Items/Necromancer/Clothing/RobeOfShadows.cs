using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Necromancer.Clothing;

/// <summary>Level 3 necromancer robe. +15 INT, 40% Holy reduction.</summary>
[SerializationGenerator(0, false)]
public partial class RobeOfShadows : BaseNecroRobe
{
    public override int RequiredNecromancerLevel => 3;
    public override int IntBonus => 15;
    public override int HolyDamageReduction => 40;

    [Constructible]
    public RobeOfShadows() : base(0x0497)
    {
        Name = "Robe of Shadows";
    }
}
