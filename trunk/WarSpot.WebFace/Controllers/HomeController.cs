using System.Web.Mvc;

namespace WarSpot.WebFace.Controllers
{
	public class HomeController : AuthorizedController
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to ASP.NET MVC!";

			return View();
		}

		public ActionResult About()
		{
			return View();
		}
	}
}
