using System;
using Server;
using Server.Mobiles;
using System.Collections.Generic;
using Server.Targeting;
using Server.Logging;
// Timer is in Server namespace

namespace PvPStadium.Mechanics;

public static class VampireLifeDrain
{
    public struct LifeDrainConfig
    {
        public bool Enabled;
        public double CooldownSec;
        public int TickMs;
        public int PerTickHP;               // default if no level mapping
        public double MaxDurationSec;       // default if no level mapping
        public Dictionary<int,int> PerTickHPByLevel;
        public Dictionary<int,double> MaxDurationSecByLevel;
        public int MaxRange;
        // Anti-paladin modifiers
        public bool PaladinModifierEnabled;
        public bool PaladinPvpOnly;
        public Dictionary<int,double> PaladinPerTickMultiplierByLevel;
        public double PaladinDefaultMultiplier;
    }

    public static LifeDrainConfig Config = new LifeDrainConfig
    {
        Enabled = true,
        CooldownSec = 420,
        TickMs = 1000,
        PerTickHP = 3,
        MaxDurationSec = 12,
        PerTickHPByLevel = new Dictionary<int,int> { {1,2},{2,3},{3,4},{4,5} },
        MaxDurationSecByLevel = new Dictionary<int,double> { {1,8},{2,10},{3,12},{4,14} },
        MaxRange = 8,
        PaladinModifierEnabled = true,
        PaladinPvpOnly = true,
        PaladinPerTickMultiplierByLevel = new Dictionary<int,double> { {1,0.75},{2,0.5},{3,0.35},{4,0.25} },
        PaladinDefaultMultiplier = 0.5
    };

    private static readonly ILogger _log = LogFactory.GetLogger(typeof(VampireLifeDrain));

    public static void RegisterCommand()
    {
        Server.CommandSystem.Register("LifeDrain", AccessLevel.Player, e =>
        {
            if (e.Mobile is not PlayerMobile pm)
            {
                return;
            }

            if (!IsVampire(pm))
            {
                pm.SendMessage(0x22, "Эта способность доступна только вампиру.");
                return;
            }
            var st = PvPStadium.Races.RaceStateStore.GetOrCreate(pm.Serial);
            if (!st.HasLifeDrainAbility)
            {
                pm.SendMessage(0x22, "Вы ещё не овладели способностью Life Drain.");
                return;
            }
            if (!Config.Enabled)
            {
                pm.SendMessage(0x22, "Способность временно недоступна.");
                return;
            }

            if (!CheckCooldown(pm, out var remaining))
            {
                pm.SendMessage(0x22, $"Перезарядка: {Math.Ceiling(remaining)} сек.");
                return;
            }

            pm.Target = new LifeDrainTarget(pm) { CheckLOS = true };
            pm.SendMessage(0x44, "Выберите цель для Life Drain.");
            pm.PublicOverheadMessage(MessageType.Label, 0x44, false, "LifeDrain: выберите цель");
        });
    }

    private static bool IsVampire(PlayerMobile pm)
    {
        return PvPStadium.Races.RaceStateStore.TryGet(pm.Serial, out var st) && st != null && st.RaceKey == "vampire";
    }

    private static bool CheckCooldown(PlayerMobile pm, out double remainingSec)
    {
        remainingSec = 0;
        var st = PvPStadium.Races.RaceStateStore.GetOrCreate(pm.Serial);
        var now = DateTime.UtcNow;
        var lastTicks = st.LastLifeDrainAtTicks;
        if (lastTicks > 0)
        {
            var last = new DateTime(lastTicks, DateTimeKind.Utc);
            var next = last.AddSeconds(Config.CooldownSec);
            if (now < next)
            {
                remainingSec = (next - now).TotalSeconds;
                return false;
            }
        }
        return true;
    }

    private static void BeginCooldown(PlayerMobile pm)
    {
        var st = PvPStadium.Races.RaceStateStore.GetOrCreate(pm.Serial);
        st.LastLifeDrainAtTicks = DateTime.UtcNow.Ticks;
        PvPStadium.Races.RaceStateStore.Save();
    }

    private sealed class LifeDrainTarget : Target
    {
        private readonly PlayerMobile _pm;
        public LifeDrainTarget(PlayerMobile pm) : base(12, false, TargetFlags.Harmful)
        {
            _pm = pm;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (from != _pm)
            {
                return;
            }
            if (targeted is not Mobile mob)
            {
                _pm.SendMessage(0x22, "Неверная цель.");
                return;
            }
            if (mob == _pm || mob.Deleted || mob.Map == Map.Internal || mob.Blessed || mob.AccessLevel > AccessLevel.Player)
            {
                _pm.SendMessage(0x22, "Недопустимая цель.");
                return;
            }
            if (_pm.Hits >= _pm.HitsMax)
            {
                _pm.SendMessage(0x22, "Вы итак полны сил.");
                return;
            }
            if (_pm.Map != mob.Map || !_pm.InRange(mob, Config.MaxRange))
            {
                _pm.SendMessage(0x22, "Слишком далеко.");
                return;
            }
            // Строгая проверка препятствий: видимость и прямая линия (если доступна)
            bool blocked = false;
            if (!_pm.CanSee(mob) || !_pm.InLOS(mob))
            {
                blocked = true;
            }
            else
            {
                try
                {
                    // Если доступен API карты для LOS, используем дополнительную проверку
                    if (_pm.Map != null && !_pm.Map.LineOfSight(_pm, mob))
                    {
                        blocked = true;
                    }
                }
                catch { }
            }
            if (blocked)
            {
                _pm.SendMessage(0x22, "Между вами и целью препятствие.");
                return;
            }

            _pm.RevealingAction();
            // Уровень вампира кастера
            var st = PvPStadium.Races.RaceStateStore.GetOrCreate(_pm.Serial);
            var lvl = st.Level > 0 ? st.Level : 1;
            var per = Config.PerTickHPByLevel != null && Config.PerTickHPByLevel.TryGetValue(lvl, out var v) ? v : Config.PerTickHP;
            var dur = Config.MaxDurationSecByLevel != null && Config.MaxDurationSecByLevel.TryGetValue(lvl, out var d) ? d : Config.MaxDurationSec;

            // Определение отражения: если цель — вампир и её уровень больше кастера
            bool reflect = false;
            if (mob is PlayerMobile tpm && PvPStadium.Races.RaceStateStore.TryGet(tpm.Serial, out var tc) && tc != null && tc.RaceKey == "vampire")
            {
                var tlvl = tc.Level > 0 ? tc.Level : 1;
                if (tlvl > lvl)
                {
                    reflect = true;
                    _pm.SendMessage(0x22, "Цель отражает дренаж!");
                }
            }

            // Анти‑паладин множитель рассчитываем ОДИН РАЗ на старте канала
            try
            {
                if (Config.PaladinModifierEnabled)
                {
                    // Жертва дренажа: если reflect=false, жертва = mob; если reflect=true, жертва = _pm
                    Mobile victim = reflect ? _pm : mob;
                    var pvpVictim = victim as PlayerMobile;
                    if (pvpVictim != null || !Config.PaladinPvpOnly)
                    {
                        if (pvpVictim != null && PvPStadium.Races.RaceStateStore.TryGet(pvpVictim.Serial, out var pc) && pc != null && pc.RaceKey == "paladin")
                        {
                            var plvl = pc.Level > 0 ? pc.Level : 1;
                            if (Config.PaladinPerTickMultiplierByLevel != null && Config.PaladinPerTickMultiplierByLevel.TryGetValue(plvl, out var mult))
                            {
                                per = Math.Max(1, (int)Math.Floor(per * mult));
                            }
                            else
                            {
                                per = Math.Max(1, (int)Math.Floor(per * Config.PaladinDefaultMultiplier));
                            }
                        }
                    }
                }
            }
            catch { }

            // КД стартует только при фактическом запуске канала
            BeginCooldown(_pm);
            new LifeDrainChannel(_pm, mob, per, dur, reflect).Start();
        }
    }

    private sealed class LifeDrainChannel : Timer
    {
        private readonly PlayerMobile _pm;
        private readonly Mobile _target;
        private readonly DateTime _end;
        private readonly int _perTick;
        private readonly bool _reflect;
        public LifeDrainChannel(PlayerMobile pm, Mobile target, int perTick, double durationSec, bool reflect) : base(TimeSpan.Zero, TimeSpan.FromMilliseconds(Config.TickMs))
        {
            _pm = pm;
            _target = target;
            _perTick = perTick;
            _reflect = reflect;
            _end = DateTime.UtcNow.AddSeconds(durationSec);
        }

        protected override void OnTick()
        {
            try
            {
                if (_pm.Deleted || _target.Deleted || !_pm.Alive || !_target.Alive || DateTime.UtcNow >= _end)
                {
                    Stop();
                    return;
                }
                if (_pm.Map != _target.Map || !_pm.InRange(_target, Config.MaxRange) || !_pm.InLOS(_target))
                {
                    _pm.SendMessage(0x22, "Связь прервана.");
                    Stop();
                    return;
                }
                try
                {
                    if (_pm.Map != null && !_pm.Map.LineOfSight(_pm, _target))
                    {
                        _pm.SendMessage(0x22, "Связь прервана (препятствие).");
                        Stop();
                        return;
                    }
                }
                catch { }

                Mobile src = _reflect ? _pm : _target;
                Mobile dst = _reflect ? _target : _pm;

                // Anti-paladin modifier теперь рассчитывается на старте канала; здесь используем сохранённый per-tick
                // Paladin holy aura: optional block or weaken per tick if active on victim
                if (PaladinHolyAura.IsAuraActive(src))
                {
                    if (PaladinHolyAura.Config.BlocksLifeDrain)
                    {
                        // Тик заблокирован святой аурой
                        return; // пропускаем тик без переноса
                    }
                    else
                    {
                        var weakened = Math.Max(1, (int)Math.Floor(_perTick * PaladinHolyAura.Config.Multiplier));
                        _ = weakened; // value will be used below when computing takeAvail
                        var takeAvailTmp = Math.Min(weakened, src.Hits);
                        if (takeAvailTmp <= 0)
                        {
                            Stop();
                            return;
                        }
                        src.Damage(takeAvailTmp, _pm);
                        var beforeTmp = dst.Hits;
                        dst.Hits = Math.Min(dst.HitsMax, dst.Hits + takeAvailTmp);
                        var gainedTmp = dst.Hits - beforeTmp;
                        try { Effects.SendMovingParticles(src, dst, 0x373A, 10, 0, false, false, 0x21, 0, 0, 0x100); Effects.PlaySound(dst.Location, dst.Map, 0x1F5); } catch {}
                        if (gainedTmp > 0)
                        {
                            dst.SendMessage(0x35, $"Life Drain (+{gainedTmp}).");
                            if (_reflect) { _pm.SendMessage(0x22, "Отражение крови бьёт по вам!"); }
                        }
                        return;
                    }
                }

                var takeAvail = Math.Min(_perTick, src.Hits);
                if (takeAvail <= 0)
                {
                    Stop();
                    return;
                }

                // Перенос HP: src теряет, dst получает
                src.Damage(takeAvail, _pm);
                var before = dst.Hits;
                dst.Hits = Math.Min(dst.HitsMax, dst.Hits + takeAvail);
                var gained = dst.Hits - before;

                // Visuals (направление от src к dst)
                try
                {
                    Effects.SendMovingParticles(src, dst, 0x373A, 10, 0, false, false, 0x21, 0, 0, 0x100);
                    Effects.PlaySound(dst.Location, dst.Map, 0x1F5);
                }
                catch { }

                if (gained > 0)
                {
                    dst.SendMessage(0x35, $"Life Drain (+{gained}).");
                    if (_reflect)
                    {
                        _pm.SendMessage(0x22, "Отражение крови бьёт по вам!");
                    }
                }
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger(typeof(LifeDrainChannel)).Warning(ex, "LifeDrain tick error");
                Stop();
            }
        }
    }
}
