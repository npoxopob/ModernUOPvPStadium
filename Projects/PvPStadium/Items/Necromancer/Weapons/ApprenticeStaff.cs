using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Necromancer.Weapons;

/// <summary>Level 1 necromancer staff. No procs.</summary>
[SerializationGenerator(0)]
public partial class ApprenticeStaff : BaseNecroStaff
{
    public override int RequiredNecromancerLevel => 1;

    public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
    public override WeaponAbility SecondaryAbility => WeaponAbility.CrushingBlow;

    public override int AosMinDamage => 13;
    public override int AosMaxDamage => 16;
    public override int AosSpeed => 33;
    public override float MlSpeed => 3.25f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public ApprenticeStaff() : base(0xDF0) // black staff
    {
        Name = "Apprentice Staff";
        Hue = 0x0455;
        Weight = 6.0;
    }
}
