using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>
/// Base class for amazon spears (fencing).
/// Features: chance to paralyze on hit.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseAmazonSpear : BaseSpear
{
    public virtual int RequiredAmazonLevel => 1;
    public virtual double ParalyzeChance => 0.25;
    public virtual double ParalyzeDuration => 3.0;

    protected BaseAmazonSpear(int itemID) : base(itemID)
    {
        LootType = LootType.Blessed;
    }

    public override bool CanEquip(Mobile from)
    {
        if (!AmazonItemHelper.IsAmazon(from, RequiredAmazonLevel))
        {
            from.SendMessage(0x22, "Only an Amazon can wield this weapon.");
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

        if (Utility.RandomDouble() < ParalyzeChance && !defender.Frozen && !defender.Paralyzed)
        {
            defender.Paralyze(TimeSpan.FromSeconds(ParalyzeDuration));
            defender.PlaySound(0x204);
            defender.FixedEffect(0x376A, 6, 1);
            attacker.PublicOverheadMessage(MessageType.Emote, 0x3B2, false, "*Paralyzing Strike!*");
        }
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);
        list.Add(1042971, $"{"Paralyze"}\t{(int)(ParalyzeChance * 100)}% ({ParalyzeDuration}{"s"}");
    }
}
