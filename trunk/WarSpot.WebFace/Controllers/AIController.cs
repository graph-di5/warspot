using System.IO;
using System.Web;
using System.Web.Mvc;
using WarSpot.Cloud.Storage;
using WarSpot.WebFace.Security;


namespace WarSpot.WebFace.Controllers
{
	public class AIController : AuthorizedController
	{
		//
		// GET: /AI/
		//[Authorize(Roles = "User")]
		//[Authorize]
		public ActionResult Index()
		{
			// HttpContext.Current.User
			string[] names = new string[] {};
			var customIdentity = User.Identity as CustomIdentity;
			if (customIdentity != null)
			{
				names = Warehouse.GetListOfIntellects(customIdentity.Id);
				if(customIdentity.IsInRole("Developer"))
				{
					string[] ids = new string[]{};
					(from i in Warehouse.db.Intellect
					 where i.AccountAccount_ID == customIdentity.Id
					 select i).ToString();
					//Warehouse.Db.Intellect.Where(i => i.AccountAccount_ID == customIdentity.Id);
				}
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

#if false
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
#endif
	}
}
