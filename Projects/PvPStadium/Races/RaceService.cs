using System;
using Server;
using Server.Gumps;
using Server.Logging;
using Server.Mobiles;
using Server.Accounting;
using Server.Items;

namespace PvPStadium.Races;

internal static class RaceService
{
    private static readonly ILogger _log = LogFactory.GetLogger(typeof(RaceService));

    public static void OnConnected(Mobile m)
    {
        if (m is not PlayerMobile pm)
        {
            return;
        }

        // Если логическая раса уже выбрана — ничего не показываем
        var hasChoice = RaceStateStore.TryGet(pm.Serial, out var choice) && choice != null && !string.IsNullOrEmpty(choice.RaceKey);
        if (hasChoice)
        {
            return;
        }

        // Принудительно ставим Human при первом входе (игрок мог выбрать Elf/Gargoyle на экране создания)
        pm.Race = Race.Human;
        pm.BodyMod = 0;
        pm.Body = Race.Human.AliveBody(pm);

        // PvP Stadium: remove Young status for new characters on first login
        if (pm.Account is Account acc)
        {
            acc.Young = false;
        }
        pm.Young = false;

        // Нормализацию внешности перенесено в CharacterCreation; здесь больше не чистим внешний вид
        // Автопоказ гампа отключён по запросу. Игроки могут открыть выбор рас командой [Race].
    }

    public static void Assign(PlayerMobile pm, Race race)
    {
        var old = pm.Race;
        ApplyRace(pm, race);
        RaceStateStore.Set(pm.Serial, race.RaceIndex);
        RaceStateStore.Save();

        if (old != race)
        {
            // Оранжевое уведомление игроку о смене расы
            pm.SendMessage(0x35, $"Ваша раса изменена на {race.Name}.");
        }

        _log.Information("[PvP Stadium] Race '{Race}' assigned to {Player}", race.Name, pm);
    }

    public static void ApplyRace(PlayerMobile pm, Race race)
    {
        // Установка расы применяет тело и резисты через Mobile.Race setter
        pm.Race = race;
        // Принудительно синхронизируем тело с расой и сбрасываем модификатор тела
        pm.BodyMod = 0;
        pm.Body = race.AliveBody(pm);
        // После выбора расы оставим игроку возможность настраивать внешность далее, поэтому
        // здесь НЕ обнуляем одежду/волосы
    }

    // Назначение логической PvP-расы с уровнем (без изменения Core Race)
    public static void AssignLogical(PlayerMobile pm, string raceKey, int level)
    {
        if (string.IsNullOrWhiteSpace(raceKey))
        {
            pm.SendMessage(0x22, "Неизвестная раса.");
            return;
        }

        var spec = RaceRegistry.GetSpec(raceKey);
        if (spec == null)
        {
            pm.SendMessage(0x22, "Неизвестная раса.");
            return;
        }

        var st = RaceStateStore.GetOrCreate(pm.Serial);
        st.RaceKey = raceKey;
        st.Level = level;
        RaceStateStore.Save();

        var levelName = RaceRegistry.GetLevelName(raceKey, level);
        pm.SendMessage(0x35, $"Ваша раса изменена на {levelName}.");
        pm.InvalidateProperties();
        _log.Information("[PvP Stadium] Logical race '{RaceKey}' (level {Level}) assigned to {Player}", raceKey, level, pm);
    }

    private static void NormalizeAppearance(PlayerMobile pm)
    {
        try
        {
            // Стандартный цвет тела
            pm.Hue = 0;

            // Сброс волос и растительности на лице
            pm.HairItemID = 0;
            pm.FacialHairItemID = 0;
            pm.HairHue = 0;
            pm.FacialHairHue = 0;

            // Удаляем стартовые вещи: все, что одето на слоях одежды/украшений/доспехов
            // Оставляем рюкзак, банк и прочие системные слои
            for (int i = pm.Items.Count - 1; i >= 0; i--)
            {
                var it = pm.Items[i];
                if (it == null)
                    continue;

                var layer = it.Layer;
                if (layer == Layer.Backpack || layer == Layer.Bank || layer == Layer.Mount || layer == Layer.Invalid)
                    continue;

                if (layer == Layer.Hair || layer == Layer.FacialHair)
                {
                    // уже сброшено выше, но на всякий случай
                    it.Delete();
                    continue;
                }

                // Всё остальное считаем одеждой/экипировкой
                it.Delete();
            }
        }
        catch (Exception ex)
        {
            _log.Warning(ex, "[PvP Stadium] Failed to normalize appearance for {Player}", pm);
        }
    }
}
