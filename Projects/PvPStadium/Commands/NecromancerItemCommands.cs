using Server;
using Server.Mobiles;
using Server.Targeting;
using PvPStadium.Items.Necromancer;
using PvPStadium.Items.Necromancer.Weapons;
using PvPStadium.Items.Necromancer.Clothing;
using PvPStadium.Items.Necromancer.Jewelry;

namespace PvPStadium.Commands;

public static class NecromancerItemCommands
{
    public static void Initialize()
    {
        CommandSystem.Register("GiveNecromancerKit", AccessLevel.GameMaster, e =>
        {
            e.Mobile.Target = new GiveNecromancerKitTarget();
            e.Mobile.SendMessage(0x44, "Target a player to give them a necromancer item kit.");
        });
    }

    private sealed class GiveNecromancerKitTarget : Target
    {
        public GiveNecromancerKitTarget() : base(12, false, TargetFlags.None) { }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is not PlayerMobile pm)
            {
                from.SendMessage(0x22, "Target must be a player.");
                return;
            }

            var level = NecromancerItemHelper.GetNecromancerLevel(pm);
            if (level < 1)
            {
                from.SendMessage(0x22, $"{pm.Name} is not a necromancer. Use [SetPvPRace necromancer <level> first.");
                return;
            }

            var pack = pm.Backpack;
            if (pack == null)
            {
                from.SendMessage(0x22, $"{pm.Name} has no backpack!");
                return;
            }

            var count = 0;

            // Ring and robe for all levels
            pack.DropItem(new NecroRing(level));
            count++;

            switch (level)
            {
                case 1: // Apprentice
                    pack.DropItem(new ApprenticeStaff());
                    pack.DropItem(new ApprenticeDagger());
                    pack.DropItem(new ApprenticeRobe());
                    count += 3;
                    break;

                case 2: // Dark Adept
                    pack.DropItem(new StaffOfDecay());
                    pack.DropItem(new DaggerOfSoulDrain());
                    pack.DropItem(new RobeOfDarkness());
                    pack.DropItem(new DarkEssence());
                    count += 4;
                    break;

                case 3: // Dark Master
                    pack.DropItem(new StaffOfDamnation());
                    pack.DropItem(new DaggerOfTorment());
                    pack.DropItem(new RobeOfShadows());
                    pack.DropItem(new DarkEssence());
                    count += 4;
                    break;

                case 4: // Lich
                    pack.DropItem(new StaffOfOblivion());
                    pack.DropItem(new DaggerOfAnnihilation());
                    pack.DropItem(new RobeOfTheVoid());
                    pack.DropItem(new DarkEssence());
                    count += 4;
                    break;
            }

            from.SendMessage(0x44, $"Gave {count} necromancer items to {pm.Name} (level {level}).");
            pm.SendMessage(0x26, "You have received your necromancer equipment!");
        }
    }
}
