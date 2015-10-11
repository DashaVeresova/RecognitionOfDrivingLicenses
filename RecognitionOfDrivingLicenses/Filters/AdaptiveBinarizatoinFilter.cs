using System.Drawing;
using RecognitionOfDrivingLicenses.Helpers;
using RecognitionOfDrivingLicenses.Interfaces;

namespace RecognitionOfDrivingLicenses.Filters
{
    public class AdaptiveBinarizatoinFilter: IBinarization
    {
        public Bitmap GetBinaryImage(Bitmap bitmap, ProgressDelegate progressDelegate)
        {
            var result = new Bitmap(bitmap.Width, bitmap.Height);
            var bytes = FilterHelper.BitmapToArray(bitmap);
            
            var iterationProgress = (double)50 / (bitmap.Width * bitmap.Height);
            var innerIterationProgress = (double)50 / (bitmap.Width * bitmap.Height);
            double progress = 0;

            for (var i = 0; i < bitmap.Width; i++)
            {
                for (var j = 0; j < bitmap.Height; j++)
                {
                    var pixel = bytes[i, j];
                    var pixelIntensity = SurroundedValue(i, j, bytes);

                    progress += innerIterationProgress;
                    ProgressBarHelper.UpdateProgresssBar(progress, progressDelegate);

                    result.SetPixel(i, j, pixel > pixelIntensity - 5 ? Color.White : Color.Black);
                    
                    progress += iterationProgress;

                    ProgressBarHelper.UpdateProgresssBar(progress, progressDelegate);
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
