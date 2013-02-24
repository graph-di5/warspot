using System;
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
}