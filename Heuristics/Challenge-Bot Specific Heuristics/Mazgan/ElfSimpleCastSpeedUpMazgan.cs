using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfSimpleCastSpeedUpMazgan : Heuristic
    {
        public ElfSimpleCastSpeedUpMazgan(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            //if (Constants.Game.GetAllMyElves()[0].MaxSpeed > Constants.Game.ElfMaxSpeed)
            //    return -1;

            return virtualGame.futureSpeedUpSpells.Count;
        }
    }
}