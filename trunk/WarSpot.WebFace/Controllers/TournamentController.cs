using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WarSpot.Cloud.Storage;
using WarSpot.WebFace.Models;
using Warehouse = WarSpot.Cloud.Storage.Warehouse;
using WarSpot.WebFace.Security;
using Stage = WarSpot.WebFace.Models.Stage;


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
						myTournaments.Add(new Models.Tournament
						                  	{
												Creator = t.Creator_ID.ToString(),
												ID = t.Tournament_ID,
												Description = t.Description,
												MaxPlayers = t.MaxPlayers,
												StartTime = t.StartTime,
												TournamentName = t.Tournament_Name,
												IsIn = true,
												State = (State) t.State_Code
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
							IsIn = false,
							State = (State)tournament.State_Code
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
								IsIn = false,
								State = (State) tournament.State_Code
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
				IsIn = IsIn(id),
				State = (State)tournament.State_Code,
			  Stages = new List<Stage>(
				tournament.Stages.ToArray().Select(s => new Stage
				       	{
				       		StartTime = s.StartTime,
									State = (State) s.State_Code,
									Games = new List<GameModel>(s.Games.Select(g => new GameModel(g)))
				       	}
				))
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
											IsIn = false,
											State = State.NotStarted
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
			    var t2 = Cloud.Storage.Tournament.CreateTournament(
			        t.ID,
			        t.MaxPlayers,
			        t.StartTime,
			        customIdentity.Id,
			        t.TournamentName,
			        0x0,
			        "type",
			        t.Description
			        );
				Warehouse.db.AddToTournament(t2);
				for (int i = 0; i < t.StagesCount; i++)
				{
				    var st = Cloud.Storage.Stage.CreateStage(Guid.NewGuid(), 0x0, "TO DO: // ", t.StartTime);
				    st.Tournament = t2;
					Warehouse.db.AddToStages(st);
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
				IsIn = IsIn(id),
				State = (State) tournament.State_Code
			};
			return View(res);
		}
	}
}
