using ElfKingdom;

namespace SkillZ
{
    public class MoveAction : Action
    {
        private Location nextLocation;

        public MoveAction(GameObject gameObject, Location location) : base(gameObject)
        {
            nextLocation = location;
        }

        public override bool DoActionInNextTurn(VirtualGame virtualGame)
        {
            virtualGame.SetFutureLocation((Elf)gameObject, nextLocation);
            return true;
        }

        public override void RemoveActionInNextTurn(VirtualGame virtualGame)
        {
            virtualGame.RemoveFutureLocation(gameObject);
        }

        public override void DoGameAction()
        {
            Elf elf = (Elf)gameObject;

            elf.MoveTo(nextLocation);
        }

        public override string ToString()
        {
            return $"{GetType().Name} to nextLocation {nextLocation}";
        }
    }
}
