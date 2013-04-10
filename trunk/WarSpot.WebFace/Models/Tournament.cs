using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WarSpot.WebFace.Models
{
	public class Tournament
	{
		public Guid ID { get; set; }
		public string Creator { get; set; }
		[Required]
		[Display(Name = "Название турнира")]
		public string TournamentName { get; set; }
		[Required]
		[Display(Name = "Описание турнира")]
		public string Description { get; set; }
		[Required]
		[Display(Name = "Время старта")]
		[DataType(DataType.DateTime)]
		public DateTime StartTime { get; set; }
		[Required]
		[Display(Name = "Максимальное количество участников")]
		public long MaxPlayers { get; set; }
		public List<string> Players { get; set; }
		// todo 
		//public string State;
	}
}