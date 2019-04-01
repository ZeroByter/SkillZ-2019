using ElfKingdom;

namespace SkillZ
{
    public class VirtualGameObject
    {
        //if we can consistently knowhow UniqueIds are generated, we can insert our own fake ones here...

        /// <summary>
        /// This might not always be set, since it is possible we are simulating a gameObject that doesn't exist yet...
        /// Use with caution!
        /// </summary>
        public GameObject realGameObject;

        /// <summary>
        /// If we are predicting a soon-to-be created GameObject, this is the GameObject that created us
        /// </summary>
        public GameObject creator;

        public Player owner;

        public Location location;

        public int health;

        public VirtualGameObject(GameObject realGameObject)
        {
            this.realGameObject = realGameObject;
            owner = realGameObject.Owner;
            location = realGameObject.GetLocation();
            health = realGameObject.CurrentHealth;
        }

        public VirtualGameObject(Player owner, Location location, int health, GameObject creator = null)
        {
            this.creator = creator;
            this.owner = owner;
            this.location = location;
            this.health = health;
        }

        public bool DoesHaveRealGameObject()
        {
            return realGameObject != null;
        }

        public bool IsDead()
        {
            return health <= 0;
        }

        public override bool Equals(object obj)
        {
            if(realGameObject != null && obj is GameObject)
            {
                return realGameObject == obj;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
