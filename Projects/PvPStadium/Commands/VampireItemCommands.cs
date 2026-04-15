using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Targeting;
using PvPStadium.Items.Vampire;
using PvPStadium.Items.Vampire.Weapons;
using PvPStadium.Items.Vampire.Clothing;
using PvPStadium.Items.Vampire.Jewelry;
using PvPStadium.Races;

namespace PvPStadium.Commands;

public static class VampireItemCommands
{
    public static void Initialize()
    {
        CommandSystem.Register("GiveVampireKit", AccessLevel.GameMaster, OnGiveVampireKit);
    }

    [Usage("GiveVampireKit")]
    [Description("Gives a full set of vampire items to the target based on their vampire level.")]
    private static void OnGiveVampireKit(CommandEventArgs e)
    {
        e.Mobile.SendMessage("Target a vampire player to give them their class kit.");
        e.Mobile.Target = new VampireKitTarget();
    }

    private class VampireKitTarget : Target
    {
        public VampireKitTarget() : base(12, false, TargetFlags.None) { }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is not PlayerMobile pm)
            {
                from.SendMessage(0x22, "Target must be a player.");
                return;
            }

            var level = VampireItemHelper.GetVampireLevel(pm);
            if (level <= 0)
            {
                from.SendMessage(0x22, "That player is not a vampire. Use [Race] first.");
                return;
            }

            GiveKit(pm, level);
            from.SendMessage(0x44, $"Gave vampire kit (level {level}) to {pm.Name}.");
            pm.SendMessage(0x44, "You have received your vampire equipment!");
        }
    }

    private static void GiveKit(PlayerMobile pm, int level)
    {
        var pack = pm.Backpack;
        if (pack == null)
        {
            return;
        }

        // Bottle of Blood — all vampire levels get it
        pack.DropItem(new BottleOfBlood());

        switch (level)
        {
            case 1:
                pack.DropItem(new GhoulShroud());
                pack.DropItem(new GhoulClaw());
                pack.DropItem(new SickleOfNewborn());
                break;

            case 2:
                pack.DropItem(new VampireShroud());
                pack.DropItem(new VampireClaw());
                pack.DropItem(new BloodySickle());
                pack.DropItem(new BloodyHand());
                break;

            case 3:
                pack.DropItem(new VampireKnightShroud());
                pack.DropItem(new VampireKnightClaw());
                pack.DropItem(new StingOfPain());
                pack.DropItem(new HandOfPain());
                pack.DropItem(new FieryHand());
                break;

            case 4:
                pack.DropItem(new NosferatuShroud());
                pack.DropItem(new NosferatuClaw());
                pack.DropItem(new StingOfRevenge());
                pack.DropItem(new HandOfPain()); // level 4 uses level 3 ranged
                pack.DropItem(new FieryHand());
                pack.DropItem(new BloodAmulet());
                break;
        }
    }
}
