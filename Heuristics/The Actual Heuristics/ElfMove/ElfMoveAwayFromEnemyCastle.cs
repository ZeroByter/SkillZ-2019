using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveAwayFromEnemyCastle : Heuristic
    {
        private float avoidRadius;

        public ElfMoveAwayFromEnemyCastle(float weight, float avoidRadius) : base(weight)
        {
            this.avoidRadius = avoidRadius;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                float distanceToEnemyCastle = pair.Value.GetFutureLocation().DistanceF(Constants.Game.GetEnemyCastle());

                if(distanceToEnemyCastle <= avoidRadius)
                {
                    score -= avoidRadius - distanceToEnemyCastle;
                }
            }

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}