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
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if(context.HttpContext.Session.GetObject<User>("user") == null)
				Yetkisiz(context);
		}

		private void Yetkisiz(ActionExecutingContext context)
		{
			context.Result =
				new RedirectToRouteResult(
				new RouteValueDictionary {
								{ "Controller", "Accounts" },
								{ "Action", "Index" }});
		}
	}
}
