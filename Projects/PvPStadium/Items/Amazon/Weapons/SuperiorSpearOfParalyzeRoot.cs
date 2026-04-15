using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 3 (Amazon Queen) spear. 50% paralyze.</summary>
[SerializationGenerator(0)]
public partial class SuperiorSpearOfParalyzeRoot : BaseAmazonSpear
{
    public override int RequiredAmazonLevel => 3;
    public override double ParalyzeChance => 0.50;
    public override double ParalyzeDuration => 4.0;

    public override WeaponAbility PrimaryAbility => WeaponAbility.ParalyzingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.DoubleStrike;

    public override int AosMinDamage => 15;
    public override int AosMaxDamage => 18;
    public override int AosSpeed => 42;
    public override float MlSpeed => 2.75f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public SuperiorSpearOfParalyzeRoot() : base(0x1403)
    {
        Name = "Superior Spear of Paralyze Root";
        Hue = 0x0A79;
        Weight = 5.0;
    }
}
