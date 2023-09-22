using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
		public IActionResult Index(AccountsViewModel model)
		{
			if (ModelState.IsValid)
			{
				User user = db.User.Where(u => u.username == model.username).FirstOrDefault();
				if (user == null)
				{
					ModelState.AddModelError("", "Kullanıcı adı ve şifrenizi kontrol ediniz");
				}
				else
				{
					if(model.username == ayarlar.CreatorUsername & model.password == ayarlar.CreatorPassword)
					{
						SuperYetkiVer(user);
						return RedirectToAction("Index", "Home");
					}

					if (user.password != model.password)
					{
						HataArttır(user);
						ModelState.AddModelError("", "Kullanıcı adı ve şifrenizi kontrol ediniz");
					}
					else
					{
						YetkiVer(user);
						return RedirectToAction("Index", "Home");
					}
				}
			}
			return View(model);
		}

		private void YetkiVer(User user)
		{
			HttpContext.Session.SetObject("user", user);
		}

		private void SuperYetkiVer(User user)
		{
			YetkiVer(user);
			HttpContext.Session.SetString("admin", "evet");
		}

		private bool HataArttır(User user)
		{
			bool ret = true;
			user.hatali++;
			if (user.hatali > 5)
			{
				user.kilitli = true;
				ret = false;
			}
			db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			db.SaveChanges();
			return ret;
		}

		[Yetki]
		public IActionResult ÇıkışYap()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Accounts");
		}		
	}
}