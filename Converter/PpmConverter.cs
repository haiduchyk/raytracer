namespace Converter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Converter;

    public class PpmConverter : IImageConverter
    {
        public string Extension => "ppm";
        private const string Format = "P3";
        
        private List<string> lines;
        private Color[,] pixels;
        private int maxColorValue;
        private const int ColorsAmountInPixel = 3;
        private readonly Regex whitespace = new Regex(@"\s+");

        public byte[] Encode(Image image)
        {
            var header = GetHeader(image);
            var colors = ConvertImageToString(image);
            var data = header + colors;
            return Encoding.ASCII.GetBytes(data);;
        }

        private string GetHeader(Image image)
        {
            var format = $"{Format}\n";
            var sizes = $"{image.Width} {image.Height}\n";
            var maxColorVal = "255\n";
            
            return format + sizes + maxColorVal;
        }


        private string ConvertImageToString(Image image)
        {
            var result = new StringBuilder();
            for (var i = 0; i < image.Height; i++)
            {
                for (var j = 0; j < image.Width; j++)
                {
                    var color = image[i, j];
                    result.Append(color.R).Append(" ")
                        .Append(color.G).Append(" ")
                        .Append(color.B).Append(" ");
                }
                result.AppendLine();
            }
            return result.ToString();
        }

        public Image Decode(byte[] bytes)
        {
            var data = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            SetupLines(data);
            RemoveComments();
            CheckFormat();
            SetupMaxColorValue();

            var (height, width) = GetSizes();

            var image = CreateImage(height, width);
            
            return image;
        }

        private void SetupLines(string data)
        {
            lines = data.Split('\n').ToList();
        }

        private void RemoveComments()
        {
            lines.RemoveAll(l => l[0] == '#');
        }

        private void CheckFormat()
        {
            if (lines[0] != Format) throw new Exception("Invalid format");
        }

        private (int height, int width) GetSizes()
        {
            var line = lines[1];
            var sizes = line.Split(" ");

            if (sizes.Length != 2)
            {
                throw new Exception("Invalid sizes dimension");
            }

            if (!int.TryParse(sizes[0], out var width))
            {
                throw new Exception("Invalid width format");
            }

            if (!int.TryParse(sizes[1], out var height))
            {
                throw new Exception("Invalid height format");
            }

            return (height, width);
        }

        private void SetupMaxColorValue()
        {
            if (!int.TryParse(lines[2], out maxColorValue))
            {
                throw new Exception("Invalid maximum color value (Maxval) format");
            }

            if (maxColorValue <= 0 || maxColorValue >= 65536)
            {
                throw new Exception("Maximum color value (Maxval) must be in range (0; 65536)");
            }
        }

        private Image CreateImage(int height, int width)
        {
            var colorsBytes = GetColorBytes();
            var image = new Image(height, width);

            var enumerable = colorsBytes as byte[] ?? colorsBytes.ToArray();
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var index = i * width + j;
                    var colorBytes = enumerable
                        .Skip(index * ColorsAmountInPixel)
                        .Take(ColorsAmountInPixel).ToList();
                    
                    var color = ConvertToColor(colorBytes);
                    image[i, j] = color;
                }
            }

            return image;
        }

        private Color ConvertToColor(List<byte> colorComponents)
        {
            if (colorComponents.Count != 3)
            {
                throw new Exception("Invalid color dimension");
            }

            var r = colorComponents[0];
            var g = colorComponents[1];
            var b = colorComponents[2];

            return new Color(r, g, b);
        }

        private IEnumerable<byte> GetColorBytes()
        {
            var colorRows = lines.Skip(3);
            return colorRows
                .SelectMany(GetColorNumbers)
                .Select(ConvertToColorByte);
        }

        private IEnumerable<string> GetColorNumbers(string line)
        {
            return whitespace.Split(line).Where(number => !string.IsNullOrEmpty(number));
        }

        private byte ConvertToColorByte(string colorString)
        {
            if (!float.TryParse(colorString, out var colorNumber))
            {
                throw new Exception("Invalid color format");
            }

            if (colorNumber > maxColorValue || colorNumber < 0)
            {
                throw new Exception($"Color number must be in range (0; {maxColorValue})");
            }

            return (byte) (colorNumber / maxColorValue * 255);
        }
    }
}