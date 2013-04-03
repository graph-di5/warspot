using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WarSpot.Cloud.Storage;
using WarSpot.WebFace.Models;

namespace WarSpot.WebFace.Controllers
{
	[Authorize(Roles = "MetaAdmin")]
	public class UsersController : AuthorizedController
	{
		//
		// GET: /Users/

		public ActionResult Index()
		{
			return View(Warehouse.db.Account.ToArray().Select(account => new ViewAccountModel()
				{
					Id = account.Account_ID.ToString(), 
					UserName = account.Account_Name, 
					Roles = (from RoleType role in Enum.GetValues(typeof (RoleType))
			              select new AccountRole
			                      {
			                        RoleType = role, Is = Warehouse.IsUser(account.Account_ID, role), Until = Warehouse.UserRoleValidUntil(account.Account_ID, role),
			                      }).ToList()
				}).ToList());
		}

		//
		// GET: /Users/Edit/<GUID>

		public ActionResult Edit(string id)
		{
			// todo make view looking good
			var guid = Guid.Parse(id);
			var account = (from a in Warehouse.db.Account
										 where a.Account_ID == guid
										 select a).FirstOrDefault();
			// todo optimise this to minimize db actions
			if (account != null)
			{
				return View(new ViewAccountModel()
					{
						Id = account.Account_ID.ToString(),
						UserName = account.Account_Name,
						Roles = (from RoleType role in Enum.GetValues(typeof(RoleType))
										 select new AccountRole
										 {
											 RoleType = role,
											 Is = Warehouse.IsUser(account.Account_ID, role),
											 Until = Warehouse.UserRoleValidUntil(account.Account_ID, role),
										 }).ToList()
					});
			}
			return View("Index");
		}

		//
		// POST: /Users/Edit/<GUID>

		[HttpPost]
		public ActionResult Edit(string id, ViewAccountModel model)
		{
			try
			{
				var guid = Guid.Parse(id);
				var account = (from a in Warehouse.db.Account
											 where a.Account_ID == guid
											 select a).FirstOrDefault();
				if (account != null)
				{
					account.Account_Name = model.UserName;
					foreach (var accountRole in model.Roles)
					{
						if(accountRole.Is)
						{
							Warehouse.SetUserRole(accountRole.RoleType, guid, accountRole.Until);
						}
					}
					Warehouse.db.SaveChanges();

					return RedirectToAction("Index");
				}
				return RedirectToAction("Edit", new { model.Id });

			}
			catch
			{
				return View();
			}
		}

#if false
		//
		// GET: /Users/Delete/<GUID>

		public ActionResult Delete(string id)
		{
			return View();
		}

		//
		// POST: /Users/Delete/<GUID>

		[HttpPost]
		public ActionResult Delete(string id, FormCollection collection)
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
