using System.Web.Mvc;

namespace WarSpot.Client.WebFace.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Hi, this is WarSpot prpoject.";

			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "About.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Contacts@blabla.fuu";

			return View();
		}
	}
}
