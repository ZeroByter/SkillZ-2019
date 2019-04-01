using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    class ElfBuildManaFountain : Heuristic
    {
        private int maxManaFountains;
        private float minDistanceFromEnemyElves;
        private float minDistanceFromEnemyPortal;
        private float minDistanceFromEnemyTornado;
        private float maxDistanceFromCastle;

        public ElfBuildManaFountain(
            float weight, int maxManaFountains, 
            float minDistanceFromEnemyElves,
            float minDistanceFromEnemyPortal,
            float minDistanceFromEnemyTornado,
            float maxDistanceFromCastle) : base(weight)
        {
            this.maxManaFountains = maxManaFountains;
            this.minDistanceFromEnemyElves = minDistanceFromEnemyElves;
            this.minDistanceFromEnemyPortal = minDistanceFromEnemyPortal;
            this.minDistanceFromEnemyTornado = minDistanceFromEnemyTornado;
            this.maxDistanceFromCastle = maxDistanceFromCastle;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            int numberOfFutureManaFountains = virtualGame.futureManaFountains.Count;
            if (numberOfFutureManaFountains == 0) return 0;

            int numberOfManaFountains = Constants.GameCaching.GetMyManaFountains().Length;
            if (numberOfManaFountains >= maxManaFountains) return 0;

            foreach (var buildLocationPair in virtualGame.futureManaFountains)
            {
                if (buildLocationPair.Value.location.Distance(Constants.Game.GetMyCastle()) > maxDistanceFromCastle)
                {
                    numberOfFutureManaFountains--;
                    continue;
                }

                Location manaFountainLocation = buildLocationPair.Value.location;
                //build only near my castle
                //if (buildLocationPair.Value.location.Distance(game.GetMyCastle()) > game.CastleSize + 10 * game.ManaFountainSize) return 0;
                foreach (Elf enemyElf in Constants.GameCaching.GetAllEnemyElves())
                {
                    Location enemyElfLocation;
                    if (enemyElf.IsAlive())
                    {
                        enemyElfLocation = enemyElf.GetLocation();
                    }
                    else
                    {
                        enemyElfLocation = enemyElf.InitialLocation;
                    }

                    int distanceToBuildLocation = manaFountainLocation.Distance(enemyElfLocation);

                    if (distanceToBuildLocation < minDistanceFromEnemyElves)
                    {
                        return 0;
                    }
                }

                if(Constants.GameCaching.GetEnemyPortalsInArea(new Circle(manaFountainLocation, minDistanceFromEnemyPortal)).Count > 0)
                {
                    return 0;
                }
                if (Constants.GameCaching.GetEnemyTornadoesInArea(new Circle(manaFountainLocation, minDistanceFromEnemyTornado)).Count > 0)
                {
                    return 0;
                }
            }

            float score = Mathf.Min(maxManaFountains - numberOfManaFountains, numberOfFutureManaFountains);

            return score * Utilities.GetManaFountainRatio();
        }
    }
}

/*The objective of this heuristic is to build mana fountains when we are far away enough from enemy elves or their initial locations
 * Members: minDistanceFromEnemyElves - the minimum distance we need to have from any enemy elf in order to build the mana fountain
 * Scoring: for each future mana fountain:
 *              for each enemyElf:
 *                  if enemyElf is within minimum distance, return 0;
 *          else, return 1f - number of my mana fountains * 0.25f (so we should only have at most 4 mana fountains)*/
