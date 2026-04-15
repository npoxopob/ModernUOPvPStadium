using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Necromancer.Weapons;

/// <summary>
/// Base class for necromancer daggers (fencing, one-handed).
/// Features: Soul Drain on hit — steals HP from target and heals attacker.
/// Optional poison proc at higher levels.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseNecroDagger : BaseSword
{
    public virtual int RequiredNecromancerLevel => 1;

    /// <summary>Chance to proc Soul Drain (0.0–1.0).</summary>
    public virtual double SoulDrainChance => 0.0;

    /// <summary>Min HP stolen by Soul Drain.</summary>
    public virtual int SoulDrainMin => 8;

    /// <summary>Max HP stolen by Soul Drain.</summary>
    public virtual int SoulDrainMax => 15;

    /// <summary>If true, applies poison on hit.</summary>
    public virtual bool AppliesPoison => false;

    /// <summary>Poison level (1=Lesser, 2=Regular, 3=Greater, 4=Deadly, 5=Lethal).</summary>
    public virtual int PoisonLevel => 2;

    /// <summary>Chance to apply poison (0.0–1.0).</summary>
    public virtual double PoisonChance => 0.33;

    // -- Empowerment state (set by Dark Essence) --
    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _empowerCharges;

    // Fencing skill (dagger-like)
    public override SkillName DefSkill => SkillName.Fencing;
    public override WeaponType DefType => WeaponType.Piercing;
    public override WeaponAnimation DefAnimation => WeaponAnimation.Pierce1H;

    protected BaseNecroDagger(int itemID) : base(itemID)
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
        base.OnHit(attacker, defender, damageBonus);

        if (defender == null || !defender.Alive || !attacker.Alive)
        {
            return;
        }

        // Soul Drain proc
        if (SoulDrainChance > 0.0 && Utility.RandomDouble() < SoulDrainChance)
        {
            var drain = Utility.RandomMinMax(SoulDrainMin, SoulDrainMax);

            // Empowered: +50% Soul Drain
            if (_empowerCharges > 0)
            {
                drain = (int)(drain * 1.5);
                _empowerCharges--;

                if (_empowerCharges <= 0)
                {
                    attacker.SendMessage(0x44, "The dark empowerment on your dagger has faded.");
                }
            }

            var actualDrain = Math.Min(drain, defender.Hits);

            defender.Damage(actualDrain, attacker);
            attacker.Hits = Math.Min(attacker.HitsMax, attacker.Hits + actualDrain);

            attacker.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            defender.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
            attacker.PlaySound(0x44B);
            attacker.PublicOverheadMessage(MessageType.Emote, 0x455, false, "*Soul Drain!*");
        }

        // Poison proc
        if (AppliesPoison && Utility.RandomDouble() < PoisonChance)
        {
            var poison = Poison.GetPoison(PoisonLevel);
            defender.ApplyPoison(attacker, poison);
        }
    }

    /// <summary>Called by Dark Essence to empower this dagger.</summary>
    public void ApplyEmpowerment(int charges)
    {
        _empowerCharges += charges;
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (SoulDrainChance > 0.0)
        {
            list.Add(1042971, $"{"Soul Drain"}\t{(int)(SoulDrainChance * 100)}% ({SoulDrainMin}-{SoulDrainMax} {"HP"}");
        }

        if (AppliesPoison)
        {
            list.Add(1042971, $"{"Poison"}\t{(int)(PoisonChance * 100)}%");
        }

        if (_empowerCharges > 0)
        {
            list.Add(1042971, $"{"Empowered"}\t{_empowerCharges} {"charges"}");
        }
    }
}
