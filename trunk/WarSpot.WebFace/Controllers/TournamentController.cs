using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WarSpot.WebFace.Models;
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
				var p = Warehouse.db.Account.First(a => a.Account_ID == customIdentity.Id);
				var myTournaments = new List<Models.Tournament>();
				foreach (var t in p.TournamentPlayer)
				{
					myTournaments.Add(new Models.Tournament()
					        	{
											// todo
					        		Creator = t.Creator_ID.ToString(),
											ID = t.Tournament_ID,
											Description = t.Description,
											MaxPlayers = t.MaxPlayers,
											StartTime = t.StartTime,
											TournamentName = t.Tournament_Name
					        	});
				}
				ViewData["my"] = myTournaments;

				// todo filter public 
				var openTournaments = new List<Models.Tournament>();
				foreach (var tournament in Warehouse.db.Tournament.ToArray().Where(
					t => myTournaments.All(mt => mt.ID != t.Tournament_ID)))
				{
					openTournaments.Add(new Tournament()
					{
						// todo
						Creator = tournament.Creator_ID.ToString(),
						ID = tournament.Tournament_ID,
						Description = tournament.Description,
						MaxPlayers = tournament.MaxPlayers,
						StartTime = tournament.StartTime,
						TournamentName = tournament.Tournament_Name
					});
					
				}
				ViewData["open"] = openTournaments;
			}
			return View();
		}


		public ActionResult View(Guid id)
		{
			var tournament = Warehouse.db.Tournament.First(t => t.Tournament_ID == id);
			var res = new Tournament()
			{
				// todo
				Creator = tournament.Creator_ID.ToString(),
				ID = tournament.Tournament_ID,
				Description = tournament.Description,
				MaxPlayers = tournament.MaxPlayers,
				StartTime = tournament.StartTime,
				TournamentName = tournament.Tournament_Name,
				Players = new List<string>()
			};
			return View(res);
		}

		[Authorize(Roles = "TournamentsAdmin")]
		public ActionResult Create()
		{
			return View(new Models.Tournament()
			            	{
			            		ID = Guid.NewGuid()
			            	});
		}

		[HttpPost]
		[Authorize(Roles = "TournamentsAdmin")]
		public ActionResult Create(Models.Tournament t)
		{
			var customIdentity = User.Identity as CustomIdentity;
			if (customIdentity != null && t.ID != Guid.Empty)
			{
				Warehouse.db.AddToTournament(Cloud.Storage.Tournament.CreateTournament(
					t.ID,
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
			return Redirect("Index");
		}

	}


}
