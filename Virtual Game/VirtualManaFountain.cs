using ElfKingdom;

namespace SkillZ
{
    public class VirtualManaFountain : VirtualGameObject
    {
        public VirtualManaFountain(GameObject realGameObject) : base(realGameObject)
        {
        }

        public VirtualManaFountain(Player owner, Location location, int health, GameObject creator = null) : base(owner, location, health, creator)
        {
        }
    }
}
