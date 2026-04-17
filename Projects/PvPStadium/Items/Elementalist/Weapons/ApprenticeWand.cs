using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Weapons;

[SerializationGenerator(0)]
public partial class ApprenticeWand : BaseElementalistWand
{
    public override int RequiredElementalistLevel => 1;
    public override int MaxOvercharge => 3;
    public override int AosMinDamage => 11;
    public override int AosMaxDamage => 14;
    public override int AosSpeed => 48;
    public override float MlSpeed => 2.25f;
    public override int AosStrengthReq => 20;

    [Constructible]
    public ApprenticeWand() : base(0xDF2) // black staff graphic (short wand)
    {
        Name = "Apprentice Wand";
        Hue = 0x489; // fire hue
        Weight = 4.0;
    }
}
