namespace Converter
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IUniversalImageConverter
    {
        List<string> GetAvailableExtensions();
        void TryToConvert(string path, string extension, string goalExtension, string goalPath);
        void WriteImage(Image image, string path, string goalExtension, string goalPath);
    }

    public class UniversalImageConverter : IUniversalImageConverter
    {
        private List<IImageConverter> imageConverters;

        private IFileWorker fileWorker;

        public UniversalImageConverter(ImageConvertersProvider imageConvertersProvider, IFileWorker fileWorker)
        {
            imageConverters = imageConvertersProvider.ImageConverters;
            this.fileWorker = fileWorker;
        }

        public List<string> GetAvailableExtensions()
        {
            return imageConverters.Select(i => i.Extension).ToList();
        }

        public void TryToConvert(string path, string extension, string goalExtension, string goalPath)
        {
            var converter = imageConverters.First(c => c.Extension == extension);
            var file = fileWorker.Read(path);
            var image = converter.Decode(file);
            WriteImage(image, path, goalExtension, goalPath);
        }

        public void WriteImage(Image image, string path, string goalExtension, string goalPath)
        {
            var goalConvertor = imageConverters.First(c => c.Extension == goalExtension);
            var result = goalConvertor.Encode(image);
            fileWorker.Write(result, goalPath ?? path, goalExtension);
        }
    }
}