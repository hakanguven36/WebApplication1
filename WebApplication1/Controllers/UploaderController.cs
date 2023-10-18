using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Araclar;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Yetki("admin")]
    public class UploaderController : Controller
    {
        private readonly MyContext db;
        string rootPath = "";

        public UploaderController(MyContext db, IWebHostEnvironment environment)
        {
            rootPath = Path.Combine(environment.WebRootPath, "dosyalar");
            this.db = db;
        }

        public IActionResult Index()
        {
            return View(db.Project.ToList());
        }

        public IActionResult ListFilesOfProject(int projectID)
        {
            List<Photo> photoList = db.Photo.Where(u => u.ProjectID == projectID).ToList();
            return PartialView(photoList);
        }

        [HttpPost]
        public IActionResult Yukleyici(IFormCollection form)
        {
            var files = form.Files;

            if (files.Count < 1)
                return Json("Hiç dosya gönderilmedi!");

            List<string> errorList = new List<string>();

            foreach (IFormFile file in files)
            {
                try
                {
                    var contentType = file.ContentType;
                    var fileName = file.FileName;
                    var lenght = file.Length;
                    string sysname = GetUniqueFileName(fileName);

                    using (Stream stream = new MemoryStream()) { 
                        file.CopyTo(stream);
                        Image orjImage = new Bitmap(stream);
                        Image image1024 = ResizeTo1280w(orjImage);
                        image1024.Save(Path.Combine(rootPath, sysname));

                        Image thumb = ResizeImage(orjImage, new Size(200,200));
                        thumb.Save(Path.Combine(rootPath, "thumbs", sysname));
                    }

                    Photo photo = new Photo();
                    int formProjeID = int.Parse(form["projectID"].tooString());
                    photo.Project = db.Project.FirstOrDefault(u => u.ID == formProjeID);
                    photo.contentType = contentType;
                    photo.sizeMB = lenght / (1024.0 * 1024.0);
                    photo.orjname = fileName;
                    photo.extention = Path.GetExtension(fileName);
                    photo.date = DateTime.Now;
                    photo.completed = false;
                    photo.sysname = sysname;
                    db.Add(photo);
                    db.SaveChanges();
                }
                catch(Exception e)
                {
                    errorList.Add(file.FileName + " yüklenemedi => " + e.Message.sapString(0,200));
                }
            }
            ViewBag.errorList = errorList;
            return View();
        }

        public IActionResult ResmiSil(int id)
        {
            Photo photo = db.Photo.FirstOrDefault(u => u.ID == id);
            if(photo != null)
            {
                FileInfo fi = new FileInfo(Path.Combine(rootPath, photo.sysname));
                if (fi.Exists)
                    fi.Delete();
                FileInfo ft = new FileInfo(Path.Combine(rootPath, "thumbs", photo.sysname));
                if (ft.Exists)
                    ft.Delete();

                db.Remove(photo);
                db.SaveChanges();
            }
            return Json("Silindi!");
        }

        public IActionResult ShowPreview(int id)
        {
            Photo r = db.Photo.FirstOrDefault(u => u.ID == id);
            ViewBag.path = Path.Combine("/dosyalar", r.sysname);
            return PartialView();
        }


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
            g.InterpolationMode = InterpolationMode.Bicubic;
            // Draw image with new width and height
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (Image)b;
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

        private string GetUniqueFileName(string fileName)
        {
            return Guid.NewGuid().ToString() + Path.GetExtension(fileName);
        }
    }
}
