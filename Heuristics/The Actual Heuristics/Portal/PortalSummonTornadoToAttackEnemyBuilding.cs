using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    abstract class PortalSummonTornadoToAttackEnemyBuilding : Heuristic
    {
        protected float attackRange;

        public PortalSummonTornadoToAttackEnemyBuilding(float weight, float attackRangePercentage) : base(weight)
        {
            if (attackRangePercentage < 0 || attackRangePercentage > 1)
            {
                throw new System.Exception("attackRangePercentage must be between 0 and 1!");
            }

            int elfMaxDistance = (Constants.Game.TornadoMaxHealth / Constants.Game.TornadoSuffocationPerTurn) * Constants.Game.TornadoMaxSpeed;
            attackRange = elfMaxDistance * attackRangePercentage;
        }

        protected abstract float GetNumOfBuildingsToAttack(Portal creator);

        private float GetPortalScore(Portal creator)
        {
            Circle portalCircle = new Circle(creator.GetLocation(), Constants.Game.PortalSize + attackRange);
            Circle manaFountainCircle = new Circle(creator.GetLocation(), Constants.Game.ManaFountainSize + attackRange);
            Circle largestCircle = new Circle(creator.GetLocation(), Mathf.Max(portalCircle.GetRadius(), manaFountainCircle.GetRadius()));
            List<ManaFountain> enemyManaFountainsInArea = Constants.GameCaching.GetEnemyManaFountainsInArea(manaFountainCircle);
            List<Portal> enemyPortalsInArea = Constants.GameCaching.GetEnemyPortalsInArea(portalCircle);

            int numOfManaFountainsInBuildingStatus = Constants.GameCaching.GetEnemyElvesInAreaCurrentlyBuildingManaFountains(manaFountainCircle).Count;
            int numOfPortalsInBuildingStatus = Constants.GameCaching.GetEnemyElvesInAreaCurrentlyBuildingPortals(portalCircle).Count;
            int numOfEnemyPortalsAndManaFountains = enemyManaFountainsInArea.Count + enemyPortalsInArea.Count + numOfManaFountainsInBuildingStatus + numOfPortalsInBuildingStatus;

            List<Tornado> myTornadoesInArea = Constants.GameCaching.GetMyTornadoesInArea(largestCircle);
            int numOfHealthyTornadoesInArea = 0;
            foreach (Tornado tornado in myTornadoesInArea)
            {
                if (tornado.CurrentHealth >= 8)
                {
                    numOfHealthyTornadoesInArea++;
                }
            }

            if (numOfHealthyTornadoesInArea >= numOfEnemyPortalsAndManaFountains) return 0;
            foreach (Portal myPortal in Constants.GameCaching.GetMyPortalsInAreaCurrentlySummoningTornadoes(portalCircle))
            {
                if (myPortal.UniqueId == creator.UniqueId) continue;
                numOfHealthyTornadoesInArea++;
            }
            if (numOfHealthyTornadoesInArea >= numOfEnemyPortalsAndManaFountains) return 0;

            return GetNumOfBuildingsToAttack(creator);
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (var virtualTornado in virtualGame.futureTornadoes.Values)
            {
                score += GetPortalScore((Portal)virtualTornado.creator);
            }

            return score;
        }
    }
}
