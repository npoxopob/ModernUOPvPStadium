using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Monk.Weapons;

/// <summary>Level 3 monk fists. 25% Rapid Flurry (3 hits), fast 2.0s.</summary>
[SerializationGenerator(0)]
public partial class TigerFists : BaseMonkFists
{
    public override int RequiredMonkLevel => 3;

    public override double FlurryChance => 0.25;
    public override int FlurryHits => 3;
    public override double FlurryDamageMod => 0.40;

    public override WeaponAbility PrimaryAbility => WeaponAbility.Disarm;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ParalyzingBlow;

    public override int AosMinDamage => 14;
    public override int AosMaxDamage => 17;
    public override int AosSpeed => 50;
    public override float MlSpeed => 2.0f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public TigerFists() : base(0x13B9)
    {
        Name = "Tiger Fists";
        Hue = 0x08B0;
        Weight = 1.0;
    }
}
