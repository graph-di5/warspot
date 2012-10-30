using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using WarSpot.Cloud.Storage.Account;
using System.IO;
using System.Net;



namespace WarSpot.Cloud.Storage.Account
{
	public class AccountDataSource
	{
		private static CloudStorageAccount storageAccount; // БД со списком аккаунтов
		private AccountDataContext context;


        private static CloudBlobClient blobStorage; // BLOB-база c DLL
        private static bool storageInitialized = false;
        private static object gate = new object();

		static AccountDataSource()
		{
			//CloudStorageAccount.SetConfigurationSettingPublisher(
			//          (a, b) => b(RoleEnvironment.GetConfigurationSettingValue(a)));

			storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

			CloudTableClient.CreateTablesFromModel(
			typeof(AccountDataContext),
			storageAccount.TableEndpoint.AbsoluteUri,
			storageAccount.Credentials);
		}

		public AccountDataSource()
		{
			this.context = new AccountDataContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
			this.context.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(1));
		}

        public bool UploadDLL(string username, string path)
        {
            InitializeStorage();

            // Загружаем DLL-ку в BLOB-базу в папку userdll/имяюзера/имяюзера.DLL(*)
            string uniqueBlobName = string.Format("userdll/{0}/{0}", username);
            CloudBlockBlob blob = blobStorage.GetBlockBlobReference(uniqueBlobName);
            blob.Properties.ContentType = "dll"; // (*)
            blob.UploadFile(path);
            System.Diagnostics.Trace.TraceInformation("Uploaded dll '{0}' to blob storage as '{1}'", path, uniqueBlobName);

            // Выбираем запись в БД со списком аккаунтов и обновляем в нужном аккаунты ссылку на его DLL-ку
            var results = (from g in this.context.AccountEntry select g).ToList();
            var entry = results.Where(g => g.Name == username).FirstOrDefault<AccountEntry>();
            entry.UpdateDLL(blob.Uri.ToString());
            
            return true;
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

                    // create blob container
                    blobStorage = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobStorage.GetContainerReference("userdll");
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


		public bool AddAccountEntry(AccountEntry newItem) // создаем новый аккаунт
		{
			var results = (from g in this.context.AccountEntry select g).ToList(); // посмотрим, что у нас уже лежит
			var entry = results.Where(g => g.Name == newItem.Name).FirstOrDefault<AccountEntry>(); // посмотрим, нет ли аккаунта с таким же
			// именем

			if (entry != null) // если есть
			{
				return false; // false!!!
			}

			else // если нет
			{
				this.context.AddObject("AccountEntry", newItem); // создаем новую запись в тамблице
				this.context.SaveChanges();
				return true;
			}
		}

		public bool CheckAccountEntry(string username, string pass) // проверяем на совпадение имени\пароля
		{
			var results = (from g in this.context.AccountEntry select g).ToList();
			var entry = results.Where(g => g.Name == username).FirstOrDefault<AccountEntry>();

			if (entry != null) // нашли аккаунт с нужным именем
			{
				if (entry.Pass == pass) // проверяем правильность пароля
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else // не нашли аккаунт с нужным именем
			{
				return false;
			}

		}



	}
}