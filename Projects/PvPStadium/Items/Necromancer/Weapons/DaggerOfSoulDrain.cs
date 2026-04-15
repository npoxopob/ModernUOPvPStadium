using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Necromancer.Weapons;

/// <summary>Level 2 necromancer dagger. 25% Soul Drain 8-15 HP.</summary>
[SerializationGenerator(0)]
public partial class DaggerOfSoulDrain : BaseNecroDagger
{
    public override int RequiredNecromancerLevel => 2;
    public override double SoulDrainChance => 0.25;
    public override int SoulDrainMin => 8;
    public override int SoulDrainMax => 15;

    public override WeaponAbility PrimaryAbility => WeaponAbility.InfectiousStrike;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ShadowStrike;

    public override int AosMinDamage => 13;
    public override int AosMaxDamage => 15;
    public override int AosSpeed => 53;
    public override float MlSpeed => 2.00f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public DaggerOfSoulDrain() : base(0xF52)
    {
        Name = "Dagger of Soul Drain";
        Hue = 0x0482;
        Weight = 2.0;
    }
}
