using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using PvPStadium.Items.Paladin.Weapons;

namespace PvPStadium.Items.Paladin;

/// <summary>
/// Holy Essence — paladin consumable (level 2+).
/// Use on a paladin sword to bless it (8-10 charges of enhanced holy damage vs chaos).
/// Use on a chaos-class player to deal 20-25 holy damage directly.
/// Has 50 charges total.
/// </summary>
[SerializationGenerator(0, false)]
public partial class HolyEssence : Item
{
    public static int MaxCharges => 50;
    public static int BlessChargesMin => 8;
    public static int BlessChargesMax => 10;
    public static int DirectDamageMin => 20;
    public static int DirectDamageMax => 25;
    public static double CooldownSeconds => 2.0;

    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _charges;

    [Constructible]
    public HolyEssence() : base(0x0EFB) // bottle graphic
    {
        Name = "Holy Essence";
        Hue = 0x0990;
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

        if (!PaladinItemHelper.IsPaladin(from, 2))
        {
            from.SendMessage(0x22, "Only a Knight of Justice or higher can use Holy Essence.");
            return;
        }

        if (_charges <= 0)
        {
            from.SendMessage(0x22, "This Holy Essence is empty.");
            return;
        }

        from.SendMessage(0x3B2, "Target a paladin weapon to bless, or a chaos enemy to smite.");
        from.Target = new HolyEssenceTarget(this);
    }

    public void ConsumeCharge(Mobile from, int count = 1)
    {
        _charges = Math.Max(0, _charges - count);
        InvalidateProperties();

        if (_charges <= 0)
        {
            from.SendMessage(0x22, "The Holy Essence is now empty.");
        }
    }

    public override void AddNameProperties(IPropertyList list)
    {
        base.AddNameProperties(list);
        list.Add(1042971, $"Charges: {_charges}/{MaxCharges}");
        list.Add(1042971, "Bless weapons or smite chaos enemies");
    }

    private class HolyEssenceTarget : Target
    {
        private readonly HolyEssence _essence;

        public HolyEssenceTarget(HolyEssence essence) : base(2, false, TargetFlags.None)
        {
            _essence = essence;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (_essence.Deleted || !_essence.IsChildOf(from.Backpack))
                return;

            if (_essence._charges <= 0)
            {
                from.SendMessage(0x22, "This Holy Essence is empty.");
                return;
            }

            // Target is a paladin sword — bless it
            if (targeted is BasePaladinSword sword)
            {
                if (!sword.IsChildOf(from.Backpack) && sword.Parent != from)
                {
                    from.SendMessage(0x22, "The weapon must be in your pack or equipped.");
                    return;
                }

                var blessCharges = Utility.RandomMinMax(BlessChargesMin, BlessChargesMax);
                sword.ApplyBlessing(blessCharges);
                _essence.ConsumeCharge(from);

                from.SendMessage(0x3B2, $"You bless the weapon with {blessCharges} charges of holy power.");
                from.PlaySound(0x1F2);
                from.FixedParticles(0x375A, 10, 15, 5037, 0x480, 0, EffectLayer.Waist);
                sword.InvalidateProperties();
                return;
            }

            // Target is a chaos-class player — direct holy damage
            if (targeted is PlayerMobile target && PaladinItemHelper.IsChaosClass(target))
            {
                if (!from.CanBeginAction<HolyEssence>())
                {
                    from.SendMessage(0x22, "You must wait before using Holy Essence again.");
                    return;
                }

                if (!from.CanBeHarmful(target))
                    return;

                from.DoHarmful(target);
                from.BeginAction<HolyEssence>();
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), () => from.EndAction<HolyEssence>());

                var damage = Utility.RandomMinMax(DirectDamageMin, DirectDamageMax);
                AOS.Damage(target, from, damage, 0, 100, 0, 0, 0); // holy (fire) damage

                target.FixedParticles(0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot);
                target.PlaySound(0x208);
                from.PublicOverheadMessage(MessageType.Emote, 0x480, false, "*Holy Fire!*");

                _essence.ConsumeCharge(from);
                return;
            }

            from.SendMessage(0x22, "You can only use this on a paladin weapon or a chaos enemy.");
        }
    }
}
