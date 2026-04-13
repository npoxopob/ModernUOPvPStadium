using Server;
using Server.Mobiles;
using Server.Targeting;
using PvPStadium.Items.Monk;
using PvPStadium.Items.Monk.Weapons;
using PvPStadium.Items.Monk.Clothing;
using PvPStadium.Items.Monk.Jewelry;

namespace PvPStadium.Commands;

public static class MonkItemCommands
{
    public static void Initialize()
    {
        CommandSystem.Register("GiveMonkKit", AccessLevel.GameMaster, e =>
        {
            e.Mobile.Target = new GiveMonkKitTarget();
            e.Mobile.SendMessage(0x44, "Target a player to give them a monk item kit.");
        });
    }

    private sealed class GiveMonkKitTarget : Target
    {
        public GiveMonkKitTarget() : base(12, false, TargetFlags.None) { }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is not PlayerMobile pm)
            {
                from.SendMessage(0x22, "Target must be a player.");
                return;
            }

            var level = MonkItemHelper.GetMonkLevel(pm);
            if (level < 1)
            {
                from.SendMessage(0x22, $"{pm.Name} is not a monk. Use [SetPvPRace monk <level> first.");
                return;
            }

            var pack = pm.Backpack;
            if (pack == null)
            {
                from.SendMessage(0x22, $"{pm.Name} has no backpack!");
                return;
            }

            var count = 0;

            // Beads and Gi for all levels
            pack.DropItem(new MonkBeads(level));
            count++;

            switch (level)
            {
                case 1:
                    pack.DropItem(new BambooStaff());
                    pack.DropItem(new ApprenticeFists());
                    pack.DropItem(new DiscipleGi());
                    count += 3;
                    break;

                case 2:
                    pack.DropItem(new IronStaff());
                    pack.DropItem(new IronFists());
                    pack.DropItem(new AcolyteGi());
                    pack.DropItem(new Incense());
                    count += 4;
                    break;

                case 3:
                    pack.DropItem(new DragonStaff());
                    pack.DropItem(new TigerFists());
                    pack.DropItem(new MonkGi());
                    pack.DropItem(new Incense());
                    count += 4;
                    break;

                case 4:
                    pack.DropItem(new CelestialStaff());
                    pack.DropItem(new FistsOfHeaven());
                    pack.DropItem(new GrandmasterGi());
                    pack.DropItem(new Incense());
                    count += 4;
                    break;
            }

            from.SendMessage(0x44, $"Gave {count} monk items to {pm.Name} (level {level}).");
            pm.SendMessage(0x26, "You have received your monk equipment!");
        }
    }
}
