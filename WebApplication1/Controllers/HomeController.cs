using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using WebApplication1.Araclar;

namespace WebApplication1.Controllers
{
    [Yetki]
    public class HomeController : Controller
    {
        private readonly MyContext db;
        string fileRoot = "";

        public HomeController(MyContext db, IWebHostEnvironment environment)
        {
            fileRoot = Path.Combine(environment.WebRootPath, "dosyalar");
            this.db = db;
        }

        public IActionResult Index(int? id = 1)
        {
            string hata = "";
            int skipint = id ?? 1;
            skipint--;
            int filesCount = db.HamResim.Count();
            if (filesCount == 0)
            {
                hata += "Veritabanında resim yok! ";
                ViewBag.hata = hata;
                return View();
            }
            if(skipint+1 > filesCount)
            {
                hata += "öyle bir resim yok! ";
                ViewBag.hata = hata;
                return View();
            }
            HamResim resim = db.HamResim.Skip(skipint).FirstOrDefault();

            
            
            ViewBag.path = Path.Combine("/dosyalar", resim.sysname);

            ViewBag.imageIndex = skipint+1;
            ViewBag.filesCount = filesCount;



            return View();
        }

        private static Image ResizeTo1280w(Image image)
        {
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            float percentage = (float)1280 / (float)sourceWidth;
            int destWidth = (int)(sourceWidth * percentage);
            int destHeight = (int)(sourceHeight * percentage);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (Image)b;
        }

        private static Image CropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        /*
        private static Image ResizeImage(Image imgToResize, Size size)
        {
            
            int sourceWidth = imgToResize.Width;
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

        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
