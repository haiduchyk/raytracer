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