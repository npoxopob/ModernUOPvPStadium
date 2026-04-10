using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Vampire;

/// <summary>
/// Vampire racial healing item. 100 charges, heals 26-38 HP + Vampirism skill bonus.
/// Cooldown: 1 second between drinks.
/// Only vampires can use.
/// pvp_alfa: Bottle of Blood — Vampirism bonus: if skill > 110: +5 + (skill-1000)/20; else +(skill-1000)/10.
/// Reduced by 30% under Holy Essence effect (not implemented yet).
/// </summary>
[SerializationGenerator(0, false)]
public partial class BottleOfBlood : Item
{
    public static int MinHeal => 26;
    public static int MaxHeal => 38;
    public static int MaxCharges => 100;
    public static double CooldownSeconds => 1.0;

    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _charges;

    [Constructible]
    public BottleOfBlood() : base(0x0F0E) // potion bottle graphic
    {
        Name = "Bottle of Blood";
        Hue = 0x0020; // dark red
        Weight = 1.0;
        LootType = LootType.Blessed;
        _charges = MaxCharges;
    }

    public override void AddNameProperties(IPropertyList list)
    {
        base.AddNameProperties(list);
        list.Add(1042971, $"Charges: {_charges}/{MaxCharges}");
        list.Add(1042971, "Vampire Only");
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (from is not PlayerMobile pm)
            return;

        if (!IsChildOf(pm.Backpack))
        {
            pm.SendMessage(0x22, "That must be in your pack to use.");
            return;
        }

        if (!VampireItemHelper.IsVampire(pm))
        {
            pm.SendMessage(0x22, "Only a vampire can drink blood.");
            return;
        }

        if (pm.Hits >= pm.HitsMax)
        {
            pm.SendMessage(0x22, "You are already at full health.");
            return;
        }

        if (_charges <= 0)
        {
            pm.SendMessage(0x22, "The bottle is empty.");
            return;
        }

        // Cooldown check
        if (!pm.BeginAction<BottleOfBlood>())
        {
            pm.SendMessage(0x22, "You must wait before drinking again.");
            return;
        }

        // Calculate heal amount
        var baseHeal = Utility.RandomMinMax(MinHeal, MaxHeal);
        var bonus = GetVampirismBonus(pm);
        var totalHeal = baseHeal + bonus;

        pm.Heal(totalHeal);

        // Effects
        pm.PlaySound(0x2D6); // drink sound
        pm.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);

        if (pm.Body.IsHuman && !pm.Mounted)
            pm.Animate(34, 5, 1, true, false, 0);

        _charges--;
        InvalidateProperties();

        if (_charges <= 0)
            pm.SendMessage(0x22, "The bottle is now empty.");

        // End cooldown after delay
        Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), pm.EndAction<BottleOfBlood>);
    }

    /// <summary>
    /// Bonus heal from Vampirism skill, matching pvp_alfa formula:
    /// if skill > 110.0: +5 + (skill - 100.0) / 2
    /// else: +(skill - 100.0) / 1
    /// Minimum bonus: 0
    /// </summary>
    private static int GetVampirismBonus(PlayerMobile pm)
    {
        // Vampirism is a custom skill; check if it exists. If not, use Spirit Speak as proxy.
        // In pvp_alfa: Vampirism was a custom skill. We'll use SpiritSpeak for now.
        var skill = pm.Skills[SkillName.SpiritSpeak].Value;

        if (skill <= 100.0)
            return 0;

        if (skill > 110.0)
            return 5 + (int)((skill - 100.0) / 2.0);

        return (int)(skill - 100.0);
    }
}
