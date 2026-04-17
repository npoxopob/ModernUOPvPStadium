using System;
using System.Collections.Generic;
using Server;

namespace PvPStadium.Items.Elementalist;

/// <summary>
/// Overcharge system for the Elementalist race.
/// Each weapon hit accumulates a charge of the current element.
/// When max charges reached, next attack triggers an Elemental Burst (powerful AoE).
/// Switching element resets charges. Burst has a cooldown.
/// </summary>
public static class OverchargeSystem
{
    private static readonly Dictionary<Serial, OverchargeState> _states = new();

    /// <summary>Burst cooldown in seconds (default, Lv4 reduces to 4s).</summary>
    public const double DefaultBurstCooldown = 6.0;

    /// <summary>Burst AoE radius in tiles (default 2, Lv4 mantle increases to 3).</summary>
    public const int DefaultBurstRadius = 2;

    // -- Fire Burst (Inferno) --
    public const int FireBurstDamagePerChargeMin = 8;
    public const int FireBurstDamagePerChargeMax = 15;
    public const double FireDotDuration = 4.0;
    public const int FireDotDamagePerTick = 5;

    // -- Ice Burst (Blizzard) --
    public const int IceBurstDamagePerChargeMin = 5;
    public const int IceBurstDamagePerChargeMax = 10;
    public const double IceParalyzeDuration = 2.5;
    public const double IceSlowDuration = 5.0;

    // -- Lightning Burst (Thunderstorm) --
    public const int LightningBurstDamagePerChargeMin = 6;
    public const int LightningBurstDamagePerChargeMax = 12;
    public const int LightningManaDrainMin = 30;
    public const int LightningManaDrainMax = 60;
    public const int LightningStamDrainMin = 20;
    public const int LightningStamDrainMax = 40;

    public static OverchargeState GetState(Mobile m)
    {
        if (!_states.TryGetValue(m.Serial, out var st))
        {
            st = new OverchargeState();
            _states[m.Serial] = st;
        }

        return st;
    }

    public static int GetCharges(Mobile m)
    {
        if (!_states.TryGetValue(m.Serial, out var st))
        {
            return 0;
        }

        return st.Charges;
    }

    public static ElementType GetElement(Mobile m)
    {
        return GetState(m).CurrentElement;
    }

    /// <summary>Add one Overcharge. Returns new charge count.</summary>
    public static int AddCharge(Mobile m, int maxCharges)
    {
        var st = GetState(m);
        if (st.Charges < maxCharges)
        {
            st.Charges++;
        }

        return st.Charges;
    }

    /// <summary>Check if Overcharge is full.</summary>
    public static bool IsFull(Mobile m, int maxCharges)
    {
        return GetCharges(m) >= maxCharges;
    }

    /// <summary>Check if Burst is on cooldown.</summary>
    public static bool IsOnCooldown(Mobile m)
    {
        if (!_states.TryGetValue(m.Serial, out var st))
        {
            return false;
        }

        return DateTime.UtcNow < st.BurstCooldownUntil;
    }

    /// <summary>Consume all charges and start Burst cooldown. Returns charges consumed.</summary>
    public static int ConsumeBurst(Mobile m, double cooldownSeconds)
    {
        var st = GetState(m);
        var charges = st.Charges;
        st.Charges = 0;
        st.BurstCooldownUntil = DateTime.UtcNow.AddSeconds(cooldownSeconds);
        return charges;
    }

    /// <summary>Switch element, resetting charges.</summary>
    public static ElementType SwitchElement(Mobile m)
    {
        var st = GetState(m);
        st.CurrentElement = st.CurrentElement switch
        {
            ElementType.Fire => ElementType.Ice,
            ElementType.Ice => ElementType.Lightning,
            _ => ElementType.Fire
        };
        st.Charges = 0;
        return st.CurrentElement;
    }

    /// <summary>Set charges to max instantly (for Elemental Scroll).</summary>
    public static void FillCharges(Mobile m, int maxCharges)
    {
        var st = GetState(m);
        st.Charges = maxCharges;
    }

    /// <summary>Clear state (logout/death).</summary>
    public static void Clear(Mobile m)
    {
        _states.Remove(m.Serial);
    }

    public class OverchargeState
    {
        public ElementType CurrentElement = ElementType.Fire;
        public int Charges;
        public DateTime BurstCooldownUntil = DateTime.MinValue;
    }
}
