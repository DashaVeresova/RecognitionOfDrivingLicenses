using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecognitionOfDrivingLicenses
{
    public class BinarizationByOtsu
    {
        public byte[,] GetFilteredImage(byte[,] initialBytes)
        {
            var otsuThreshold = OtsuThreshold(initialBytes);

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
                }
            }

            return initialBytes;
        }

        private int OtsuThreshold(byte[,] pixelArray)
        {
            int min = pixelArray[0, 0];
            int max = pixelArray[0, 0];
            int temp1;
            int histSize;

            int alpha, beta, threshold = 0;
            double sigma, maxSigma = -1;
            double w1, a;

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

            histSize = max - min + 1;

            int[] hist = new int[histSize];

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
            alpha = beta = 0;
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

                w1 = (double)beta / temp1;
                a = (double)alpha / beta - (double)(temp - alpha) / (temp1 - beta);
                sigma = w1 * (1 - w1) * a * a;

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
