using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Elementalist.Weapons;

/// <summary>
/// Base class for elementalist wands (Macing skill).
/// Features: Overcharge system — each hit accumulates elemental charges.
/// At max charges, triggers Elemental Burst (AoE damage + element-specific effects).
/// Double-click switches element (Fire → Ice → Lightning), resetting charges.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseElementalistWand : BaseBashing
{
    public virtual int RequiredElementalistLevel => 1;

    /// <summary>Max Overcharge charges (3/4/5/6 by level).</summary>
    public virtual int MaxOvercharge => 3;

    /// <summary>Chance (0.0-1.0) for elemental proc on normal hit.</summary>
    public virtual double ProcChance => 0.0;

    /// <summary>Available elements (Lv1: Fire only, Lv2: +Ice, Lv3+: all three).</summary>
    public virtual bool HasIce => false;
    public virtual bool HasLightning => false;

    /// <summary>Burst cooldown override (Lv4 = 4s, others = 6s).</summary>
    public virtual double BurstCooldown => OverchargeSystem.DefaultBurstCooldown;

    /// <summary>Burst AoE radius override.</summary>
    public virtual int BurstRadius => OverchargeSystem.DefaultBurstRadius;

    protected BaseElementalistWand(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!ElementalistItemHelper.IsElementalist(from, RequiredElementalistLevel))
        {
            from.SendMessage(0x22, "Only an elementalist can wield this wand.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (Parent != from)
        {
            from.SendMessage(0x22, "You must equip the wand first.");
            return;
        }

        var newElement = OverchargeSystem.SwitchElement(from);

        // Only allow switching to elements this weapon supports
        if (newElement == ElementType.Ice && !HasIce)
        {
            OverchargeSystem.SwitchElement(from); // skip Ice
            newElement = OverchargeSystem.GetElement(from);
        }

        if (newElement == ElementType.Lightning && !HasLightning)
        {
            OverchargeSystem.SwitchElement(from); // skip Lightning → back to Fire
            newElement = OverchargeSystem.GetElement(from);
        }

        var (color, name) = GetElementInfo(newElement);
        from.SendMessage(color, $"Element switched to: {name}. Overcharge reset.");
        from.PlaySound(0x1E9);
        from.FixedParticles(0x375A, 10, 15, 5037, color, 0, EffectLayer.Waist);
    }

    public override void OnHit(Mobile attacker, Mobile defender, double damageBonus = 1.0)
    {
        base.OnHit(attacker, defender, damageBonus);

        if (defender == null || !defender.Alive || !attacker.Alive)
        {
            return;
        }

        var element = OverchargeSystem.GetElement(attacker);

        // Normal hit elemental proc
        if (ProcChance > 0.0 && Utility.RandomDouble() < ProcChance)
        {
            ApplyElementalProc(attacker, defender, element);
        }

        // Accumulate Overcharge
        var charges = OverchargeSystem.AddCharge(attacker, MaxOvercharge);
        var (color, name) = GetElementInfo(element);
        attacker.SendMessage(color, $"Overcharge [{name}]: {charges}/{MaxOvercharge}");

        // Check for Burst
        if (OverchargeSystem.IsFull(attacker, MaxOvercharge) && !OverchargeSystem.IsOnCooldown(attacker))
        {
            TriggerBurst(attacker, defender, element);
        }
    }

    private void TriggerBurst(Mobile attacker, Mobile defender, ElementType element)
    {
        // Check for Double Burst (ring Lv3+): chance to not consume charges
        var doubleBurstChance = GetDoubleBurstChance(attacker);
        var keepCharges = doubleBurstChance > 0.0 && Utility.RandomDouble() < doubleBurstChance;

        int charges;
        if (keepCharges)
        {
            charges = OverchargeSystem.GetCharges(attacker);
            // Still apply cooldown
            OverchargeSystem.ConsumeBurst(attacker, BurstCooldown);
            OverchargeSystem.FillCharges(attacker, MaxOvercharge); // restore
            attacker.SendMessage(0x35, "Double Burst! Charges preserved!");
        }
        else
        {
            charges = OverchargeSystem.ConsumeBurst(attacker, BurstCooldown);
        }

        var (color, name) = GetElementInfo(element);
        attacker.PublicOverheadMessage(MessageType.Emote, color, false, $"*Elemental Burst: {name}!*");

        // Get AoE targets
        var map = attacker.Map;
        if (map == null)
        {
            return;
        }

        // Apply burst to primary target
        ApplyBurstEffect(attacker, defender, element, charges);

        // Apply burst AoE to nearby enemies
        foreach (var m in map.GetMobilesInRange<PlayerMobile>(defender.Location, BurstRadius))
        {
            if (m == attacker || m == defender || !m.Alive || !attacker.CanBeHarmful(m))
            {
                continue;
            }

            ApplyBurstEffect(attacker, m, element, charges);
        }
    }

    internal static void ApplyBurstEffectStatic(Mobile attacker, Mobile target, ElementType element, int charges) =>
        ApplyBurstEffect(attacker, target, element, charges);

    private static void ApplyBurstEffect(Mobile attacker, Mobile target, ElementType element, int charges)
    {
        switch (element)
        {
            case ElementType.Fire:
            {
                // Inferno: fire damage + fire DoT
                var dmg = charges * Utility.RandomMinMax(
                    OverchargeSystem.FireBurstDamagePerChargeMin,
                    OverchargeSystem.FireBurstDamagePerChargeMax);
                AOS.Damage(target, attacker, dmg, 0, 100, 0, 0, 0);
                target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                target.PlaySound(0x208);

                // Apply fire DoT (use BeginAction to prevent stacking)
                if (target.CanBeginAction<InfernoDot>())
                {
                    target.BeginAction<InfernoDot>();
                    var dotState = new InfernoDotState(target, attacker, 4);
                    Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), 4, InfernoDotTick, dotState);
                }

                break;
            }
            case ElementType.Ice:
            {
                // Blizzard: cold damage + paralyze + slow
                var dmg = charges * Utility.RandomMinMax(
                    OverchargeSystem.IceBurstDamagePerChargeMin,
                    OverchargeSystem.IceBurstDamagePerChargeMax);
                AOS.Damage(target, attacker, dmg, 0, 0, 100, 0, 0);
                target.FixedParticles(0x376A, 9, 32, 5005, 0x480, 0, EffectLayer.Waist);
                target.PlaySound(0x10B);

                if (!target.Frozen && !target.Paralyzed)
                {
                    target.Paralyze(TimeSpan.FromSeconds(OverchargeSystem.IceParalyzeDuration));
                }

                break;
            }
            case ElementType.Lightning:
            {
                // Thunderstorm: energy damage + mana drain + stamina drain
                var dmg = charges * Utility.RandomMinMax(
                    OverchargeSystem.LightningBurstDamagePerChargeMin,
                    OverchargeSystem.LightningBurstDamagePerChargeMax);
                AOS.Damage(target, attacker, dmg, 0, 0, 0, 0, 100);

                var manaDrain = Utility.RandomMinMax(
                    OverchargeSystem.LightningManaDrainMin,
                    OverchargeSystem.LightningManaDrainMax);
                var stamDrain = Utility.RandomMinMax(
                    OverchargeSystem.LightningStamDrainMin,
                    OverchargeSystem.LightningStamDrainMax);

                target.Mana = Math.Max(0, target.Mana - manaDrain);
                target.Stam = Math.Max(0, target.Stam - stamDrain);

                target.FixedParticles(0x3818, 1, 11, 0x13A8, 0, 0, EffectLayer.Head);
                target.PlaySound(0x29);
                break;
            }
        }
    }

    internal static void ApplyElementalProcStatic(Mobile attacker, Mobile defender, ElementType element) =>
        ApplyElementalProc(attacker, defender, element);

    private static void ApplyElementalProc(Mobile attacker, Mobile defender, ElementType element)
    {
        switch (element)
        {
            case ElementType.Fire:
            {
                var dmg = Utility.RandomMinMax(4, 8);
                AOS.Damage(defender, attacker, dmg, 0, 100, 0, 0, 0);
                defender.FixedParticles(0x3709, 10, 15, 5052, EffectLayer.LeftFoot);
                break;
            }
            case ElementType.Ice:
            {
                // Slow effect: reduce stam
                defender.Stam = Math.Max(0, defender.Stam - 10);
                defender.FixedParticles(0x376A, 9, 20, 5005, 0x480, 0, EffectLayer.Waist);
                defender.SendMessage(0x480, "You feel sluggish from the cold!");
                break;
            }
            case ElementType.Lightning:
            {
                var dmg = Utility.RandomMinMax(3, 6);
                AOS.Damage(defender, attacker, dmg, 0, 0, 0, 0, 100);
                defender.Mana = Math.Max(0, defender.Mana - Utility.RandomMinMax(5, 12));
                defender.FixedParticles(0x3818, 1, 11, 0x13A8, 0, 0, EffectLayer.Head);
                break;
            }
        }
    }

    private static double GetDoubleBurstChance(Mobile m)
    {
        if (m is not PlayerMobile pm)
        {
            return 0.0;
        }

        var ring = pm.FindItemOnLayer<Jewelry.ElementalistRing>(Layer.Ring);
        if (ring == null)
        {
            return 0.0;
        }

        return ring.DoubleBurstChance;
    }

    private static (int hue, string name) GetElementInfo(ElementType element) => element switch
    {
        ElementType.Fire => (0x489, "Fire"),
        ElementType.Ice => (0x480, "Ice"),
        ElementType.Lightning => (0x490, "Lightning"),
        _ => (0x480, "Unknown")
    };

    // -- Inferno DoT --

    private class InfernoDotState
    {
        public readonly Mobile Target;
        public readonly Mobile Attacker;
        public int TicksRemaining;

        public InfernoDotState(Mobile target, Mobile attacker, int ticks)
        {
            Target = target;
            Attacker = attacker;
            TicksRemaining = ticks;
        }
    }

    private static void InfernoDotTick(InfernoDotState state)
    {
        state.TicksRemaining--;

        if (!state.Target.Alive || state.TicksRemaining < 0)
        {
            state.Target.EndAction<InfernoDot>();
            return;
        }

        state.Target.Damage(OverchargeSystem.FireDotDamagePerTick, state.Attacker);
        state.Target.FixedParticles(0x3709, 1, 10, 5052, EffectLayer.LeftFoot);

        if (state.TicksRemaining <= 0)
        {
            state.Target.EndAction<InfernoDot>();
        }
    }

    private class InfernoDot;

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        list.Add(1042971, $"{"Overcharge"}\t{MaxOvercharge} {"max charges"}");

        if (ProcChance > 0.0)
        {
            list.Add(1042971, $"{"Elemental Proc"}\t{(int)(ProcChance * 100)}%");
        }

        var elements = "Fire";
        if (HasIce)
        {
            elements += ", Ice";
        }

        if (HasLightning)
        {
            elements += ", Lightning";
        }

        list.Add(1042971, $"{"Elements"}\t{elements}");

        if (BurstCooldown < OverchargeSystem.DefaultBurstCooldown)
        {
            list.Add(1042971, $"{"Burst Cooldown"}\t{BurstCooldown}{"s"}");
        }
    }
}
