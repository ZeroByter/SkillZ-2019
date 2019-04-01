using ElfKingdom;

namespace SkillZ
{
    public class VirtualIceTroll : VirtualCreature
    {
        public VirtualIceTroll(GameObject realGameObject) : base(realGameObject)
        {
        }

        public VirtualIceTroll(Player owner, Location location, int health, GameObject creator = null) : base(owner, location, health, creator)
        {
        }
    }
}
