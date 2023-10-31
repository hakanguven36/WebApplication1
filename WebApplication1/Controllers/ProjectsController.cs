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
            Project project = db.Project.Include(u => u.photos).Include(u => u.annoList).FirstOrDefault(u => u.ID == id);
            if (project == null)
                throw new Exception("Hata: Proje bulunamadı!");

            List<Photo> photoList = project.photos;
            if (photoList.Count == 0)
                throw new Exception("Hata: Projede fotoğraf yok!");

            ModelJson.Project project_json = new ModelJson.Project();
            project_json.name = project.name;
            List<ModelJson.Annotation> annotationList = new List<ModelJson.Annotation>();
            foreach (var annotation in project.annoList)
            {
                ModelJson.Annotation anno_json = new ModelJson.Annotation() { name = annotation.name };
                annotationList.Add(anno_json);
            }
            project_json.annotations = annotationList;


            List<ModelJson.Photo> photoList_json = new List<ModelJson.Photo>();
            foreach (Photo photo in project.photos)
            {
                ModelJson.Photo photo_json = new ModelJson.Photo();
                photo_json.path = photo.orjname;

                List<ModelJson.Label> labelList_json = new List<ModelJson.Label>();
                // 1) string'i normal label'a çevir
                List<Label> labelList = string.IsNullOrWhiteSpace(photo.labels)? 
                    new List<Label>() : 
                    JsonConvert.DeserializeObject<List<Label>>(photo.labels);
                // 2) label'a label_json'a çevir
                foreach (var label in labelList)
                {
                    ModelJson.Label label_json = new ModelJson.Label();
                    label_json.annoID = label.annoID;


                    List<ModelJson.Point> pointList_json = new List<ModelJson.Point>();
                    foreach (var point in label.points)
                    {
                        pointList_json.Add(new ModelJson.Point() { x = (int)point.x, y = (int)point.y });
                    }

                    label_json.points = pointList_json;

                    labelList_json.Add(label_json);
                }



                photo_json.labels = labelList_json;

                photoList_json.Add(photo_json);
            }

            project_json.photos = photoList_json;

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Culture = System.Globalization.CultureInfo.CurrentCulture;
            string jsonobj = JsonConvert.SerializeObject(project_json, jsonSettings);
            return File(GetByteArray(jsonobj), "text/json", "label.json");
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
