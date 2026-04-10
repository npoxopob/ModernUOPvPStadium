using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Human.Weapons;

/// <summary>Level 4 human sword. 3 sockets, higher base damage.</summary>
[SerializationGenerator(0, false)]
public partial class CommanderSword : BaseHumanSword
{
    public override int RequiredHumanLevel => 4;
    public override int MaxSockets => 3;

    public override WeaponAbility PrimaryAbility => WeaponAbility.ArmorIgnore;
    public override WeaponAbility SecondaryAbility => WeaponAbility.MortalStrike;

    public override int AosMinDamage => 18;
    public override int AosMaxDamage => 22;
    public override int AosSpeed => 38;
    public override float MlSpeed => 2.75f;
    public override int AosStrengthReq => 10;
    public override int InitMinHits => 255;
    public override int InitMaxHits => 255;

    [Constructible]
    public CommanderSword() : base(0xF61)
    {
        Name = "Commander's Sword";
        Hue = 0x0A09;
        Weight = 6.0;
    }
}
