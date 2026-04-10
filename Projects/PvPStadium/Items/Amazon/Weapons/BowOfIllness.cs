using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 2 (Amazon) bow. 50% poison + distance critical.</summary>
[SerializationGenerator(0, false)]
public partial class BowOfIllness : BaseAmazonBow
{
    public override int RequiredAmazonLevel => 2;
    public override bool HasPoisonProc => true;
    public override double PoisonChance => 0.50;
    public override int PoisonLevel => 2; // Greater

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
    public BowOfIllness() : base(0x13B2)
    {
        Name = "Bow of Illness";
        Hue = 0x0A70;
        Weight = 4.0;
    }
}
