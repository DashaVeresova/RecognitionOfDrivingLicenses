using System;
using System.Drawing;

namespace RecognitionOfDrivingLicenses.Helpers
{
    public static class FilterHelper
    {
        public static byte[,] BitmapToArray(Bitmap bitmap)
        {
            var result = new byte[bitmap.Width, bitmap.Height];

            for (var i = 0; i < bitmap.Width; i++)
            {
                for (var j = 0; j < bitmap.Height; j++)
                {
                    result[i, j] = CountLumin(bitmap.GetPixel(i, j));
                }
            }

            return result;
        }

        public static Bitmap ArrayToBitmap(byte[,] pixelArray)
        {
            var bitmap = new Bitmap(pixelArray.GetLength(0), pixelArray.GetLength(1));

            for (var i = 0; i < pixelArray.GetLength(0); i++)
            {
                for (var j = 0; j < pixelArray.GetLength(1); j++)
                {
                    bitmap.SetPixel(i, j, Color.FromArgb(pixelArray[i, j], pixelArray[i, j], pixelArray[i, j]));
                }
            }

            return bitmap;
        }

        public static byte CountLumin(Color color)
        {
            var red = color.R;
            var green = color.G;
            var blue = color.B;

            return Convert.ToByte(0.3 * red + 0.59 * green + 0.11 * blue);
        }

        public static int SetColor(int color, int offset = 0)
        {
            var sum = color + offset;

            if (sum < 0)
            {
                return 0;
            }

            if (sum > 255)
            {
                return 255;
            }

            return sum;
        }
    }
}
