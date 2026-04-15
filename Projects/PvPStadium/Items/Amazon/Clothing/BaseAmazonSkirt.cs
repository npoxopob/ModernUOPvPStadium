using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Amazon.Clothing;

/// <summary>
/// Base class for amazon skirts (BaseOuterLegs).
/// Features: DEX bonus via AosAttributes, Clumsy resist/reflect, optional Weaken reflect.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseAmazonSkirt : BaseOuterLegs
{
    public virtual int RequiredAmazonLevel => 1;
    public virtual int DexBonus => 10;

    /// <summary>If true, resists Clumsy spell.</summary>
    public virtual bool ClumsyResist => false;

    /// <summary>If true, reflects Clumsy spell back to caster.</summary>
    public virtual bool ClumsyReflect => false;

    /// <summary>If true, reflects Weaken spell back to caster.</summary>
    public virtual bool WeakenReflect => false;

    protected BaseAmazonSkirt(int hue) : base(0x1516, hue) // female armor skirt graphic
    {
        LootType = LootType.Blessed;
        Weight = 3.0;

        // Use built-in AosAttributes so BaseClothing handles add/remove correctly
        Attributes.BonusDex = DexBonus;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!AmazonItemHelper.IsAmazon(from, RequiredAmazonLevel))
        {
            from.SendMessage(0x22, "Only an Amazon can wear this skirt.");
            return false;
        }

        return base.CanEquip(from);
    }

    /// <summary>Checks if wearing amazon skirt with Clumsy resist.</summary>
    public static bool HasClumsyResist(Mobile m)
    {
        var skirt = m.FindItemOnLayer<BaseAmazonSkirt>(Layer.OuterLegs);
        return skirt != null && (skirt.ClumsyResist || skirt.ClumsyReflect);
    }

    /// <summary>Checks if wearing amazon skirt with Clumsy reflect. Returns true if blocked.</summary>
    public static bool TryClumsyReflect(Mobile target, Mobile caster)
    {
        var skirt = target.FindItemOnLayer<BaseAmazonSkirt>(Layer.OuterLegs);
        if (skirt == null)
        {
            return false;
        }

        if (skirt.ClumsyReflect && caster != null && caster != target)
        {
            target.SendMessage(0x3B2, "Your skirt reflects the Clumsy spell!");
            target.FixedParticles(0x375A, 10, 15, 5037, 0x3B2, 0, EffectLayer.Waist);
            return true;
        }

        if (skirt.ClumsyResist)
        {
            target.SendMessage(0x3B2, "Your skirt protects you from the Clumsy spell!");
            target.FixedParticles(0x375A, 10, 15, 5037, 0x3B2, 0, EffectLayer.Waist);
            return true;
        }

        return false;
    }

    /// <summary>Checks if wearing amazon skirt with Weaken reflect.</summary>
    public static bool TryWeakenReflect(Mobile target, Mobile caster)
    {
        var skirt = target.FindItemOnLayer<BaseAmazonSkirt>(Layer.OuterLegs);
        if (skirt == null || !skirt.WeakenReflect)
        {
            return false;
        }

        if (caster != null && caster != target)
        {
            target.SendMessage(0x3B2, "Your skirt reflects the Weaken spell!");
            target.FixedParticles(0x375A, 10, 15, 5037, 0x3B2, 0, EffectLayer.Waist);
            return true;
        }

        return false;
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (ClumsyReflect)
        {
            list.Add(1042971, $"{"Reflects Clumsy"}");
        }
        else if (ClumsyResist)
        {
            list.Add(1042971, $"{"Resists Clumsy"}");
        }

        if (WeakenReflect)
        {
            list.Add(1042971, $"{"Reflects Weaken"}");
        }
    }
}
