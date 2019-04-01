using ElfKingdom;

namespace SkillZ
{
    public class LocationF
    {
        public float x;
        public float y;

        public LocationF() { }

        public LocationF(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static LocationF operator /(LocationF location, float b)
        {
            location.x /= b;
            location.y /= b;
            return location;
        }

        public static LocationF operator -(LocationF location, LocationF b)
        {
            location.x -= b.x;
            location.y -= b.y;
            return location;
        }

        public static LocationF operator +(LocationF location, LocationF b)
        {
            location.x += b.x;
            location.y += b.y;
            return location;
        }

        public static LocationF operator *(LocationF location, float b)
        {
            location.x *= b;
            location.y *= b;
            return location;
        }

        public float Dot(LocationF l2)
        {
            return x * l2.x + y * l2.y;
        }

        public float Magnitude()
        {
            return Mathf.Sqrt(y * y + x * x);
        }

        public LocationF Normalized()
        {
            float magnitude = Magnitude();
            if (magnitude > 0)
            {
                return this / magnitude;
            }
            else
            {
                return new LocationF(0, 0);
            }
        }

        public Location GetIntLocation()
        {
            return new Location(Mathf.RoundToInt(y), Mathf.RoundToInt(x));
        }

        public float Distance(MapObject target)
        {
            return Mathf.Sqrt(Mathf.Pow(x - target.GetLocation().Col, 2) + Mathf.Pow(y - target.GetLocation().Row, 2));
        }

        public bool InRange(MapObject target, float distance)
        {
            return Distance(target) <= distance;
        }

        public override string ToString()
        {
            return $"LocationF({x},{y})";
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null) return false;

            if(obj is Location)
            {
                if (!GetType().Equals(obj.GetType())) return false;

                Location other = (Location)obj;

                return Mathf.RoundToInt(x) == other.Col && Mathf.RoundToInt(y) == other.Row;
            }
            else
            {
                if (!GetType().Equals(obj.GetType())) return false;

                LocationF other = (LocationF)obj;

                return x == other.x && y == other.y;
            }
        }

        public override int GetHashCode()
        {
            return Mathf.RoundToInt(x);
        }
    }
}
