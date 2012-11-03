using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSpot.Cloud.Storage.Intellect
{
    public class IntellectDataContex : Microsoft.WindowsAzure.StorageClient.TableServiceContext
    {
        public IntellectDataContex(string baseAddress, Microsoft.WindowsAzure.StorageCredentials credentials)
            : base(baseAddress, credentials)
        {
        }

        public IQueryable<IntellectEntry> IntellectEntry
        {
            get
            {
                return this.CreateQuery<IntellectEntry>("IntellectEntry");
            }
        }

    }
}
