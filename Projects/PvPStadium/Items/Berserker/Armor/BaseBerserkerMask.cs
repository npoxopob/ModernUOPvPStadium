using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Berserker.Armor;

/// <summary>
/// Base class for berserker masks (bear mask headgear).
/// Features: periodic HP regeneration every 3 seconds (if INT >= 70).
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseBerserkerMask : BaseHat
{
    /// <summary>Minimum berserker level required to equip.</summary>
    public virtual int RequiredBerserkerLevel => 1;

    /// <summary>Min HP healed per tick.</summary>
    public virtual int RegenMin => 3;

    /// <summary>Max HP healed per tick.</summary>
    public virtual int RegenMax => 6;

    /// <summary>Interval in seconds between regen ticks.</summary>
    public virtual double RegenInterval => 3.0;

    /// <summary>Minimum INT required for regen to work.</summary>
    public virtual int MinInt => 70;

    private TimerExecutionToken _regenTimerToken;
    private Mobile? _regenTarget;

    protected BaseBerserkerMask(int itemID, int hue) : base(itemID, hue)
    {
        LootType = LootType.Blessed;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!BerserkerItemHelper.IsBerserker(from, RequiredBerserkerLevel))
        {
            from.SendMessage(0x22, "Only a berserker can wear this mask.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void OnAdded(IEntity parent)
    {
        base.OnAdded(parent);

        if (parent is Mobile m)
        {
            StartRegenTimer(m);
        }
    }

    public override void OnRemoved(IEntity parent)
    {
        base.OnRemoved(parent);
        StopRegenTimer();
    }

    public override void OnDelete()
    {
        StopRegenTimer();
        base.OnDelete();
    }

    private void StartRegenTimer(Mobile wearer)
    {
        StopRegenTimer();
        _regenTarget = wearer;
        Timer.StartTimer(TimeSpan.FromSeconds(RegenInterval), TimeSpan.FromSeconds(RegenInterval),
            RegenTickCallback, out _regenTimerToken);
    }

    private void StopRegenTimer()
    {
        _regenTimerToken.Cancel();
        _regenTarget = null;
    }

    private void RegenTickCallback()
    {
        if (_regenTarget != null)
        {
            OnRegenTick(_regenTarget);
        }
    }

    private void OnRegenTick(Mobile wearer)
    {
        if (Deleted || wearer.Deleted || !wearer.Alive || wearer.Map == Map.Internal)
        {
            StopRegenTimer();
            return;
        }

        // Check mask is still equipped
        if (wearer.FindItemOnLayer(Layer) != this)
        {
            StopRegenTimer();
            return;
        }

        // Require minimum INT
        if (wearer.Int < MinInt)
        {
            return;
        }

        var missing = wearer.HitsMax - wearer.Hits;
        if (missing <= 0)
        {
            return;
        }

        var heal = Utility.RandomMinMax(RegenMin, RegenMax);
        heal = Math.Min(heal, missing);
        wearer.Hits += heal;
        wearer.FixedParticles(0x376A, 9, 32, 5005, 0x26, 0, EffectLayer.Waist);
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);
        list.Add(1042971, $"{"HP Regen"}\t{RegenMin}-{RegenMax} {"every"} {RegenInterval}{"s"}");
        list.Add(1042971, $"{"Requires"}\t{MinInt} {"INT"}");
    }
}
