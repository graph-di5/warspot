using System;
using System.Web.Mvc;
using WarSpot.Cloud.Storage;
using WarSpot.WebFace.Security;

namespace WarSpot.WebFace.Controllers
{
	public class ErrorController : AuthorizedController
	{
		[AllowAnonymous]
		public ActionResult Index(int statusCode, Exception error)
		{
			var customIdentity = User.Identity as CustomIdentity;
			if (customIdentity != null)
			{
				if (customIdentity.IsInRole("Developer"))//Warehouse.IsUser(customIdentity.Id, RoleType.Developer))
				{
					ViewData.Model = error;
				}
			}

			Response.StatusCode = statusCode;
			Response.TrySkipIisCustomErrors = true;
			ViewData.Add("StatusCode", statusCode);
			return View();
		}
	}
}
