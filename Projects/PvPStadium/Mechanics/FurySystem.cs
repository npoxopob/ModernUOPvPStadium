using System;
using System.Collections.Concurrent;
using Server;
using Server.Mobiles;
using PvPStadium.Items.Berserker;

namespace PvPStadium.Mechanics;

/// <summary>
/// Fury system for berserker class.
/// Fury is accumulated when taking damage and spent when attacking.
/// Fury adds bonus damage: +fury/20 per attack hit.
/// At max fury, grants temporary paralyze immunity and resets.
/// </summary>
public static class FurySystem
{
    private static readonly ConcurrentDictionary<int, int> _fury = new();

    /// <summary>Max fury by berserker level. Level 1 has no fury.</summary>
    public static int GetMaxFury(int level) => level switch
    {
        2 => 250,
        3 => 250,
        4 => 300,
        _ => 0
    };

    /// <summary>Get current fury for a mobile.</summary>
    public static int GetFury(Mobile m) =>
        _fury.TryGetValue(m.Serial.ToInt32(), out var f) ? f : 0;

    /// <summary>Set fury for a mobile (clamped to 0..max).</summary>
    public static void SetFury(Mobile m, int value)
    {
        var level = BerserkerItemHelper.GetBerserkerLevel(m);
        var max = GetMaxFury(level);
        if (max <= 0)
        {
            _fury.TryRemove(m.Serial.ToInt32(), out _);
            return;
        }

        value = Math.Clamp(value, 0, max);
        _fury[m.Serial.ToInt32()] = value;
    }

    /// <summary>
    /// Called when a berserker takes damage. Adds damage amount to fury.
    /// Called from DamageHooks.
    /// </summary>
    public static void OnDamageTaken(Mobile target, int damageAmount)
    {
        var level = BerserkerItemHelper.GetBerserkerLevel(target);
        if (level < 2)
            return; // Adept of Might has no fury

        var max = GetMaxFury(level);
        var current = GetFury(target);
        var newFury = current + damageAmount;

        if (newFury >= max)
        {
            // Max fury reached — grant paralyze immunity, reset
            newFury = 0;
            SetFury(target, 0);

            target.PlaySound(0x177);
            target.PublicOverheadMessage(MessageType.Emote, 0x22, false, "*Looks furiously!*");
            target.FixedParticles(0x375A, 10, 15, 5037, 0x26, 0, EffectLayer.Waist);

            // Grant temporary paralyze immunity (10 seconds)
            if (target.CanBeginAction<FuryParalyzeImmunity>())
            {
                target.BeginAction<FuryParalyzeImmunity>();
                Timer.StartTimer(TimeSpan.FromSeconds(10.0), () => target.EndAction<FuryParalyzeImmunity>());
                target.SendMessage(0x3B2, "Your fury grants you paralyze immunity for 10 seconds!");
            }
        }
        else
        {
            SetFury(target, newFury);
        }

        // Update fury display
        SendFuryMessage(target, GetFury(target), max);
    }

    /// <summary>
    /// Calculates bonus damage from fury and consumes it.
    /// Called from BaseBerserkerAxe.OnHit.
    /// Returns bonus damage to add.
    /// </summary>
    public static int ConsumeFuryForDamage(Mobile attacker)
    {
        var level = BerserkerItemHelper.GetBerserkerLevel(attacker);
        if (level < 2)
            return 0;

        var fury = GetFury(attacker);
        if (fury <= 0)
            return 0;

        var bonus = fury / 20;
        if (bonus <= 0)
            return 0;

        // Consume fury equal to bonus dealt
        var consumed = Math.Min(fury, bonus);
        SetFury(attacker, fury - consumed);

        var max = GetMaxFury(level);
        SendFuryMessage(attacker, GetFury(attacker), max);

        return bonus;
    }

    /// <summary>
    /// Checks if target has fury-based paralyze immunity.
    /// Called from ParalyzeHooks.
    /// </summary>
    public static bool HasFuryParalyzeImmunity(Mobile m) =>
        !m.CanBeginAction<FuryParalyzeImmunity>();

    private static void SendFuryMessage(Mobile m, int current, int max)
    {
        if (m is PlayerMobile pm && pm.NetState != null)
        {
            pm.SendMessage(0x26, $"Fury: {current}/{max}");
        }
    }

    // Marker type for BeginAction/EndAction paralyze immunity
    private class FuryParalyzeImmunity;
}
