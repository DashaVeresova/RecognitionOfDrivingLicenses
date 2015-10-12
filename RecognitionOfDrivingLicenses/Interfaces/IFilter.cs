using System.Drawing;
using RecognitionOfDrivingLicenses.Helpers;

namespace RecognitionOfDrivingLicenses.Interfaces
{
    public interface IFilter
    {
        Bitmap GetFilteredImage(Bitmap bitmap, int size = 1, ProgressDelegate progressDelegate = null);
    }
}
