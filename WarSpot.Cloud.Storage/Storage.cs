using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Entity;
using System.Collections;

namespace WarSpot.Cloud.Storage
{
    public class Storage
    {
        #region BLOB SECTION VAR 
        private const string CONNECTIONSTRING = "UseDevelopmentStorage=true";

        private static bool storageInitialized = false;
        private static object gate = new object();
        private static CloudBlobClient blobStorage;
        private static CloudBlobContainer container;

        #endregion BLOB SECTION END

        #region DATABASE SECTION VAR 
        public DBContext db;
        #endregion DATABASE SECTION END


        public Storage()
        {
            this.InitializeStorage(); // инициализируем BLOB-хранилище
            db = new DBContext(); // инициализируем базу данных
        }

        #region BLOB METHODS 
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
                    var storageAccount = CloudStorageAccount.FromConfigurationSetting(CONNECTIONSTRING);

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
            db.AddToIntellect(Intellect.CreateIntellect(new System.Guid(), name, Account_ID));
            db.SaveChanges();            
            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
           
            blob.UploadByteArray(data);

        }

        #endregion 

        #region DATABASE METHODS
        public bool Register(string username, string password)
        {
            List<Account> test = (from b in db.Account
                        where b.Account_Name == username
                        select b).ToList<Account>();

            if (test.Count() == 0)
                return false; 
            else
            {
                db.AddToAccount(Account.CreateAccount(new System.Guid(), username, password));
                db.SaveChanges();
                return true;
            }
        }

        public bool Login(string username, string password)
        {
            var test = (from b in db.Account
                        where b.Account_Name == username
                        select b).FirstOrDefault();

            return test.Account_Password == password;
        }

        public string[] GetListOfIntellects(Guid userID)
        {
            
            List<string> result = new List<string>();
            
            var test = (from b in db.Intellect
                        where b.AccountAccount_ID == userID
                        select b).ToList<Intellect>();

            foreach (Intellect i in test)
            {
                result.Add(i.Intellect_Name);
            }

            return result.ToArray();
        }

        public bool DeleteIntellect(string name, Guid userID)
        {
            
            var test = (from b in db.Intellect
                        where b.AccountAccount_ID == userID
                        select b).ToList<Intellect>();

            var result = test.Where(b => b.Intellect_Name == name).FirstOrDefault<Intellect>();

            if (result != null)
            {
                string neededname = string.Format("{0}/{1}", userID.ToString(), name);
                CloudBlockBlob blob = container.GetBlockBlobReference(neededname);
                blob.Delete();                    

                db.Intellect.DeleteObject(result);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
            
        
    }
}
