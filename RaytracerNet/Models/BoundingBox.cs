namespace RaytracerNet
{
    using System.Numerics;

    public class BoundingBox
    {
        public Vector3 Mins;
        public Vector3 Maxs;
        
        public Vector3 Size;

        public BoundingBox() { }

        public BoundingBox(Trig t)
        {
            SetBounds(t);
            UpdateSize();
        }

        public void SetBounds(Trig t)
        {
            Mins = t.Mins;
            Maxs = t.Maxs;
            
            UpdateSize();
        }

        public void UpdateSize()
        {
            Size.X = Mins.X < Maxs.X ? Maxs.X - Mins.X : Mins.X - Maxs.X;
            Size.Y = Mins.Y < Maxs.Y ? Maxs.Y - Mins.Y : Mins.Y - Maxs.Y;
            Size.Z = Mins.Z < Maxs.Z ? Maxs.Z - Mins.Z : Mins.Z - Maxs.Z;
        }


    };
}