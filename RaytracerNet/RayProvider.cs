namespace RaytracerNet
{
    using System;
    using System.Numerics;

    public interface IRayProvider
    {
        public void Setup(Camera camera);
        public Ray GetRay(int x, int y);
    }
    
    public class RayProvider : IRayProvider
    {
        private Camera camera = new Camera(new Vector3(0, 0, 1f), 1000, 700, 30);
        private float imageAspectRatio;
        private float scale;

        public void Setup(Camera camera)
        {
            this.camera = camera;
            imageAspectRatio = camera.PixelsWidth / (float) camera.PixelHeight;
            scale = (float) Math.Tan(camera.Fov / 2 / 180f * Math.PI);
        }

        public Ray GetRay(int x, int y)
        {
            var pixelNdcX = (x + 0.5f) / camera.PixelsWidth;
            var pixelNdcY = (y + 0.5f) / camera.PixelHeight;

            var pixelScreenX = (pixelNdcX * 2 - 1) * imageAspectRatio;
            var pixelScreenY = 1 - 2 * pixelNdcY;

            var pixelCameraX = pixelScreenX * scale;
            var pixelCameraY = pixelScreenY * scale;

            var rayOrigin = camera.Origin;
            var rayDirection = new Vector3(pixelCameraX, pixelCameraY, -1) - rayOrigin;
            rayDirection = Vector3.Normalize(rayDirection);

            return new Ray(rayOrigin, rayDirection);
        }
    }
}