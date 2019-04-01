using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfDontStopAttackOnEnemyPortalIfWontDie : Heuristic
    {
        private int minimumLavaGiantDamageToMyCastle;

        public ElfDontStopAttackOnEnemyPortalIfWontDie(float weight, int minimumLavaGiantDamageToMyCastle) : base(weight)
        {
            this.minimumLavaGiantDamageToMyCastle = minimumLavaGiantDamageToMyCastle;
        }

        private List<Elf> GetAllElvesInRange(List<GameObject> elves, Location source)
        {
            List<Elf> returnList = new List<Elf>();

            foreach(GameObject elf in elves)
            {
                if(elf.InRange(source, Constants.Game.ElfAttackRange + ((Elf)elf).MaxSpeed * 2))
                {
                    returnList.Add((Elf)elf);
                }
            }

            return returnList;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (var pair in virtualGame.attackedToAttackersList)
            {
                if(pair.Key is Portal)
                {
                    Portal enemyPortal = (Portal)pair.Key;

                    int lavaGiantDamageToMyCastle = enemyPortal.PredictLavaGiantDamageDoneToCastleIfNotHitByEnemy(Constants.Game.GetMyCastle());

                    if (lavaGiantDamageToMyCastle >= minimumLavaGiantDamageToMyCastle)
                    {
                        foreach(GameObject myElf in pair.Value)
                        {
                            int healthChange = myElf.HealthDifference();

                            if(healthChange == 0)
                            {
                                score++;
                            }
                            else
                            {
                                int timeToDeath = myElf.CurrentHealth / healthChange;
                                int timeToFinishDestroyingPortal = enemyPortal.CurrentHealth / ((Elf)myElf).AttackMultiplier;

                                if(timeToFinishDestroyingPortal <= timeToDeath) //if we will destroy the portal before we will die, we add a score. i am including <= because I think if we can destroy the portal, than its better to die along with it instead of contiuning to play with one health or very low health
                                {
                                    score++;
                                }
                            }
                        }
                    }
                }
            }

            return score;
        }
    }
}