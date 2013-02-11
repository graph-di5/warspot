using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
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

		private CloudQueue CreateQueue()
		{
			StorageCredentialsAccountAndKey accountAndKey = new StorageCredentialsAccountAndKey("account", "key");
			CloudStorageAccount account = new CloudStorageAccount(accountAndKey, true);
			CloudQueueClient client = account.CreateCloudQueueClient();
			CloudQueue queue = client.GetQueueReference("queue");
			queue.CreateIfNotExist();
			return queue;
		}

		public static List<IBeingInterface> AddBeing(byte[] dll)
		{
			List<IBeingInterface> _objects = new List<IBeingInterface>();
			Assembly assembly = Assembly.Load(dll);
			string iMyInterfaceName = typeof(IBeingInterface).ToString();
			System.Reflection.TypeDelegator[] defaultConstructorParametersTypes = new System.Reflection.TypeDelegator[0];
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

		private static void MemoryStreamer(List<TeamIntellectList> listIntellect)
		{
			Stream stream = new MemoryStream();
            // todo: вынести stream из конструктора в параметр метода
			WarSpot.MatchComputer.Computer computer = new WarSpot.MatchComputer.Computer(listIntellect, stream);
            computer.Compute();
            Warehouse.Start();

            Guid gameID = Guid.NewGuid();
            Warehouse.UploadReplay(ReadFully(stream), gameID);

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



		private static Message ParseMessage(CloudQueueMessage message)
		{
			Message msg = new Message();
		    Stream stream = new MemoryStream(message.AsBytes);

		    var bf = new BinaryFormatter();
		    msg = (Message) bf.Deserialize(stream);

            #region oldVersion// old version. to delete
            /*
			// получаем из сообщения из очереди имена интеллектов
			// надо разделять имена интеллектов в сообщении каким-то стандартным сепаратором, например, пробелом
			List<Guid> namesOfIntellects = new List<Guid>();

			string content = message.AsString;
			int position = 0;
			string guidName = "";
			while (position <= content.Length)
			{
				if (position == content.Length || Char.IsSeparator(content[position]))
				{
					Guid guid = new Guid(guidName);
					namesOfIntellects.Add(guid);
					guidName = "";
					position++;
					continue;
				}
				guidName += content[position++];
			}
			// 
			msg.ID = Guid.NewGuid();
			msg.ListOfDlls = namesOfIntellects;
            */
#endregion

			return msg;
		}

		private List<TeamIntellectList> getIntellects(Message msg)
		{
			List<Guid> namesOfIntellects = msg.ListOfDlls;
			List<byte[]> intellects = new List<byte[]>();

			// получаем список интеллектов как список массивов байтов
			foreach (var name in namesOfIntellects)
			{
                Warehouse.Start();
				intellects.Add(Warehouse.DownloadIntellect(name));
			}

			// а вот тут надо переделать
			List<TeamIntellectList> listIntellect = new List<TeamIntellectList>();
			TeamIntellectList teamIntellectList = new TeamIntellectList();

			foreach (var dll in intellects)
			{
				teamIntellectList.Members.Add(ParseIntellect(dll));
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
			CloudQueue queue = CreateQueue();
			int timeout = 1000;

			while (true)
			{
				CloudQueueMessage msg = queue.GetMessage();
				//CloudQueueMessage msg = new CloudQueueMessage("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4 936DA01F-9ABD-4d9d-80C7-02AF85C822A8");
			
				_are.WaitOne(0);

				if (_are.WaitOne(1))
				{
					break;
					//return;
				}
				if (msg != null)
				{
					MemoryStreamer(getIntellects(ParseMessage(msg)));
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
