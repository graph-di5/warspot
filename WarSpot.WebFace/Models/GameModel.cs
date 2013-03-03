using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WarSpot.Cloud.Storage;

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
		public string CreationTime { get; set; }

		[Display(Name = "Список интеллектов")]
		public List<string> Intellects;

		public GameModel(Game game)
		{
			Id = game.Game_ID;
			AccountID = game.AccountAccount_ID;
			Replay = game.Replay;
			CreationTime = game.CreationTime;
			Name = game.Game_Name;
			Intellects = new List<string>();
			foreach (var intellect in game.Intellects)
			{
				Intellects.Add(intellect.Intellect_Name);
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