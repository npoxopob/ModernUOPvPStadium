using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Paladin.Weapons;

/// <summary>
/// Level 2 (Knight of Justice) paladin mace.
/// Stamina drain 3-8, 25% bone break, bonus vs chaos, 10% heal.
/// </summary>
[SerializationGenerator(0, false)]
public partial class MaceOfDisruption : BasePaladinMace
{
    public override int RequiredPaladinLevel => 2;

    public override int StaminaDrainMin => 3;
    public override int StaminaDrainMax => 8;

    public override bool HasBoneBreak => true;
    public override double BoneBreakChance => 0.25;

    public override bool HasChaosBonus => true;
    public override int ChaosBonusDamage => 5;
    public override double HealFraction => 0.10;

    public override WeaponAbility PrimaryAbility => WeaponAbility.CrushingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ConcussionBlow;

    public override int AosMinDamage => 14;
    public override int AosMaxDamage => 17;
    public override int AosSpeed => 33;
    public override float MlSpeed => 3.25f;
    public override int AosStrengthReq => 40;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public MaceOfDisruption() : base(0x1407)
    {
        Name = "Mace of Disruption";
        Hue = 0x0A85;
        Weight = 7.6;
    }
}
