using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 3 (Amazon Queen) bow. 50% poison + enhanced distance critical.</summary>
[SerializationGenerator(0)]
public partial class SuperiorBowOfIllness : BaseAmazonBow
{
    public override int RequiredAmazonLevel => 3;
    public override bool HasPoisonProc => true;
    public override double PoisonChance => 0.50;
    public override int PoisonLevel => 3; // Deadly
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
    public SuperiorBowOfIllness() : base(0x13B2)
    {
        Name = "Superior Bow of Illness";
        Hue = 0x0A77;
        Weight = 3.8;
    }
}
