using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Vampire.Weapons;

/// <summary>
/// Level 4 vampire fencing weapon. 25% bleed: 1-3 ticks, 15-18 dmg/tick.
/// Higher base damage than lower tiers.
/// </summary>
[Flippable(0x18C, 0x18D)]
[SerializationGenerator(0, false)]
public partial class StingOfRevenge : BaseVampireSickle
{
    [Constructible]
    public StingOfRevenge() : base(0x18C)
    {
        Name = "Sting of Revenge";
        Hue = 0x0AD0;
    }

    public override int RequiredVampireLevel => 4;
    public override double BleedChance => 0.25;
    public override int BleedMinTicks => 1;
    public override int BleedMaxTicks => 3;
    public override int BleedMinDamage => 15;
    public override int BleedMaxDamage => 18;

    public override double DefaultWeight => 5.0;
    public override WeaponAbility PrimaryAbility => WeaponAbility.InfectiousStrike;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ShadowStrike;

    public override int AosStrengthReq => 20;
    public override int AosMinDamage => 15;
    public override int AosMaxDamage => 17;
    public override int AosSpeed => 42;
    public override float MlSpeed => 2.75f;

    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;
}
