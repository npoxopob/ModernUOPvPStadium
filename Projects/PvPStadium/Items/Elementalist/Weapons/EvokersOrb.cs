using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Weapons;

[SerializationGenerator(0)]
public partial class EvokersOrb : BaseElementalistOrb
{
    public override int RequiredElementalistLevel => 2;
    public override int MaxOvercharge => 4;
    public override double ProcChance => 0.25;
    public override bool HasIce => true;
    public override int AosMinDamage => 11;
    public override int AosMaxDamage => 14;
    public override int AosSpeed => 57;
    public override float MlSpeed => 1.75f;
    public override int AosStrengthReq => 10;

    [Constructible]
    public EvokersOrb() : base(0x0E2D)
    {
        Name = "Evoker's Orb";
        Hue = 0x48D;
        Weight = 2.0;
    }
}
