using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Vampire.Weapons;

/// <summary>
/// Level 3 vampire fencing weapon. 25% bleed: 1-3 ticks, 15-18 dmg/tick.
/// </summary>
[Flippable(0x18C, 0x18D)]
[SerializationGenerator(0, false)]
public partial class StingOfPain : BaseVampireSickle
{
    [Constructible]
    public StingOfPain() : base(0x18C)
    {
        Name = "Sting of Pain";
        Hue = 0x0A26;
    }

    public override int RequiredVampireLevel => 3;
    public override double BleedChance => 0.25;
    public override int BleedMinTicks => 1;
    public override int BleedMaxTicks => 3;
    public override int BleedMinDamage => 15;
    public override int BleedMaxDamage => 18;

    public override double DefaultWeight => 5.0;
    public override WeaponAbility PrimaryAbility => WeaponAbility.InfectiousStrike;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ShadowStrike;

    public override int AosStrengthReq => 20;
    public override int AosMinDamage => 13;
    public override int AosMaxDamage => 15;
    public override int AosSpeed => 42;
    public override float MlSpeed => 2.75f;

    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;
}
