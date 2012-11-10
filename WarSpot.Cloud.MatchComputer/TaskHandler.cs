using System.Threading;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace WarSpot.Cloud.MatchComputer
{
	internal class TaskHandler
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
					if(_are.WaitOne(timeout))
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
