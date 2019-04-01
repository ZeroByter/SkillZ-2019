using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonLavaGiantToAttackEnemyCastle : Heuristic
    {
        public PortalSummonLavaGiantToAttackEnemyCastle(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            /*float lowestScore = 0;
            bool iterated = false;

            Castle enemyCastle = Constants.Game.GetEnemyCastle();

            foreach (KeyValuePair<int, VirtualGameObject> pair in virtualGame.futureGameObjects)
            {
                VirtualGameObject creature = pair.Value;

                if(creature is VirtualLavaGiant)
                {
                    float tempScore = creature.DistanceF(enemyCastle);

                    if (!iterated || tempScore <= lowestScore)
                    {
                        iterated = true;
                        lowestScore = tempScore;
                    }
                }
            }

            return lowestScore / Constants.Game.ElfMaxSpeed;*/

            float score = 0;

            Castle enemyCastle = Constants.Game.GetEnemyCastle();

            foreach (KeyValuePair<int, VirtualLavaGiant> pair in virtualGame.futureLavaGiants)
            {
                score -= pair.Value.DistanceF(enemyCastle);
            }

            return score;
        }
    }
}
