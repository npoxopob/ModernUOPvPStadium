using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Monk.Weapons;

/// <summary>
/// Base class for monk staves (macing, two-handed).
/// Features: Counter Strike — consumes Chi charges for bonus damage.
/// On hit: spends all Chi charges, bonus dmg = charges x multiplier.
/// High Chi counts add Stun and Knockback.
/// Bonus flat damage vs Berserker.
/// </summary>
[SerializationGenerator(0, false)]
public abstract partial class BaseMonkStaff : BaseStaff
{
    public virtual int RequiredMonkLevel => 1;

    /// <summary>Chance to trigger Counter Strike (consumes Chi) (0.0-1.0).</summary>
    public virtual double CounterStrikeChance => 0.0;

    /// <summary>Damage multiplier per Chi charge on Counter Strike.</summary>
    public virtual int ChiDamageMultiplier => 4;

    /// <summary>Extra flat damage vs Berserker targets.</summary>
    public virtual int BerserkerBonusDamage => 0;

    /// <summary>If true, Counter Strike always stuns on proc (even below 5 Chi).</summary>
    public virtual bool CounterStrikeAlwaysStuns => false;

    protected BaseMonkStaff(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!MonkItemHelper.IsMonk(from, RequiredMonkLevel))
        {
            from.SendMessage(0x22, "Only a monk can wield this weapon.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void OnHit(Mobile attacker, Mobile defender, double damageBonus = 1.0)
    {
        base.OnHit(attacker, defender, damageBonus);

        if (defender == null || !defender.Alive || !attacker.Alive)
            return;

        // Bonus damage vs Berserker
        if (BerserkerBonusDamage > 0 && IsBerserker(defender))
        {
            defender.Damage(BerserkerBonusDamage, attacker);
            defender.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
        }

        // Counter Strike — consume Chi for bonus damage
        if (CounterStrikeChance > 0.0 && ChiSystem.GetCharges(attacker) > 0
            && Utility.RandomDouble() < CounterStrikeChance)
        {
            ApplyCounterStrike(attacker, defender);
        }
    }

    private void ApplyCounterStrike(Mobile attacker, Mobile defender)
    {
        var (bonusDamage, stun, knockback) = ChiSystem.ConsumeCharges(attacker);
        if (bonusDamage <= 0)
            return;

        // Apply multiplier (design: x3 Lv2, x4 Lv3, x5 Lv4)
        var scaledDamage = bonusDamage / ChiSystem.DamagePerCharge * ChiDamageMultiplier;

        defender.Damage(scaledDamage, attacker);

        // Visual effects
        defender.FixedParticles(0x37B9, 10, 25, 5013, 0x480, 0, EffectLayer.Head);
        defender.PlaySound(0x2F4);
        attacker.PublicOverheadMessage(MessageType.Emote, 0x480, false, "*Counter Strike!*");

        // Stun
        if (stun || CounterStrikeAlwaysStuns)
        {
            defender.Freeze(TimeSpan.FromSeconds(ChiSystem.StunDuration));
            defender.SendMessage(0x22, "You are stunned by a powerful counter strike!");
        }

        // Knockback (2 tiles away from attacker)
        if (knockback)
        {
            ApplyKnockback(attacker, defender, 2);
            defender.SendMessage(0x22, "The force of the blow knocks you back!");
        }
    }

    private static void ApplyKnockback(Mobile attacker, Mobile defender, int tiles)
    {
        if (defender.Map == null || defender.Map == Map.Internal)
            return;

        var dx = defender.X - attacker.X;
        var dy = defender.Y - attacker.Y;

        // Normalize direction
        if (dx != 0) dx = dx > 0 ? 1 : -1;
        if (dy != 0) dy = dy > 0 ? 1 : -1;

        // If same tile, push north
        if (dx == 0 && dy == 0) dy = -1;

        var newX = defender.X + dx * tiles;
        var newY = defender.Y + dy * tiles;
        var newZ = defender.Map.GetAverageZ(newX, newY);

        if (defender.Map.CanFit(newX, newY, newZ, 16, false, true))
        {
            defender.MoveToWorld(new Point3D(newX, newY, newZ), defender.Map);
            defender.ProcessDelta();
        }
    }

    private static bool IsBerserker(Mobile m)
    {
        if (m is not PlayerMobile)
            return false;

        if (!Races.RaceStateStore.TryGet(m.Serial, out var st) || st == null)
            return false;

        return st.RaceKey == "berserker";
    }

    public override void AddNameProperties(IPropertyList list)
    {
        base.AddNameProperties(list);

        var chi = ChiSystem.GetCharges(RootParent as Mobile ?? Parent as Mobile);
        if (chi > 0)
        {
            list.Add(1042971, $"Chi: {chi}");
        }

        if (CounterStrikeChance > 0.0)
        {
            list.Add(1042971, $"Counter Strike: {(int)(CounterStrikeChance * 100)}% (x{ChiDamageMultiplier} Chi dmg)");
        }

        if (BerserkerBonusDamage > 0)
        {
            list.Add(1042971, $"vs Berserker: +{BerserkerBonusDamage} damage");
        }
    }
}
