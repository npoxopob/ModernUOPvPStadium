using Server;
using Server.Mobiles;
using Server.Targeting;
using PvPStadium.Items.Human;
using PvPStadium.Items.Human.Weapons;
using PvPStadium.Items.Human.Armor;
using PvPStadium.Items.Human.Jewelry;
using PvPStadium.Items.Human.Clothing;
using PvPStadium.Items.Human.Crystals;

namespace PvPStadium.Commands;

public static class HumanItemCommands
{
    public static void Initialize()
    {
        CommandSystem.Register("GiveHumanKit", AccessLevel.GameMaster, e =>
        {
            e.Mobile.Target = new GiveHumanKitTarget();
            e.Mobile.SendMessage(0x44, "Target a player to give them a human item kit.");
        });

        CommandSystem.Register("GiveCrystal", AccessLevel.GameMaster, e =>
        {
            if (e.Arguments.Length < 2)
            {
                e.Mobile.SendMessage("Usage: [GiveCrystal <fire|ice|poison|thunder|life> <power 1-3>");
                return;
            }

            var typeStr = e.Arguments[0].ToLower();
            var type = typeStr switch
            {
                "fire" => CrystalType.Fire,
                "ice" => CrystalType.Ice,
                "poison" => CrystalType.Poison,
                "thunder" => CrystalType.Thunder,
                "life" => CrystalType.Life,
                _ => CrystalType.None
            };

            if (type == CrystalType.None)
            {
                e.Mobile.SendMessage(0x22, "Invalid crystal type. Use: fire, ice, poison, thunder, life");
                return;
            }

            if (!int.TryParse(e.Arguments[1], out var power) || power < 1 || power > 3)
            {
                e.Mobile.SendMessage(0x22, "Power must be 1-3.");
                return;
            }

            var crystal = new WeaponCrystal(type, power);
            e.Mobile.Backpack?.DropItem(crystal);
            e.Mobile.SendMessage(0x44, $"Created {type} Crystal (power {power}).");
        });
    }

    private sealed class GiveHumanKitTarget : Target
    {
        public GiveHumanKitTarget() : base(12, false, TargetFlags.None) { }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is not PlayerMobile pm)
            {
                from.SendMessage(0x22, "Target must be a player.");
                return;
            }

            var level = HumanItemHelper.GetHumanLevel(pm);
            if (level < 1)
            {
                from.SendMessage(0x22, $"{pm.Name} is not a human. Use [SetPvPRace human <level> first.");
                return;
            }

            var pack = pm.Backpack;
            if (pack == null)
            {
                from.SendMessage(0x22, $"{pm.Name} has no backpack!");
                return;
            }

            var count = 0;

            // Ring and cloak for all levels
            pack.DropItem(new HumanRing(level));
            count++;

            // Give a set of crystals (one of each type at appropriate power)
            var crystalPower = level >= 4 ? 3 : level >= 3 ? 2 : 1;
            pack.DropItem(new WeaponCrystal(CrystalType.Fire, crystalPower));
            pack.DropItem(new WeaponCrystal(CrystalType.Ice, crystalPower));
            pack.DropItem(new WeaponCrystal(CrystalType.Poison, crystalPower));
            pack.DropItem(new WeaponCrystal(CrystalType.Thunder, crystalPower));
            pack.DropItem(new WeaponCrystal(CrystalType.Life, crystalPower));
            count += 5;

            switch (level)
            {
                case 1: // Militia
                    pack.DropItem(new MilitiaSword());
                    pack.DropItem(new MilitiaShield());
                    pack.DropItem(new MilitiaCloak());
                    count += 3;
                    break;

                case 2: // Veteran
                    pack.DropItem(new VeteranSword());
                    pack.DropItem(new VeteranShield());
                    pack.DropItem(new VeteranCloak());
                    count += 3;
                    break;

                case 3: // Captain
                    pack.DropItem(new CaptainSword());
                    pack.DropItem(new CaptainShield());
                    pack.DropItem(new CaptainCloak());
                    count += 3;
                    break;

                case 4: // Commander
                    pack.DropItem(new CommanderSword());
                    pack.DropItem(new CommanderShield());
                    pack.DropItem(new CommanderCloak());
                    count += 3;
                    break;
            }

            from.SendMessage(0x44, $"Gave {count} human items to {pm.Name} (level {level}).");
            pm.SendMessage(0x26, "You have received your human equipment!");
        }
    }
}
