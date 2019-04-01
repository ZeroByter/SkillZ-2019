using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfLeadChasingElfToEnemyCastle : Heuristic
    {
        private float maxChasingDistance;
        private float enemyCastleProximity;

        public ElfLeadChasingElfToEnemyCastle(float weight, float maxChasingDistance, float enemyCastleProximity) : base(weight)
        {
            this.maxChasingDistance = maxChasingDistance;
            this.enemyCastleProximity = enemyCastleProximity;
        }

        //this causes us the long execution. This is the only way I can think of getting the chasing enemy elf
        private Elf GetChasingEnemyElf(Elf myElf)
        {
            Elf closestEnemyElf = (Elf)Constants.GameCaching.GetEnemyElvesInArea(new Circle(myElf, maxChasingDistance)).ToArray().GetClosest(myElf);

            if(closestEnemyElf != null && closestEnemyElf.DistanceF(myElf) <= maxChasingDistance)
            {
                if (closestEnemyElf.CurrentHealth >= myElf.CurrentHealth && closestEnemyElf.IsHeadingTowards(myElf))
                {
                    return closestEnemyElf;
                }
            }

            return null;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            Castle enemyCastle = Constants.Game.GetEnemyCastle();

            foreach (FutureLocation pair in virtualGame.GetFutureLocations().Values)
            {
                Elf chasingElf = GetChasingEnemyElf(pair.GetElf());

                if(chasingElf != null)
                {
                    if(pair.GetElf().DistanceF(enemyCastle) < enemyCastleProximity)
                    {
                        if (!pair.GetElf().InAttackRange(chasingElf))
                        {
                            Location elfNextLocation = pair.GetFutureLocation();

                            score -= elfNextLocation.Distance(chasingElf); //move to chasing enemy elf
                        }
                    }
                    else
                    {
                        Location elfNextLocation = pair.GetFutureLocation();

                        score -= elfNextLocation.Distance(enemyCastle); //move to enemy castle
                    }
                }
            }

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}
