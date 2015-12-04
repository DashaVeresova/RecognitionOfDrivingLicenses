using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using RecognitionOfDrivingLicenses.Helpers;

namespace RecognitionOfDrivingLicenses
{
    public class ImageHandler
    {
        private Bitmap _bitmap;
        private byte[,] _bytes;

        public ImageHandler(Bitmap bitmap, byte[,] bytes)
        {
            _bitmap = bitmap;
            _bytes = bytes;
            //var intencity = new byte[_bitmap.Width, _bitmap.Height];
            //for (var i = 0; i <= _bitmap.Width - 1; i++)
            //{
            //    for (var j = 0; j <= _bitmap.Height - 1; j++)
            //    {
            //        intencity[i, j] = FilterHelper.CountLumin(_bitmap.GetPixel(i, j));
            //    }
            //}
            //_bytes = intencity; 
        }

        public Bitmap Bitmap
        {
            get
            {
                return _bitmap;
            }
            set
            {
                _bitmap = value;
                _bytes = FilterHelper.BitmapToArray(_bitmap);
            }
        }

        public int[] CalculateLineHistogram()
        {
            var result = new int[_bitmap.Height];
            for (var j = 0; j < _bitmap.Height; j++)
            {
                for (var i = 0; i < _bitmap.Width; i++)
                {
                    if (_bytes[i, j] == 255)
                    {
                        result[j]++;
                    }
                }
            }
            return result;
        }

        public IEnumerable<Rectangle> DeterminateYCoordinates(int[] histogram)
        {

            var minEdges = _bitmap.Width / 50;

            var insideTextArea = false;
            var result = new List<Rectangle>();

            var topBorder = 0;

            for (var i = 3; i < histogram.Length - 3; i++)
            {
                if (!insideTextArea)
                {
                    var borderPretender = TryGetSharpTopEdge(histogram, i, minEdges);
                    if (borderPretender != -1)
                    {
                        topBorder = i;
                        insideTextArea = true;
                    }
                }
                else
                {
                    var bottomBorder = TryGetSharpBottomEdge(histogram, i, minEdges);
                    if (bottomBorder != -1)
                    {
                        insideTextArea = false;
                        result.Add(new Rectangle(0, topBorder, _bitmap.Width, bottomBorder - topBorder));
                    }
                }
            }

            return result;
        }


        private int TryGetSharpTopEdge(int[] histogram, int index, int minEdges)
        {
            for (var i = index - 3; i <= index; i++)
            {
                if ((histogram[i] == 0 && histogram[index] >= minEdges))
                {
                    return i;
                }

                if (histogram[i] == 0)
                {
                    continue;
                }

                if (histogram[index] / histogram[i] >= 5)
                {
                    return i;
                }
            }

            return -1;
        }

        private int TryGetSharpBottomEdge(int[] histogram, int index, int minEdges)
        {
            for (var i = index; i <= index + 3; i++)
            {
                if ((histogram[i] == 0 && histogram[index] >= minEdges))
                {
                    return i;
                }

                if (histogram[i] == 0)
                {
                    continue;
                }

                if (histogram[index] / histogram[i] >= 7)
                {
                    return i;
                }
            }

            return -1;
        }

        public IEnumerable<Rectangle> DeterminatesWords(IEnumerable<Rectangle> rectangles)
        {
            var result = new List<Rectangle>();

            foreach (var rectangle in rectangles)
            {
                var isInWord = false;
                var left = 0;

                for (int i = 0; i < _bitmap.Width; i++)
                {
                    var isBlackLine = true;
                    for (int j = rectangle.Top; j <= rectangle.Bottom; j++)
                    {
                        if (_bytes[i, j] != 0)
                        {
                            isBlackLine = false;
                        }
                    }

                    if (isBlackLine && isInWord)
                    {
                        result.Add(new Rectangle(left, rectangle.Top, i - left, rectangle.Height));
                        isInWord = false;
                    }

                    if (!isInWord && !isBlackLine)
                    {
                        isInWord = true;
                        left = i;
                    }
                }
            }

            return result;
        }

        public Rectangle DetectionText(Bitmap temp)
        {
            BitmapData bmpData = temp.LockBits(new Rectangle(0, 0, temp.Width, temp.Height),
                   ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            // Получаем адрес первой линии.
            IntPtr ptr = bmpData.Scan0;
            int numBytes = bmpData.Stride * temp.Height;
            byte[] rgbValues = new byte[numBytes];
            // Копируем значения в массив.
            Marshal.Copy(ptr, rgbValues, 0, numBytes);

            int[] hist = new int[temp.Height];
            hist.Initialize();
            for (int i = 0; i < temp.Height; i++)
            {
                //int index = bmpData.Stride * i;
                int j;
                for (j = temp.Width - temp.Width * 4 / 5; j < temp.Width * 4 / 5; j++)
                {

                    int index = bmpData.Stride * i + j * 3;
                    hist[i] = rgbValues[index] == 0 ? hist[i] += 1 : hist[i] += 0;

                }

                //                g.DrawLine(new Pen(Brushes.Red, 1), new Point(0, i), new Point(hist[i], i));

            }
            int start = 0; int end = 0;
            int j1 = 0, j2 = 0;
            for (int i = 0; i < temp.Height / 2; i++)
            {
                if (hist[i] != 0 && hist[i + 1] == 0)
                {
                    j1 = 0;
                    i++;
                    while (hist[i] == 0)
                    {
                        j1++;
                        i++;
                    }
                    if (j2 < j1)
                    {
                        j2 = j1;
                        start = --i;
                    }
                }

            }
            j1 = 0; j2 = 0;
            for (int i = temp.Height - 1; i > temp.Height - temp.Height / 6; i--)
            {
                if (hist[i] != 0 && hist[i - 1] == 0)
                {
                    j1 = 0;
                    i--;
                    while (hist[i] == 0)
                    {
                        j1++;
                        i--;
                    }
                    if (j2 < j1)
                    {
                        j2 = j1;
                        end = ++i;
                    }
                }

            }
            hist = new int[temp.Width];
            hist.Initialize();
            for (int j = 0; j < temp.Width; j++)
            {

                for (int i = start; i < end; i++)
                {
                    int index = bmpData.Stride * i + j * 3;
                    hist[j] = rgbValues[index] == 0 ? hist[j] += 1 : hist[j] += 0;
                }
            }
            int startW = 0, endW = 0;
            j1 = 0; j2 = 0;
            for (int i = 0; i < temp.Width / 2; i++)
            {
                if (hist[i] != 0 && hist[i + 1] == 0)
                {
                    j1 = 0;
                    i++;
                    while (hist[i] == 0)
                    {
                        j1++;
                        i++;
                    }
                    if (j2 < j1)
                    {

                        j2 = j1;
                        startW = --i;
                    }
                }

            }
            j1 = 0; j2 = 0;
            for (int i = temp.Width - 1; i > temp.Width / 2; i--)
            {
                if (hist[i] != 0 && hist[i - 1] == 0)
                {
                    j1 = 0;
                    i--;
                    while (hist[i] == 0)
                    {
                        j1++;
                        i--;
                    }
                    if (j2 < j1)
                    {
                        j2 = j1;
                        endW = ++i;
                    }

                }
            }
            temp.UnlockBits(bmpData);
            return new Rectangle(startW, start, endW - startW, (end - start));
        }
    }
}
