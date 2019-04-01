using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class TotalLavaGiantAttackBasedOnEnemyCastleHealth : Heuristic
    {
        public TotalLavaGiantAttackBasedOnEnemyCastleHealth(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            return virtualGame.GetCombinedDamageToEnemyCastle() / Constants.Game.GetEnemyCastle().CurrentHealth;
        }
    }
}

/*- num of total attacks / enemy castle current health 
  num of total attacks: The amounts of health that all lava giants will reduce from the enemy castle if no enemy game object will hit it.
                        all lava giants: existing lava giants, already sommoned lava giants, virtual lava giants*/
