using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Necromancer.Weapons;

/// <summary>
/// Base class for necromancer staves (macing, two-handed).
/// Features: Curse of Decay on hit — INT debuff + mana drain.
/// Bonus damage vs "light" classes (paladin).
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseNecroStaff : BaseStaff
{
    public virtual int RequiredNecromancerLevel => 1;

    /// <summary>Chance to apply Curse of Decay (0.0–1.0).</summary>
    public virtual double CurseChance => 0.0;

    /// <summary>Min INT reduction from Curse of Decay.</summary>
    public virtual int CurseIntMin => 15;

    /// <summary>Max INT reduction from Curse of Decay.</summary>
    public virtual int CurseIntMax => 25;

    /// <summary>Duration of Curse of Decay in seconds.</summary>
    public virtual double CurseDuration => 10.0;

    /// <summary>Mana drained on Curse of Decay proc.</summary>
    public virtual int ManaDrainMin => 10;

    /// <summary>Mana drained on Curse of Decay proc.</summary>
    public virtual int ManaDrainMax => 20;

    /// <summary>Extra flat damage vs light targets (paladin).</summary>
    public virtual int LightBonusDamage => 0;

    protected BaseNecroStaff(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!NecromancerItemHelper.IsNecromancer(from, RequiredNecromancerLevel))
        {
            from.SendMessage(0x22, "Only a necromancer can wield this weapon.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void OnHit(Mobile attacker, Mobile defender, double damageBonus = 1.0)
    {
        // Bonus damage vs light classes
        var isLight = NecromancerItemHelper.IsLightClass(defender);
        if (isLight && LightBonusDamage > 0)
        {
            defender.Damage(LightBonusDamage, attacker);
            defender.FixedParticles(0x374A, 10, 15, 5038, 1109, 0, EffectLayer.Head);
        }

        base.OnHit(attacker, defender, damageBonus);

        if (defender == null || !defender.Alive || !attacker.Alive)
            return;

        // Curse of Decay proc
        if (CurseChance > 0.0 && Utility.RandomDouble() < CurseChance)
        {
            ApplyCurseOfDecay(attacker, defender);
        }
    }

    private void ApplyCurseOfDecay(Mobile attacker, Mobile defender)
    {
        var modName = $"{Serial}NecroDecay";

        // Don't stack
        if (defender.GetStatMod(modName) != null)
            return;

        var intReduction = Utility.RandomMinMax(CurseIntMin, CurseIntMax);
        defender.AddStatMod(new StatMod(StatType.Int, modName, -intReduction,
            TimeSpan.FromSeconds(CurseDuration)));

        // Mana drain
        var manaDrain = Utility.RandomMinMax(ManaDrainMin, ManaDrainMax);
        defender.Mana = Math.Max(0, defender.Mana - manaDrain);

        defender.FixedParticles(0x374A, 10, 15, 5038, 1109, 0, EffectLayer.Head);
        defender.PlaySound(0x1FB);
        defender.SendMessage(0x22, "A dark curse saps your mind!");
        attacker.PublicOverheadMessage(MessageType.Emote, 0x455, false, "*Curse of Decay!*");
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (CurseChance > 0.0)
        {
            list.Add(1042971, $"{"Curse of Decay"}\t{(int)(CurseChance * 100)}% (-{CurseIntMin}-{CurseIntMax} {"INT"}, {CurseDuration}{"s"}");
        }

        if (LightBonusDamage > 0)
        {
            list.Add(1042971, $"{"vs Paladin"}\t+{LightBonusDamage} {"damage"}");
        }
    }
}
