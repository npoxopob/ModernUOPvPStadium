using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Paladin.Weapons;

/// <summary>
/// Level 4 (Guardian of Heaven) paladin sword.
/// 50% chance holy damage 12-18, bonus vs chaos +11, 10% heal, lightning visual.
/// </summary>
[SerializationGenerator(0, false)]
public partial class BladeOfFate : BasePaladinSword
{
    public override int RequiredPaladinLevel => 4;

    public override bool HasHolyProc => true;
    public override double HolyProcChance => 0.5;
    public override int HolyMinDamage => 12;
    public override int HolyMaxDamage => 18;

    public override bool HasChaosBonus => true;
    public override int ChaosBonusDamage => 11;
    public override double HealFraction => 0.10;

    public override WeaponAbility PrimaryAbility => WeaponAbility.CrushingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ParalyzingBlow;

    public override int AosMinDamage => 17;
    public override int AosMaxDamage => 21;
    public override int AosSpeed => 30;
    public override float MlSpeed => 3.00f;
    public override int AosStrengthReq => 40;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public BladeOfFate() : base(0x13FF)
    {
        Name = "Blade of Fate";
        Hue = 0x0A57;
        Weight = 6.7;
    }
}
