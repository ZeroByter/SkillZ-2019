using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalDontSummonIfTornadoNearBy : Heuristic
    {
        private float nearByDist;

        public PortalDontSummonIfTornadoNearBy(float weight, float nearByDist) : base(weight)
        {
            this.nearByDist = nearByDist;
        }

        private float getVirtualCreatureScore(Location virtualCreatureLocation)
        {
            if(Constants.GameCaching.GetEnemyTornadoesInArea(new Circle(virtualCreatureLocation, nearByDist)).Count > 0)
            {
                return -1;
            }
            return 0;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;
            foreach(VirtualIceTroll myCreature in virtualGame.futureIceTrolls.Values)
            {
                score += getVirtualCreatureScore(myCreature.location);
            }
            foreach (VirtualTornado myCreature in virtualGame.futureTornadoes.Values)
            {
                score += getVirtualCreatureScore(myCreature.location);
            }
            foreach (VirtualLavaGiant myCreature in virtualGame.futureLavaGiants.Values)
            {
                score += getVirtualCreatureScore(myCreature.location);
            }


            return score;
        }
    }
}
