using ElfKingdom;
using System.Collections.Generic;
using System.Linq;

namespace SkillZ.IndividualHeuristics
{
    class ElfCastInvisibilityWhenBeingChased : Heuristic
    {
        //the radius around us we check if we are being chased
        private float radius;
        //the radius around us we check if there are no other enemies not chasing us around us except the one chasing us (to prevent randomly speeding up when we are not actually being chased)
        private float aloneRadius;
        private float nearMapEdgesPadding;

        public ElfCastInvisibilityWhenBeingChased(float weight, float radius, float aloneRadius, float nearMapEdgesPadding) : base(weight)
        {
            this.radius = radius;
            this.aloneRadius = aloneRadius;
            this.nearMapEdgesPadding = nearMapEdgesPadding;
        }

        private GameObject GetEnemyChasingGameObject(Elf myElf)
        {
            Circle checkArea = new Circle(myElf, radius);

            foreach (Elf enemyElf in Constants.GameCaching.GetEnemyElvesInArea(checkArea))
            {
                if (enemyElf.IsHeadingTowards(myElf, 0.9f)) return enemyElf;
            }

            foreach (IceTroll enemyIceTroll in Constants.GameCaching.GetEnemyIceTrollsInArea(checkArea))
            {
                if (enemyIceTroll.IsHeadingTowards(myElf, 0.9f)) return enemyIceTroll;
            }

            return null;
        }

        //is it just us in the `aloneRadius`, or are there other enemies?
        private bool AreWeAloneWithChasingEnemy(Elf myElf, GameObject chasingEnemy)
        {
            Circle checkArea = new Circle(myElf, aloneRadius);

            foreach (Elf enemyElf in Constants.GameCaching.GetEnemyElvesInArea(checkArea))
            {
                if (enemyElf.UniqueId == chasingEnemy.UniqueId) continue;

                return false;
            }

            foreach (IceTroll enemyIceTroll in Constants.GameCaching.GetEnemyIceTrollsInArea(checkArea))
            {
                if (enemyIceTroll.UniqueId == chasingEnemy.UniqueId) continue;

                return false;
            }

            return true;
        }

        public override float GetScore(VirtualGame virtualGame)
        {
            if (Constants.Game.GetMyMana() <= Constants.Game.LavaGiantCost) return 0;

            float score = 0;

            foreach(var pair in virtualGame.futureInvisibilitySpells)
            {
                Elf myElf = (Elf)pair.Value.realGameObject;

                GameObject chasingEnemy = GetEnemyChasingGameObject(myElf);

                if(chasingEnemy != null)
                {
                    if(myElf.GetLocation().Row < nearMapEdgesPadding ||
                        myElf.GetLocation().Row > Constants.Game.Rows - nearMapEdgesPadding ||
                        myElf.GetLocation().Col < nearMapEdgesPadding ||
                        myElf.GetLocation().Col > Constants.Game.Cols - nearMapEdgesPadding)
                    {
                        score++; //bonus if we are near the map edges, we shouldn't be here!
                    }

                    if(AreWeAloneWithChasingEnemy(myElf, chasingEnemy))
                    {
                        score++;
                    }
                }
            }

            return score;
        }
    }
}