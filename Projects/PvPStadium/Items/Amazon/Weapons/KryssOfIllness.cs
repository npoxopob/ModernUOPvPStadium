using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 2 (Amazon) kryss. 50% poison + weaken 24-35 STR, 15s.</summary>
[SerializationGenerator(0)]
public partial class KryssOfIllness : BaseAmazonKryss
{
    public override int RequiredAmazonLevel => 2;
    public override double ProcChance => 0.50;
    public override int WeakenStrMin => 24;
    public override int WeakenStrMax => 35;
    public override double WeakenDuration => 15.0;
    public override bool AppliesPoison => true;
    public override int PoisonLevel => 2; // Greater

    public override WeaponAbility PrimaryAbility => WeaponAbility.ArmorIgnore;
    public override WeaponAbility SecondaryAbility => WeaponAbility.InfectiousStrike;

    public override int AosMinDamage => 13;
    public override int AosMaxDamage => 15;
    public override int AosSpeed => 53;
    public override float MlSpeed => 2.00f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public KryssOfIllness() : base(0x1401)
    {
        Name = "Kryss of Illness";
        Hue = 0x0A70;
        Weight = 5.0;
    }
}
