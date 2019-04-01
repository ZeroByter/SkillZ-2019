using ElfKingdom;

namespace SkillZ
{
    static class VirtualGameObjectExtensions
    {
        public static float DistanceF(this VirtualGameObject source, MapObject target)
        {
            return Mathf.Sqrt(Mathf.Pow(source.location.Col - target.GetLocation().Col, 2) + Mathf.Pow(source.location.Row - target.GetLocation().Row, 2));
        }

        public static int GetSize(this VirtualGameObject gameObject)
        {
            if (gameObject is VirtualIceTroll) return Constants.Game.IceTrollAttackRange;
            if (gameObject is VirtualLavaGiant) return Constants.Game.LavaGiantAttackRange;
            if (gameObject is VirtualPortal) return Constants.Game.PortalSize;
            return 0;
        }

        public static int GetMaxSpeed(this VirtualGameObject gameObject)
        {
            if (gameObject is VirtualIceTroll) return Constants.Game.IceTrollMaxSpeed;
            if (gameObject is VirtualLavaGiant) return Constants.Game.LavaGiantMaxSpeed;
            return 0;
        }

        public static int GetAttackMultiplier(this VirtualGameObject gameObject)
        {
            if (gameObject is VirtualIceTroll) return Constants.Game.IceTrollAttackMultiplier;
            if (gameObject is VirtualLavaGiant) return Constants.Game.LavaGiantAttackMultiplier;
            return 0;
        }

        public static int GetMaxHealth(this VirtualGameObject gameObject)
        {
            if (gameObject is VirtualIceTroll) return Constants.Game.IceTrollMaxHealth;
            if (gameObject is VirtualLavaGiant) return Constants.Game.LavaGiantMaxHealth;
            if (gameObject is VirtualPortal) return Constants.Game.PortalMaxHealth;
            return 0;
        }

        public static int GetSummoningDuration(this VirtualGameObject gameObject)
        {
            if (gameObject is VirtualIceTroll) return Constants.Game.IceTrollSummoningDuration;
            if (gameObject is VirtualLavaGiant) return Constants.Game.LavaGiantSummoningDuration;
            if (gameObject is VirtualPortal) return Constants.Game.PortalBuildingDuration;
            return 0;
        }

        public static int GetSuffocationPerTurn(this VirtualCreature creature)
        {
            if (creature is VirtualIceTroll) return Constants.Game.IceTrollSuffocationPerTurn;
            if (creature is VirtualLavaGiant) return Constants.Game.LavaGiantSuffocationPerTurn;
            return 0;
        }

        public static int PredictedDamageDoneToTarget(this VirtualCreature creature, GameObject target)
        {
            int timeToArrival = Mathf.CeilToInt((creature.location.DistanceF(target) - creature.GetSize() - target.GetSize()) / creature.GetMaxSpeed());
            int timeToSuffocation = creature.GetMaxHealth() / creature.GetSuffocationPerTurn();

            //if we will arrive before we die
            if (timeToArrival < timeToSuffocation)
            {
                int timeSpentAtTarget = timeToSuffocation - timeToArrival;
                int damageDone = timeSpentAtTarget * creature.GetAttackMultiplier();
                return damageDone;
            }
            else
            {
                return 0;
            }
        }
    }
}
