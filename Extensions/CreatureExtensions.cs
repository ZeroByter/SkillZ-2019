using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ
{
    public static class CreatureExtensions
    {
        public static int GetSuffocation(this Creature creature)
        {
            if (creature is IceTroll) return Constants.Game.IceTrollSuffocationPerTurn;
            if (creature is LavaGiant) return Constants.Game.LavaGiantSuffocationPerTurn;
            return 0;
        }

        public static int GetCost(this Creature creature)
        {
            if (creature is IceTroll) return Constants.Game.IceTrollCost;
            if (creature is LavaGiant) return Constants.Game.LavaGiantCost;
            return 0;
        }

        public static int PredictedDamageDoneToTarget(this Creature creature, GameObject target)
        {
            int timeToArrival = creature.TimeToArrive(target, true);
            int timeToSuffocation = creature.CurrentHealth / creature.SummoningDuration;

            //if we will arrive before we die
            if(timeToArrival < timeToSuffocation)
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
