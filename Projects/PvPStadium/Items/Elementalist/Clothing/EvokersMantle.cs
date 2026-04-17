using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Clothing;

/// <summary>
/// Level 2 (Evoker) elementalist mantle.
/// +10 INT, +8% Spell Damage, 10% Spell Absorb.
/// </summary>
[SerializationGenerator(0)]
public partial class EvokersMantle : BaseElementalistMantle
{
    public override int RequiredElementalistLevel => 2;
    public override int IntBonus => 10;
    public override int SpellDamageBonus => 8;
    public override int SpellAbsorbChance => 10;

    [Constructible]
    public EvokersMantle() : base(0x489) // fire red-orange
    {
        Name = "Evoker's Mantle";
    }
}
