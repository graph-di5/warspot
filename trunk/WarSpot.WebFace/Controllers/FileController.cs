using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WarSpot.Cloud.Storage;

namespace WarSpot.WebFace.Controllers
{
	public class FileController : AuthorizedController
	{
		//
		// GET: /File/

		[AllowAnonymous]
		public ActionResult Index()
		{
			// check user role here, if admin show all files with duplicates
			return View(Warehouse.GetListOfFiles());
		}

		//
		// GET: /File/Download/<GUID>
		[AllowAnonymous]
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

		[AllowAnonymous]
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
		public ActionResult Create(string name, string description,
			string longDescription, HttpPostedFileBase file)
		{
			try
			{
				var target = new MemoryStream();
				file.InputStream.CopyTo(target);
				// TODO: check res
				var res = Warehouse.UploadFile(name, description,
				                               longDescription, target.ToArray());
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
