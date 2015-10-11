using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognitionOfDrivingLicenses
{
    public interface IFilter
    {
        Bitmap GetFilteredImage(Bitmap bmp, int size); 
    }
}
