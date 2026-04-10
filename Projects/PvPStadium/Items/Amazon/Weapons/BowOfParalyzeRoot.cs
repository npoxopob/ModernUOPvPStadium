using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 2 (Amazon) bow. 33% paralyze + distance critical.</summary>
[SerializationGenerator(0, false)]
public partial class BowOfParalyzeRoot : BaseAmazonBow
{
    public override int RequiredAmazonLevel => 2;
    public override bool HasParalyzeProc => true;
    public override double ParalyzeChance => 0.33;
    public override double ParalyzeDuration => 3.0;

    public override WeaponAbility PrimaryAbility => WeaponAbility.ParalyzingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.MortalStrike;

    public override int AosMinDamage => 15;
    public override int AosMaxDamage => 19;
    public override int AosSpeed => 25;
    public override float MlSpeed => 4.25f;
    public override int AosStrengthReq => 30;
    public override int DefMaxRange => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public BowOfParalyzeRoot() : base(0x13B2)
    {
        Name = "Bow of Paralyze Root";
        Hue = 0x0A72;
        Weight = 4.0;
    }
}
