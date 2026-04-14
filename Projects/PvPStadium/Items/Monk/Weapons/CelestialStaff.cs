using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Monk.Weapons;

/// <summary>
/// Level 4 monk staff. 40% Counter Strike (x5 Chi damage), +12 vs Berserker.
/// Counter Strike always stuns regardless of Chi count.
/// </summary>
[SerializationGenerator(0)]
public partial class CelestialStaff : BaseMonkStaff
{
    public override int RequiredMonkLevel => 4;

    public override double CounterStrikeChance => 0.40;
    public override int ChiDamageMultiplier => 5;
    public override int BerserkerBonusDamage => 12;
    public override bool CounterStrikeAlwaysStuns => true;

    public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
    public override WeaponAbility SecondaryAbility => WeaponAbility.CrushingBlow;

    public override int AosMinDamage => 18;
    public override int AosMaxDamage => 22;
    public override int AosSpeed => 33;
    public override float MlSpeed => 2.75f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public CelestialStaff() : base(0xDF0)
    {
        Name = "Celestial Staff";
        Hue = 0x0A09;
        Weight = 6.0;
    }
}
