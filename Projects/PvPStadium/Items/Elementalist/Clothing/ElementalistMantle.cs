using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Clothing;

/// <summary>
/// Level 3 (Elementalist) mantle.
/// +15 INT, +12% Spell Damage, 20% Spell Absorb.
/// </summary>
[SerializationGenerator(0)]
public partial class ElementalistMantle : BaseElementalistMantle
{
    public override int RequiredElementalistLevel => 3;
    public override int IntBonus => 15;
    public override int SpellDamageBonus => 12;
    public override int SpellAbsorbChance => 20;

    [Constructible]
    public ElementalistMantle() : base(0x490) // electric blue
    {
        Name = "Elementalist's Mantle";
    }
}
