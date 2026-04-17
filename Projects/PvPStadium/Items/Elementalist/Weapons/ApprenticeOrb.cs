using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Elementalist.Weapons;

[SerializationGenerator(0)]
public partial class ApprenticeOrb : BaseElementalistOrb
{
    public override int RequiredElementalistLevel => 1;
    public override int MaxOvercharge => 3;
    public override int AosMinDamage => 9;
    public override int AosMaxDamage => 12;
    public override int AosSpeed => 53;
    public override float MlSpeed => 2.00f;
    public override int AosStrengthReq => 10;

    [Constructible]
    public ApprenticeOrb() : base(0x0E2D) // crystal ball graphic
    {
        Name = "Apprentice Orb";
        Hue = 0x489;
        Weight = 2.0;
    }
}
