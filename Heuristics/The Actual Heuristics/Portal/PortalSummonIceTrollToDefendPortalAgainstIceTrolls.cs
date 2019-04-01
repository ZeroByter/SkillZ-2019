using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonIceTrollToDefendPortalAgainstIceTrolls : Heuristic
    {
        public int protectionRadius;

        public PortalSummonIceTrollToDefendPortalAgainstIceTrolls(float weight, int protectionRadius) : base(weight)
        {
            this.protectionRadius = protectionRadius;
        }

        private float GetPortalScore(VirtualIceTroll vIceTroll)
        {
            float score = 0;

            foreach(IceTroll iceTroll in Constants.Game.GetEnemyIceTrolls())
            {
                float distance = iceTroll.Distance(vIceTroll.location);

                if(distance <= protectionRadius)
                {
                    score += distance;
                }
            }

            return score / Constants.Game.IceTrollMaxSpeed;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (KeyValuePair<int, VirtualIceTroll> pair in virtualGame.futureIceTrolls)
            {
                score += GetPortalScore(pair.Value);
            }

            return score;
        }
    }
}
