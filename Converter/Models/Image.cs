namespace Converter
{
    public class Image
    {
        public int Height => pixels.GetLength(0);
        public int Width => pixels.GetLength(1);

        private readonly Color[,] pixels;

        public Image(int height, int width)
        {
            pixels = new Color[height, width];
        }

        public Color this[int i, int j]
        {
            get => pixels[i, j];
            set => pixels[i, j] = value;
        }
    }
}