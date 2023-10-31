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

        public IActionResult GetImage(string _navi)
        {
            NavigateViewModel navi = Newtonsoft.Json.JsonConvert.DeserializeObject<NavigateViewModel>(_navi);
            navi.error = "";
            navi.path = "";
            navi.orjname = "";
            navi.labels = "[]";
            navi.filesCount = 0;
            navi.photoID = 0;
            
            int projectID = navi.projectID;
            if(projectID == 0)
            {
                navi.error = "Proje ID boş olamaz!";
                navi.photoNo = 0;
                return JSON(navi);
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
                navi.photoNo = 0;
                return JSON(navi);
            }

            Photo photo = new Photo();
            switch (navi.seen)
            {
                case 1:
                    navi.photoNo = Math.Clamp(navi.photoNo, 1, navi.filesCount);
                    photo = db.Photo.Where(u => u.ProjectID == projectID).Where(u => u.completed == false).Skip(navi.photoNo - 1).FirstOrDefault();
                    break;
                case 2:
                    navi.photoNo = Math.Clamp(navi.photoNo, 1, navi.filesCount);
                    photo = db.Photo.Where(u => u.ProjectID == projectID).Where(u => u.completed == true).Skip(navi.photoNo - 1).FirstOrDefault();
                    break;
                case 3:
                    navi.photoNo = Math.Clamp(navi.photoNo, 1, navi.filesCount);
                    photo = db.Photo.Where(u => u.ProjectID == projectID).Skip(navi.photoNo - 1).FirstOrDefault();
                    break;
            }

            if (photo == null)
            {                
                navi.error = "Bu sınırlar içinde resim yoktur. Sistem hatası olabilir!";                
                navi.photoNo = 0;
                return JSON(navi);
            }

            navi.path = Path.Combine(rootPath, photo.sysname);
            navi.orjname = photo.orjname;
            navi.labels = photo.labels??"[]";
            navi.photoID = photo.ID;

            return JSON(navi);
        }

        

        [HttpPost]
        public IActionResult SetImage(string _navi)
        {
            NavigateViewModel navi = Newtonsoft.Json.JsonConvert.DeserializeObject<NavigateViewModel>(_navi);
            try
            {
                User currentUser = CurrentUser();
                if (currentUser == null)
                    throw new Exception("Kullanıcı değilsiniz!");

                int projectID = navi.projectID;
                int photoID = navi.photoID;

                Photo photo = db.Photo.FirstOrDefault(u => u.ID == photoID);
                photo.labels = navi.labels;
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

        private JsonResult JSON(object obj)
            => Json(Newtonsoft.Json.JsonConvert.SerializeObject(obj));

        private User CurrentUser()
            => HttpContext.Session.GetObject<User>("user");
    }
}
