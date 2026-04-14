using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Monk.Weapons;

/// <summary>
/// Level 4 monk fists. 33% Rapid Flurry (4 hits), ultra-fast 1.75s.
/// Shatters Human shield ReflectPhysical on Flurry.
/// </summary>
[SerializationGenerator(0)]
public partial class FistsOfHeaven : BaseMonkFists
{
    public override int RequiredMonkLevel => 4;

    public override double FlurryChance => 0.33;
    public override int FlurryHits => 4;
    public override double FlurryDamageMod => 0.40;
    public override bool DestroysShields => true;

    public override WeaponAbility PrimaryAbility => WeaponAbility.Disarm;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ParalyzingBlow;

    public override int AosMinDamage => 16;
    public override int AosMaxDamage => 20;
    public override int AosSpeed => 50;
    public override float MlSpeed => 1.75f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public FistsOfHeaven() : base(0x13B9)
    {
        Name = "Fists of Heaven";
        Hue = 0x0A09;
        Weight = 1.0;
    }
}
