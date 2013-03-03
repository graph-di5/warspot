using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.Autoscaling;

namespace WarSpot.Cloud.MatchComputer
{
	public class WorkerRole : RoleEntryPoint
	{
		private CloudQueue _queue;

		//private bool
		readonly AutoResetEvent _are = new AutoResetEvent(false);
		private Autoscaler autoscaler;

		public override void Run()
		{
			int timeout = 1000;

			// Это образец реализации рабочего процесса. Замените его собственной логикой.
			Trace.WriteLine("WarSpot.Cloud.Computer entry point called", "Information");

#if false
					this.autoscaler = EnterpriseLibraryContainer.Current.GetInstance<Autoscaler>();
            this.autoscaler.Start();
#endif
			/*while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }*/

			//todo rewrite this

			TaskHandler _handler = new TaskHandler();

			_handler.Start();
			while (true)
			{
				_are.WaitOne(timeout);
				/*
var msg = _queue.GetMessage();
if (msg != null)
{
	//обработка сообщения

	//а затем удаление
	// we need to delete message before it'll apeared in queue 'cause timeout 
	_queue.DeleteMessage(msg);

	// todo start here the match
}
Thread.Sleep(100);*/
			}

		}

		public override bool OnStart()
		{

			CloudStorageAccount.SetConfigurationSettingPublisher(
																	(a, b) => b(RoleEnvironment.GetConfigurationSettingValue(a)));

			// Задайте максимальное число одновременных подключений 
			ServicePointManager.DefaultConnectionLimit = 12;

			// Дополнительные сведения по управлению изменениями конфигурации
			// см. раздел MSDN по ссылке http://go.microsoft.com/fwlink/?LinkId=166357.

			//StorageCredentialsAccountAndKey accountAndKey = new StorageCredentialsAccountAndKey("account",
			//  System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("key")));
			CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
			CloudQueueClient client = account.CreateCloudQueueClient();
			_queue = client.GetQueueReference("queue");
			_queue.CreateIfNotExist();

			return base.OnStart();
		}
	}
}
