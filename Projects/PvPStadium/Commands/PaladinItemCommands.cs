using Server;
using Server.Mobiles;
using Server.Targeting;
using PvPStadium.Items.Paladin;
using PvPStadium.Items.Paladin.Weapons;
using PvPStadium.Items.Paladin.Jewelry;

namespace PvPStadium.Commands;

public static class PaladinItemCommands
{
    public static void Initialize()
    {
        CommandSystem.Register("GivePaladinKit", AccessLevel.GameMaster, e =>
        {
            e.Mobile.Target = new GivePaladinKitTarget();
            e.Mobile.SendMessage(0x44, "Target a player to give them a paladin item kit.");
        });
    }

    private sealed class GivePaladinKitTarget : Target
    {
        public GivePaladinKitTarget() : base(12, false, TargetFlags.None) { }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is not PlayerMobile pm)
            {
                from.SendMessage(0x22, "Target must be a player.");
                return;
            }

            var level = PaladinItemHelper.GetPaladinLevel(pm);
            if (level < 1)
            {
                from.SendMessage(0x22, $"{pm.Name} is not a paladin. Use [SetPvPRace paladin <level> first.");
                return;
            }

            var pack = pm.Backpack;
            if (pack == null)
            {
                from.SendMessage(0x22, $"{pm.Name} has no backpack!");
                return;
            }

            var count = 0;

            // All levels get Holy Essence
            pack.DropItem(new HolyEssence());
            count++;

            switch (level)
            {
                case 1: // Follower of Light
                    pack.DropItem(new BladeOfLight());
                    pack.DropItem(new BlessedMace());
                    count += 2;
                    break;

                case 2: // Knight of Justice
                    pack.DropItem(new BladeOfJustice());
                    pack.DropItem(new MaceOfDisruption());
                    pack.DropItem(new HolyRing());
                    count += 3;
                    break;

                case 3: // Paladin
                    pack.DropItem(new AngerOfHeaven());
                    pack.DropItem(new HolyBonebreaker());
                    pack.DropItem(new RingOfHeaven());
                    // Also give lower-tier weapons
                    pack.DropItem(new BladeOfJustice());
                    pack.DropItem(new MaceOfDisruption());
                    count += 5;
                    break;

                case 4: // Guardian of Heaven
                    pack.DropItem(new BladeOfFate());
                    pack.DropItem(new MaceOfRetribution());
                    pack.DropItem(new RingOfHeaven());
                    // Also give lower-tier weapons
                    pack.DropItem(new AngerOfHeaven());
                    pack.DropItem(new HolyBonebreaker());
                    count += 5;
                    break;
            }

            from.SendMessage(0x44, $"Gave {count} paladin items to {pm.Name} (level {level}).");
            pm.SendMessage(0x3B2, "You have received your paladin equipment!");
        }
    }
}
