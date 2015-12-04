using System;
using System.Collections.Generic;
using System.Drawing;
using RecognitionOfDrivingLicenses.Helpers;
using RecognitionOfDrivingLicenses.Interfaces;

namespace RecognitionOfDrivingLicenses.Filters
{
    public class GausFilter: IFilter
    {
        private int _size;
        private readonly bool _colored;
        private readonly ProgressDelegate _progressDelegate;

        public GausFilter(int size, bool colored = false, ProgressDelegate progressDelegate = null)
        {
            _size = size;
            _colored = colored;
            _progressDelegate = progressDelegate;
        }

        public Bitmap GetFilteredImage(Bitmap bitmap)
        {
            if (_size % 2 == 0)
            {
                _size++;
            }
            
            var gausList = new List<double>();
            var u = -1;

            for (int i = 0; i <= _size / 2 + 1; i++)
            {
                u += i;
            }

            for (int i = 0; i <= u; i++)
            {
                gausList.Add(1 / (Math.Sqrt(Math.PI * 2 * u)) * Math.Exp(-((double)(i * i)) / (2 * u)));
            }

            var matrix = new double[_size, _size];
            int k = 1;
            int c = _size / 2;

            matrix[c, c] = gausList[0];

            for (int i = 1; i <= c; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    matrix[c + i, c + j] = gausList[k];
                    matrix[c + i, c - j] = gausList[k];
                    matrix[c - i, c + j] = gausList[k];
                    matrix[c - i, c - j] = gausList[k];
                    matrix[c + j, c + i] = gausList[k];
                    matrix[c - j, c + i] = gausList[k];
                    matrix[c + j, c - i] = gausList[k];
                    matrix[c - j, c - i] = gausList[k];
                    k++;
                }
            }

            double sum = 0;

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    sum += matrix[i, j];
                }
            }

            var dst = new Bitmap(bitmap);

            int xLength = matrix.GetLength(0) / 2;
            int yLength = matrix.GetLength(1) / 2;

            var iterationProgress = (double)100/(bitmap.Width*bitmap.Height);
            double progress = 0;

            var bytes = FilterHelper.BitmapToArray(bitmap);

            for (int i = xLength; i < bitmap.Width - xLength; i++)
            {
                for (int j = yLength; j < bitmap.Height - yLength; j++)
                {
                    int res = 0;
                    int red = 0, green = 0, blue = 0;

                    for (int ii = -xLength; ii < xLength + 1; ii++)
                    {
                        for (int jj = -yLength; jj < yLength + 1; jj++)
                        {
                            if (_colored)
                            {
                                var pixelColored = bitmap.GetPixel(i + ii, j + jj);
                                red += (int) (pixelColored.R*matrix[ii + xLength, jj + yLength]);
                                green += (int) (pixelColored.G*matrix[ii + xLength, jj + yLength]);
                                blue += (int) (pixelColored.B*matrix[ii + xLength, jj + yLength]);
                            }
                            else
                            {
                                byte pixel = bytes[i + ii, j + jj];
                                res += (int) (pixel*matrix[ii + xLength, jj + yLength]);
                            }
                        }
                    }

                    if (_colored)
                    {
                        red = (int)(red / sum);
                        green = (int)(green / sum);
                        blue = (int)(blue / sum); 
                        var color = Color.FromArgb(bitmap.GetPixel(i, j).A, FilterHelper.SetColor(red), FilterHelper.SetColor(green), FilterHelper.SetColor(blue));
                        dst.SetPixel(i, j, color);
                    }
                    else 
                    {
                        res = (int)(res / sum);
                        dst.SetPixel(i, j, Color.FromArgb(res, res, res));
                    }
                    
                    progress += iterationProgress;

                    ProgressBarHelper.UpdateProgresssBar(progress, _progressDelegate);
                }
            }

            return dst;         
        }
    }
}
