using System;
using ModernUO.Serialization;
using Server;
using Server.Engines.Harvest;
using Server.Items;
using Server.Mobiles;
using PvPStadium.Mechanics;

namespace PvPStadium.Items.Berserker.Weapons;

/// <summary>
/// Base class for berserker axes (two-handed, swordsmanship).
/// Features: fury bonus damage on hit (consumed from FurySystem).
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseBerserkerAxe : BaseAxe
{
    /// <summary>Minimum berserker level required to equip.</summary>
    public virtual int RequiredBerserkerLevel => 1;

    protected BaseBerserkerAxe(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;
    }

    public override HarvestSystem HarvestSystem => null; // Not a lumber tool

    public override bool CanEquip(Mobile from)
    {
        if (!BerserkerItemHelper.IsBerserker(from, RequiredBerserkerLevel))
        {
            from.SendMessage(0x22, "Only a berserker can wield this weapon.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void OnHit(Mobile attacker, Mobile defender, double damageBonus = 1.0)
    {
        base.OnHit(attacker, defender, damageBonus);

        if (defender == null || !defender.Alive || !attacker.Alive)
            return;

        // Fury bonus damage
        var furyBonus = FurySystem.ConsumeFuryForDamage(attacker);
        if (furyBonus > 0)
        {
            AOS.Damage(defender, attacker, furyBonus, 100, 0, 0, 0, 0); // physical
            defender.FixedParticles(0x37B9, 1, 5, 9948, 0x26, 0, EffectLayer.Head);
        }
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);
        list.Add(1042971, $"{"Berserker Weapon"}");

        if (RequiredBerserkerLevel >= 2)
        {
            list.Add(1042971, $"{"Fury"}\t{"bonus damage from accumulated rage"}");
        }
    }
}
