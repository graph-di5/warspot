﻿using System;
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
			TeamIntellectList _secondTeam = new TeamIntellectList();
			_firstTeam.Number = 1;
			_secondTeam.Number = 2;

			Console.WriteLine("Enter the directory with first team's AIs: ");
			_directory = Console.ReadLine();
			DirectoryInfo dir1 = new DirectoryInfo(_directory);
			var _items1 = dir1.GetFiles();
			Console.WriteLine("DLL for team 1: ");
			foreach (var item in _items1)
			{
				Console.WriteLine(item.FullName);
				_firstTeam.Members.Add(ParseIntellect(item.FullName));
			}

			Console.WriteLine("Enter the directory with second team's AIs: ");
			_directory = Console.ReadLine();
			DirectoryInfo dir2 = new DirectoryInfo(_directory);
			var _items2 = dir2.GetFiles();
			Console.WriteLine("DLL for team 2: ");
			foreach (var item in _items2)
			{
				Console.WriteLine(item.FullName);
				_secondTeam.Members.Add(ParseIntellect(item.FullName));
			}

			Console.WriteLine("Enter the directory for serialized match history: ");
			_directory = Console.ReadLine();

			Console.WriteLine("First team: {0}. Second team: {1}.",_firstTeam.Members.Count(), _secondTeam.Members.Count());
			Console.WriteLine("Computing...");
			FileStream fs = new FileStream(_directory + "file.s", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			var _matcher = new ComputerMatcher(_listIntellect, fs);
			while (_matcher.Update() != 0) ;//Гоняем update пока кто-то не выиграет.
			fs.Close();
			Console.WriteLine("All done. Take your match history in: {0}", _directory);
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
