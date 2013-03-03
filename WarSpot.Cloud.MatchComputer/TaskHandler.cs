using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Reflection;
using WarSpot.Contracts.Intellect;
using System.IO;
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
			List<IBeingInterface> _objects = new List<IBeingInterface>();
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
					_objects.Add(iAI);
				}
			}
			return _objects;
		}

		private static void MemoryStreamer(List<TeamIntellectList> listIntellect, Message msg)
		{
			var stream = new MemoryStream();
			// todo: вынести stream из конструктора в параметр метода
			Computer computer = new Computer(listIntellect, stream);
			computer.Compute();

			Warehouse.UploadReplay(stream.ToArray(), msg.ID);

			stream.Dispose();
		}

		public static byte[] ReadFully(Stream input)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}





		private List<TeamIntellectList> getIntellects(Message msg)
		{
			// todo а вот тут надо переделать; но уже лучше (DI)
			var listIntellect = new List<TeamIntellectList>();

			var teamNumber = 0;
			// получаем список интеллектов как список массивов байтов
			foreach (var dll in msg.ListOfDlls.Select(id => Warehouse.DownloadIntellect(id)).ToList())
			{
				var teamIntellectList = new TeamIntellectList
																	{
																		Number = ++teamNumber,
																		Members = new List<Type>()
																		          	{
																		          		ParseIntellect(dll)
																		          	}
																	};
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
#if false
					var defaultCtor = t.GetConstructor(new Type[0]);
					if (defaultCtor != null)
					{
						var inst = defaultCtor.Invoke(new Type[0]);
						return inst as IBeingInterface;
					}
#endif
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
					MemoryStreamer(getIntellects(msg), msg);
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
