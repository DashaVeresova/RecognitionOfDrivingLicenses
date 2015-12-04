using System.Drawing;

namespace RecognitionOfDrivingLicenses.Interfaces
{
    public interface IFilter
    {
        Bitmap GetFilteredImage(Bitmap bitmap);
    }
}
