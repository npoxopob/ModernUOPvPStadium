using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 4 (Elite Amazon) kryss. 60% lethal poison + weaken 35-50 STR, 18s.</summary>
[SerializationGenerator(0)]
public partial class EliteKryssOfIllness : BaseAmazonKryss
{
    public override int RequiredAmazonLevel => 4;
    public override double ProcChance => 0.60;
    public override int WeakenStrMin => 35;
    public override int WeakenStrMax => 50;
    public override double WeakenDuration => 18.0;
    public override bool AppliesPoison => true;
    public override int PoisonLevel => 4; // Lethal

    public override WeaponAbility PrimaryAbility => WeaponAbility.ArmorIgnore;
    public override WeaponAbility SecondaryAbility => WeaponAbility.InfectiousStrike;

    public override int AosMinDamage => 18;
    public override int AosMaxDamage => 22;
    public override int AosSpeed => 53;
    public override float MlSpeed => 2.00f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public EliteKryssOfIllness() : base(0x1401)
    {
        Name = "Elite Kryss of Illness";
        Hue = 0x0489;
        Weight = 5.0;
    }
}
