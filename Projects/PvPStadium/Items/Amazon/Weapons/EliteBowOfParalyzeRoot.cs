using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 4 (Elite Amazon) bow. 60% paralyze + 25% distance critical.</summary>
[SerializationGenerator(0)]
public partial class EliteBowOfParalyzeRoot : BaseAmazonBow
{
    public override int RequiredAmazonLevel => 4;
    public override bool HasParalyzeProc => true;
    public override double ParalyzeChance => 0.60;
    public override double ParalyzeDuration => 5.0;
    public override double CritBonus => 0.25;

    public override WeaponAbility PrimaryAbility => WeaponAbility.ParalyzingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.MortalStrike;

    public override int AosMinDamage => 20;
    public override int AosMaxDamage => 24;
    public override int AosSpeed => 25;
    public override float MlSpeed => 4.25f;
    public override int AosStrengthReq => 30;
    public override int DefMaxRange => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public EliteBowOfParalyzeRoot() : base(0x13B2)
    {
        Name = "Elite Bow of Paralyze Root";
        Hue = 0x0489;
        Weight = 3.8;
    }
}
