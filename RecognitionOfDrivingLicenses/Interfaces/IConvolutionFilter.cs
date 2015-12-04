namespace RecognitionOfDrivingLicenses.Interfaces
{
    public interface IConvolutionFilter
    {
        byte[,] Filter(byte[,] image, double[,] filter);
    }
}
