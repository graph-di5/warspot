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
		private List<Being> _objects;
		private List<GameAction> _actions;//Сначала задаём действия, затем делаем их в нуном порядке.
		private List<GameAction> _doneActions; //для истории действий
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

		/// <summary>
		///Загрузка всех объектов списками команд
		/// </summary>
		public ComputerMatcher(List<TeamIntellectList> _listIntellect, StreamWriter _stream)
		{
			_objects = new List<Being>();
			_actions = new List<GameAction>();
			_doneActions = new List<GameAction>();
			_step = 0;
			//_stream = new Stream() //создание какого то потока для сериализации ивентов
			_formatter = new BinaryFormatter();
			_world = new World();

			int c = 5; //Константа площади начального расположения, не знаю, куда ей запихнуть 
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
					var newBeing = new Being(Team.Members[i]);
					newBeing.Characteristics.Coordinates = new XNA.Framework.Vector2(pos[i].Item1, pos[i].Item2);
					_world.Map[pos[i].Item1, pos[i].Item2].Being = newBeing;
					// todo придумать 
					newBeing.Construct(Team.Number, 0, 10000, null);
					
					_objects.Add(newBeing);
				}
			}
		}

		public void Update()
		{
			_actions.Clear();
			_doneActions.Clear();
			_step++;
			//Obtaining new actions from beings
			foreach (var curObject in _objects)
			{
				// todo send NOT NULL world info
				
				_actions.Add(curObject.Think(_step, curObject.Characteristics, null));
			}

			for (GameAction curAction = _actions[0]; curAction != null; curAction = _actions[0])
			{
				_formatter.Serialize(_stream, curAction); //Сериализация ивента

				if (curAction.Cost() <= _objects.Find(a => a.Characteristics.Id == curAction.SenderId).Characteristics.Ci)
				{
					curAction.Execute();
					_doneActions.Add(curAction);
					_actions.Remove(curAction);
				}
			}

			var _objectsToDelete = new List<Being>();//Список объектов на удаление

			foreach (var curObject in _objects)//проверяем мёртвых
			{
				if (curObject.Characteristics.Health <= 0)
				{
					var _die = new GameActionDie(curObject.Characteristics.Id);//Пишем от его имени действие смерти.
					_die.Execute();//Выполняем его (выброс энергии из трупа, к примеру)
					_doneActions.Add(_die);//генерируем в историю событие смерти. ToDo: решить, здесь его генерировать, или же внутри execute
					_objectsToDelete.Add(curObject);//Вносим умерших в список на удаление
				}
			}

			for (int i = 0; i < _objectsToDelete.Count; i++ )
			{
				_objects.Remove(_objectsToDelete[i]);//удаляем все умершие объекты из главного списка
			}

			_objectsToDelete.Clear();

			if (_objects.FindAll(a => a.Characteristics.Team != 0).GroupBy(a => a.Characteristics.Team).Count() == 1)
			{
				int winer = _objects.Find(a => a.Characteristics.Team != 0).Characteristics.Team;
				//Генерируем здесь сообщение о победе, пишем это как-то в историю, если надо.
				//Закрываем молотилку.
			}
		}

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
    }		
}
