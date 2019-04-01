using ElfKingdom;

namespace SkillZ
{
    public class VirtualCreature : VirtualGameObject
    {
        public VirtualCreature(GameObject realGameObject) : base(realGameObject)
        {
        }

        public VirtualCreature(Player owner, Location location, int health, GameObject creator = null) : base(owner, location, health, creator)
        {
        }
    }
}
