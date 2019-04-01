using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ {
    public static class ElfExtensions {
        public static List<GameObject> TargetedGameObjects = new List<GameObject>();

        

        /// <summary>
        /// Runs away from any ice trolls if it needs to. Returns true if it needs to run away, false otherwise
        /// </summary>
        /// <param name="elf"></param>
        /// <returns> If is running away from ice troll true, else false</returns>
        public static bool RunAwayFromIceTrolls(this Elf elf)
        {
            IceTroll[] iceTrolls = Constants.GameCaching.GetEnemyIceTrolls();

            foreach (IceTroll iceTroll in iceTrolls)
            {
                Location elfDirection = LastPosition.GetDirection(elf).GetIntLocation();
                Location nextLocation = elf.GetLocation().Add(elfDirection);

                if (iceTroll.CurrentHealth - Constants.Game.IceTrollSuffocationPerTurn * iceTroll.Distance(nextLocation) > 0 // if ice troll going to die when they meet
                    && iceTroll.Distance(nextLocation) < Constants.Game.IceTrollAttackRange) // if the ice troll is going to reach the elf
                {
                    float angle = Mathf.GetAngle(elf, iceTroll);
                    Location runAwayLocation = Mathf.GetNewLocationFromLocation(iceTroll, angle, 2000);
                    elf.MoveTo(runAwayLocation);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Runs away from any elfs if it needs to. Returns true if it needs to run away, false otherwise
        /// </summary>
        /// <param name="elf"></param>
        /// <returns> If is running away from elfs true, else false</returns>
        public static bool RunAwayFromElfs(this Elf elf)
        {
            Elf[] elfs = Constants.GameCaching.GetEnemyLivingElves();

            LocationF myNextLocation = elf.GetLocation().GetFloatLocation() + LastPosition.GetVelocity(elf);

            foreach (Elf enemyElf in elfs)
            {
                LocationF enemyNextLocation = enemyElf.GetLocation().GetFloatLocation() + LastPosition.GetVelocity(enemyElf);

                if (enemyElf.InRange(elf, enemyElf.AttackRange * 2) || enemyNextLocation.InRange(elf, enemyElf.AttackRange * 2)) // if the ice troll is going to reach the elf
                {
                    float angle = Mathf.GetAngle(elf, enemyElf) + 10;
                    Location runAwayLocation = Mathf.GetNewLocationFromLocation(enemyElf, angle, enemyElf.AttackRange * 2);
                    elf.MoveTo(runAwayLocation);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Runs away from both elfs and ice trolls if it needs to
        /// </summary>
        /// <param name="elf"></param>
        /// <returns>True if ran away from anthing, false if nothing happened</returns>
        public static bool RunAwayFromElfsOrIceTrolls(this Elf elf)
        {
            bool ranAwayFromIceTrolls = elf.RunAwayFromIceTrolls();
            if (ranAwayFromIceTrolls)
            {
                return true;
            }
            else
            {
                bool ranAwayFromElfs = elf.RunAwayFromElfs();
                return ranAwayFromElfs;
            }
        }

        public static void TryAttack(this Elf elf, GameObject target)
        {
            Utilities.TryAttack(elf, target);
        }

        public static void MoveToIntercept(this Elf elf, MapObject target)
        {
            if(target is GameObject)
            {
                Location interceptLocation = LastPosition.GetInterceptLocation(elf, (GameObject)target);
                elf.MoveTo(interceptLocation);
            }
            else
            {
                elf.MoveTo(target);
            }
        }

        public static bool HasSpeedUp(this Elf elf)
        {
            foreach (Spell spell in elf.CurrentSpells)
            {
                if (spell is SpeedUp)
                    return true;
            }

            return false;
        }
    }
}
