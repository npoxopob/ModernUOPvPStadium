using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Monk.Weapons;

/// <summary>Level 1 monk staff. No Counter Strike procs.</summary>
[SerializationGenerator(0, false)]
public partial class BambooStaff : BaseMonkStaff
{
    public override int RequiredMonkLevel => 1;

    public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ParalyzingBlow;

    public override int AosMinDamage => 13;
    public override int AosMaxDamage => 16;
    public override int AosSpeed => 33;
    public override float MlSpeed => 3.25f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public BambooStaff() : base(0xDF0)
    {
        Name = "Bamboo Staff";
        Hue = 0x0835;
        Weight = 5.0;
    }
}
