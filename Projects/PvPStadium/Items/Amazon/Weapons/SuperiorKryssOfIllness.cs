using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 3 (Amazon Queen) kryss. 50% poison + weaken 28-40 STR, 15s.</summary>
[SerializationGenerator(0, false)]
public partial class SuperiorKryssOfIllness : BaseAmazonKryss
{
    public override int RequiredAmazonLevel => 3;
    public override double ProcChance => 0.50;
    public override int WeakenStrMin => 28;
    public override int WeakenStrMax => 40;
    public override double WeakenDuration => 15.0;
    public override bool AppliesPoison => true;
    public override int PoisonLevel => 3; // Deadly

    public override WeaponAbility PrimaryAbility => WeaponAbility.ArmorIgnore;
    public override WeaponAbility SecondaryAbility => WeaponAbility.InfectiousStrike;

    public override int AosMinDamage => 15;
    public override int AosMaxDamage => 18;
    public override int AosSpeed => 53;
    public override float MlSpeed => 2.00f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public SuperiorKryssOfIllness() : base(0x1401)
    {
        Name = "Superior Kryss of Illness";
        Hue = 0x0A77;
        Weight = 5.0;
    }
}
