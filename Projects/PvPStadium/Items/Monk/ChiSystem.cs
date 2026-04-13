using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace PvPStadium.Items.Monk;

/// <summary>
/// Chi (Ци) accumulation system for the Monk race.
/// Charges build on successful evasion (dodge), consumed on next attack.
/// Each charge = +4 bonus damage. 5+ charges = Stun. 8+ = Knockback.
/// Charges decay after 8 seconds without evasion.
/// </summary>
public static class ChiSystem
{
    private static readonly Dictionary<Serial, ChiState> _states = new();

    /// <summary>Bonus damage per Chi charge.</summary>
    public const int DamagePerCharge = 4;

    /// <summary>Charges needed for stun effect.</summary>
    public const int StunThreshold = 5;

    /// <summary>Charges needed for knockback effect.</summary>
    public const int KnockbackThreshold = 8;

    /// <summary>Stun duration in seconds.</summary>
    public const double StunDuration = 2.0;

    /// <summary>Seconds before Chi charges decay to zero.</summary>
    public const double DecaySeconds = 8.0;

    public static int GetCharges(Mobile m)
    {
        if (!_states.TryGetValue(m.Serial, out var st))
            return 0;

        // Check decay
        if (DateTime.UtcNow - st.LastGainTime > TimeSpan.FromSeconds(DecaySeconds))
        {
            _states.Remove(m.Serial);
            return 0;
        }

        return st.Charges;
    }

    /// <summary>Add a Chi charge from evasion. Returns new charge count.</summary>
    public static int AddCharge(Mobile m, int maxChi)
    {
        if (!_states.TryGetValue(m.Serial, out var st))
        {
            st = new ChiState();
            _states[m.Serial] = st;
        }

        // Check decay first
        if (DateTime.UtcNow - st.LastGainTime > TimeSpan.FromSeconds(DecaySeconds))
        {
            st.Charges = 0;
        }

        st.LastGainTime = DateTime.UtcNow;

        if (st.Charges < maxChi)
        {
            st.Charges++;
        }

        return st.Charges;
    }

    /// <summary>
    /// Consume all Chi charges and return (bonusDamage, shouldStun, shouldKnockback).
    /// </summary>
    public static (int bonusDamage, bool stun, bool knockback) ConsumeCharges(Mobile m)
    {
        var charges = GetCharges(m);
        if (charges <= 0)
            return (0, false, false);

        _states.Remove(m.Serial);

        var damage = charges * DamagePerCharge;
        var stun = charges >= StunThreshold;
        var knockback = charges >= KnockbackThreshold;

        return (damage, stun, knockback);
    }

    /// <summary>Clear all Chi state (on logout/death).</summary>
    public static void Clear(Mobile m) => _states.Remove(m.Serial);

    private class ChiState
    {
        public int Charges;
        public DateTime LastGainTime = DateTime.UtcNow;
    }
}
