namespace RaytracerNet
{
    using System.Numerics;

    public struct Camera
    {
        public Vector3 Origin;
        public int PixelsWidth;
        public int PixelHeight;
        public float Fov;


        public Camera(Vector3 origin, int pixelsWidth, int pixelHeight, float fov)
        {
            Origin = origin;
            PixelsWidth = pixelsWidth;
            PixelHeight = pixelHeight;
            Fov = fov;
        }
    }
}