using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace PvPStadium.Commands;

public static class TestCommands
{
    public static void Initialize()
    {
        // [TestDamage <amount> — target a player to deal raw damage
        CommandSystem.Register("TestDamage", AccessLevel.GameMaster, e =>
        {
            if (e.Arguments.Length < 1 || !int.TryParse(e.Arguments[0], out var amount) || amount < 1)
            {
                e.Mobile.SendMessage("Usage: [TestDamage <amount>");
                return;
            }

            e.Mobile.Target = new TestDamageTarget(amount);
            e.Mobile.SendMessage(0x44, $"Target a player to deal {amount} damage.");
        });

        // [TestPoison <level> — target a player to apply poison (0=Lesser, 1=Regular, 2=Greater, 3=Deadly, 4=Lethal)
        CommandSystem.Register("TestPoison", AccessLevel.GameMaster, e =>
        {
            var level = 0;
            if (e.Arguments.Length >= 1)
            {
                int.TryParse(e.Arguments[0], out level);
            }

            level = Math.Clamp(level, 0, 4);

            e.Mobile.Target = new TestPoisonTarget(level);
            e.Mobile.SendMessage(0x44, $"Target a player to apply level {level} poison.");
        });
    }

    private sealed class TestDamageTarget : Target
    {
        private readonly int _amount;

        public TestDamageTarget(int amount) : base(12, false, TargetFlags.None) => _amount = amount;

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is not Mobile target)
            {
                from.SendMessage(0x22, "Target must be a mobile.");
                return;
            }

            var hpBefore = target.Hits;
            target.Damage(_amount, from);
            var hpAfter = target.Hits;
            var actual = hpBefore - hpAfter;

            from.SendMessage(0x44, $"Dealt damage: requested={_amount}, actual HP lost={actual} (HP: {hpBefore} -> {hpAfter})");
        }
    }

    private sealed class TestPoisonTarget : Target
    {
        private readonly int _level;

        public TestPoisonTarget(int level) : base(12, false, TargetFlags.None) => _level = level;

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is not Mobile target)
            {
                from.SendMessage(0x22, "Target must be a mobile.");
                return;
            }

            var poison = Poison.GetPoison(_level);
            var result = target.ApplyPoison(from, poison);
            from.SendMessage(0x44, $"Poison level {_level} applied to {target.Name}: result={result}");
        }
    }
}
