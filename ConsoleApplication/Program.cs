namespace CGLab1
{
    using System;
    using System.Linq;
    using ConsoleApplication;
    using Converter;
    using Microsoft.Extensions.DependencyInjection;

    public static class Program
    {
        private static IServiceProvider serviceProvider;

        public static void Main(string[] args)
        {
            RegisterServices();
            RunApp(args);
            DisposeServices();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IProgram, ConsoleProgram>();
            services.AddSingleton<IFileWorker, FileWorker>();
            services.AddSingleton<ImageConvertersProvider>();
            services.AddSingleton<IUniversalImageConverter, UniversalImageConverter>();
            services.AddSingleton<InputProcessor>();

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