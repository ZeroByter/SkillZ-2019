using ElfKingdom;

namespace SkillZ
{
    class BuildManaFountainAction : Action
    {
        public BuildManaFountainAction(GameObject gameObject) : base(gameObject) { }

        public override bool DoActionInNextTurn(VirtualGame virtualGame)
        {
            return virtualGame.AddFutureManaFountain((Elf)gameObject);
        }

        public override void RemoveActionInNextTurn(VirtualGame virtualGame)
        {
            virtualGame.RemoveFutureManaFountain((Elf)gameObject);
        }

        public override void DoGameAction()
        {
            Elf elf = (Elf)gameObject;

            elf.BuildManaFountain();
        }
    }
}
