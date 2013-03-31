using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WarSpot.Cloud.Storage;
using WarSpot.Common;

namespace WarSpot.WebFace.Models
{
	public class GameModel
	{
		[Display(Name = "Имя игры")]
		public string Name { get; set; }

		[Display(Name = "ID")]
		public Guid Id { get; set; }

		[Display(Name = "U_ID")]
		public Guid AccountID { get; set; }

		[Display(Name = "Путь к записи игры")]
		public string Replay { get; set; }

		[Display(Name = "Время старта игры")]
		public DateTime CreationTime { get; set; }

		[Display(Name = "Список комманд")]
		public List<TeamTextInfo> Teams;

		[Display(Name = "Команда победитель")]
		public Guid Winner { get; set; }

		[Display(Name = "Число ходов")]
		public int Steps {get; set; }

		public GameModel(Game game)
		{
			Id = game.Game_ID;
			AccountID = game.Creator_ID;
			Replay = game.Replay;
			CreationTime = game.CreationTime;
			Name = game.Game_Name;
			Teams = new List<TeamTextInfo>();
			if (game.GameDetail == null)
			{
				game.GameDetail = Warehouse.db.GameDetails.FirstOrDefault(gd => gd.Game.Game_ID == game.Game_ID);
			}
			if (game.GameDetail != null)
			{
				Winner = game.GameDetail.Winner_ID;
				Steps = game.GameDetail.StepsCount;
			}
			foreach (var team in game.Teams)
			{
				Teams.Add(new TeamTextInfo()
				          	{
				          		TeamId = team.Team_ID,
											IsWinner = team.Team_ID == Winner,
											Members = team.Intellects.ToList().Select(intellect
												=>
												string.Format("{0}: {1}",
												intellect.Account.Account_Name,
												intellect.Intellect_Name)).ToList()
					          	});
			}
		}

	}

	public class NewGameModel
	{
		public NewGameModel()
		{
			Intellects = new List<KeyValuePair<Guid, string>>();
		}

		public List<KeyValuePair<Guid, string>> Intellects { get; set; }

		[Display(Name = "Имя игры")]
		public string Name { get; set; }

	}

	public class GameIntellectsChoice
	{
		//public 
	}
}