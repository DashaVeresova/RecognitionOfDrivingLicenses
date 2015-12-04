using System.Drawing;
using RecognitionOfDrivingLicenses.Helpers;
using RecognitionOfDrivingLicenses.Interfaces;

namespace RecognitionOfDrivingLicenses.Filters
{
    public class BinarizationByOtsu: IBinarization
    {
        private readonly ProgressDelegate _progressDelegate;

        public BinarizationByOtsu(ProgressDelegate progressDelegate)
        {
            _progressDelegate = progressDelegate;
        }

        public Bitmap GetBinaryImage(Bitmap bitmap)
        {
            var initialBytes = FilterHelper.BitmapToArray(bitmap);
            var otsuThreshold = OtsuThreshold(initialBytes);

            var iterationProgress = (double)100 / (bitmap.Width * bitmap.Height);
            double progress = 0;

            for (var i = 0; i < initialBytes.GetLength(0); i++)
            {
                for (var j = 0; j < initialBytes.GetLength(1); j++)
                {
                    if (initialBytes[i, j] < otsuThreshold)
                    {
                        initialBytes[i, j] = 0;
                    }
                    else
                    {
                        initialBytes[i, j] = 255;
                    }

                    progress += iterationProgress;
                    
                    ProgressBarHelper.UpdateProgresssBar(progress, _progressDelegate);
                }
            }

            return FilterHelper.ArrayToBitmap(initialBytes);
        }

        private int OtsuThreshold(byte[,] pixelArray)
        {
            int min = pixelArray[0, 0];
            int max = pixelArray[0, 0];
            int temp1;

            int beta, threshold = 0;
            double maxSigma = -1;

            /**** Построение гистограммы ****/
            /* Узнаем наибольший и наименьший полутон */
            int temp;
            for (var i = 0; i < pixelArray.GetLength(0); i++)
            {
                for (var j = 0; j < pixelArray.GetLength(1); j++)
                {
                    temp = pixelArray[i, j];

                    if (temp < min)
                    {
                        min = temp;
                    }
                    if (temp >= max)
                    {
                        max = temp;
                    }
                }
            }

            int histSize = max - min + 1;

            var hist = new int[histSize];

            /* Считаем сколько каких полутонов */
            for (var i = 0; i < pixelArray.GetLength(0); i++)
            {
                for (var j = 0; j < pixelArray.GetLength(1); j++)
                {
                    hist[pixelArray[i, j] - min]++;
                }
            }

            /**** Гистограмма построена ****/

            temp = temp1 = 0;
            int alpha = beta = 0;
            /* Для расчета математического ожидания первого класса */
            for (var i = 0; i <= (max - min); i++)
            {
                temp += i * hist[i];
                temp1 += hist[i];
            }

            /* Основной цикл поиска порога
            Пробегаемся по всем полутонам для поиска такого, при котором внутриклассовая дисперсия минимальна */
            for (var i = 0; i < (max - min); i++)
            {
                alpha += i * hist[i];
                beta += hist[i];

                double w1 = (double)beta / temp1;
                double a = (double)alpha / beta - (double)(temp - alpha) / (temp1 - beta);
                double sigma = w1 * (1 - w1) * a * a;

                if (sigma > maxSigma)
                {
                    maxSigma = sigma;
                    threshold = i;
                }
            }
            return threshold + min;
        }
    }
}
