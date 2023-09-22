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
        private readonly IWebHostEnvironment hostEnvironment;        
        private List<string> files;
        private int imageIndex = 0;
        private string root_ham, root_temp, root_bos, root_kultur, root_yabanci, root_karisik;

        public HomeController(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;

            root_ham = Path.Combine(hostEnvironment.WebRootPath, "dosyalar/Ham");
            root_temp = Path.Combine(hostEnvironment.WebRootPath, "dosyalar/Temp");
            root_bos = Path.Combine(hostEnvironment.WebRootPath, "dosyalar/Bos");
            root_kultur = Path.Combine(hostEnvironment.WebRootPath, "dosyalar/Kultur");
            root_yabanci = Path.Combine(hostEnvironment.WebRootPath, "dosyalar/Yabanci");
            root_karisik = Path.Combine(hostEnvironment.WebRootPath, "dosyalar/Karisik");

            files = Directory.GetFiles(root_ham).ToList();
        }

        public IActionResult Index(int imageIndex)
        {
            if (!Directory.Exists(root_ham) || !Directory.Exists(root_temp) || !Directory.Exists(root_bos) || !Directory.Exists(root_kultur) || !Directory.Exists(root_yabanci) || !Directory.Exists(root_karisik))
            {
                return Json("Klasörlerden biri yada birkaçı eksik! wwwroot/dosyalar içinde Ham, Temp, Bos, Kultur, Yabanci, Karisik isimli klasörleri oluşturun ");
            }

            if(files.Count == 0)
            {
                return Json("Belirtilen konumda görsel bulunamadı!");
            }

            if (imageIndex >= files.Count)
            {
                imageIndex = files.Count - 1;
            }
            else if(imageIndex < 0)
            {
                imageIndex = 0;
            }

            ViewBag.filesCount = files.Count;
            this.imageIndex = imageIndex;
            ViewBag.imageIndex = imageIndex;

            using (FileStream output = new FileStream(Path.Combine(root_temp, "temp1280.jpg"), FileMode.Create))
            {
                Image img = Image.FromFile(files[imageIndex]);
                Image newimg = ResizeTo1280w(img);
                newimg.Save(output, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            
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
