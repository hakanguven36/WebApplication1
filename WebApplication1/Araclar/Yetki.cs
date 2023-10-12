using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Araclar
{
	public class YetkiAttribute : ActionFilterAttribute
	{
		public string admin { get; set; }
		public YetkiAttribute() => admin = "";
		public YetkiAttribute(string admin) => this.admin = admin;

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			User user = context.HttpContext.Session.GetObject<User>("user");
			if (user == null)
            {
				Yetkisiz(context);
				return;
            }
			if(admin == "admin")
				if(user.admin == false)
                {
					Adminsiz(context);
					return;
                }

		}

		private void Yetkisiz(ActionExecutingContext context) =>
			context.Result =
				new RedirectToRouteResult(
				new RouteValueDictionary {
								{ "Controller", "Accounts" },
								{ "Action", "Index" }});
		

		private void Adminsiz(ActionExecutingContext context) =>
			context.Result =
				new RedirectToRouteResult(
				new RouteValueDictionary {
								{ "Controller", "Accounts" },
								{ "Action", "Index" }});
		
	}
}
