using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfSimpleBuildPortalToAttackEnemyElf : Heuristic
    {
        public int maxDistanceFromEnemyElf;
        public int maxDistanceFromMyCastle;

        public ElfSimpleBuildPortalToAttackEnemyElf(float weight, int maxDistanceFromEnemyElf, int maxDistanceFromMyCastle) : base(weight)
        {
            this.maxDistanceFromEnemyElf = maxDistanceFromEnemyElf;
            this.maxDistanceFromMyCastle = maxDistanceFromMyCastle;
        }

        private float GetEnemyElfScore(VirtualGame virtualGame, Elf enemyElf)
        {
            Castle myCastle = Constants.Game.GetMyCastle();

            Dictionary<int, VirtualPortal> futurePortalsInArea = virtualGame.GetVirtualPortalsInArea(enemyElf.GetLocation(), maxDistanceFromEnemyElf);
           
            return futurePortalsInArea.Count;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (Elf enemyElf in Constants.GameCaching.GetEnemyElvesInArea(new Circle(Constants.Game.GetMyCastle().GetLocation(), maxDistanceFromMyCastle)))
            {
                score += GetEnemyElfScore(virtualGame, enemyElf);
            }

            return score;
        }

        public override string ToString()
        {
            return $"Heuristic {GetType().Name} has weight {weight} and maxDistanceFromEnemyElf = {maxDistanceFromEnemyElf} and maxDistanceFromMyCastle = {maxDistanceFromMyCastle}";
        }
    }
}