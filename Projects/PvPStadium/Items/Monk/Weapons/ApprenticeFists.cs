using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Monk.Weapons;

/// <summary>Level 1 monk fists. Fast, no Flurry proc.</summary>
[SerializationGenerator(0, false)]
public partial class ApprenticeFists : BaseMonkFists
{
    public override int RequiredMonkLevel => 1;

    public override WeaponAbility PrimaryAbility => WeaponAbility.Disarm;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ParalyzingBlow;

    public override int AosMinDamage => 10;
    public override int AosMaxDamage => 13;
    public override int AosSpeed => 50;
    public override float MlSpeed => 2.25f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public ApprenticeFists() : base(0x13C6) // leather gloves
    {
        Name = "Apprentice Fists";
        Hue = 0x0835;
        Weight = 1.0;
        Layer = Layer.OneHanded;
    }
}
