using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web.Mvc;
using WarSpot.Cloud.Storage;
using WarSpot.WebFace.Models;
using WarSpot.WebFace.Security;

namespace WarSpot.WebFace.Controllers
{
	[Authorize(Roles = "User")]
	public class GamesController : AuthorizedController
	{
		//
		// GET: /Games/

		public ActionResult Index()
		{
			var res = new List<GameModel>();
			var customIdentity = User.Identity as CustomIdentity;
			if (customIdentity != null)
			{
				Warehouse.db.Refresh(RefreshMode.StoreWins, Warehouse.db.Game);
				var games = (from b in Warehouse.db.Game
										 where b.Creator_ID == customIdentity.Id
										 select b).ToList<Game>();
				res.AddRange(games.Select(game => new GameModel(game)));
			}
			return View(res);
		}

		//
		// GET: /Games/Details/<GUID>

		public ActionResult Details(Guid id)
		{
			var game = (from g in Warehouse.db.Game
									where g.Game_ID == id
									select g).FirstOrDefault();
			if (game != null)
			{
				return View(new GameModel(game));
			}
			return View("Index");
		}

		// GET: /Games/Download/<GUID>
		public FileResult Download(Guid id)
		{
			var game = Warehouse.db.Game.First(g => g.Game_ID == id);
			return new FileContentResult(Warehouse.GetReplayAsMemoryStream(id).ToArray(), "application/octet-stream")
			{
				// todo get write name here
				FileDownloadName = "replay_" + game.Game_Name + ".out"
			};
		}


		//
		// GET: /Games/Create

		public ActionResult Create()
		{
			var m = new NewGameModel();
			foreach (var i in Warehouse.db.Intellect)
			{
				m.Intellects.Add(new KeyValuePair<Guid, string>(i.Intellect_ID,
					String.Format("{0}: {1}", i.Account.Account_Name, i.Intellect_Name)));
			}
			return View(m);
		}

		//
		// POST: /Games/Create

		[HttpPost]
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(collection["Name"]))
				{
					collection["Name"] = "New game";
				}
				// TODO: rewrite this to right IDs of the form filed
				var intellects = new List<Guid>
				                 	{
				                 		Guid.Parse(collection["intellect01"]), 
														Guid.Parse(collection["intellect02"])
				                 	};
				var customIdentity = User.Identity as CustomIdentity;
				Guid? res = null;
				if (customIdentity != null)
				{
					res = Warehouse.BeginMatch(intellects, customIdentity.Id, collection["Name"]);
				}
				return RedirectToAction("Index");
			}
			catch(Exception e)
			{
				// todo report error
				return View("Create");
			}
		}

		// GET: /Games/Play/<Guid>
		public ActionResult Play(Guid id)
		{
			var replay = Warehouse.GetReplay(id);
			if (replay == null)
			{
				// todo report error
				return RedirectToAction("Details", id);
			}
			var actualReplay = replay.Data.Events;
            if (actualReplay.Count == 0)
			{
				// todo report error
				return RedirectToAction("Details", id);
			}
            return View(actualReplay);
		}

#if false
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
#endif
	}
}
