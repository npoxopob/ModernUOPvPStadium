using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Paladin.Weapons;

/// <summary>
/// Level 1 (Follower of Light) paladin sword. Basic weapon, no special procs.
/// </summary>
[SerializationGenerator(0)]
public partial class BladeOfLight : BasePaladinSword
{
    public override int RequiredPaladinLevel => 1;

    // No holy proc at level 1
    public override bool HasHolyProc => false;
    public override bool HasChaosBonus => false;
    public override double HealFraction => 0.0;

    public override WeaponAbility PrimaryAbility => WeaponAbility.CrushingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ParalyzingBlow;

    public override int AosMinDamage => 13;
    public override int AosMaxDamage => 16;
    public override int AosSpeed => 30;
    public override float MlSpeed => 3.00f;
    public override int AosStrengthReq => 40;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public BladeOfLight() : base(0x13FF) // viking sword graphic
    {
        Name = "Blade of Light";
        Hue = 0x0438;
        Weight = 7.8;
    }
}
