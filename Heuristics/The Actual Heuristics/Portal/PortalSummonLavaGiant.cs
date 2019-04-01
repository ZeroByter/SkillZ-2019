using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonLavaGiant : Heuristic
    {
        private int minimumDamageOutput = 0;

        public PortalSummonLavaGiant(float weight, int minimumDamageOutput) : base(weight)
        {
            this.minimumDamageOutput = minimumDamageOutput;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            Castle enemyCastle = Constants.Game.GetEnemyCastle();

            foreach (KeyValuePair<int, VirtualLavaGiant> pair in virtualGame.futureLavaGiants)
            {
                int damageToCastle = Utilities.PredictFutureDamage(pair.Value.location, enemyCastle, Constants.Game.LavaGiantAttackRange, Constants.Game.LavaGiantMaxSpeed, Constants.Game.LavaGiantAttackMultiplier, Constants.Game.LavaGiantMaxHealth, Constants.Game.LavaGiantSuffocationPerTurn);

                if (damageToCastle > minimumDamageOutput) score++;
            }

            return score;
        }
    }
}
