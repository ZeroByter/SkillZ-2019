using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfSimpleBuildPortalToAttackEnemyManaFountain : Heuristic
    {
        public int maxDistanceFromEnemyManaFountain;

        public ElfSimpleBuildPortalToAttackEnemyManaFountain(float weight, int maxDistanceFromEnemyManaFountain) : base(weight)
        {
            this.maxDistanceFromEnemyManaFountain = maxDistanceFromEnemyManaFountain;
        }

        private float GetEnemyManaFountainScore(VirtualGame virtualGame, ManaFountain manaFountain)
        {
            return virtualGame.GetVirtualPortalsInArea(manaFountain.GetLocation(), maxDistanceFromEnemyManaFountain).Count;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (ManaFountain manaFountain in Constants.GameCaching.GetEnemyManaFountains())
            {
                score += GetEnemyManaFountainScore(virtualGame, manaFountain);
            }

            return score;
        }

        public override string ToString()
        {
            return $"Heuristic {GetType().Name} has weight {weight} and maxDistanceFromEnemyManaFountain = {maxDistanceFromEnemyManaFountain}";
        }
    }
}