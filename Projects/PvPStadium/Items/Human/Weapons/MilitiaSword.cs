using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Human.Weapons;

/// <summary>Level 1 human sword. 1 socket.</summary>
[SerializationGenerator(0)]
public partial class MilitiaSword : BaseHumanSword
{
    public override int RequiredHumanLevel => 1;
    public override int MaxSockets => 1;

    public override WeaponAbility PrimaryAbility => WeaponAbility.ArmorIgnore;
    public override WeaponAbility SecondaryAbility => WeaponAbility.Disarm;

    public override int AosMinDamage => 13;
    public override int AosMaxDamage => 16;
    public override int AosSpeed => 38;
    public override float MlSpeed => 2.75f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public MilitiaSword() : base(0xF61) // longsword
    {
        Name = "Militia Sword";
        Hue = 0x0835;
        Weight = 6.0;
    }
}
