namespace Converter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;

    public class BmpConvertor : IImageConverter
    {
        private byte[] data;
        public string Extension => "bmp";

        private int currentIndex;

        public Image Decode(byte[] bytes)
        {
            data = bytes;
            var fileHeader = SetupFields<FileHeader>();
            var infoHeader = SetupFields<InfoHeader>();
            var pixelData = SetupFields<PixelData>();

            currentIndex = fileHeader.Offset;

            return ReadPixelArray(infoHeader, pixelData);
        }

        private T SetupFields<T>() where T : new()
        {
            var header = new T();

            var fields = header.GetType().GetFields();

            foreach (var field in fields)
            {
                var size = Marshal.SizeOf(field.FieldType);
                var valueInBytes = ReadBytes(size);
                var integerValue = ConvertLittleEndian(valueInBytes);
                var value = Convert.ChangeType(integerValue, field.FieldType);
                field.SetValue(header, value);
            }

            return header;
        }

        private int ConvertLittleEndian(byte[] array)
        {
            var integer = 0;

            for (var i = array.Length - 1; i >= 0; i--)
            {
                integer <<= 8;
                integer |= array[i];
            }

            return integer;
        }


        private byte[] ReadBytes(int amount)
        {
            var result = data.Skip(currentIndex).Take(amount).ToArray();
            currentIndex += amount;
            return result;
        }

        private Image ReadPixelArray(InfoHeader infoHeader, PixelData pixelData)
        {
            var image = new Image(infoHeader.Height, infoHeader.Width);

            var bytesPerPixel = infoHeader.BitCount / 8;

            var redMaskShift = CalculateShiftToRight(pixelData.RedMask);
            var greenMaskShift = CalculateShiftToRight(pixelData.GreenMask);
            var blueMaskShift = CalculateShiftToRight(pixelData.BlueMask);
            var alphaMaskShift = CalculateShiftToRight(pixelData.AlphaMask);

            var redChannelMultiplier = 255 / (byte) (pixelData.RedMask >> redMaskShift);
            var greenChannelMultiplier = 255 / (byte) (pixelData.GreenMask >> greenMaskShift);
            var blueChannelMultiplier = 255 / (byte) (pixelData.BlueMask >> blueMaskShift);
            var alphaChannelMultiplier = 0;

            if (pixelData.AlphaMask != 0)
            {
                alphaChannelMultiplier = 255 / (byte) (pixelData.AlphaMask >> alphaMaskShift);
            }


            for (var i = 0; i < image.Height; i++)
            {
                for (var j = 0; j < image.Width; j++)
                {
                    var pixelBits = 0;
                    if (bytesPerPixel > 4)
                    {
                        throw new Exception($"Invalid amount of bytes per pixel: {bytesPerPixel}");
                    }

                    var pixelBytes = (ReadBytes(bytesPerPixel));
                    pixelBits |= ConvertLittleEndian(pixelBytes);

                    var r = (byte) (((pixelBits & pixelData.RedMask) >> redMaskShift) * redChannelMultiplier);
                    var g = (byte) (((pixelBits & pixelData.GreenMask) >> greenMaskShift) * greenChannelMultiplier);
                    var b = (byte) (((pixelBits & pixelData.BlueMask) >> blueMaskShift) * blueChannelMultiplier);


                    var a = (byte) (((pixelBits & pixelData.AlphaMask) >> alphaMaskShift) * alphaChannelMultiplier);

                    var color = pixelData.AlphaMask != 0 ? new Color(r, g, b) : new Color(r, g, b, a);

                    image[infoHeader.Height - 1 - i, j] = color;
                }
            }

            return image;
        }

        public int CalculateShiftToRight(int value)
        {
            if (value == 0) return 0;

            var shift = 0;

            while ((value & 0b1) != 1)
            {
                value >>= 1;
                shift += 1;
            }

            return shift;
        }


        public byte[] Encode(Image image)
        {
            var result = new List<byte>();
            
            var fileHeader = DefaultBmpHeaders.GetFileHeader(image);
            var infoHeader = DefaultBmpHeaders.GetInfoHeader(image);
            var pixelData = new PixelData();

            WriteData(fileHeader, result);
            WriteData(infoHeader, result);
            WriteData(pixelData, result);
            
            result.AddRange(new byte[fileHeader.Offset - result.Count]);
            
            ConvertImage(image, result);
            
            return result.ToArray();
        }
        

        private void WriteData(object data, List<byte> bytes)
        {
            var fields = data.GetType().GetFields();

            foreach (var field in fields)
            {
                var size = Marshal.SizeOf(field.FieldType);
            
                var value = Convert.ToInt32(field.GetValue(data));
                var valueInBytes = BitConverter.GetBytes(value).Take(size);
                
                bytes.AddRange(valueInBytes);
            }
        }

        private void ConvertImage(Image image, List<byte> bytes)
        {
            var rowAlignment = (image.Width * 3) % 4;
            
            for (var i = 0; i < image.Height; i++)
            {
                for (var j = 0; j < image.Width; j++)
                {
                    var color = image[image.Height - 1 - i, j];
                    
                    bytes.AddRange(color.GetBytes().Reverse());
                }
                bytes.AddRange(new byte[rowAlignment]);
            }
        }
    }
}