using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;
using WebApplication1.Araclar;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Yetki]
    public class HomeController : Controller
    {
        private readonly MyContext db;
        private readonly string rootPath;
        

        public HomeController(MyContext db)
        {
            rootPath = "/dosyalar";
            this.db = db;
        }

        public IActionResult Index(int? _imageNo, int? _seen)
        {
            int seen = _seen ?? 0;
            seen = Math.Clamp(seen, 0, 2);
            ViewBag.seen = seen;

            int imageNo = _imageNo ?? 1;
            
            int filesCount = db.Photo.Count();
            if (filesCount == 0)
            {
                ViewBag.hata = "Veritabanında resim yok!";
                return View();
            }

            Photo resim = new Photo();
            switch (seen)
            {
                case 0:
                    filesCount = db.Photo.Where(u => u.completed == false).Count();
                    imageNo = Math.Clamp(imageNo, 1, filesCount);
                    resim = db.Photo.Where(u => u.completed == false).Skip(imageNo - 1).FirstOrDefault();
                    break;
                case 1:
                    filesCount = db.Photo.Where(u => u.completed == true).Count();
                    imageNo = Math.Clamp(imageNo, 1, filesCount);
                    resim = db.Photo.Where(u => u.completed == true).Skip(imageNo - 1).FirstOrDefault();
                    break;
                default:
                    imageNo = Math.Clamp(imageNo, 1, filesCount);
                    resim = db.Photo.Skip(imageNo-1).FirstOrDefault();
                    break;
            }


            ViewBag.imageNo = imageNo;
            ViewBag.filesCount = filesCount;

            if (resim == null)
            {
                ViewBag.hata = "Öyle bir resim yok!";
                return View();
            }
            else
            {
                ViewBag.path = Path.Combine(rootPath, resim.sysname);
                ViewBag.imageID = resim.ID;
            }
            return View();
        }

        public IActionResult GetEtiketler(int FotoID)
        {
            List<LabelViewModel> labels = new List<LabelViewModel>();
            foreach (var item in db.Label.Where(u => u.PhotoID == FotoID).ToList())
            {
                labels.Add(new LabelViewModel() { label = item.label, cursorCol = item.cursorCol, cursorRow = item.cursorRow, cursorSize = item.cursorSize });
            } 
            return Json(labels);
        }

        [HttpPost]
        public IActionResult Tamamlanan(List<LabelViewModel> labels)
        {
            try
            {
                Photo foto = db.Photo.FirstOrDefault(u => u.ID == labels.FirstOrDefault().fotoID);

                List<Label> previousListToDelete = db.Label.Where(u => u.PhotoID == foto.ID).ToList();
                if (previousListToDelete.Count() > 0)
                {
                    db.RemoveRange(previousListToDelete);
                    db.SaveChanges();
                }

                if (labels.Count > 0)
                {
                    foto.completed = true;

                    foreach (var item in labels)
                    {
                        Label label = new Label();
                        label.Photo = foto;
                        label.label = item.label;
                        label.cursorCol = item.cursorCol;
                        label.cursorRow = item.cursorRow;
                        label.cursorSize = item.cursorSize;
                        db.Add(label);
                        db.SaveChanges();
                    }
                }
                else
                {
                    foto.completed = false;
                }
                db.Update(foto);
                db.SaveChanges();
                return Json("Tamam.");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
