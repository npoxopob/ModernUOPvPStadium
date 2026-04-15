using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Berserker.Weapons;

/// <summary>
/// Level 1 (Adept of Might) berserker axe. Basic two-handed axe.
/// </summary>
[Flippable(0x13FB, 0x13FA)]
[SerializationGenerator(0)]
public partial class AdeptAxe : BaseBerserkerAxe
{
    public override int RequiredBerserkerLevel => 1;

    public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
    public override WeaponAbility SecondaryAbility => WeaponAbility.BleedAttack;

    public override int AosMinDamage => 13;
    public override int AosMaxDamage => 16;
    public override int AosSpeed => 29;
    public override float MlSpeed => 3.75f;
    public override int AosStrengthReq => 80;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public AdeptAxe() : base(0x13FB) // large battle axe
    {
        Name = "Axe of Adept";
        Hue = 0x0A31;
        Weight = 7.5;
    }
}
