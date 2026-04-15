using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Paladin.Weapons;

/// <summary>
/// Base class for paladin mace weapons (mace fighting).
/// Features: stamina drain, bone break debuff, bonus vs chaos, heal on non-chaos.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BasePaladinMace : BaseBashing
{
    // -- Config per subclass --

    /// <summary>Minimum paladin level required to equip.</summary>
    public virtual int RequiredPaladinLevel => 1;

    /// <summary>Min extra stamina drained on hit (on top of BaseBashing default 3-5).</summary>
    public virtual int StaminaDrainMin => 0;

    /// <summary>Max extra stamina drained on hit.</summary>
    public virtual int StaminaDrainMax => 0;

    /// <summary>If true, weapon can apply bone break debuff.</summary>
    public virtual bool HasBoneBreak => false;

    /// <summary>Chance (0.0-1.0) to apply bone break per hit.</summary>
    public virtual double BoneBreakChance => 0.25;

    /// <summary>Duration of bone break debuff in seconds.</summary>
    public virtual double BoneBreakDuration => 10.0;

    /// <summary>DEX penalty applied during bone break.</summary>
    public virtual int BoneBreakDexPenalty => 15;

    /// <summary>If true, deals bonus damage vs chaos classes.</summary>
    public virtual bool HasChaosBonus => false;

    /// <summary>Extra flat damage vs chaos targets.</summary>
    public virtual int ChaosBonusDamage => 0;

    /// <summary>Fraction of damage healed on non-chaos hit.</summary>
    public virtual double HealFraction => 0.0;

    protected BasePaladinMace(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!PaladinItemHelper.IsPaladin(from, RequiredPaladinLevel))
        {
            from.SendMessage(0x22, "Only a paladin can wield this weapon.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void OnHit(Mobile attacker, Mobile defender, double damageBonus = 1.0)
    {
        base.OnHit(attacker, defender, damageBonus);

        if (defender == null || !defender.Alive || !attacker.Alive)
        {
            return;
        }

        var isChaos = PaladinItemHelper.IsChaosClass(defender);

        // Extra stamina drain
        if (StaminaDrainMax > 0)
        {
            var drain = Utility.RandomMinMax(StaminaDrainMin, StaminaDrainMax);
            defender.Stam -= drain;
        }

        // Bone break debuff
        if (HasBoneBreak && Utility.RandomDouble() < BoneBreakChance)
        {
            ApplyBoneBreak(attacker, defender);
        }

        // Bonus damage vs chaos
        if (HasChaosBonus && isChaos && ChaosBonusDamage > 0)
        {
            AOS.Damage(defender, attacker, ChaosBonusDamage, 100, 0, 0, 0, 0); // physical holy damage
            defender.FixedParticles(0x375A, 1, 15, 5054, 0x480, 0, EffectLayer.Head);
        }

        // Heal on non-chaos hit
        if (HealFraction > 0.0 && !isChaos)
        {
            GetBaseDamageRange(attacker, out var wMin, out var wMax);
            var estimated = Math.Max(1, (wMin + wMax) / 2);
            var healed = Math.Max(1, (int)(estimated * HealFraction));
            var missing = attacker.HitsMax - attacker.Hits;
            if (missing > 0)
            {
                healed = Math.Min(healed, missing);
                attacker.Hits += healed;
                attacker.FixedParticles(0x376A, 9, 32, 5005, 0x480, 0, EffectLayer.Waist);
            }
        }
    }

    private void ApplyBoneBreak(Mobile attacker, Mobile defender)
    {
        // Check if already affected
        if (!defender.CanBeginAction<BoneBreakTimer>())
        {
            return;
        }

        defender.BeginAction<BoneBreakTimer>();

        var mod = new DefaultSkillMod(SkillName.Parry, $"{Serial}BoneBreak", true, -BoneBreakDexPenalty);
        defender.AddSkillMod(mod);
        defender.AddStatMod(new StatMod(StatType.Dex, $"{Serial}BoneBreakDex", -BoneBreakDexPenalty, TimeSpan.Zero));

        defender.SendMessage(0x22, "Your bones crack from the impact!");
        defender.PlaySound(0x204);
        defender.FixedParticles(0x3728, 10, 15, 5038, EffectLayer.Head);

        attacker.PublicOverheadMessage(MessageType.Emote, 0x480, false, "*Bone Break!*");

        var state = new BoneBreakState(defender, mod, $"{Serial}BoneBreakDex");
        Timer.DelayCall(TimeSpan.FromSeconds(BoneBreakDuration), EndBoneBreak, state);
    }

    private static void EndBoneBreak(BoneBreakState state)
    {
        state.Defender.RemoveSkillMod(state.Mod);
        state.Defender.RemoveStatMod(state.StatModName);
        state.Defender.EndAction<BoneBreakTimer>();
        if (state.Defender.Alive)
        {
            state.Defender.SendMessage(0x3B2, "Your bones mend and the pain subsides.");
        }
    }

    private record struct BoneBreakState(Mobile Defender, DefaultSkillMod Mod, string StatModName);

    // Timer marker type for BeginAction/EndAction
    private class BoneBreakTimer;

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (StaminaDrainMax > 0)
        {
            list.Add(1042971, $"{"Stamina Drain"}\t{StaminaDrainMin}-{StaminaDrainMax}");
        }

        if (HasBoneBreak)
        {
            list.Add(1042971, $"{"Bone Break"}\t{(int)(BoneBreakChance * 100)}%, {BoneBreakDuration}{"s"}");
        }

        if (HasChaosBonus)
        {
            list.Add(1042971, $"{"Bonus vs Chaos"}\t+{ChaosBonusDamage}");
        }

        if (HealFraction > 0.0)
        {
            list.Add(1042971, $"{"Heals"}\t{(int)(HealFraction * 100)}% {"on hit (non-chaos)"}");
        }
    }
}
