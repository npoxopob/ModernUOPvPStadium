using Server;
using PvPStadium.Races;
using Server.Mobiles;
using Server.Gumps;

namespace PvPStadium.Commands;

public static class RaceCommands
{
    
    public static void Initialize()
    {
        CommandSystem.Register("RaceSelect", AccessLevel.GameMaster, e =>
        {
            if (e.Mobile is PlayerMobile pm)
            {
                pm.CloseGump<RaceSelectionGump>();
                pm.SendGump(new RaceSelectionGump(pm));
            }
        });

        CommandSystem.Register("RaceInfo", AccessLevel.Counselor, e =>
        {
            if (e.Mobile is PlayerMobile pm)
            {
                if (PvPStadium.Races.RaceStateStore.TryGet(pm.Serial, out var c) && c != null && !string.IsNullOrEmpty(c.RaceKey))
                {
                    var name = PvPStadium.Races.RaceRegistry.GetLevelName(c.RaceKey!, c.Level);
                    e.Mobile.SendMessage($"LogicalRace: {c.RaceKey} [{name}], Level={c.Level}");
                }
                else
                {
                    e.Mobile.SendMessage("LogicalRace: (not set)");
                }
            }
        });

        CommandSystem.Register("SetRace", AccessLevel.GameMaster, e =>
        {
            if (e.Arguments.Length < 1)
            {
                e.Mobile.SendMessage("Usage: SetRace <Human|Elf|Gargoyle>");
                return;
            }
            if (e.Mobile is not PlayerMobile pm)
            {
                e.Mobile.SendMessage("Player required.");
                return;
            }
            if (!Race.TryParse(e.Arguments[0], null, out var race) || race == null)
            {
                e.Mobile.SendMessage("Unknown race.");
                return;
            }
            RaceService.Assign(pm, race);
        });
    }
}
