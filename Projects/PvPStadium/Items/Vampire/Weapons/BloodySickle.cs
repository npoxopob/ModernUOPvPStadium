using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Vampire.Weapons;

/// <summary>
/// Level 2 vampire fencing weapon. 25% bleed: 1-2 ticks, 13-16 dmg/tick.
/// </summary>
[Flippable(0x18C, 0x18D)]
[SerializationGenerator(0, false)]
public partial class BloodySickle : BaseVampireSickle
{
    [Constructible]
    public BloodySickle() : base(0x18C)
    {
        Name = "Bloody Sickle";
        Hue = 0x0A11;
    }

    public override int RequiredVampireLevel => 2;
    public override double BleedChance => 0.25;
    public override int BleedMinTicks => 1;
    public override int BleedMaxTicks => 2;
    public override int BleedMinDamage => 13;
    public override int BleedMaxDamage => 16;

    public override double DefaultWeight => 5.0;
    public override WeaponAbility PrimaryAbility => WeaponAbility.InfectiousStrike;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ShadowStrike;

    public override int AosStrengthReq => 20;
    public override int AosMinDamage => 12;
    public override int AosMaxDamage => 14;
    public override int AosSpeed => 42;
    public override float MlSpeed => 2.75f;

    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;
}
