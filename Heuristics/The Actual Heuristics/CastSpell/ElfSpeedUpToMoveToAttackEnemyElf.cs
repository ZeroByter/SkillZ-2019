using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfSpeedUpToMoveToAttackEnemyElf : Heuristic
    {
        private float radius;

        public ElfSpeedUpToMoveToAttackEnemyElf(float weight, float radius) : base(weight)
        {
            this.radius = radius;
        }

        private float GetVirtualSpeedUpScore(VirtualSpeedUp virtualSpeed)
        {
            Elf myElf = (Elf)virtualSpeed.creator;
            List<Elf> enemyElves = Constants.GameCaching.GetEnemyElvesInArea(new Circle(myElf.GetLocation(), radius));
            if (enemyElves.Count == 0) return 0; //if there are no enemy elves, return 0

            Dictionary<int, GameObject> myElves = new Dictionary<int, GameObject>();

            float enemyCombinedHealth = 0;

            foreach (Elf enemyElf in enemyElves)
            {
                if (enemyElf.InAttackRange(myElf))
                {
                    return 0;
                }
                else 
                {
                    enemyCombinedHealth += enemyElf.CurrentHealth;
                    
                    foreach (Elf elf in Constants.GameCaching.GetMyElvesInArea(new Circle(enemyElf.GetLocation(), radius)))
                    {
                        myElves[elf.UniqueId] = elf;
                    }
                }
            }

            float ourCombinedHealth = myElf.CurrentHealth;

            //add all combined health of our elves
            foreach (KeyValuePair<int, GameObject> pair in myElves)
            {
                if (myElf.UniqueId == pair.Key) continue;
                ourCombinedHealth += pair.Value.CurrentHealth;
            }

            //if we are weaker than the enemy
            if (ourCombinedHealth < enemyCombinedHealth)
            {
                return 0;
            }
            //if we are equal in power to the enemy
            else if (ourCombinedHealth == enemyCombinedHealth)
            {
                //go through each enemy elf
                foreach (GameObject enemyElf in enemyElves)
                {
                    //if the enemy elf is on our castle side
                    if (enemyElf.OnSameSideAsCastle()) //if enemy elf is our side of the map
                    {
                        return 1;
                    }
                }

                //if we didn't find any elves on our castle side, return 0
                return 0;
            }

            return ourCombinedHealth / enemyCombinedHealth;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (VirtualSpeedUp virtualSpeedUp in virtualGame.futureSpeedUpSpells.Values)
            {
                score += GetVirtualSpeedUpScore(virtualSpeedUp);
            }

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}
