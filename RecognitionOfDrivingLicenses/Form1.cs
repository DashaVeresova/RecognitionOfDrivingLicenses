using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Logging.Configuration;
using RecognitionOfDrivingLicenses.Filters;
using RecognitionOfDrivingLicenses.Helpers;
using RecognitionOfDrivingLicenses.Interfaces;
using Bsuir.Misoi.Core.Images.Implementation.Segmentation;
using Bsuir.Misoi.Core.Images.Implementation;
using Bsuir.Misoi.WebUI.Models;
using Bsuir.Misoi.WebUI.Storage.Implementation;
using System.Drawing.Imaging;

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
        private async void btnGrayImage_Click(object sender, EventArgs e)
        {
            await ApplyFilterTask(FilterType.Gray);
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
                    binarization = new AdaptiveBinarizatoinFilter(UpdateProgtessBar);
                    break;
                case FilterType.BinarizationByOtsu:
                    binarization = new BinarizationByOtsu(UpdateProgtessBar);
                    break;
                case FilterType.Gaus:
                    filter = new GausFilter(windowSize, false, UpdateProgtessBar);
                    break;
                case FilterType.Median:
                    filter = new Filters.MedianFilter(windowSize, false, UpdateProgtessBar);
                    break;
                case FilterType.Gray:
                    filter = new GrayFilter();
                    break;
                case FilterType.Edge:
                    filter = new Filters.EdgeFilter();
                    break;
                case FilterType.Sharpen:
                    filter = new SharpenFilter();
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
                result = filter.GetFilteredImage(bitmap);
            }
            else if (binarization != null)
            {
                result = binarization.GetBinaryImage(bitmap);
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
            try
            {
                Invoke(new Action(delegate
                {
                    if (progress <= 100)
                    {
                        progressBar1.Value = (int)progress;
                    }
                }));
            }
            catch (Exception exception) { }

        }

        private void btnSwitchImage_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox2.Image;
        }

        private async void bетDetect_Click(object sender, EventArgs e)
        {
            var originalImage = new Bitmap(pictureBox1.Image);

            var image = pictureBox1.Image;
            //await ApplyFilterTask(FilterType.BinarizationByOtsu);
            //btnSwitchImage_Click(sender, e);
            await ApplyFilterTask(FilterType.Sharpen);
            btnSwitchImage_Click(sender, e);
            //await ApplyFilterTask(FilterType.AdaptiveBinarization);
            //btnSwitchImage_Click(sender, e);
            //await ApplyFilterTask(FilterType.Median);
            //btnSwitchImage_Click(sender, e);
            await ApplyFilterTask(FilterType.Edge);
            btnSwitchImage_Click(sender, e);
            //await ApplyFilterTask(FilterType.Median);
            //btnSwitchImage_Click(sender, e);
            var filteredImage = new Bitmap(pictureBox2.Image);
            var bytes = FilterHelper.BitmapToArray(filteredImage);

            var imageHandler = new ImageHandler(originalImage, bytes);
            var rect = imageHandler.DetectionText(originalImage);
            //Bitmap tempImage = null;
            //if (rect.Width > 0 && rect.Height > 0)
            //{
            //    tempImage = filteredImage.Clone(rect, filteredImage.PixelFormat);
            //    imageHandler.Bitmap = tempImage;
            //    pictureBox1.Image = tempImage;
            //}

            var histogram = imageHandler.CalculateLineHistogram();
            var borders = imageHandler.DeterminateYCoordinates(histogram);
            var frames = imageHandler.DeterminatesWords(borders);

            using (var graphics = Graphics.FromImage(image))
            {
                var pen = new Pen(Color.OrangeRed, 1);
                foreach (var frame in frames.Where(x => x.Width > 4))
                {
                    graphics.DrawRectangle(pen, frame);
                }               
                pictureBox2.Image = image;
            }

            pictureBox1.Image = filteredImage;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var imageDataProvider = new ImageDataProvider();
            var imageFactory = new ImageFactory();

            var convolutionFilter = new Bsuir.Misoi.Core.Images.Implementation.ConvolutionFilter();

            var binarizationFilter = new BinarizationFilter();
            var segmentationAlogithm = new SegmentationAlgorithm(binarizationFilter);
            var findResultDrawer = new FindResultDrawer();

            var _imageProcessorService = new ImageProcessorsService(convolutionFilter, segmentationAlogithm, findResultDrawer);

            var _imageRepository = new ImageRepository(imageDataProvider, imageFactory);

            var _imageUrlProvider = new ImageUrlProvider();

            var bitmap = new Bitmap(pictureBox1.Image);
            var iimage = new BitmapImage(bitmap);
            var result = new ImageProcessorsResult();
            var sourceImage = iimage;

            var processResult = _imageProcessorService.ProcessImageAsync("Clip document", sourceImage).Result;
            if (processResult.Successful)
            {
                result.Successful = true;
                _imageRepository.SaveImageAsync(processResult.ProcessedImage).Wait();
                result.ProcessedImageUrl = _imageUrlProvider.GetImageUrl(processResult.ProcessedImage.Name);
                result.ProcessedImageName = processResult.ProcessedImage.Name;
                result.SourceImageUrl = _imageUrlProvider.GetImageUrl(sourceImage.Name);
                result.SourceImageName = sourceImage.Name;
                pictureBox2.Image = new Bitmap(processResult.ProcessedImage.Name + ".jpg");
            }
            else
            {
                result.Successful = false;
            }
            // return result;


        }
    }
}
