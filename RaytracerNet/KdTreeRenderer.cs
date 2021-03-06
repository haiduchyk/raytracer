namespace RaytracerNet
{
    using System;
    using System.Linq;
    using System.Numerics;
    using System.Threading.Tasks;
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
        private const int ScreenWidth = 4000;
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

            var renderParallelized = new ParallelizeRender();
            renderParallelized.Render(SetPixel, image);

            stopwatch.StopAndShow();

            return image;
        }

        private void SetPixel(Image image, int x, int y)
        {
            var ray = rayProvider.GetRay(x, y);
            image[y, x] = GetColor(ray);
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
            var barycentric = Vector3.Zero;
            
            kdTree.Root.Traverse(ray, ref tmin, ref trig, ref barycentric);
            var point = ray.PointAt(tmin);
            return CalculateColorFor(point, trig, barycentric);
        }
        
        private Color CalculateColorFor(Vector3 point, Trig trig, Vector3 barycentric)
        {
            var normal = barycentric.Z * trig.NA + barycentric.X * trig.NB + barycentric.Y * trig.NC;
            normal = Vector3.Normalize(normal);
            
            var lightDirection = Vector3.Normalize(lightPosition - point);
            var hitColor = Math.Max(0, Vector3.Dot(normal, lightDirection));
            var color = (normal + Vector3.One) * 0.5f * 255 * hitColor;
            
            return new Color( (int) color.X, (int) color.Y, (int) color.Z);
        }

    }
}