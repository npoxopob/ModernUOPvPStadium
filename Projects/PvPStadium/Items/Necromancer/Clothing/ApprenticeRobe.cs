using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Necromancer.Clothing;

/// <summary>Level 1 necromancer robe. +10 INT, no Holy reduction.</summary>
[SerializationGenerator(0)]
public partial class ApprenticeRobe : BaseNecroRobe
{
    public override int RequiredNecromancerLevel => 1;
    public override int IntBonus => 10;

    [Constructible]
    public ApprenticeRobe() : base(0x0455)
    {
        Name = "Apprentice Robe";
    }
}
