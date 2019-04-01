using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonIceTrollToStopEnemyElvesFromGoingToMyCastle : Heuristic
    {
        private float maxDistanceFromElf;
        private float closestDistance;
        private int turnsToReviveBiggerThan;
        private Circle monitoredCircle;

        public PortalSummonIceTrollToStopEnemyElvesFromGoingToMyCastle(
            float weight, float maxDistanceFromElf, float closestDistance,
            int turnsToReviveBiggerThan, Circle monitoredCircle) : base(weight)
        {
            this.maxDistanceFromElf = maxDistanceFromElf;
            this.closestDistance = closestDistance;
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
                    if (elf.InitialLocation.Distance(circle.GetCenter()) > maxDistanceFromElf) continue;
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
            Castle myCastle = Constants.Game.GetMyCastle();
            float distanceToMyCastle = portal.Distance(myCastle);

            Circle circle = new Circle(portal.GetLocation(), maxDistanceFromElf);
            foreach (Elf enemyElf in Constants.Game.GetAllDeadButAlmostRevivedElvesInArea(circle, turnsToReviveBiggerThan))
            {
                int enemyElfDistanceToMyCastle = enemyElf.InitialLocation.Distance(myCastle);

                //if the elf distance to our castle minus our distance to our castle is bigger than closestDistance
                if (enemyElfDistanceToMyCastle - distanceToMyCastle < closestDistance)
                {
                    continue;
                }

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

            foreach (Elf enemyElf in Constants.GameCaching.GetEnemyElvesInArea(circle))
            {
                int enemyElfDistanceToMyCastle = enemyElf.Distance(myCastle);

                //if the elf distance to our castle minus our distance to our castle is bigger than closestDistance
                if (enemyElfDistanceToMyCastle - distanceToMyCastle < closestDistance)
                {
                    continue;
                }

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

            if (monitoredCircle == null)
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
