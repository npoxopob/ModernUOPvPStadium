using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Vampire.Weapons;

/// <summary>
/// Level 1 vampire fencing weapon. No special effect.
/// </summary>
[Flippable(0x18C, 0x18D)]
[SerializationGenerator(0)]
public partial class SickleOfNewborn : BaseVampireSickle
{
    [Constructible]
    public SickleOfNewborn() : base(0x18C)
    {
        Name = "Sickle of Newborn";
        Hue = 0x0455;
    }

    public override int RequiredVampireLevel => 1;
    public override double BleedChance => 0.0;

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
