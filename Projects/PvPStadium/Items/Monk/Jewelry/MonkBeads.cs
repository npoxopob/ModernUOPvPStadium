using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Monk.Jewelry;

/// <summary>
/// Monk meditation beads (bracelet slot).
/// Level 1: +5 DEX, Chi max 3
/// Level 2: +8 DEX, Chi max 5, 20% Dodge Magic
/// Level 3: +10 DEX, Chi max 7, 30% Dodge Magic
/// Level 4: +12 DEX, Chi max 10, 40% Dodge Magic, +3 HP regen
/// </summary>
[SerializationGenerator(0)]
public partial class MonkBeads : BaseBracelet
{
    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _requiredLevel;

    [SerializableField(1)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _dodgeMagicChance; // 0-100

    [Constructible]
    public MonkBeads() : this(1)
    {
    }

    [Constructible]
    public MonkBeads(int level) : base(0x1086)
    {
        _requiredLevel = level;
        LootType = LootType.Blessed;

        switch (level)
        {
            case 1:
                Name = "Prayer Beads";
                Hue = 0x0835;
                Attributes.BonusDex = 5;
                break;
            case 2:
                Name = "Meditation Beads";
                Hue = 0x08AB;
                Attributes.BonusDex = 8;
                _dodgeMagicChance = 20;
                break;
            case 3:
                Name = "Jade Beads";
                Hue = 0x08B0;
                Attributes.BonusDex = 10;
                _dodgeMagicChance = 30;
                break;
            default:
                Name = "Celestial Beads";
                Hue = 0x0A09;
                Attributes.BonusDex = 12;
                Attributes.RegenHits = 3;
                _dodgeMagicChance = 40;
                break;
        }
    }

    public override double DefaultWeight => 0.1;

    public override bool CanEquip(Mobile from)
    {
        if (!MonkItemHelper.IsMonk(from, _requiredLevel))
        {
            from.SendMessage(0x22, "Only a monk can wear these beads.");
            return false;
        }

        return base.CanEquip(from);
    }

    /// <summary>
    /// Checks if the monk wearing beads dodges a magical attack.
    /// Returns true if the magic is dodged (negated).
    /// </summary>
    public static bool TryDodgeMagic(Mobile target)
    {
        var beads = target.FindItemOnLayer<MonkBeads>(Layer.Bracelet);
        if (beads == null || beads._dodgeMagicChance <= 0)
        {
            return false;
        }

        if (Utility.Random(100) < beads._dodgeMagicChance)
        {
            target.SendMessage(0x480, "Your meditation allows you to dodge the spell!");
            target.FixedParticles(0x375A, 10, 15, 5037, 0x480, 0, EffectLayer.Waist);
            target.PlaySound(0x28E);
            return true;
        }

        return false;
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        var maxChi = MonkItemHelper.MaxChi(_requiredLevel);
        list.Add(1042971, $"{"Max Chi"}\t{maxChi}");

        if (_dodgeMagicChance > 0)
        {
            list.Add(1042971, $"{"Dodge Magic"}\t{_dodgeMagicChance}%");
        }
    }
}
