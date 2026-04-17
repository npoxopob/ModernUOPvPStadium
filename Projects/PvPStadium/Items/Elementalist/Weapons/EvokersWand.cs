using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Weapons;

[SerializationGenerator(0)]
public partial class EvokersWand : BaseElementalistWand
{
    public override int RequiredElementalistLevel => 2;
    public override int MaxOvercharge => 4;
    public override double ProcChance => 0.25;
    public override bool HasIce => true;
    public override int AosMinDamage => 13;
    public override int AosMaxDamage => 16;
    public override int AosSpeed => 48;
    public override float MlSpeed => 2.25f;
    public override int AosStrengthReq => 20;

    [Constructible]
    public EvokersWand() : base(0xDF2)
    {
        Name = "Evoker's Wand";
        Hue = 0x48D;
        Weight = 4.0;
    }
}
