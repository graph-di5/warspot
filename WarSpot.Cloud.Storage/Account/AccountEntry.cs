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
            // Row key allows sorting, so we make sure the rows come back in time order
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
            this.Name = _name;
            this.Pass = _pass;
        }



        public string Name;
        public string Pass;

    }
}