using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Paladin.Weapons;

/// <summary>
/// Base class for paladin sword weapons (swordsmanship).
/// Features: holy damage proc vs all targets, bonus damage vs chaos classes,
/// heal on non-chaos hit, and optional blessing system via Holy Essence.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BasePaladinSword : BaseSword
{
    // -- Config per subclass --

    /// <summary>Minimum paladin level required to equip.</summary>
    public virtual int RequiredPaladinLevel => 1;

    /// <summary>If true, weapon has holy damage proc on hit.</summary>
    public virtual bool HasHolyProc => false;

    /// <summary>Chance (0.0-1.0) to trigger holy damage proc.</summary>
    public virtual double HolyProcChance => 0.5;

    /// <summary>Min holy (fire) damage on proc.</summary>
    public virtual int HolyMinDamage => 0;

    /// <summary>Max holy (fire) damage on proc.</summary>
    public virtual int HolyMaxDamage => 0;

    /// <summary>If true, deals bonus damage vs chaos classes.</summary>
    public virtual bool HasChaosBonus => false;

    /// <summary>Extra flat damage vs chaos targets (necromancer/vampire).</summary>
    public virtual int ChaosBonusDamage => 0;

    /// <summary>Fraction of damage healed when hitting non-chaos targets (0.0-1.0).</summary>
    public virtual double HealFraction => 0.0;

    // -- Blessing state (set by Holy Essence) --

    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _blessingCharges;

    protected BasePaladinSword(int itemID) : base(itemID)
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
            return;

        var isChaos = PaladinItemHelper.IsChaosClass(defender);

        // Bonus damage vs chaos classes
        if (HasChaosBonus && isChaos && ChaosBonusDamage > 0)
        {
            AOS.Damage(defender, attacker, ChaosBonusDamage, 0, 100, 0, 0, 0);
            defender.FixedParticles(0x375A, 1, 15, 5054, 0x480, 0, EffectLayer.Head);
        }

        // Heal wielder when hitting non-chaos targets
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

        // Holy damage proc
        if (HasHolyProc && HolyMaxDamage > 0 && Utility.RandomDouble() < HolyProcChance)
        {
            var holyDmg = Utility.RandomMinMax(HolyMinDamage, HolyMaxDamage);

            // Blessed weapon deals extra vs chaos
            if (_blessingCharges > 0 && isChaos)
            {
                holyDmg = (int)(holyDmg * 1.5);
                _blessingCharges--;

                if (_blessingCharges <= 0)
                {
                    attacker.SendMessage(0x44, "The holy blessing on your weapon has faded.");
                }
            }

            AOS.Damage(defender, attacker, holyDmg, 0, 100, 0, 0, 0); // 100% fire = holy
            defender.FixedParticles(0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot);
            defender.PlaySound(0x208);
            attacker.PublicOverheadMessage(MessageType.Emote, 0x480, false, "*Holy Strike!*");
        }
    }

    /// <summary>
    /// Called by Holy Essence to bless this weapon.
    /// </summary>
    public void ApplyBlessing(int charges)
    {
        _blessingCharges += charges;
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (HasHolyProc)
        {
            list.Add(1042971, $"{"Holy Damage"}\t{HolyMinDamage}-{HolyMaxDamage} ({(int)(HolyProcChance * 100)}%)");
        }

        if (HasChaosBonus)
        {
            list.Add(1042971, $"{"Bonus vs Chaos"}\t+{ChaosBonusDamage}");
        }

        if (HealFraction > 0.0)
        {
            list.Add(1042971, $"{"Heals"}\t{(int)(HealFraction * 100)}% {"on hit (non-chaos)"}");
        }

        if (_blessingCharges > 0)
        {
            list.Add(1042971, $"{"Blessed"}\t{_blessingCharges} {"charges"}");
        }
    }
}
