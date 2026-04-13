using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Monk;

/// <summary>
/// Incense — monk consumable (level 2+).
/// Instantly grants 3 Chi charges. 30 second cooldown.
/// 50 charges total.
/// </summary>
[SerializationGenerator(0, false)]
public partial class Incense : Item
{
    public static int MaxCharges => 50;
    public static int ChiGrant => 3;
    public static double CooldownSeconds => 30.0;

    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _charges;

    [Constructible]
    public Incense() : base(0x1BD1) // candle/incense graphic
    {
        Name = "Monk Incense";
        Hue = 0x0835;
        Weight = 1.0;
        LootType = LootType.Blessed;
        _charges = MaxCharges;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!IsChildOf(from.Backpack))
        {
            from.SendLocalizedMessage(1042001);
            return;
        }

        if (!MonkItemHelper.IsMonk(from, 2))
        {
            from.SendMessage(0x22, "Only an Acolyte or higher can use Incense.");
            return;
        }

        if (_charges <= 0)
        {
            from.SendMessage(0x22, "This incense is burned out.");
            return;
        }

        if (!from.CanBeginAction<Incense>())
        {
            from.SendMessage(0x22, "You must wait before using incense again.");
            return;
        }

        var level = MonkItemHelper.GetMonkLevel(from);
        var maxChi = MonkItemHelper.MaxChi(level);

        // Grant Chi charges
        for (var i = 0; i < ChiGrant; i++)
        {
            ChiSystem.AddCharge(from, maxChi);
        }

        var currentChi = ChiSystem.GetCharges(from);

        _charges--;
        InvalidateProperties();

        from.BeginAction<Incense>();
        Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), () => from.EndAction<Incense>());

        from.FixedParticles(0x376A, 9, 32, 5007, 0x480, 0, EffectLayer.Waist);
        from.PlaySound(0x1E3);
        from.SendMessage(0x480, $"The incense fills you with energy. Chi: {currentChi}/{maxChi}");

        if (_charges <= 0)
        {
            from.SendMessage(0x22, "The incense is now fully burned.");
        }
    }

    public override void AddNameProperties(IPropertyList list)
    {
        base.AddNameProperties(list);
        list.Add(1042971, $"Charges: {_charges}/{MaxCharges}");
        list.Add(1042971, $"Grants +{ChiGrant} Chi instantly ({CooldownSeconds}s cooldown)");
    }
}
