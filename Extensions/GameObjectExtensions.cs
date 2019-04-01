using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ {
    public static class GameObjectExtensions {
        public static bool IsMine(this GameObject gameObject)
        {
            return gameObject.Owner == Constants.Game.GetMyself();
        }

        public static bool IsHeadingTowards(this GameObject gameObject, MapObject target, float certainty)
        {
            if(target is GameObject)
            {
                //is the gameObject heading towards the target or the target's intercept location????
                if(target is Elf)
                {
                    if (LastPosition.IsHeadingTowards(gameObject, ((Elf)gameObject).InterceptLocation((GameObject)target), certainty)) return true;
                }

                return LastPosition.IsHeadingTowards(gameObject, target, certainty);
            }
            else
            {
                return LastPosition.IsHeadingTowards(gameObject, target, certainty);
            }
        }

        public static bool IsHeadingTowards(this GameObject gameObject, MapObject target)
        {
            return gameObject.IsHeadingTowards(target, 0.9f);
        }

        public static int GetSize(this GameObject gameObject)
        {
            if (gameObject is Elf) return Constants.Game.ElfAttackRange;
            if (gameObject is IceTroll) return Constants.Game.IceTrollAttackRange;
            if (gameObject is LavaGiant) return Constants.Game.LavaGiantAttackRange;
            if (gameObject is Building) return ((Building)gameObject).Size;
            return 0;
        }

        

        /// <summary>
        /// Alias of GameObject.GetSize
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static int GetAttackRange(this GameObject gameObject)
        {
            return gameObject.GetSize();
        }

        public static int GetAttackMultiplier(this GameObject gameObject)
        {
            if (gameObject is Elf) return Constants.Game.ElfAttackMultiplier;
            if (gameObject is IceTroll) return Constants.Game.IceTrollAttackMultiplier;
            if (gameObject is LavaGiant) return Constants.Game.LavaGiantAttackMultiplier;
            return 0;
        }

        public static Location InterceptLocation(this Elf source, MapObject target)
        {
            if(target is GameObject)
            {
                return LastPosition.GetInterceptLocation(source, (GameObject)target);
            }
            else
            {
                return target.GetLocation();
            }
        }

        public static int GetMaxSpeed(this GameObject gameObject)
        {
            if (gameObject is Elf) return ((Elf)gameObject).MaxSpeed;
            if (gameObject is IceTroll) return Constants.Game.IceTrollMaxSpeed;
            if (gameObject is LavaGiant) return Constants.Game.LavaGiantMaxSpeed;
            return 0;
        }

        

        /// <summary>
        /// How many turns will it take this gameObject to arrive to target?
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int TimeToArrive(this GameObject source, MapObject target, bool includeSizes = false)
        {
            int distance = source.Distance(target);

            if (includeSizes)
            {
                distance -= source.GetSize();
                if (target is GameObject) distance -= ((GameObject)target).GetSize();
            }

            return distance / source.GetMaxSpeed();
        }

        public static int ArrivalTime(this GameObject source, MapObject target, bool includeSizes = false)
        {
            return Constants.Game.Turn + source.TimeToArrive(target, includeSizes);
        }

        #region Health prediction stuff
        public static int HealthDifference(this GameObject gameObject)
        {
            return LastHealth.HealthDifference(gameObject);
        }

        private static void GetGameObjectDamageAndTimeDelay(this GameObject gameObject, out int timeDelay, out int totalDamage, int recursionCount, bool enableRecursion = true)
        {
            timeDelay = 0;
            totalDamage = 0;

            int timeToArrive = gameObject.TimeToArrive(gameObject, true);
            int predictedTurnsToDeath;
            if (enableRecursion)
            {
                predictedTurnsToDeath = gameObject.PredictedTurnsToDeath(recursionCount);
            }
            else
            {
                predictedTurnsToDeath = gameObject.PredictedTurnsToDeathOnlyHealthDifference();
            }

            if (timeToArrive < predictedTurnsToDeath)
            {
                timeDelay = -timeToArrive;
                totalDamage = (predictedTurnsToDeath - timeDelay) * gameObject.GetAttackMultiplier();
                return;
            }
        }

        /// <summary>
        /// Returns in how many turns this gameObject will die. This function also attempts to predict Creatures behaviours
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="predictElves"></param>
        /// <returns>In how many turns this gameObject is predicated to die, will return -1 if not expected to die</returns>
        public static int PredictedTurnsToDeath(this GameObject gameObject, int recursionCount = 0)
        {
            recursionCount++;

            int timeDelay = 0; //when calculating enemies that have not yet actually arrived to us, we also must take into consideration the amount of turns it will take those enemies to arrive
            int totalDamage = 0; //total damage this gameObject took
            totalDamage -= gameObject.HealthDifference();

            Player enemy = gameObject.Owner.GetEnemy();

            //TODO: Fix recursion problem
            //The problem isn't necessarily the recursion itself, but the amount of entities it has to go through, thus causing a stackoverflow.
            //I see one solution:
            //Iteration would completely solve this problem, although given the requirements for this function I don't think it is possible
            //The reason iteration is impossible is:
            // - in order to check until when something will be alive, we have to know how long each of it's nearby enemies will be alive:
            // |  - and in order to check how long each of the nearby enemies will live we have to:
            //   |  - check how long each of those enemies' enemies will live
            //     |  - repeat...
            //
            //I tried solving this with a recursion limit of two but for some reason it doesn't work.
            //Tried debugging as well but I get an 'too many logs limit' error due to the StackOverflowException

            if(recursionCount <= 3)
            {
                //check incoming elfes
                foreach (Elf elf in enemy.GetLivingElves())
                {
                    if (!elf.IsBuilding && elf.IsHeadingTowards(gameObject, 0.9f))
                    {
                        GetGameObjectDamageAndTimeDelay(elf, out int objectTimeDelay, out int objectTotalDamage, recursionCount);

                        timeDelay += objectTimeDelay;
                        totalDamage += objectTotalDamage;
                    }
                }

                //here we are predicting damage from icetrolls, since we can reliably guess what icetrolls will do at any given moment
                //Disabled checking icetrolls since they cause stackoverflow exception, esspecially when played against bot named 'Trolly'
                foreach (IceTroll troll in enemy.GetIceTrolls())
                {
                    GameObject closestEnemyToEnemy = (GameObject)gameObject.Owner.GetAllGameObjects().ToArray().GetClosest(troll); //we dont check if this is null since this takes into account castle, which always exists
                    if (gameObject == closestEnemyToEnemy) //if the gameObject we are checking is the closest to this troll (meaning this troll will attempt to attack it
                    {
                        GetGameObjectDamageAndTimeDelay(troll, out int objectTimeDelay, out int objectTotalDamage, recursionCount, false);

                        timeDelay += objectTimeDelay;
                        totalDamage += objectTotalDamage;
                    }
                }

                if (gameObject is Castle) //if the gameObject we are checking is a castle
                {
                    foreach (LavaGiant lavaGiant in enemy.LavaGiants)
                    {
                        GetGameObjectDamageAndTimeDelay(lavaGiant, out int objectTimeDelay, out int objectTotalDamage, recursionCount);

                        timeDelay += objectTimeDelay;
                        totalDamage += objectTotalDamage;
                    }
                }
            }

            if (totalDamage == 0)
            {
                return Constants.Game.MaxTurns - Constants.Game.Turn;
            }
            else
            {
                System.Console.WriteLine($"Mathf.CeilToInt({gameObject.CurrentHealth} / {totalDamage}) + {timeDelay} = {Mathf.CeilToInt(gameObject.CurrentHealth / totalDamage) + timeDelay}");
                return Mathf.CeilToInt(gameObject.CurrentHealth / totalDamage) + timeDelay;
            }
        }

        /// <summary>
        /// Gets predicted turns to death based only on HealthDifference during the last turn
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static int PredictedTurnsToDeathOnlyHealthDifference(this GameObject gameObject)
        {
            int totalDamage = gameObject.HealthDifference();

            if (totalDamage == 0)
            {
                return Constants.Game.MaxTurns - Constants.Game.Turn;
            }
            else
            {
                return Mathf.CeilToInt(gameObject.CurrentHealth / totalDamage);
            }
        }

        /// <summary>
        /// Returns the predicted turn number of when this gameObject will die
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static int GetPredictedDeathTurn(this GameObject gameObject)
        {
            return Constants.Game.Turn + gameObject.PredictedTurnsToDeath();
        }
        #endregion

        public static bool InAttackRange(this GameObject source, MapObject target)
        {
            int targetAttackRange = 0;
            if(target is GameObject)
            {
                targetAttackRange = ((GameObject)target).GetSize();
            }

            return source.InRange(target, source.GetSize() + targetAttackRange);
        }

        public static LocationF GetDirection(this GameObject source)
        {
            return LastPosition.GetDirection(source);
        }

        public static LocationF GetVelocity(this GameObject source)
        {
            return LastPosition.GetVelocity(source);
        }
    }
}
