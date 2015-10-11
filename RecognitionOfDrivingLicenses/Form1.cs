using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using RecognitionOfDrivingLicenses.Filters;
using RecognitionOfDrivingLicenses.Helpers;
using RecognitionOfDrivingLicenses.Interfaces;

namespace RecognitionOfDrivingLicenses
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = trbrfilterWndow.Value.ToString(CultureInfo.InvariantCulture);
        }

        public Bitmap GetInitialBitmap()
        {
            var bitmap = new Bitmap(pictureBox1.Image);
            return bitmap;
        }

        private void btnBinarization_Click(object sender, EventArgs e)
        {
            ApplyFilter(FilterType.BinarizationByOtsu);
        }
        
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void btnMedianFilter_Click(object sender, EventArgs e)
        {
            ApplyFilter(FilterType.Median);
        }

        private void btnGausFilter_Click(object sender, EventArgs e)
        {
            ApplyFilter(FilterType.Gaus);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = trbrfilterWndow.Value.ToString(CultureInfo.InvariantCulture);
        }

        private void btnAdaptiveBinarization_Click(object sender, EventArgs e)
        {
            ApplyFilter(FilterType.AdaptiveBinarization);
        }

        private void ApplyFilter(FilterType filterType)
        {
            IFilter filter = null;
            IBinarization binarization = null;

            if (FilterType.AdaptiveBinarization == filterType)
            {
                binarization = new AdaptiveBinarizatoinFilter();
            }
            else if (FilterType.BinarizationByOtsu == filterType)
            {
                binarization = new BinarizationByOtsu();
            }
            else if (FilterType.Gaus == filterType)
            {
                filter = new GausFilter();
            }
            else if (FilterType.Median == filterType)
            {
                filter = new MedianFilter();
            }

            var bitmap = GetInitialBitmap();
            Bitmap result = null;

            if (filter != null)
            {
                result = filter.GetFilteredImage(bitmap, trbrfilterWndow.Value);
            }
            else if (binarization != null)
            {
                result = binarization.GetBinaryImage(bitmap);
            }

            pictureBox2.Image = result;
        }
    }
}
