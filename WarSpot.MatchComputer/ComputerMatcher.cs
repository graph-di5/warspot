using System.Collections.Generic;
using WarSpot.Contracts.Intellect.Actions;
using System.Reflection;
using WarSpot.Contracts.Intellect;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System;

namespace WarSpot.MatchComputer
{

	

	public class TeamIntellectList //Класс для загрузки команд с их ДЛЛками.
	{
		public int Number { set; get ;}
		public List<IBeingInterface> Members;
	}

	public class ComputerMatcher
	{
		public const float DAMAGE = 100500;//ToDo: Решить, куда всякое подобное пихнуть, и будет ли урон всегда одинаковым.

		private List<Being> _objects;
		private List<GameAction> _actions;//Сначала задаём действия, затем делаем их в нуном порядке.
		private List<WarSpotEvent> _eventsHistory;//Для истории событий (не действий). Это и отправляется пользователю для просмотра матча.
		private ulong _step;
		private Stream _stream;
		private BinaryFormatter _formatter;
		private World _world;

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
		
		private IWorldCell [,] ProcessWorld (int cx, int cy, int radius)
		{
			IWorldCell [,] res = new IWorldCell[radius * 2 + 1, radius * 2 + 1];
			for (int dx = -radius, i = 0; dx <= radius; dx++, i++)
				for (int dy = -radius, j = 0; dy <= radius; dy++, j++)
					res[i, j] = _world.Map[(cx + dx) % _world.Height, (cy + dy) % _world.Width];
			return res;
		}

		/// <summary>
		///Загрузка всех объектов списками команд
		/// </summary>
		public ComputerMatcher (List<TeamIntellectList> _listIntellect, StreamWriter _stream)//Имена потоков одинаковые (хранимого и получаемого)
		{
			_objects = new List<Being>();
			_actions = new List<GameAction>();
			_eventsHistory = new List<WarSpotEvent>();
			_step = 0;
			//_stream = new Stream() //создание какого то потока для сериализации ивентов
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

			foreach (TeamIntellectList Team in _listIntellect)
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

				//foreach (IBeingInterface _newIntellect in Team.Members)
				for (int i = 0; i < Team.Members.Count; i++)
				{
					var newBeing = new Being(Team.Members[i], Team.Number);
					newBeing.Characteristics.X = pos[i].Item1;//
					newBeing.Characteristics.Y = pos[i].Item2;//Убрал Coordinates
					_world.Map[pos[i].Item1, pos[i].Item2].Being = newBeing;
					// todo придумать 
					newBeing.Construct(Team.Number, 0, 10000, ProcessWorld(pos[i].Item1, pos[i].Item2, newBeing.Characteristics.MaxSeeDistance));
					
					_objects.Add(newBeing);
				}

				curNum++;
			}
		}

		public void Update()
		{
			_actions.Clear();
			_step++;
			//Obtaining new actions from beings
			foreach (var curObject in _objects)
			{
				// todo send NOT NULL world info
				int xPos = curObject.Characteristics.X; //
				int yPos = curObject.Characteristics.Y; //Убрал Coordinates
				int seeDistance = curObject.Characteristics.MaxSeeDistance;

				_actions.Add(curObject.Think(_step, curObject.Characteristics, ProcessWorld(xPos, yPos, seeDistance)));
			}

			for (GameAction curAction = _actions[0]; curAction != null; curAction = _actions[0])
			{
				_formatter.Serialize(_stream, curAction); //Сериализация ивента

				#region Event Dealer 
				if (curAction.Cost() <= _objects.Find(a => a.Characteristics.Id == curAction.SenderId).Characteristics.Ci)
				{
					curAction.Execute();//А это мы уже не используем.

					//ToDo: Переписать так, чтобы возможность действия считалась внутри свича (логичнее для сложных оценок стоимости действий)
					Being actor;
					Being target;
					switch (curAction.ActionType)
					{
						case ActionTypes.GameActionAtack:
							var atackAction = curAction as GameActionAttack;
							actor = _objects.Find(a => a.Characteristics.Id == atackAction.SenderId);
							target = _objects.Find(a => a.Characteristics.Id == atackAction.TargetId);

							actor.Characteristics.Ci -= atackAction.Cost();//применяем изменения
							target.Characteristics.Health -= DAMAGE; //Можно переписать на урон, зависящий от потраченной на удар энергии
							
							_eventsHistory.Add(new GameEventCiChange(atackAction.SenderId, actor.Characteristics.Ci));
							_eventsHistory.Add(new GameEventHealthChange(atackAction.TargetId, target.Characteristics.Health));//пишем историю

							break;

						case ActionTypes.GameActionDie:
							var deathAction = curAction as GameActionDie;

							_eventsHistory.Add(new GameEventDeath(deathAction.SenderId));

							break;

						case ActionTypes.GameActionEat:
							var eatAction = curAction as GameActionEat;
							actor = _objects.Find(a => a.Characteristics.Id == eatAction.SenderId);
							
							actor.Characteristics.Ci += _world.Map[actor.Characteristics.X, actor.Characteristics.Y].Ci;//увеличиваем энергию существа
							_world.Map[actor.Characteristics.X, actor.Characteristics.Y].Ci = 0;//Убираем энергию из клетки
							
							_eventsHistory.Add(new GameEventCiChange(eatAction.SenderId, actor.Characteristics.Ci));
							_eventsHistory.Add(new GameEventWorldCiChanged(actor.Characteristics.X, actor.Characteristics.Y, 0));//Событие в клетке по координатам существа

							break;

						case ActionTypes.GameActionGiveCi:

							var giveCiAction = curAction as GameActionGiveCi;
							actor = _objects.Find(a => a.Characteristics.Id == giveCiAction.SenderId);
							target = _objects.Find(a => a.Characteristics.Id == giveCiAction.TargetId);

							actor.Characteristics.Ci -= giveCiAction.Cost();
							target.Characteristics.Ci += giveCiAction.Cost();

							_eventsHistory.Add(new GameEventCiChange(giveCiAction.SenderId, actor.Characteristics.Ci));
							_eventsHistory.Add(new GameEventCiChange(giveCiAction.TargetId, target.Characteristics.Ci));

							break;

						case ActionTypes.GameActionMove:

							var moveAction = curAction as GameActionMove;
							actor = _objects.Find(a => a.Characteristics.Id == moveAction.SenderId);

							actor.Characteristics.X += moveAction.ShiftX;
							actor.Characteristics.Y += moveAction.ShiftY;

							_eventsHistory.Add(new GameEventMove(moveAction.SenderId, moveAction.ShiftX, moveAction.ShiftY));

							break;

						case ActionTypes.GameActionTreat:

							var treatAction = curAction as GameActionGiveCi;
							actor = _objects.Find(a => a.Characteristics.Id == treatAction.SenderId);
							target = _objects.Find(a => a.Characteristics.Id == treatAction.TargetId);

							actor.Characteristics.Health -= treatAction.Cost();
							target.Characteristics.Health += treatAction.Cost();

							_eventsHistory.Add(new GameEventHealthChange(treatAction.SenderId, actor.Characteristics.Health));
							_eventsHistory.Add(new GameEventHealthChange(treatAction.TargetId, target.Characteristics.Health));

							break;
					}

					#endregion
					_actions.Remove(curAction);
				}
			}
			#region Objects Deleter //Здесь удаляются все, у кого кончилось здоровье
			var _objectsToDelete = new List<Being>();//Список объектов на удаление

			foreach (var curObject in _objects)//проверяем мёртвых
			{
				if (curObject.Characteristics.Health <= 0)
				{
					_world.Map[curObject.Characteristics.X, curObject.Characteristics.Y].Ci += curObject.Characteristics.Ci;//Из существа при смерти вываливается энергия. ToDo: Таки энергия или энергия из здоровья (мясо)--как сделать? 

					_eventsHistory.Add(new GameEventDeath(curObject.Characteristics.Id));
					_eventsHistory.Add(new GameEventWorldCiChanged(curObject.Characteristics.X, curObject.Characteristics.Y, _world.Map[curObject.Characteristics.X, curObject.Characteristics.Y].Ci));

					_objectsToDelete.Add(curObject);//Вносим умерших в список на удаление
				}
			}

			for (int i = 0; i < _objectsToDelete.Count; i++ )
			{
				_objects.Remove(_objectsToDelete[i]);//удаляем все умершие объекты из главного списка
			}

			_objectsToDelete.Clear();
			#endregion

			if (_objects.FindAll(a => a.Characteristics.Team != 0).GroupBy(a => a.Characteristics.Team).Count() == 1)
			{
				int winer = _objects.Find(a => a.Characteristics.Team != 0).Characteristics.Team;
				_eventsHistory.Add(new SystemEventCommandWin(winer));//Объявляем победителя
				_eventsHistory.Add(new SystemEventMatchEnd());//И матч заканчивается. Логично.

				BinaryFormatter bf = new BinaryFormatter();//Создаём буфер для сериализации
				MemoryStream tempStorage = new MemoryStream();//Здесь будем хранить сериализованную историю.
				bf.Serialize(tempStorage, _eventsHistory);
				//tempStorage.ToArray();//- Этот метод исключает неиспользуемые байты в MemoryStream из массива. //http://www.cyberforum.ru/csharp-net/thread35406.html
				
				//ToDo: Выгружать наш буфер в поток. Как мы это будем делать? По частям через сокет, напрямую одним куском?
				//Как-то закрываем молотилку.
			}
		}
		#region To Handler //Удалите это, если загрузку интеллекта из DLL уже перенесли в хэндлер.
		//Это должно быть перенесено в хэндлер:
		/*
		public void AddBeing(string _fullPath)
		{//Загрузка интерфейса отсюда: http://hashcode.ru/questions/108025/csharp-загрузка-dll-в-c-по-пользовательскому-пути
		    Assembly assembly = Assembly.LoadFrom(_fullPath);//вытаскиваем библиотеку
		    string iMyInterfaceName = typeof(IBeingInterface).ToString();
		    System.Reflection.TypeDelegator[] defaultConstructorParametersTypes = new System.Reflection.TypeDelegator[0];
		    object[] defaultConstructorParameters = new object[0];
			
		    IBeingInterface iAI = null;
			
		    foreach (System.Reflection.TypeDelegator type in assembly.GetTypes())
		    {
		        if (type.GetInterface(iMyInterfaceName) != null)
		        {
		            ConstructorInfo defaultConstructor = type.GetConstructor(defaultConstructorParametersTypes);
		            object instance = defaultConstructor.Invoke(defaultConstructorParameters);
		            iAI = instance as IBeingInterface;//Достаём таки нужный интерфейс
		        }
		    }
			
		    var newBeing = new Being(iAI);//Возможно, стоит перестраховаться, и написать проверку на не null.
		    _objects.Add(newBeing);
		}
		 */

		#endregion
	}		
}

