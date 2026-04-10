using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Berserker;

/// <summary>
/// Berserker racial consumable — heals HP and restores Stamina.
/// 25 charges, 4 second cooldown.
/// Healing amount scales with berserker level.
/// </summary>
[SerializationGenerator(0, false)]
public partial class Restoration : Item
{
    public static int MaxCharges => 25;
    public static double CooldownSeconds => 4.0;

    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _charges;

    [Constructible]
    public Restoration() : base(0x0F0E) // potion bottle graphic
    {
        Name = "Restoration";
        Hue = 0x042C;
        Weight = 1.0;
        LootType = LootType.Blessed;
        _charges = MaxCharges;
    }

    /// <summary>HP heal range by level.</summary>
    private static (int min, int max) GetHealRange(int level) => level switch
    {
        1 => (13, 17),
        2 => (15, 19),
        3 => (17, 24),
        4 => (17, 24),
        _ => (10, 15)
    };

    /// <summary>Stamina restore range by level.</summary>
    private static (int min, int max) GetStaminaRange(int level) => level switch
    {
        1 => (20, 25),
        2 => (25, 30),
        3 => (30, 35),
        4 => (20, 25),
        _ => (15, 20)
    };

    public override void OnDoubleClick(Mobile from)
    {
        if (!IsChildOf(from.Backpack))
        {
            from.SendLocalizedMessage(1042001);
            return;
        }

        var level = BerserkerItemHelper.GetBerserkerLevel(from);
        if (level < 1)
        {
            from.SendMessage(0x22, "Only a berserker can use Restoration.");
            return;
        }

        if (_charges <= 0)
        {
            from.SendMessage(0x22, "This Restoration flask is empty.");
            return;
        }

        if (from.Hits >= from.HitsMax && from.Stam >= from.StamMax)
        {
            from.SendMessage(0x22, "You are already at full health and stamina.");
            return;
        }

        if (!from.CanBeginAction<Restoration>())
        {
            from.SendMessage(0x22, "You must wait before using Restoration again.");
            return;
        }

        from.BeginAction<Restoration>();
        Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), () => from.EndAction<Restoration>());

        var (healMin, healMax) = GetHealRange(level);
        var (stamMin, stamMax) = GetStaminaRange(level);

        var heal = Utility.RandomMinMax(healMin, healMax);
        var stam = Utility.RandomMinMax(stamMin, stamMax);

        var hpMissing = from.HitsMax - from.Hits;
        if (hpMissing > 0)
        {
            from.Hits += Math.Min(heal, hpMissing);
        }

        var stamMissing = from.StamMax - from.Stam;
        if (stamMissing > 0)
        {
            from.Stam += Math.Min(stam, stamMissing);
        }

        _charges--;
        InvalidateProperties();

        from.PlaySound(0x31);
        from.FixedParticles(0x376A, 9, 32, 5005, 0x26, 0, EffectLayer.Waist);
        from.SendMessage(0x3B2, $"You drink the Restoration. (+{Math.Min(heal, hpMissing > 0 ? hpMissing : 0)} HP, +{Math.Min(stam, stamMissing > 0 ? stamMissing : 0)} Stam)");

        if (_charges <= 0)
        {
            from.SendMessage(0x22, "The Restoration flask is now empty.");
        }
    }

    public override void AddNameProperties(IPropertyList list)
    {
        base.AddNameProperties(list);
        list.Add(1042971, $"Charges: {_charges}/{MaxCharges}");
        list.Add(1042971, "Restores HP and Stamina");
    }
}
