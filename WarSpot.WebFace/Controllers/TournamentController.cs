using System;
using System.Linq;
using System.Web.Mvc;
using Warehouse = WarSpot.Cloud.Storage.Warehouse;
using WarSpot.WebFace.Security;


namespace WarSpot.WebFace.Controllers
{
	public class TournamentController : AuthorizedController
	{
		//
		// GET: /Tournament/

		public ActionResult Index()
		{
			var customIdentity = User.Identity as CustomIdentity;
			if (customIdentity != null)
			{
				ViewData["my"] =
					(from t in Warehouse.db.Tournament
					 where t.Player.Contains(Warehouse.db.Account.First(a => a.Account_ID == customIdentity.Id))
					 select t);
			}
			return View();
		}


		public ActionResult View(Guid id)
		{
			return View();
		}

		[Authorize(Roles = "TournamentsAdmin")]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "TournamentsAdmin")]
		public ActionResult Create(Models.Tournament t)
		{
			var customIdentity = User.Identity as CustomIdentity;
			if (customIdentity != null)
			{
				Warehouse.db.AddToTournament(Cloud.Storage.Tournament.CreateTournament(
					Guid.NewGuid(),
					t.MaxPlayers,
					t.StartTime,
					customIdentity.Id,
					t.TournamentName,
					"Waiting",
					"type",
					t.Description
					));
				Warehouse.db.SaveChanges();
			}
			return View("Index");
		}

	}


}
