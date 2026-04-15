using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Paladin.Weapons;

/// <summary>
/// Level 3 (Paladin) sword.
/// 50% chance holy damage 10-15, bonus vs chaos, 10% heal on non-chaos hit.
/// </summary>
[SerializationGenerator(0)]
public partial class AngerOfHeaven : BasePaladinSword
{
    public override int RequiredPaladinLevel => 3;

    public override bool HasHolyProc => true;
    public override double HolyProcChance => 0.5;
    public override int HolyMinDamage => 10;
    public override int HolyMaxDamage => 15;

    public override bool HasChaosBonus => true;
    public override int ChaosBonusDamage => 8;
    public override double HealFraction => 0.10;

    public override WeaponAbility PrimaryAbility => WeaponAbility.CrushingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ParalyzingBlow;

    public override int AosMinDamage => 14;
    public override int AosMaxDamage => 18;
    public override int AosSpeed => 30;
    public override float MlSpeed => 3.00f;
    public override int AosStrengthReq => 40;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public AngerOfHeaven() : base(0x13FF)
    {
        Name = "Anger of Heaven";
        Hue = 0x0990;
        Weight = 6.8;
    }
}
