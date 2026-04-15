using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Vampire.Weapons;

/// <summary>
/// Level 2 vampire ranged weapon (heavy crossbow). Fire visual on hit, no ignition.
/// pvp_alfa: DAM 95-120, range 10.
/// </summary>
[Flippable(0x13FD, 0x13FC)]
[SerializationGenerator(0)]
public partial class BloodyHand : BaseVampireHand
{
    [Constructible]
    public BloodyHand() : base(0x13FD)
    {
        Name = "Bloody Hand";
        Hue = 0x0846; // bloodrock color
    }

    public override int RequiredVampireLevel => 2;
    public override bool HasIgnition => false;

    public override double DefaultWeight => 9.0;
    public override WeaponAbility PrimaryAbility => WeaponAbility.MovingShot;
    public override WeaponAbility SecondaryAbility => WeaponAbility.Dismount;

    public override int AosStrengthReq => 80;
    public override int AosMinDamage => 19;
    public override int AosMaxDamage => 24;
    public override int AosSpeed => 22;
    public override float MlSpeed => 5.00f;

    public override int DefMaxRange => 10;

    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;
}
