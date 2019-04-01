using ElfKingdom;

namespace SkillZ
{
    class BuildPortalAction : Action
    {
        public BuildPortalAction(GameObject gameObject) : base(gameObject) { }

        public override bool DoActionInNextTurn(VirtualGame virtualGame)
        {
            return virtualGame.AddFuturePortal((Elf)gameObject);
        }

        public override void RemoveActionInNextTurn(VirtualGame virtualGame)
        {
            virtualGame.RemoveFuturePortal((Elf)gameObject);
        }

        public override void DoGameAction()
        {
            Elf elf = (Elf)gameObject;

            elf.BuildPortal();
        }
    }
}
