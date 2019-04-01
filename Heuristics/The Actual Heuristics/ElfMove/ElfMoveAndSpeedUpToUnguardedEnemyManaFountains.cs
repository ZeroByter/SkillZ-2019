using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveAndSpeedUpToUnguardedEnemyManaFountains : Heuristic
    {
        private int distanceFromEnemyPortals;

        public ElfMoveAndSpeedUpToUnguardedEnemyManaFountains(float weight, int distanceFromEnemyPortals) : base(weight)
        {
            this.distanceFromEnemyPortals = distanceFromEnemyPortals;
        }

        private float GetLocationScore(VirtualGame virtualGame, Elf elf, Location elfFutureLocation)
        {
            float score = 0;

            int keepAwayDistance = Constants.Game.ElfAttackRange + Constants.Game.ManaFountainSize - 25;

            foreach (ManaFountain enemyManaFountain in Constants.GameCaching.GetEnemyManaFountains())
            {
                if (Constants.GameCaching.GetEnemyPortalsInArea(new Circle(enemyManaFountain, distanceFromEnemyPortals)).Count > 0) continue;
                Location targetLocation = enemyManaFountain.GetNewLocation(elf, 0, keepAwayDistance);

                score -= elfFutureLocation.DistanceF(targetLocation);

                if (virtualGame.DoesElfHaveFutureSpeedUp(elf)) score += 1;
            }

            return score;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float highestScore = 0;
            bool iterated = false;

            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                float tempScore = GetLocationScore(virtualGame, pair.Value.GetElf(), pair.Value.GetFutureLocation());

                if (!iterated || tempScore > highestScore)
                {
                    iterated = true;
                    highestScore = tempScore;
                }
            }

            return highestScore / Constants.Game.ElfMaxSpeed;
        }
    }
}