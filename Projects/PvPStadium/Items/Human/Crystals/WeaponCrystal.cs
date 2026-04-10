using ModernUO.Serialization;
using Server;
using Server.Items;

namespace PvPStadium.Items.Human.Crystals;

/// <summary>
/// A crystal that can be socketed into a Human weapon.
/// Double-click then target a human sword to insert.
/// </summary>
[SerializationGenerator(0, false)]
public partial class WeaponCrystal : Item
{
    [SerializableField(0)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private CrystalType _crystalType;

    [SerializableField(1)]
    [SerializedCommandProperty(AccessLevel.GameMaster)]
    private int _power; // 1-3, affects proc strength

    [Constructible]
    public WeaponCrystal() : this(CrystalType.Fire, 1) { }

    [Constructible]
    public WeaponCrystal(CrystalType type, int power) : base(0x1F19) // small gem graphic
    {
        _crystalType = type;
        _power = System.Math.Clamp(power, 1, 3);
        Weight = 0.5;
        Stackable = false;

        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        (Name, Hue) = _crystalType switch
        {
            CrystalType.Fire    => ($"Fire Crystal (power {_power})", 0x0489),
            CrystalType.Ice     => ($"Ice Crystal (power {_power})", 0x0480),
            CrystalType.Poison  => ($"Poison Crystal (power {_power})", 0x0A4C),
            CrystalType.Thunder => ($"Thunder Crystal (power {_power})", 0x0487),
            CrystalType.Life    => ($"Life Crystal (power {_power})", 0x0990),
            _                   => ("Crystal", 0)
        };
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!IsChildOf(from.Backpack))
        {
            from.SendLocalizedMessage(1042001);
            return;
        }

        from.SendMessage(0x3B2, "Target a human weapon to socket this crystal.");
        from.Target = new CrystalSocketTarget(this);
    }

    public override void AddNameProperties(IPropertyList list)
    {
        base.AddNameProperties(list);

        var desc = _crystalType switch
        {
            CrystalType.Fire    => "Adds fire damage on hit",
            CrystalType.Ice     => "Adds cold damage + stamina drain",
            CrystalType.Poison  => "Adds poison proc on hit",
            CrystalType.Thunder => "Adds energy damage + mana drain",
            CrystalType.Life    => "Adds lifesteal on hit",
            _                   => ""
        };

        if (desc.Length > 0)
            list.Add(1042971, desc);
    }
}
