using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class PortalSummonIceTrollToDefendPortalAgainstTornadoesIfKillsForSure : Heuristic
    {
        public float protectionRadius;

        public PortalSummonIceTrollToDefendPortalAgainstTornadoesIfKillsForSure(float weight, float protectionRadius) : base(weight)
        {
            this.protectionRadius = protectionRadius;
        }

        
        private float GetVirtualIceTrollScore(VirtualIceTroll virtualIceTroll)
        {
            Circle circle = new Circle(virtualIceTroll.location, protectionRadius);
            
            if (Constants.GameCaching.GetEnemyLavaGiantsInArea(circle).Count > 0) return 0;
            foreach (IceTroll myIceTroll in Constants.GameCaching.GetMyIceTrollsInArea(circle))
            {
                if (myIceTroll.CurrentHealth > 8) return 0;
            }

            float numOfHits = 0;
            foreach (Tornado enemyTornado in Constants.GameCaching.GetEnemyTornadoesInArea(circle))
            {
                float enemyTornadoDistFromPortal = virtualIceTroll.location.Distance(enemyTornado);
                if (Constants.GameCaching.GetEnemyIceTrollsInArea(new Circle(virtualIceTroll.location, enemyTornadoDistFromPortal)).Count > 0) return 0;
                if (Constants.GameCaching.GetMyClosestPortalToEnemyTornado(enemyTornado).Distance(enemyTornado) < enemyTornadoDistFromPortal - 1) continue;

                if (enemyTornadoDistFromPortal < Constants.Game.TornadoMaxSpeed * 8) return 0;
                float numOfStepsToPortal = Mathf.Max(0, (enemyTornadoDistFromPortal - Constants.Game.PortalSize - Constants.Game.TornadoAttackRange) / Constants.Game.TornadoMaxSpeed);
                if (numOfStepsToPortal < enemyTornado.CurrentHealth)
                {
                    numOfHits += (enemyTornado.CurrentHealth - numOfStepsToPortal);
                }
            }

            if(numOfHits <= virtualIceTroll.creator.CurrentHealth && numOfHits > 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (VirtualIceTroll virtualIceTroll in virtualGame.futureIceTrolls.Values)
            {
                score += GetVirtualIceTrollScore(virtualIceTroll);
            }

            return score;
        }
    }
}