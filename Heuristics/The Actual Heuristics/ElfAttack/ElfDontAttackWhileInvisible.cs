using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfDontAttackWhileInvisible : Heuristic
    {
        public ElfDontAttackWhileInvisible(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach(GameObject attacker in virtualGame.GetAllFutureAttacks())
            {
                if(attacker is Elf)
                {
                    Elf elf = (Elf)attacker;

                    if (elf.Invisible)
                    {
                        score--;
                    }
                }
            }

            return score;
        }
    }
}