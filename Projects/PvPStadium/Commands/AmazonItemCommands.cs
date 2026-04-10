using Server;
using Server.Mobiles;
using Server.Targeting;
using PvPStadium.Items.Amazon;
using PvPStadium.Items.Amazon.Weapons;
using PvPStadium.Items.Amazon.Clothing;
using PvPStadium.Items.Amazon.Jewelry;

namespace PvPStadium.Commands;

public static class AmazonItemCommands
{
    public static void Initialize()
    {
        CommandSystem.Register("GiveAmazonKit", AccessLevel.GameMaster, e =>
        {
            e.Mobile.Target = new GiveAmazonKitTarget();
            e.Mobile.SendMessage(0x44, "Target a player to give them an amazon item kit.");
        });
    }

    private sealed class GiveAmazonKitTarget : Target
    {
        public GiveAmazonKitTarget() : base(12, false, TargetFlags.None) { }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is not PlayerMobile pm)
            {
                from.SendMessage(0x22, "Target must be a player.");
                return;
            }

            var level = AmazonItemHelper.GetAmazonLevel(pm);
            if (level < 1)
            {
                from.SendMessage(0x22, $"{pm.Name} is not an amazon. Use [SetPvPRace amazon <level> first.");
                return;
            }

            var pack = pm.Backpack;
            if (pack == null)
            {
                from.SendMessage(0x22, $"{pm.Name} has no backpack!");
                return;
            }

            var count = 0;

            // Earrings for all levels
            pack.DropItem(new AmazonEarrings(level));
            count++;

            switch (level)
            {
                case 1: // Amazon Girl
                    pack.DropItem(new AmazonianSpear());
                    pack.DropItem(new AmazonianKryss());
                    pack.DropItem(new AmazonianBow());
                    pack.DropItem(new SkirtOfDexterity());
                    count += 4;
                    break;

                case 2: // Amazon
                    pack.DropItem(new SpearOfParalyzeRoot());
                    pack.DropItem(new KryssOfIllness());
                    pack.DropItem(new BowOfParalyzeRoot());
                    pack.DropItem(new BowOfIllness());
                    pack.DropItem(new AmazonSkirt());
                    count += 5;
                    break;

                case 3: // Superior Amazon
                    pack.DropItem(new SuperiorSpearOfParalyzeRoot());
                    pack.DropItem(new SuperiorKryssOfIllness());
                    pack.DropItem(new SuperiorBowOfParalyzeRoot());
                    pack.DropItem(new SuperiorBowOfIllness());
                    pack.DropItem(new SuperiorAmazonSkirt());
                    count += 5;
                    break;

                case 4: // Elite Amazon
                    pack.DropItem(new EliteSpearOfParalyzeRoot());
                    pack.DropItem(new EliteKryssOfIllness());
                    pack.DropItem(new EliteBowOfParalyzeRoot());
                    pack.DropItem(new EliteBowOfIllness());
                    pack.DropItem(new EliteAmazonSkirt());
                    count += 5;
                    break;
            }

            from.SendMessage(0x44, $"Gave {count} amazon items to {pm.Name} (level {level}).");
            pm.SendMessage(0x26, "You have received your amazon equipment!");
        }
    }
}
