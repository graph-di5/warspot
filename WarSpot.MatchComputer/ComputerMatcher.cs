using System.Collections.Generic;
using WarSpot.Contracts.Intellect.Actions;
using System.Reflection;
using WarSpot.Contracts.Intellect;

namespace WarSpot.MatchComputer
{
	public class ComputerMatcher
	{
		private List<Being> _objects;
		private List<GameAction> _actions;//Сначала задаём действия, затем делаем их в нуном порядке.
		private List<GameAction> _didedActions; //для истории действий
		private ulong _step;

		public ComputerMatcher(List<string> _listDll)
		{
			_objects = new List<Being>();
			_actions = new List<GameAction>();
			_didedActions = new List<GameAction>();
			_step = 0;

			foreach (string _newDll in _listDll)
				AddBeing(_newDll);
		}

		public ComputerMatcher()
		{
			_objects = new List<Being>();
			_actions = new List<GameAction>();
			_didedActions = new List<GameAction>();
			_step = 0;
		}

		public void Update()
		{
			_actions.Clear();
			_didedActions.Clear();
			_step++;
			//Obtaining new actions from beings
			foreach (var curObject in _objects)
			{
				// todo send NOT NULL world info
				_actions.Add(curObject.Think(_step, curObject.Characteristics, null));
			}

			for (GameAction curAction = _actions[0]; curAction != null; curAction = _actions[0])
			{
				if (curAction.Cost() <= _objects.Find(a => a.Characteristics.Id == curAction.SenderId).Characteristics.Ci)
				{
					curAction.Execute();
					_didedActions.Add(curAction);
					_actions.Remove(curAction);
				}
			}
			
				_actions.Clear();
		}

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
    }		
}
