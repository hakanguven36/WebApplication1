using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApplication1.Araclar;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
	public class AccountsController : Controller
	{
		MyContext db;
		private readonly GenelAyarlar ayarlar;

		public AccountsController(MyContext db, IOptions<GenelAyarlar> ayarlar)
        {
			this.db = db;
            this.ayarlar = ayarlar.Value;
        }
		
		public IActionResult Index() => View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index(AccountsViewModel model)
		{
			if (ModelState.IsValid)
			{
				User user = db.User.Where(u => u.username == model.username).FirstOrDefault();
				if (user == null)
				{
					ModelState.AddModelError("", "Kullanıcı adı ve şifrenizi kontrol ediniz");
					return View(model);
				}
				
				if (user.kilitli)
				{
					ModelState.AddModelError("", "Kullanıcı adı ve şifrenizi kontrol ediniz");
					return View(model);
				}
				
				if (user.password != model.password)
				{
					bool kilitli = HataArttır(user);
					if (kilitli)
						ModelState.AddModelError("", "Kullanıcı hesabınız kilitlenmiştir. Yönetici ile görüşünüz.");
					else
						ModelState.AddModelError("", "Kullanıcı adı ve şifrenizi kontrol ediniz");
					return View(model);

				}
				else
				{
					YetkiVer(user);
					return RedirectToAction("Index", "Home");
				}
			}
			return View(model);
		}

		private void YetkiVer(User user)
		{
			user.admin = user.username == ayarlar.CreatorUsername;
			HttpContext.Session.SetObject("user", user);
		}

		private bool HataArttır(User user)
		{
			bool kilitli = false;

			user.hatali++;
			user.kilitli = kilitli = user.hatali > 5 ? true : false;

			db.Update(user);
			db.SaveChanges();
			return kilitli;
		}

		[Yetki]
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Accounts");
		}

		public IActionResult Kurulum()
        {
            try
            {
				User user = db.User.FirstOrDefault(u => u.username == ayarlar.CreatorUsername);
				if (user == null)
					user = new User();
				user.username = ayarlar.CreatorUsername;
				user.password = ayarlar.CreatorPassword;
				db.Update(user);
				db.SaveChanges();
				return Json("Tamam");
			}
            catch (Exception e) 
            {
				return Json(e.Message);
            }
			
        }
	}
}