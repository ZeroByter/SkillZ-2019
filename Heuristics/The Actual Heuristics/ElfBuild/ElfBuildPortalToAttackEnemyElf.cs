using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildPortalToAttackEnemyElf : Heuristic
    {
        public readonly int maxDistanceFromEnemyElf;
        public readonly int maxDistanceFromMyCastle;

        public ElfBuildPortalToAttackEnemyElf(float weight, int maxDistanceFromEnemyElf, int maxDistanceFromMyCastle) : base(weight)
        {
            this.maxDistanceFromEnemyElf = maxDistanceFromEnemyElf;
            this.maxDistanceFromMyCastle = maxDistanceFromMyCastle;
        }

        private float GetEnemyElfScore(VirtualGame virtualGame, Elf enemyElf)
        {
            Castle myCastle = Constants.Game.GetMyCastle();

            Dictionary<int, VirtualPortal> futurePortalsInArea = virtualGame.GetVirtualPortalsInArea(enemyElf.GetLocation(), maxDistanceFromEnemyElf);
            if (futurePortalsInArea.Count == 0) return 0;

            int enemyDistanceToMyCastle = enemyElf.Distance(myCastle);

            if (enemyElf.InAttackRange(myCastle))
            {
                return futurePortalsInArea.Count;
            }

            int distanceToAttackRange = enemyDistanceToMyCastle - enemyElf.AttackRange - myCastle.Size;

            float distanceFactor = Mathf.InverseLerp(0, maxDistanceFromMyCastle - enemyElf.AttackRange - Constants.Game.PortalSize, maxDistanceFromMyCastle - enemyElf.AttackRange - Constants.Game.PortalSize - distanceToAttackRange);
            
            return distanceFactor * futurePortalsInArea.Count;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach(Elf enemyElf in Constants.GameCaching.GetEnemyElvesInArea(new Circle(Constants.Game.GetMyCastle().GetLocation(), maxDistanceFromMyCastle)))
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

/*Building portal in order to attack enemy elf:
  The scoring is per enemy elf.
  members:
    maxDistanceFromEnemyElf - the maximum distance of virtual portal from the enemy elf.
	maxDistanceFromMyCastle - the maximum distance of enemy elf from my castle in order to consider putting a portal against it.
  score: The scoring is per enemy elf.
    for each virtual portal:
	 if enemy elf can attack my castle 1.
     otherwise: do linear calculation over how close the enemy elf is to attack range in relation to maxDistanceFromMyCastle.*/
