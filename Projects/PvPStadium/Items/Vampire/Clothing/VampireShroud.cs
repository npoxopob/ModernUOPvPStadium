using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Vampire.Clothing;

/// <summary>
/// Level 2 vampire shroud. +5 STR, poison resistance. Hides helm.
/// pvp_alfa: color 0a11.
/// </summary>
[SerializationGenerator(0)]
public partial class VampireShroud : BaseVampireShroud
{
    [Constructible]
    public VampireShroud() : base(0x0A11)
    {
        Name = "Vampire Shroud";
    }

    public override int RequiredVampireLevel => 2;
    public override int StrBonus => 5;
    public override bool PoisonResist => true;
    public override bool PoisonReflect => false;
}
