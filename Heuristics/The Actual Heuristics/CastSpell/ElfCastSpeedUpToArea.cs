using ElfKingdom;
using System.Collections.Generic;
using System.Linq;

namespace SkillZ.IndividualHeuristics
{
    class ElfCastSpeedUpToArea : Heuristic
    {
        private Circle area;

        public ElfCastSpeedUpToArea(float weight, Circle area) : base(weight)
        {
            this.area = area;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (virtualGame.futureSpeedUpSpells.Count == 0) return 0;
            if (Constants.GameCaching.GetMyPortals().Length < 1) return 0;
            if (Constants.Game.GetMyMana() <= Constants.Game.LavaGiantCost) return 0;

            float score = 0;

            foreach(var pair in virtualGame.futureSpeedUpSpells)
            {
                if (!pair.Value.realGameObject.IsHeadingTowards(area.GetCenter(), 0.5f)) continue;

                score += pair.Value.realGameObject.DistanceF(area.GetCenter());
            }

            //The reason I normalize this is by ElfMaxSpeed is so that the score would mimic that of MoveTo heuristics so that the final weighted score would be around the same
            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}