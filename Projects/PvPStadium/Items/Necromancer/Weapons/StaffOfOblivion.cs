using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Necromancer.Weapons;

/// <summary>Level 4 necromancer staff. 40% Curse of Decay, +11 vs paladin.</summary>
[SerializationGenerator(0, false)]
public partial class StaffOfOblivion : BaseNecroStaff
{
    public override int RequiredNecromancerLevel => 4;
    public override double CurseChance => 0.40;
    public override int CurseIntMin => 25;
    public override int CurseIntMax => 40;
    public override double CurseDuration => 15.0;
    public override int ManaDrainMin => 20;
    public override int ManaDrainMax => 35;
    public override int LightBonusDamage => 11;

    public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
    public override WeaponAbility SecondaryAbility => WeaponAbility.CrushingBlow;

    public override int AosMinDamage => 18;
    public override int AosMaxDamage => 22;
    public override int AosSpeed => 33;
    public override float MlSpeed => 3.25f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public StaffOfOblivion() : base(0xDF0)
    {
        Name = "Staff of Oblivion";
        Hue = 0x0386;
        Weight = 6.0;
    }
}
