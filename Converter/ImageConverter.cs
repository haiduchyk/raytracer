namespace Converter
{
    public interface IImageConverter : IImageReader, IImageWriter
    {
        string Extension { get; }
    }

    public interface IImageReader
    {
        Image Decode(byte[] bytes);
    }

    public interface IImageWriter
    {
        byte[] Encode(Image image);
    }
}