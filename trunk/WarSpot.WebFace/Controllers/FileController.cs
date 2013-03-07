using System;
using System.Web.Mvc;
using WarSpot.Cloud.Storage;

namespace WarSpot.WebFace.Controllers
{
	public class FileController : Controller
	{
		//
		// GET: /File/

		public ActionResult Index()
		{
			// check user role here, if admin show all files with duplicates
			return View(Warehouse.GetFileList());
		}

		//
		// GET: /File/Download/<GUID>
		public FileResult Download(Guid id)
		{
			// todo check error
			var file = Warehouse.DownloadFile(id);
			return new FileContentResult(file.Data, "application/octet-stream")
			{
				FileDownloadName = file.Name
			};
		}

		//
		// GET: /File/Details/<GUID>

		public ActionResult Details(Guid id)
		{
			return View();
		}

		//
		// GET: /File/Create
		[Authorize(Roles = "Admin")]
		public ActionResult Create()
		{
			return View();
		}

		//
		// POST: /File/Create

		[HttpPost]
		[Authorize(Roles = "Admin")]
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
		// GET: /File/Edit/5

		[Authorize(Roles = "Admin")]
		public ActionResult Edit(int id)
		{
			return View();
		}

		//
		// POST: /File/Edit/5

		[HttpPost]
		[Authorize(Roles = "Admin")]
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
		// GET: /File/Delete/5

		[Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
		{
			return View();
		}

		//
		// POST: /File/Delete/5

		[HttpPost]
		[Authorize(Roles = "Admin")]
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
