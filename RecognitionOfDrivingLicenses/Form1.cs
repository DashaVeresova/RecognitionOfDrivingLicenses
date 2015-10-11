using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecognitionOfDrivingLicenses
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = trackBar1.Value.ToString();
        }

        private void btnBinarization_Click(object sender, EventArgs e)
        {
            var image = GetOriginalImage();

            var binar = new BinarizationByOtsu();
            var result = binar.GetFilteredImage(FilterHelper.BitmapToArray(new Bitmap(image)));

            pictureBox2.Image = FilterHelper.ArrayToBitmap(result);
        }

        public Image GetOriginalImage()
        {
            return pictureBox1.Image;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void btnMedianFilter_Click(object sender, EventArgs e)
        { 
            var image = GetOriginalImage();
            var median = new MedianFilter();
            var result = median.GetFilteredImage(new Bitmap(image), trackBar1.Value);

            pictureBox2.Image = result;
        }

        private void btnGausFilter_Click(object sender, EventArgs e)
        {
            var image = GetOriginalImage();

            var gaus = new GausFilter();
            var result = gaus.GetFilteredImage(new Bitmap(image), trackBar1.Value);

            pictureBox2.Image = result;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value.ToString();
        }

        private void btnAdaptiveBinarization_Click(object sender, EventArgs e)
        {
            var image = GetOriginalImage();

            var adaptive = new AdaptiveBinarizatoinFilter();
            var result = adaptive.GetFilteredImage(new Bitmap(image));

            pictureBox2.Image = result;
        }
    }
}
