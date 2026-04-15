using System;
using System.Collections.Generic;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Vampire.Weapons;

/// <summary>
/// Base class for vampire fencing weapons (sickles/stings).
/// Provides optional bleed-on-hit effect.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseVampireSickle : BaseKnife
{
    /// <summary>Chance to apply bleed (0.0 = never, 0.25 = 25%).</summary>
    public virtual double BleedChance => 0.0;

    /// <summary>Min number of bleed ticks.</summary>
    public virtual int BleedMinTicks => 1;

    /// <summary>Max number of bleed ticks.</summary>
    public virtual int BleedMaxTicks => 2;

    /// <summary>Min damage per bleed tick.</summary>
    public virtual int BleedMinDamage => 13;

    /// <summary>Max damage per bleed tick.</summary>
    public virtual int BleedMaxDamage => 16;

    /// <summary>Minimum vampire level required to equip.</summary>
    public virtual int RequiredVampireLevel => 1;

    // Fencing skill overrides (BaseKnife defaults to Swords)
    public override SkillName DefSkill => SkillName.Fencing;
    public override WeaponType DefType => WeaponType.Piercing;
    public override WeaponAnimation DefAnimation => WeaponAnimation.Pierce1H;

    protected BaseVampireSickle(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;
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

        if (BleedChance <= 0.0 || defender == null || !defender.Alive)
            return;

        if (Utility.RandomDouble() < BleedChance && !VampireBleed.IsBleeding(defender))
        {
            var ticks = Utility.RandomMinMax(BleedMinTicks, BleedMaxTicks);
            VampireBleed.Begin(defender, attacker, ticks, BleedMinDamage, BleedMaxDamage);
            attacker.SendMessage(0x22, "Your target is bleeding!");
            defender.SendMessage(0x22, "You are bleeding!");
        }
    }
}

/// <summary>
/// Vampire-specific bleed DoT. Separate from the built-in BleedAttack weapon ability.
/// </summary>
public static class VampireBleed
{
    private static readonly Dictionary<Mobile, BleedTimer> _active = new();

    public static bool IsBleeding(Mobile m) => _active.ContainsKey(m);

    public static void Begin(Mobile target, Mobile attacker, int ticks, int minDmg, int maxDmg)
    {
        if (_active.TryGetValue(target, out var existing))
            existing.Stop();

        var timer = new BleedTimer(target, attacker, ticks, minDmg, maxDmg);
        _active[target] = timer;
        timer.Start();
    }

    public static void End(Mobile target)
    {
        if (_active.Remove(target, out var t))
            t.Stop();
    }

    private class BleedTimer : Timer
    {
        private readonly Mobile _target;
        private readonly Mobile _attacker;
        private readonly int _minDmg;
        private readonly int _maxDmg;
        private int _remaining;

        public BleedTimer(Mobile target, Mobile attacker, int ticks, int minDmg, int maxDmg)
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
            _target.Damage(dmg, _attacker);
            _target.PlaySound(0x133);
            _target.FixedParticles(0x377A, 244, 25, 9950, 31, 0, EffectLayer.Waist);

            _remaining--;
            if (_remaining <= 0)
                End(_target);
        }
    }
}
