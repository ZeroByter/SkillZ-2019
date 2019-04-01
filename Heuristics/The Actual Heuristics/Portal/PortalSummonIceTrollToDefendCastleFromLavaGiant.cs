using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonIceTrollToDefendCastleAgainstLavaGiant : Heuristic
    {

        private float minCastleHealth;
        private int radius;

        public PortalSummonIceTrollToDefendCastleAgainstLavaGiant(float weight, float minCastleHealth, int radius) : base(weight)
        {
            this.minCastleHealth = minCastleHealth;
            this.radius = radius;
        }

        public int GetLavaGiantPotentialDamage(Location futureTrollLocation)
        {
            int sum = 0;

            //foreach (LavaGiant giant in Constants.GameCaching.GetEnemyLavaGiantsInArea(new Circle(Constants.Game.GetMyCastle(), futureTrollLocation.Distance(Constants.Game.GetMyCastle()) * 2)))
            foreach (LavaGiant giant in Constants.GameCaching.GetEnemyLavaGiants())
            {
                int turnsOfAttack = giant.CurrentHealth / Constants.Game.LavaGiantSuffocationPerTurn - giant.TimeToArrive(Constants.Game.GetMyCastle(), true);
                sum += turnsOfAttack * giant.AttackMultiplier;
            }

            return sum;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (VirtualIceTroll troll in virtualGame.futureIceTrolls.Values)
            {
                if ((Constants.Game.GetMyCastle().CurrentHealth - GetLavaGiantPotentialDamage(troll.creator.GetLocation())) < minCastleHealth)
                {
                    score += troll.location.Distance(Constants.Game.GetMyCastle());
                }
            }
            
            return score;
        }
    }
}

/*The goal is that if enemy elf gets too close to my portal I will have more power there than my enemy.
So, the scoring is for each enemy elf and my portal that are close to one another.
My portal should be protected only by ice trolls (my elf should not be considered even though they might decide to help due to other heuristics)
Once an enemy elf enters my portal circle we take into account the elves, ice trolls and portals that he brings with him
Notice that we don't want to waist power, so the goal is to have a bit more power.
Notice that we consider what will be our chances to win this battle if we add virtual ice trolls.
  If adding the virtual ice doesn't help a lot then the score for that will be low.


members:
  radiusToDefend - the radius from my portal that if an enemy elf gets with in it should be answered with enough power
  
score:
    for each portal and enemy with in radiusToDefend distance:
        defence circle:
            location: the middle location between the portal and the enemy elf
            radius: radiusToDefend
        current defending health: summation of the health of all my ice trolls with in the defence circle.
        enemy attacking health: summation of the health of all the enemy elves and ice trolls that are in the defence circle + for each enemy portal add the max health of ice troll
	 
            if my current defending health is > than the enemy attacking health return 0
            otherwise:
            virtual defending health: for each virtual ice troll with in the defence circle add max health of ice troll
                score: Math.max(2, (current defending health + virtual defending health) / enemy attacking health)
*/
