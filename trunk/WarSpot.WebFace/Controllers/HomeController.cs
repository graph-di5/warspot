using System.Web.Mvc;

namespace WarSpot.WebFace.Controllers
{
	[AllowAnonymous]
	public class HomeController : AuthorizedController
	{
		[AllowAnonymous]
		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to ASP.NET MVC!";

			return View();
		}

		[AllowAnonymous]
		public ActionResult About()
		{
			return View();
		}
	}
}
