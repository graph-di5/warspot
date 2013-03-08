using System.IO;
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
			//DirectoryInfo

			return View();
		}

		[AllowAnonymous]
		public ActionResult About()
		{
			return View();
		}
	}
}
