using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ.Bots
{
    class MainBot : HeuristicsBot
    {
        private float getMinimumManaToSave()
        {
            Game game = Constants.Game;
            float minManaToSave = Mathf.Max(game.IceTrollCost, game.TornadoCost);
            minManaToSave = Mathf.Max(game.PortalCost, minManaToSave);
            return minManaToSave * 1.2f;
        }

        public override void SetupHeuristics()
        {
            Game game = Constants.Game;

            Castle myCastle = game.GetMyCastle();
            Castle enemyCastle = game.GetEnemyCastle();
            float myCastleDistFromEnemyCastle = myCastle.Distance(enemyCastle);

            //If all enemy resources which might put our mana fountain in danger are far away from the elf
            //and the elf is close to our castle then we would like to build mana fountain.
            //Notice that, this heuristic max score value before multilying it by the weight might be much higher
            //Than 1 it depends on the balance between our mana fountains and the enemy mana fountains.
            //Our first mana fountain will get score of at least 5 (multiplying by 1500 we will get score of 7500)
            //This clearly after answering the above restrictions.
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfBuildManaFountain(1500, 3, game.ElfMaxSpeed * 20, game.TornadoMaxSpeed * game.TornadoMaxHealth, game.TornadoMaxSpeed * game.TornadoMaxHealth, myCastleDistFromEnemyCastle * 0.75f + game.CastleSize + game.PortalSize));
            //after building mana fountain we must at least build one portal to protect it
            //The following heuristics make sure that when our elf getting away from the mana fountain it built.
            //It will stop in the first place it can build portal and wait until it have enough money to build portal.
            //Notice that if the elf choose to go away on a path that already contains portal, it will not build a portal. 
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfDontMoveIfNearMyManaFountainAndCanBuildPortal(10000, game.ManaFountainSize + game.PortalSize * 2));
            //when we build manafountanis, we should always build atleast one portal near it to defend it
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfBuildPortalNearManaFountain(10000, game.ManaFountainSize + game.PortalSize * 2));

            //It is really important to have enough money in order to decide in which manner we want to use it.
            //So unless there is a very good reason (which score above 6100) we wait.
            float minManaToSave = getMinimumManaToSave();
            Heuristics.AddHeuristic(new IndividualHeuristics.SaveMinimumManaBeforeDeciding(6100, Mathf.CeilToInt(minManaToSave)));

            //here we may decide to build a portal if our elf is somewhat in-between the enemy elf and our castle
            //therefore, bulding the portal would defend the castle by standing inbetween the castla and the enemy portal
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfBuildPortalToDefendCastleFromEnemyElf(2000, game.ElfMaxSpeed * 15, game.ElfMaxSpeed * 7));
            //once we get near an enemy portal, we should build a portal of our own near it in order to 'challenge' it and make sure we can
            //respond to threats from it
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfBuildPortalToFaceEnemyPortal(2000, game.ElfMaxSpeed * 15, game.ElfMaxSpeed * -15));
            //simple heuristic for building portals when near enemy elves, usually with the intention of summoning icetrolls to scare away/kill the elves
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleBuildPortalToAttackEnemyElf(6000, game.ElfMaxSpeed * 8, game.ElfMaxSpeed * 25));

            //very simple heuristics to summon tornadoes from portals that are near enemy portals in order to destroy them
            //notice that as the attackRangePercentage decreases, the weight increases because we want to prioritize attacking enemy portals that are-
            //closer to our portal than further away ones
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonTornadoToAttackPortals(8000, 0.35f));
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonTornadoToAttackPortals(7000, 0.25f));
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonTornadoToAttackPortals(3000, 0.5f));
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonTornadoToAttackPortals(500, 0.75f));

            //very much like the three heuristics above, these heuristics are nearly identical but are intended to attack enemy manafountains instead of portals
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonTornadoToAttackManaFountains(40000, 0.25f));
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonTornadoToAttackManaFountains(17000, 0.35f));
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonTornadoToAttackManaFountains(8000, 0.65f));

            Circle monitoredCircle = new Circle(myCastle, myCastle.Size + game.ElfAttackRange + 15 * game.ElfMaxSpeed);
            //we want to 'keep an eye on' and monitor enemy elves that are near/heading towards our castle.
            //We do this by simply moving to a set distance from the enemy elves once they get to a certain Circle area from our castle
            //Generally, we want to monitor every and any elf in the map, but not as much as we want to monitor elves near our castle (hence why the higher weight)
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleMonitorEnemyElf(4000, game.ElfMaxSpeed * 3, game.ElfMaxSpeed * 3, game.ElfMaxSpeed * 20, monitoredCircle));
            //Similarly to the heuristic above, we also want to monitor every elf in the map, regardless of whether or not he is close to the castle.
            //this time, we do so from a greater distance, since not always the enemy elf would be of complete interest to us, but we still want to 'monitor' it
            //we also keep our distance and stay pretty far away from the enemy elf since we prefer to deal with/control the enemy elf with creatures rather than with elves directly
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleMonitorEnemyElf(1000, game.ElfMaxSpeed * 8, game.ElfMaxSpeed * 3, myCastleDistFromEnemyCastle, null));

            //This heuristic get really small weight and it is for the case that we don't have nothing to do.
            //In such a case we should go to some enemy portal in order to ruin it.
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleMonitorEnemyPortal(50, game.ElfAttackRange + game.PortalSize - 1, game.ElfMaxSpeed * 5.5f, myCastleDistFromEnemyCastle, null));
            //This heuristic continues the one above, but just give it a bit higher weight in case our elf
            //got 2 steps away from attacking the enemy portal.
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleMonitorEnemyPortal(150, game.ElfAttackRange + game.PortalSize - 1, game.ElfMaxSpeed * 5.5f, game.ElfAttackRange + game.PortalSize + game.ElfMaxSpeed * 2, null));
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleMonitorEnemyPortal(400, game.ElfAttackRange + game.PortalSize - 1, game.ElfMaxSpeed * 5.5f, game.VolcanoSize / 2 + game.ElfAttackRange * 4, new Circle(game.GetVolcano(), game.VolcanoSize * 3)));

            //with these heuristics, we simply want to monitor/get into attackRange of enemy mana fountains near our elves
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleMonitorEnemyManaFountain(4000, game.ElfAttackRange + game.ManaFountainSize - 1, game.ElfMaxSpeed * 3, game.ElfAttackRange + game.ManaFountainSize + game.ElfMaxSpeed * 10, null));
            //if we already got into one step away from the enemy manaFountain we add an extra 4000 weighted score since we are so close, we wouldn't mind getting hit from icetrolls or elves simply because we are already so close
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleMonitorEnemyManaFountain(4000, game.ElfAttackRange + game.ManaFountainSize - 1, game.ElfMaxSpeed, game.ElfAttackRange + game.ManaFountainSize + game.ElfMaxSpeed, null));
            //if our elves have nothing better to do, this heuristic will monitor every manafountain anywhere in the map regardless of distance.
            //low weight of 50 since we only want to do this if we have nothing else to do.
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleMonitorEnemyManaFountain(50, game.ElfAttackRange + game.ManaFountainSize - 1, game.ElfMaxSpeed * 5.5f, myCastleDistFromEnemyCastle, null));

            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleMonitorEnemyManaFountain(4000, game.ManaFountainSize + game.ElfMaxSpeed * 15f, 0, game.ElfMaxSpeed * 20, null));
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleSpeedUpToMonitorEnemyManaFountain(4000, game.ManaFountainSize + game.ElfMaxSpeed * 20f, 0, game.ElfMaxSpeed * 15, null));
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleSpeedUpToMonitorEnemyManaFountain(4000, game.ManaFountainSize + game.ElfMaxSpeed * 15f, game.ElfMaxSpeed * 12f, game.ElfMaxSpeed * 15, null));

            //Destroying enemy Mana Fountain is one of the most important thing in this game.
            //Hence we gave very high weight for attacking mana fountain if it is with in range.
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfAttackEnemyManaFountains(20000));
            //Destroying enemy Portal is an important thing, so we gave it high weight.
            //Still it is not as important as saving the health of our elf.
            //So we gave this heuristic smaller rate than ElfMoveAwayFromIceTrolls
            //Also ElfKeepDistanceFromEnemyElves get higher score when the balance is in favor of the enemy elf.
            //Otherwise if the enemy elf is within attack range we will choose to attack him since
            //ElfAttackEnemyElf got also higher weight.
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfAttackEnemyPortals(4000));
            //This heuristic is for the case that tornado is with in attack range of our elf
            //and our elf doesn't have very important things todo (if this is the case then the elf attack the enemy tornado)
            //Notice that there is no heuristic in which we try to catch the tornado with our elf.
            //We attack only if by chance the tornado go through our elf and our elf doesn't have 
            //something better to do then attack the enemy tornado.
            //we might think of adding a heuristic in which our elf go in order to attack the tornado
            //and not just encounter by chance.
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfAttackEnemyTornado(1100));

            //we want portals to summon icetrolls to stop/prevent enemy elves from reaching out castle
            //we check how much health the elf has in order to know how many icetrolls to make
            //if an enemy elf is extremely close to our portals, we have a high weight in order to summon an icetroll asap to kill/injure the enemy elf
            //also notice that this heuristic only handles the case that the portal is closer to our castle than the enemy elf (in-between). this means this is our last chance to chase away the elf from our castle
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonIceTrollToStopEnemyElvesFromGoingToMyCastle(8000, game.PortalSize + game.ElfAttackRange + 1, game.ElfMaxSpeed * 4f, 9, null));
            //these other two heuristics are for summoning icetrolls 'more generally' and when further away from our portals
            //this specific heuristic directly below the comment is more specifically targeted against enemy elves which are closer to our castle, therefore they become more important
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonIceTrollToStopEnemyElvesFromGoingToMyCastle(2000, game.ElfMaxSpeed * 10, game.ElfMaxSpeed * 10, 9, new Circle(myCastle, myCastle.Size + game.ElfAttackRange + game.IceTrollMaxHealth * game.IceTrollMaxSpeed * 0.5f)));
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonIceTrollToStopEnemyElvesFromGoingToMyCastle(1000, game.ElfMaxSpeed * 10, game.ElfMaxSpeed * 6, 9, null));

            //once an elf is potentially coming to our portals, we assume it does so in order to kill our portal
            //to mitigate this we summon an icetroll to cause the elf to run away from the portal, or to even injure/kill the elf
            //relatively high weight due to obvious importance
            //As the elf gets nearer to our castle the urge to kill it (or chase him away) is increasing.
            //so we have a few such heuristics with monitored circle such that as the elf gets closer to our castle we add more weight(+6500 for each circle the enemy elf enters).

            //In addition notice that as the enemy elf get near enough to our castle we may send an ice troll from a more far away place and it will still help in running away the elf from our castle. This is the reason why in line 128 we are ok with sending an ice troll from a portal which is 20 steps from the enemy elf.
            //TODO: Change some of the numbers(like the 20 I mention above) to relative number from the ice troll max health(for example instead of 20 write game.IceTrollMaxHealth - 5 or instead of 12 write game.IceTrollMaxHealth * 0.5f)
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonIceTrollToDefendPortalAgainstElves(6500, game.ElfAttackRange + game.PortalSize + game.ElfMaxSpeed, 9, new Circle(enemyCastle, enemyCastle.Size + game.LavaGiantAttackRange + game.LavaGiantMaxSpeed * 6)));
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonIceTrollToDefendPortalAgainstElves(6500, game.IceTrollMaxSpeed * 20, 9, new Circle(enemyCastle, enemyCastle.Size + game.LavaGiantAttackRange + game.ElfMaxSpeed * 2)));
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonIceTrollToDefendPortalAgainstElves(6000, game.ElfMaxSpeed * 12, 9, new Circle(myCastle, myCastle.Size + game.ElfAttackRange + game.IceTrollMaxHealth * game.IceTrollMaxSpeed * 0.8f)));
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonIceTrollToDefendPortalAgainstElves(1000, game.ElfMaxSpeed * 6, 9, null));

            //just like enemy elves, we want to summon icetrolls to protect our portal from enemy tornadoes.
            //when dealing with enemy elves, we have to mostly guess what the elf wants/plans to do
            //here, since the behaviour and rules of tornadoes are very clearly known to us, we don't have to guess and at any given time we can know exactly what a tornado will do
            //we have higher distances to give us more opportunities to kill the enemy tornado before it reaches us
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummoningIceTrollToDefendPortalAgainstTornadoes(1000, game.ElfMaxSpeed * 15));
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummoningIceTrollToDefendPortalAgainstTornadoes(50, game.ElfMaxSpeed * 14));
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummoningIceTrollToDefendPortalAgainstTornadoes(50, game.ElfMaxSpeed * 13));
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummoningIceTrollToDefendPortalAgainstTornadoes(50, game.ElfMaxSpeed * 12));

            //if we know for certain we can kill the enemy tornado for sure, we summon an icetroll
            //this has a much higher weight so that we would not miss this opportunity
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonIceTrollToDefendPortalAgainstTornadoesIfKillsForSure(7000, game.ElfMaxSpeed * 15));

            //if we are near an enemy elf, it is pretty important we attack it, and thats the reason for the 5000 score
            //the weight isn't higher because we might have better things to do than to fight/get injured while fighting an enemy elf (enemy elf attacks us while we attack it)
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfAttackEnemyElf(5000));
            //if we don't have much health, we want to keep our distance from enemy elves to avoid dying
            //Notice that the score we return is -1 * the ratio between the enemy health and our elf(or zero if our elf has more health). Which means that when the balance is in favor for the enemy elf the score will be smaller than - 1.For example if the ratio is 2 the score will be -6000 which means that if our elf in attack range from the enemy elf it will still run away since the score for attacking the enemy elf is only 5000.
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfKeepDistanceFromEnemyElves(3000, game.ElfMaxSpeed * 4.5f, game.ElfMaxSpeed * 5.5f, 5));
            //when too close to enemy elves and if our elves don't have much health left, we go invisible in order to protect ourselves
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfInvisibilitySpellToKeepDistanceFromEnemyElves(6100, game.ElfMaxSpeed * 3.5f, game.ElfMaxSpeed * 4.5f, 4));
            //fighting/messing with enemy icetrolls is pointless and futile. we pretty much always want to keep our distance from enemy icetrolls
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfMoveAwayFromIceTrolls(6000, game.ElfMaxSpeed * 4.5f, 2f, 4));
            //before we move away from enemy icetrolls, we want to cast invisibility in order to allow for a quick and safe escape.
            //this must always have a higher weight than 'ElfMoveAwayFromIceTrolls' to ensure it occurs before 'ElfMoveAwayFromIceTrolls'
            //We only use it if the IceTroll is already within attack range, otherwise there is not need to get invisible.
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfInvisibilitySpellBeforeMoveAwayFromIceTrolls(6100));

            //if we have more combined health than the combined health of enemies in the given heuristic distance,
            //we go in to attackRange in order to attack the enemy elf
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfMoveToAttackEnemyElf(1000, game.ElfMaxSpeed * 6));

            //This heuristic intention was to try save money for building mana fountains near our castle.
            //Specifically we want to save money if:
            //One of our elves is in distance of 30 or less steps
            //Or one of our elves will revive in <= 10 turns (we assume that the initial location of the elves is near our castle)
            //The heuristics score also depend on the balance between the amount of our mana fountains and the enemy mana fountains.
            //In the end the importance of this heuristic was to save more money in order to make smarter actions against enemy attacks
            Heuristics.AddHeuristic(new IndividualHeuristics.SaveManaForBalancingManaFountains(500, game.ElfMaxSpeed * 30, 10));

            //a sort of naive heuristic for summon lavagiants in order to hurt enemy castle
            Heuristics.AddHeuristic(new IndividualHeuristics.NumOfAttacksByVirtualLavaGiant(7000, 0.75f, 0));
            Heuristics.AddHeuristic(new IndividualHeuristics.NumOfAttacksByVirtualLavaGiant(7000, 0.5f, 90));
            Heuristics.AddHeuristic(new IndividualHeuristics.NumOfAttacksByVirtualLavaGiant(7000, 0.25f, 300));

            Circle buildPortalNearEnemyCastleCircle = new Circle(enemyCastle, enemyCastle.Size + game.ElfMaxSpeed * 15);
            //simply add the heurustic for elves to build portals near castle
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfBuildPortal(1000, buildPortalNearEnemyCastleCircle));
            List<Circle> circles = new List<Circle>();
            //in this heuristic we find the best pair of elves and portal locations in order to decice to which 'best' location an elf should go
            //the reason we add the same circle twice is so that we would find the best locations for both elves instead of just one
            //this should probably be improved at some point
            circles.Add(buildPortalNearEnemyCastleCircle);
            circles.Add(buildPortalNearEnemyCastleCircle);
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleMoveToBestBuildPortalArea(50, circles, myCastleDistFromEnemyCastle));

            Heuristics.AddHeuristic(new IndividualHeuristics.ElfMoveToEnemyCastle(1));
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfAttackEnemyCastle(2));

            //'simple' heuristics to build attack-portals near enemy manafountains with the future intention of summoning tornadoes and destroying them
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleBuildPortalToAttackEnemyManaFountain(8000, game.ElfMaxSpeed * 10));
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleBuildPortalToAttackEnemyManaFountain(1100, Mathf.CeilToInt(game.TornadoMaxHealth * game.TornadoMaxSpeed * 0.6f)));

            //we dont want to build portals or manafountains if there are tornadoes nearby
            //meaning, we might not get to finish building whatever it is we wanted to build
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfDontBuildIfTornadoesNearBy(5000, game.TornadoMaxSpeed * 6));
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfDontBuildIfTornadoesNearBy(1000, game.TornadoMaxSpeed * 9));
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfDontBuildIfTornadoesNearBy(500, game.TornadoMaxSpeed * 12));

            //a sort of case-specific heuristic, once we approach the end of the game (usually turn 750) we want to summon lavagiants in order to get the most amount of damage as possible before we won't have a choice anymore
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonLavaGiantNearTurnLimit(2000, 45));
            //when we are attacking a portal and there are enemies nearby but we will be able to finish destroying the portal, we want to finish destroying the portal
            //instead of running away too soon
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfContinueAttackingPortalWhichAboutToDie(10000, 1));

            //we want to monitor enemy elves that are not too close to other enemies
            //also notice we don't get into attackrange from the enemyelf, but just keep a relatively small distance
            Circle monitoredCircleArroundEnemyCastle = new Circle(enemyCastle, enemyCastle.Size + game.ElfAttackRange + 15 * game.ElfMaxSpeed);
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleMonitorEnemyElf(1000, game.ElfMaxSpeed * 3, game.ElfMaxSpeed * 3, game.ElfMaxSpeed * 10, monitoredCircleArroundEnemyCastle));

            //obviously we dont want to build portals if we are surrounded by enemy elves and it looks like we won't be able to finish the portal
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfDontBuildPortalIfTooManyElvesWithinRange(10000, 1, game.ElfMaxSpeed * 8.5f));

            //we dont want our portals to summon anything if they won't be able to finish it, waste of mana
            //here we simply input the parameters as to how far away do we check enemies and stuff
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalDontSummonIfTornadoNearBy(20000, game.PortalSize + game.TornadoAttackRange + game.TornadoMaxSpeed));

            //if our elf is near enemy castle and the 'field' around the elf is empty, we want to build a portal so that we can attack the enemy castle with LavaGiants
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfBuildPortalIfEmptyBoardAndNearEnemyCastle(20000, game.CastleSize + game.ElfMaxSpeed * 15));

            //Just as we move away from IceTrolls, we want to also move away from enemy portals currently summoning IceTrolls
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfMoveAwayFromPortalsSummoningIceTrolls(2000, game.IceTrollMaxSpeed * 5, 3));

            //Elves keeping thier distance from other elves is always good!
            //This makes it harder for our enemy to kill both of our elves at the same time and keeps our defense strong
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfMoveAwayFromFriendlyElves(1000, game.IceTrollMaxSpeed * 15));

            Heuristics.AddHeuristic(new IndividualHeuristics.ElfBuildAttackPortal(5000, game.CastleSize + game.PortalSize, game.CastleSize + game.ElfMaxSpeed * 10, game.PortalSize * 2));

            //heuristic for finalling killing the enemy castle if we can do so
            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonLavaGiantToFinalKillEnemyCastle(10000));

            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleMonitorVolcano(3000, game.VolcanoSize + game.ElfAttackRange, 0, game.GetVolcano().Distance(game.GetMyCastle()), null, 5));
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfSimpleSpeedUpToMonitorVolcano(4000, game.VolcanoSize + game.ElfAttackRange, 0, game.ElfMaxSpeed * 20, null));
            Heuristics.AddHeuristic(new IndividualHeuristics.ElfAttackVolcano(7000)); //this heuristic must always have more weight than ElfMoveToVolcano AND ElfBuildPortalNearVolcano

            Heuristics.AddHeuristic(new IndividualHeuristics.ElfBuildPortalNearVolcano(2000, game.VolcanoSize + game.LavaGiantMaxSpeed * 5));

            Heuristics.AddHeuristic(new IndividualHeuristics.PortalSummonIceTrollToDefendCastleAgainstLavaGiant(30000f, game.CastleMaxHealth * 0.1f, game.CastleSize + game.LavaGiantMaxSpeed * 5));

            Heuristics.AddHeuristic(new IndividualHeuristics.ElfMoveAwayFromEnemyCastle(6000, game.ElfMaxSpeed * 12)); //this heuristic must always have more weight than ElfMoveToVolcano AND ElfBuildPortalNearVolcano
        }
    }
}