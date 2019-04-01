using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummoningIceTrollToDefendPortalAgainstTornadoes : Heuristic
    {
        public float protectionRadius;

        public PortalSummoningIceTrollToDefendPortalAgainstTornadoes(float weight, float protectionRadius) : base(weight)
        {
            this.protectionRadius = protectionRadius;
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

            foreach (Tornado tornado in Constants.GameCaching.GetEnemyTornadoesInArea(circle))
            {
                if (!tornado.IsHeadingTowards(portal)) continue;

                total += tornado.CurrentHealth;
            }

            return total;
        }

        private float GetPortalScore(VirtualGame virtualGame, Portal portal)
        {
            float score = 0;

            foreach (Tornado enemyTornado in Constants.GameCaching.GetEnemyTornadoesInArea(new Circle(portal.GetLocation(), protectionRadius)))
            {
                float enemyTornadoDistFromPortal = enemyTornado.DistanceF(portal.GetLocation());
                float numOfStepsToPortal = Mathf.Max(0, (enemyTornadoDistFromPortal - Constants.Game.PortalSize - Constants.Game.TornadoAttackRange) / Constants.Game.TornadoMaxSpeed);
                if(numOfStepsToPortal > enemyTornado.CurrentHealth)
                {
                    continue;
                }

                float protectionRadius = enemyTornadoDistFromPortal - Constants.Game.TornadoMaxSpeed * 2;
                if (protectionRadius <= 0) continue;

                Circle protectionCircle = new Circle(portal.GetLocation(), protectionRadius);

                float defenseHealth = GetDefenceHealth(protectionCircle);
                float offenseHealth = GetOffenseHealth(protectionCircle, portal) + enemyTornado.CurrentHealth;

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

            foreach (Portal portal in Constants.GameCaching.GetMyPortals())
            {
                score += GetPortalScore(virtualGame, portal);
            }

            return score;
        }
    }
}