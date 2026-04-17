using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Clothing;

/// <summary>
/// Level 1 (Initiate) elementalist mantle.
/// +5 INT, +5% Spell Damage.
/// </summary>
[SerializationGenerator(0)]
public partial class InitiateMantle : BaseElementalistMantle
{
    public override int RequiredElementalistLevel => 1;
    public override int IntBonus => 5;
    public override int SpellDamageBonus => 5;
    public override int SpellAbsorbChance => 0;

    [Constructible]
    public InitiateMantle() : base(0x26B) // warm orange
    {
        Name = "Initiate's Mantle";
    }
}
