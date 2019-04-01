using ElfKingdom;
using System.Collections.Generic;

namespace SkillZ
{
    class ActionsFactory
    {
        private static ActionsFactory Singletron;

        public static ActionsFactory GetInstance()
        {
            if(Singletron == null)
            {
                Singletron = new ActionsFactory();
            }

            return Singletron;
        }

        private ActionsFactory() {}

        public Dictionary<int, List<Action>> GetAllActions(GameNextTurnActions nextTurnActions)
        {
            Dictionary<int, List<Action>> possibleActions = new Dictionary<int, List<Action>>();
            List<GameObject> ourGameObjects = Constants.GameCaching.GetMyControllableGameObjects();

            ElfMoveTargets moveTargets = new ElfMoveTargets();

            foreach (GameObject ourGameObject in ourGameObjects)
            {
                if(ourGameObject is Elf)
                {
                    ElfActionGenerator actionGenerator = new ElfActionGenerator((Elf)ourGameObject, moveTargets);

                    possibleActions.Add(ourGameObject.UniqueId, actionGenerator.GenerateActions());
                }

                if (ourGameObject is Portal)
                {
                    PortalActionGenerator actionGenerator = new PortalActionGenerator((Portal)ourGameObject);

                    possibleActions.Add(ourGameObject.UniqueId, actionGenerator.GenerateActions());
                }
            }

            return possibleActions;
        }
    }
}
