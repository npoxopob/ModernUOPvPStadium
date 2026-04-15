using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Paladin.Weapons;

/// <summary>
/// Level 1 (Follower of Light) paladin mace. Basic weapon, no special procs.
/// </summary>
[SerializationGenerator(0)]
public partial class BlessedMace : BasePaladinMace
{
    public override int RequiredPaladinLevel => 1;

    // No special effects at level 1
    public override bool HasBoneBreak => false;
    public override bool HasChaosBonus => false;
    public override double HealFraction => 0.0;

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
    public BlessedMace() : base(0x1407) // war mace graphic
    {
        Name = "Blessed Mace";
        Hue = 0x0438;
        Weight = 8.1;
    }
}
