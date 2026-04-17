using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Elementalist.Weapons;

/// <summary>
/// Base class for elementalist orbs (Macing skill, faster attack speed).
/// Same Overcharge/Burst system as wands but with faster speed and lower damage.
/// Lv4 orb: every 4th hit triggers a mini-Burst (50% damage, no cooldown).
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseElementalistOrb : BaseBashing
{
    public virtual int RequiredElementalistLevel => 1;
    public virtual int MaxOvercharge => 3;
    public virtual double ProcChance => 0.0;
    public virtual bool HasIce => false;
    public virtual bool HasLightning => false;
    public virtual double BurstCooldown => OverchargeSystem.DefaultBurstCooldown;
    public virtual int BurstRadius => OverchargeSystem.DefaultBurstRadius;

    /// <summary>If true, every 4th hit triggers a mini-Burst (Lv4 only).</summary>
    public virtual bool HasMiniBurst => false;

    [SerializableField(0)]
    private int _hitCounter;

    protected BaseElementalistOrb(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!ElementalistItemHelper.IsElementalist(from, RequiredElementalistLevel))
        {
            from.SendMessage(0x22, "Only an elementalist can wield this orb.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (Parent != from)
        {
            from.SendMessage(0x22, "You must equip the orb first.");
            return;
        }

        var newElement = OverchargeSystem.SwitchElement(from);

        if (newElement == ElementType.Ice && !HasIce)
        {
            OverchargeSystem.SwitchElement(from);
            newElement = OverchargeSystem.GetElement(from);
        }

        if (newElement == ElementType.Lightning && !HasLightning)
        {
            OverchargeSystem.SwitchElement(from);
            newElement = OverchargeSystem.GetElement(from);
        }

        var (color, name) = GetElementInfo(newElement);
        from.SendMessage(color, $"Element switched to: {name}. Overcharge reset.");
        from.PlaySound(0x1E9);
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

        // Check for full Burst
        if (OverchargeSystem.IsFull(attacker, MaxOvercharge) && !OverchargeSystem.IsOnCooldown(attacker))
        {
            TriggerBurst(attacker, defender, element, false);
        }

        // Mini-Burst on every 4th hit (Lv4 orb)
        if (HasMiniBurst)
        {
            _hitCounter++;
            if (_hitCounter >= 4)
            {
                _hitCounter = 0;
                TriggerBurst(attacker, defender, element, true);
            }
        }
    }

    private void TriggerBurst(Mobile attacker, Mobile defender, ElementType element, bool isMini)
    {
        int charges;
        if (isMini)
        {
            // Mini-Burst: uses half the max charges for damage calc, no cooldown, no consume
            charges = MaxOvercharge / 2;
            attacker.PublicOverheadMessage(MessageType.Emote, 0x490, false, "*Mini-Burst!*");
        }
        else
        {
            // Check Double Burst
            var doubleBurstChance = GetDoubleBurstChance(attacker);
            var keepCharges = doubleBurstChance > 0.0 && Utility.RandomDouble() < doubleBurstChance;

            if (keepCharges)
            {
                charges = OverchargeSystem.GetCharges(attacker);
                OverchargeSystem.ConsumeBurst(attacker, BurstCooldown);
                OverchargeSystem.FillCharges(attacker, MaxOvercharge);
                attacker.SendMessage(0x35, "Double Burst! Charges preserved!");
            }
            else
            {
                charges = OverchargeSystem.ConsumeBurst(attacker, BurstCooldown);
            }

            var (color, name) = GetElementInfo(element);
            attacker.PublicOverheadMessage(MessageType.Emote, color, false, $"*Elemental Burst: {name}!*");
        }

        var map = attacker.Map;
        if (map == null)
        {
            return;
        }

        ApplyBurstEffect(attacker, defender, element, charges);

        foreach (var m in map.GetMobilesInRange<PlayerMobile>(defender.Location, BurstRadius))
        {
            if (m == attacker || m == defender || !m.Alive || !attacker.CanBeHarmful(m))
            {
                continue;
            }

            ApplyBurstEffect(attacker, m, element, charges);
        }
    }

    private static void ApplyBurstEffect(Mobile attacker, Mobile target, ElementType element, int charges)
    {
        // Reuse wand burst logic — identical effects
        BaseElementalistWand.ApplyBurstEffectStatic(attacker, target, element, charges);
    }

    private static void ApplyElementalProc(Mobile attacker, Mobile defender, ElementType element)
    {
        BaseElementalistWand.ApplyElementalProcStatic(attacker, defender, element);
    }

    private static double GetDoubleBurstChance(Mobile m)
    {
        if (m is not PlayerMobile pm)
        {
            return 0.0;
        }

        var ring = pm.FindItemOnLayer<Jewelry.ElementalistRing>(Layer.Ring);
        return ring?.DoubleBurstChance ?? 0.0;
    }

    private static (int hue, string name) GetElementInfo(ElementType element) => element switch
    {
        ElementType.Fire => (0x489, "Fire"),
        ElementType.Ice => (0x480, "Ice"),
        ElementType.Lightning => (0x490, "Lightning"),
        _ => (0x480, "Unknown")
    };

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

        if (HasMiniBurst)
        {
            list.Add(1042971, $"{"Mini-Burst every 4th hit"}");
        }
    }
}
