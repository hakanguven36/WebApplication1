using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using WebApplication1.Araclar;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Yetki]
    public class HomeController : Controller
    {
        private readonly MyContext db;
        string fileRoot = "";

        public HomeController(MyContext db, IWebHostEnvironment environment)
        {
            fileRoot = Path.Combine(environment.WebRootPath, "dosyalar");
            this.db = db;
        }

        public IActionResult Index(int? id, int? seenorwhat)
        {
            string hata = "";
            int skipint = id ?? 1;
            skipint--;
            int filesCount = db.HamResim.Count();
            if (filesCount == 0)
            {
                hata += "Veritabanında resim yok! ";
                ViewBag.hata = hata;
                return View();
            }
            if(skipint+1 > filesCount)
            {
                hata += "öyle bir resim yok! ";
                ViewBag.hata = hata;
                return View();
            }

            if (seenorwhat != null)
            {
                HttpContext.Session.SetInt32("seenorwhat", seenorwhat??0);
                skipint = 0;
            }

            int sow = HttpContext.Session.GetInt32("seenorwhat")??0;
            HamResim resim = new HamResim();
            switch (sow)
            {
                case 0:
                    filesCount = db.HamResim.Where(u => u.seenOrWhat == SEENORWHAT.undone).Count();
                    resim = db.HamResim.Where(u => u.seenOrWhat == SEENORWHAT.undone).Skip(skipint).FirstOrDefault();
                    break;
                case 1:
                    filesCount = db.HamResim.Where(u => u.seenOrWhat == SEENORWHAT.done).Count();
                    resim = db.HamResim.Where(u => u.seenOrWhat == SEENORWHAT.done).Skip(skipint).FirstOrDefault();
                    break;
                default:
                    resim = db.HamResim.Skip(skipint).FirstOrDefault();
                    break;
            }


            ViewBag.imageIndex = skipint + 1;
            ViewBag.filesCount = filesCount;
            ViewBag.seenorwhat = HttpContext.Session.GetInt32("seenorwhat")??0;

            if (resim == null)
            {
                hata += "öyle bir resim yok! ";
                ViewBag.hata = hata;
                return View();
            }
            else
            {
                ViewBag.path = Path.Combine("/dosyalar", resim.sysname);
                ViewBag.hamResimID = resim.ID;
            }
            return View();
        }

        public IActionResult GetEtiketler(int hamResimID)
        {
            List<SecimlerViewModel> secimler = new List<SecimlerViewModel>();
            foreach (var item in db.Etiket.Where(u => u.HamResimID == hamResimID).ToList())
            {
                secimler.Add(new SecimlerViewModel() { choice = item.choice, cursorCol = item.cursorCol, cursorRow = item.cursorRow, cursorSize = item.cursorSize });
            } 
            return Json(secimler);
        }

        [HttpPost]
        public IActionResult Tamamlanan(List<SecimlerViewModel> secimler, int hamResimID)
        {
            try
            {
                HamResim resim = db.HamResim.FirstOrDefault(u => u.ID == hamResimID);

                List<Etiket> willDelEtiketList = db.Etiket.Where(u => u.HamResimID == hamResimID).ToList();
                if (willDelEtiketList.Count() > 0)
                {
                    db.RemoveRange(willDelEtiketList);
                    db.SaveChanges();
                }

                if (secimler.Count > 0)
                {
                    resim.seenOrWhat = SEENORWHAT.done;

                    foreach (var item in secimler)
                    {
                        Etiket etiket = new Etiket();
                        etiket.choice = item.choice;
                        etiket.cursorCol = item.cursorCol;
                        etiket.cursorRow = item.cursorRow;
                        etiket.HamResim = resim;
                        etiket.cursorSize = item.cursorSize;
                        db.Add(etiket);
                        db.SaveChanges();
                    }
                }
                else
                {
                    resim.seenOrWhat = SEENORWHAT.undone;                    
                }
                db.Update(resim);
                db.SaveChanges();
                return Json("tamam");
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
