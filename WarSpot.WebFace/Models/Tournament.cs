﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WarSpot.Cloud.Tournament;

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
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }
		
		[Required]
		[Display(Name = "Время старта")]
		[DataType(DataType.DateTime)]
		public DateTime StartTime { get; set; }

		[Display(Name = "Участник")]
		//[DataType(DataType.Custom)]
		public bool IsIn { get; set; }
		
		[Required]
		[Display(Name = "Максимальное количество участников")]
		public long MaxPlayers { get; set; }

		[Required]
		[Display(Name = "Количество этапов")]
		public long StagesCount { get; set; }

		[Display(Name = "Участники")]
		public List<string> Players { get; set; }

		[Display(Name = "Статус")]
		public State State { get; set; }

		// todo stages
		public List<Stage> Stages { get; set; }
	}

	public class Stage
	{
		[Display(Name = "Статус")]
		public State State { get; set; }
	
		[Required]
		[Display(Name = "Время старта")]
		[DataType(DataType.DateTime)]
		public DateTime StartTime { get; set; }

	}
}