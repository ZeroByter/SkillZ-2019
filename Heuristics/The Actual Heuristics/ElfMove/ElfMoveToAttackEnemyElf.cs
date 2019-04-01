using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfMoveToAttackEnemyElf : Heuristic
    {
        private float radius;

        public ElfMoveToAttackEnemyElf(float weight, float radius) : base(weight)
        {
            this.radius = radius;
        }

        private float GetEnemyElfScore(VirtualGame virtualGame, Elf myElf)
        {
            if (Constants.GameCaching.GetEnemyElvesInArea(new Circle(myElf, Constants.Game.ElfAttackRange)).Count > 0) return 0;

            //The future location of the given elf
            FutureLocation elfFutureLocation = virtualGame.GetFutureLocation(myElf);
            //Get all enemy elves in area near the future location elf within `radius`
            List<Elf> enemyElves = Constants.GameCaching.GetEnemyElvesInArea(new Circle(elfFutureLocation.GetFutureLocation(), radius));
            if (enemyElves.Count == 0) return 0; //if there are no enemy elves, return 0

            Dictionary<int, GameObject> myElves = new Dictionary<int, GameObject>();

            float enemyCombinedHealth = 0;
            float minimumDistance = radius;

            foreach (Elf enemyElf in enemyElves)
            {
                //add combined health of enemy elf
                enemyCombinedHealth += enemyElf.CurrentHealth;

                //get distance from enemyElf to my elf's future location
                float distanceToMyElf = enemyElf.DistanceF(elfFutureLocation.GetFutureLocation());

                //if the distance is less than the minimum distance, set the minimum distance
                if (distanceToMyElf < minimumDistance)
                {
                    minimumDistance = distanceToMyElf;
                }

                //add all of my elves within radius of enemy elf based on future location
                foreach (GameObject elf in enemyElf.GetMyLivingElvesInAreaBasedOnFutureLocation(virtualGame, radius))
                {
                    myElves[elf.UniqueId] = elf;
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
                        return radius - minimumDistance;
                    }
                }

                //if we didn't find any elves on our castle side, return 0
                return 0;
            }

            return (radius - minimumDistance) * ourCombinedHealth / enemyCombinedHealth;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (Elf myElf in Constants.GameCaching.GetMyLivingElves())
            {
                score += GetEnemyElfScore(virtualGame, myElf);
            }

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}

//- We should consider calculating the enemy health only if its turnsToBuild<2 (only remember this but don't implement it)

/*- ElfMoveToAttackEnemyElf
  My Elves going towards enemy elf to attack it. implementation:
  members: radius - the distance from enemy elf which it is worth to start chasing. (I suggest to start with elfMaxSpeed*6)
  score: 
    for each elf:
	  if there is an enemy elf in attack range return zero score
	  if the health of all the enemy elves is smaller than 
	  the health of my elf + my elves that are also near enemy elves then take my elf towards the nearest enemy elf.
	  otherwise return zero.
	  if the health is equal then it depends if there is enemy elf which is closer to my castle than enemy castle
*/
