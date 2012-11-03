using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;

namespace WarSpot.Cloud.Storage.Account
{
    public class AccountEntry : Microsoft.WindowsAzure.StorageClient.TableServiceEntity

    {


        public AccountEntry(string _name, string _pass)
        {             
            PartitionKey = DateTime.UtcNow.ToString("MMddyyyy");
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
            this.User_ID = Guid.NewGuid(); // или лучше будет: = new Guid(username); ?
            this.Name = _name;
            this.Pass = _pass;
        }

        public string Name {get; set;}
        public string Pass { get; set; }
        public Guid User_ID { get; set; }


    }
}