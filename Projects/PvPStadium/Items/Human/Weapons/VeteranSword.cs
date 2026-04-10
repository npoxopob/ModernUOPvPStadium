using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Human.Weapons;

/// <summary>Level 2 human sword. 2 sockets.</summary>
[SerializationGenerator(0, false)]
public partial class VeteranSword : BaseHumanSword
{
    public override int RequiredHumanLevel => 2;
    public override int MaxSockets => 2;

    public override WeaponAbility PrimaryAbility => WeaponAbility.ArmorIgnore;
    public override WeaponAbility SecondaryAbility => WeaponAbility.ConcussionBlow;

    public override int AosMinDamage => 14;
    public override int AosMaxDamage => 17;
    public override int AosSpeed => 38;
    public override float MlSpeed => 2.75f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public VeteranSword() : base(0xF61)
    {
        Name = "Veteran Sword";
        Hue = 0x08AB;
        Weight = 6.0;
    }
}
