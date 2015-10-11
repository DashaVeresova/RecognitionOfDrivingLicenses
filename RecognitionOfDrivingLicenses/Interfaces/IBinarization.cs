using System.Drawing;

namespace RecognitionOfDrivingLicenses.Interfaces
{
    public interface IBinarization
    {
        Bitmap GetBinaryImage(Bitmap bmp); 
    }
}
