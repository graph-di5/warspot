using System.Collections.Generic;
using WarSpot.Contracts.Intellect.Actions;
using System.Reflection;
using WarSpot.Contracts.Intellect;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System;
using WarSpot.Cloud.MatchComputer;

namespace WarSpot.MatchComputer
{
	//public class TeamIntellectList //Класс для загрузки команд с их ДЛЛками.

	public class TeamIntellectList //Класс для загрузки команд с их интеллектами.
	
	{
		public int Number { set; get ;}
		public List<IBeingInterface> Members;
	}

	public class ComputerMatcher
	{
		private List<Being> _objects;
		private List<GameAction> _actions;//Сначала задаём действия, затем делаем их в нуном порядке.
		private List<WarSpotEvent> _eventsHistory;//Для истории событий (не действий). Это и отправляется пользователю для просмотра матча.
		private ulong _turnNumber;
		private Stream _stream;//Поток для вывода сериализованной истории
		private BinaryFormatter _formatter;//Создаём буфер для сериализации
		private World _world;

		/// <summary>
		///Универсальный перемешиватель списков.
		/// </summary>
		/// <param name="list">Список на перемешивание в случайном порядке.</param>
		private void RandomShuffle<T> (List<T> list)
		{
			Random rand = new Random();
			for (int i = 0; i < list.Count; i++)
			{
				int next = rand.Next(list.Count - i - 1) + i;
				T tmp = list[i];
				list[i] = list[next];
				list[next] = tmp;
			}
		}

		/// <summary>
		///Выдача области видимости вокруг начальной точки. (квадрат)
		/// </summary>
		/// <param name="x">Координата X центральной точки области.</param>
		/// <param name="y">Координата Y центральной точки области.</param>
		/// <param name="radius">Количество точек видимости в каждую сторону.</param>
		private IWorldCell [,] ProcessWorld (int x, int y, int radius)
		{
			IWorldCell [,] res = new IWorldCell[radius * 2 + 1, radius * 2 + 1];
			for (int dx = -radius, i = 0; dx <= radius; dx++, i++)
				for (int dy = -radius, j = 0; dy <= radius; dy++, j++)
					res[i, j] = _world[(x + dx) % _world.Height, (y + dy) % _world.Width];
			return res;
		}

		/// <summary>
		///Высчитывание и вывод истории матча при заданных командах интеллектов.
		/// </summary>
		/// <param name="listIntellect">Список для загрузки всех интеллектов командами.</param>
		/// <param name="stream">Поток для выгрузки сериализованной истории событий.</param>
		public ComputerMatcher (List<TeamIntellectList> listIntellect, Stream stream)
		{
			_objects = new List<Being>();
			_actions = new List<GameAction>();
			_eventsHistory = new List<WarSpotEvent>();
			_turnNumber = 0;
			_stream = stream;
			_formatter = new BinaryFormatter();
			_world = new World();

			int c = 5; //Константа площади начального расположения, не знаю, куда её запихнуть 
			int min_dist = c * 2; // минимально возможное расстояние между стойбищами при генерации

			List <Tuple <int, int>> center = new List<Tuple<int, int>>();
			for (int i = 0; i < _world.Width; i++)
				for (int j = 0; j < _world.Height; j++)
					center.Add(new Tuple<int, int>(i, j));
			RandomShuffle(center);

			int curNum = 0;

			foreach (TeamIntellectList Team in listIntellect)
			{
				while (curNum < center.Count)
				{
					for (int i = 0; i < curNum; i++)
						if (Math.Abs(center[i].Item1 - center[curNum].Item1) + Math.Abs(center[i].Item2 - center[curNum].Item2) < min_dist)
							continue;
					curNum++;
					break;
				}

				if (curNum == center.Count)
				{
					throw new Exception();
				}

				int center_x = center[curNum].Item1, center_y = center[curNum].Item2;
				List <Tuple <int, int>> pos = new List<Tuple<int, int>>();
				for (int i = -c / 2; i <= c / 2; i++)
					for (int j = -c / 2; j < c / 2; j++)
						pos.Add(new Tuple<int, int>(i + center_x, j + center_y));
				RandomShuffle(pos);

				for (int i = 0; i < Team.Members.Count; i++)
				{
					var newBeing = new Being(Team.Members[i], Team.Number);
					newBeing.Characteristics.X = pos[i].Item1;
					newBeing.Characteristics.Y = pos[i].Item2;
					_world[pos[i].Item1, pos[i].Item2].BeingValue = newBeing;
					// todo придумать 
					newBeing.Construct(Team.Number, 0, 100);

					_objects.Add(newBeing);
					_eventsHistory.Add(new GameEventBirth(newBeing.Characteristics.Id, newBeing.Characteristics));//Рождение записываем в историю.
				}

				curNum++;
			}
		}

		/// <summary>
		///Произведение очередного кода, запись результатов в историю. Вывод истории, если матч завершился.
		/// </summary>
		public int Update()
		{
			_actions.Clear();
			_eventsHistory.Add(new SystemEventTurnStarted(_turnNumber));//Записываем в историю начало хода.

			var _objectsToDelete = new List<Being>();//Список объектов на удаление.

			//Obtaining new actions from beings
			foreach (var curObject in _objects)
			{//ToDo: send NOT NULL world info
				_actions.Add(curObject.Think(_turnNumber, curObject.Characteristics, ProcessWorld(curObject.Characteristics.X, curObject.Characteristics.Y, curObject.Characteristics.MaxSeeDistance)));
			}

			for (GameAction curAction = _actions[0]; curAction != null; curAction = _actions[0])
			{
#region Event Dealer //Проход по всем действиям этого хода.
				
				Being actor;
				Being target;
				float cost;
				int distance;

				switch (curAction.ActionType)//Выясняем тип действия, и выполняем, если это возможно.
				{
					#region GameActionAtack
					case ActionTypes.GameActionAtack:
						var atackAction = curAction as GameActionAttack;
						actor = _objects.Find(a => a.Characteristics.Id == atackAction.SenderId);
						target = _objects.Find(a => a.Characteristics.Id == atackAction.TargetId);
						cost = 20 + atackAction.Ci;
						distance = Math.Abs(actor.Characteristics.X - target.Characteristics.X) + Math.Abs(actor.Characteristics.Y - target.Characteristics.Y);

						if ((actor.Characteristics.Ci >= cost) & (actor.Characteristics.Health > 0) & (distance <= 3))
						{
							actor.Characteristics.Ci -= cost;//применяем изменения
							target.Characteristics.Health -= atackAction.Ci;

							_eventsHistory.Add(new GameEventCiChange(atackAction.SenderId, actor.Characteristics.Ci));//пишем историю
							_eventsHistory.Add(new GameEventHealthChange(atackAction.TargetId, target.Characteristics.Health));
						}

						break;
					#endregion
					#region GameActionEat
					case ActionTypes.GameActionEat:
						var eatAction = curAction as GameActionEat;
						actor = _objects.Find(a => a.Characteristics.Id == eatAction.SenderId);

						if (actor.Characteristics.Health > 0)
						{
							if (_world[actor.Characteristics.X, actor.Characteristics.Y].Ci > 20)
							{
								actor.Characteristics.Ci += 20;//Съесть можно не больше 20 энергии за ход.
								_world[actor.Characteristics.X, actor.Characteristics.Y].Ci -= 20;
							}
							else
							{
								actor.Characteristics.Ci += _world[actor.Characteristics.X, actor.Characteristics.Y].Ci;//увеличиваем энергию существа
								_world[actor.Characteristics.X, actor.Characteristics.Y].Ci = 0;//Убираем энергию из клетки
							}
							_eventsHistory.Add(new GameEventCiChange(eatAction.SenderId, actor.Characteristics.Ci));
							_eventsHistory.Add(new GameEventWorldCiChanged(actor.Characteristics.X, actor.Characteristics.Y, 0));//Событие в клетке по координатам существа
						}

						break;
					#endregion
					#region GameActionGiveCi
					case ActionTypes.GameActionGiveCi:

						var giveCiAction = curAction as GameActionGiveCi;
						actor = _objects.Find(a => a.Characteristics.Id == giveCiAction.SenderId);
						target = _objects.Find(a => a.Characteristics.Id == giveCiAction.TargetId);
						cost = 20 + giveCiAction.Ci;
						distance = Math.Abs(actor.Characteristics.X - target.Characteristics.X) + Math.Abs(actor.Characteristics.Y - target.Characteristics.Y);

						if ((actor.Characteristics.Ci >= cost) & (actor.Characteristics.Health > 0) & (distance <=3))
						{
							actor.Characteristics.Ci -= cost;
							target.Characteristics.Ci += giveCiAction.Ci;

							_eventsHistory.Add(new GameEventCiChange(giveCiAction.SenderId, actor.Characteristics.Ci));
							_eventsHistory.Add(new GameEventCiChange(giveCiAction.TargetId, target.Characteristics.Ci));
						}

						break;
					#endregion
					#region GameActionMove
					case ActionTypes.GameActionMove:

						var moveAction = curAction as GameActionMove;
						actor = _objects.Find(a => a.Characteristics.Id == moveAction.SenderId);
						cost = (Math.Abs(moveAction.ShiftX) + Math.Abs(moveAction.ShiftY)) * actor.Characteristics.MaxHealth / 100;
						distance = Math.Abs(moveAction.ShiftX) + Math.Abs(moveAction.ShiftY);

						if ((actor.Characteristics.Ci >= cost) & (actor.Characteristics.Health > 0) & (_world[actor.Characteristics.X, actor.Characteristics.Y].Being.Equals(null))
							& (distance <= actor.Characteristics.MaxStep))
						{
							actor.Characteristics.X += moveAction.ShiftX;
							actor.Characteristics.Y += moveAction.ShiftY;

							_eventsHistory.Add(new GameEventMove(moveAction.SenderId, moveAction.ShiftX, moveAction.ShiftY));
						}

						break;
					#endregion
					#region GameActionTreat
					case ActionTypes.GameActionTreat:

						var treatAction = curAction as GameActionGiveCi;
						actor = _objects.Find(a => a.Characteristics.Id == treatAction.SenderId);
						target = _objects.Find(a => a.Characteristics.Id == treatAction.TargetId);
						cost = treatAction.Ci;
						distance = Math.Abs(actor.Characteristics.X - target.Characteristics.X) + Math.Abs(actor.Characteristics.Y - target.Characteristics.Y);

						if ((actor.Characteristics.Ci >= cost) & (actor.Characteristics.Health > 0) & (distance <= 3))
						{
							actor.Characteristics.Health -= cost;
							target.Characteristics.Health += cost/3;

							_eventsHistory.Add(new GameEventHealthChange(treatAction.SenderId, actor.Characteristics.Health));
							_eventsHistory.Add(new GameEventHealthChange(treatAction.TargetId, target.Characteristics.Health));
						}

						break;
					#endregion
					#region GameActionMakeOffspring
					case ActionTypes.GameActionMakeOffspring:
						var birthAcrion = curAction as GameActionMakeOffspring;
						actor = _objects.Find(a => a.Characteristics.Id == birthAcrion.SenderId);
						var _offspring = new Being(actor.Me, actor.Characteristics.Team);
						_offspring.Construct(actor.Characteristics.Team, _turnNumber, birthAcrion.Ci);//Вызываем пользовательский конструктор.
						
						//Собственная проверка стоимости
						cost = _offspring.Characteristics.MaxHealth * 0.8f//Стоимость максимального здоровья
							+ _offspring.Characteristics.MaxStep * _offspring.Characteristics.MaxStep//Стоимость максимального шага
							+ (_offspring.Characteristics.MaxSeeDistance / 2.0f) * (_offspring.Characteristics.MaxSeeDistance / 2.0f);//Стоимость дистанции видимости
						
						var _emptyEnvirons = new List<WorldCell>();
						var _environs = ProcessWorld(actor.Characteristics.X, actor.Characteristics.Y, 1);//Собираем информацию об окресностях.

						_emptyEnvirons.Clear(); //ToDo: Нужно ли?

						foreach (WorldCell c in _environs)//Собираем список пустых ячеек вокруг существа
						{
							if (c.Being.Equals(null))
							{
								_emptyEnvirons.Add(c);
							}
						}

						if (_emptyEnvirons.Count() > 0)
						{
							Random r = new Random();
							int d = r.Next(_emptyEnvirons.Count - 1);//Номер клетки из списка пустых клеток вокруг существа.
						
							if ((actor.Characteristics.Ci >= cost) & (actor.Characteristics.Ci >= birthAcrion.Ci)
								 & (actor.Characteristics.Health > 0) & (actor.Characteristics.Ci >= _offspring.Characteristics.MaxHealth * 0.9f) & (actor.Characteristics.Health >= actor.Characteristics.MaxHealth * 0.8f))
							{
								_offspring.Characteristics.Health = _offspring.Characteristics.MaxHealth * 0.6f;
								_offspring.Characteristics.Ci = _offspring.Characteristics.MaxHealth * 0.3f;
								_offspring.Characteristics.X = _emptyEnvirons[d].X;
								_offspring.Characteristics.Y = _emptyEnvirons[d].Y;

								_objects.Add(_offspring);
								_eventsHistory.Add(new GameEventBirth(_offspring.Characteristics.Id, _offspring.Characteristics));
								actor.Characteristics.Ci -= cost;
								_eventsHistory.Add(new GameEventCiChange(actor.Characteristics.Id, actor.Characteristics.Ci));
							}
						}

						break;
					#endregion
				}
				
				_actions.Remove(curAction);//Удаляем  проверенное действие
			}
#endregion
#region Grim Reaper //Здесь удаляются все, у кого кончилось здоровье.

			foreach (var curObject in _objects)//проверяем мёртвых
			{
				if (curObject.Characteristics.Health <= 0)
				{
					_world[curObject.Characteristics.X, curObject.Characteristics.Y].Ci += (curObject.Characteristics.Ci+curObject.Characteristics.MaxHealth/5);//Из существа при смерти вываливается энергия.

					_eventsHistory.Add(new GameEventDeath(curObject.Characteristics.Id));
					_eventsHistory.Add(new GameEventWorldCiChanged(curObject.Characteristics.X, curObject.Characteristics.Y, _world[curObject.Characteristics.X, curObject.Characteristics.Y].Ci));

					_objectsToDelete.Add(curObject);//Вносим умерших в список на удаление
				}
			}

			for (int i = 0; i < _objectsToDelete.Count; i++ )
			{
				_objects.Remove(_objectsToDelete[i]);//удаляем все умершие объекты из главного списка
			}

			_objectsToDelete.Clear();
#endregion
#region Check For Winning //Если остались представители лишь одной команды--эта команда победила.
			if (_objects.FindAll(a => a.Characteristics.Team != 0).GroupBy(a => a.Characteristics.Team).Count() == 1)
			{
				int winer = _objects.Find(a => a.Characteristics.Team != 0).Characteristics.Team;
				_eventsHistory.Add(new SystemEventCommandWin(winer));//Объявляем победителя
				_eventsHistory.Add(new SystemEventMatchEnd());//И матч заканчивается.

				PullOut();//Отдаём историю событий.

				return 0;
			}
			else
			{
				_turnNumber++;
				return 1;//Если нужны ещё ходы.
			}
#endregion
		}

		public void GetIntellect(string name)
		{
			byte[] intellect = TaskHandler.GetIntellect(name);
		}


		/// <summary>
		///Выгрузка в поток накопившейся истории событий, очистка истории для дальнейшего накопления.
		/// </summary>
		public void PullOut()
		{
			_formatter.Serialize(_stream, _eventsHistory);//Отдаём всё, что успело накопиться в истории событий с последнего вызова этого метода.
			_eventsHistory.Clear();//Очищаем историю.

		}

	}		
}

