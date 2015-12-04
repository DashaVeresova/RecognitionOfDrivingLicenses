using System;
using System.Drawing;
using RecognitionOfDrivingLicenses.Helpers;
using RecognitionOfDrivingLicenses.Interfaces;

namespace RecognitionOfDrivingLicenses.Filters
{
    public class EdgeFilter : IFilter
    {
        public Bitmap GetFilteredImage(Bitmap bitmap)
        {
            var heigth = bitmap.Height;
            var width = bitmap.Width;
            var newImage = new byte[width, heigth];

            var bytes = FilterHelper.BitmapToArray(bitmap);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < heigth; j++)
                {
                    if ((i > 0 && i < width - 1) && (j > 0 && j < heigth - 1))
                    {
                        var left = Math.Abs(bytes[i, j] - bytes[i - 1, j]);
                        var upper = Math.Abs(bytes[i, j] - bytes[i, j - 1]);
                        var rightUpper = Math.Abs(bytes[i, j] - bytes[i + 1, j - 1]);
                        newImage[i, j] = (byte)Math.Max(Math.Max(left, upper), rightUpper);
                    }
                    else
                    {
                        newImage[i, j] = 0;
                    }
                }
            }
            return FilterHelper.ArrayToBitmap(newImage);
        }
    }
}
