using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WarSpot.Cloud.Storage;
using WarSpot.MatchComputer;
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
				var games = (from b in Warehouse.db.Game
										 where b.AccountAccount_ID == customIdentity.Id
										 select b).ToList<Game>();
				res.AddRange(games.Select(game => new GameModel()
																						{
																							Id = game.Game_ID,
																							AccountID = game.AccountAccount_ID,
																							Replay = game.Replay
																						}));
			}
			return View(res);
		}

		//
		// GET: /Games/Details/<GUID>

		public ActionResult Details(Guid id)
		{
			var res = new GameModel();
			var game = (from g in Warehouse.db.Game
									where g.Game_ID == id
									select g).FirstOrDefault();
			if (game != null)
			{
				res.Id = game.Game_ID;
				res.Replay = game.Replay;
				res.AccountID = game.AccountAccount_ID;
			}
			return View(res);
		}

		// GET: /Games/Download/<GUID>
		public FileResult Download(Guid id)
		{
			return new FileContentResult(Warehouse.GetReplay(id).data, "application/octet-stream")
			{
				// todo get date here
				FileDownloadName = "replay_" + id + ".out"
			};
		}


		//
		// GET: /Games/Create

		public ActionResult Create()
		{
			var m = new NewGameModel();
			foreach (var i in Warehouse.db.Intellect)
			{
				m.Intellects.Add(new KeyValuePair<Guid, string>(i.Intellect_ID, i.Intellect_Name));
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
					// todo что-то нужно решить с названием игры.
					res = Warehouse.BeginMatch(intellects, customIdentity.Id, "title");
				}
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: /Games/Play/<Guid>
		public ActionResult Play(Guid id)
		{
			var list = Deserializator.Deserialize(new MemoryStream(Warehouse.GetReplay(id).data));
			if (list.Count == 0)
			{
				// todo report error
				return RedirectToAction("Details", id);
			}
			return View(list);
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
