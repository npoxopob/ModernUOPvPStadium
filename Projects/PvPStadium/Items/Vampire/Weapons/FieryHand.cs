using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Vampire.Weapons;

/// <summary>
/// Level 3 vampire ranged weapon (rare). 25% ignition: 2-3 ticks of 13-20 fire damage.
/// pvp_alfa: DAM 82-92, range 10, color 0aee.
/// </summary>
[Flippable(0x13FD, 0x13FC)]
[SerializationGenerator(0)]
public partial class FieryHand : BaseVampireHand
{
    [Constructible]
    public FieryHand() : base(0x13FD)
    {
        Name = "Fiery Hand";
        Hue = 0x0AEE;
    }

    public override int RequiredVampireLevel => 3;
    public override bool HasIgnition => true;
    public override double IgnitionChance => 0.25;
    public override int IgnitionMinTicks => 2;
    public override int IgnitionMaxTicks => 3;
    public override int IgnitionMinDamage => 13;
    public override int IgnitionMaxDamage => 20;

    public override double DefaultWeight => 9.0;
    public override WeaponAbility PrimaryAbility => WeaponAbility.MovingShot;
    public override WeaponAbility SecondaryAbility => WeaponAbility.Dismount;

    public override int AosStrengthReq => 80;
    public override int AosMinDamage => 16;
    public override int AosMaxDamage => 18;
    public override int AosSpeed => 22;
    public override float MlSpeed => 5.00f;

    public override int DefMaxRange => 10;

    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;
}
