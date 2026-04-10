using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Berserker.Armor;

/// <summary>Level 1 (Adept of Might) mask. Regen 3-6 HP every 3s.</summary>
[SerializationGenerator(0, false)]
public partial class AdeptMask : BaseBerserkerMask
{
    public override int RequiredBerserkerLevel => 1;
    public override int RegenMin => 3;
    public override int RegenMax => 6;

    [Constructible]
    public AdeptMask() : base(0x1545, 0x0A31) // bear mask
    {
        Name = "Mask of Adept";
        Weight = 7.0;
    }
}
