using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;
using PvPStadium.Items.Human.Crystals;

namespace PvPStadium.Items.Human.Weapons;

/// <summary>
/// Base class for human swords (swordsmanship).
/// Features: crystal socket system — up to MaxSockets crystals can be inserted.
/// Each crystal fires its effect on hit.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseHumanSword : BaseSword
{
    public virtual int RequiredHumanLevel => 1;

    /// <summary>Number of crystal sockets available.</summary>
    public virtual int MaxSockets => 1;

    [SerializableField(0)]
    private CrystalType _socket1;

    [SerializableField(1)]
    private int _power1;

    [SerializableField(2)]
    private CrystalType _socket2;

    [SerializableField(3)]
    private int _power2;

    [SerializableField(4)]
    private CrystalType _socket3;

    [SerializableField(5)]
    private int _power3;

    protected BaseHumanSword(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;
    }

    public int UsedSockets
    {
        get
        {
            var count = 0;
            if (_socket1 != CrystalType.None) count++;
            if (_socket2 != CrystalType.None) count++;
            if (_socket3 != CrystalType.None) count++;
            return count;
        }
    }

    /// <summary>Attempts to socket a crystal. Returns false if no free slots.</summary>
    public bool TrySocketCrystal(CrystalType type, int power)
    {
        if (type == CrystalType.None)
            return false;

        if (_socket1 == CrystalType.None && MaxSockets >= 1)
        {
            _socket1 = type;
            _power1 = power;
            return true;
        }

        if (_socket2 == CrystalType.None && MaxSockets >= 2)
        {
            _socket2 = type;
            _power2 = power;
            return true;
        }

        if (_socket3 == CrystalType.None && MaxSockets >= 3)
        {
            _socket3 = type;
            _power3 = power;
            return true;
        }

        return false;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!HumanItemHelper.IsHuman(from, RequiredHumanLevel))
        {
            from.SendMessage(0x22, "Only a human can wield this weapon.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void OnHit(Mobile attacker, Mobile defender, double damageBonus = 1.0)
    {
        base.OnHit(attacker, defender, damageBonus);

        if (defender == null || !defender.Alive || !attacker.Alive)
            return;

        // Fire all socketed crystals
        if (_socket1 != CrystalType.None)
            ApplyCrystalEffect(attacker, defender, _socket1, _power1);
        if (_socket2 != CrystalType.None)
            ApplyCrystalEffect(attacker, defender, _socket2, _power2);
        if (_socket3 != CrystalType.None)
            ApplyCrystalEffect(attacker, defender, _socket3, _power3);
    }

    private static void ApplyCrystalEffect(Mobile attacker, Mobile defender, CrystalType type, int power)
    {
        switch (type)
        {
            case CrystalType.Fire:
            {
                // 5/8/12 fire damage
                var dmg = power switch { 1 => 5, 2 => 8, _ => 12 };
                AOS.Damage(defender, attacker, dmg, 0, 100, 0, 0, 0);
                defender.FixedParticles(0x3709, 10, 15, 5052, EffectLayer.LeftFoot);
                defender.PlaySound(0x208);
                break;
            }
            case CrystalType.Ice:
            {
                // 4/6/10 cold damage + 5/10/15 stamina drain
                var dmg = power switch { 1 => 4, 2 => 6, _ => 10 };
                var stam = power switch { 1 => 5, 2 => 10, _ => 15 };
                AOS.Damage(defender, attacker, dmg, 0, 0, 100, 0, 0);
                defender.Stam = Math.Max(0, defender.Stam - stam);
                defender.FixedParticles(0x376A, 9, 32, 5005, 0x480, 0, EffectLayer.Waist);
                defender.PlaySound(0x10B);
                break;
            }
            case CrystalType.Poison:
            {
                // 33/40/50% chance, level 1/2/3
                var chance = power switch { 1 => 0.33, 2 => 0.40, _ => 0.50 };
                var level = power switch { 1 => 1, 2 => 2, _ => 3 };
                if (Utility.RandomDouble() < chance)
                {
                    defender.ApplyPoison(attacker, Poison.GetPoison(level));
                }
                break;
            }
            case CrystalType.Thunder:
            {
                // 4/7/11 energy damage + 8/15/25 mana drain
                var dmg = power switch { 1 => 4, 2 => 7, _ => 11 };
                var mana = power switch { 1 => 8, 2 => 15, _ => 25 };
                AOS.Damage(defender, attacker, dmg, 0, 0, 0, 0, 100);
                defender.Mana = Math.Max(0, defender.Mana - mana);
                defender.FixedParticles(0x3818, 1, 11, 0x13A8, 0, 0, EffectLayer.Head);
                defender.PlaySound(0x29);
                break;
            }
            case CrystalType.Life:
            {
                // 4/7/10 lifesteal
                var drain = power switch { 1 => 4, 2 => 7, _ => 10 };
                var actual = Math.Min(drain, defender.Hits);
                defender.Damage(actual, attacker);
                attacker.Hits = Math.Min(attacker.HitsMax, attacker.Hits + actual);
                attacker.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                attacker.PlaySound(0x44B);
                break;
            }
        }
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        list.Add(1042971, $"{"Sockets"}\t{UsedSockets}/{MaxSockets}");

        if (_socket1 != CrystalType.None)
            list.Add(1042971, $"{"[1]"}\t{_socket1} ({"power"} {_power1})");
        if (_socket2 != CrystalType.None)
            list.Add(1042971, $"{"[2]"}\t{_socket2} ({"power"} {_power2})");
        if (_socket3 != CrystalType.None)
            list.Add(1042971, $"{"[3]"}\t{_socket3} ({"power"} {_power3})");
    }
}
