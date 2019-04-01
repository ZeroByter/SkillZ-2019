using ElfKingdom;

namespace SkillZ
{
    public abstract class Heuristic
    {
        public float weight = 1f;

        public Heuristic(float weight)
        {
            this.weight = weight;
        }

        public abstract float GetScore(VirtualGame virtualGame);

        public virtual void UpdateState(Game game) { }

        public float GetWeightedScore(VirtualGame virtualGame)
        {
            return GetScore(virtualGame) * weight;
        }

        public override string ToString()
        {
            return $"Heuristic {GetType().Name} has weight {weight}";
        }
    }
}
