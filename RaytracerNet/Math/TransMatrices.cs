namespace RaytracerNet
{
    using System.Numerics;

    public static class TransMatrices
    {
        // Cow
        public static Matrix4x4 TranslateM = new Matrix4x4(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, -5,
            0, 0, 0, 1
        );
        
        public static Matrix4x4 RotateM = new Matrix4x4(
            1, 0, 0, 0,
            0, 0, 1, 0,
            0, -1, 0, 0,
            0, 0, 0, 1
        );
        
        public static Matrix4x4 ScaleM = new Matrix4x4(
            2, 0, 0, 0,
            0, 2, 0, 0,
            0, 0, 2, 0,
            0, 0, 0, 1
        );
        
        // Teapot
        // public static Matrix4x4 TranslateM = new Matrix4x4(
        //     1, 0, 0, 0,
        //     0, 1, 0, 0,
        //     0, 0, 1, -200,
        //     0, 0, 0, 1
        // );
        //
        // public static Matrix4x4 RotateM = new Matrix4x4(
        //     1, 0, 0, 0,
        //     0, 0, 1, 0,
        //     0, -1, 0, 0,
        //     0, 0, 0, 1
        // );
        //
        // public static Matrix4x4 ScaleM = new Matrix4x4(
        //     2, 0, 0, 0,
        //     0, 2, 0, 0,
        //     0, 0, 2, 0,
        //     0, 0, 0, 1
        // );
    }
}