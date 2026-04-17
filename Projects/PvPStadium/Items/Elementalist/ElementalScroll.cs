using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Elementalist;

/// <summary>
/// Elemental Scroll — consumable for the Elementalist race.
/// Instantly fills the player's Overcharge to maximum for their current weapon.
/// Cooldown: 20 seconds.
/// Only elementalists can use.
/// </summary>
[SerializationGenerator(0)]
public partial class ElementalScroll : Item
{
    public const int MaxCharges = 5;
    public const double CooldownSeconds = 20.0;

    [SerializableField(0)]
    [InvalidateProperties]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _charges;

    [Constructible]
    public ElementalScroll() : base(0x0E34) // scroll graphic
    {
        Name = "Elemental Scroll";
        Hue = 0x489; // fire orange
        Weight = 1.0;
        LootType = LootType.Blessed;
        _charges = MaxCharges;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (from is not PlayerMobile pm)
        {
            return;
        }

        if (!IsChildOf(pm.Backpack))
        {
            pm.SendMessage(0x22, "That must be in your pack to use.");
            return;
        }

        if (!ElementalistItemHelper.IsElementalist(pm))
        {
            pm.SendMessage(0x22, "Only an elementalist can use this scroll.");
            return;
        }

        if (_charges <= 0)
        {
            pm.SendMessage(0x22, "The scroll is depleted.");
            return;
        }

        if (!pm.BeginAction<ElementalScroll>())
        {
            pm.SendMessage(0x22, "You must wait before using another scroll.");
            return;
        }

        // Determine max charges from equipped weapon
        var maxOvercharge = GetMaxOvercharge(pm);

        OverchargeSystem.FillCharges(pm, maxOvercharge);

        var element = OverchargeSystem.GetElement(pm);
        var (color, elementName) = element switch
        {
            ElementType.Fire => (0x489, "Fire"),
            ElementType.Ice => (0x480, "Ice"),
            ElementType.Lightning => (0x490, "Lightning"),
            _ => (0x489, "Unknown")
        };

        pm.SendMessage(color, $"The scroll floods you with {elementName} energy! Overcharge: {maxOvercharge}/{maxOvercharge}.");
        pm.PlaySound(0x1FA); // magic sound
        pm.FixedParticles(0x375A, 10, 15, 5037, color, 0, EffectLayer.Waist);

        Charges--;

        if (_charges <= 0)
        {
            pm.SendMessage(0x22, "The scroll crumbles to ash.");
        }

        Timer.DelayCall(TimeSpan.FromSeconds(CooldownSeconds), EndScrollCooldown, pm);
    }

    private static void EndScrollCooldown(Mobile m)
    {
        m?.EndAction<ElementalScroll>();
    }

    /// <summary>
    /// Returns the MaxOvercharge for the weapon the player is currently holding.
    /// Falls back to 3 if no elementalist weapon is equipped.
    /// </summary>
    private static int GetMaxOvercharge(PlayerMobile pm)
    {
        var weapon = pm.FindItemOnLayer<Weapons.BaseElementalistWand>(Layer.TwoHanded)
                     ?? (Item?)pm.FindItemOnLayer<Weapons.BaseElementalistOrb>(Layer.TwoHanded);

        return weapon switch
        {
            Weapons.BaseElementalistWand w => w.MaxOvercharge,
            Weapons.BaseElementalistOrb o => o.MaxOvercharge,
            _ => 3 // default fallback
        };
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);
        list.Add(1042971, $"{"Charges"}\t{_charges}/{MaxCharges}");
        list.Add(1042971, $"{"Fills Overcharge instantly"}");
        list.Add(1042971, $"{"Cooldown"}\t{CooldownSeconds}{"s"}");
        list.Add(1042971, $"{"Elementalist Only"}");
    }
}
