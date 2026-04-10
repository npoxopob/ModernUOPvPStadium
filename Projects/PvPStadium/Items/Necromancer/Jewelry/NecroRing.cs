using System;
using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Necromancer.Jewelry;

/// <summary>
/// Necromancer ring. Provides magic bonuses via AosAttributes.
/// Level 1: +1 Faster Casting
/// Level 2: +1 FC, +5% Spell Damage
/// Level 3: +1 FC, +10% Spell Damage, 33% Paralyze Resist
/// Level 4: +2 FC, +15% Spell Damage, 33% Paralyze Reflect
/// </summary>
[SerializationGenerator(0, false)]
public partial class NecroRing : BaseRing
{
    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _requiredLevel;

    [SerializableField(1)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private bool _paralyzeResist;

    [SerializableField(2)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private bool _paralyzeReflect;

    [Constructible]
    public NecroRing() : this(1)
    {
    }

    [Constructible]
    public NecroRing(int level) : base(0x108A) // gold ring
    {
        _requiredLevel = level;
        LootType = LootType.Blessed;

        switch (level)
        {
            case 1:
                Name = "Apprentice Ring";
                Hue = 0x0455;
                Attributes.CastSpeed = 1;
                break;
            case 2:
                Name = "Ring of Dark Power";
                Hue = 0x0482;
                Attributes.CastSpeed = 1;
                Attributes.SpellDamage = 5;
                break;
            case 3:
                Name = "Ring of the Lich";
                Hue = 0x0497;
                Attributes.CastSpeed = 1;
                Attributes.SpellDamage = 10;
                _paralyzeResist = true;
                break;
            default: // 4+
                Name = "Ring of the Void";
                Hue = 0x0386;
                Attributes.CastSpeed = 2;
                Attributes.SpellDamage = 15;
                _paralyzeReflect = true;
                break;
        }
    }

    public override double DefaultWeight => 0.1;

    public override bool CanEquip(Mobile from)
    {
        if (!NecromancerItemHelper.IsNecromancer(from, _requiredLevel))
        {
            from.SendMessage(0x22, "Only a necromancer can wear this ring.");
            return false;
        }

        return base.CanEquip(from);
    }

    /// <summary>Checks if target wears necro ring with paralyze resist (blocks paralyze 33%).</summary>
    public static bool TryResistParalyze(Mobile target)
    {
        var ring = target.FindItemOnLayer<NecroRing>(Layer.Ring);
        if (ring == null || !ring._paralyzeResist)
            return false;

        if (Utility.RandomDouble() < 0.33)
        {
            target.SendMessage(0x3B2, "Your ring resists the paralyze!");
            target.FixedParticles(0x375A, 10, 15, 5037, 0x455, 0, EffectLayer.Waist);
            return true;
        }

        return false;
    }

    /// <summary>Checks if target wears necro ring with paralyze reflect (reflects 33%).</summary>
    public static bool TryReflectParalyze(Mobile target, Mobile caster)
    {
        var ring = target.FindItemOnLayer<NecroRing>(Layer.Ring);
        if (ring == null || !ring._paralyzeReflect)
            return false;

        if (caster != null && caster != target && Utility.RandomDouble() < 0.33)
        {
            target.SendMessage(0x3B2, "Your ring reflects the paralyze!");
            target.FixedParticles(0x375A, 10, 15, 5037, 0x455, 0, EffectLayer.Waist);
            caster.Paralyze(TimeSpan.FromSeconds(5.0));
            caster.SendMessage(0x22, "The paralyze has been reflected back at you!");
            return true;
        }

        return false;
    }

    public override void AddNameProperties(IPropertyList list)
    {
        base.AddNameProperties(list);

        if (_paralyzeReflect)
            list.Add(1042971, "33% Paralyze Reflect");
        else if (_paralyzeResist)
            list.Add(1042971, "33% Paralyze Resist");
    }
}
