using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Clothing;

/// <summary>
/// Level 4 (Archmage) mantle.
/// +20 INT, +15% Spell Damage, 30% Spell Absorb.
/// </summary>
[SerializationGenerator(0)]
public partial class ArchmageMantle : BaseElementalistMantle
{
    public override int RequiredElementalistLevel => 4;
    public override int IntBonus => 20;
    public override int SpellDamageBonus => 15;
    public override int SpellAbsorbChance => 30;

    [Constructible]
    public ArchmageMantle() : base(0x497) // arcane violet-white
    {
        Name = "Archmage's Mantle";
    }
}
