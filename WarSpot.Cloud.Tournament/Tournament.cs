using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSpot.Cloud.Tournament
{
    public enum StageTypes {Duel, Deathmatch, Labyrinth};

    public enum TournamentStatus {Idle, Working, Finished}

    class Tournament
    {
        Guid Id;

        TournamentStatus Status;

        List<Guid> StagesList;

        List<Player> Players;

        public Tournament(Guid id)
        {
            Id = id;
            Status = TournamentStatus.Idle;
            //Остальное подтягиваем из баз.
        }

    }

    class Stage
    {
        Guid StageId { private set; public get; }

        StageTypes Type {private set; public get; }

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
        public Player(Guid id, Guid intellectId)
        {
            Id = id;
            IntellectID = intellectId;
            StageScore = 0;
            Points = 0;
        }

        Guid Id { private set; public get; }

        Guid IntellectID { private set; public get; }

        int StageScore { set; get; }//Полученные внутренние очки этапа.

        int Points { set; get; }//Полученные очки турнира (в этом этапе).
    }

    class Match
    {
        List<Player> Players { private set; public get; }
        StageTypes Type { private set; public get; }
        DateTime StartTime { set; get; }
        bool HasResult { private set; public get; }

        Match(List<Player> players, StageTypes type)
        {
            Players = players;
            Type = type;
            StartTime = DateTime.MaxValue;//Вроде зануления.
            HasResult = false;
        }
    }
}
