using System;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Logging.Configuration;
using RecognitionOfDrivingLicenses.Filters;
using RecognitionOfDrivingLicenses.Helpers;
using RecognitionOfDrivingLicenses.Interfaces;

namespace RecognitionOfDrivingLicenses
{
    public partial class Form1 : Form
    {
        private Task _task;

        public Form1()
        {
            InitializeComponent();
            lblWindowSize.Text = trbrfilterWndow.Value.ToString(CultureInfo.InvariantCulture);
        }

        public Bitmap GetInitialBitmap()
        {
            var bitmap = new Bitmap(pictureBox1.Image);
            return bitmap;
        }

        private async void btnBinarization_Click(object sender, EventArgs e)
        {
            await ApplyFilterTask(FilterType.BinarizationByOtsu);
        }

        private async void btnMedianFilter_Click(object sender, EventArgs e)
        {
            await ApplyFilterTask(FilterType.Median, trbrfilterWndow.Value);
        }

        private async void btnGausFilter_Click(object sender, EventArgs e)
        {
            await ApplyFilterTask(FilterType.Gaus, trbrfilterWndow.Value);
        }
        private async void btnAdaptiveBinarization_Click(object sender, EventArgs e)
        {
            await ApplyFilterTask(FilterType.AdaptiveBinarization);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            lblWindowSize.Text = trbrfilterWndow.Value.ToString(CultureInfo.InvariantCulture);
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }
        
        private Task ApplyFilterTask(FilterType filterType, int windowSize = 0)
        {
            try
            {
                _task = new Task(() => ApplyFilter(filterType, windowSize));
                _task.Start();

                return _task;
            }
            catch (Exception exception) { }

            return null;
        }

        private void ApplyFilter(FilterType filterType, int windowSize = 0)
        {
            Invoke(new ArgUtils.Action(delegate
            {
                pictureBox2.Image = null;
            }));

            IFilter filter = null;
            IBinarization binarization = null;

            switch (filterType)
            {
                case FilterType.AdaptiveBinarization:
                    binarization = new AdaptiveBinarizatoinFilter();
                    break;
                case FilterType.BinarizationByOtsu:
                    binarization = new BinarizationByOtsu();
                    break;
                case FilterType.Gaus:
                    filter = new GausFilter();
                    break;
                case FilterType.Median:
                    filter = new MedianFilter();
                    break;
            }

            var bitmap = GetInitialBitmap();
            Bitmap result = null;

            Invoke(new ArgUtils.Action(delegate
            {
                progressBar1.Value = 0;
            }));

            if (filter != null)
            {
                result = filter.GetFilteredImage(bitmap, windowSize, UpdateProgtessBar);
            }
            else if (binarization != null)
            {
                result = binarization.GetBinaryImage(bitmap, UpdateProgtessBar);
            }
            
            Invoke(new ArgUtils.Action(delegate
            {
                if (progressBar1.Value < 100)
                {
                    progressBar1.Value = 100;
                }
                pictureBox2.Image = result;
            }));
        }

        private void UpdateProgtessBar(double progress)
        {
            Invoke(new Action(delegate
            {
                if (progress <= 100)
                {
                    progressBar1.Value = (int)progress;
                }
            }));
        }
    }
}
