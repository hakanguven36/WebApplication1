using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Araclar;
using WebApplication1.Models;
using System.Text.Json;

namespace WebApplication1.Controllers
{
    [Yetki("admin")]
    public class ProjectsController : Controller
    {
        private readonly MyContext db;

        public ProjectsController(MyContext db)
        {
            this.db = db;
        }

        
        public IActionResult Index()
        {
            return View(db.Project.Include(u=>u.annoList).ToList());
        }

        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult Create(Project project)
        {
            try
            {
                if(db.Project.FirstOrDefault(u=>u.name == project.name) != null)
                {
                    return Json("", "Bu isimde bir proje zaten var!");
                }

                db.Add(project);
                db.SaveChanges();

                return Json("Proje oluşturuldu.");
            }
            catch(Exception e)
            {
                return Json(e.Message);
            }
        }
        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var proje = db.Project.FirstOrDefault(u=>u.ID ==  id);
            if (proje == null)
            {
                return Json("Proje bulunamadı!");
            }
            
            return PartialView(proje);
        }
        public IActionResult GetAnnoList(int id)
        {
            List<Annotation> annoList = db.Annotation.Where(u => u.ProjectID == id).ToList();
            return Json(annoList);
        }

        [HttpPost]
        public IActionResult Edit(Project project)
        {
            try
            {
                var listToDelete = db.Annotation.Where(u => u.ProjectID == project.ID);
                db.RemoveRange(listToDelete);
                db.SaveChanges();

                db.Update(project);
                db.SaveChanges();
                return Json("Değişiklikler uygulandı.");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var proje = db.Project.Include(u=>u.annoList).FirstOrDefault(u => u.ID == id);
                db.Remove(proje);
                db.SaveChanges();
                return Json("Silindi.");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
    }
}
