using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Vampire.Weapons;

/// <summary>
/// Level 1 vampire sword. Life steal: dmg/4. No charge mechanic.
/// </summary>
[Flippable(0x13B6, 0x13B5)]
[SerializationGenerator(0, false)]
public partial class GhoulClaw : BaseVampireClaw
{
    [Constructible]
    public GhoulClaw() : base(0x13B6)
    {
        Name = "Ghoul Claw";
        Hue = 0x0455;
    }

    public override int RequiredVampireLevel => 1;
    public override int LifeStealDivisor => 4;
    public override bool HasChargeMechanic => false;

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
