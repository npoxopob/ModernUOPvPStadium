using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Weapons;

[SerializationGenerator(0)]
public partial class StormOrb : BaseElementalistOrb
{
    public override int RequiredElementalistLevel => 3;
    public override int MaxOvercharge => 5;
    public override double ProcChance => 0.33;
    public override bool HasIce => true;
    public override bool HasLightning => true;
    public override int AosMinDamage => 13;
    public override int AosMaxDamage => 16;
    public override int AosSpeed => 62;
    public override float MlSpeed => 1.50f;
    public override int AosStrengthReq => 10;

    [Constructible]
    public StormOrb() : base(0x0F5C)
    {
        Name = "Storm Orb";
        Hue = 0x490; // lightning electric blue
        Weight = 2.0;
    }
}
