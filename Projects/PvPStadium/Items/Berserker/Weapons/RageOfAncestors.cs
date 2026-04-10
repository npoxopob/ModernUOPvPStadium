using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Berserker.Weapons;

/// <summary>
/// Level 4 (Child of Ragnar) elite berserker axe. Highest fury-enhanced damage.
/// </summary>
[Flippable(0x13FB, 0x13FA)]
[SerializationGenerator(0, false)]
public partial class RageOfAncestors : BaseBerserkerAxe
{
    public override int RequiredBerserkerLevel => 4;

    public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
    public override WeaponAbility SecondaryAbility => WeaponAbility.BleedAttack;

    public override int AosMinDamage => 18;
    public override int AosMaxDamage => 22;
    public override int AosSpeed => 29;
    public override float MlSpeed => 3.75f;
    public override int AosStrengthReq => 80;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public RageOfAncestors() : base(0x13FB)
    {
        Name = "Rage of Ancestors";
        Hue = 0x0A2C;
        Weight = 6.5;
    }
}
