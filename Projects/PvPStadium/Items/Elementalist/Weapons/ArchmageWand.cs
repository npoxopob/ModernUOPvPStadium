using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Weapons;

[SerializationGenerator(0)]
public partial class ArchmageWand : BaseElementalistWand
{
    public override int RequiredElementalistLevel => 4;
    public override int MaxOvercharge => 6;
    public override double ProcChance => 0.40;
    public override bool HasIce => true;
    public override bool HasLightning => true;
    public override double BurstCooldown => 4.0;
    public override int AosMinDamage => 17;
    public override int AosMaxDamage => 21;
    public override int AosSpeed => 48;
    public override float MlSpeed => 2.25f;
    public override int AosStrengthReq => 20;

    [Constructible]
    public ArchmageWand() : base(0xDF2)
    {
        Name = "Archmage Wand";
        Hue = 0x497;
        Weight = 4.0;
    }
}
