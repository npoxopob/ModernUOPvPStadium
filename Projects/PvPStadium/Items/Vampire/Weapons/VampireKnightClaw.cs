using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Vampire.Weapons;

/// <summary>
/// Level 3 vampire sword. Life steal: dmg/3. Charge → Hell Blaze (32-43 fire).
/// </summary>
[Flippable(0x13B6, 0x13B5)]
[SerializationGenerator(0, false)]
public partial class VampireKnightClaw : BaseVampireClaw
{
    [Constructible]
    public VampireKnightClaw() : base(0x13B6)
    {
        Name = "Vampire Knight Claw";
        Hue = 0x0A26;
    }

    public override int RequiredVampireLevel => 3;
    public override int LifeStealDivisor => 3;
    public override bool HasChargeMechanic => true;
    public override int ChargeThreshold => 200;
    public override int BlazeMinDamage => 32;
    public override int BlazeMaxDamage => 43;

    public override double DefaultWeight => 5.0;
    public override WeaponAbility PrimaryAbility => WeaponAbility.DoubleStrike;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ParalyzingBlow;

    public override int AosStrengthReq => 25;
    public override int AosMinDamage => 14;
    public override int AosMaxDamage => 18;
    public override int AosSpeed => 37;
    public override float MlSpeed => 3.00f;

    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;
}
