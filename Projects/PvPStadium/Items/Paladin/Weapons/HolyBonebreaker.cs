using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Paladin.Weapons;

/// <summary>
/// Level 3 (Paladin) mace.
/// Stamina drain 5-10, 33% bone break, bonus vs chaos, 10% heal.
/// </summary>
[SerializationGenerator(0, false)]
public partial class HolyBonebreaker : BasePaladinMace
{
    public override int RequiredPaladinLevel => 3;

    public override int StaminaDrainMin => 5;
    public override int StaminaDrainMax => 10;

    public override bool HasBoneBreak => true;
    public override double BoneBreakChance => 0.33;

    public override bool HasChaosBonus => true;
    public override int ChaosBonusDamage => 8;
    public override double HealFraction => 0.10;

    public override WeaponAbility PrimaryAbility => WeaponAbility.CrushingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ConcussionBlow;

    public override int AosMinDamage => 15;
    public override int AosMaxDamage => 19;
    public override int AosSpeed => 33;
    public override float MlSpeed => 3.25f;
    public override int AosStrengthReq => 40;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public HolyBonebreaker() : base(0x1407)
    {
        Name = "Holy Bonebreaker";
        Hue = 0x0980;
        Weight = 7.1;
    }
}
