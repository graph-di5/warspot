using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSpot.Cloud.Storage.Intellect
{
    public class IntellectEntry : Microsoft.WindowsAzure.StorageClient.TableServiceEntity
    {


        public IntellectEntry(string _name, Guid _User_ID, Guid _Blob_ID)
        {
            PartitionKey = DateTime.UtcNow.ToString("MMddyyyy");
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
            this.DLL_Name = _name;
            this.Intellect_ID = Guid.NewGuid();
            this.Blob_ID = _Blob_ID;
            this.User_ID = _User_ID;

        }

        public string DLL_Name { get; set; }
        public Guid Blob_ID { get; set; }
        public Guid Intellect_ID { get; set; }
        public Guid User_ID { get; set; }


    }
}
