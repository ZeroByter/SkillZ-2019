using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveToVolcano : Heuristic
    {
        public ElfMoveToVolcano(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            Volcano volcano = Constants.Game.GetVolcano();

            if (volcano.DamageByEnemy > volcano.MaxHealth / 2) return 0;
            if (!volcano.IsActive()) return 0;

            foreach (KeyValuePair<int, FutureLocation> pair in virtualGame.GetFutureLocations())
            {
                Location elfNextLocation = pair.Value.GetFutureLocation();

                //to ensure we wont get too close to the volcano we are trying to kill
                if (elfNextLocation.Distance(volcano) < volcano.Size + Constants.Game.ElfAttackRange - 50) continue;

                float distance = elfNextLocation.Distance(volcano);

                score -= distance / Constants.Game.ElfMaxSpeed;
            }
            
            //optional route if we want to always go to enemy volcano, but deprioritize it if its inactive
            //if (!volcano.IsActive()) score *= 0.5f;

            return score;
        }
    }
}
