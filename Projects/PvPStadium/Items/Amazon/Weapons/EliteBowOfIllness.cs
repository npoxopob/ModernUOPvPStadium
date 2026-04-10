using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>Level 4 (Elite Amazon) bow. 60% lethal poison + 25% distance critical.</summary>
[SerializationGenerator(0, false)]
public partial class EliteBowOfIllness : BaseAmazonBow
{
    public override int RequiredAmazonLevel => 4;
    public override bool HasPoisonProc => true;
    public override double PoisonChance => 0.60;
    public override int PoisonLevel => 4; // Lethal
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
    public EliteBowOfIllness() : base(0x13B2)
    {
        Name = "Elite Bow of Illness";
        Hue = 0x0489;
        Weight = 3.8;
    }
}
