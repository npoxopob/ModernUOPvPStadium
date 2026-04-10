using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Gumps;
using Server.Targeting;
using PvPStadium.Races;

namespace PvPStadium.Commands;

public static class PvPRaceCommands
{
    public static void Initialize()
    {
        // Основная команда выбора расы
        CommandSystem.Register("Race", AccessLevel.Player, e =>
        {
            if (e.Mobile is PlayerMobile pm)
            {
                pm.CloseGump<RaceSelectionGump>();
                pm.SendGump(new RaceSelectionGump(pm));
            }
        });

        // Абилка вампира: активный Life Drain
        PvPStadium.Mechanics.VampireLifeDrain.RegisterCommand();

        // Выдать способность Life Drain выбранной цели (или себе, если нет таргета)
        CommandSystem.Register("GiveLifeDrain", AccessLevel.GameMaster, e =>
        {
            if (e.Mobile is PlayerMobile src)
            {
                src.Target = new GiveLifeDrainTarget();
                src.SendMessage(0x44, "Укажите игрока для выдачи Life Drain (ESC — выдать себе)");
            }
        });

        // Забрать способность Life Drain у цели (или себя)
        CommandSystem.Register("RevokeLifeDrain", AccessLevel.GameMaster, e =>
        {
            if (e.Mobile is PlayerMobile src)
            {
                src.Target = new RevokeLifeDrainTarget();
                src.SendMessage(0x44, "Укажите игрока для отзыва Life Drain (ESC — у себя)");
            }
        });

        CommandSystem.Register("RaceReload", AccessLevel.GameMaster, e =>
        {
            RaceRegistry.Load();
            e.Mobile.SendMessage(0x44, "Race list reloaded.");
        });

        // GM command to set PvP logical race and level on a target player
        CommandSystem.Register("SetPvPRace", AccessLevel.GameMaster, e =>
        {
            if (e.Arguments.Length < 2)
            {
                e.Mobile.SendMessage("Usage: [SetPvPRace <raceKey> <level>");
                e.Mobile.SendMessage("Example: [SetPvPRace vampire 4");
                e.Mobile.SendMessage("Keys: vampire, paladin, berserker, amazon, necromancer, human");
                return;
            }

            var raceKey = e.Arguments[0].ToLower();
            if (!int.TryParse(e.Arguments[1], out var level) || level < 1 || level > 4)
            {
                e.Mobile.SendMessage(0x22, "Level must be 1-4.");
                return;
            }

            e.Mobile.Target = new SetPvPRaceTarget(raceKey, level);
            e.Mobile.SendMessage(0x44, $"Target a player to set race={raceKey} level={level}");
        });

        // Сброс КД LifeDrain у выбранной цели
        CommandSystem.Register("LifeDrainReset", AccessLevel.GameMaster, e =>
        {
            if (e.Mobile is PlayerMobile src)
            {
                src.Target = new LifeDrainResetTarget();
                src.SendMessage(0x44, "Укажите игрока для сброса КД LifeDrain");
            }
        });
    }

    private sealed class GiveLifeDrainTarget : Target
    {
        public GiveLifeDrainTarget() : base(12, false, TargetFlags.None) { }
        protected override void OnTarget(Mobile from, object targeted)
        {
            var pm = targeted as PlayerMobile ?? from as PlayerMobile;
            if (pm == null) return;

            var st = RaceStateStore.GetOrCreate(pm.Serial);
            st.HasLifeDrainAbility = true;
            RaceStateStore.Save();
            pm.SendMessage(0x35, "Вам выдана способность Life Drain.");
            if (from != pm) from.SendMessage(0x44, $"Выдан Life Drain: {pm.Name}");
        }
        protected override void OnTargetCancel(Mobile from, TargetCancelType cancel)
        {
            from.SendMessage(0x22, "Операция отменена.");
        }
    }

    private sealed class RevokeLifeDrainTarget : Target
    {
        public RevokeLifeDrainTarget() : base(12, false, TargetFlags.None) { }
        protected override void OnTarget(Mobile from, object targeted)
        {
            var pm = targeted as PlayerMobile ?? from as PlayerMobile;
            if (pm == null) return;

            var st = RaceStateStore.GetOrCreate(pm.Serial);
            st.HasLifeDrainAbility = false;
            RaceStateStore.Save();
            pm.SendMessage(0x22, "Способность Life Drain отозвана.");
            if (from != pm) from.SendMessage(0x44, $"Отозван Life Drain: {pm.Name}");
        }
        protected override void OnTargetCancel(Mobile from, TargetCancelType cancel)
        {
            from.SendMessage(0x22, "Операция отменена.");
        }
    }

    private sealed class LifeDrainResetTarget : Target
    {
        public LifeDrainResetTarget() : base(12, false, TargetFlags.None) { }
        protected override void OnTarget(Mobile from, object targeted)
        {
            var pm = targeted as PlayerMobile;
            if (pm == null)
            {
                from.SendMessage(0x22, "Нужен игрок.");
                return;
            }
            var st = RaceStateStore.GetOrCreate(pm.Serial);
            st.LastLifeDrainAtTicks = 0;
            RaceStateStore.Save();
            from.SendMessage(0x44, $"КД LifeDrain сброшен у {pm.Name}.");
            pm.SendMessage(0x35, "Ваш кулдаун LifeDrain был сброшен администратором.");
        }
        protected override void OnTargetCancel(Mobile from, TargetCancelType cancel)
        {
            from.SendMessage(0x22, "Операция отменена.");
        }
    }

    private sealed class SetPvPRaceTarget : Target
    {
        private readonly string _raceKey;
        private readonly int _level;

        public SetPvPRaceTarget(string raceKey, int level) : base(12, false, TargetFlags.None)
        {
            _raceKey = raceKey;
            _level = level;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            var pm = targeted as PlayerMobile;
            if (pm == null)
            {
                from.SendMessage(0x22, "Target must be a player.");
                return;
            }

            var st = RaceStateStore.GetOrCreate(pm.Serial);
            st.RaceKey = _raceKey;
            st.Level = _level;
            RaceStateStore.Save();

            var levelName = RaceRegistry.GetLevelName(_raceKey, _level);
            from.SendMessage(0x44, $"Set {pm.Name} to {_raceKey} level {_level} ({levelName}).");
            pm.SendMessage(0x35, $"Your PvP race is now: {levelName}");
            pm.InvalidateProperties();
        }

        protected override void OnTargetCancel(Mobile from, TargetCancelType cancel)
        {
            from.SendMessage(0x22, "Cancelled.");
        }
    }
}
