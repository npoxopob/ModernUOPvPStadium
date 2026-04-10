using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Paladin.Weapons;

/// <summary>
/// Level 4 (Guardian of Heaven) paladin mace.
/// Stamina drain 8-11, 40% bone break, bonus vs chaos, 10% heal.
/// </summary>
[SerializationGenerator(0, false)]
public partial class MaceOfRetribution : BasePaladinMace
{
    public override int RequiredPaladinLevel => 4;

    public override int StaminaDrainMin => 8;
    public override int StaminaDrainMax => 11;

    public override bool HasBoneBreak => true;
    public override double BoneBreakChance => 0.40;
    public override double BoneBreakDuration => 12.0;

    public override bool HasChaosBonus => true;
    public override int ChaosBonusDamage => 11;
    public override double HealFraction => 0.10;

    public override WeaponAbility PrimaryAbility => WeaponAbility.CrushingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ConcussionBlow;

    public override int AosMinDamage => 18;
    public override int AosMaxDamage => 22;
    public override int AosSpeed => 33;
    public override float MlSpeed => 3.25f;
    public override int AosStrengthReq => 40;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public MaceOfRetribution() : base(0x1407)
    {
        Name = "Mace of Retribution";
        Hue = 0x0A90;
        Weight = 7.0;
    }
}
