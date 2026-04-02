using System;
using Server.Mobiles;

namespace Server;

public static class CombatEvents
{
    // Fired after a melee weapon hit is resolved (attacker dealt 'damage' to defender)
    public static event Action<Mobile, Mobile, int>? WeaponMeleeHit;

    public static void RaiseWeaponMeleeHit(Mobile attacker, Mobile defender, int damage)
    {
        WeaponMeleeHit?.Invoke(attacker, defender, damage);
    }
}
