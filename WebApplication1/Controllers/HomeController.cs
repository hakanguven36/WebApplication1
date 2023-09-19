using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        List<string> files;
        private readonly IWebHostEnvironment hostEnvironment;

        public HomeController(IWebHostEnvironment hostEnvironment)
        {
            string HamDir = hostEnvironment.WebRootPath + @"/dosyalar/Ham";
            files = Directory.GetFiles(HamDir).ToList();
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult Index(int resindex)
        {

            ViewBag.filesCount = files.Count();
            ViewBag.resindex = resindex;

            FileStream output = new FileStream(hostEnvironment.WebRootPath + @"/dosyalar/Temp/buyuk.jpg", FileMode.Create);
            String path = Path.Combine(files[resindex]);
            Image img = Image.FromFile(path);
            Image newimg = ResizeImage(img, new Size(1280, 720));
            newimg.Save(output, System.Drawing.Imaging.ImageFormat.Jpeg);
            output.Close();

            return View();

            //finally
            //{
            //    HttpContext.Response.OnCompleted(async () => await Task.Run(() => output.Dispose()));
            //}
        }


        public IActionResult ResimCrop(int resindex, int cropindex)
        {
            MemoryStream output = new MemoryStream();
            try
            {
                Image img = Image.FromFile(files[resindex]);
                Image newimg = ResizeImage(img, new Size(1280, 720));
                Image cropped = CropImage(newimg, new Rectangle((cropindex / 5) * 256 , (cropindex % 5) * 144, 256, 144));
                cropped.Save(output, System.Drawing.Imaging.ImageFormat.Jpeg);
                return File(output.GetBuffer(), "image/jpeg");
            }
            catch (Exception e)
            {
                return Json("Hata: " + e.Message);
            }
            finally
            {
                HttpContext.Response.OnCompleted(async () => await Task.Run(() => output.Dispose()));
            }
        }

        private static Image CropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }



        private static Image ResizeImage(Image imgToResize, Size size)
        {
            // Get the image current width
            int sourceWidth = imgToResize.Width;
            // Get the image current height
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            // Calculate width and height with new desired size
            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);
            nPercent = Math.Min(nPercentW, nPercentH);
            // New Width and Height
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (Image)b;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
