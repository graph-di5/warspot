﻿using System;
using System.Collections.Generic;

namespace WarSpot.Cloud.Tournament
{
	public enum Status { Idle, Working, Finished };

	class Tournament
	{
		public Guid Id { private set; get; }

		public Status State { private set; get; }

		public List<Guid> StagesList { private set; get; }

		public List<Guid> PlayersIdList;
		
		public List<Score> Scores { private set; get; }

		public Tournament(Guid id)
		{
			Id = id;
			State = Status.Idle;
		}
	}

	class Stage
	{
		public Guid StageId { private set; get; }

		public Guid TournamentId { private set; get; }

		public List<Score> Scores { private set; get; }

		public List<Player> Players;

		public Stage(Guid tournamentId)
		{
			StageId = new Guid();
			TournamentId = tournamentId;
		}

		public Stage(Guid id)
		{
			StageId = id;
		}
	}

	class Game//Собственно, один матч
	{
		public Guid Id { private set; get; }
		public List<Player> Players { private set; get; }
		public DateTime StartTime { set; get; }//Чтобы удалять зависшие
		public bool HasResult { set; get; }

		public Game(Guid id, List<Player> players)
		{
			Id = Id;
			Players = players;
			StartTime = DateTime.MaxValue;//Вроде зануления.
			HasResult = false;
		}
	}

	class Player//Для хранения данных об игроке в этапе
	{
		public Guid Id { private set; get; }

		public Guid IntellectID { private set; get; }

		public Player(Guid id, Guid intellectId)
		{
			Id = id;
			IntellectID = intellectId;
		}
	}
	
	class Score
	{
		public int Points { private set; get; }
		public Guid PlayerId { private set; get; }
		public Guid SubStageId { private set; get; }

		Score(int points, Guid playerId, Guid subStageId)
		{
			Points = points;
			PlayerId = playerId;
			SubStageId = subStageId;
		}
	}
}
