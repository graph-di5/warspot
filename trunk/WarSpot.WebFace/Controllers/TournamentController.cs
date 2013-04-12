using System;
using System.Collections.Generic;
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

				if (customIdentity.IsInRole("User"))
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
												TournamentName = t.Tournament_Name,
												IsIn = true
											});
					}
					ViewData["my"] = myTournaments;

					// todo filter public 
					var openTournaments = new List<Models.Tournament>();
					foreach (var tournament in Warehouse.db.Tournament.ToArray().Where(
						t => myTournaments.All(mt => mt.ID != t.Tournament_ID)))
					{
						openTournaments.Add(new Models.Tournament()
						{
							// todo
							Creator = tournament.Creator_ID.ToString(),
							ID = tournament.Tournament_ID,
							Description = tournament.Description,
							MaxPlayers = tournament.MaxPlayers,
							StartTime = tournament.StartTime,
							TournamentName = tournament.Tournament_Name,
							IsIn = false
						});

					}
					ViewData["open"] = openTournaments;
				}
				if (customIdentity.IsInRole("TournamentsAdmin"))
				{
					var openTournaments = Warehouse.db.Tournament.ToArray().Select(
						tournament => new Models.Tournament()
							{
								// todo
								Creator = tournament.Creator_ID.ToString(),
								ID = tournament.Tournament_ID,
								Description = tournament.Description,
								MaxPlayers = tournament.MaxPlayers,
								StartTime = tournament.StartTime,
								TournamentName = tournament.Tournament_Name,
								IsIn = false
							}).ToList();
					ViewData["edit"] = openTournaments;
				}
			}
			return View();
		}


		public ActionResult View(Guid id)
		{
			var customIdentity = User.Identity as CustomIdentity;
			var tournament = Warehouse.db.Tournament.First(t => t.Tournament_ID == id);
			var res = new Models.Tournament()
			{
				// todo
				Creator = tournament.Creator_ID.ToString(),
				ID = tournament.Tournament_ID,
				Description = tournament.Description,
				MaxPlayers = tournament.MaxPlayers,
				StartTime = tournament.StartTime,
				TournamentName = tournament.Tournament_Name,
				Players = new List<string>(from p in tournament.Player select p.Account_Name),
				IsIn = IsIn(id)
			};

			List<KeyValuePair<Guid, string>> intellects = Warehouse.db.Intellect.Where(ii => ii.AccountAccount_ID == customIdentity.Id).ToArray().Select(i => new KeyValuePair<Guid, string>(i.Intellect_ID, String.Format("{0}", i.Intellect_Name))).ToList();

			ViewData["intellects"] = intellects;
			return View(res);
		}

		[Authorize(Roles = "TournamentsAdmin")]
		public ActionResult Create()
		{
			return View(new Models.Tournament()
										{
											ID = Guid.NewGuid(),
											MaxPlayers = 128,
											StagesCount = 2,
											StartTime = DateTime.UtcNow.AddDays(1),
											IsIn = false
										});
		}

		[HttpPost]
		[Authorize(Roles = "TournamentsAdmin")]
		public ActionResult Create(Models.Tournament t)
		{
			var customIdentity = User.Identity as CustomIdentity;
			if (customIdentity != null && t.ID != Guid.Empty)
			{
				// todo use Warehouse.CreateTournament()
				Warehouse.db.AddToTournament(Cloud.Storage.Tournament.CreateTournament(
					t.ID,
					t.MaxPlayers,
					t.StartTime,
					customIdentity.Id,
					t.TournamentName,
					0x0,
					"type",
					t.Description
					));
				for (int i = 0; i < t.StagesCount; i++)
				{
					Warehouse.AddStage(t.ID, t.StartTime);
					//Warehouse.db.AddToStages(Cloud.Storage.Stage.CreateStage(Guid.NewGuid(), 0x0, "TO DO: // ", t.StartTime));
					// Было:
					/*Guid.NewGuid(),
"Waiting",
"type",
t.StartTime,
t.ID));*/
				}
				Warehouse.db.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		private bool IsIn(Guid id)
		{
			var tournament = Warehouse.db.Tournament.First(t => t.Tournament_ID == id);
			var customIdentity = User.Identity as CustomIdentity;
			return customIdentity != null && tournament.Player.Any(p => p.Account_ID == customIdentity.Id);
		}

		public ActionResult Join(Guid id)
		{
			var customIdentity = User.Identity as CustomIdentity;
			if (customIdentity != null)
			{
				if (IsIn(id))
				{
					Warehouse.LeaveTournament(id, customIdentity.Id);
				}
				else
				{
					Warehouse.JoinTournament(id, customIdentity.Id);
				}
			}

			Warehouse.db.SaveChanges();
			return RedirectToAction("View", new { id });
		}

		public ActionResult UpdateAI(FormCollection collection)
		{
			var customIdentity = User.Identity as CustomIdentity;
			var id = Guid.Parse(collection["tournamentId"]);
			if (customIdentity != null)
			{
				foreach (var stage in Warehouse.db.Tournament.First(t => t.Tournament_ID == id).Stages)
				{
					var aiID = Guid.Parse(collection["intellect01"]);
					var ai = stage.Intellects.FirstOrDefault(i => i.AccountAccount_ID == customIdentity.Id);
					if(ai != null && stage.Intellects.Contains(ai))
					{
						stage.Intellects.Remove(ai);
					}
					ai = Warehouse.db.Intellect.First(i => i.Intellect_ID == aiID);
					stage.Intellects.Add(ai);
				}
				Warehouse.db.SaveChanges();
			}
			return RedirectToAction("View", new { id });
		}

		[Authorize(Roles = "TournamentsAdmin")]
		public ActionResult Edit(Guid id)
		{
			var tournament = Warehouse.db.Tournament.First(t => t.Tournament_ID == id);
			var res = new Models.Tournament()
			{
				// todo
				Creator = tournament.Creator_ID.ToString(),
				ID = tournament.Tournament_ID,
				Description = tournament.Description,
				MaxPlayers = tournament.MaxPlayers,
				StartTime = tournament.StartTime,
				TournamentName = tournament.Tournament_Name,
				Players = new List<string>(from p in tournament.Player select p.Account_Name),
				IsIn = IsIn(id)
			};
			return View(res);
		}
	}
}
