using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using PvPStadium.Items.Necromancer.Weapons;

namespace PvPStadium.Items.Necromancer;

/// <summary>
/// Dark Essence — necromancer consumable (level 2+).
/// Use on a necro dagger to empower it (8-10 charges of enhanced Soul Drain).
/// Use on a light-class player (paladin) to deal 20-25 dark damage directly.
/// Has 50 charges total.
/// </summary>
[SerializationGenerator(0, false)]
public partial class DarkEssence : Item
{
    public static int MaxCharges => 50;
    public static int EmpowerChargesMin => 8;
    public static int EmpowerChargesMax => 10;
    public static int DirectDamageMin => 20;
    public static int DirectDamageMax => 25;
    public static double CooldownSeconds => 2.0;

    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _charges;

    [Constructible]
    public DarkEssence() : base(0x0EFB) // bottle graphic
    {
        Name = "Dark Essence";
        Hue = 0x0455;
        Weight = 1.0;
        LootType = LootType.Blessed;
        _charges = MaxCharges;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!IsChildOf(from.Backpack))
        {
            from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            return;
        }

        if (!NecromancerItemHelper.IsNecromancer(from, 2))
        {
            from.SendMessage(0x22, "Only a Dark Adept or higher can use Dark Essence.");
            return;
        }

        if (_charges <= 0)
        {
            from.SendMessage(0x22, "This Dark Essence is empty.");
            return;
        }

        from.SendMessage(0x455, "Target a necro dagger to empower, or a light enemy to curse.");
        from.Target = new DarkEssenceTarget(this);
    }

    public void ConsumeCharge(Mobile from, int count = 1)
    {
        _charges = Math.Max(0, _charges - count);
        InvalidateProperties();

        if (_charges <= 0)
        {
            from.SendMessage(0x22, "The Dark Essence is now empty.");
        }
    }

    public override void AddNameProperties(IPropertyList list)
    {
        base.AddNameProperties(list);
        list.Add(1042971, $"Charges: {_charges}/{MaxCharges}");
        list.Add(1042971, "Empower daggers or curse light enemies");
    }

    private class DarkEssenceTarget : Target
    {
        private readonly DarkEssence _essence;

        public DarkEssenceTarget(DarkEssence essence) : base(2, false, TargetFlags.None)
        {
            _essence = essence;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (_essence.Deleted || !_essence.IsChildOf(from.Backpack))
                return;

            if (_essence._charges <= 0)
            {
                from.SendMessage(0x22, "This Dark Essence is empty.");
                return;
            }

            // Target is a necro dagger — empower it
            if (targeted is BaseNecroDagger dagger)
            {
                if (!dagger.IsChildOf(from.Backpack) && dagger.Parent != from)
                {
                    from.SendMessage(0x22, "The weapon must be in your pack or equipped.");
                    return;
                }

                var empowerCharges = Utility.RandomMinMax(EmpowerChargesMin, EmpowerChargesMax);
                dagger.ApplyEmpowerment(empowerCharges);
                _essence.ConsumeCharge(from);

                from.SendMessage(0x455, $"You empower the dagger with {empowerCharges} charges of dark energy.");
                from.PlaySound(0x1FB);
                from.FixedParticles(0x374A, 10, 15, 5038, 0x455, 0, EffectLayer.Waist);
                dagger.InvalidateProperties();
                return;
            }

            // Target is a light-class player — direct dark damage
            if (targeted is PlayerMobile target && NecromancerItemHelper.IsLightClass(target))
            {
                if (!from.CanBeginAction<DarkEssence>())
                {
                    from.SendMessage(0x22, "You must wait before using Dark Essence again.");
                    return;
                }

                if (!from.CanBeHarmful(target))
                    return;

                from.DoHarmful(target);
                from.BeginAction<DarkEssence>();
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), () => from.EndAction<DarkEssence>());

                var damage = Utility.RandomMinMax(DirectDamageMin, DirectDamageMax);
                AOS.Damage(target, from, damage, 0, 0, 100, 0, 0); // dark (cold) damage

                target.FixedParticles(0x374A, 10, 30, 5038, 1109, 0, EffectLayer.Head);
                target.PlaySound(0x1FB);
                from.PublicOverheadMessage(MessageType.Emote, 0x455, false, "*Dark Curse!*");

                _essence.ConsumeCharge(from);
                return;
            }

            from.SendMessage(0x22, "You can only use this on a necro dagger or a light enemy.");
        }
    }
}
