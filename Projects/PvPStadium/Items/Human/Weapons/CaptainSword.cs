using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Human.Weapons;

/// <summary>Level 3 human sword. 3 sockets.</summary>
[SerializationGenerator(0, false)]
public partial class CaptainSword : BaseHumanSword
{
    public override int RequiredHumanLevel => 3;
    public override int MaxSockets => 3;

    public override WeaponAbility PrimaryAbility => WeaponAbility.ArmorIgnore;
    public override WeaponAbility SecondaryAbility => WeaponAbility.MortalStrike;

    public override int AosMinDamage => 15;
    public override int AosMaxDamage => 19;
    public override int AosSpeed => 38;
    public override float MlSpeed => 2.75f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public CaptainSword() : base(0xF61)
    {
        Name = "Captain's Sword";
        Hue = 0x08B0;
        Weight = 6.0;
    }
}
