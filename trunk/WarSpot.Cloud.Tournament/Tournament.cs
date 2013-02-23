using System;
using System.Collections.Generic;

namespace WarSpot.Cloud.Tournament
{
	public enum StageTypes { Duel, Deathmatch, Labyrinth };

	public enum TournamentStatus { Idle, Working, Finished }

	class Tournament
	{
		Guid Id;

		TournamentStatus Status;

		List<Guid> StagesList;

		List<Player> Players;

		public Tournament(Guid id)
		{
			Id = id;
			Status = TournamentStatus.Working;//Объект создаётся в момент начала турнира.
			//Остальное подтягиваем из баз.
		}

	}

	class Stage
	{
		public Guid StageId { private set; get; }

		public StageTypes Type { private set; get; }

		List<Player> Players;

		List<Match> Matches;

		public Stage(StageTypes type)
		{
			StageId = new Guid();
			Type = type;
		}
	}

	class Player//Для хранения данных об игроке в этапе
	{
		public Guid Id { private set; get; }

		public Guid IntellectID { private set; get; }

		public int StageScore { set; get; }//Полученные внутренние очки этапа.

		public int Points { set; get; }//Полученные очки турнира (в этом этапе).

        public Player(Guid id, Guid intellectId)
        {
            Id = id;
            IntellectID = intellectId;
            StageScore = 0;
            Points = 0;
        }
	}

	class Match
	{
		public List<Player> Players { private set;  get; }
		public StageTypes Type { private set; get; }
		public DateTime StartTime { set; get; }
		public bool HasResult { private set; get; }

		Match(List<Player> players, StageTypes type)
		{
			Players = players;
			Type = type;
			StartTime = DateTime.MaxValue;//Вроде зануления.
			HasResult = false;
		}
	}
}
