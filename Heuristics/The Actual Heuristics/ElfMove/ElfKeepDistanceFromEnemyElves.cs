using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfKeepDistanceFromEnemyElves : Heuristic
    {
        private float radius;
        private float radiusWhenDontHaveMuchHealth;
        private int whatIsNotMuchHealth;

        public ElfKeepDistanceFromEnemyElves(float weight, float radius, float radiusWhenDontHaveMuchHealth, int whatIsNotMuchHealth) : base(weight)
        {
            this.radius = radius;
            this.radiusWhenDontHaveMuchHealth = radiusWhenDontHaveMuchHealth;
            this.whatIsNotMuchHealth = whatIsNotMuchHealth;
        }

        private float GetMyElfScore(VirtualGame virtualGame, Elf myElf)
        {
            FutureLocation elfFutureLocation = virtualGame.GetFutureLocation(myElf);
            float useRadius = radius;
            if (myElf.CurrentHealth < whatIsNotMuchHealth) useRadius = radiusWhenDontHaveMuchHealth;

            List<Elf> enemyElves = elfFutureLocation.GetFutureLocation().GetEnemyLivingElvesInArea(useRadius);
            if (enemyElves.Count == 0) return 0;

            Dictionary<int, GameObject> myElves = new Dictionary<int, GameObject>();

            int enemyCombinedHealth = 0;
            float minimumDistance = useRadius;

            foreach (Elf enemyElf in enemyElves)
            {
                enemyCombinedHealth += enemyElf.CurrentHealth;

                float distanceToMyElf = enemyElf.DistanceF(elfFutureLocation.GetFutureLocation());

                if (distanceToMyElf < minimumDistance)
                {
                    minimumDistance = distanceToMyElf;
                }

                foreach (Elf elf in enemyElf.GetMyLivingElvesInAreaBasedOnFutureLocation(virtualGame, useRadius))
                {
                    myElves[elf.UniqueId] = elf;
                }
            }

            float ourCombinedHealth = 0;

            foreach (KeyValuePair<int, GameObject> pair in myElves)
            {
                ourCombinedHealth += pair.Value.CurrentHealth;
            }

            if (ourCombinedHealth == enemyCombinedHealth)
            {
                foreach (GameObject enemyElf in enemyElves)
                {
                    //if (enemyElf.OnSameSideAsCastle()) //if enemy elf is our side of the map
                    if (enemyElf.Distance(myElf) <= Constants.Game.ElfAttackRange || enemyElf.OnSameSideAsCastle())
                    {
                        return 0;
                    }
                }
            }
            else if (ourCombinedHealth > enemyCombinedHealth)
            {
                return 0;
            }

            /*don't round to int.
            We should consider calculating the enemy health only if its turnsToBuild<2 (only remember this but don't implement it)
            There are more differences but I prefer what you implemented. Lets see how it works.*/
            ourCombinedHealth = Mathf.Max(1f, myElf.CurrentHealth - 2);
            float factor = (enemyCombinedHealth / ourCombinedHealth);
            float diffDistFromMyCastle = myElf.GetLocation().Distance(Constants.Game.GetMyCastle()) - elfFutureLocation.GetFutureLocation().Distance(Constants.Game.GetMyCastle());
            if (diffDistFromMyCastle > Constants.Game.ElfMaxSpeed * 0.5f)
            {
                return -1 * useRadius * factor;
            }
            else
            {
                return -1 * (useRadius - minimumDistance) * factor;
            }
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (Elf myElf in Constants.GameCaching.GetMyLivingElves())
            {
                score += GetMyElfScore(virtualGame, myElf);
            }

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}

/*- ElfKeepDistanceFromEnemyElves
  My Elves keeping a distance from enemy elves. implementation:
  members: 
  radius - the distance from enemy elf which it is worth to keep distance from. (I suggest to start with elfMaxSpeed*4.5)
  radiusWhenDontHaveMuchHealth - the distance from enemy elf which it is worth to keep distance from when my elf don't have much health. (I suggest to start with elfMaxSpeed*5.5)
  score: 
    for each elf:
	  if the health of all the enemy elves is bigger than 
	  the health of my elf + my elves that are also near enemy elves then try to get out of the radius.
	  if the health is equal then it depends if there is enemy elf which is closer to my castle than enemy castle.
*/
