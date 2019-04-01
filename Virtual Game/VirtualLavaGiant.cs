using ElfKingdom;

namespace SkillZ
{
    public class VirtualLavaGiant : VirtualCreature
    {
        public VirtualLavaGiant(GameObject realGameObject) : base(realGameObject)
        {
        }

        public VirtualLavaGiant(Player owner, Location location, int health, GameObject creator = null) : base(owner, location, health, creator)
        {
        }
    }
}
