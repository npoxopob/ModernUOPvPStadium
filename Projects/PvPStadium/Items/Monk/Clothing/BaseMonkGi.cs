using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Monk.Clothing;

/// <summary>
/// Base class for monk Gi (BaseOuterTorso).
/// Features: DEX bonus via AosAttributes, Evasion bonus (adds to DCI),
/// Physical resist bonus, and Paralyze immunity at Lv4.
/// On dodge (attacker misses), grants Chi charge via ChiSystem.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseMonkGi : BaseOuterTorso
{
    public virtual int RequiredMonkLevel => 1;
    public virtual int DexBonus => 5;

    /// <summary>Defense Chance Increase (simulates Evasion bonus).</summary>
    public virtual int EvasionBonus => 5;

    /// <summary>Physical resist percentage bonus.</summary>
    public virtual int PhysicalResistBonus => 0;

    /// <summary>If true, wearer is immune to paralyze.</summary>
    public virtual bool ParalyzeImmunity => false;

    protected BaseMonkGi(int hue) : base(0x1F03, hue)
    {
        LootType = LootType.Blessed;
        Weight = 2.0;

        Attributes.BonusDex = DexBonus;
        Attributes.DefendChance = EvasionBonus;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!MonkItemHelper.IsMonk(from, RequiredMonkLevel))
        {
            from.SendMessage(0x22, "Only a monk can wear this gi.");
            return false;
        }

        return base.CanEquip(from);
    }

    /// <summary>Returns true if the mobile is wearing a Gi with paralyze immunity.</summary>
    public static bool HasParalyzeImmunity(Mobile m)
    {
        var gi = m.FindItemOnLayer<BaseMonkGi>(Layer.OuterTorso);
        return gi?.ParalyzeImmunity == true;
    }

    /// <summary>Returns the physical resist bonus from equipped Gi.</summary>
    public static int GetPhysicalResistBonus(Mobile m)
    {
        var gi = m.FindItemOnLayer<BaseMonkGi>(Layer.OuterTorso);
        return gi?.PhysicalResistBonus ?? 0;
    }

    /// <summary>
    /// Called when the monk dodges an attack. Adds a Chi charge.
    /// Should be hooked from the damage/miss system.
    /// </summary>
    public static void OnDodge(Mobile monk)
    {
        var level = MonkItemHelper.GetMonkLevel(monk);
        if (level < 1)
        {
            return;
        }

        var maxChi = MonkItemHelper.MaxChi(level);
        var charges = ChiSystem.AddCharge(monk, maxChi);

        monk.FixedParticles(0x375A, 1, 10, 5037, 0x480, 0, EffectLayer.Waist);
        monk.SendMessage(0x480, $"Chi: {charges}/{maxChi}");
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (EvasionBonus > 0)
        {
            list.Add(1042971, $"{"Evasion"}\t+{EvasionBonus}%");
        }

        if (PhysicalResistBonus > 0)
        {
            list.Add(1042971, $"{"Physical Resist"}\t+{PhysicalResistBonus}%");
        }

        if (ParalyzeImmunity)
        {
            list.Add(1042971, $"{"Paralyze Immunity"}");
        }
    }
}
