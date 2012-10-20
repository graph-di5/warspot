using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace WarSpot.Cloud.UserService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

		public void addMsg(CloudQueueMessage msg)
		{
			StorageCredentialsAccountAndKey accountAndKey = new StorageCredentialsAccountAndKey("account", "key");
			CloudStorageAccount account = new CloudStorageAccount(accountAndKey, true);
			CloudQueueClient client = account.CreateCloudQueueClient();
			CloudQueue queue = client.GetQueueReference("events");
			queue.CreateIfNotExist();

			queue.AddMessage(msg);
		}
    }
}
