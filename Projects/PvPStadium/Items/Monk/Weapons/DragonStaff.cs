using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Monk.Weapons;

/// <summary>Level 3 monk staff. 33% Counter Strike (x4 Chi damage), +8 vs Berserker.</summary>
[SerializationGenerator(0)]
public partial class DragonStaff : BaseMonkStaff
{
    public override int RequiredMonkLevel => 3;

    public override double CounterStrikeChance => 0.33;
    public override int ChiDamageMultiplier => 4;
    public override int BerserkerBonusDamage => 8;

    public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
    public override WeaponAbility SecondaryAbility => WeaponAbility.CrushingBlow;

    public override int AosMinDamage => 15;
    public override int AosMaxDamage => 19;
    public override int AosSpeed => 33;
    public override float MlSpeed => 2.75f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public DragonStaff() : base(0xDF0)
    {
        Name = "Dragon Staff";
        Hue = 0x08B0;
        Weight = 6.0;
    }
}
