using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 1 (Amazon Girl) spear. 25% paralyze.</summary>
[SerializationGenerator(0, false)]
public partial class AmazonianSpear : BaseAmazonSpear
{
    public override int RequiredAmazonLevel => 1;
    public override double ParalyzeChance => 0.25;
    public override double ParalyzeDuration => 3.0;

    public override WeaponAbility PrimaryAbility => WeaponAbility.ParalyzingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.DoubleStrike;

    public override int AosMinDamage => 12;
    public override int AosMaxDamage => 14;
    public override int AosSpeed => 42;
    public override float MlSpeed => 2.75f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public AmazonianSpear() : base(0x1403) // short spear
    {
        Name = "Amazonian Spear";
        Hue = 0x0439;
        Weight = 6.0;
    }
}
