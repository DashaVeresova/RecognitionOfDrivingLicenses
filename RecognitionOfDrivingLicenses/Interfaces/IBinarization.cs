using System.Drawing;
using RecognitionOfDrivingLicenses.Helpers;

namespace RecognitionOfDrivingLicenses.Interfaces
{
    public interface IBinarization
    {
        Bitmap GetBinaryImage(Bitmap bitmap, ProgressDelegate progressDelegate = null); 
    }
}
