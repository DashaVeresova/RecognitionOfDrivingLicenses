using System.Collections.Generic;
using System.Drawing;
using RecognitionOfDrivingLicenses.Helpers;
using RecognitionOfDrivingLicenses.Interfaces;

namespace RecognitionOfDrivingLicenses.Filters
{
    public class MedianFilter: IFilter
    {
        public Bitmap GetFilteredImage(Bitmap bitmap, int size, ProgressDelegate progressDelegate)
        {
            var result = new Bitmap(bitmap);

            var iterationProgress = (double)100 / (bitmap.Width * bitmap.Height);

            double progress = 0;

            for (int i = size; i < bitmap.Width - size; i++)
            {
                for (int j = size; j < bitmap.Height - size; j++)
                {
                    var redList = new List<int>();
                    var greenList = new List<int>();
                    var blueList = new List<int>();
                    
                    for (int ii = -size; ii < size + 1; ii++)
                    {
                        for (int jj = -size; jj < size + 1; jj++)
                        {
                            var pixel = bitmap.GetPixel(i + ii, j + jj);

                            redList.Add(pixel.R);
                            greenList.Add(pixel.G);
                            blueList.Add(pixel.B);
                        }
                    }

                    redList.Sort();
                    greenList.Sort();
                    blueList.Sort();

                    var red = redList[size];
                    var green = greenList[size];
                    var blue = blueList[size];

                    var color = Color.FromArgb(bitmap.GetPixel(i, j).A, FilterHelper.SetColor(red), FilterHelper.SetColor(green), FilterHelper.SetColor(blue));

                    result.SetPixel(i, j, color);

                    progress += iterationProgress;

                    ProgressBarHelper.UpdateProgresssBar(progress, progressDelegate);
                }
            }

            return result;
        }
    }
}
