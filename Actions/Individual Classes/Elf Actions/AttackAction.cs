using ElfKingdom;

namespace SkillZ
{
    public class AttackAction : Action
    {
        public GameObject attackObject;
        public string attackType;

        public AttackAction(GameObject gameObject, GameObject target) : base(gameObject)
        {
            attackObject = target;
            attackType = target.Type;
        }

        public override bool DoActionInNextTurn(VirtualGame virtualGame)
        {
            virtualGame.DoAttackDamage(gameObject, attackObject);
            return true;
        }

        public override void RemoveActionInNextTurn(VirtualGame virtualGame)
        {
            virtualGame.RevertAttackDamage(gameObject, attackObject);
        }

        public override void DoGameAction()
        {
            Elf elf = (Elf)gameObject;

            elf.Attack(attackObject);
        }

        public override string ToString()
        {
            return $"{GetType().Name} has target {attackObject} with attackType {attackType}";
        }
    }
}
