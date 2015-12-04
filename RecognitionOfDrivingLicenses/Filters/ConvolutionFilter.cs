using RecognitionOfDrivingLicenses.Interfaces;

namespace RecognitionOfDrivingLicenses.Filters
{
    public class ConvolutionFilter : IConvolutionFilter
    {
        private const int Factor = 1;
        private const int Bias = 0;

        public byte[,] Filter(byte[,] image, double[,] filter)
        {
            var result = new byte[image.GetLength(0), image.GetLength(1)];


            int filterOffset = (filter.GetLength(0) - 1) / 2;
            for (int offsetY = filterOffset; offsetY < image.GetLength(1) - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < image.GetLength(0) - filterOffset; offsetX++)
                {
                    double intensiity = 0;


                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            var imagePixel = image[offsetX + filterX, offsetY + filterY];


                            intensiity += (double)(imagePixel) * filter[filterY + filterOffset, filterX + filterOffset];
                        }
                    }

                    intensiity = Factor * intensiity + Bias;

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
            return (byte)value;
        }
    }
}
