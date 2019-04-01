using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalDontSummonIfWontBeAbleToFinish : Heuristic
    {
        public PortalDontSummonIfWontBeAbleToFinish(float weight, int minimumMana) : base(weight)
        {
        }

        private int GetScoreFinalCount()
        {
            return 0;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (var virtualLavaGiant in virtualGame.futureLavaGiants.Values)
            {
                Portal portal = (Portal)virtualLavaGiant.creator;

                int healthDiff = portal.HealthDifference();

                if (portal.CurrentHealth / healthDiff <= Constants.Game.LavaGiantSummoningDuration) score--;
            }

            foreach (var virtualIceTroll in virtualGame.futureIceTrolls.Values)
            {
                Portal portal = (Portal)virtualIceTroll.creator;

                int healthDiff = portal.HealthDifference();

                if (portal.CurrentHealth / healthDiff <= Constants.Game.IceTrollSummoningDuration) score--;
            }

            foreach (var virtualTornadoes in virtualGame.futureTornadoes.Values)
            {
                Portal portal = (Portal)virtualTornadoes.creator;

                int healthDiff = portal.HealthDifference();

                if (portal.CurrentHealth / healthDiff <= Constants.Game.TornadoSummoningDuration) score--;
            }

            return score * Mathf.Min(1, GetScoreFinalCount());
        }
    }
}
