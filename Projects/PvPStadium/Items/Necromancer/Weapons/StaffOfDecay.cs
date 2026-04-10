using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Necromancer.Weapons;

/// <summary>Level 2 necromancer staff. 25% Curse of Decay, +5 vs paladin.</summary>
[SerializationGenerator(0, false)]
public partial class StaffOfDecay : BaseNecroStaff
{
    public override int RequiredNecromancerLevel => 2;
    public override double CurseChance => 0.25;
    public override int CurseIntMin => 15;
    public override int CurseIntMax => 25;
    public override double CurseDuration => 10.0;
    public override int ManaDrainMin => 10;
    public override int ManaDrainMax => 15;
    public override int LightBonusDamage => 5;

    public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
    public override WeaponAbility SecondaryAbility => WeaponAbility.CrushingBlow;

    public override int AosMinDamage => 14;
    public override int AosMaxDamage => 17;
    public override int AosSpeed => 33;
    public override float MlSpeed => 3.25f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public StaffOfDecay() : base(0xDF0)
    {
        Name = "Staff of Decay";
        Hue = 0x0482;
        Weight = 6.0;
    }
}
