using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Berserker.Armor;

/// <summary>Level 2 (Barbarian) mask. Regen 5-9 HP every 3s.</summary>
[SerializationGenerator(0, false)]
public partial class BarbarianMask : BaseBerserkerMask
{
    public override int RequiredBerserkerLevel => 2;
    public override int RegenMin => 5;
    public override int RegenMax => 9;

    [Constructible]
    public BarbarianMask() : base(0x1545, 0x047F)
    {
        Name = "Mask of Barbarian";
        Weight = 7.0;
    }
}
