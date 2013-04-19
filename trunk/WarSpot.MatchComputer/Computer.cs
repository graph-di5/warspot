using System;
using System.Collections.Generic;
using System.Linq;
using WarSpot.Common;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;
using WarSpot.Contracts.Service;

namespace WarSpot.MatchComputer
{
	public class Computer
	{
		private readonly List<Being> _objects;
		private readonly List<GameAction> _actions;//Сначала задаём действия, затем делаем их в нужом порядке.
		private readonly MatchReplay _eventsHistory;//Для истории событий (не действий). Это и отправляется пользователю для просмотра матча.
		private readonly World _world;
		private ulong _turnNumber;

		/// <summary>
		///Универсальный перемешиватель списков.
		/// </summary>
		/// <param name="list">Список на перемешивание в случайном порядке.</param>
		private static void RandomShuffle<T>(IList<T> list)
		{
			var rand = new Random();
			for (var i = 0; i < list.Count; i++)
			{
				int next = rand.Next(list.Count - i - 1) + i;
				T tmp = list[i];
				list[i] = list[next];
				list[next] = tmp;
			}
		}

		/// <summary>
		/// Выдача области видимости вокруг начальной точки. (квадрат)
		/// </summary>
		/// <param name="x">Координата X центральной точки области.</param>
		/// <param name="y">Координата Y центральной точки области.</param>
		/// <param name="radius">Количество точек видимости в каждую сторону.</param>
		private WorldInfo ProcessWorld(int x, int y, int radius)
		{
			var res = new WorldInfo(radius);
			for (int dx = -radius; dx <= radius; dx++)
				for (int dy = -radius; dy <= radius; dy++)
				{
					var w = _world[x + dx, y + dy];
					res[dx, dy] = new WorldCellInfo
												{
													Ci = w.Ci,
													BeingCharacteristics = (w.BeingValue == null) ? null : w.BeingValue.Characteristics
												};
				}
			return res;
		}

		private WorldCell[,] GetWorldPart(int x, int y, int radius)
		{
			var res = new WorldCell[radius * 2 + 1, radius * 2 + 1];
			for (int dx = -radius, i = 0; dx <= radius; dx++, i++)
				for (int dy = -radius, j = 0; dy <= radius; dy++, j++)
					res[i, j] = _world[(x + dx) % _world.Height, (y + dy) % _world.Width];
			return res;
		}

		/// <summary>
		///Высчитывание и вывод истории матча при заданных командах интеллектов.
		/// </summary>
		/// <param name="listIntellect">Список для загрузки всех интеллектов командами.</param>
		public Computer(IEnumerable<TeamIntellectList> listIntellect)
		{
			_objects = new List<Being>();
			_actions = new List<GameAction>();
			_eventsHistory = new MatchReplay();
			_turnNumber = 0;
			_world = new World();

			int c = 5; //Константа площади начального расположения, не знаю, куда её запихнуть 
			int minDist = c * 2; // минимально возможное расстояние между стойбищами при генерации

			var center = new List<Tuple<int, int>>();
			for (int i = 0; i < _world.Width; i++)
				for (int j = 0; j < _world.Height; j++)
					center.Add(new Tuple<int, int>(i, j));
			RandomShuffle(center);

			_eventsHistory.Events.Add(new SystemEventWorldCreated(_world.Width, _world.Height));
			_eventsHistory.Events.Add(new SystemEventTurnStarted(_turnNumber));//Записываем в историю начало хода.

			int curNum = 0;

			foreach (TeamIntellectList team in listIntellect)
			{
				while (curNum < center.Count)
				{
					for (int i = 0; i < curNum; i++)
						if (Math.Abs(center[i].Item1 - center[curNum].Item1) + Math.Abs(center[i].Item2 - center[curNum].Item2) < minDist)
							continue;
					curNum++;
					break;
				}

				if (curNum == center.Count)
				{
					throw new Exception();
				}

				int center_x = center[curNum].Item1, center_y = center[curNum].Item2;
				List<Tuple<int, int>> pos = new List<Tuple<int, int>>();
				for (int i = -c / 2; i <= c / 2; i++)
					for (int j = -c / 2; j < c / 2; j++)
						pos.Add(new Tuple<int, int>(i + center_x, j + center_y));
				RandomShuffle(pos);

				for (int i = 0; i < team.Members.Count; i++)
				{
					if (team.Members[i] == null)
					{
						continue;
					}
					var newBeing = new Being(team.Members[i]);
					newBeing.Construct(0, 60);
                    if (60 >= newBeing.Characteristics.Cost())//Проверка, можно ли создать начальное существо с такими характеристиками
                    {
                        newBeing.Characteristics.Team = team.TeamId;
                        newBeing.Characteristics.X = pos[i].Item1;
                        newBeing.Characteristics.Y = pos[i].Item2;
                        _world[pos[i].Item1, pos[i].Item2].BeingValue = newBeing;

                        _objects.Add(newBeing);
                        _eventsHistory.Events.Add(new GameEventBirth(newBeing.Characteristics.Id, newBeing.Characteristics));//Рождение записываем в историю.
                    }
				}

				curNum++;
			}

			_turnNumber++;
		}

		public MatchReplay Compute()
		{
			while (Update() != 0)
			{
			}

			return _eventsHistory;//После окончания игры отдаём историю событий
		}

		/// <summary>
		///Произведение очередного кода, запись результатов в историю. Вывод истории, если матч завершился.
		/// </summary>
		private int Update()
		{
			_actions.Clear();
			_eventsHistory.Events.Add(new SystemEventTurnStarted(_turnNumber));//Записываем в историю начало хода.

			var objectsToDelete = new List<Being>();//Список объектов на удаление.

			//Obtaining new actions from beings
			foreach (var curObject in _objects)
			{
				_actions.Add(curObject.Think(_turnNumber, curObject.Characteristics,
				ProcessWorld(curObject.Characteristics.X, curObject.Characteristics.Y, curObject.Characteristics.MaxSeeDistance)));
			}

			foreach (var curObject in _objects)
			{
				curObject.Characteristics.Ci -= 0.01f * (0.1f * curObject.Characteristics.MaxHealth +
				0.05f * curObject.Characteristics.MaxSeeDistance * curObject.Characteristics.MaxSeeDistance +
				0.3f * curObject.Characteristics.MaxStep);
				_eventsHistory.Events.Add(new GameEventCiChange(curObject.Characteristics.Id, curObject.Characteristics.Ci));
			}

			#region Event Dealer //Проход по всем действиям этого хода.

			RandomShuffle(_actions);//Перемешиваем действия для увеличения энтропии.

			foreach (var curAction in _actions)
			{
				Being actor;
				Being target;
				float cost;
				int distance;

				switch (curAction.ActionType)//Выясняем тип действия, и выполняем, если это возможно.
				{
				#region GameActionAtack
				case ActionTypes.GameActionAtack:
					var atackAction = curAction as GameActionAttack;
					if (atackAction == null)
					{
						break;
					}
					actor = _objects.Find(a => a.Characteristics.Id == atackAction.SenderId);
					cost = 0.2f * actor.Characteristics.MaxHealth + atackAction.Damage;
					distance = Math.Abs(atackAction.X) + Math.Abs(atackAction.Y);

					_eventsHistory.Events.Add(new GameEventCiChange(atackAction.SenderId, actor.Characteristics.Ci));//В любом случае отнимаем энергию.

					if (_world[actor.Characteristics.X + atackAction.X, actor.Characteristics.Y + atackAction.Y].BeingValue != null)
					{
						target = _objects.Find(a => a.Characteristics.Id == _world[actor.Characteristics.X + atackAction.X, actor.Characteristics.Y + atackAction.Y].BeingValue.Characteristics.Id);


						if ((actor.Characteristics.Ci >= cost) && (actor.Characteristics.Health > 0) && (distance <= 3))
						{
							actor.Characteristics.Ci -= cost;//применяем изменения
							target.Characteristics.Health -= atackAction.Damage;


							_eventsHistory.Events.Add(new GameEventHealthChange(target.Characteristics.Id, target.Characteristics.Health));
						}
					}

					break;
				#endregion
				#region GameActionEat
				case ActionTypes.GameActionEat:
					var eatAction = curAction as GameActionEat;
					if (eatAction == null)
					{
						break;
					}

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
						_eventsHistory.Events.Add(new GameEventCiChange(eatAction.SenderId, actor.Characteristics.Ci));
						_eventsHistory.Events.Add(new GameEventWorldCiChanged(actor.Characteristics.X, actor.Characteristics.Y, 0));//Событие в клетке по координатам существа
					}

					break;
				#endregion
				#region GameActionGiveCi
				case ActionTypes.GameActionGiveCi:

					var giveCiAction = curAction as GameActionGiveCi;
					if (giveCiAction == null)
					{
						break;
					}

					actor = _objects.Find(a => a.Characteristics.Id == giveCiAction.SenderId);
					cost = 0.1f * actor.Characteristics.MaxHealth + giveCiAction.Ci;
					distance = Math.Abs(giveCiAction.X) + Math.Abs(giveCiAction.Y);

					if ((actor.Characteristics.Ci >= cost) && (actor.Characteristics.Health > 0) && (distance <= 3))
					{
						actor.Characteristics.Ci -= cost;//В любом случае отнимаем энергию
						_eventsHistory.Events.Add(new GameEventCiChange(giveCiAction.SenderId, actor.Characteristics.Ci));

						if (_world[actor.Characteristics.X + giveCiAction.X, actor.Characteristics.Y + giveCiAction.Y].BeingValue != null)
						{
							target = _objects.Find(a => a.Characteristics.Id == _world[actor.Characteristics.X + giveCiAction.X, actor.Characteristics.Y + giveCiAction.Y].BeingValue.Characteristics.Id);
							target.Characteristics.Ci += giveCiAction.Ci;
							_eventsHistory.Events.Add(new GameEventCiChange(target.Characteristics.Id, target.Characteristics.Ci));
						}

						else//Если в клетке никого, энергия скидывается в клетку.
						{
							_world[actor.Characteristics.X + giveCiAction.X, actor.Characteristics.Y + giveCiAction.Y].Ci += giveCiAction.Ci;
							_eventsHistory.Events.Add(new GameEventWorldCiChanged(actor.Characteristics.X + giveCiAction.X, actor.Characteristics.Y + giveCiAction.Y, giveCiAction.Ci));
						}
					}

					break;
				#endregion
				#region GameActionMove
				case ActionTypes.GameActionMove:

					var moveAction = curAction as GameActionMove;
					if (moveAction == null)
					{
						break;
					}
					actor = _objects.Find(a => a.Characteristics.Id == moveAction.SenderId);
					cost = actor.Characteristics.MaxStep * (Math.Abs(moveAction.ShiftX) + Math.Abs(moveAction.ShiftY)) * actor.Characteristics.MaxHealth * 0.01f;
					distance = Math.Abs(moveAction.ShiftX) + Math.Abs(moveAction.ShiftY);

					if ((actor.Characteristics.Ci >= cost) && (actor.Characteristics.Health > 0)
						&& (_world[actor.Characteristics.X + moveAction.ShiftX, actor.Characteristics.Y + moveAction.ShiftY].BeingValue == null)
						&& (distance <= actor.Characteristics.MaxStep))
					{
						_world[actor.Characteristics.X, actor.Characteristics.Y].BeingValue = null;//Удаляем из клетки ссылку на существо.
						actor.Characteristics.X += moveAction.ShiftX;
						actor.Characteristics.Y += moveAction.ShiftY;
						actor.Characteristics.Ci -= cost;
						_world[actor.Characteristics.X, actor.Characteristics.Y].BeingValue = actor;//Существо появляется в другой клетке.

						_eventsHistory.Events.Add(new GameEventCiChange(actor.Characteristics.Id, actor.Characteristics.Ci));
						_eventsHistory.Events.Add(new GameEventMove(moveAction.SenderId, moveAction.ShiftX, moveAction.ShiftY));
					}

					break;
				#endregion
				#region GameActionTreat
				case ActionTypes.GameActionTreat:

					var treatAction = curAction as GameActionTreat;
					if (treatAction == null)
					{
						break;
					}

					actor = _objects.Find(a => a.Characteristics.Id == treatAction.SenderId);
					cost = treatAction.UsingCi;
					distance = Math.Abs(treatAction.X) + Math.Abs(treatAction.Y);

					if ((_world[actor.Characteristics.X + treatAction.X, actor.Characteristics.Y + treatAction.Y].BeingValue != null)
							&& (actor.Characteristics.Ci >= cost) && (actor.Characteristics.Health > 0) && (distance <= 3) && (cost <= actor.Characteristics.Ci * 0.6f))
					{//Если в клетке никого нет, ничего не происходит
						actor.Characteristics.Ci -= cost;
						target = _objects.Find(a => a.Characteristics.Id == _world[actor.Characteristics.X + treatAction.X, actor.Characteristics.Y + treatAction.Y].BeingValue.Characteristics.Id);
						target.Characteristics.Health += cost / 3;

						_eventsHistory.Events.Add(new GameEventHealthChange(treatAction.SenderId, actor.Characteristics.Health));
						_eventsHistory.Events.Add(new GameEventHealthChange(treatAction.TargetId, target.Characteristics.Health));
					}

					break;
				#endregion
				#region GameActionMakeOffspring
				case ActionTypes.GameActionMakeOffspring:
					var birthAcrion = curAction as GameActionMakeOffspring;
					if (birthAcrion == null)
					{
						break;
					}
					actor = _objects.Find(a => a.Characteristics.Id == birthAcrion.SenderId);
					var offspring = new Being(actor.Me.Copy());
					// //!! todo understand what is happening here with team number
					offspring.Construct(_turnNumber, birthAcrion.Ci);//Вызываем пользовательский конструктор.

					//Собственная проверка стоимости
					cost = offspring.Characteristics.MaxHealth * 0.8f//Стоимость максимального здоровья
						+ offspring.Characteristics.MaxStep * offspring.Characteristics.MaxStep//Стоимость максимального шага
						+ (offspring.Characteristics.MaxSeeDistance / 2.0f) * (offspring.Characteristics.MaxSeeDistance / 2.0f);//Стоимость дистанции видимости

					var emptyEnvirons = new List<WorldCell>();
					var environs = GetWorldPart(actor.Characteristics.X, actor.Characteristics.Y, 1);//Собираем информацию об окресностях.

					emptyEnvirons.AddRange(from WorldCell c in environs where c.BeingValue == null select c);

					if (emptyEnvirons.Count() > 0)
					{
						var r = new Random();
						int d = r.Next(emptyEnvirons.Count - 1);//Номер клетки из списка пустых клеток вокруг существа.

						if ((actor.Characteristics.Ci >= cost) && (actor.Characteristics.Ci >= birthAcrion.Ci)
							 && (actor.Characteristics.Health > 0) && (actor.Characteristics.Ci >= offspring.Characteristics.MaxHealth * 0.9f)
							 && (actor.Characteristics.Health >= actor.Characteristics.MaxHealth * 0.8f))
						{
							offspring.Characteristics.Health = offspring.Characteristics.MaxHealth * 0.6f;
							offspring.Characteristics.Ci = offspring.Characteristics.MaxHealth * 0.3f;
							offspring.Characteristics.X = emptyEnvirons[d].X;
							offspring.Characteristics.Y = emptyEnvirons[d].Y;

							_objects.Add(offspring);
							_world[offspring.Characteristics.X, offspring.Characteristics.Y].BeingValue = offspring;//В ячейке мира появляется рождённое существо.
							_eventsHistory.Events.Add(new GameEventBirth(offspring.Characteristics.Id, offspring.Characteristics));
							actor.Characteristics.Ci -= cost;
							_eventsHistory.Events.Add(new GameEventCiChange(actor.Characteristics.Id, actor.Characteristics.Ci));
						}
					}

					break;
				#endregion
				}

				// todo remove this 
#if false
				_actions.Remove(curAction);//Удаляем  проверенное действие
#endif
			}
			#endregion
			#region Grim Reaper //Здесь удаляются все, у кого кончилось здоровье.

			foreach (var curObject in _objects)//проверяем мёртвых
			{
				if (curObject.Characteristics.Health <= 0)
				{
					_world[curObject.Characteristics.X, curObject.Characteristics.Y].Ci += (curObject.Characteristics.Ci + curObject.Characteristics.MaxHealth / 5);//Из существа при смерти вываливается энергия.
					_world[curObject.Characteristics.X, curObject.Characteristics.Y].BeingValue = null;

					_eventsHistory.Events.Add(new GameEventDeath(curObject.Characteristics.Id));
					_eventsHistory.Events.Add(new GameEventWorldCiChanged(curObject.Characteristics.X, curObject.Characteristics.Y, _world[curObject.Characteristics.X, curObject.Characteristics.Y].Ci));

					objectsToDelete.Add(curObject);//Вносим умерших в список на удаление
				}
			}

			foreach (var t in objectsToDelete)
			{
				Guid team = t.Characteristics.Team;

				_objects.Remove(t);//удаляем умершего из главного списка

				if ((team != Guid.Empty) && (!(_objects.Any(a => a.Characteristics.Team == team))))
				{//Если команда не нулевая, и нет никого с такой же командой, пишем событие проигрыша команды.
					_eventsHistory.Events.Add(new SystemEventCommandDead(team));
				}
			}

			objectsToDelete.Clear();
			#endregion
			#region Check For Winning //Если остались представители лишь одной команды--эта команда победила.
			if (_objects.FindAll(a => a.Characteristics.Team != Guid.Empty).GroupBy(a => a.Characteristics.Team).Count() == 1)
			{
				Guid winer = _objects.Find(a => a.Characteristics.Team != Guid.Empty).Characteristics.Team;
				_eventsHistory.Events.Add(new SystemEventCommandWin(winer));//Объявляем победителя
				_eventsHistory.Events.Add(new SystemEventMatchEnd());//И матч заканчивается.
				_eventsHistory.WinnerTeam = winer;
				_eventsHistory.Steps = _turnNumber;
				return 0;//Ходы больше не нужны
			}
			if (!_objects.FindAll(a => a.Characteristics.Team != Guid.Empty).GroupBy(a => a.Characteristics.Team).Any())
			{
				// system  command win
				_eventsHistory.Events.Add(new SystemEventCommandWin(Guid.Empty));//Объявляем победителем 0 команду ToDo: Вставить определение победителя.
				_eventsHistory.Events.Add(new SystemEventMatchEnd());//И матч заканчивается.
				_eventsHistory.WinnerTeam = Guid.Empty;
				_eventsHistory.Steps = _turnNumber;
				return 0;//Ходы больше не нужны
			}

			_turnNumber++;
			return 1;//Нужны ещё ходы.

			#endregion
		}

		// todo delete this functions

#if false
		/*public void GetIntellect(string name)
		{
			byte[] intellect = TaskHandler.GetIntellect(name);
		}
		*/

		public void FileStreamer()
		{
			string address=" ";
			//FileMode mode = new FileMode();
			FileStream fileStream = new FileStream(address, FileMode.Open);
		}


#endif

	}
}

