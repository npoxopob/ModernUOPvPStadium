using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Vampire.Weapons;

/// <summary>
/// Level 2 vampire sword. Life steal: dmg/4. Charge → Hell Blaze (28-39 fire).
/// </summary>
[Flippable(0x13B6, 0x13B5)]
[SerializationGenerator(0)]
public partial class VampireClaw : BaseVampireClaw
{
    [Constructible]
    public VampireClaw() : base(0x13B6)
    {
        Name = "Vampire Claw";
        Hue = 0x0A11;
    }

    public override int RequiredVampireLevel => 2;
    public override int LifeStealDivisor => 4;
    public override bool HasChargeMechanic => true;
    public override int ChargeThreshold => 200;
    public override int BlazeMinDamage => 28;
    public override int BlazeMaxDamage => 39;

    public override double DefaultWeight => 5.0;
    public override WeaponAbility PrimaryAbility => WeaponAbility.DoubleStrike;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ParalyzingBlow;

    public override int AosStrengthReq => 25;
    public override int AosMinDamage => 13;
    public override int AosMaxDamage => 16;
    public override int AosSpeed => 37;
    public override float MlSpeed => 3.00f;

    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;
}
