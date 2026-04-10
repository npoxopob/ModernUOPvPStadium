using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Vampire.Weapons;

/// <summary>
/// Base class for vampire ranged weapons (crossbow-type "Hands").
/// Fire visual on hit, optional ignition DoT.
/// </summary>
[SerializationGenerator(0, false)]
public abstract partial class BaseVampireHand : BaseRanged
{
    /// <summary>Minimum vampire level required to equip.</summary>
    public virtual int RequiredVampireLevel => 2;

    /// <summary>If true, has a chance to apply ignition DoT.</summary>
    public virtual bool HasIgnition => false;

    /// <summary>Chance to ignite target (0.0–1.0).</summary>
    public virtual double IgnitionChance => 0.25;

    /// <summary>Min ignition ticks.</summary>
    public virtual int IgnitionMinTicks => 2;

    /// <summary>Max ignition ticks.</summary>
    public virtual int IgnitionMaxTicks => 3;

    /// <summary>Min fire damage per ignition tick.</summary>
    public virtual int IgnitionMinDamage => 13;

    /// <summary>Max fire damage per ignition tick.</summary>
    public virtual int IgnitionMaxDamage => 20;

    // Heavy crossbow visuals
    public override int EffectID => 0x1BFE;
    public override Type AmmoType => typeof(Bolt);
    public override Item Ammo => new Bolt();

    protected BaseVampireHand(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;
        Layer = Layer.TwoHanded;
        Resource = CraftResource.RegularWood;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!VampireItemHelper.IsVampire(from, RequiredVampireLevel))
        {
            from.SendMessage(0x22, "Only a vampire can wield this weapon.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void OnHit(Mobile attacker, Mobile defender, double damageBonus = 1.0)
    {
        base.OnHit(attacker, defender, damageBonus);

        if (defender == null || !defender.Alive)
            return;

        // Fire visual on every hit
        defender.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
        defender.PlaySound(0x208);

        // Ignition DoT
        if (HasIgnition && Utility.RandomDouble() < IgnitionChance && !VampireIgnition.IsBurning(defender))
        {
            var ticks = Utility.RandomMinMax(IgnitionMinTicks, IgnitionMaxTicks);
            VampireIgnition.Begin(defender, attacker, ticks, IgnitionMinDamage, IgnitionMaxDamage);
            attacker.PublicOverheadMessage(
                MessageType.Emote, 0x22, false, "*Let the hell fire burn you!*"
            );
        }
    }
}

/// <summary>
/// Vampire ignition (fire) DoT effect. Ticks fire damage over time.
/// </summary>
public static class VampireIgnition
{
    private static readonly System.Collections.Generic.Dictionary<Mobile, IgnitionTimer> _active = new();

    public static bool IsBurning(Mobile m) => _active.ContainsKey(m);

    public static void Begin(Mobile target, Mobile attacker, int ticks, int minDmg, int maxDmg)
    {
        if (_active.TryGetValue(target, out var existing))
            existing.Stop();

        var timer = new IgnitionTimer(target, attacker, ticks, minDmg, maxDmg);
        _active[target] = timer;
        timer.Start();
    }

    public static void End(Mobile target)
    {
        if (_active.Remove(target, out var t))
            t.Stop();
    }

    private class IgnitionTimer : Timer
    {
        private readonly Mobile _target;
        private readonly Mobile _attacker;
        private readonly int _minDmg;
        private readonly int _maxDmg;
        private int _remaining;

        public IgnitionTimer(Mobile target, Mobile attacker, int ticks, int minDmg, int maxDmg)
            : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
        {
            _target = target;
            _attacker = attacker;
            _remaining = ticks;
            _minDmg = minDmg;
            _maxDmg = maxDmg;
        }

        protected override void OnTick()
        {
            if (!_target.Alive || _remaining <= 0)
            {
                End(_target);
                return;
            }

            var dmg = Utility.RandomMinMax(_minDmg, _maxDmg);
            // Pure fire damage
            AOS.Damage(_target, _attacker, dmg, 0, 100, 0, 0, 0);
            _target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
            _target.PlaySound(0x208);

            _remaining--;
            if (_remaining <= 0)
                End(_target);
        }
    }
}
