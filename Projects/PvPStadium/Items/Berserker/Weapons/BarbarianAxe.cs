using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Berserker.Weapons;

/// <summary>
/// Level 2 (Barbarian) berserker axe. Fury-enhanced damage.
/// </summary>
[Flippable(0x13FB, 0x13FA)]
[SerializationGenerator(0)]
public partial class BarbarianAxe : BaseBerserkerAxe
{
    public override int RequiredBerserkerLevel => 2;

    public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
    public override WeaponAbility SecondaryAbility => WeaponAbility.BleedAttack;

    public override int AosMinDamage => 14;
    public override int AosMaxDamage => 17;
    public override int AosSpeed => 29;
    public override float MlSpeed => 3.75f;
    public override int AosStrengthReq => 80;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public BarbarianAxe() : base(0x13FB)
    {
        Name = "Barbarian Axe";
        Hue = 0x047F;
        Weight = 7.0;
    }
}
