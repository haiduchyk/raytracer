namespace Root
{
    using System;
    using System.Linq;
    using ConsoleApplication;
    using Converter;
    using Microsoft.Extensions.DependencyInjection;
    using RaytracerNet;

    public static class Program
    {
        private static IServiceProvider serviceProvider;

        public static void Main(string[] args)
        {
            args = new[]
            {
                // "--source=simplecow.obj",
                "--source=cow.obj",
                // "--source=dragon3.obj",
                // "--source=teapot.obj",
                // "--source=car4.obj",
                "--output=rendered.bmp"
            };
            
            RegisterServices();
            RunApp(args);
            DisposeServices();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IProgram, ConsoleProgram>();
            services.AddSingleton<IProgressBar, ProgressBar>();
            services.AddSingleton<IStopwatch, ConsoleStopwatch>();
            
            services.AddSingleton<IFileWorker, FileWorker>();
            services.AddSingleton<IRayProvider, RayProvider>();
            services.AddSingleton<ImageConvertersProvider>();
            services.AddSingleton<IUniversalImageConverter, UniversalImageConverter>();
            services.AddSingleton<InputProcessor>();
            services.AddSingleton<ObjReader>();
            services.AddSingleton<IRenderer, KdTreeRenderer>();

            serviceProvider = services.BuildServiceProvider(true);
        }

        private static void RunApp(string[] args)
        {
            var scope = serviceProvider.CreateScope();
            var consoleApp = scope.ServiceProvider.GetService<IProgram>();
            consoleApp.ProcessInput(args.ToList());
        }

        private static void DisposeServices()
        {
            if (serviceProvider is IDisposable disposable)
                disposable.Dispose();
        }
    }
}