using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Vampire.Clothing;

/// <summary>
/// Base class for vampire shrouds (robes). Provides:
/// - Stat bonuses (STR, Parrying skill)
/// - Poison protection (resist or reflect)
/// - Helm hiding visual flag
/// All shrouds are Blessed (kept on death) and indestructible.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseVampireShroud : BaseOuterTorso
{
    /// <summary>Minimum vampire level required to equip.</summary>
    public virtual int RequiredVampireLevel => 1;

    /// <summary>STR bonus while equipped.</summary>
    public virtual int StrBonus => 0;

    /// <summary>Parrying skill bonus while equipped.</summary>
    public virtual double ParryBonus => 0.0;

    /// <summary>If true, incoming poison spells are blocked (resist).</summary>
    public virtual bool PoisonResist => false;

    /// <summary>If true, incoming poison is reflected back to attacker.</summary>
    public virtual bool PoisonReflect => false;

    protected BaseVampireShroud(int hue) : base(0x1F03, hue)
    {
        Name = "Vampire Shroud";
        LootType = LootType.Blessed;
        Weight = 3.0;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!VampireItemHelper.IsVampire(from, RequiredVampireLevel))
        {
            from.SendMessage(0x22, "Only a vampire can wear this shroud.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void OnAdded(IEntity parent)
    {
        base.OnAdded(parent);

        if (parent is not Mobile mob)
        {
            return;
        }

        var serial = Serial;

        if (StrBonus != 0)
        {
            mob.AddStatMod(new StatMod(StatType.Str, $"{serial}VStr", StrBonus, TimeSpan.Zero));
        }

        if (ParryBonus > 0.0)
        {
            mob.AddSkillMod(new DefaultSkillMod(SkillName.Parry, $"{Serial}VParry", true, ParryBonus));
        }
    }

    public override void OnRemoved(IEntity parent)
    {
        if (parent is Mobile mob)
        {
            var serial = Serial;
            mob.RemoveStatMod($"{serial}VStr");

            if (ParryBonus > 0.0)
            {
                var modName = $"{serial}VParry";
                foreach (var mod in mob.SkillMods)
                {
                    if (mod.Name == modName)
                    {
                        mob.RemoveSkillMod(mod);
                        break;
                    }
                }
            }

            mob.CheckStatTimers();
        }

        base.OnRemoved(parent);
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (StrBonus != 0)
        {
            list.Add(1042971, $"{"Strength Bonus"}\t+{StrBonus}");
        }

        if (ParryBonus > 0.0)
        {
            list.Add(1042971, $"{"Parrying Bonus"}\t+{ParryBonus:F0}");
        }

        if (PoisonReflect)
        {
            list.Add(1042971, $"{"Poison Reflect"}");
        }
        else if (PoisonResist)
        {
            list.Add(1042971, $"{"Poison Resistance"}");
        }
    }
}
