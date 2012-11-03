using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace WarSpot.Cloud.MatchComputer
{
	internal class TaskHandler
	{
		AutoResetEvent are = new AutoResetEvent(false);

		public void Start()
		{
			Thread thread = new Thread(new ThreadStart(ThreadFunctions));
			thread.Start();
		}

		public void Stop(Thread thread)
		{
			are.Set();
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

		public void ThreadFunctions()
		{
			CloudQueue queue = CreateQueue();
			int timeout = 1000;

			while (true)
			{
				CloudQueueMessage msg = queue.GetMessage();
				are.WaitOne(0);

				if (are.Set())
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
					are.WaitOne(timeout);
				}
			}
		}
	}
}
