using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WarSpot.Cloud.Storage.Account
{
    public class AccountDataContext : Microsoft.WindowsAzure.StorageClient.TableServiceContext
    {
        public AccountDataContext(string baseAddress, Microsoft.WindowsAzure.StorageCredentials credentials) 
            : base(baseAddress, credentials)
        {
        }

        public IQueryable<AccountEntry> AccountEntry
        {
            get
            {
                return this.CreateQuery<AccountEntry>("AccountEntry");
            }
        }


    }
}