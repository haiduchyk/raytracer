namespace Converter
{
    public struct Color
    {
        public readonly byte R;
        public readonly byte G;
        public readonly byte B;
        public readonly byte A;

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(int r, int g, int b, int a = 255)
        {
            R = (byte)  r;
            G = (byte)  g;
            B = (byte)  b;
            A = (byte)  a;
        }
        
        public byte[] GetBytes()
        {
            return new[] {R, G, B};
        }

        public static readonly Color White = new Color(255, 255, 255);
        public static readonly Color Black = new Color(0, 0, 0);
    }
}