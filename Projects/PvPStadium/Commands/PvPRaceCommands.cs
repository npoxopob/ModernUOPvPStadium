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
}
