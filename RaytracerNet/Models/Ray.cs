namespace RaytracerNet
{
    using System.Numerics;

    public struct Ray
    {
        public Vector3 Origin;
        public Vector3 Dir;

        public Ray(Vector3 origin, Vector3 dir)
        {
            Origin = origin;
            Dir = dir;
        }

        public Vector3 PointAt( float t) => Origin + Dir * t;
    }
}