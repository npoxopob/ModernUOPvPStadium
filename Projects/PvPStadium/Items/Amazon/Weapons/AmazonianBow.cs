using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 1 (Amazon Girl) bow. Distance critical, no procs.</summary>
[SerializationGenerator(0)]
public partial class AmazonianBow : BaseAmazonBow
{
    public override int RequiredAmazonLevel => 1;

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
    public AmazonianBow() : base(0x13B2) // bow graphic
    {
        Name = "Amazonian Bow";
        Hue = 0x0439;
        Weight = 5.0;
    }
}
