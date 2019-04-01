using ElfKingdom;

namespace SkillZ
{
    public class Circle
    {
        private Location center;
        private float radius;

        public Circle(Location center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public Circle(MapObject center, float radius)
        {
            this.center = center.GetLocation();
            this.radius = radius;
        }

        public Location GetCenter()
        {
            return center;
        }

        public float GetRadius()
        {
            return radius;
        }

        public bool IsLocationInside(MapObject source)
        {
            return source.DistanceF(center) <= radius;
        }

        public override string ToString()
        {
            return $"({center.Row}, {center.Col}) - radius = {radius}";
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || !GetType().Equals(obj.GetType())) return false;

            Circle other = (Circle)obj;

            return center.Col == other.center.Col && center.Row == other.center.Row && radius == other.radius;
        }

        public override int GetHashCode()
        {
            return center.Col;
        }
    }
}
