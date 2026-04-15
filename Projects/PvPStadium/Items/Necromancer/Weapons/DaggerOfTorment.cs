using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Necromancer.Weapons;

/// <summary>Level 3 necromancer dagger. 33% Soul Drain 12-20 HP + 33% poison.</summary>
[SerializationGenerator(0)]
public partial class DaggerOfTorment : BaseNecroDagger
{
    public override int RequiredNecromancerLevel => 3;
    public override double SoulDrainChance => 0.33;
    public override int SoulDrainMin => 12;
    public override int SoulDrainMax => 20;
    public override bool AppliesPoison => true;
    public override int PoisonLevel => 3; // Greater
    public override double PoisonChance => 0.33;

    public override WeaponAbility PrimaryAbility => WeaponAbility.InfectiousStrike;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ShadowStrike;

    public override int AosMinDamage => 15;
    public override int AosMaxDamage => 18;
    public override int AosSpeed => 53;
    public override float MlSpeed => 2.00f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public DaggerOfTorment() : base(0xF52)
    {
        Name = "Dagger of Torment";
        Hue = 0x0497;
        Weight = 2.0;
    }
}
