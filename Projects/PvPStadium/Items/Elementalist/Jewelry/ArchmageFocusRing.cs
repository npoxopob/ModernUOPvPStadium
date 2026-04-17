using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Jewelry;

/// <summary>
/// Level 4 (Archmage) ring.
/// +2 Faster Casting, +12% Spell Damage, 25% Double Burst chance, +5 Mana Regen.
/// </summary>
[SerializationGenerator(0)]
public partial class ArchmageFocusRing : ElementalistRing
{
    public override int RequiredElementalistLevel => 4;
    public override double DoubleBurstChance => 0.25;

    [Constructible]
    public ArchmageFocusRing() : base("Archmage Focus Ring", 0x497)
    {
        Attributes.CastSpeed = 2;
        Attributes.SpellDamage = 12;
        Attributes.RegenMana = 5;
    }
}
