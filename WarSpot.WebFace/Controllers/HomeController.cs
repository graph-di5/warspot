using System.Web.Mvc;

namespace WarSpot.WebFace.Controllers
{
	[AllowAnonymous]
	public class HomeController : AuthorizedController
	{
		[AllowAnonymous]
		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to WarSpot!";
			return View();
		}

		[AllowAnonymous]
		public ActionResult About()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Details()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Contacts()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult FastStart()
		{
			return View();
		}
	}
}
