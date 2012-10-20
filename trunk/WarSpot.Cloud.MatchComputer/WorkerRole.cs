using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace WarSpot.Cloud.MatchComputer
{
    public class WorkerRole : RoleEntryPoint
    {
		private CloudQueue queue;

        public override void Run()
        {
            // Это образец реализации рабочего процесса. Замените его собственной логикой.
            Trace.WriteLine("WarSpot.Cloud.MatchComputer entry point called", "Information");

            /*while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }*/

			var msg = queue.GetMessage();
			if (msg != null)
			{ 
				//обработка сообщения
				
				//а затем удаление
				queue.DeleteMessage(msg);
			}
        }

        public override bool OnStart()
        {
            // Задайте максимальное число одновременных подключений 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Дополнительные сведения по управлению изменениями конфигурации
            // см. раздел MSDN по ссылке http://go.microsoft.com/fwlink/?LinkId=166357.

			StorageCredentialsAccountAndKey accountAndKey = new StorageCredentialsAccountAndKey("account", "key");
			CloudStorageAccount account = new CloudStorageAccount(accountAndKey, true);
			CloudQueueClient client = account.CreateCloudQueueClient();
			CloudQueue queue = client.GetQueueReference("queue");
			queue.CreateIfNotExist();

            return base.OnStart();
        }
    }
}
