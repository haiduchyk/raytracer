namespace ConsoleApplication
{
    using System.Collections.Generic;
    using Converter;
    public interface IProgram
    {
        void ProcessInput(List<string> args);
    }
    public class ConsoleProgram : IProgram
    {
        private readonly InputProcessor inputProcessor;
        private readonly IUniversalImageConverter universalImageConverter;
        public ConsoleProgram(InputProcessor inputProcessor, IUniversalImageConverter universalImageConverter)
        {
            this.inputProcessor = inputProcessor;
            this.universalImageConverter = universalImageConverter;
        }
        public void ProcessInput(List<string> args)
        {
            inputProcessor.SetupKeyWords(args);
            var path = inputProcessor.Source.Value;
            var extension = inputProcessor.SourceExtension;
            var goalExtension = inputProcessor.Goal.Value;
            var goalPath = inputProcessor.Output.Value;
            
            universalImageConverter.TryToConvert(path, extension, goalExtension, goalPath);
        }
    }
}