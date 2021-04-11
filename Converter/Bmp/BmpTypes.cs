namespace Converter
{
    public class FileHeader
    {
        public short Type;
        public int Size;
        public int Reserved;
        public int Offset;
    };

    public class InfoHeader
    {
        public int Size;
        public int Width;
        public int Height;
        public short Planes;
        public short BitCount;
        public int Compression;
        public int SizeImage;
        public int XPelsPerMeter;
        public int YPelsPerMeter;
        public int ColorUsed;
        public int ColorImportant;
    };

    public class PixelData
    {
        public int RedMask;
        public int GreenMask;
        public int BlueMask;
        public int AlphaMask;
    };

    public class DefaultBmpHeaders
    {
        private static FileHeader FileHeader => new FileHeader
        {
            Type = 0x4D42,
            Reserved = 0,
            Offset = 122
        };

        private static InfoHeader InfoHeader => new InfoHeader
        {
            Size = 108,
            Planes = 1,
            BitCount = 24,
            Compression = 0,
            XPelsPerMeter = 11811,
            YPelsPerMeter = 11811,
            ColorUsed = 0,
            ColorImportant = 0,
        };
        
        public static FileHeader GetFileHeader(Image image)
        {
            var header = FileHeader;
            header.Size = image.Height * image.Width * 3 + header.Offset;
            return header;
        }

        public static InfoHeader GetInfoHeader(Image image)
        {
            var header = InfoHeader;
            
            header.Width = image.Width;
            header.Height = image.Height;
            header.SizeImage = image.Height * image.Width * 3;

            return header;
        }
    }
}