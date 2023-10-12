using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Araclar;
using WebApplication1.Models;

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
            return View(db.Project.Include(u=>u.Annotation).ToList());
        }

        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                return Json("ok");
            }
            catch(Exception e)
            {
                return Json(e.Message);
            }
        }
        
        public IActionResult Edit(int id)
        {
            var proje = db.Project.Include(u=>u.Annotation).FirstOrDefault(u=>u.ID ==  id);
            if (proje == null)
            {
                return Json("Proje bulunamadı!");
            }
            return Json(proje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Project proje)
        {
            try
            {
                db.Update(proje);
                db.SaveChanges();
                return Json("ok");
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
                var proje = db.Project.Include(u=>u.Annotation).FirstOrDefault(u => u.ID == id);
                db.Remove(proje);
                return Json("ok");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
    }
}
