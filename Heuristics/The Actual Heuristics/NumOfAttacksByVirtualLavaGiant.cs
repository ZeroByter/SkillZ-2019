using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class NumOfAttacksByVirtualLavaGiant : Heuristic
    {
        private float subtractFromPredictedDamagePercent;
        private float subtractFromPredictedDamage;
        private int minMana;

        public NumOfAttacksByVirtualLavaGiant(float weight, float subtractFromPredictedDamagePercent, int minMana) : base(weight)
        {
            this.subtractFromPredictedDamagePercent = subtractFromPredictedDamagePercent;
            this.subtractFromPredictedDamage = Constants.Game.LavaGiantMaxHealth * subtractFromPredictedDamagePercent;
            this.minMana = minMana;

            if (subtractFromPredictedDamagePercent < 0 || subtractFromPredictedDamagePercent >= 1)
            {
                throw new System.Exception("subtractFromPredictedDamagePercent must be between 0 and 1 (excluding 1)!");
            }
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (Constants.Game.GetEnemyMana() > 100 && (virtualGame.mana * 2 < Constants.Game.GetEnemyMana())) return 0;
            if (virtualGame.mana <= minMana) return 0;

            float futureAttacksCombined = 0;

            foreach (KeyValuePair<int, VirtualLavaGiant> pair in virtualGame.futureLavaGiants)
            {
                futureAttacksCombined += Mathf.Max(0, pair.Value.location.PredictLavaGiantDamageDoneToCastleIfNotHitByEnemy(Constants.Game.GetEnemyCastle()) - subtractFromPredictedDamage);
            }

            return futureAttacksCombined / (Constants.Game.LavaGiantMaxHealth * (1 - subtractFromPredictedDamagePercent));
        }
    }
}

/*- num of future attacks / lavaMaxHealth
    num of future attacks: The amounts of health that the virtual lava giants will reduce from the enemy castle if no enemy game object will hit it*/
