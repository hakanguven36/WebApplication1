using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WebApplication1.Araclar;
using WebApplication1.Models;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetImagePath(NavigateViewModel model)
        {
            model.error = "";

            int filesCount = db.Photo.Count();
            if (filesCount == 0)
            {
                model.error = "Veritabanında resim yok!";
                return Json(model);
            }

            model.imageNo = model.imageNo == 0 ? 1 : model.imageNo;

            Photo photo = new Photo();
            if(model.seen == null)
            {
                model.imageNo = Math.Clamp(model.imageNo, 1, filesCount);
                photo = db.Photo.Skip(model.imageNo - 1).FirstOrDefault();
            }
            else
            {
                filesCount = db.Photo.Where(u => u.completed == model.seen).Count();
                if (filesCount == 0)
                {
                    model.error = "Bu seçimde resim yok!";
                    return Json(model);
                }
                model.imageNo = Math.Clamp(model.imageNo, 1, filesCount);
                photo = db.Photo.Where(u => u.completed == false).Skip(model.imageNo - 1).FirstOrDefault();
            }
            

            if (photo == null)
            {
                model.error = "Öyle bir resim yok!";
                return Json(model);
            }
            else
            {
                model.path = Path.Combine(rootPath, photo.sysname);
                model.imageID = photo.ID;
                
            }
            return Json(model);
        }

        [HttpGet]
        public IActionResult GetLabels(int FotoID)
        {
            List<Label> labelList = db.Label.Include(u=>u.Annotation).Where(u => u.PhotoID == FotoID).ToList();
            List<LabelViewModel> lwm = new List<LabelViewModel>();
            foreach (var item in labelList)
            {
                var n = new LabelViewModel()
                {
                    AnnotationID = item.AnnotationID,
                    rectangle = new Rectangle() { beginX = item.beginX, beginY = item.beginY, endX = item.endX, endY = item.endY }
                };
                lwm.Add(n);
            }
            return Json(lwm);
        }

        [HttpPost]
        public IActionResult SetLabels(List<LabelViewModel> labelViewModel)
        {
            try
            {
                User currentUser = CurrentUser();
                if (currentUser == null)
                    throw new Exception("Kullanıcı değilsiniz!");

                int photoID = labelViewModel.FirstOrDefault().photoID;

                if (db.Label.Any(u => u.PhotoID == photoID))
                {
                    var previousListToDelete = db.Label.Where(u => u.PhotoID == photoID);
                    db.RemoveRange(previousListToDelete);
                    db.SaveChanges();
                }

                Photo photo = db.Photo.FirstOrDefault(u => u.ID == photoID);
                
                List<Label> labelList = new List<Label>();
                foreach (var item in labelViewModel)
                {
                    Annotation Annotation = db.Annotation.FirstOrDefault(u => u.ID == item.AnnotationID);

                    Label label = new Label();
                    label.Photo = photo;
                    label.User = currentUser;
                    label.Annotation = Annotation ;
                    label.beginX = item.rectangle.beginX;
                    label.beginY = item.rectangle.beginY;
                    label.endX = item.rectangle.endX;
                    label.endY = item.rectangle.endY;
                    labelList.Add(label);
                }
                db.AddRange(labelList);
                db.SaveChanges();

                photo.completed = true;
                db.Update(photo);
                db.SaveChanges();

                return Json("ok");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        private User CurrentUser() => HttpContext.Session.GetObject<User>("user");
    }
}
