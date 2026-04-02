using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

namespace PvPStadium.Races;

internal class RaceSelectionGump : Gump
{
    private readonly PlayerMobile _pm;
    private readonly RaceSpec[] _specs;

    public RaceSelectionGump(PlayerMobile pm) : base(0, 0)
    {
        _pm = pm;
        var specs = RaceRegistry.Specs;
        _specs = new RaceSpec[specs.Count];
        for (int i = 0; i < specs.Count; i++)
        {
            _specs[i] = specs[i];
        }

        Closable = true; Draggable = true; Resizable = false;

        AddPage(0);
        AddBackground(50, 50, 460, 320, 9270);
        AddLabel(80, 70, 1152, "Выбор архетипа (PvP Stadium)");

        for (int i = 0; i < _specs.Length; i++)
        {
            int y = 110 + i * 30;
            AddLabel(100, y, 1152, _specs[i].Display);
            AddButton(70, y, 4005, 4007, i + 1, GumpButtonType.Reply, 0);
        }
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        if (sender?.Mobile != _pm)
            return;

        int bid = info.ButtonID;
        if (bid <= 0)
        {
            return;
        }

        int index = bid - 1;
        if (index >= 0 && index < _specs.Length)
        {
            _pm.CloseGump<RaceLevelGump>();
            _pm.SendGump(new RaceLevelGump(_pm, _specs[index]));
        }
    }
}

internal class RaceLevelGump : Gump
{
    private readonly PlayerMobile _pm;
    private readonly RaceSpec _spec;

    public RaceLevelGump(PlayerMobile pm, RaceSpec spec) : base(0, 0)
    {
        _pm = pm;
        _spec = spec;

        Closable = true; Draggable = true; Resizable = false;

        AddPage(0);
        AddBackground(50, 50, 460, 320, 9270);
        AddLabel(80, 70, 1152, $"{spec.Display}: выбор уровня");

        for (int i = 0; i < spec.Levels.Count; i++)
        {
            var lv = spec.Levels[i];
            int y = 110 + i * 30;
            AddLabel(100, y, 1152, lv.Name);
            AddButton(70, y, 4005, 4007, lv.Level, GumpButtonType.Reply, 0);
        }
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        if (sender?.Mobile != _pm)
            return;

        int level = info.ButtonID;
        if (level <= 0)
        {
            return;
        }

        RaceService.AssignLogical(_pm, _spec.Key, level);
    }
}
