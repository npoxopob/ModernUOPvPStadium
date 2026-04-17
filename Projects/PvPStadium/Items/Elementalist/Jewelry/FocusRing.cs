using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Jewelry;

/// <summary>
/// Level 1 (Initiate) elementalist ring.
/// +1 Faster Casting.
/// </summary>
[SerializationGenerator(0)]
public partial class FocusRing : ElementalistRing
{
    public override int RequiredElementalistLevel => 1;

    [Constructible]
    public FocusRing() : base("Focus Ring", 0x489)
    {
        Attributes.CastSpeed = 1;
    }
}
