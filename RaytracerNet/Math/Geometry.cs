namespace RaytracerNet
{
    using System.Numerics;

    public static class Geometry
    {
        public static bool ThereIsIntersection(Vector3 rayOrigin, Vector3 rayVector, Trig inTriangle, out float t)
        {
            t = int.MaxValue;
            
            var vertex0 = inTriangle.A;
            var vertex1 = inTriangle.B;
            var vertex2 = inTriangle.C;
            var edge1 = vertex1 - vertex0;
            var edge2 = vertex2 - vertex0;
            var h = Vector3.Cross(rayVector, edge2);

            var a = Vector3.Dot(edge1, h);
            var EPSILON = 1e-5f;
            if (a > -EPSILON && a < EPSILON)
            {
                return false;
            }

            var f = 1 / a;
            var s = rayOrigin - vertex0;
            var u = f * Vector3.Dot(s, h);
            if (u < 0.0 || u > 1.0)
            {
                return false;
            }

            var q = Vector3.Cross(s, edge1);
            var v = f * Vector3.Dot(rayVector, q);
            if (v < 0.0 || u + v > 1.0)
            {
                return false;
            }

            // At this stage we can compute t to find out where the intersection point is on the line.
            t = f * Vector3.Dot(edge2, q);
            return t > EPSILON;
        }

        public static float GetMin(float a, float b, float c)
        {
            return a < b ? (a < c ? a : c) : (b < c ? b : c);
        }
        
        public static float GetMax(float a, float b, float c)
        {
            return a > b ? (a > c ? a : c) : (b > c ? b : c);
        }
    }
}