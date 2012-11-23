using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Reflection;
using WarSpot.Contracts.Intellect;
//using WarSpot.MatchComputer;

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

		// заглушка выдирания интеллекта
		public static byte[] GetIntellect(string name)
		{
			// Setup the connection to Windows Azure Storage
			string connectionString = "myConnectionString";
			var storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(connectionString));
			var blobClient = storageAccount.CreateCloudBlobClient();
			// Get and create the container
			var blobContainer = blobClient.GetContainerReference(name);
			blobContainer.CreateIfNotExist();
			// upload a text blob
			var blob = blobContainer.GetBlobReference(Guid.NewGuid().ToString());
			blob.UploadText("Hello Windows Azure");


			byte[] dll = blob.DownloadByteArray();
			return dll;
		}

		public void AddBeing(byte[] dll)
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

			#region
			//Загрузка интерфейса отсюда: http://hashcode.ru/questions/108025/csharp-загрузка-dll-в-c-по-пользовательскому-пути
			//Assembly assembly = Assembly.LoadFrom(_fullPath);//вытаскиваем библиотеку
			/*string iMyInterfaceName = typeof(IBeingInterface).ToString();
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
		*/
			//var newBeing = new Being(iAI);//Возможно, стоит перестраховаться, и написать проверку на не null.
			//_objects.Add(newBeing);
			#endregion
		}

		public void SerializeData()
		{

		}
		
		public void ThreadFunctions()
		{
			CloudQueue queue = CreateQueue();
			int timeout = 1000;

			while (true)
			{
				CloudQueueMessage msg = queue.GetMessage();
				_are.WaitOne(0);

				if (_are.WaitOne(1))
				{
					break;
					//return;
				}
				if (msg != null)
				{
					// calculate in matchcomputer

					continue;
				}

				if (msg == null)
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
