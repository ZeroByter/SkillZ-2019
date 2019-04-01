using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ
{
    class ElfActionGenerator : ActionGenerator
    {
        public ElfMoveTargets moveTargets;
        public Elf elf;

        public ElfActionGenerator(Elf elf, ElfMoveTargets moveTargets)
        {
            this.elf = elf;
            this.moveTargets = moveTargets;
        }

        public List<Action> GenerateActions()
        {
            List<Action> actions = new List<Action>();

            if (elf.IsBuilding) return actions;

            //can we place a portal here?
            if (elf.CanBuildPortal())
            {
                actions.Add(new BuildPortalAction(elf));
            }

            if (elf.CanBuildManaFountain())
            {
                actions.Add(new BuildManaFountainAction(elf));
            }

            if (elf.CanCastInvisibility())
            {
                actions.Add(new CastInvisibility(elf));
            }

            if (Constants.Game.ElfMaxSpeed != 0)
            {
                if (elf.CanCastSpeedUp() && !elf.IsBuilding)
                {
                    actions.Add(new CastSpeedUp(elf));
                }

                //check movement in every direction in 5 degrees steps
                for (int i = 0; i < 360; i += 5)
                {
                    Location newLocation = elf.GetNewLocation(i, elf.MaxSpeed/*(elf.MaxSpeed + Constants.Game.ElfMaxSpeed) / 2*/);

                    if (newLocation.InMap())
                    {
                        actions.Add(new MoveAction(elf, newLocation));
                    }
                }

                /*foreach (Location permMoveLocation in ElfMoveTargets.PermanentMoveLocations)
                {
                    Location newLocation = elf.GetLocation().Towards(permMoveLocation, (elf.MaxSpeed + Constants.Game.ElfMaxSpeed) / 2);

                    actions.Add(new MoveAction(elf, newLocation));
                }

                foreach (Location moveLocation in moveTargets.moveLocations)
                {
                    Location newLocation = elf.GetLocation().Towards(moveLocation, (elf.MaxSpeed + Constants.Game.ElfMaxSpeed) / 2);

                    actions.Add(new MoveAction(elf, newLocation));
                }*/
            }
			
			//We used to use InRange here (and also in other places in the bot) but we actually found InRange to be too unreliable and sometimes didn't return the expected results. So we check manually.

            if (Constants.Game.GetVolcano().IsActive() && Constants.Game.GetVolcano().DamageByEnemy <= Constants.Game.VolcanoMaxHealth / 2 && elf.Distance(Constants.Game.GetVolcano()) <= (Constants.Game.ElfAttackRange + Constants.Game.VolcanoSize))
            {
                actions.Add(new AttackAction(elf, Constants.Game.GetVolcano()));
            }

            if (elf.Distance(Constants.Game.GetEnemyCastle()) <= (Constants.Game.ElfAttackRange + Constants.Game.CastleSize))
            {
                actions.Add(new AttackAction(elf, Constants.Game.GetEnemyCastle()));
            }

            foreach (Portal enemyPortal in Constants.GameCaching.GetEnemyPortals())
            {
                if (elf.Distance(enemyPortal) <= (Constants.Game.ElfAttackRange + enemyPortal.Size))
                {
                    actions.Add(new AttackAction(elf, enemyPortal));
                }
            }

            foreach (ManaFountain enemyGameObject in Constants.GameCaching.GetEnemyManaFountains())
            {
                if (elf.Distance(enemyGameObject) <= (Constants.Game.ElfAttackRange + enemyGameObject.Size))
                {
                    actions.Add(new AttackAction(elf, enemyGameObject));
                }
            }



            foreach (Elf enemyGameObject in Constants.GameCaching.GetEnemyLivingElves())
            {
                if (elf.Distance(enemyGameObject) <= Constants.Game.ElfAttackRange)
                {
                    actions.Add(new AttackAction(elf, enemyGameObject));
                }
            }

            foreach (GameObject enemyGameObject in Constants.GameCaching.GetEnemyCreatures())
            {
                if (elf.Distance(enemyGameObject) <= Constants.Game.ElfAttackRange)
                {
                    actions.Add(new AttackAction(elf, enemyGameObject));
                }
            }

            return actions;
        }
    }
}
