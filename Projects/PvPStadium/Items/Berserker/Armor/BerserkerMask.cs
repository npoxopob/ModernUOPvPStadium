using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Berserker.Armor;

/// <summary>Level 3 (Berserker) mask. Regen 6-12 HP every 3s.</summary>
[SerializationGenerator(0, false)]
public partial class BerserkerMask : BaseBerserkerMask
{
    public override int RequiredBerserkerLevel => 3;
    public override int RegenMin => 6;
    public override int RegenMax => 12;

    [Constructible]
    public BerserkerMask() : base(0x1545, 0x0492)
    {
        Name = "Mask of Berserker";
        Weight = 7.0;
    }
}
