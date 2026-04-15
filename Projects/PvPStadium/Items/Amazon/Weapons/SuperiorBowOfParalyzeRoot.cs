using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 3 (Amazon Queen) bow. 50% paralyze + enhanced distance critical.</summary>
[SerializationGenerator(0)]
public partial class SuperiorBowOfParalyzeRoot : BaseAmazonBow
{
    public override int RequiredAmazonLevel => 3;
    public override bool HasParalyzeProc => true;
    public override double ParalyzeChance => 0.50;
    public override double ParalyzeDuration => 4.0;
    public override double CritBonus => 0.20;

    public override WeaponAbility PrimaryAbility => WeaponAbility.ParalyzingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.MortalStrike;

    public override int AosMinDamage => 17;
    public override int AosMaxDamage => 21;
    public override int AosSpeed => 25;
    public override float MlSpeed => 4.25f;
    public override int AosStrengthReq => 30;
    public override int DefMaxRange => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public SuperiorBowOfParalyzeRoot() : base(0x13B2)
    {
        Name = "Superior Bow of Paralyze Root";
        Hue = 0x0A79;
        Weight = 3.8;
    }
}
