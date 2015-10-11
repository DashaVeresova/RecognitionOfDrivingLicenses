namespace RecognitionOfDrivingLicenses
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnBinarization = new System.Windows.Forms.Button();
            this.trbrfilterWndow = new System.Windows.Forms.TrackBar();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.btnMedianFilter = new System.Windows.Forms.Button();
            this.btnGausFilter = new System.Windows.Forms.Button();
            this.lblWindowSize = new System.Windows.Forms.Label();
            this.btnAdaptiveBinarization = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnGrayImage = new System.Windows.Forms.Button();
            this.btnSwitchImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbrfilterWndow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(12, 92);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(625, 481);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnBinarization
            // 
            this.btnBinarization.Location = new System.Drawing.Point(12, 12);
            this.btnBinarization.Name = "btnBinarization";
            this.btnBinarization.Size = new System.Drawing.Size(90, 23);
            this.btnBinarization.TabIndex = 2;
            this.btnBinarization.Text = "Бинаризация";
            this.btnBinarization.UseVisualStyleBackColor = true;
            this.btnBinarization.Click += new System.EventHandler(this.btnBinarization_Click);
            // 
            // trbrfilterWndow
            // 
            this.trbrfilterWndow.Location = new System.Drawing.Point(12, 43);
            this.trbrfilterWndow.Maximum = 50;
            this.trbrfilterWndow.Name = "trbrfilterWndow";
            this.trbrfilterWndow.Size = new System.Drawing.Size(446, 45);
            this.trbrfilterWndow.TabIndex = 4;
            this.trbrfilterWndow.Value = 2;
            this.trbrfilterWndow.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox2.ImageLocation = "";
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(643, 92);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(625, 481);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(1124, 63);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(141, 23);
            this.btnOpenFile.TabIndex = 6;
            this.btnOpenFile.Text = "Открыть Изображение";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // btnMedianFilter
            // 
            this.btnMedianFilter.Location = new System.Drawing.Point(263, 12);
            this.btnMedianFilter.Name = "btnMedianFilter";
            this.btnMedianFilter.Size = new System.Drawing.Size(126, 23);
            this.btnMedianFilter.TabIndex = 7;
            this.btnMedianFilter.Text = "Медианный Фильтр";
            this.btnMedianFilter.UseVisualStyleBackColor = true;
            this.btnMedianFilter.Click += new System.EventHandler(this.btnMedianFilter_Click);
            // 
            // btnGausFilter
            // 
            this.btnGausFilter.Location = new System.Drawing.Point(395, 12);
            this.btnGausFilter.Name = "btnGausFilter";
            this.btnGausFilter.Size = new System.Drawing.Size(104, 23);
            this.btnGausFilter.TabIndex = 8;
            this.btnGausFilter.Text = "Фильтр Гаусса";
            this.btnGausFilter.UseVisualStyleBackColor = true;
            this.btnGausFilter.Click += new System.EventHandler(this.btnGausFilter_Click);
            // 
            // lblWindowSize
            // 
            this.lblWindowSize.AutoSize = true;
            this.lblWindowSize.Location = new System.Drawing.Point(464, 52);
            this.lblWindowSize.Name = "lblWindowSize";
            this.lblWindowSize.Size = new System.Drawing.Size(27, 13);
            this.lblWindowSize.TabIndex = 9;
            this.lblWindowSize.Text = "Size";
            // 
            // btnAdaptiveBinarization
            // 
            this.btnAdaptiveBinarization.Location = new System.Drawing.Point(108, 13);
            this.btnAdaptiveBinarization.Name = "btnAdaptiveBinarization";
            this.btnAdaptiveBinarization.Size = new System.Drawing.Size(149, 23);
            this.btnAdaptiveBinarization.TabIndex = 10;
            this.btnAdaptiveBinarization.Text = "Адаптивная Бинаризация";
            this.btnAdaptiveBinarization.UseVisualStyleBackColor = true;
            this.btnAdaptiveBinarization.Click += new System.EventHandler(this.btnAdaptiveBinarization_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 579);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1253, 23);
            this.progressBar1.TabIndex = 11;
            // 
            // btnGrayImage
            // 
            this.btnGrayImage.Location = new System.Drawing.Point(506, 12);
            this.btnGrayImage.Name = "btnGrayImage";
            this.btnGrayImage.Size = new System.Drawing.Size(122, 23);
            this.btnGrayImage.TabIndex = 12;
            this.btnGrayImage.Text = "Серое Изображение";
            this.btnGrayImage.UseVisualStyleBackColor = true;
            this.btnGrayImage.Click += new System.EventHandler(this.btnGrayImage_Click);
            // 
            // btnSwitchImage
            // 
            this.btnSwitchImage.Location = new System.Drawing.Point(622, 70);
            this.btnSwitchImage.Name = "btnSwitchImage";
            this.btnSwitchImage.Size = new System.Drawing.Size(34, 23);
            this.btnSwitchImage.TabIndex = 13;
            this.btnSwitchImage.Text = "<<---";
            this.btnSwitchImage.UseVisualStyleBackColor = true;
            this.btnSwitchImage.Click += new System.EventHandler(this.btnSwitchImage_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1277, 612);
            this.Controls.Add(this.btnSwitchImage);
            this.Controls.Add(this.btnGrayImage);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnAdaptiveBinarization);
            this.Controls.Add(this.lblWindowSize);
            this.Controls.Add(this.btnGausFilter);
            this.Controls.Add(this.btnMedianFilter);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.trbrfilterWndow);
            this.Controls.Add(this.btnBinarization);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbrfilterWndow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnBinarization;
        private System.Windows.Forms.TrackBar trbrfilterWndow;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnMedianFilter;
        private System.Windows.Forms.Button btnGausFilter;
        private System.Windows.Forms.Label lblWindowSize;
        private System.Windows.Forms.Button btnAdaptiveBinarization;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnGrayImage;
        private System.Windows.Forms.Button btnSwitchImage;
    }
}

