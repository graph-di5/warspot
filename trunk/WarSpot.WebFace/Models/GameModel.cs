using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WarSpot.WebFace.Models
{
	public class GameModel
	{
		[Display(Name = "ID")]
		public Guid Id { get; set; }

		[Display(Name = "U_ID")]
		public Guid AccountID { get; set; }
		
		[Display(Name = "Путь к записи игры")]
		public string Replay { get; set; }
	}

	public class NewGameModel
	{
		public NewGameModel()
		{
			Intellects = new List<KeyValuePair<Guid, string>>();
		}
		public List<KeyValuePair<Guid, string>> Intellects { get; set; }
	}

	public class GameIntellectsChoice
	{
		//public 
	}
}