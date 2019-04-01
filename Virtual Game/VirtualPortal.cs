using ElfKingdom;

namespace SkillZ
{
    public class VirtualPortal : VirtualGameObject
    {
        public VirtualPortal(GameObject realGameObject) : base(realGameObject)
        {
        }

        public VirtualPortal(Player owner, Location location, int health, GameObject creator = null) : base(owner, location, health, creator)
        {
        }
    }
}
