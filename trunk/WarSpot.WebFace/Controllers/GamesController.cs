using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WarSpot.WebFace.Controllers
{
	[Authorize(Roles = "User")]
	public class GamesController : AuthorizedController
	{
		//
		// GET: /Games/

		public ActionResult Index()
		{
			return View();
		}

		//
		// GET: /Games/Details/5

		public ActionResult Details(int id)
		{
			return View();
		}

		//
		// GET: /Games/Create

		public ActionResult Create()
		{
			return View();
		}

		//
		// POST: /Games/Create

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
		// GET: /Games/Edit/5

		public ActionResult Edit(int id)
		{
			return View();
		}

		//
		// POST: /Games/Edit/5

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
		// GET: /Games/Delete/5

		public ActionResult Delete(int id)
		{
			return View();
		}

		//
		// POST: /Games/Delete/5

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
