using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class SaveManaForBalancingManaFountains : Heuristic
    {
        private float distanceOfMyElfFromMyCastle;
        private int turnsToRevive;

        public SaveManaForBalancingManaFountains(float weight, float distanceOfMyElfFromMyCastle, int turnsToRevive) : base(weight)
        {
            this.distanceOfMyElfFromMyCastle = distanceOfMyElfFromMyCastle;
            this.turnsToRevive = turnsToRevive;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (virtualGame.futureManaFountains.Count > 0) return 0;
            if (virtualGame.mana >= Constants.Game.ManaFountainCost) return 0;

            float futureCosts = virtualGame.mana - Constants.Game.GetMyMana();
            if (futureCosts == 0) return 0;

            bool isElfAboutToRevive = false;
            foreach(Elf elf in Constants.GameCaching.GetAllMyElves())
            {
                if(!elf.IsAlive() && elf.TurnsToRevive <= turnsToRevive)
                {
                    isElfAboutToRevive = true;
                    break;
                }
            }

            if(!isElfAboutToRevive &&
                Constants.GameCaching.GetMyElvesInArea(new Circle(Constants.Game.GetMyCastle(), distanceOfMyElfFromMyCastle)).Count == 0)
            {
                return 0;
            }

            float score = futureCosts / Constants.Game.ManaFountainCost;

            return score * Utilities.GetManaFountainRatio();
        }
    }
}
