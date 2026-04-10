using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Necromancer.Weapons;

/// <summary>Level 3 necromancer staff. 33% Curse of Decay, +8 vs paladin.</summary>
[SerializationGenerator(0, false)]
public partial class StaffOfDamnation : BaseNecroStaff
{
    public override int RequiredNecromancerLevel => 3;
    public override double CurseChance => 0.33;
    public override int CurseIntMin => 20;
    public override int CurseIntMax => 30;
    public override double CurseDuration => 12.0;
    public override int ManaDrainMin => 15;
    public override int ManaDrainMax => 25;
    public override int LightBonusDamage => 8;

    public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
    public override WeaponAbility SecondaryAbility => WeaponAbility.CrushingBlow;

    public override int AosMinDamage => 15;
    public override int AosMaxDamage => 19;
    public override int AosSpeed => 33;
    public override float MlSpeed => 3.25f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public StaffOfDamnation() : base(0xDF0)
    {
        Name = "Staff of Damnation";
        Hue = 0x0497;
        Weight = 6.0;
    }
}
