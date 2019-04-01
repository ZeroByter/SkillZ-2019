using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    /// <summary>
    /// Elf will not build a portal (get -1 score) if we calculate he will die before he finishes the portal
    /// </summary>
    class ElfDontBuildPortalWhileBeingAttackedAndMightNotFinish : Heuristic
    {
        public ElfDontBuildPortalWhileBeingAttackedAndMightNotFinish(float weight) : base(weight)
        {
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            foreach (KeyValuePair<int, VirtualPortal> pair in virtualGame.futurePortals)
            {
                Elf elf = (Elf)pair.Value.creator;

                //first we get the health difference from last turn to this turn
                int healthChange = elf.HealthDifference();

                List<IceTroll> threatningIceTrolls = new List<IceTroll>();
                if (threatningIceTrolls.Count > 0)
                {
                    foreach (IceTroll enemyIceTroll in threatningIceTrolls)
                    {
                        healthChange -= enemyIceTroll.PredictedDamageDoneToTarget(elf);
                    }
                }

                //if we are losing health
                if (healthChange < 0)
                {
                    int timeToDie = elf.CurrentHealth / healthChange;

                    //if we will die before we complete the portal we want to build
                    if (timeToDie <= Constants.Game.PortalBuildingDuration)
                    {
                        return -1;
                    }
                }
            }

            return 0;
        }
    }
}
