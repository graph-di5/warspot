using System.Web.Mvc;

namespace WarSpot.WebFace.Controllers
{
	[AllowAnonymous]
	public class DocumentController : AuthorizedController
	{
		//
		// GET: /Document/

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult EULA()
		{
			return View();
		}

		public ActionResult SmallGuide()
		{
			return View();
		}

		public ActionResult Rules()
		{
			return View();
		}

		public ActionResult WorldDescription()
		{
			return View();
		}

		public ActionResult GameActionsFullDescription()
		{
			return View();
		}

		public ActionResult Tournaments()
		{
			return View();
		}
	}
}
