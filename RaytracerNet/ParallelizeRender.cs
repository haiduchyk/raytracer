namespace RaytracerNet
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Converter;

    public class ParallelizeRender
    {
        private readonly int maxThreadsAmount = Environment.ProcessorCount;

        private Image image;
        private Action<Image, int, int> pixelSetter;

        public void Render(Action<Image, int, int> pixelSetter, Image image)
        {
            this.image = image;
            this.pixelSetter = pixelSetter;

            var tasks = new List<Task>();

            for (var i = 0; i < maxThreadsAmount; i++)
            {
                var i1 = i;
                var task = Task.Factory.StartNew(() => RunThread(i1));
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
        }

        private void RunThread(int offset)
        {
            for (var i = offset; i < image.Height * image.Width; i += maxThreadsAmount)
            {
                var y = i / image.Width;
                var x = i - image.Width * y;
                pixelSetter(image, x, y);
            }
        }
    }
}