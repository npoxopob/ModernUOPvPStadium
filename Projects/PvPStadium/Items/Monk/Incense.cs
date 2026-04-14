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
[SerializationGenerator(0)]
public partial class Incense : Item
{
    public const int MaxCharges = 50;
    public const int ChiGrant = 3;
    public static readonly TimeSpan CooldownDelay = TimeSpan.FromSeconds(30.0);

    [SerializableField(0)]
    [InvalidateProperties]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _charges;

    [Constructible]
    public Incense() : base(0x1BD1)
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

        for (var i = 0; i < ChiGrant; i++)
        {
            ChiSystem.AddCharge(from, maxChi);
        }

        var currentChi = ChiSystem.GetCharges(from);

        Charges--;

        from.BeginAction<Incense>();
        Timer.DelayCall(CooldownDelay, EndCooldown, from);

        from.FixedParticles(0x376A, 9, 32, 5007, 0x480, 0, EffectLayer.Waist);
        from.PlaySound(0x1E3);
        from.SendMessage(0x480, $"The incense fills you with energy. Chi: {currentChi}/{maxChi}");

        if (_charges <= 0)
        {
            from.SendMessage(0x22, "The incense is now fully burned.");
        }
    }

    private static void EndCooldown(Mobile m)
    {
        m?.EndAction<Incense>();
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);
        list.Add(1042971, $"{"Charges"}\t{_charges}/{MaxCharges}");
        list.Add(1042971, $"{"Use"}\tGrants +{ChiGrant} Chi (30s cooldown)");
    }
}
