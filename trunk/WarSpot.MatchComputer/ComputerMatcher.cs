using System.Collections.Generic;
using WarSpot.Contracts.Intellect.Actions;
using System.Reflection;
using WarSpot.Contracts.Intellect;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WarSpot.MatchComputer
{
	public class TeamIntellectList //Класс для загрузки команд с их ДЛЛками.
	{
		public int Number { set; get ;}
		public List<IBeingInterface> Members;
	}

	public class ComputerMatcher
	{
		private List<int> _teamsMembersCounter; 
		private List<Being> _objects;
		private List<GameAction> _actions;//Сначала задаём действия, затем делаем их в нуном порядке.
		private List<GameAction> _doneActions; //для истории действий
		private ulong _step;
		private Stream _stream;
		private BinaryFormatter _formatter;

		public ComputerMatcher ()
		{
			_objects = new List<Being>();
			_actions = new List<GameAction>();
			_doneActions = new List<GameAction>();
			_step = 0;
			_teamsMembersCounter = new List<int>();
			//_stream = new Stream() //создание какого то потока для сериализации ивентов
			_formatter = new BinaryFormatter();
		}
		
		/// <summary>
		/// Номера команд ОБЯЗАНЫ быть упорядоченны и начинаться с нуля [0,1,2,...]
		/// </summary>
		public ComputerMatcher(List<TeamIntellectList> _listIntellect)
		{
			foreach(TeamIntellectList Team in _listIntellect)
			{
				_teamsMembersCounter.Add(0);//Добавляем новую команду
				foreach (IBeingInterface _newIntellect in Team.Members)
				{
					var newBeing = new Being(_newIntellect);
					_teamsMembersCounter[Team.Number]++;
					_objects.Add(newBeing);
				}
			}
		}

		//public ComputerMatcher(List<string> _listDll)
		//{
		//    _objects = new List<Being>();
		//    _actions = new List<GameAction>();
		//    _didedActions = new List<GameAction>();
		//    _step = 0;

		//    foreach (string _newDll in _listDll)
		//        AddBeing(_newDll);
		//}

		//public ComputerMatcher()
		//{
		//    _objects = new List<Being>();
		//    _actions = new List<GameAction>();
		//    _didedActions = new List<GameAction>();
		//    _step = 0;
		//}

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

			foreach (var curObject in _objects)//проверяем мёртвых
			{
				if (curObject.Characteristics.Health <= 0)
				{
					var _die = new GameActionDie(curObject.Characteristics.Id);//Пишем от его имени действие смерти.
					_die.Execute();//Выполняем его (выброс энергии из трупа, к примеру)
					_doneActions.Add(_die);
					_objects.Remove(curObject);
				}
			}			
		}

		//public void AddBeing(string _fullPath)
		//{//Загрузка интерфейса отсюда: http://hashcode.ru/questions/108025/csharp-загрузка-dll-в-c-по-пользовательскому-пути
		//    Assembly assembly = Assembly.LoadFrom(_fullPath);//вытаскиваем библиотеку
		//    string iMyInterfaceName = typeof(IBeingInterface).ToString();
		//    System.Reflection.TypeDelegator[] defaultConstructorParametersTypes = new System.Reflection.TypeDelegator[0];
		//    object[] defaultConstructorParameters = new object[0];
			
		//    IBeingInterface iAI = null;
			
		//    foreach (System.Reflection.TypeDelegator type in assembly.GetTypes())
		//    {
		//        if (type.GetInterface(iMyInterfaceName) != null)
		//        {
		//            ConstructorInfo defaultConstructor = type.GetConstructor(defaultConstructorParametersTypes);
		//            object instance = defaultConstructor.Invoke(defaultConstructorParameters);
		//            iAI = instance as IBeingInterface;//Достаём таки нужный интерфейс
		//        }
		//    }
			
		//    var newBeing = new Being(iAI);//Возможно, стоит перестраховаться, и написать проверку на не null.
		//    _objects.Add(newBeing);
		//}
    }		
}
