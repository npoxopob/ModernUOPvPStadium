using Server;
using Server.Mobiles;
using Server.Targeting;
using PvPStadium.Items.Berserker;
using PvPStadium.Items.Berserker.Weapons;
using PvPStadium.Items.Berserker.Armor;
using PvPStadium.Items.Berserker.Clothing;

namespace PvPStadium.Commands;

public static class BerserkerItemCommands
{
    public static void Initialize()
    {
        CommandSystem.Register("GiveBerserkerKit", AccessLevel.GameMaster, e =>
        {
            e.Mobile.Target = new GiveBerserkerKitTarget();
            e.Mobile.SendMessage(0x44, "Target a player to give them a berserker item kit.");
        });
    }

    private sealed class GiveBerserkerKitTarget : Target
    {
        public GiveBerserkerKitTarget() : base(12, false, TargetFlags.None) { }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is not PlayerMobile pm)
            {
                from.SendMessage(0x22, "Target must be a player.");
                return;
            }

            var level = BerserkerItemHelper.GetBerserkerLevel(pm);
            if (level < 1)
            {
                from.SendMessage(0x22, $"{pm.Name} is not a berserker. Use [SetPvPRace berserker <level> first.");
                return;
            }

            var pack = pm.Backpack;
            if (pack == null)
            {
                from.SendMessage(0x22, $"{pm.Name} has no backpack!");
                return;
            }

            var count = 0;

            // All levels get Restoration
            pack.DropItem(new Restoration());
            count++;

            switch (level)
            {
                case 1: // Adept of Might
                    pack.DropItem(new AdeptAxe());
                    pack.DropItem(new AdeptMask());
                    count += 2;
                    break;

                case 2: // Barbarian
                    pack.DropItem(new BarbarianAxe());
                    pack.DropItem(new BarbarianMask());
                    pack.DropItem(new BarbarianKilt());
                    count += 3;
                    break;

                case 3: // Berserker
                    pack.DropItem(new AncientAvenger());
                    pack.DropItem(new BerserkerMask());
                    pack.DropItem(new AncientKilt());
                    pack.DropItem(new BarbarianAxe());
                    count += 4;
                    break;

                case 4: // Child of Ragnar
                    pack.DropItem(new RageOfAncestors());
                    pack.DropItem(new EliteBerserkerMask());
                    pack.DropItem(new KiltOfAncestors());
                    pack.DropItem(new AncientAvenger());
                    count += 4;
                    break;
            }

            from.SendMessage(0x44, $"Gave {count} berserker items to {pm.Name} (level {level}).");
            pm.SendMessage(0x26, "You have received your berserker equipment!");
        }
    }
}
