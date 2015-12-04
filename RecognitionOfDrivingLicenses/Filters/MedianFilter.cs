using System.Collections.Generic;
using System.Drawing;
using RecognitionOfDrivingLicenses.Helpers;
using RecognitionOfDrivingLicenses.Interfaces;

namespace RecognitionOfDrivingLicenses.Filters
{
    public class MedianFilter: IFilter
    {
        private readonly int _size;
        private readonly bool _colored;
        private readonly ProgressDelegate _progressDelegate;

        public MedianFilter(int size, bool colored = false, ProgressDelegate progressDelegate = null)
        {
            _size = size;
            _colored = colored;
            _progressDelegate = progressDelegate;
        }

        public Bitmap GetFilteredImage(Bitmap bitmap)
        {
            var result = new Bitmap(bitmap);

            var iterationProgress = (double)100 / (bitmap.Width * bitmap.Height);

            double progress = 0;
            
            var bytes = FilterHelper.BitmapToArray(bitmap);

            for (int i = _size; i < bitmap.Width - _size; i++)
            {
                for (int j = _size; j < bitmap.Height - _size; j++)
                {
                    var res = new List<int>();
                    var redList = new List<int>();
                    var greenList = new List<int>();
                    var blueList = new List<int>();
                    
                    for (int ii = -_size; ii < _size + 1; ii++)
                    {
                        for (int jj = -_size; jj < _size + 1; jj++)
                        {
                            if (_colored)
                            {
                                var pixelColored = bitmap.GetPixel(i + ii, j + jj);
                                redList.Add(pixelColored.R);
                                greenList.Add(pixelColored.G);
                                blueList.Add(pixelColored.B);
                            }
                            else
                            {
                                res.Add(bytes[i + ii, j + jj]);
                            }
                        }
                    }
                    if (_colored)
                    {
                        redList.Sort();
                        greenList.Sort();
                        blueList.Sort();

                        var red = redList[_size];
                        var green = greenList[_size];
                        var blue = blueList[_size];

                        var color = Color.FromArgb(bitmap.GetPixel(i, j).A, FilterHelper.SetColor(red), FilterHelper.SetColor(green), FilterHelper.SetColor(blue));

                        result.SetPixel(i, j, color);
                    }
                    else
                    {
                        res.Sort();
                        result.SetPixel(i, j, Color.FromArgb(res[_size], res[_size], res[_size]));
                    }
                    
                    progress += iterationProgress;

                    ProgressBarHelper.UpdateProgresssBar(progress, _progressDelegate);
                }
            }

            return result;
        }
    }
}
