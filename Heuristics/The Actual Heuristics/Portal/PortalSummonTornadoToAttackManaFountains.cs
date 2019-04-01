using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonTornadoToAttackManaFountains : PortalSummonTornadoToAttackEnemyBuilding
    {
        public PortalSummonTornadoToAttackManaFountains(float weight, float attackRangePercentage) : base(weight, attackRangePercentage) { }

        protected override float GetNumOfBuildingsToAttack(Portal creator)
        {
            var circle = new Circle(creator, Constants.Game.ManaFountainSize + attackRange);
            return Constants.GameCaching.GetEnemyManaFountainsInArea(circle).Count + Constants.GameCaching.GetEnemyElvesInAreaCurrentlyBuildingManaFountains(circle).Count;
        }
    }
}
