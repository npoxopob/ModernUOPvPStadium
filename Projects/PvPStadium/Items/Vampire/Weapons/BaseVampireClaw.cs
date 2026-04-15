using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Vampire.Weapons;

/// <summary>
/// Base class for vampire claw weapons (swords).
/// Provides life steal on hit and optional charge → Hell Blaze mechanic.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseVampireClaw : BaseSword
{
    // -- Config per subclass --
    /// <summary>Divisor for life steal: healed = damage / LifeStealDivisor.</summary>
    public virtual int LifeStealDivisor => 4;

    /// <summary>If true, this claw accumulates blood charges for Hell Blaze.</summary>
    public virtual bool HasChargeMechanic => false;

    /// <summary>Blood drained threshold to trigger Hell Blaze.</summary>
    public virtual int ChargeThreshold => 200;

    /// <summary>Min fire damage on Hell Blaze proc.</summary>
    public virtual int BlazeMinDamage => 28;

    /// <summary>Max fire damage on Hell Blaze proc.</summary>
    public virtual int BlazeMaxDamage => 39;

    /// <summary>Color when fully charged.</summary>
    public virtual int ChargedHue => 0x0A4C;

    /// <summary>Minimum vampire level required to equip.</summary>
    public virtual int RequiredVampireLevel => 1;

    // -- Persistent state --
    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _bloodDrained;

    protected BaseVampireClaw(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!VampireItemHelper.IsVampire(from, RequiredVampireLevel))
        {
            from.SendMessage(0x22, "Only a vampire can wield this weapon.");
            return false;
        }

        return base.CanEquip(from);
    }

    public override void OnHit(Mobile attacker, Mobile defender, double damageBonus = 1.0)
    {
        base.OnHit(attacker, defender, damageBonus);

        if (defender == null || !defender.Alive || !attacker.Alive)
            return;

        // Life steal: heal attacker for estimated damage / divisor
        // Use weapon base damage as estimate since Mobile doesn't expose per-hit damage
        GetBaseDamageRange(attacker, out var wMin, out var wMax);
        var estimatedDmg = Math.Max(1, (wMin + wMax) / 2);
        var healed = Math.Max(1, estimatedDmg / LifeStealDivisor);

        // Don't overheal
        var missing = attacker.HitsMax - attacker.Hits;
        if (missing > 0)
        {
            healed = Math.Min(healed, missing);
            attacker.Hits += healed;
            attacker.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
        }

        // Charge mechanic
        if (HasChargeMechanic)
        {
            var drain = Utility.RandomMinMax(40, 80);
            _bloodDrained += drain;

            if (_bloodDrained >= ChargeThreshold)
            {
                // Hell Blaze!
                var blazeDmg = Utility.RandomMinMax(BlazeMinDamage, BlazeMaxDamage);
                // Fire + poison damage type
                AOS.Damage(defender, attacker, blazeDmg, 0, 100, 0, 0, 0);
                defender.FixedParticles(0x3715, 1, 30, 9502, 1160, 0, EffectLayer.Waist);
                defender.PlaySound(0x208);
                attacker.PublicOverheadMessage(MessageType.Emote, 0x22, false, "*Feel the hell blaze!*");

                _bloodDrained = 0;
                Hue = _originalHue;
            }
            else if (_bloodDrained >= ChargeThreshold * 3 / 4 && Hue != ChargedHue)
            {
                // Visual hint: weapon glows when nearly charged
                Hue = ChargedHue;
            }
        }
    }

    // Store original hue for reset after blaze
    private int _originalHue;

    public override void OnAdded(IEntity parent)
    {
        base.OnAdded(parent);
        _originalHue = Hue;
    }
}
