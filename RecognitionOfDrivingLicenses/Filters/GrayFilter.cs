using System.Drawing;
using RecognitionOfDrivingLicenses.Helpers;
using RecognitionOfDrivingLicenses.Interfaces;

namespace RecognitionOfDrivingLicenses.Filters
{
    public class GrayFilter: IFilter
    {
        public Bitmap GetFilteredImage(Bitmap bitmap, int size = 1, ProgressDelegate progressDelegate = null)
        {
            var pixelArray = FilterHelper.BitmapToArray(bitmap);
            var result = FilterHelper.ArrayToBitmap(pixelArray);

            return result;
        }
    }
}
