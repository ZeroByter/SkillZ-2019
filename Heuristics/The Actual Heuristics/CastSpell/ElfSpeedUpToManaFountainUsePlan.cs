using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.IndividualHeuristics
{
    /// <summary>
    /// Still has bugs! Do not use!
    /// Still has bugs! Do not use!
    /// 
    /// Remove this warning when you finish fixing this heuristic...
    /// </summary>
    public class ElfSpeedUpToManaFountainUsePlan : Heuristic
    {
        private class ElfPlanState
        {
            public Elf elf;
            public bool isActive;
            public enum Step { SpeedUp = 0, Movement = 1 }
            public Step step;
            public Location targetLocation;

            public ElfPlanState(Elf elf, Location targetLocation)
            {
                this.elf = elf;
                isActive = true;
                step = Step.SpeedUp;
                this.targetLocation = targetLocation;
            }
        }

        //private static Dictionary<int, ElfPlanState> elfStates = new Dictionary<int, ElfPlanState>();
        ElfPlanState elfState;

        private int scanForManaFountainsWithinRange;
        private int minimumNormalStepsFromManaFountain;
        private int numOfTurns;

        public ElfSpeedUpToManaFountainUsePlan(float weight, int numOfTurns, int minimumNormalStepsFromManaFountain) : base(weight)
        {
            this.scanForManaFountainsWithinRange = GetDistanceForCheckingManaFountains(numOfTurns);
            this.minimumNormalStepsFromManaFountain = minimumNormalStepsFromManaFountain;
            this.numOfTurns = numOfTurns;
        }

        private ElfPlanState CreateElfState(Elf elf, ManaFountain targetManaFountain)
        {
            Location targetLocation = targetManaFountain.GetLocation().Towards(elf, Constants.Game.ManaFountainSize + Constants.Game.ElfAttackRange);
            return new ElfPlanState(elf, targetLocation);
        }

        /*private static ElfPlanState GetElfState(Elf elf)
        {
            ElfPlanState plan = null;

            elfStates.TryGetValue(elf.UniqueId, out plan);

            return plan;
        }*/

        private ManaFountain FindSuitableManaFountain(Elf myElf)
        {
            //we only proceed if we don't have enough mana for sending a tornado (or building a portal and then sending a tornado)
            if (Constants.Game.GetMyMana() > Constants.Game.TornadoCost + Constants.Game.PortalCost) return null;

            //- Our elf will get with in at most 5 steps to attack range.
            List<ManaFountain> manaFountainsInRange = Constants.GameCaching.GetEnemyManaFountainsInArea(new Circle(myElf, scanForManaFountainsWithinRange));

            //first we iterate over every manaFountain in range

            foreach (ManaFountain manaFountain in manaFountainsInRange)
            {
                //mana fountain should not be too close, ideally should be further away than 6 normal elf steps
                if (myElf.Distance(manaFountain) > Constants.Game.ElfMaxSpeed * minimumNormalStepsFromManaFountain)
                {
                    //- The elf for sure will not get hit before it reach to the mana fountain.
                    //- There are no existing ice trolls that will reach him when he will get to the mana fountain.
                    if (CanForSureSafelyReachManaFountain(myElf, manaFountain))
                    {
                        //- The enemy elf will not get to our elf before it destroy the mana fountain (like Nahalal 4 does, you should also notice if the enemy elf is currently summoning).
                        if (!WillEnemyElfKillUsBeforeWeDestroy(myElf, manaFountain))
                        {
                            return manaFountain; //this, times the weight we would give the heuristic, is a 'very high score'
                        }
                    }
                }
            }

            return null;
        }

        public override void UpdateState(Game game)
        {
            if (elfState != null)
            {
                bool stopPlan = false;

                if (elfState.step == ElfPlanState.Step.SpeedUp)
                {
                    if (elfState.elf.HasSpeedUp())
                    {
                        elfState.step = ElfPlanState.Step.Movement;
                    }
                    else
                    {
                        stopPlan = true;
                    }
                }
                else if (elfState.step == ElfPlanState.Step.Movement)
                {
                    Location expectedLocation = elfState.elf.GetLocation().Towards(elfState.targetLocation, elfState.elf.MaxSpeed);
                    if (elfState.elf.GetLocation().Distance(expectedLocation) > Constants.Game.ElfMaxSpeed) {
                        stopPlan = true;
                    }
                    if (stopPlan)
                    {
                        ElfMoveTargets.RemoveHeuristicCircle(GetType().Name);
                        elfState = null;
                    }
                }
            }

            if (elfState == null && Constants.GameCaching.GetEnemyManaFountains().Length == 1)
            {
                foreach (Elf elf in Constants.Game.GetMyLivingElves())
                {
                    ManaFountain suitableTargetManaFountain = FindSuitableManaFountain(elfState.elf);

                    if (suitableTargetManaFountain != null)
                    {
                        elfState = CreateElfState(elf, suitableTargetManaFountain);
                        ElfMoveTargets.AddHeuristicCircleWithTurnLimit(GetType().Name, new Circle(elfState.targetLocation, Constants.Game.ManaFountainSize + Constants.Game.ElfAttackRange), numOfTurns);
                        break;
                    }
                }
            }
        }

        //changed to public static for unit tests
        public static int GetDistanceForCheckingManaFountains(int numOfTurns)
        {
            int distance = (Constants.Game.ElfMaxSpeed * Constants.Game.SpeedUpMultiplier) * numOfTurns;

            if (Constants.Game.SpeedUpExpirationTurns < numOfTurns)
            {
                distance = Constants.Game.SpeedUpExpirationTurns * (Constants.Game.ElfMaxSpeed * Constants.Game.SpeedUpMultiplier) + Constants.Game.ElfMaxSpeed * (numOfTurns - Constants.Game.SpeedUpExpirationTurns);
            }

            return distance - Constants.Game.ElfAttackRange - Constants.Game.ManaFountainSize;
        }

        private bool CanForSureSafelyReachManaFountain(Elf myElf, ManaFountain manaFountain)
        {
            //TODO, implement all these functions into GameCaching as GetEnemyIceTrollsInConeArea
            foreach (IceTroll iceTroll in Constants.GameCaching.GetEnemyIceTrolls())
            {
                //0.68f is almost about 45 degrees (not exactly but close enough)
                if (Utilities.IsLocationInCone(myElf, manaFountain, iceTroll, 0.68f))
                {
                    return false;
                }
            }

            foreach (Elf enemyElf in Constants.GameCaching.GetEnemyLivingElves())
            {
                //0.68f is almost about 45 degrees (not exactly but close enough)
                if (Utilities.IsLocationInCone(myElf, manaFountain, enemyElf, 0.68f))
                {
                    return false;
                }
            }

            foreach (Portal enemyPortal in Constants.GameCaching.GetEnemyPortalsCurrentlySummoningIceTrolls())
            {
                //0.68f is almost about 45 degrees (not exactly but close enough)
                if (Utilities.IsLocationInCone(myElf, manaFountain, enemyPortal, 0.68f))
                {
                    return false;
                }
            }

            return true;
        }

        private bool WillEnemyElfKillUsBeforeWeDestroy(Elf elf, ManaFountain manaFountain)
        {
            //the range of the circle is how long it would take us to destroy the manafountain times the ElfMaxSpeed
            var circle = new Circle(manaFountain, manaFountain.CurrentHealth / Constants.Game.ElfAttackMultiplier * Constants.Game.ElfMaxSpeed);

            int timeToReachManaFountain = (elf.Distance(manaFountain) - elf.AttackRange - manaFountain.Size) / Constants.Game.ElfMaxSpeed;
            //total time it will take to kill manafountain (including getting there)
            int totalTimeToKillManaFountain = manaFountain.CurrentHealth / elf.AttackMultiplier + timeToReachManaFountain;

            //this is the location we will finally attack the manafountain from
            Location attackManaFountainLocation = manaFountain.Location.Towards(elf, manaFountain.Size + elf.AttackRange);

            foreach (Elf enemyElf in Constants.GameCaching.GetEnemyElvesInArea(circle))
            {
                int timeToReachOurAttackingLocation = enemyElf.Distance(attackManaFountainLocation) / Constants.Game.ElfMaxSpeed;
                int totalTimeToKillUs = elf.CurrentHealth / enemyElf.AttackMultiplier + timeToReachOurAttackingLocation;

                if (totalTimeToKillUs >= totalTimeToKillManaFountain) return true;
            }

            //TODO: loop through enemy elves in this circle area and check if they can reach us before we finish or are summoning icetrolls

            return false;
        }

        private bool WillEnemyIceTrollKillUsBeforeWeDestroy(Elf elf, ManaFountain manaFountain)
        {
            //the range of the circle is how long it would take us to destroy the manafountain times the ElfMaxSpeed
            var circle = new Circle(manaFountain, manaFountain.CurrentHealth / Constants.Game.ElfAttackMultiplier * Constants.Game.ElfMaxSpeed);

            int timeToReachManaFountain = (elf.Distance(manaFountain) - elf.AttackRange - manaFountain.Size) / Constants.Game.ElfMaxSpeed;
            //total time it will take to kill manafountain (including getting there)
            int totalTimeToKillManaFountain = manaFountain.CurrentHealth / elf.AttackMultiplier + timeToReachManaFountain;

            //this is the location we will finally attack the manafountain from
            Location attackManaFountainLocation = manaFountain.Location.Towards(elf, manaFountain.Size + elf.AttackRange);

            foreach (IceTroll enemyIceTroll in Constants.GameCaching.GetEnemyIceTrollsInArea(circle))
            {
                int timeToReachOurAttackingLocation = enemyIceTroll.Distance(attackManaFountainLocation) / Constants.Game.IceTrollMaxSpeed;
                int totalTimeToKillUs = elf.CurrentHealth / enemyIceTroll.AttackMultiplier + timeToReachOurAttackingLocation;

                if (totalTimeToKillUs >= totalTimeToKillManaFountain) return true;
            }

            return false;
        }

        private float GetSpeedUpScore(VirtualSpeedUp futureSpeedUp)
        {
            //i will place comment near each line which will specify what condition I believe it checks for:
            //check me if i am wrong

            //- Our elf will get with in at most 5 steps to attack range.
            List<ManaFountain> manaFountainsInRange = Constants.GameCaching.GetEnemyManaFountainsInArea(new Circle(futureSpeedUp.location, Constants.Game.ElfMaxSpeed * 5 - Constants.Game.ElfAttackRange - Constants.Game.ManaFountainSize));
            //- There is only one mana fountain (otherwise we should probably find a way to send a tornado)
            if (manaFountainsInRange.Count > 1) return 0;

            //first we iterate over every manaFountain in range

            foreach (ManaFountain manaFountain in manaFountainsInRange)
            {
                //- The elf for sure will not get hit before it reach to the mana fountain.
                //- There are no existing ice trolls that will reach him when he will get to the mana fountain.
                if (CanForSureSafelyReachManaFountain((Elf)futureSpeedUp.realGameObject, manaFountain))
                {
                    //- The enemy elf will not get to our elf before it destroy the mana fountain (like Nahalal 4 does, you should also notice if the enemy elf is currently summoning).
                    if (!WillEnemyElfKillUsBeforeWeDestroy((Elf)futureSpeedUp.realGameObject, manaFountain))
                    {
                        return 50; //this, times the weight we would give the heuristic, is a 'very high score'
                    }
                }
            }

            return 0;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            float score = 0;

            foreach (Elf elf in Constants.GameCaching.GetMyLivingElves())
            {
                if (elfState.isActive)
                {
                    if (elfState.step == ElfPlanState.Step.SpeedUp)
                    {
                        if (virtualGame.DoesElfHaveFutureSpeedUp(elf)) score++;
                    }
                    else if (elfState.step == ElfPlanState.Step.Movement && elfState.targetLocation != null)
                    {
                        if (virtualGame.DoesElfHaveFutureLocationIn(elf, elf.GetLocation().Towards(elfState.targetLocation, elf.MaxSpeed)))
                        {
                            score++;
                        }
                    }
                }
            }

            return score;
        }
    }
}
