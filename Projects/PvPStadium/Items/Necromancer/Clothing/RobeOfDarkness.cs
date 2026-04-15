using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Necromancer.Clothing;

/// <summary>Level 2 necromancer robe. +10 INT, 25% Holy reduction.</summary>
[SerializationGenerator(0)]
public partial class RobeOfDarkness : BaseNecroRobe
{
    public override int RequiredNecromancerLevel => 2;
    public override int IntBonus => 10;
    public override int HolyDamageReduction => 25;

    [Constructible]
    public RobeOfDarkness() : base(0x0482)
    {
        Name = "Robe of Darkness";
    }
}
