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
            int projectID = HttpContext.Session.GetInt32("projectID")??0;
            if (projectID != 0)
                ViewBag.projectID = projectID;
            return View();
        }

        public IActionResult GetProjectList(int projectID)
        {
            if(projectID != 0)
                HttpContext.Session.SetInt32("projectID", projectID);
            return Json(db.Project.Include(u => u.annoList).ToList());
        }

        public IActionResult GetImage(NavigateViewModel navi)
        {
            navi.error = "";
            int projectID = navi.projectID;
            if(projectID == 0)
            {
                navi.error = "Proje ID boş olamaz!";
                navi.path = "";
                navi.filesCount = 0;
                navi.imageID = 0;
                navi.imageNo = 0;
                return Json(navi);
            }

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
                navi.error = "Bu bağlamda resim bulunmamaktadır!";
                navi.path = "";
                navi.filesCount = 0;
                navi.imageID = 0;
                navi.imageNo = 0;
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
                navi.error = "Bu sınırlar içinde resim yoktur. Sistem hatası olabilir!";
                navi.path = "";
                navi.filesCount = 0;
                navi.imageID = 0;
                navi.imageNo = 0;
                return Json(navi);
            }

            navi.path = Path.Combine(rootPath, photo.sysname);
            navi.imageID = photo.ID;

            return Json(navi);
        }

        [HttpGet]
        public IActionResult GetLabels(int photoID)
        {
            List<Label> labelList = db.Label.Include(u=>u.points).Where(u => u.photoID == photoID).ToList();
            List<LabelViewModel> model = new List<LabelViewModel>();
            foreach (var item in labelList)
            {
                var m = new LabelViewModel();
                m.id = item.ID;
                m.annoID = item.annoID;
                m.photoID = item.photoID;
                m.projectID = item.projectID;
                m.sizeFactor = item.sizeFactor;
                m.shape = (int)item.shape;

                m.points = new List<Point>();
                if(item.points != null)
                    foreach (var p in item.points)
                    {
                        m.points.Add(new Point() { x = p.X, y = p.Y });
                    }
                model.Add(m);
            }
            return Json(model);
        }

        [HttpPost]
        public IActionResult SetLabels(string labelListJson)
        {
            List<LabelViewModel> modelList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LabelViewModel>>(labelListJson);
            try
            {
                User currentUser = CurrentUser();
                if (currentUser == null)
                    throw new Exception("Kullanıcı değilsiniz!");

                // modelden GENEL bilgileri al
                int projectID = modelList.FirstOrDefault().projectID;
                int photoID = modelList.FirstOrDefault().photoID;

                // Bu fotoğrafla ilgili tüm label bilgisini sil.
                var previousListToDelete = db.Label.Where(u => u.photoID == photoID).ToList();
                if (previousListToDelete.Any())
                {
                    db.RemoveRange(previousListToDelete);
                    db.SaveChanges();
                }

                // modelden gelen label bilgisini listele
                List<Label> labelList = new List<Label>();
                foreach (var item in modelList)
                {
                    Annotation Annotation = db.Annotation.FirstOrDefault(u => u.ID == item.annoID);

                    Label label = new Label();
                    label.photoID = photoID;
                    label.projectID = item.projectID;
                    label.userID = currentUser.ID;
                    label.annoID = item.annoID;
                    label.sizeFactor = item.sizeFactor;
                    label.shape = (SHAPE)item.shape;

                    List<Coordinate> points = new List<Coordinate>();
                    foreach (var p in item.points)
                    {
                        points.Add(new Coordinate() { X = p.x, Y = p.y });
                    }
                    label.points = points;

                    labelList.Add(label);
                }

                // listelenen label bilgisini db'e kaydet.
                db.AddRange(labelList);
                db.SaveChanges();

                // Fotoğrafın bilgisini güncelle.
                Photo photo = db.Photo.FirstOrDefault(u => u.ID == photoID);
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
