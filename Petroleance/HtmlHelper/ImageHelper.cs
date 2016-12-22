using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace FanArt.Web.HtmlHelper
{
    public class ImageHelper
    { 
        public static string ImageParam(byte[] content)
        {
            MemoryStream stream = new MemoryStream(content);
            System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

            int height = image.Height;
            int width = image.Width;

            return width.ToString();
        }


        public static byte[] CropImage(byte[] content, int x, int y, int width, int height)
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                return CropImage(stream, x, y, width, height);
            }
        }

        public static byte[] CropImage(Stream content, int x, int y, int width, int height)
        {
            //Parsing stream to bitmap
            using (Bitmap sourceBitmap = new Bitmap(content))
            {
                //Get new dimensions
                double sourceWidth = Convert.ToDouble(sourceBitmap.Size.Width);
                double sourceHeight = Convert.ToDouble(sourceBitmap.Size.Height);
                Rectangle cropRect = new Rectangle(x, y, width, height);

                //Creating new bitmap with valid dimensions
                using (Bitmap newBitMap = new Bitmap(cropRect.Width, cropRect.Height))
                {
                    using (Graphics g = Graphics.FromImage(newBitMap))
                    {
                        g.DrawImage(sourceBitmap, new Rectangle(0, 0, newBitMap.Width, newBitMap.Height), cropRect, GraphicsUnit.Pixel);
                        return GetBitmapBytes(newBitMap);
                    }
                }

            }
        }

        public static byte[] GetBitmapBytes(Bitmap source)
        {
            //Settings to increase quality of the image
            ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()[1];
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, 80L);

            //Temporary stream to save the bitmap
            using (MemoryStream tmpStream = new MemoryStream())
            {
                source.Save(tmpStream, codec, parameters);

                //Get image bytes from temporary stream
                byte[] result = new byte[tmpStream.Length];
                tmpStream.Seek(0, SeekOrigin.Begin);
                tmpStream.Read(result, 0, (int)tmpStream.Length);

                return result;
            }
        }


        public static byte[] ResizeImage(byte[] content, Size size)
        {

            using (var streamOriginal = new MemoryStream(content))
            using (var imgOriginal = Image.FromStream(streamOriginal))
            {
                //get original width and height of the incoming image
                var originalWidth = imgOriginal.Width; // 1000
                var originalHeight = imgOriginal.Height; // 800

                //get the percentage difference in size of the dimension that will change the least
                var percWidth = ((float)size.Width / (float)originalWidth); // 0.2
                var percHeight = ((float)size.Height / (float)originalHeight); // 0.25
                var percentage = Math.Max(percHeight, percWidth); // 0.25

                //get the ideal width and height for the resize (to the next whole number)
                var width = (int)Math.Max(originalWidth * percentage, size.Width); // 250
                var height = (int)Math.Max(originalHeight * percentage, size.Height); // 200

                //actually resize it
                using (var resizedBmp = new Bitmap(width, height))
                {
                    using (var graphics = Graphics.FromImage((Image)resizedBmp))
                    {
                        graphics.InterpolationMode = InterpolationMode.Low;
                        graphics.DrawImage(imgOriginal, 0, 0, width, height);
                    }
                    return GetBitmapBytes(resizedBmp);
                }
            }
        }

        public static byte[] ResizeImageFree(byte[] content, String size)
        {
            int height = 0;
            int width = 0;
            using (var streamOriginal = new MemoryStream(content))
            using (var imgOriginal = Image.FromStream(streamOriginal))
            {
                //get original width and height of the incoming image
                var originalWidth = imgOriginal.Width; // 1000
                var originalHeight = imgOriginal.Height; // 800

                //get the percentage difference in size of the dimension that will change the least
                if(originalWidth> originalHeight)
                {
                    if (size == "l")
                    {
                        height = ((int)originalHeight * 1280) / (int)originalWidth;
                        width = 1280;
                    }
                    else
                   if (size == "m")
                    {
                        height = ((int)originalHeight * 820) / (int)originalWidth;
                        width = 820;
                    }
                    else
                    if (size == "s")
                    {
                        height = ((int)originalHeight * 320) / (int)originalWidth;
                        width = 320;
                    }
                }
                else
                {
                    if (size == "l")
                    {
                        width = ((int)originalWidth * 720) / (int)originalHeight;
                        height = 720;
                    }
                    else
                   if (size == "m")
                    {
                        width = ((int)originalWidth * 405) / (int)originalHeight;
                        height = 405;
                    }
                    else
                    if (size == "s")
                    {
                        width = ((int)originalWidth * 180) / (int)originalHeight;
                        height = 180;
                     }
                }

                var percWidth = ((float)width / (float)originalWidth); // 0.2
                var percHeight = ((float)height / (float)originalHeight); // 0.25
                var percentage = Math.Max(percHeight, percWidth); // 0.25

                //get the ideal width and height for the resize (to the next whole number)
                var swidth = (int)Math.Max(originalWidth * percentage, width); // 250
                var sheight = (int)Math.Max(originalHeight * percentage, height); // 200

                using (var resizedBmp = new Bitmap(swidth, sheight))
                {
                    using (var graphics = Graphics.FromImage((Image)resizedBmp))
                    {
                        graphics.InterpolationMode = InterpolationMode.Low;
                        graphics.DrawImage(imgOriginal, 0, 0, width, height);
                    }
                    return GetBitmapBytes(resizedBmp);
                }
            }
        }

    }
}