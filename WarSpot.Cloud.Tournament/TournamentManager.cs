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

		private void Perform()
		{
			var _activeTournaments = Warehouse.GetActiveTournaments();
			if (_activeTournaments.Any())//Проверка, есть ли запущенные турниры
			{
				foreach (Storage.Tournament t in _activeTournaments)
				{
				    var stage = Warehouse.GetTournamentStages(t.Tournament_ID).First(); // TODO: adjust for multiple stages
				    var games = Warehouse.GetListOfStageGames(stage.Stage_ID);
					if (games.Any())
					{	
						if (games.All(x=>Warehouse.DoesMatchHasResult(x.Game_ID)))
						{
                            var scores = t.Player.ToDictionary(x => x.Account_ID, x => 0);
							// Считает очки, определяет победителей, формирует отчётность, завершает турнир.
							foreach (Game g in games)
							{								
								// Добавляет очко победителю
							    var replay = Warehouse.GetReplay(g.Game_ID);
							    
							    var winner = g.Teams.First(x => x.Team_ID == replay.Data.WinnerTeam).Intellects.First().Account;

							    scores[winner.Account_ID] += 1;
							}

						    foreach (var score in scores)
						    {
						        Warehouse.AddScore(stage, t.Player.First(x => x.Account_ID == score.Key), score.Value);
						    }

							//Публикует очки в базу, сортирует по очкам
							//Определяет победителей турнира, формирует отчёт

                            Warehouse.UpdateStage(stage.Stage_ID, State.Finished);
						}
					}
					else
					{
						foreach (var p1 in stage.Intellects)
						{
							foreach (var p2 in stage.Intellects)
							{
								if (p1 != p2)
								{
									var _intList = new List<Guid>();
									_intList.Add(p1.Intellect_ID);
									_intList.Add(p2.Intellect_ID);

									Warehouse.BeginMatch(_intList, t.Creator_ID, 
                                        string.Format("{0}: {1} vs. {2}",t.Tournament_Name, p1.Intellect_Name, p2.Intellect_Name), 
                                        stage.Stage_ID);
								}
							}
						}
					}
				}
			}
		}
	}
}
