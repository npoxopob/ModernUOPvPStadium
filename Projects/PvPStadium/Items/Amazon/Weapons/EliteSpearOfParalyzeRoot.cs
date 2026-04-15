using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 4 (Elite Amazon) spear. 60% paralyze, 5s.</summary>
[SerializationGenerator(0)]
public partial class EliteSpearOfParalyzeRoot : BaseAmazonSpear
{
    public override int RequiredAmazonLevel => 4;
    public override double ParalyzeChance => 0.60;
    public override double ParalyzeDuration => 5.0;

    public override WeaponAbility PrimaryAbility => WeaponAbility.ParalyzingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.DoubleStrike;

    public override int AosMinDamage => 18;
    public override int AosMaxDamage => 22;
    public override int AosSpeed => 42;
    public override float MlSpeed => 2.75f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public EliteSpearOfParalyzeRoot() : base(0x1403)
    {
        Name = "Elite Spear of Paralyze Root";
        Hue = 0x0489;
        Weight = 5.0;
    }
}
