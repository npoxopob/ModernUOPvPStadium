using Server;
using Server.Mobiles;
using Server.Targeting;
using PvPStadium.Items.Elementalist;
using PvPStadium.Items.Elementalist.Weapons;
using PvPStadium.Items.Elementalist.Clothing;
using PvPStadium.Items.Elementalist.Jewelry;

namespace PvPStadium.Commands;

public static class ElementalistItemCommands
{
    public static void Initialize()
    {
        CommandSystem.Register("GiveElementalistKit", AccessLevel.GameMaster, e =>
        {
            e.Mobile.Target = new GiveElementalistKitTarget();
            e.Mobile.SendMessage(0x44, "Target a player to give them an elementalist item kit.");
        });
    }

    private sealed class GiveElementalistKitTarget : Target
    {
        public GiveElementalistKitTarget() : base(12, false, TargetFlags.None) { }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is not PlayerMobile pm)
            {
                from.SendMessage(0x22, "Target must be a player.");
                return;
            }

            var level = ElementalistItemHelper.GetElementalistLevel(pm);
            if (level < 1)
            {
                from.SendMessage(0x22, $"{pm.Name} is not an elementalist. Use [SetPvPRace elementalist <level> first.");
                return;
            }

            var pack = pm.Backpack;
            if (pack == null)
            {
                from.SendMessage(0x22, $"{pm.Name} has no backpack!");
                return;
            }

            var count = 0;

            // All levels get an Elemental Scroll
            pack.DropItem(new ElementalScroll());
            count++;

            switch (level)
            {
                case 1: // Initiate
                    pack.DropItem(new ApprenticeWand());
                    pack.DropItem(new ApprenticeOrb());
                    pack.DropItem(new InitiateMantle());
                    pack.DropItem(new FocusRing());
                    count += 4;
                    break;

                case 2: // Evoker
                    pack.DropItem(new EvokersWand());
                    pack.DropItem(new EvokersOrb());
                    pack.DropItem(new EvokersMantle());
                    pack.DropItem(new EvokerFocusRing());
                    // Also give Lv1 items
                    pack.DropItem(new ApprenticeWand());
                    pack.DropItem(new ApprenticeOrb());
                    count += 6;
                    break;

                case 3: // Elementalist
                    pack.DropItem(new StormWand());
                    pack.DropItem(new StormOrb());
                    pack.DropItem(new ElementalistMantle());
                    pack.DropItem(new ResonanceRing());
                    // Also give Lv2 items
                    pack.DropItem(new EvokersWand());
                    pack.DropItem(new EvokersOrb());
                    count += 6;
                    break;

                case 4: // Archmage
                    pack.DropItem(new ArchmageWand());
                    pack.DropItem(new ArchmageOrb());
                    pack.DropItem(new ArchmageMantle());
                    pack.DropItem(new ArchmageFocusRing());
                    // Also give Lv3 items
                    pack.DropItem(new StormWand());
                    pack.DropItem(new StormOrb());
                    count += 6;
                    break;
            }

            from.SendMessage(0x44, $"Gave {count} elementalist items to {pm.Name} (level {level}).");
            pm.SendMessage(0x3B2, "You have received your elementalist equipment!");
        }
    }
}
