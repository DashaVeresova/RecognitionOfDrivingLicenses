﻿using System;
using System.Collections.Generic;
using System.Drawing;
using RecognitionOfDrivingLicenses.Helpers;
using RecognitionOfDrivingLicenses.Interfaces;

namespace RecognitionOfDrivingLicenses.Filters
{
    public class GausFilter: IFilter
    {
        public Bitmap GetFilteredImage(Bitmap bitmap, int size, ProgressDelegate progressDelegate)
        {
            if (size % 2 == 0)
            {
                size++;
            }
            
            var gausList = new List<double>();
            var u = -1;

            for (int i = 0; i <= size / 2 + 1; i++)
            {
                u += i;
            }

            for (int i = 0; i <= u; i++)
            {
                gausList.Add(1 / (Math.Sqrt(Math.PI * 2 * u)) * Math.Exp(-((double)(i * i)) / (2 * u)));
            }

            var matrix = new double[size, size];
            int k = 1;
            int c = size / 2;

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

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    sum += matrix[i, j];
                }
            }

            var dst = new Bitmap(bitmap);

            int xLength = matrix.GetLength(0) / 2;
            int yLength = matrix.GetLength(1) / 2;

            var iterationProgress = (double)100/(bitmap.Width*bitmap.Height);
            double progress = 0;

            for (int i = xLength; i < bitmap.Width - xLength; i++)
            {
                for (int j = yLength; j < bitmap.Height - yLength; j++)
                {
                    int red = 0, green = 0, blue = 0;

                    for (int ii = -xLength; ii < xLength + 1; ii++)
                    {
                        for (int jj = -yLength; jj < yLength + 1; jj++)
                        {
                            var pixel = bitmap.GetPixel(i + ii, j + jj);
                            red += (int)(pixel.R * matrix[ii + xLength, jj + yLength]);
                            green += (int)(pixel.G * matrix[ii + xLength, jj + yLength]);
                            blue += (int)(pixel.B * matrix[ii + xLength, jj + yLength]);
                        }
                    }

                    red = (int)(red / sum);
                    green = (int)(green / sum);
                    blue = (int)(blue / sum);

                    var color = Color.FromArgb(bitmap.GetPixel(i, j).A, FilterHelper.SetColor(red), FilterHelper.SetColor(green), FilterHelper.SetColor(blue));
                    dst.SetPixel(i, j, color);

                    progress += iterationProgress;

                    ProgressBarHelper.UpdateProgresssBar(progress, progressDelegate);
                }
            }

            return dst;         
        }
    }
}
