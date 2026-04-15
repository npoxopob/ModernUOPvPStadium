using ModernUO.Serialization;
using Server;

namespace PvPStadium.Items.Vampire.Clothing;

/// <summary>
/// Level 1 vampire shroud. No stat bonuses, no poison protection. Hides helm.
/// pvp_alfa: color 0455.
/// </summary>
[SerializationGenerator(0)]
public partial class GhoulShroud : BaseVampireShroud
{
    [Constructible]
    public GhoulShroud() : base(0x0455)
    {
        Name = "Ghoul Shroud";
    }

    public override int RequiredVampireLevel => 1;
    public override int StrBonus => 0;
    public override bool PoisonResist => false;
    public override bool PoisonReflect => false;
}
