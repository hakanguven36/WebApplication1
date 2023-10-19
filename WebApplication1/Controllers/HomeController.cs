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

        public IActionResult Index(int? projectID)
        {
            if(projectID != null)
                HttpContext.Session.SetInt32("selectedProjectID", projectID??0);
            ViewBag.selectedProjectID = HttpContext.Session.GetInt32("selectedProjectID");
            return View();
        }

        public IActionResult GetProjectList()
        {
            return Json(db.Project.Include(u => u.annoList).ToList());
        }

        public IActionResult GetImage(NavigateViewModel navi)
        {
            navi.error = "";
            int projectID = navi.projectID;

            switch (navi.seen)
            {
                case 1:
                    navi.filesCount = db.Photo.Where(u => u.ProjectID == projectID).Where(u => u.completed == false).Count();
                    break;
                case 2:
                    navi.filesCount = db.Photo.Where(u => u.ProjectID == projectID).Where(u => u.completed == true).Count();
                    break;
                case 3:
                    navi.filesCount = db.Photo.Where(u => u.ProjectID == projectID).Count();
                    break;
            }

            if (navi.filesCount == 0)
            {
                navi.error = "Veritabanında resim yok!";
                return Json(navi);
            }


            Photo photo = new Photo();
            switch (navi.seen)
            {
                case 1:
                    navi.imageNo = Math.Clamp(navi.imageNo, 1, navi.filesCount);
                    photo = db.Photo.Where(u => u.ProjectID == projectID).Where(u => u.completed == false).Skip(navi.imageNo - 1).FirstOrDefault();
                    break;
                case 2:
                    navi.imageNo = Math.Clamp(navi.imageNo, 1, navi.filesCount);
                    photo = db.Photo.Where(u => u.ProjectID == projectID).Where(u => u.completed == true).Skip(navi.imageNo - 1).FirstOrDefault();
                    break;
                case 3:
                    navi.imageNo = Math.Clamp(navi.imageNo, 1, navi.filesCount);
                    photo = db.Photo.Where(u => u.ProjectID == projectID).Skip(navi.imageNo - 1).FirstOrDefault();
                    break;
            }

            if (photo == null)
            {
                navi.error = "Öyle bir resim yok!";
                return Json(navi);
            }

            navi.path = Path.Combine(rootPath, photo.sysname);
            navi.imageID = photo.ID;

            return Json(navi);
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
