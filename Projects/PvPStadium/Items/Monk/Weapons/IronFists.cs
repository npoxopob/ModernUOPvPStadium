using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Monk.Weapons;

/// <summary>Level 2 monk fists. 20% Rapid Flurry (3 hits).</summary>
[SerializationGenerator(0)]
public partial class IronFists : BaseMonkFists
{
    public override int RequiredMonkLevel => 2;

    public override double FlurryChance => 0.20;
    public override int FlurryHits => 3;
    public override double FlurryDamageMod => 0.40;

    public override WeaponAbility PrimaryAbility => WeaponAbility.Disarm;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ParalyzingBlow;

    public override int AosMinDamage => 12;
    public override int AosMaxDamage => 15;
    public override int AosSpeed => 50;
    public override float MlSpeed => 2.0f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public IronFists() : base(0x13B9)
    {
        Name = "Iron Fists";
        Hue = 0x08AB;
        Weight = 1.0;
    }
}
