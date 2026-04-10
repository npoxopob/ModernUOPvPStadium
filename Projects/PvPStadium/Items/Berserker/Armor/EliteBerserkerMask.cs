using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Berserker.Armor;

/// <summary>Level 4 (Child of Ragnar) mask. Regen 6-14 HP every 3s.</summary>
[SerializationGenerator(0, false)]
public partial class EliteBerserkerMask : BaseBerserkerMask
{
    public override int RequiredBerserkerLevel => 4;
    public override int RegenMin => 6;
    public override int RegenMax => 14;

    [Constructible]
    public EliteBerserkerMask() : base(0x1545, 0x0A2C)
    {
        Name = "Mask of Elite Berserker";
        Weight = 7.0;
    }
}
