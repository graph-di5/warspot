using System;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using WarSpot.Common;
using WarSpot.Contracts.Intellect;
using WarSpot.MatchComputer;
using WarSpot.Cloud.Common;
using WarSpot.Cloud.Storage;

namespace WarSpot.Cloud.MatchComputer
{
	public class TaskHandler
	{
		readonly AutoResetEvent _are = new AutoResetEvent(false);
		Thread _thread;

		public void Start()
		{
			_thread = new Thread(new ThreadStart(ThreadFunctions));
			_thread.Start();
		}

		public void Stop(Thread thread)
		{
			_are.Set();
			thread.Join();
		}


		public static List<IBeingInterface> AddBeing(byte[] dll)
		{
			List<IBeingInterface> objects = new List<IBeingInterface>();
			Assembly assembly = Assembly.Load(dll);
			string iMyInterfaceName = typeof(IBeingInterface).ToString();
			TypeDelegator[] defaultConstructorParametersTypes = new TypeDelegator[0];
			object[] defaultConstructorParameters = new object[0];

			IBeingInterface iAI = null;

			foreach (Type type in assembly.GetTypes())
			{
				if (type.GetInterface(iMyInterfaceName) != null)
				{
					ConstructorInfo defaultConstructor = type.GetConstructor(defaultConstructorParametersTypes);
					object instance = defaultConstructor.Invoke(defaultConstructorParameters);
					iAI = (IBeingInterface)instance;//Достаём таки нужный интерфейс
					//
					objects.Add(iAI);
				}
			}
			return objects;
		}

		private static void ComputeMatch(List<TeamIntellectList> listIntellect, Message msg)
		{
			var computer = new Computer(listIntellect);
			var res = computer.Compute();
			Warehouse.UploadReplay(res, msg.ID);
		}

		private static List<TeamIntellectList> GetIntellects(Message msg)
		{
			var listIntellect = new List<TeamIntellectList>();

			foreach(var team in msg.TeamList)
			{
				var teamIntellectList = new TeamIntellectList
																	{
																		TeamId = team.TeamId,
																		Members = new List<Type>()
																	};
				foreach (var intellectId in team.Members)
				{
					var dll = Warehouse.DownloadIntellect(intellectId);
					teamIntellectList.Members.Add(ParseIntellect(dll));
				}
				listIntellect.Add(teamIntellectList);
			}
			return listIntellect;
		}

		public static Type ParseIntellect(byte[] dll)
		{
			Assembly assembly = Assembly.Load(dll);//вытаскиваем библиотеку
			var referencedAssemblies = assembly.GetReferencedAssemblies();

			var floudIntellect = false;
			foreach (var referencedAssembly in referencedAssemblies)
			{
				if (referencedAssembly.Name == "WarSpot.Contracts.Intellect")
				{

					if (referencedAssembly.Version.Major == Assembly.GetExecutingAssembly().GetName().Version.Major &&
						referencedAssembly.Version.Minor == Assembly.GetExecutingAssembly().GetName().Version.Minor)
					{
						floudIntellect = true;

					}
					break;
				}
			}
			if (!floudIntellect)
				return null;

			string iMyInterfaceName = typeof(IBeingInterface).ToString();
			foreach (var t in assembly.GetTypes())
			{
				if (t.GetInterface(iMyInterfaceName) != null)
				{
					return t;
				}
			}
			return null;
		}


		public void ThreadFunctions()
		{

			int timeout = 1000;

			while (true)
			{
				Message msg = Warehouse.GetMessage();
				//CloudQueueMessage msg = new CloudQueueMessage("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4 936DA01F-9ABD-4d9d-80C7-02AF85C822A8");

				_are.WaitOne(0);

				if (_are.WaitOne(1))
				{
					break;
					//return;
				}
				if (msg != null)
				{
					ComputeMatch(GetIntellects(msg), msg);
					continue;
				}

				else //(msg == null)
				{
					if (_are.WaitOne(timeout))
					{
						break;
					}
					else
					{

					}
				}
			}
		}
	}
}
