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


        [HttpGet]
        public IActionResult GetLabels(int FotoID) =>
            Json(db.Label.Where(u => u.PhotoID == FotoID).ToList());
        

        [HttpPost]
        public IActionResult SetLabels(List<Label> labelList)
        {
            try
            {
                User currentUser = CurrentUser();
                if (currentUser == null)
                    throw new Exception("Kullanıcı değilsiniz!");

                int photoID = labelList.FirstOrDefault().PhotoID;

                if (db.Label.Any(u => u.PhotoID == photoID))
                {
                    var previousListToDelete = db.Label.Where(u => u.PhotoID == photoID);
                    db.RemoveRange(previousListToDelete);
                    db.SaveChanges();
                }

                Photo photo = db.Photo.FirstOrDefault(u => u.ID == photoID);

                if (labelList.Any())
                {
                    labelList.ForEach(u => u.UserID = currentUser.ID);
                    db.AddRange(labelList);
                    db.SaveChanges();

                    photo.completed = true;
                }
                else
                {
                    photo.completed = false;
                }
                db.Update(photo);
                db.SaveChanges();
                return Json("Tamam.");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }


        private User CurrentUser()
        {
            User user = HttpContext.Session.GetObject<User>("user");
            return user;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
