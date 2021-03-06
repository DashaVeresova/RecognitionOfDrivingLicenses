﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Bsuir.Misoi.Core.Images.Implementation.Hough;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class BitmapImage : IImage
    {
        private byte[] _pixelBuffer;
        private int _bitMapStride;
        private int _height;
        private int _width;

        public BitmapImage(Bitmap bitmap)
        {
            FromBitmap(bitmap);
            bitmap.Dispose();
        }

        public void FromBitmap(Bitmap bitmap)
        {
            _height = bitmap.Height;
            _width = bitmap.Width;
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);
            _bitMapStride = bitmapData.Stride;
            _pixelBuffer = new byte[_bitMapStride*bitmap.Height];
            Marshal.Copy(bitmapData.Scan0, _pixelBuffer, 0, _pixelBuffer.Length);
            bitmap.UnlockBits(bitmapData);
        }

        public int Height => _height;

        public string Name { get; set; }

        public int Width => _width;

        public Pixel GetPixel(int x, int y)
        {
            int byteOffset = y * _bitMapStride + x * 4;
            return new Pixel { B = _pixelBuffer[byteOffset], G = _pixelBuffer[byteOffset + 1], R = _pixelBuffer[byteOffset + 2] };
        }

        public void Save(Stream saveStream)
        {
            Bitmap resultBitmap = ToBitmap();

            var extension = Path.GetExtension(this.Name);
            ImageFormat imageFormat;
            if (extension == ".png")
            {
                imageFormat = ImageFormat.Png;
            }
            else if (extension == ".jpg" || extension == ".jpeg")
            {
                imageFormat = ImageFormat.Jpeg;
            }
            else
            {
                extension = ".jpg";
                imageFormat = ImageFormat.Jpeg;
                //throw new NotSupportedException("unsupported image format");
            }
            resultBitmap.Save(this.Name + extension);
            resultBitmap.Dispose();
        }

        public void SetPixel(int x, int y, Pixel pixel)
        {
            int byteOffset = y * _bitMapStride + x * 4;
            _pixelBuffer[byteOffset] = pixel.B;
            _pixelBuffer[byteOffset + 1] = pixel.G;
            _pixelBuffer[byteOffset + 2] = pixel.R;
        }

        public IImage Clone()
        {
            var image = (BitmapImage)this.MemberwiseClone();
            image._pixelBuffer = (byte[])_pixelBuffer.Clone();
            image.Name = Guid.NewGuid() + Path.GetExtension(Name);
            return image;
        }

        public void Clip(IList<Point> points, float angle)
        {
            //int xOffset = points.Min(p => p.X);
            //int yOffset = points.Min(p => p.Y);

            //GraphicsPath gp = new GraphicsPath();   // a Graphicspath
            //gp.AddPolygon(points.Select(p => new PointF(p.X, p.Y)).ToArray());        // with one Polygon

            //Bitmap bmp1 = new Bitmap(500, 500);  // ..some new Bitmap
            //                                     // and some old one..:
            //using (Bitmap bmp0 = ToBitmap())
            //{
            //    using (Graphics g = Graphics.FromImage(bmp1))
            //    {
            //        g.Clip = new Region(gp); // restrict drawing region
            //        g.RotateTransform(45);
            //        g.DrawImage(bmp0, 0, 0); // draw clipped
            //    }

            //}
            //FromBitmap(bmp1);

            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;

            GraphicsPath gpdest = new GraphicsPath();

            var source = ToBitmap();

            //Your polygon
            var pesource = points.Select(p => new System.Drawing.Point(p.X, p.Y)).ToArray();

            //Determine the destination size/position
            x = source.Width;
            y = source.Height;

            foreach (var p in pesource)
            {
                if (p.X < x)
                    x = p.X;
                if (p.X > width)
                    width = p.X;

                if (p.Y < y)
                    y = p.Y;
                if (p.Y > height)
                    height = p.Y;
            }

            height = height - y;
            width = width - x;

            gpdest.AddPolygon(pesource);
            Matrix m = new Matrix(1, 0, 0, 1, -x, -y);
            gpdest.Transform(m);

            //Create the Bitmap
            var clipped = new Bitmap(width, height);
            for (int i = 0; i < clipped.Height; i++)
            {
                for (int j = 0; j < clipped.Width; j++)
                {
                    clipped.SetPixel(j, i, Color.White);
                }
            }

            //Draw on the Bitmap
            using (Graphics g = Graphics.FromImage(clipped))
            {
                var rotationMatrinx = new Matrix();
                rotationMatrinx.RotateAt(angle, new PointF(width / 2, height / 2));
                g.Transform = rotationMatrinx;
                g.SetClip(gpdest);
                g.DrawImage(source, -x, -y);
            }



            FromBitmap(clipped);
        }

        public static Bitmap CropRotatedRect(Bitmap source, Rectangle rect, float angle)
        {
            Bitmap result = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                using (Matrix mat = new Matrix())
                {
                    mat.Translate(-rect.Location.X, -rect.Location.Y);
                    mat.RotateAt(angle, rect.Location);
                    g.Transform = mat;
                    g.DrawImage(source, new System.Drawing.Point(0, 0));
                }
            }
            return result;
        }

        public Bitmap ToBitmap()
        {
            Bitmap resultBitmap = new Bitmap(_width, _height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                     resultBitmap.Width, resultBitmap.Height),
                                                      ImageLockMode.WriteOnly,
                                                 PixelFormat.Format32bppArgb);

            Marshal.Copy(_pixelBuffer, 0, resultData.Scan0, _pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
    }
}
