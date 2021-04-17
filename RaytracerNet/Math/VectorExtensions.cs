namespace RaytracerNet
{
    using System;
    using System.Numerics;

    public static class VectorExtensions
    {
        public static int GetIndexOfMaxValueOfThree(this Vector3 v)
        {
            return v.X > v.Y ? (v.X > v.Z ? 0 : 2) : (v.Y > v.Z ? 1 : 2);
        }

        public static float GetByIndex(this Vector3 v, int index)
        {
            return index switch
            {
                0 => v.X,
                1 => v.Y,
                2 => v.Z,
                _ => throw new IndexOutOfRangeException()
            };
        }
    }
}