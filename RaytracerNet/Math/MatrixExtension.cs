namespace RaytracerNet
{
    using System.Numerics;

    public static class MatrixExtension
    {
        public static Vector4 MultiplyBy(this Matrix4x4 m, Vector4 v)
        {
            return new Vector4(
                m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z + m.M14 * v.W,
                m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z + m.M24 * v.W,
                m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z + m.M34 * v.W,
                m.M41 * v.X + m.M42 * v.Y + m.M43 * v.Z + m.M44 * v.W
            );
        } 
    }
}