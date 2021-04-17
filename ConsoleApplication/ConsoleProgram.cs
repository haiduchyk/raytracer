namespace ConsoleApplication
{
    using System.Collections.Generic;
    using Converter;
    using RaytracerNet;

    public interface IProgram
    {
        void ProcessInput(List<string> args);
    }

    public class ConsoleProgram : IProgram
    {
        private readonly InputProcessor inputProcessor;
        private readonly ObjReader objReader;
        private readonly IRenderer renderer;
        private readonly IUniversalImageConverter universalImageConverter;

        public ConsoleProgram(
            InputProcessor inputProcessor,
            IUniversalImageConverter universalImageConverter,
            ObjReader objReader,
            IRenderer renderer
        )
        {
            this.inputProcessor = inputProcessor;
            this.universalImageConverter = universalImageConverter;
            this.objReader = objReader;
            this.renderer = renderer;
        }

        public void ProcessInput(List<string> args)
        {
            inputProcessor.SetupKeyWords(args);
            var path = inputProcessor.Source.Value;
            var mesh = objReader.Read(path);
            
            mesh.Transform(TransMatrices.TranslateM, TransMatrices.RotateM, TransMatrices.ScaleM);
            var image = renderer.Render(mesh);
            
            var goalExtension = inputProcessor.OutputExtension;
            var goalPath = inputProcessor.OutputPath.Value;
            
            universalImageConverter.WriteImage(image, path, goalExtension, goalPath);
        }
    }
}