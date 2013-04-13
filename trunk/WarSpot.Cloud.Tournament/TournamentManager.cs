using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarSpot.Cloud.Storage;
using WarSpot.Cloud.Tournament;
using System.Threading;

namespace WarSpot.Cloud.Tournament
{
	class TournamentManager
	{
		/*1.(Большой цикл)
		 *	Проверка, есть ли запущенные турниры
		 *		0 Засыпает
		 *		1 Берёт все запущенные турниры, проходит по каждому:
		 *			2.(Малый цикл)			
		 *				Проверка, есть ли список матчей
		 *					0 Создаёт, уходит в малый цикл. (матчи запускаются при создании)
		 *					1 Проверка, все ли матчи посчитаны
		 *						0 Уходит в малый цикл
		 *						1 Считает очки, определяет победителей, формирует отчётность, завершает турнир.
		 */

		private static TournamentManager _tournamentMeneger;
		private ManualResetEvent _runingEvent;//Событие запуска работы этого менеджера
		private Thread _thread;

		private TournamentManager()
		{
			_runingEvent = new ManualResetEvent(false);
			_thread = new Thread(new ThreadStart(ThreadFunction));
			_thread.Start();
		}

		public static TournamentManager GetInstance()
		{
			if (_tournamentMeneger == null)
			{
				lock (typeof(TournamentManager))
				{
					if (_tournamentMeneger == null)
					{
						_tournamentMeneger = new TournamentManager();
					}
				}
			}

			return _tournamentMeneger;
		}

		public void Stop()
		{
			_runingEvent.Set();//Останавливает работу менеджера
		}

		private void ThreadFunction()
		{
			int _timeout = 5000;

			while (!_runingEvent.WaitOne(_timeout))
			{
				Perform();
			}
		}

		private List<Game> GetStageGames(Guid stageID)
		{
			List<Game> _gamesList = new List<Game>(); 
						
			List<Guid> _gamesIdList = Warehouse.GetListOfStageGames(stageID);
			for (int i = 0; i < _gamesIdList.Count(); i++)
			{
				List<Guid> _IntellectsIdList = Warehouse.GetGameIntellects(_gamesIdList[i]);
				List<Player> _players = new List<Player>();
				foreach (var intel in _IntellectsIdList)
				{				
					var _newPlayer = new Player(intel, Warehouse.GetIntellectOwner(intel));
					_players.Add(_newPlayer);
				}
				var _game = new Game(_gamesIdList[i], _players);
                _game.StartTime = Warehouse.GetGameStartTime(_gamesIdList[i]);
                _game.HasResult = Warehouse.DoesMatchHasResult(_gamesIdList[i]);

				_gamesList.Add(_game);
			}

			return _gamesList;
		}

		public Stage GetStage(Guid stageId)
		{
			Stage _stage = new Stage(stageId);

			List<Guid> _IntellectsIdList = Warehouse.GetGameIntellects(stageId);
			List<Player> _players;
			foreach (var intel in _IntellectsIdList)
			{
				var _newPlayer = new Player(intel, Warehouse.GetIntellectOwner(intel));
				_players.Add(_newPlayer);
			}

			_stage.Players = _players;

			return _stage;
		}

		private bool IsAllMatchesDone(Guid tournamentId)
		{
            Guid _stageId = Warehouse.GetTournamentStages(tournamentId).First<Guid>();

			List<Guid> _stageGamesIdList = GetStageGames(_stageId);

			for (int i = 0; i < _stageGamesList.Count(); i++)
			{
				if (!Warehouse.DoesMatchHasResult(_stageGamesIdList[i]))
				{
					return false;
				}					
			}
			return true;
		}

		private void Perform()
		{
			var _activeTournaments = Warehouse.GetActiveTournaments();
			if (_activeTournaments.Any())//Проверка, есть ли запущенные турниры
			{
				foreach (Guid t in _activeTournaments)
				{					
					if (Warehouse.IsGamesListExist())
					{	
						if (IsAllMatchesDone(t))
						{
							// Считает очки, определяет победителей, формирует отчётность, завершает турнир.
							List<Game> _listOfGames = GetStageGames(Warehouse.GetTournamentStages(t).First<Guid>());
							foreach (Game g in _listOfGames)
							{								
								// Добавляет очко победителю
							}
							//Публикует очки в базу, сортирует по очкам
							//Определяет победителей турнира, формирует отчёт
                            Warehouse.UpdateStage(Warehouse.GetTournamentStages(t).First<Guid>(), State.Finished);
						}
					}
					else
					{
						Stage _stage = GetStage(t);
						foreach (Player p1 in _stage.Players)
						{
							foreach (Player p2 in _stage.Players)
							{
								if (p1 != p2)
								{
									var _intList = new List<Guid>();
									_intList.Add(p1.IntellectID);
									_intList.Add(p2.IntellectID);

									Warehouse.BeginMatch(_intList, _stage.Id, "");//ID пользователя--это идентификатор создателя матча? В данном случае--турнир или его создатель? Нужен ли Title
								}
							}
						}
					}
				}
			}
		}
	}
}
