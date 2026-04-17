using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Weapons;

[SerializationGenerator(0)]
public partial class StormWand : BaseElementalistWand
{
    public override int RequiredElementalistLevel => 3;
    public override int MaxOvercharge => 5;
    public override double ProcChance => 0.33;
    public override bool HasIce => true;
    public override bool HasLightning => true;
    public override int AosMinDamage => 15;
    public override int AosMaxDamage => 18;
    public override int AosSpeed => 48;
    public override float MlSpeed => 2.25f;
    public override int AosStrengthReq => 20;

    [Constructible]
    public StormWand() : base(0xDF2)
    {
        Name = "Storm Wand";
        Hue = 0x490;
        Weight = 4.0;
    }
}
