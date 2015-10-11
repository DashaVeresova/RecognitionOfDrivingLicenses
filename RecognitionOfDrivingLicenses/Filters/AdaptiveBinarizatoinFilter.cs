using System.Drawing;
using RecognitionOfDrivingLicenses.Helpers;
using RecognitionOfDrivingLicenses.Interfaces;

namespace RecognitionOfDrivingLicenses.Filters
{
    public class AdaptiveBinarizatoinFilter: IBinarization
    {
        public Bitmap GetBinaryImage(Bitmap bmp)
        {
            var result = new Bitmap(bmp.Width, bmp.Height);
            var bytes = FilterHelper.BitmapToArray(bmp);

            for (var i = 0; i < bmp.Width; i++)
            {
                for (var j = 0; j < bmp.Height; j++)
                {
                    var pixel = bytes[i, j];
                    var pixelIntensity = SurroundedValue(i, j, bytes);

                    result.SetPixel(i, j, pixel > pixelIntensity - 5 ? Color.White : Color.Black);
                }
            }

            return result;
        }

        private byte SurroundedValue(int x, int y, byte[,] bmp)
        {
            const int radius = 20;

            var left = (x - radius < 0) ? 0 : x - radius;
            var top = (y - radius < 0) ? 0 : y - radius;

            var right = (x + radius < bmp.GetLength(0)) ? x + radius : bmp.GetLength(0) - 1;
            var bottom = (y + radius < bmp.GetLength(1)) ? y + radius : bmp.GetLength(1) - 1;

            var summaryIntensity = 0;
            var counter = 0;

            for (var i = left; i <= right; i++)
            {
                for (var j = top; j <= bottom; j++)
                {
                    summaryIntensity += bmp[i, j];
                    counter++;
                }
            }
            return (byte)(summaryIntensity / counter);
        }
    }
}
