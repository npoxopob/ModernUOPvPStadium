using System;
using ModernUO.Serialization;
using Server;
using Server.Items;
using Server.Mobiles;

namespace PvPStadium.Items.Amazon.Weapons;

/// <summary>
/// Base class for amazon krysses (swordsmanship, kryss graphic).
/// Features: chance to apply poison + STR weaken debuff on hit.
/// </summary>
[SerializationGenerator(0)]
public abstract partial class BaseAmazonKryss : BaseSword
{
    public virtual int RequiredAmazonLevel => 1;
    public virtual double ProcChance => 0.50;
    public virtual int WeakenStrMin => 18;
    public virtual int WeakenStrMax => 25;
    public virtual double WeakenDuration => 10.0;
    public virtual bool AppliesPoison => false;
    public virtual int PoisonLevel => 2; // Greater poison

    public override SkillName DefSkill => SkillName.Fencing;
    public override WeaponType DefType => WeaponType.Piercing;

    protected BaseAmazonKryss(int itemID) : base(itemID)
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

        if (Utility.RandomDouble() < ProcChance)
        {
            // Apply poison
            if (AppliesPoison)
            {
                var poison = Poison.GetPoison(PoisonLevel);
                defender.ApplyPoison(attacker, poison);
            }

            // Apply STR weaken debuff
            var weakenAmount = Utility.RandomMinMax(WeakenStrMin, WeakenStrMax);
            var modName = $"{Serial}AmazonWeaken";

            // Don't stack
            if (defender.GetStatMod(modName) == null)
            {
                defender.AddStatMod(new StatMod(StatType.Str, modName, -weakenAmount,
                    TimeSpan.FromSeconds(WeakenDuration)));
                defender.SendMessage(0x22, "You feel weakened!");
                defender.FixedParticles(0x3779, 10, 15, 5009, EffectLayer.Waist);
                defender.PlaySound(0x1E6);
                attacker.PublicOverheadMessage(MessageType.Emote, 0x3B2, false, "*Venomous Strike!*");
            }
        }
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (AppliesPoison)
        {
            list.Add(1042971, $"{"Poison + Weaken"}\t{(int)(ProcChance * 100)}%");
        }
        else
        {
            list.Add(1042971, $"{"Weaken"}\t{(int)(ProcChance * 100)}% ({WeakenStrMin}-{WeakenStrMax} {"STR"}, {WeakenDuration}{"s"}");
        }
    }
}
