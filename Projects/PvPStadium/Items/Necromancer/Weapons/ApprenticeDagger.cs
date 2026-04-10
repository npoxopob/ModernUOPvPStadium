using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Necromancer.Weapons;

/// <summary>Level 1 necromancer dagger. No procs.</summary>
[SerializationGenerator(0, false)]
public partial class ApprenticeDagger : BaseNecroDagger
{
    public override int RequiredNecromancerLevel => 1;

    public override WeaponAbility PrimaryAbility => WeaponAbility.InfectiousStrike;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ShadowStrike;

    public override int AosMinDamage => 12;
    public override int AosMaxDamage => 14;
    public override int AosSpeed => 53;
    public override float MlSpeed => 2.00f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public ApprenticeDagger() : base(0xF52) // dagger
    {
        Name = "Apprentice Dagger";
        Hue = 0x0455;
        Weight = 2.0;
    }
}
