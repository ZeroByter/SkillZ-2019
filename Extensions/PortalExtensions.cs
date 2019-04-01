using ElfKingdom;

namespace SkillZ
{
    public static class PortalExtensions
    {
        public static bool IsCurrentlySummoning(this Portal portal, string creatureType)
        {
            return portal.CurrentlySummoning == creatureType;
        }
    }
}
