using ElfKingdom;

namespace SkillZ
{
    public abstract class Action
    {
        public GameObject gameObject;

        public Action(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public abstract bool DoActionInNextTurn(VirtualGame virtualGame);

        public abstract void RemoveActionInNextTurn(VirtualGame virtualGame);

        public abstract void DoGameAction();

        public override string ToString()
        {
            return $"{GetType().Name} has gameObject {gameObject}";
        }
    }
}
