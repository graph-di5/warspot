﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Entity;

namespace WarSpot.Cloud.Storage
{
    public class Storage
    {
        private static bool storageInitialized = false;
        private static object gate = new object();

        private static CloudBlobClient blobStorage;

        private static CloudBlobContainer container;

        public Storage()
        {
            this.InitializeStorage();
        }

        private void InitializeStorage()
        {
            if (storageInitialized)
            {
                return;
            }

            lock (gate)
            {
                if (storageInitialized)
                {
                    return;
                }

                try
                {
                    // read account configuration settings
                    var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

                    // create blob container for images
                    blobStorage = storageAccount.CreateCloudBlobClient();
                    container = blobStorage.GetContainerReference("intellects");
                    container.CreateIfNotExist();

                    // configure container for public access
                    var permissions = container.GetPermissions();
                    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                    container.SetPermissions(permissions);
                }
                catch (WebException)
                {
                    throw new WebException("Storage services initialization failure. "
                       + "Check your storage account configuration settings. If running locally, "
                       + "ensure that the Development Storage service is running.");
                }

                storageInitialized = true;
            }
        }

        public void Upload(Guid Account_ID, string name, byte[] data)
        {

            string uniqueBlobName = string.Format("{0}/{1}", Account_ID.ToString(), name);
            
            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
            
            blob.UploadByteArray(data);

        }

    }
}
