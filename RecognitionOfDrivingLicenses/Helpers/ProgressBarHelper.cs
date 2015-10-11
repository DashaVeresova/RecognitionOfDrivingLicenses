namespace RecognitionOfDrivingLicenses.Helpers
{
    public static class ProgressBarHelper
    {
        public static void UpdateProgresssBar(double progress, ProgressDelegate progressDelegate)
        {
            if (progressDelegate != null)
            {
                progressDelegate.Invoke(progress);
            }
        }
    }
}
