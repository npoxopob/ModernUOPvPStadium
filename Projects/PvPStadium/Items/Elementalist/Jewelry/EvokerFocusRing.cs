using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Jewelry;

/// <summary>
/// Level 2 (Evoker) elementalist ring.
/// +1 Faster Casting, +5% Spell Damage.
/// </summary>
[SerializationGenerator(0)]
public partial class EvokerFocusRing : ElementalistRing
{
    public override int RequiredElementalistLevel => 2;

    [Constructible]
    public EvokerFocusRing() : base("Evoker Focus Ring", 0x48D)
    {
        Attributes.CastSpeed = 1;
        Attributes.SpellDamage = 5;
    }
}
