using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Elementalist.Jewelry;

/// <summary>
/// Base ring for the Elementalist race.
/// Subclasses set Attributes.CastSpeed / SpellDamage / RegenMana in their constructors.
/// The custom DoubleBurstChance property is used by BaseElementalistWand/Orb to check
/// whether the player has a chance to keep charges after a Burst.
/// </summary>
[SerializationGenerator(0)]
public partial class ElementalistRing : BaseRing
{
    /// <summary>Minimum elementalist level required to equip.</summary>
    public virtual int RequiredElementalistLevel => 1;

    /// <summary>
    /// Chance (0.0-1.0) to trigger a Double Burst: burst fires but Overcharge charges
    /// are NOT consumed. Checked in BaseElementalistWand/Orb TriggerBurst.
    /// </summary>
    public virtual double DoubleBurstChance => 0.0;

    [Constructible]
    public ElementalistRing() : base(0x108A)
    {
        Name = "Focus Ring";
        Hue = 0x489;
        LootType = LootType.Blessed;
        Attributes.CastSpeed = 1;
    }

    protected ElementalistRing(string name, int hue) : base(0x108A)
    {
        Name = name;
        Hue = hue;
        LootType = LootType.Blessed;
    }

    public override double DefaultWeight => 0.1;

    public override bool CanEquip(Mobile from)
    {
        if (!ElementalistItemHelper.IsElementalist(from, RequiredElementalistLevel))
        {
            from.SendMessage(0x22, "Only an elementalist can wear this ring.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list); // shows standard Attributes (FC, SD, ManaRegen) automatically

        if (DoubleBurstChance > 0.0)
        {
            list.Add(1042971, $"{"Double Burst Chance"}\t{(int)(DoubleBurstChance * 100)}%");
        }

        var levelName = RequiredElementalistLevel switch
        {
            1 => "Initiate",
            2 => "Evoker",
            3 => "Elementalist",
            4 => "Archmage",
            _ => $"Level {RequiredElementalistLevel}"
        };

        list.Add(1042971, $"{"Requires"}\t{levelName}");
    }
}
