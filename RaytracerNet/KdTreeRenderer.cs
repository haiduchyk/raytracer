namespace RaytracerNet
{
    using System;
    using System.Linq;
    using System.Numerics;
    using ConsoleApplication;
    using Converter;

    public interface IRenderer
    {
        Image Render(Mesh mesh);
    }

    public class KdTreeRenderer : IRenderer
    {
        private readonly IProgressBar progressBar;
        private readonly IStopwatch stopwatch;
        private readonly IRayProvider rayProvider;
        
        private KdTree kdTree;
        private const int ScreenWidth = 1000;
        private const int ScreenHeight = (int) (ScreenWidth / 1.5);
        private Vector3 lightPosition = new Vector3(10, 10, 0);
        private Camera camera = new Camera(new Vector3(0, 0, 1f), ScreenWidth, ScreenHeight, 30);

        public KdTreeRenderer(IProgressBar progressBar, IStopwatch stopwatch, IRayProvider rayProvider)
        {
            this.progressBar = progressBar;
            this.stopwatch = stopwatch;
            this.rayProvider = rayProvider;
        }

        public Image Render(Mesh mesh)
        {
            BuildTree(mesh);
            
            rayProvider.Setup(camera);
            var image = new Image(ScreenHeight, ScreenWidth);
            stopwatch.Start();

            for (var y = 0; y < image.Height; y++)
            {
                TryUpdateProgress(y, ScreenHeight);
                for (var x = 0; x < image.Width; x++)
                {
                    var ray = rayProvider.GetRay(x, y);
                    image[y, x] = GetColor(ray);
                }
            }

            stopwatch.StopAndShow();

            return image;
        }

        private void BuildTree(Mesh mesh)
        {
            stopwatch.Start();
            kdTree = new KdTree(mesh.Trigs.ToList());
            stopwatch.StopAndShow("KdTree");
        }

        private void TryUpdateProgress(int progress, int total)
        {
            if (progress % 10 == 0)
            {
                progressBar.Draw(progress, total, "Render Progress");
            }
        }


        private Color GetColor(Ray ray)
        {
            var tmin = float.MaxValue;
            var trig = new Trig();
                    
            kdTree.Root.Traverse(ray, ref tmin, ref trig);
            var point = ray.PointAt(tmin);
            return CalculateColorFor(point, trig);
        }
        
        private Color CalculateColorFor(Vector3 point, Trig trig)
        {
            var barycentric = trig.Barycentric(point);
            var normal = barycentric.X * trig.NA + barycentric.Y * trig.NB + barycentric.Z * trig.NC;

            var lightDirection = Vector3.Normalize(lightPosition - point);
            var hitColor = Math.Max(0, Vector3.Dot(normal, lightDirection));

            var color = (normal + Vector3.One) * 0.5f * 255 * hitColor;

            return new Color((int) color.X, (int) color.Y, (int) color.Z);
        }

    }
}