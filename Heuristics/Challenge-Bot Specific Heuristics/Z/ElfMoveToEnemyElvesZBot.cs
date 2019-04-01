using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveToEnemyElvesZBot : Heuristic
    {
        public ElfMoveToEnemyElvesZBot(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (Constants.Game.Turn < 127) return 0;
            if (Constants.Game.Turn > 137) return 0;

            float score = 0;

            Elf[] enemyElves = Constants.GameCaching.GetEnemyLivingElves();
            Dictionary<int, FutureLocation> myElvesLocations = virtualGame.GetFutureLocations();

            foreach (Elf enemyElf in enemyElves)
            {
                foreach (FutureLocation elfLocation in myElvesLocations.Values)
                {
                    score -= elfLocation.GetFutureLocation().Distance(enemyElf);
                }
            }

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}
