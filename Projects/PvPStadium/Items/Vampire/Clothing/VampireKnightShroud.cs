using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Vampire.Clothing;

/// <summary>
/// Level 3 vampire shroud. +10 STR, poison resistance. Hides helm.
/// pvp_alfa: color 0a26.
/// </summary>
[SerializationGenerator(0)]
public partial class VampireKnightShroud : BaseVampireShroud
{
    [Constructible]
    public VampireKnightShroud() : base(0x0A26)
    {
        Name = "Vampire Knight Shroud";
    }

    public override int RequiredVampireLevel => 3;
    public override int StrBonus => 10;
    public override bool PoisonResist => true;
    public override bool PoisonReflect => false;
}
