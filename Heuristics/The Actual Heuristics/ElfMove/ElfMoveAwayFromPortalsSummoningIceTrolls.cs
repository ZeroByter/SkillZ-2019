using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveAwayFromPortalsSummoningIceTrolls : Heuristic
    {
        private float avoidRadius;
        private int maxTurnsToSummon;

        public ElfMoveAwayFromPortalsSummoningIceTrolls(float weight, float avoidRadius, int maxTurnsToSummon) : base(weight)
        {
            this.avoidRadius = avoidRadius;
            this.maxTurnsToSummon = maxTurnsToSummon;
        }

        private float GetElfScore(VirtualGame virtualGame, Location elfFutureLocation)
        {
            List<Portal> enemyPorals = Constants.GameCaching.GetEnemyPortalsInAreaCurrentlySummoningIceTrolls(new Circle(elfFutureLocation, avoidRadius));

            float score = 0;

            foreach(Portal enemyPortal in enemyPorals)
            {
                if (enemyPortal.TurnsToSummon > maxTurnsToSummon) continue;

                float distance = elfFutureLocation.DistanceF(enemyPortal);

                if (distance < avoidRadius)
                {
                    score -= avoidRadius - distance;
                }
            }

            return score;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                score += GetElfScore(virtualGame, pair.Value.GetFutureLocation());
            }

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}