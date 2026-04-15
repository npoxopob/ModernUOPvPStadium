using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 2 (Amazon) spear. 33% paralyze.</summary>
[SerializationGenerator(0)]
public partial class SpearOfParalyzeRoot : BaseAmazonSpear
{
    public override int RequiredAmazonLevel => 2;
    public override double ParalyzeChance => 0.33;
    public override double ParalyzeDuration => 3.5;

    public override WeaponAbility PrimaryAbility => WeaponAbility.ParalyzingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.DoubleStrike;

    public override int AosMinDamage => 13;
    public override int AosMaxDamage => 15;
    public override int AosSpeed => 42;
    public override float MlSpeed => 2.75f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public SpearOfParalyzeRoot() : base(0x1403)
    {
        Name = "Spear of Paralyze Root";
        Hue = 0x0A72;
        Weight = 5.0;
    }
}
