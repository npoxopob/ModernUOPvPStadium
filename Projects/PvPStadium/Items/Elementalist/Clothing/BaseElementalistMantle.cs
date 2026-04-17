using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Elementalist.Clothing;

/// <summary>
/// Base class for elementalist mantles (outer torso).
/// Features:
///   - INT bonus via Attributes.BonusInt
///   - Spell Damage Increase via Attributes.SpellDamage
///   - Spell Absorb: chance to reduce incoming spell damage (Lv3+)
/// Progression:
///   Lv1 Initiate Mantle:      +5 INT, +5% SD
///   Lv2 Evoker's Mantle:      +10 INT, +8% SD, 10% Spell Absorb
///   Lv3 Elementalist Mantle:  +15 INT, +12% SD, 20% Spell Absorb
///   Lv4 Archmage Mantle:      +20 INT, +15% SD, 30% Spell Absorb
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseElementalistMantle : BaseOuterTorso
{
    public virtual int RequiredElementalistLevel => 1;

    /// <summary>INT bonus applied via Attributes.BonusInt in constructor.</summary>
    public virtual int IntBonus => 5;

    /// <summary>Spell Damage Increase % applied via Attributes.SpellDamage in constructor.</summary>
    public virtual int SpellDamageBonus => 5;

    /// <summary>Chance (0–100) to absorb/reduce an incoming spell by SpellAbsorbPercent.</summary>
    public virtual int SpellAbsorbChance => 0;

    /// <summary>How much of the incoming spell damage is reduced on absorb (%).</summary>
    public const int SpellAbsorbPercent = 50;

    protected BaseElementalistMantle(int hue) : base(0x1F03, hue)
    {
        LootType = LootType.Blessed;
        Weight = 3.0;

        Attributes.BonusInt = IntBonus;
        Attributes.SpellDamage = SpellDamageBonus;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!ElementalistItemHelper.IsElementalist(from, RequiredElementalistLevel))
        {
            from.SendMessage(0x22, "Only an elementalist can wear this mantle.");
            return false;
        }

        return base.CanEquip(from);
    }

    /// <summary>
    /// Returns the Spell Absorb chance for mobile m (0 if not wearing an elementalist mantle).
    /// Called by damage hooks to check if incoming spell damage should be reduced.
    /// </summary>
    public static int GetSpellAbsorbChance(Mobile m)
    {
        var mantle = m.FindItemOnLayer<BaseElementalistMantle>(Layer.OuterTorso);
        return mantle?.SpellAbsorbChance ?? 0;
    }

    /// <summary>
    /// Checks whether this hit should be absorbed and notifies the player.
    /// Returns the damage multiplier: 0.5 if absorbed, 1.0 otherwise.
    /// </summary>
    public static double TryAbsorbSpell(Mobile target)
    {
        var absorbChance = GetSpellAbsorbChance(target);
        if (absorbChance <= 0)
        {
            return 1.0;
        }

        if (Utility.Random(100) < absorbChance)
        {
            target.SendMessage(0x489, "Your mantle absorbs part of the spell!");
            target.FixedParticles(0x375A, 10, 15, 5037, 0x489, 0, EffectLayer.Waist);
            target.PlaySound(0x1F9);
            return 1.0 - SpellAbsorbPercent / 100.0;
        }

        return 1.0;
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list); // shows Attributes.BonusInt + SpellDamage automatically

        if (SpellAbsorbChance > 0)
        {
            list.Add(1042971, $"{"Spell Absorb"}\t{SpellAbsorbChance}% chance ({SpellAbsorbPercent}% reduction)");
        }

        var levelName = RequiredElementalistLevel switch
        {
            1 => "Initiate",
            2 => "Evoker",
            3 => "Elementalist",
            4 => "Archmage",
            _ => $"Level {RequiredElementalistLevel}"
        };

        list.Add(1042971, $"{"Requires"}\t{levelName}");
    }
}
