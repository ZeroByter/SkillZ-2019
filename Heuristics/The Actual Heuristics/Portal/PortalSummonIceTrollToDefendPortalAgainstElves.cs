using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonIceTrollToDefendPortalAgainstElves : Heuristic
    {
        public float protectionRadius;
        private int turnsToReviveBiggerThan;
        private Circle monitoredCircle;

        public PortalSummonIceTrollToDefendPortalAgainstElves(float weight, float protectionRadius, int turnsToReviveBiggerThan, Circle monitoredCircle) : base(weight)
        {
            this.protectionRadius = protectionRadius;
            this.turnsToReviveBiggerThan = turnsToReviveBiggerThan;
            this.monitoredCircle = monitoredCircle;
        }

        private int GetDefenceHealth(Circle circle)
        {
            int total = 0;

            foreach (IceTroll iceTroll in Constants.GameCaching.GetMyIceTrolls())
            {
                if (circle.IsLocationInside(iceTroll)) total += iceTroll.CurrentHealth;
            }

            return total;
        }

        private int GetVirtualDefenceHealth(VirtualGame virtualGame, Circle circle)
        {
            int total = 0;

            foreach (KeyValuePair<int, VirtualIceTroll> pair in virtualGame.GetVirtualIceTrollsInArea(circle.GetCenter(), circle.GetRadius()))
            {
                total += Constants.Game.IceTrollMaxHealth;
            }

            return total;
        }

        private int GetOffenseHealth(Circle circle, Portal portal)
        {
            int total = 0;

            foreach (IceTroll iceTroll in Constants.GameCaching.GetEnemyIceTrollsInArea(circle))
            {
                total += iceTroll.CurrentHealth;
            }

            foreach (Elf elf in Constants.GameCaching.GetAllEnemyElves())
            {
                if (elf.TurnsToRevive == 0)
                {
                    if (circle.IsLocationInside(elf) && elf.IsHeadingTowards(portal, 0.8f)) total += elf.CurrentHealth;
                }
                else
                {
                    if (elf.InitialLocation.Distance(circle.GetCenter()) > protectionRadius) continue;
                    if (elf.TurnsToRevive > turnsToReviveBiggerThan) continue;
                    if (circle.IsLocationInside(elf.InitialLocation)) total += Constants.Game.ElfMaxHealth;
                }
            }

            foreach (Portal enemyPortal in Constants.GameCaching.GetEnemyPortalsInArea(circle))
            {
                if (enemyPortal.CurrentlySummoning == "IceTroll") total += Constants.Game.IceTrollMaxHealth;
            }

            return total;
        }

        private float GetPortalScore(VirtualGame virtualGame, Portal portal)
        {
            float score = 0;

            foreach (Elf enemyElf in Constants.Game.GetAllDeadButAlmostRevivedElvesInArea(new Circle(portal.GetLocation(), protectionRadius), turnsToReviveBiggerThan))
            {
                float enemyElfDistFromPortal = enemyElf.InitialLocation.DistanceF(portal);

                Circle protectionCircle = new Circle(portal.GetLocation().Lerp(enemyElf.InitialLocation, 0.5f), enemyElfDistFromPortal);

                float defenseHealth = GetDefenceHealth(protectionCircle);
                float offenseHealth = GetOffenseHealth(protectionCircle, portal);

                if (defenseHealth > offenseHealth)
                {
                    continue;
                }
                else
                {
                    defenseHealth += GetVirtualDefenceHealth(virtualGame, protectionCircle);

                    score += Mathf.Min(2, defenseHealth / offenseHealth);
                }
            }

            foreach (Elf enemyElf in Constants.GameCaching.GetEnemyElvesInArea(new Circle(portal.GetLocation(), protectionRadius)))
            {
                float enemyElfDistFromPortal = enemyElf.DistanceF(portal);

                Circle protectionCircle = new Circle(portal.GetLocation().Lerp(enemyElf, 0.5f), enemyElfDistFromPortal);

                float defenseHealth = GetDefenceHealth(protectionCircle);
                float offenseHealth = GetOffenseHealth(protectionCircle, portal);

                if (defenseHealth > offenseHealth)
                {
                    continue;
                }
                else
                {
                    defenseHealth += GetVirtualDefenceHealth(virtualGame, protectionCircle);

                    score += Mathf.Min(2, defenseHealth / offenseHealth);
                }
            }

            return score;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            if(monitoredCircle == null)
            {
                foreach (Portal portal in Constants.GameCaching.GetMyPortals())
                {
                    score += GetPortalScore(virtualGame, portal);
                }
            }
            else
            {
                foreach (Portal portal in Constants.GameCaching.GetMyPortalsInArea(monitoredCircle))
                {
                    score += GetPortalScore(virtualGame, portal);
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
