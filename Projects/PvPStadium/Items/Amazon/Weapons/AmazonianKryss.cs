using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 1 (Amazon Girl) kryss. 50% weaken 18-25 STR, 10s.</summary>
[SerializationGenerator(0)]
public partial class AmazonianKryss : BaseAmazonKryss
{
    public override int RequiredAmazonLevel => 1;
    public override double ProcChance => 0.50;
    public override int WeakenStrMin => 18;
    public override int WeakenStrMax => 25;
    public override double WeakenDuration => 10.0;
    public override bool AppliesPoison => false;

    public override WeaponAbility PrimaryAbility => WeaponAbility.ArmorIgnore;
    public override WeaponAbility SecondaryAbility => WeaponAbility.InfectiousStrike;

    public override int AosMinDamage => 12;
    public override int AosMaxDamage => 14;
    public override int AosSpeed => 53;
    public override float MlSpeed => 2.00f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public AmazonianKryss() : base(0x1401) // kryss graphic
    {
        Name = "Amazonian Kryss";
        Hue = 0x0439;
        Weight = 6.0;
    }
}
