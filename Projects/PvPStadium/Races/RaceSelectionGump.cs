using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

namespace PvPStadium.Races;

internal class RaceSelectionGump : Gump
{
    private readonly PlayerMobile _pm;
    private readonly RaceSpec[] _specs;

    // Race descriptions and hues for display
    internal static readonly (string key, int hue, string desc)[] RaceInfo =
    {
        ("vampire", 0x21, "Dark warriors who drain life from enemies.\nWeapons: Claws, Sickles, Hands\nSpecial: Lifesteal, Bleed, Fire DoT, Poison Resist"),
        ("paladin", 0x480, "Holy knights who smite chaos enemies.\nWeapons: Swords, Maces\nSpecial: Holy Damage, Heal, BoneBreak, Paralyze Resist"),
        ("berserker", 0x26, "Savage fighters fueled by rage.\nWeapons: Axes\nSpecial: Fury System, HP Regen, Weaken/Curse Immunity"),
        ("amazon", 0x3B2, "Agile huntresses with deadly precision.\nWeapons: Spears, Krysses, Bows\nSpecial: Paralyze, Poison, Distance Crit, Clumsy Reflect"),
        ("necromancer", 0x455, "Dark mages who command death itself.\nWeapons: Staves, Daggers\nSpecial: Curse of Decay, Soul Drain, Mana Drain, Holy Resist"),
        ("human", 0x835, "Versatile warriors with customizable weapons.\nWeapons: Swords with Crystal Sockets\nSpecial: Fire/Ice/Poison/Thunder/Life Crystals, Shields"),
        ("monk", 0x480, "Masters of martial arts fueled by Chi.\nWeapons: Staves, Fists (Macing)\nSpecial: Chi System, Counter Strike, Rapid Flurry, Evasion"),
        ("elementalist", 0x489, "Burst mages who channel elemental forces.\nWeapons: Wands, Orbs (Macing)\nSpecial: Overcharge System, Elemental Burst (Fire/Ice/Lightning)")
    };

    public RaceSelectionGump(PlayerMobile pm) : base(0, 0)
    {
        _pm = pm;
        var specs = RaceRegistry.Specs;
        _specs = new RaceSpec[specs.Count];
        for (int i = 0; i < specs.Count; i++)
        {
            _specs[i] = specs[i];
        }

        Closable = true;
        Draggable = true;
        Resizable = false;

        // Get current race info
        string currentRace = "";
        string currentLevel = "";
        if (RaceStateStore.TryGet(pm.Serial, out var st) && st != null &&
            !string.IsNullOrEmpty(st.RaceKey) && st.Level > 0)
        {
            currentRace = st.RaceKey;
            currentLevel = RaceRegistry.GetLevelName(st.RaceKey, st.Level);
        }

        AddPage(0);

        // Main background
        AddBackground(30, 30, 560, 480, 9270);

        // Title
        AddLabel(180, 50, 1152, "PvP Stadium - Choose Your Path");

        // Current race display
        if (currentLevel.Length > 0)
        {
            AddLabel(60, 80, 0x44, $"Current: {currentLevel}");
        }
        else
        {
            AddLabel(60, 80, 0x22, "No race selected");
        }

        // Separator
        AddImageTiled(50, 105, 530, 2, 2624);

        // Race list
        for (int i = 0; i < _specs.Length; i++)
        {
            var spec = _specs[i];
            int y = 115 + i * 60;
            int hue = 1152;
            string desc = "";

            // Find matching race info
            foreach (var ri in RaceInfo)
            {
                if (ri.key == spec.Key)
                {
                    hue = ri.hue;
                    desc = ri.desc;
                    break;
                }
            }

            // Highlight current race
            bool isCurrent = spec.Key == currentRace;
            if (isCurrent)
            {
                AddImageTiled(50, y - 2, 530, 56, 2624);
            }

            // Button
            AddButton(60, y + 8, 4005, 4007, i + 1, GumpButtonType.Reply, 0);

            // Race name
            AddLabel(95, y + 5, hue, spec.Display + (isCurrent ? " [CURRENT]" : ""));

            // Levels preview
            var levels = "";
            for (int j = 0; j < spec.Levels.Count; j++)
            {
                if (j > 0)
                {
                    levels += " → ";
                }
                levels += spec.Levels[j].Name;
            }
            AddLabel(95, y + 25, 992, levels);

            // Description on hover area
            if (desc.Length > 0)
            {
                // Show first line of description
                var firstLine = desc;
                var nlIdx = desc.IndexOf('\n');
                if (nlIdx > 0)
                {
                    firstLine = desc[..nlIdx];
                }
                AddLabel(300, y + 5, 0x384, firstLine);

                // Show second line
                if (nlIdx > 0)
                {
                    var rest = desc[(nlIdx + 1)..];
                    var nl2 = rest.IndexOf('\n');
                    var secondLine = nl2 > 0 ? rest[..nl2] : rest;
                    AddLabel(300, y + 25, 0x384, secondLine);
                }
            }
        }

        // Footer
        int footerY = 115 + _specs.Length * 60 + 10;
        AddImageTiled(50, footerY, 530, 2, 2624);
        AddLabel(60, footerY + 8, 992, "Choose a path, then select your level.");
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        if (sender?.Mobile != _pm)
        {
            return;
        }

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

    // Level descriptions per race
    private static string GetLevelDesc(string raceKey, int level) => (raceKey, level) switch
    {
        ("vampire", 1) => "Basic claws. No special procs.",
        ("vampire", 2) => "Sickles with bleed, Hands with fire. Blood Amulet.",
        ("vampire", 3) => "Stronger weapons. Poison resist shroud.",
        ("vampire", 4) => "Best weapons. Poison reflect shroud. Full kit.",

        ("paladin", 1) => "Basic sword and mace. No procs.",
        ("paladin", 2) => "Holy damage proc. Holy Ring (paralyze resist). Holy Essence.",
        ("paladin", 3) => "Stronger weapons. Ring of Heaven (paralyze reflect).",
        ("paladin", 4) => "Best weapons. Maximum holy damage and chaos bonus.",

        ("berserker", 1) => "Basic axe and mask. No Fury bonus.",
        ("berserker", 2) => "Better weapons. Kilt (+5 DEX, Weaken immune). Restoration.",
        ("berserker", 3) => "Stronger weapons. Ancient Kilt (+10 DEX). Fury system.",
        ("berserker", 4) => "Best weapons. Curse immune. Max fury paralyze immunity.",

        ("amazon", 1) => "Basic spear, kryss, bow. Skirt (+10 DEX, Clumsy resist).",
        ("amazon", 2) => "Paralyze/Poison procs. Clumsy reflect skirt.",
        ("amazon", 3) => "Superior weapons. Clumsy + Weaken reflect.",
        ("amazon", 4) => "Elite weapons. +15 DEX skirt. Lethal poison. 60% procs.",

        ("necromancer", 1) => "Basic staff and dagger. Apprentice robe (+10 INT).",
        ("necromancer", 2) => "Curse of Decay. Soul Drain. Dark Essence. Holy resist 25%.",
        ("necromancer", 3) => "Stronger procs. Poison dagger. Paralyze resist ring.",
        ("necromancer", 4) => "Best weapons. +20 INT. Holy resist 60%. Paralyze reflect.",

        ("human", 1) => "Sword with 1 socket. Shield. Cloak (+5 DEX). Ring (+5 STR).",
        ("human", 2) => "Sword with 2 sockets. Shield (5% reflect). Cloak (+HP regen).",
        ("human", 3) => "Sword with 3 sockets. Shield (10% reflect). Power 2 crystals.",
        ("human", 4) => "Best sword, 3 sockets. Shield (15% reflect). Power 3 crystals.",

        ("monk", 1) => "Bamboo Staff, Apprentice Fists. Gi (+5 DEX). Prayer Beads.",
        ("monk", 2) => "Counter Strike (25%). Flurry (20%). Dodge Magic 20%. Incense.",
        ("monk", 3) => "Dragon Staff (+8 vs Berserker). Tiger Fists. 30% Dodge Magic.",
        ("monk", 4) => "Celestial weapons. +15 DEX. 40% Dodge. Paralyze Immunity.",

        ("elementalist", 1) => "Apprentice Wand/Orb. Fire only. 3 Overcharge. Initiate Mantle (+5 INT). Focus Ring (+1 FC).",
        ("elementalist", 2) => "Evoker Wand/Orb. Fire+Ice. 25% proc. Evoker Mantle (+10 INT, 10% Spell Absorb). Evoker Ring.",
        ("elementalist", 3) => "Storm Wand/Orb. All 3 elements. Resonance Ring (15% Double Burst). 20% Spell Absorb.",
        ("elementalist", 4) => "Archmage Wand/Orb. 40% proc. Mini-Burst orb. Archmage Mantle (30% Absorb). 25% Double Burst.",

        _ => ""
    };

    public RaceLevelGump(PlayerMobile pm, RaceSpec spec) : base(0, 0)
    {
        _pm = pm;
        _spec = spec;

        Closable = true;
        Draggable = true;
        Resizable = false;

        // Find race hue
        int raceHue = 1152;
        foreach (var ri in RaceSelectionGump.RaceInfo)
        {
            if (ri.key == spec.Key)
            {
                raceHue = ri.hue;
                break;
            }
        }

        AddPage(0);
        AddBackground(50, 50, 500, 340, 9270);

        // Title
        AddLabel(80, 70, raceHue, $"{spec.Display} - Choose Level");

        // Back button
        AddButton(460, 70, 4014, 4016, 999, GumpButtonType.Reply, 0);
        AddLabel(420, 70, 992, "Back");

        AddImageTiled(70, 95, 460, 2, 2624);

        for (int i = 0; i < spec.Levels.Count; i++)
        {
            var lv = spec.Levels[i];
            int y = 105 + i * 55;

            // Button
            AddButton(80, y + 8, 4005, 4007, lv.Level, GumpButtonType.Reply, 0);

            // Level name
            AddLabel(115, y + 5, raceHue, $"Level {lv.Level}: {lv.Name}");

            // Description
            var desc = GetLevelDesc(spec.Key, lv.Level);
            if (desc.Length > 0)
            {
                AddLabel(115, y + 25, 992, desc);
            }
        }
    }

    public override void OnResponse(NetState sender, in RelayInfo info)
    {
        if (sender?.Mobile != _pm)
        {
            return;
        }

        int bid = info.ButtonID;

        if (bid == 999)
        {
            // Back to race selection
            _pm.CloseGump<RaceSelectionGump>();
            _pm.SendGump(new RaceSelectionGump(_pm));
            return;
        }

        if (bid <= 0)
        {
            return;
        }

        RaceService.AssignLogical(_pm, _spec.Key, bid);
    }
}
