using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalDontSummonLavaGiantIfWeAreLessPowerful : Heuristic
    {
        private float portalsWeight;
        private float iceTrollsWeight;
        private float tornadoesWeight;
        private float manaFountainsWeight;
        private float manaWeight;

        public PortalDontSummonLavaGiantIfWeAreLessPowerful(float weight, float portalsWeight, float iceTrollsWeight, float tornadoesWeight, float manaFountainsWeight, float manaWeight) : base(weight)
        {
            this.portalsWeight = portalsWeight;
            this.iceTrollsWeight = iceTrollsWeight;
            this.tornadoesWeight = tornadoesWeight;
            this.manaFountainsWeight = manaFountainsWeight;
            this.manaWeight = manaWeight;
        }

        public float GetEnemyCombinedPower()
        {
            float score = 0;

            score += Constants.GameCaching.GetEnemyPortals().Length * portalsWeight;
            score += Constants.GameCaching.GetEnemyIceTrolls().Length * iceTrollsWeight;
            score += Constants.GameCaching.GetEnemyTornadoes().Length * tornadoesWeight;
            score += Constants.GameCaching.GetEnemyManaFountains().Length * manaFountainsWeight;
            if(Constants.GameCaching.GetEnemyLivingElves().Length > 0) score += Constants.Game.GetEnemyMana() * manaWeight;

            return score;
        }

        public float GetMyCombinedPower()
        {
            float score = 0;

            score += Constants.GameCaching.GetMyPortals().Length * portalsWeight;
            score += Constants.GameCaching.GetMyIceTrolls().Length * iceTrollsWeight;
            score += Constants.GameCaching.GetMyTornadoes().Length * tornadoesWeight;
            score += Constants.GameCaching.GetMyManaFountains().Length * manaFountainsWeight;
            if (Constants.GameCaching.GetMyLivingElves().Length > 0) score += Constants.Game.GetMyMana() * manaWeight;

            return score;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float enemyCombinedPower = GetEnemyCombinedPower();
            float myCombinedPower = GetMyCombinedPower();

            if(myCombinedPower < enemyCombinedPower)
            {
                return myCombinedPower / enemyCombinedPower * virtualGame.futureLavaGiants.Count * -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
