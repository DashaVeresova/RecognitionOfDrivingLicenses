using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class TextFindProcessor : IFindImageProcessor
    {
        public string Name => "Find text in document";

        public TextFindProcessor()
        {
        }

        public IEnumerable<IFindResult> Find(IImage image)
        {
            //var bitmap = (BitmapImage) image;
            //bitmap.FromBitmap(this.OtsuFilter(bitmap.ToBitmap()));
            var indencity = this.GetIntensityFrom(image);
            indencity = new SharpenFilter().Filter(indencity);
            indencity = new EdgeFilter().Filter(indencity);
            var histogram = this.CalculateLineHistogram(indencity);
            var borders = this.DeterminateYCoordinates(histogram, indencity.GetLength(0));
            var frames = this.DeterminatesWords(borders, this.GetIntensityFrom(image));
            return frames.Select(f => new FindResult(f.X, f.Y, f.Height, f.Width));
        }

        public Bitmap OtsuFilter(Bitmap bmp)
        {
            //Блокируем набор данных изображения в памяти
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            // Получаем адрес первой линии.
            IntPtr ptr = bmpData.Scan0;
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[numBytes];
            int height = 0, width = 0;
            int tempH = 0, tempW = 0;
            int startH = 0, startW = 0;
            // Копируем значения в массив.
            Marshal.Copy(ptr, rgbValues, 0, numBytes);
            for (int counter = 0; counter < rgbValues.Length - 2; counter += 3)
            {
                rgbValues[counter] = rgbValues[counter + 1] = rgbValues[counter + 2] = (byte)(0.2125 * rgbValues[counter] + 0.7154 * rgbValues[counter + 1] + 0.0721 * rgbValues[counter + 2]);
            }
            for (int cicleH = 0; cicleH <= 3; cicleH++)
            {
                switch (cicleH)
                {
                    case 0:
                        tempH = height = (int)bmp.Height / 3;
                        startH = 0;
                        break;
                    case 1:
                        startH = height;
                        height = tempH * 2;
                        break;
                    case 2:
                        startH = height;
                        height = tempH * 3;
                        break;
                    case 3:
                        startH = height;
                        height = bmp.Height;
                        break;
                    case 4:
                        startH = height;
                        height = tempH * 5;
                        break;
                    case 5:
                        startH = height;
                        height = bmp.Height;
                        break;
                    case 6:
                        startH = height;
                        height = tempH * 7;
                        break;
                    case 7:
                        startH = height;
                        height = tempH * 8;
                        break;
                    case 8:
                        startH = height;
                        height = tempH * 9;
                        break;
                    case 9:
                        startH = height;
                        height = tempH * 10;
                        break;
                    case 10:
                        startH = height;
                        height = bmp.Height;
                        break;
                }
                for (int cicleW = 0; cicleW <= 0; cicleW++)
                {
                    switch (cicleW)
                    {
                        case 0:
                            tempW = width = (int)bmp.Width;
                            startW = 0;
                            break;
                        case 1:
                            startW = width;
                            width = bmp.Width;
                            break;
                        case 2:
                            startW = width;
                            width = bmp.Width;
                            break;
                        case 3:
                            startW = width;
                            width = bmp.Width;
                            break;
                        case 4:
                            startW = width;
                            width = tempW * 5;
                            break;
                        case 5:
                            startW = width;
                            width = bmp.Width;
                            break;
                        case 6:
                            startW = width;
                            width = tempW * 7;
                            break;
                        case 7:
                            startW = width;
                            width = tempW * 8;
                            break;
                        case 8:
                            startW = width;
                            width = tempW * 9;
                            break;
                        case 9:
                            startW = width;
                            width = tempW * 10;
                            break;
                        case 10:
                            startW = width;
                            width = bmp.Width;
                            break;
                    }
                    //gistogramma
                    int[] hist = new int[256];
                    hist.Initialize();
                    for (int i = startH; i < height; i++)
                    {
                        int index = bmpData.Stride * i + startW * 3;
                        for (int j = startW; j < width; j++)
                        {
                            hist[rgbValues[index]]++;
                            index += 3;
                        }
                    }
                    float temp1, temp2, temp3;
                    float temp = 0;
                    int t = 0;
                    float[] vet = new float[256];
                    //Пробегаемся по всем полутонам для поиска такого, при котором внутриклассовая дисперсия минимальна
                    for (int i = 0; i < 256; i++)
                    {
                        temp1 = Px(0, i, hist);
                        temp2 = Px(i + 1, 255, hist);
                        temp3 = temp1 * temp2;
                        if (temp3 == 0) temp3 = 1;
                        float diff = (Mx(0, i, hist) * temp2) - (Mx(i + 1, 255, hist) * temp1);
                        if (temp < (float)diff * diff / temp3)
                        {
                            temp = (float)diff * diff / temp3;
                            t = i;
                        }
                    }
                    //Сам алгоритм Отсу 
                    for (int i = startH; i < height; i++)
                    {
                        int offset = bmpData.Stride * i + startW * 3;
                        for (int j = startW; j < width; j++)
                        {
                            rgbValues[offset] = (byte)((rgbValues[offset] > (byte)t) ? 255 : 0);
                            rgbValues[offset + 1] = (byte)((rgbValues[offset + 1] > (byte)t) ? 255 : 0);
                            rgbValues[offset + 2] = (byte)((rgbValues[offset + 2] > (byte)t) ? 255 : 0);

                            offset += 3;
                        }
                    }
                }
            }
            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        private static float Px(int init, int end, int[] hist)
        {
            int sum = 0;
            int i;
            for (i = init; i <= end; i++)
                sum += hist[i];

            return (float)sum;
        }
        private static float Mx(int init, int end, int[] hist)
        {
            int sum = 0;
            int i;
            for (i = init; i <= end; i++)
                sum += i * hist[i];

            return (float)sum;
        }

        public IEnumerable<Rectangle>  DeterminatesWords(List<Rectangle> rectangles, byte[,] image)
        {
            //var result = new List<Rectangle>();

            //foreach (var rectangle in rectangles)
            //{
            //    var isInWord = false;
            //    var left = 0;
            //    int width = image.GetLength(0);
            //    for (int i = 0; i < width; i++)
            //    {
            //        var isBlackLine = true;
            //        for (int j = rectangle.Top; j <= rectangle.Bottom; j++)
            //        {
            //            if (image[i, j] != 0)
            //            {
            //                isBlackLine = false;
            //            }
            //        }

            //        if (isBlackLine && isInWord)
            //        {
            //            result.Add(new Rectangle(left, rectangle.Top, i - left, rectangle.Height));
            //            isInWord = false;
            //        }

            //        if (!isInWord && !isBlackLine)
            //        {
            //            isInWord = true;
            //            left = i;
            //        }
            //    }
            //}

            //return result;

            var result = new List<Rectangle>();
            int countOfMachineReadableLines = 2;
            int width = image.GetLength(0);
            int leftBorderStart = (int) (width * 0.05);
            int rightBorderFinish = (int)(width * 0.05);

            for (int k = rectangles.Count() - countOfMachineReadableLines; k < rectangles.Count() ; k++)
            {
                var isInSymbol = false;
                var positionOfTheLeftBorder = 0;
                for (int i = leftBorderStart; i < width - rightBorderFinish; i++)
                {
                    var hasBlackInColumn = true;
                    for (int j = rectangles[k].Top; j <= rectangles[k].Bottom; j++)
                    {
                        if (image[i, j] < 150)
                        {
                            hasBlackInColumn = false;
                        }
                    }

                    if (hasBlackInColumn && isInSymbol)
                    {
                        result.Add(new Rectangle(positionOfTheLeftBorder, rectangles[k].Top, i - positionOfTheLeftBorder, rectangles[k].Height));
                        isInSymbol = false;
                    }

                    if (!isInSymbol && !hasBlackInColumn)
                    {
                        isInSymbol = true;
                        positionOfTheLeftBorder = i;
                    }
                }
            }

            return result;
        }

        public List<Rectangle> DeterminateYCoordinates(int[] histogram, int width)
        {

            var minEdges = width / 50;

            var insideTextArea = false;
            var result = new List<Rectangle>();

            var topBorder = 0;
            var bottomBorder = 0;

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
                    bottomBorder = TryGetSharpBottomEdge(histogram, i, minEdges);
                    if (bottomBorder != -1)
                    {
                        insideTextArea = false;
                        result.Add(new Rectangle(0, topBorder, width, bottomBorder - topBorder));
                    }
                }
            }

            return result;
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

                if (histogram[index] / histogram[i] >= 4)
                {
                    return i;
                }
            }

            return -1;
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

        public int[] CalculateLineHistogram(byte[,] image)
        {
            int width = image.GetLength(0);
            int height = image.GetLength(1);
            //height -= height/8;
            var result = new int[height];
            for (var j = 0; j < height; j++)
            {
                for (var i = 0; i < width; i++)
                {
                    if (image[i, j] == 255)
                    {
                        result[j]++;
                    }
                }
            }
            return result;
        }

        private byte[,] GetIntensityFrom(IImage image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));

            var intencity = new byte[image.Width, image.Height];
            for (var i = 0; i <= image.Width - 1; i++)
            {
                for (var j = 0; j <= image.Height - 1; j++)
                {
                    intencity[i, j] = this.Intensity(image.GetPixel(i, j));
                }
            }
            return intencity;
        }

        private byte Intensity(Pixel color)
        {
            return (byte) (0.3*color.R + 0.59*color.G + 0.11*color.B);
        }

        public class SharpenFilter
        {
            public string Name => nameof(SharpenFilter);

            private readonly ConvolutionFilter _convolution;

            public SharpenFilter()
            {
                _convolution = new ConvolutionFilter();
            }

            public byte[,] Filter(byte[,] image)
            {
                if (image == null) throw new ArgumentNullException(nameof(image));

                var filterMatrix = new double[,]
                {
                    {0, -2, 0},
                    {-2, 11, -2},
                    {0, -2, 0}
                };
                //var filterMatrix = new double[,]
                //{
                //    {0, 0, 0, 0, 0},
                //    {0, 0, -1, 0, 0},
                //    {0, -1, 5, -1, 0},
                //    {0, 0, -1, 0, 0},
                //    {0, 0, 0, 0, 0}
                //};
                return _convolution.Filter(image, filterMatrix);
            }
        }

        public class ConvolutionFilter
        {
            private const int Factor = 1;
            private const int Bias = 0;

            public byte[,] Filter(byte[,] image, double[,] filter)
            {
                if (filter.GetLength(0) != filter.GetLength(1))
                {
                    throw new ArgumentException("invalid filter", nameof(filter));
                }

                int width = image.GetLength(0);
                int height = image.GetLength(1);

                var result = new byte[width, height];

                double intensiity = 0;


                int filterOffset = (filter.GetLength(0) - 1)/2;
                for (int offsetY = filterOffset; offsetY < height - filterOffset; offsetY++)
                {
                    for (int offsetX = filterOffset; offsetX < width - filterOffset; offsetX++)
                    {
                        intensiity = 0;


                        for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                        {
                            for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                            {
                                var imagePixel = image[offsetX + filterX, offsetY + filterY];


                                intensiity += (double) (imagePixel)*
                                              filter[filterY + filterOffset, filterX + filterOffset];
                            }
                        }

                        intensiity = Factor*intensiity + Bias;

                        result[offsetX, offsetY] = ToByte(intensiity);
                    }
                }

                return result;
            }

            private byte ToByte(double value)
            {
                if (value > 255)
                {
                    value = 255;
                }
                else if (value < 0)
                {
                    value = 0;
                }
                return (byte) value;
            }
        }
    }

    public class EdgeFilter
    {
        public string Name => nameof(EdgeFilter);
        public byte[,] Filter(byte[,] image)
        {
            if (image == null) throw new ArgumentNullException("image");

            var heigth = image.GetLength(1);
            var width = image.GetLength(0);
            var newImage = new byte[width, heigth];
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < heigth; j++)
                {
                    if ((i > 0 && i < width - 1) && (j > 0 && j < heigth - 1))
                    {
                        var left = Math.Abs(image[i, j] - image[i - 1, j]);
                        var upper = Math.Abs(image[i, j] - image[i, j - 1]);
                        var rightUpper = Math.Abs(image[i, j] - image[i + 1, j - 1]);
                        newImage[i, j] = (byte)Math.Max(Math.Max(left, upper), rightUpper);
                    }
                    else
                    {
                        newImage[i, j] = 0;
                    }
                }
            }
            return newImage;
        }

        
    }
}