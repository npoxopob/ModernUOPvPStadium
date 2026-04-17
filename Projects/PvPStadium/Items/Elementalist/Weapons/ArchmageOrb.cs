using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Weapons;

[SerializationGenerator(0)]
public partial class ArchmageOrb : BaseElementalistOrb
{
    public override int RequiredElementalistLevel => 4;
    public override int MaxOvercharge => 6;
    public override double ProcChance => 0.40;
    public override bool HasIce => true;
    public override bool HasLightning => true;
    public override double BurstCooldown => 4.0;
    public override bool HasMiniBurst => true;
    public override int AosMinDamage => 15;
    public override int AosMaxDamage => 19;
    public override int AosSpeed => 62;
    public override float MlSpeed => 1.50f;
    public override int AosStrengthReq => 10;

    [Constructible]
    public ArchmageOrb() : base(0x0E2D)
    {
        Name = "Archmage Orb";
        Hue = 0x497;
        Weight = 2.0;
    }
}
