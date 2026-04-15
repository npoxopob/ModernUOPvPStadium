using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Berserker.Weapons;

/// <summary>
/// Level 3 (Berserker) axe. Fury-enhanced damage.
/// </summary>
[Flippable(0x13FB, 0x13FA)]
[SerializationGenerator(0)]
public partial class AncientAvenger : BaseBerserkerAxe
{
    public override int RequiredBerserkerLevel => 3;

    public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
    public override WeaponAbility SecondaryAbility => WeaponAbility.BleedAttack;

    public override int AosMinDamage => 15;
    public override int AosMaxDamage => 19;
    public override int AosSpeed => 29;
    public override float MlSpeed => 3.75f;
    public override int AosStrengthReq => 80;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public AncientAvenger() : base(0x13FB)
    {
        Name = "Ancient Avenger";
        Hue = 0x0492;
        Weight = 6.5;
    }
}
