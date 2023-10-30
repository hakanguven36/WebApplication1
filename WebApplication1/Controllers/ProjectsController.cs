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
using System.Text;

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
            return View(db.Project.Include(u => u.annoList).Include(u => u.photos).ToList());
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

        public IActionResult DownloadProject(int id)
        {
            Project project = db.Project.Include(u => u.photos).Include(u=>u.annoList).FirstOrDefault(u => u.ID == id);
            if (project == null)
                throw new Exception("Hata: Proje bulunamadı!");

            List<Photo> photoList = project.photos;
            if (photoList.Count == 0)
                throw new Exception("Hata: Projede fotoğraf yok!");


            List<object> photoObjs = new List<object>();
            foreach (Photo photo in photoList)
            {
                List<object> labelObjs = new List<object>();
                if (!string.IsNullOrWhiteSpace(photo.labels))
                {
                    List<Label> labelList = JsonConvert.DeserializeObject<List<Label>>(photo.labels);
                    foreach (Label label in labelList)
                    {
                        List<object> pointObjs = new List<object>();
                        foreach (Point point in label.points)
                        {
                            pointObjs.Add(new { x = (int)point.x, y = (int)point.y });
                        }
                        labelObjs.Add(new { annoID = label.annoID, points = Stringify(pointObjs) });
                    }
                }
                photoObjs.Add(new { photoName = photo.orjname, photoLabels = Stringify(labelObjs) });
            }

            var annoList = project.annoList.Select(u => u.name).ToList();
            List<object> annoObjs = new List<object>();
            foreach (string item in annoList)
            {
                annoObjs.Add(item);
            }
            object projectObj = new { projectName = project.name, annoList = Stringify(annoObjs), photos = Stringify(photoObjs) };

            string jsonobj = JsonConvert.SerializeObject(projectObj);
            return File(GetByteArray(jsonobj), "text/json", project.name + new Guid() + ".json");
        }

        private byte[] GetByteArray(string source)
        {
            Encoding ascii = Encoding.ASCII;
            return ascii.GetBytes(source);
        }

        private string Stringify(List<object> objList)
        {
            return JsonConvert.SerializeObject(objList);
        }
    }
}
