using Server;
using Server.Targeting;
using PvPStadium.Items.Human.Weapons;

namespace PvPStadium.Items.Human.Crystals;

public class CrystalSocketTarget : Target
{
    private readonly WeaponCrystal _crystal;

    public CrystalSocketTarget(WeaponCrystal crystal) : base(2, false, TargetFlags.None)
    {
        _crystal = crystal;
    }

    protected override void OnTarget(Mobile from, object targeted)
    {
        if (_crystal.Deleted || !_crystal.IsChildOf(from.Backpack))
        {
            return;
        }

        if (targeted is not BaseHumanSword sword)
        {
            from.SendMessage(0x22, "You can only socket crystals into a human sword.");
            return;
        }

        if (!sword.IsChildOf(from.Backpack) && sword.Parent != from)
        {
            from.SendMessage(0x22, "The weapon must be in your pack or equipped.");
            return;
        }

        if (!sword.TrySocketCrystal(_crystal.CrystalType, _crystal.Power))
        {
            from.SendMessage(0x22, "This weapon has no empty sockets.");
            return;
        }

        from.SendMessage(0x3B2, $"You socket the {_crystal.CrystalType} crystal into the weapon.");
        from.PlaySound(0x2A);
        from.FixedParticles(0x375A, 10, 15, 5037, _crystal.Hue, 0, EffectLayer.Waist);

        _crystal.Delete();
        sword.InvalidateProperties();
    }
}
