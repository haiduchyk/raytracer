namespace RaytracerNet
{
    using System.Numerics;

    public struct Trig
    {
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;
        
        public Vector3 NA;
        public Vector3 NB;
        public Vector3 NC;
        
        public Vector3 Center;
        public Vector3 Mins;
        public Vector3 Maxs;

        
        public Trig(Vector3 a, Vector3 b, Vector3 c, Vector3 na, Vector3 nb, Vector3 nc)
        {
            A = a;
            B = b;
            C = c;
            
            NA = na;
            NB = nb;
            NC = nc;

            Center = new Vector3();
            Mins = new Vector3();
            Maxs = new Vector3();
            
            ComputeBounds();
            ComputeCentroid();
        }
        
        private void ComputeCentroid()
        {
            Center.X = (A.X + B.X + C.X) / 3;
            Center.Y = (A.Y + B.Y + C.Y) / 3;
            Center.Z = (A.Z + B.Z + C.Z) / 3;
        }

        private void ComputeBounds()
        {
            Mins.X = Geometry.GetMin(A.X, B.X, C.X);
            Mins.Y = Geometry.GetMin(A.Y, B.Y, C.Y);
            Mins.Z = Geometry.GetMin(A.Z, B.Z, C.Z);

            Maxs.X = Geometry.GetMax(A.X, B.X, C.X);
            Maxs.Y = Geometry.GetMax(A.Y, B.Y, C.Y);
            Maxs.Z = Geometry.GetMax(A.Z, B.Z, C.Z);
        }
        
        public Vector3 Barycentric(Vector3 point)
        {
            Vector3 v0 = B - A, v1 = C - A, v2 = point - A;
            
            var d00 = Vector3.Dot(v0, v0);
            var d01 = Vector3.Dot(v0, v1);
            var d11 = Vector3.Dot(v1, v1);
            var d20 = Vector3.Dot(v2, v0);
            var d21 = Vector3.Dot(v2, v1);
            var invDenom = 1 / (d00 * d11 - d01 * d01);
            
            var v = (d11 * d20 - d01 * d21) * invDenom;
            var w = (d00 * d21 - d01 * d20) * invDenom;
            var u = 1 - v - w;

            return new Vector3(u, v, w);
        }

        public static int CompareByPosition(Trig t1, Trig t2, int axes)
        {
            var v1 = t1.Center.GetByIndex(axes);
            var v2 = t2.Center.GetByIndex(axes);
            
            if (v1 > v2) return 1;
            if (v1 < v2) return -1;
            return 0;
        }

        public override string ToString()
        {
            return $"a:{A}; b: {B}; c: {C}";
        }
    }
}