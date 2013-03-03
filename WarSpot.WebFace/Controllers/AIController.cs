using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using WarSpot.Cloud.Storage;
using WarSpot.WebFace.Models;
using WarSpot.WebFace.Security;

namespace WarSpot.WebFace.Controllers
{
	[Authorize(Roles = "User")]
	public class AIController : AuthorizedController
	{
		//
		// GET: /AI/
		public ActionResult Index()
		{
			List<KeyValuePair<Guid, string>> names = new List<KeyValuePair<Guid, string>> {};
			var customIdentity = User.Identity as CustomIdentity;
			if (customIdentity != null)
			{
				names = Warehouse.GetListOfIntellects(customIdentity.Id);
			}

			return View(names);
		}

		public ActionResult UploadAI()
		{
			return View();
		}

		[HttpPost]
		public ActionResult UploadAI(HttpPostedFileBase file)
		{
			// todo 
			var customIdentity = User.Identity as CustomIdentity;
			if (customIdentity == null)
			{
				return View();
			}
			MemoryStream target = new MemoryStream();
			file.InputStream.CopyTo(target);
			Warehouse.UploadIntellect(customIdentity.Id, file.FileName, target.ToArray());
			return RedirectToAction("Index");
		}

		//
		// GET: /AI/Details/<GUID>

		public ActionResult Details(Guid id)
		{
			var ii = Warehouse.db.Intellect.First(i => i.Intellect_ID == id);
			var games = ii.Games;
			var res = games.ToArray().Select(game => new GameModel(game)).ToList();
			return View(res);
		}

		//
		// GET: /AI/Create

#if false
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
#endif
		// todo enable deletion of intellects
#if false
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
#endif
	}
}
