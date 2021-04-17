namespace RaytracerNet
{
    using System;
    using System.Diagnostics;

    public interface IStopwatch
    {
        void Start();
        void StopAndShow(string info = "");
    }
    
    public class ConsoleStopwatch : IStopwatch
    {
        private Stopwatch watch;

        public void Start()
        {
            watch = Stopwatch.StartNew();
        }

        public void StopAndShow(string info = "a")

        {
            watch.Stop();
            Console.WriteLine($"{info}:{watch.ElapsedMilliseconds} ms");
        }
    }
}