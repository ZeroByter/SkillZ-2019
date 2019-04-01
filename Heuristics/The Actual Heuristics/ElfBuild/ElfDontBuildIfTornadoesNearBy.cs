using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfDontBuildIfTornadoesNearBy : Heuristic
    {
        private float radius;

        public ElfDontBuildIfTornadoesNearBy(float weight, float radius) : base(weight)
        {
            this.radius = radius;
        }


        private float GetLocationScore(Location location)
        {
            Circle circle = new Circle(location, radius);
            if (Constants.GameCaching.GetEnemyTornadoesInArea(circle).Count > 0) return -1;

            if (Constants.GameCaching.GetEnemyPortalsInAreaCurrentlySummoningTornadoes(circle).Count > 0) return -1;

            return 0;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (VirtualPortal portal in virtualGame.futurePortals.Values)
            {
                score += GetLocationScore(portal.location);
            }

            foreach (VirtualManaFountain manaFountain in virtualGame.futureManaFountains.Values)
            {
                score += GetLocationScore(manaFountain.location);
            }

            return score;
        }
    }
}