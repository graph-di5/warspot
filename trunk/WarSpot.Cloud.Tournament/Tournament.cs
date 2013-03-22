using System;
using System.Collections.Generic;

namespace WarSpot.Cloud.Tournament
{
	public enum SubStageType { Duel, Deathmatch, Labyrinth };

    public enum Status { Idle, Working, Finished };

	class Tournament
	{
		Guid Id { private set; get; };

		Status State { private set; get; };

		List<Guid> StagesList { private set; get; };

		public List<Player> Players;

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

		List<Player> Players;		

		public Stage()
		{
			StageId = new Guid();
	    }
	}

    class SubStage
    {
        public Guid SubStageId { private set; get; }

        public SubStageType Type { private set; get; }

        public Guid StageId { private set; get; }

        public Status State { private set; get; }

        public Guid ScoreId { private set; get; }

        private List<Game> Games; //Снаружи об этом знать не нужно
    }

    class Game
	{
		public List<Player> Players { private set;  get; }
		public SubStageType Type { private set; get; }
		public DateTime StartTime { set; get; }//Чтобы удалять зависшие
		public bool HasResult { private set; get; }

		Game(List<Player> players, SubStageType type)
		{
			Players = players;
			Type = type;
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
        int Points { private set; get; }
        Guid PlayerId { private set; get; }
        Guid SubStageId { private set; get; }

        Score(int points, Guid playerId, Guid subStageId)
        {
            Points = points;
            PlayerId = playerId;
            SubStageId = subStageId;
        }
    }
}
