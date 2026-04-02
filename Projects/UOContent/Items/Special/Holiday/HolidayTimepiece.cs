using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class HolidayTimepiece : Clock
{
    [Constructible]
    public HolidayTimepiece() : base(0x1086)
    {
        LootType = LootType.Blessed;
        Layer = Layer.Bracelet;
    }

    public override int LabelNumber => 1041113; // a holiday timepiece
    public override double DefaultWeight => 1.0;

    public override void OnDoubleClick(Mobile from)
    {
        if (from == null)
        {
            return;
        }
        // Если уже надето — стандартное поведение часов
        if (Parent == from)
        {
            base.OnDoubleClick(from);
            return;
        }
        // Автоэквип только из рюкзака игрока
        if (IsChildOf(from.Backpack))
        {
            _ = from.EquipItem(this);
            return;
        }
        base.OnDoubleClick(from);
    }
}
