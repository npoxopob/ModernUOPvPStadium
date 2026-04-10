using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Paladin.Weapons;

/// <summary>
/// Level 2 (Knight of Justice) paladin sword.
/// 50% chance holy damage 8-12, bonus vs chaos, 10% heal on non-chaos hit.
/// </summary>
[SerializationGenerator(0, false)]
public partial class BladeOfJustice : BasePaladinSword
{
    public override int RequiredPaladinLevel => 2;

    public override bool HasHolyProc => true;
    public override double HolyProcChance => 0.5;
    public override int HolyMinDamage => 8;
    public override int HolyMaxDamage => 12;

    public override bool HasChaosBonus => true;
    public override int ChaosBonusDamage => 5;
    public override double HealFraction => 0.10;

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
    public BladeOfJustice() : base(0x13FF)
    {
        Name = "Blade of Justice";
        Hue = 0x0A71;
        Weight = 7.3;
    }
}
