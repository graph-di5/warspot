using System.Web.Mvc;

namespace WarSpot.WebFace.Controllers
{
	public class AIController : Controller
	{
		//
		// GET: /AI/

		public ActionResult Index()
		{
			return View();
		}

		//
		// GET: /AI/Details/5

		public ActionResult Details(int id)
		{
			return View();
		}

		//
		// GET: /AI/Create

		public ActionResult Create()
		{
			return View();
		}

		//
		// POST: /AI/Create

		[HttpPost]
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				// TODO: Add insert logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		//
		// GET: /AI/Edit/5

		public ActionResult Edit(int id)
		{
			return View();
		}

		//
		// POST: /AI/Edit/5

		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add update logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		//
		// GET: /AI/Delete/5

		public ActionResult Delete(int id)
		{
			return View();
		}

		//
		// POST: /AI/Delete/5

		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}
