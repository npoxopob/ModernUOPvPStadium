using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>
/// Base class for amazon bows (archery, uses arrows).
/// Features: distance-based critical damage at optimal range (6 tiles = +15%).
/// Optional on-hit paralyze or poison+weaken procs.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseAmazonBow : BaseRanged
{
    public virtual int RequiredAmazonLevel => 1;

    // Distance critical: at 6 tiles, +15% damage
    public virtual int OptimalRange => 6;
    public virtual double CritBonus => 0.15;

    // On-hit proc
    public virtual bool HasParalyzeProc => false;
    public virtual double ParalyzeChance => 0.33;
    public virtual double ParalyzeDuration => 3.0;

    public virtual bool HasPoisonProc => false;
    public virtual double PoisonChance => 0.50;
    public virtual int PoisonLevel => 2;

    // BaseRanged abstract members
    public override int EffectID => 0xF42; // arrow
    public override Type AmmoType => typeof(Arrow);
    public override Item Ammo => new Arrow();

    protected BaseAmazonBow(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;
        Layer = Layer.TwoHanded;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!AmazonItemHelper.IsAmazon(from, RequiredAmazonLevel))
        {
            from.SendMessage(0x22, "Only an Amazon can wield this weapon.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void OnHit(Mobile attacker, Mobile defender, double damageBonus = 1.0)
    {
        // Distance-based critical
        var dist = (int)attacker.GetDistanceToSqrt(defender);
        if (dist >= OptimalRange - 1 && dist <= OptimalRange + 1)
        {
            damageBonus += CritBonus;
            attacker.SendMessage(0x3B2, "*Critical Range!*");
        }

        base.OnHit(attacker, defender, damageBonus);

        if (defender == null || !defender.Alive || !attacker.Alive)
        {
            return;
        }

        // Paralyze proc
        if (HasParalyzeProc && Utility.RandomDouble() < ParalyzeChance && !defender.Frozen && !defender.Paralyzed)
        {
            defender.Paralyze(TimeSpan.FromSeconds(ParalyzeDuration));
            defender.PlaySound(0x204);
            defender.FixedEffect(0x376A, 6, 1);
        }

        // Poison proc
        if (HasPoisonProc && Utility.RandomDouble() < PoisonChance)
        {
            var poison = Poison.GetPoison(PoisonLevel);
            defender.ApplyPoison(attacker, poison);
        }
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);
        list.Add(1042971, $"{"Critical"}\t{OptimalRange} {"tiles"}\t+{(int)(CritBonus * 100)}%");

        if (HasParalyzeProc)
        {
            list.Add(1042971, $"{"Paralyze"}\t{(int)(ParalyzeChance * 100)}%");
        }

        if (HasPoisonProc)
        {
            list.Add(1042971, $"{"Poison"}\t{(int)(PoisonChance * 100)}%");
        }
    }
}
