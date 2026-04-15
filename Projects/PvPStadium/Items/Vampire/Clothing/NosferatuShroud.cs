using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Vampire.Clothing;

/// <summary>
/// Level 4 vampire shroud. +10 STR, +10 Parrying, poison REFLECT. Hides helm.
/// pvp_alfa: color 0a28.
/// </summary>
[SerializationGenerator(0)]
public partial class NosferatuShroud : BaseVampireShroud
{
    [Constructible]
    public NosferatuShroud() : base(0x0A28)
    {
        Name = "Nosferatu Vampire Shroud";
    }

    public override int RequiredVampireLevel => 4;
    public override int StrBonus => 10;
    public override double ParryBonus => 10.0;
    public override bool PoisonResist => true;
    public override bool PoisonReflect => true;
}
