using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Jewelry;

/// <summary>
/// Level 3 (Elementalist) ring.
/// +2 Faster Casting, 15% Double Burst chance.
/// </summary>
[SerializationGenerator(0)]
public partial class ResonanceRing : ElementalistRing
{
    public override int RequiredElementalistLevel => 3;
    public override double DoubleBurstChance => 0.15;

    [Constructible]
    public ResonanceRing() : base("Resonance Ring", 0x490)
    {
        Attributes.CastSpeed = 2;
    }
}
