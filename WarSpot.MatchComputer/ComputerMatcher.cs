using System.Collections.Generic;
using WarSpot.Contracts.Intellect.Actions;
using System.Reflection;
using WarSpot.Contracts.Intellect;

namespace WarSpot.MatchComputer
{
	class ComputerMatcher
	{
		private List<Being> _objects;
		private List<GameAction> _actions;
		private ulong _step;

		ComputerMatcher()
		{
			_objects = new List<Being>();
			_actions = new List<GameAction>();
			_step = 0;
		}

		public void Update()
		{
			_actions.Clear();
			_step++;
			//Obtaining new actions from beings
			foreach (var curObject in _objects)
			{
				// todo send NOT NULL world info
				_actions.Add(curObject.Think(_step, curObject.Characteristics, null));
			}

			//Doing somethink with received actions
			/*
			foreach (var curAction in _actions)
			{
				curAction.Execute();
				// todo save action if succeded
			}
			*/

			_actions.Clear();
		}

		public void AddBeing(string _fullPath)
		{//Загрузка интерфейса отсюда: http://hashcode.ru/questions/108025/csharp-загрузка-dll-в-c-по-пользовательскому-пути
			Assembly assembly = Assembly.LoadFrom(_fullPath);//вытаскиваем библиотеку
			string iMyInterfaceName = typeof(IBeingInterface).ToString();
			System.Reflection.TypeDelegator[] defaultConstructorParametersTypes = new System.Reflection.TypeDelegator[0];
			object[] defaultConstructorParameters = new object[0];
			
			IBeingInterface iAI;
			
			foreach (System.Reflection.TypeDelegator type in assembly.GetTypes())
			{
				if (type.GetInterface(iMyInterfaceName) != null)
				{
					ConstructorInfo defaultConstructor = type.GetConstructor(defaultConstructorParametersTypes);
					object instance = defaultConstructor.Invoke(defaultConstructorParameters);
					iAI = instance as IBeingInterface;//Достаём таки нужный интерфейс
				}
			}
			
			//newBeing = new Being(iAI);
			//_objects.Add(newMob);
        }
    }		
}
