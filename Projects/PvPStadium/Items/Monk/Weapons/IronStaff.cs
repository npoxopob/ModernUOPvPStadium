using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Monk.Weapons;

/// <summary>Level 2 monk staff. 25% Counter Strike (x3 Chi damage).</summary>
[SerializationGenerator(0, false)]
public partial class IronStaff : BaseMonkStaff
{
    public override int RequiredMonkLevel => 2;

    public override double CounterStrikeChance => 0.25;
    public override int ChiDamageMultiplier => 3;

    public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
    public override WeaponAbility SecondaryAbility => WeaponAbility.CrushingBlow;

    public override int AosMinDamage => 14;
    public override int AosMaxDamage => 17;
    public override int AosSpeed => 33;
    public override float MlSpeed => 3.0f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public IronStaff() : base(0xDF0)
    {
        Name = "Iron Staff";
        Hue = 0x08AB;
        Weight = 6.0;
    }
}
