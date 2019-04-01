using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonTornadoToAttackPortals : PortalSummonTornadoToAttackEnemyBuilding
    {
        public PortalSummonTornadoToAttackPortals(float weight, float attackRangePercentage) : base(weight, attackRangePercentage) { }

        protected override float GetNumOfBuildingsToAttack(Portal creator)
        {
            //we could probably do this a little smarter.
            //such as, if an elf of ours is in attackRange, we can also check nearby icetrolls, enemy elves, and if the portal in question is nearly done summoning something

            float score = 0;
            Circle portalCircle = new Circle(creator.GetLocation(), Constants.Game.PortalSize + attackRange);
            List<Portal> enemyPortalsInArea = Constants.GameCaching.GetEnemyPortalsInArea(portalCircle);
            foreach (Portal enemyPortal in enemyPortalsInArea)
            {
                if (Constants.GameCaching.GetMyElvesInArea(new Circle(enemyPortal, enemyPortal.Size + Constants.Game.ElfAttackRange)).Count > 0) continue;
                score++;
            }
            int numOfPortalsInBuildingStatus = Constants.GameCaching.GetEnemyElvesInAreaCurrentlyBuildingPortals(portalCircle).Count;
            return score + numOfPortalsInBuildingStatus;
        }
    }
}
