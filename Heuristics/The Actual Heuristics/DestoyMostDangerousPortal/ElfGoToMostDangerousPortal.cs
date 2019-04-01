using ElfKingdom;

namespace SkillZ.IndividualHeuristics
{
    class ElfGoToMostDangerousPortal : Heuristic
    {
        int numberOfLavaGiantsSpawnedForInstantGoto;

        public ElfGoToMostDangerousPortal(float weight, int numberOfLavaGiantsSpawnedForInstantGoto) : base(weight)
        {
            this.numberOfLavaGiantsSpawnedForInstantGoto = numberOfLavaGiantsSpawnedForInstantGoto;
        }

        private Portal GetMostDangerousPortal()
        {
            float mostDangerousPortalScore = 0;
            Portal mostDangerousPortal = null;
            float currentPortalScore = 0;

            foreach (Portal enemyPortal in Constants.GameCaching.GetEnemyPortals())
            {
                int lavaGiantsSpawned = TrackPortalCreations.GetPortalLavaGiantsCount(enemyPortal);
                currentPortalScore = enemyPortal.PredictLavaGiantDamageDoneToCastleIfNotHitByEnemy(Constants.Game.GetMyCastle()) + (enemyPortal.CurrentlySummoning == "LavaGiant" ? 10 : 0);
                currentPortalScore += lavaGiantsSpawned * 200;
                if (lavaGiantsSpawned >= numberOfLavaGiantsSpawnedForInstantGoto) currentPortalScore *= 4f;
                if (currentPortalScore > mostDangerousPortalScore)
                {
                    mostDangerousPortalScore = currentPortalScore;
                    mostDangerousPortal = enemyPortal;
                }
            }

            return mostDangerousPortal;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;
            
            Portal mostDangerousPortal = GetMostDangerousPortal();

            if (mostDangerousPortal == null) return 0;

            foreach (FutureLocation elvesLocations in virtualGame.GetFutureLocations().Values)
            {
                score -= elvesLocations.GetFutureLocation().Distance(mostDangerousPortal);

                float speedUpScore = (elvesLocations.GetElf().HasSpeedUp() ? 1 : 0) * elvesLocations.GetFutureLocation().Distance(mostDangerousPortal) * weight / 2;
                if (elvesLocations.GetFutureLocation().Distance(mostDangerousPortal) > 1500) score += speedUpScore;
            }

            return score / Constants.Game.ElfMaxSpeed;
        }
    }
}
