using ElfKingdom;

namespace SkillZ
{
    public class Mathf
    {
        // Returns the sine of angle /f/ in radians.
        public static float Sin(float f) { return (float)System.Math.Sin(f); }

        // Returns the cosine of angle /f/ in radians.
        public static float Cos(float f) { return (float)System.Math.Cos(f); }

        // Returns the tangent of angle /f/ in radians.
        public static float Tan(float f) { return (float)System.Math.Tan(f); }

        // Returns the arc-sine of /f/ - the angle in radians whose sine is /f/.
        public static float Asin(float f) { return (float)System.Math.Asin(f); }

        // Returns the arc-cosine of /f/ - the angle in radians whose cosine is /f/.
        public static float Acos(float f) { return (float)System.Math.Acos(f); }

        // Returns the arc-tangent of /f/ - the angle in radians whose tangent is /f/.
        public static float Atan(float f) { return (float)System.Math.Atan(f); }

        // Returns the angle in radians whose ::ref::Tan is @@y/x@@.
        public static float Atan2(float y, float x) { return (float)System.Math.Atan2(y, x); }

        // Returns square root of /f/.
        public static float Sqrt(float f) { return (float)System.Math.Sqrt(f); }

        // Returns the absolute value of /f/.
        public static float Abs(float f) { return (float)System.Math.Abs(f); }

        // Returns the absolute value of /value/.
        public static int Abs(int value) { return System.Math.Abs(value); }

        /// *listonly*
        public static float Min(float a, float b) { return a < b ? a : b; }
        // Returns the smallest of two or more values.
        public static float Min(params float[] values)
        {
            int len = values.Length;
            if (len == 0)
                return 0;
            float m = values[0];
            for (int i = 1; i < len; i++)
            {
                if (values[i] < m)
                    m = values[i];
            }
            return m;
        }

        /// *listonly*
        public static int Min(int a, int b) { return a < b ? a : b; }
        // Returns the smallest of two or more values.
        public static int Min(params int[] values)
        {
            int len = values.Length;
            if (len == 0)
                return 0;
            int m = values[0];
            for (int i = 1; i < len; i++)
            {
                if (values[i] < m)
                    m = values[i];
            }
            return m;
        }

        /// *listonly*
        public static float Max(float a, float b) { return a > b ? a : b; }
        // Returns largest of two or more values.
        public static float Max(params float[] values)
        {
            int len = values.Length;
            if (len == 0)
                return 0;
            float m = values[0];
            for (int i = 1; i < len; i++)
            {
                if (values[i] > m)
                    m = values[i];
            }
            return m;
        }

        /// *listonly*
        public static int Max(int a, int b) { return a > b ? a : b; }
        // Returns the largest of two or more values.
        public static int Max(params int[] values)
        {
            int len = values.Length;
            if (len == 0)
                return 0;
            int m = values[0];
            for (int i = 1; i < len; i++)
            {
                if (values[i] > m)
                    m = values[i];
            }
            return m;
        }

        // Returns /f/ raised to power /p/.
        public static float Pow(float f, float p) { return (float)System.Math.Pow(f, p); }

        // Returns e raised to the specified power.
        public static float Exp(float power) { return (float)System.Math.Exp(power); }

        // Returns the logarithm of a specified number in a specified base.
        public static float Log(float f, float p) { return (float)System.Math.Log(f, p); }

        // Returns the natural (base e) logarithm of a specified number.
        public static float Log(float f) { return (float)System.Math.Log(f); }

        // Returns the base 10 logarithm of a specified number.
        public static float Log10(float f) { return (float)System.Math.Log10(f); }

        // Returns the smallest integer greater to or equal to /f/.
        public static float Ceil(float f) { return (float)System.Math.Ceiling(f); }

        // Returns the largest integer smaller to or equal to /f/.
        public static float Floor(float f) { return (float)System.Math.Floor(f); }

        // Returns /f/ rounded to the nearest integer.
        public static float Round(float f) { return (float)System.Math.Round(f); }

        // Returns the smallest integer greater to or equal to /f/.
        public static int CeilToInt(float f) { return (int)System.Math.Ceiling(f); }

        // Returns the largest integer smaller to or equal to /f/.
        public static int FloorToInt(float f) { return (int)System.Math.Floor(f); }

        // Returns /f/ rounded to the nearest integer.
        public static int RoundToInt(float f) { return (int)System.Math.Round(f); }

        // Returns the sign of /f/.
        public static float Sign(float f) { return f >= 0F ? 1F : -1F; }

        // The infamous ''3.14159265358979...'' value (RO).
        public const float PI = (float)System.Math.PI;

        // Degrees-to-radians conversion constant (RO).
        public const float Deg2Rad = PI * 2F / 360F;

        // Radians-to-degrees conversion constant (RO).
        public const float Rad2Deg = 1F / Deg2Rad;

        // Clamps a value between a minimum float and maximum float value.
        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        // Clamps value between min and max and returns value.
        // Set the position of the transform to be that of the time
        // but never less than 1 or more than 3
        //
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        // Clamps value between 0 and 1 and returns value
        public static float Clamp01(float value)
        {
            if (value < 0F)
                return 0F;
            else if (value > 1F)
                return 1F;
            else
                return value;
        }

        // Interpolates between /a/ and /b/ by /t/. /t/ is clamped between 0 and 1.
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        // Interpolates between /a/ and /b/ by /t/ without clamping the interpolant.
        public static float LerpUnclamped(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        // Same as ::ref::Lerp but makes sure the values interpolate correctly when they wrap around 360 degrees.
        public static float LerpAngle(float a, float b, float t)
        {
            float delta = Repeat((b - a), 360);
            if (delta > 180)
                delta -= 360;
            return a + delta * Clamp01(t);
        }

        // Moves a value /current/ towards /target/.
        static public float MoveTowards(float current, float target, float maxDelta)
        {
            if (Mathf.Abs(target - current) <= maxDelta)
                return target;
            return current + Mathf.Sign(target - current) * maxDelta;
        }

        // Same as ::ref::MoveTowards but makes sure the values interpolate correctly when they wrap around 360 degrees.
        static public float MoveTowardsAngle(float current, float target, float maxDelta)
        {
            float deltaAngle = DeltaAngle(current, target);
            if (-maxDelta < deltaAngle && deltaAngle < maxDelta)
                return target;
            target = current + deltaAngle;
            return MoveTowards(current, target, maxDelta);
        }

        // Interpolates between /min/ and /max/ with smoothing at the limits.
        public static float SmoothStep(float from, float to, float t)
        {
            t = Mathf.Clamp01(t);
            t = -2.0F * t * t * t + 3.0F * t * t;
            return to * t + from * (1F - t);
        }

        //*undocumented
        public static float Gamma(float value, float absmax, float gamma)
        {
            bool negative = false;
            if (value < 0F)
                negative = true;
            float absval = Abs(value);
            if (absval > absmax)
                return negative ? -absval : absval;

            float result = Pow(absval / absmax, gamma) * absmax;
            return negative ? -result : result;
        }

        // Loops the value t, so that it is never larger than length and never smaller than 0.
        public static float Repeat(float t, float length)
        {
            return Clamp(t - Mathf.Floor(t / length) * length, 0.0f, length);
        }

        // PingPongs the value t, so that it is never larger than length and never smaller than 0.
        public static float PingPong(float t, float length)
        {
            t = Repeat(t, length * 2F);
            return length - Mathf.Abs(t - length);
        }

        // Calculates the ::ref::Lerp parameter between of two values.
        public static float InverseLerp(float a, float b, float value)
        {
            if (a != b)
                return Clamp01((value - a) / (b - a));
            else
                return 0.0f;
        }

        // Calculates the shortest difference between two given angles.
        public static float DeltaAngle(float current, float target)
        {
            float delta = Mathf.Repeat((target - current), 360.0F);
            if (delta > 180.0F)
                delta -= 360.0F;
            return delta;
        }

        //My custom functions here -- not from Unity!
        public static float GetAngle(MapObject l1, MapObject l2)
        {
            int yDifference = (l1.GetLocation().Row - l2.GetLocation().Row);

            float dir = 0;
            if (yDifference == 0)
            {
                dir = 90;
            }
            else
            {
                dir = Atan((l1.GetLocation().Col - l2.GetLocation().Col) / yDifference) * 180 / PI;
            }

            if (dir < 0)
            {
                dir += 180;
            }
            if (l1.GetLocation().Col < l2.GetLocation().Col)
            {
                dir += 180;
            }
            return dir;
        }

        //Given a location, an angle, and a distance, we can get a new location
        public static Location GetNewLocationFromLocation(MapObject source, float angle, float length)
        {
            length += 2;

            angle = angle * Deg2Rad;

            Location newLocation = new Location((int)(length * Cos(angle) + source.GetLocation().Row), (int)(length * Sin(angle) + source.GetLocation().Col));

            return newLocation;
            //return source.GetLocation().Towards(newLocation, length - 2); //doing this for potentially smoother locations
        }

        public static bool CirclesCollision(MapObject circle1, int radius1, MapObject circle2, int radius2)
        {
            return Pow(circle1.GetLocation().Col - circle2.GetLocation().Col, 2) + Mathf.Pow(circle2.GetLocation().Row - circle1.GetLocation().Row, 2) < Pow(radius1 + radius2, 2);
        }

        public static bool CircleLineCollision(MapObject circle, int radius, MapObject lineStart, MapObject lineEnd)
        {
            //Ripped from Stack Overflow (https://stackoverflow.com/questions/37224912/circle-line-segment-collision), converted from JavaScript to C#
            Location v1 = new Location(lineEnd.GetLocation().Row - lineStart.GetLocation().Row, lineEnd.GetLocation().Col - lineStart.GetLocation().Col);
            Location v2 = new Location(lineStart.GetLocation().Row - circle.GetLocation().Row, lineStart.GetLocation().Col - circle.GetLocation().Col);
            float b = (v1.Col * v2.Col + v1.Row * v2.Row);
            float c = 2 * (v1.Col * v1.Col + v1.Row * v1.Row);
            b *= -2;
            float d = Sqrt(b * b - 2 * c * (v2.Col * v2.Col + v2.Row * v2.Row - radius * radius));

            return !float.IsNaN(d); //if d is a valid number, there is a collision
            /*if (d != d) //if is NaN
            {
                return false; //no intercept
            }*/

            //Rom: this code below is for finding the intercept location. We just want to know if there is a collision, and we dont really care where it happens, so whatever.
            //If in the future we will need this, then remember that this code below has not been converted to C# yet!

            /*float u1 = (b - d) / c;  // these represent the unit distance of point one and two on the line
            float u2 = (b + d) / c;
            retP1 = { };   // return points
            retP2 = { }
            ret = []; // return array
            if (u1 <= 1 && u1 >= 0)
            {  // add point if on the line segment
                retP1.x = line.p1.x + v1.x * u1;
                retP1.y = line.p1.y + v1.y * u1;
                ret[0] = retP1;
            }
            if (u2 <= 1 && u2 >= 0)
            {  // second add point if on the line segment
                retP2.x = line.p1.x + v1.x * u2;
                retP2.y = line.p1.y + v1.y * u2;
                ret[ret.length] = retP2;
            }
            return ret;*/
        }

        /*function inteceptCircleLineSeg(circle, line){
            var a, b, c, d, u1, u2, ret, retP1, retP2, v1, v2;
            v1 = {};
            v2 = {};
            v1.x = line.p2.x - line.p1.x;
            v1.y = line.p2.y - line.p1.y;
            v2.x = line.p1.x - circle.center.x;
            v2.y = line.p1.y - circle.center.y;
            b = (v1.x * v2.x + v1.y * v2.y);
            c = 2 * (v1.x * v1.x + v1.y * v1.y);
            b *= -2;
            d = Math.sqrt(b * b - 2 * c * (v2.x * v2.x + v2.y * v2.y - circle.radius * circle.radius));
            if(isNaN(d)){ // no intercept
                return [];
            }
            u1 = (b - d) / c;  // these represent the unit distance of point one and two on the line
            u2 = (b + d) / c;    
            retP1 = {};   // return points
            retP2 = {}  
            ret = []; // return array
            if(u1 <= 1 && u1 >= 0){  // add point if on the line segment
                retP1.x = line.p1.x + v1.x * u1;
                retP1.y = line.p1.y + v1.y * u1;
                ret[0] = retP1;
            }
            if(u2 <= 1 && u2 >= 0){  // second add point if on the line segment
                retP2.x = line.p1.x + v1.x * u2;
                retP2.y = line.p1.y + v1.y * u2;
                ret[ret.length] = retP2;
            }       
            return ret;
        }*/

        public static bool IsLocationOnRightOfLine(Location lineStart, Location lineEnd, Location location)
        {
            float d = (location.Col - lineStart.Col) * (lineEnd.Row - lineStart.Row) - (location.Row - lineStart.Row) * (lineEnd.Col - lineStart.Col);
            return d < 0;
        }
    }
}
