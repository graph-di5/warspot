using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace WarSpot.Cloud.MatchComputer
{
	class Service
	{
		public CloudQueueMessage GetMsg()
		{
			StorageCredentialsAccountAndKey accountAndKey = new StorageCredentialsAccountAndKey("account", "key");
			CloudStorageAccount account = new CloudStorageAccount(accountAndKey, true);
			CloudQueueClient client = account.CreateCloudQueueClient();
			CloudQueue queue = client.GetQueueReference("events");
			queue.CreateIfNotExist();

			CloudQueueMessage msg = queue.GetMessage();
			return msg;
		}
	}
}
