using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Monk.Weapons;

/// <summary>
/// Base class for monk knuckle weapons (macing, one-handed, fast).
/// Features: Rapid Flurry — series of quick hits (3-4 x 40% damage).
/// Each hit can trigger on-hit effects. Lv4: destroys Human shield ReflectPhysical.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseMonkFists : BaseBashing
{
    public virtual int RequiredMonkLevel => 1;

    /// <summary>Chance to trigger Rapid Flurry (0.0-1.0).</summary>
    public virtual double FlurryChance => 0.0;

    /// <summary>Number of extra hits in a Flurry.</summary>
    public virtual int FlurryHits => 3;

    /// <summary>Damage multiplier for each Flurry hit (0.0-1.0).</summary>
    public virtual double FlurryDamageMod => 0.4;

    /// <summary>If true, Flurry removes ReflectPhysical from Human shields.</summary>
    public virtual bool DestroysShields => false;

    protected BaseMonkFists(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!MonkItemHelper.IsMonk(from, RequiredMonkLevel))
        {
            from.SendMessage(0x22, "Only a monk can wield these.");
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

        // Rapid Flurry proc
        if (FlurryChance > 0.0 && Utility.RandomDouble() < FlurryChance)
        {
            ApplyRapidFlurry(attacker, defender);
        }
    }

    private void ApplyRapidFlurry(Mobile attacker, Mobile defender)
    {
        attacker.PublicOverheadMessage(MessageType.Emote, 0x480, false, "*Rapid Flurry!*");
        attacker.PlaySound(0x525);

        // Destroy Human shield ReflectPhysical at Lv4
        if (DestroysShields)
        {
            TryDestroyShieldReflect(defender);
        }

        // Deal multiple quick hits at reduced damage
        var baseDamage = Utility.RandomMinMax(AosMinDamage, AosMaxDamage);

        for (var i = 0; i < FlurryHits; i++)
        {
            if (!defender.Alive || !attacker.Alive)
            {
                break;
            }

            var hitDamage = Math.Max(1, (int)(baseDamage * FlurryDamageMod));
            defender.Damage(hitDamage, attacker);
            defender.FixedParticles(0x3728, 1, 8, 9916, 0x480, 0, EffectLayer.RightHand);
        }
    }

    private static void TryDestroyShieldReflect(Mobile defender)
    {
        var shield = defender.FindItemOnLayer(Layer.TwoHanded);
        if (shield is BaseShield baseShield && baseShield.Attributes.ReflectPhysical > 0)
        {
            baseShield.Attributes.ReflectPhysical = 0;
            baseShield.InvalidateProperties();
            defender.SendMessage(0x22, "Your shield's reflection is shattered!");
            defender.PlaySound(0x1F8);
            defender.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Waist);
        }
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (FlurryChance > 0.0)
        {
            list.Add(1042971, $"{"Rapid Flurry"}\t{(int)(FlurryChance * 100)}% ({FlurryHits} hits x {(int)(FlurryDamageMod * 100)}%)");
        }

        if (DestroysShields)
        {
            list.Add(1042971, $"{"Shatters shield reflection"}");
        }
    }
}
