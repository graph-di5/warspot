using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.IO;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;
using WarSpot.MatchComputer;
namespace OfflineMatcher
{
	class OfflineMatcher
	{
		static void Main(string[] args)
		{
			string _directory;
			List<TeamIntellectList> _listIntellect = new List<TeamIntellectList>();
			TeamIntellectList _firstTeam = new TeamIntellectList();
			_firstTeam.Number = 1;
					
			Console.WriteLine("Enter the directory with you AI's: ");
			_directory = Console.ReadLine();
			DirectoryInfo dir = new DirectoryInfo(_directory);
			var _items = dir.GetFiles();
			Console.WriteLine("1");
			foreach (var item in _items)
			{
				Console.WriteLine(item.FullName);
				_firstTeam.Members.Add(ParseIntellect(item.FullName));
			}

			//Console.WriteLine(_firstTeam.Members.Count());
			Console.ReadLine();
			}

		
		public static IBeingInterface ParseIntellect(string _fullPath)
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
				    
		    return iAI;
		}
	}
}
