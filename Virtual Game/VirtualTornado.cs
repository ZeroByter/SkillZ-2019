using ElfKingdom;

namespace SkillZ
{
    public class VirtualTornado : VirtualCreature
    {
        public VirtualTornado(GameObject realGameObject) : base(realGameObject)
        {
        }

        public VirtualTornado(Player owner, Location location, int health, GameObject creator = null) : base(owner, location, health, creator)
        {
        }
    }
}
