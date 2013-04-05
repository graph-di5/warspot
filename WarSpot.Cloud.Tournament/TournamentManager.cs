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
		private bool _active;
		private Thread _thread;

		private TournamentManager()
		{
			_active = true;
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

		public void Run()
		{
			_active = true;
		}

		public void Stop()
		{
			_active = false;
		}

		private void ThreadFunction()
		{
			int _timeout = 5000;

			while (true)
			{
				if (_active)
				{
					perform();
				}

				else
				{
					Thread.Sleep(_timeout);
				}
			}
		}

		private void perform()
		{
			//var _activeTournaments = Warehouse.GetActiveTournaments();//Здесь запрос
			//if (_activeTournaments.Count() != 0)//Проверка, есть ли запущенные турниры
			//{
			//    foreach (Tournament t in _activeTournaments)
			//    {
			//        if (Warehouse.IsGamesListExist())
			//        {
			//            if (Warehouse.AllMatchesHasResults())
			//            {
			//                // Считает очки, определяет победителей, формирует отчётность, завершает турнир.
			//                List<Game> _listOfGames = Warehouse.GetListOfTournamentGames(t.Id);
			//                foreach (Game g in _listOfGames)
			//                {
			//                    // Добавляет очко победителю (t.Scores....+= 1;)
			//                }
			//                t.Scores.SortByPoints();
			//                //Определяет победителей турнира, формирует отчёт
			//                t.State = Status.Finished;
			//            }
			//        }
			//        else
			//        {
			//            foreach (Player p1 in t.Players)
			//            {
			//                foreach (Player p2 in t.Players)
			//                {
			//                    if (p1 != p2)
			//                    {
			//                        Warehouse.BeginMatch(p1.IntellectID, p2.IntellectID);//Не уверен, какие поля ещё нужны
			//                    }
			//                }
			//            }						
			//        }
			//    }
			//}
		}//Засыпание вне

	}
}
