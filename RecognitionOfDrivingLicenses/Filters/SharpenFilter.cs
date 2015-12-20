using System.Drawing;
using RecognitionOfDrivingLicenses.Helpers;
using RecognitionOfDrivingLicenses.Interfaces;

namespace RecognitionOfDrivingLicenses.Filters
{
    public class SharpenFilter : IFilter
    {
        private readonly IConvolutionFilter _convolution;
        public SharpenFilter()
        {
            _convolution = new ConvolutionFilter();
        }

        public Bitmap GetFilteredImage(Bitmap bitmap)
        {
            var bytes = FilterHelper.BitmapToArray(bitmap);
          //  var filterMatrix = new double[,]
          //{ { -1,  0,  1, },
          //        { -2,  0,  2, },
          //        { -1,  0,  1, }, };
            var filterMatrix = new double[,]
            {
                {0, -2, 0},
                {-2, 11, -2},
                {0, -2, 0}
            };
            //var filterMatrix = new double[,]
            //{
            //    {0, 0, 0, 0, 0},
            //    {0, 0, -1, 0, 0},
            //    {0, -1, 5, -1, 0},
            //    {0, 0, -1, 0, 0},
            //    {0, 0, 0, 0, 0}
            //};

            var result = _convolution.Filter(bytes, filterMatrix);
            return FilterHelper.ArrayToBitmap(result);
        }
    }
}
