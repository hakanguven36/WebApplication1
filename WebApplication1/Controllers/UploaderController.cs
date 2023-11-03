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
        private const int OrientationKey = 0x0112;
        private const int NotSpecified = 0;
        private const int NormalOrientation = 1;
        private const int MirrorHorizontal = 2;
        private const int UpsideDown = 3;
        private const int MirrorVertical = 4;
        private const int MirrorHorizontalAndRotateRight = 5;
        private const int RotateLeft = 6;
        private const int MirorHorizontalAndRotateLeft = 7;
        private const int RotateRight = 8;

        private readonly MyContext db;
        private string rootPath = "";

        public UploaderController(MyContext db, IWebHostEnvironment environment)
        {
            rootPath = Path.Combine(environment.WebRootPath, "dosyalar");
            this.db = db;
        }

        public IActionResult Index()
        {
            return View(db.Project.Include(u=>u.photos).ToList());
        }

        public IActionResult ListFilesOfProject(int projectID, int pageNo, int pageSize)
        {
            int photoCount = db.Photo.Where(u => u.ProjectID == projectID).Count();

            if (photoCount == 0)
            {
                return PartialView(new List<Photo>());
            }

            pageSize = pageSize == 0 ? 30 : pageSize;
            int pageCount = (int)Math.Ceiling(photoCount / (pageSize *1.0));
            pageNo = Math.Clamp(pageNo, 1, pageCount);

            List<Photo> photoList = db.Photo.Where(u => u.ProjectID == projectID).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            return PartialView(photoList);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 268435456)]
        public IActionResult Yukleyici(IFormCollection form)
        {
            try
            {
                var files = form.Files;

                if (files.Count < 1)
                    throw new Exception("No files has been sent!");

                int formProjeID = int.Parse(form["projectID"].tooString());
                if (formProjeID == 0)
                    throw new Exception("Project can't be empty!");

                List<string> errorList = new List<string>();

                foreach (IFormFile file in files)
                {
                    try
                    {
                        var contentType = file.ContentType;
                        var fileName = file.FileName;
                        var lenght = file.Length;
                        string sysname = GetUniqueFileName(fileName);

                        using (Stream stream = new MemoryStream())
                        {
                            file.CopyTo(stream);
                            Image orjImage = new Bitmap(stream);
                            
                            //Image image1024 = ResizeTo1280w(orjImage);
                            orjImage.Save(Path.Combine(rootPath, sysname));
                            
                            //Image thumb = ResizeImage(orjImage, new Size(100,100));
                            Image thumb = orjImage.GetThumbnailImage(100, 100, null, IntPtr.Zero);
                            thumb.Save(Path.Combine(rootPath, "thumbs", sysname));
                        }

                        Photo photo = new Photo();
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
                    catch (Exception e)
                    {
                        errorList.Add(file.FileName + " yüklenemedi => " + e.Message.sapString(0, 200));
                    }
                }
                ViewBag.errorList = errorList;
                return Json("ok");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
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
            return Json("Resim silindi!");
        }

        public IActionResult RemovePhotoFromDB(int id, bool hardRemove)
        {
            try
            {
                if (hardRemove)
                {
                    return ResmiSil(id);
                }
                else
                {
                    Photo photo = db.Photo.FirstOrDefault(u => u.ID == id);
                    string photoName = photo.orjname;
                    if (photo != null)
                    {
                        db.Remove(photo);
                        db.SaveChanges();
                        return Json("Şu resim kaldırıldı=> " + photoName );
                    }
                    else
                    {
                        return Json("Hata: Resim bulunamadı!");
                    }
                }
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
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

            #region Orientation Fix
            // Fix orientation if needed.
            /*
            if (imgToResize.PropertyIdList.Contains(OrientationKey))
            {
                var orientation = (int)imgToResize.GetPropertyItem(OrientationKey).Value[0];
                switch (orientation)
                {
                    case NotSpecified: // Assume it is good.
                    case NormalOrientation:
                        // No rotation required.
                        break;
                    case MirrorHorizontal:
                        b.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case UpsideDown:
                        b.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case MirrorVertical:
                        b.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case MirrorHorizontalAndRotateRight:
                        b.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case RotateLeft:
                        b.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case MirorHorizontalAndRotateLeft:
                        b.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case RotateRight:
                        b.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    default:
                        throw new NotImplementedException("An orientation of " + orientation + " isn't implemented.");
                }
            }
            */
            #endregion

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
