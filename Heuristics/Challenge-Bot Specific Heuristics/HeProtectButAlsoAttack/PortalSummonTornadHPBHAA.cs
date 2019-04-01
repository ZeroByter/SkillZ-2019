using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonTornadHPBHAA : Heuristic
    {
        public PortalSummonTornadHPBHAA(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            if(Constants.Game.Turn <= 35)
            {
                foreach(KeyValuePair<int, VirtualTornado> keyValuePair in virtualGame.futureTornadoes)
                {
                    VirtualTornado tornado = keyValuePair.Value;
                    if (Constants.GameCaching.GetMyTornadoes().Length != 0)
                        score += tornado.location.Distance(Constants.GameCaching.GetMyTornadoes().GetClosest(tornado.location));
                    else
                        score += 1;
                }
            }

            return score;
        }
    }
}