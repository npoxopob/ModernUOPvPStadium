using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Necromancer.Weapons;

/// <summary>Level 4 necromancer dagger. 40% Soul Drain 15-25 HP + 40% deadly poison.</summary>
[SerializationGenerator(0, false)]
public partial class DaggerOfAnnihilation : BaseNecroDagger
{
    public override int RequiredNecromancerLevel => 4;
    public override double SoulDrainChance => 0.40;
    public override int SoulDrainMin => 15;
    public override int SoulDrainMax => 25;
    public override bool AppliesPoison => true;
    public override int PoisonLevel => 4; // Deadly
    public override double PoisonChance => 0.40;

    public override WeaponAbility PrimaryAbility => WeaponAbility.InfectiousStrike;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ShadowStrike;

    public override int AosMinDamage => 18;
    public override int AosMaxDamage => 22;
    public override int AosSpeed => 53;
    public override float MlSpeed => 2.00f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public DaggerOfAnnihilation() : base(0xF52)
    {
        Name = "Dagger of Annihilation";
        Hue = 0x0386;
        Weight = 2.0;
    }
}
