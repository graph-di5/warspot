using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using WarSpot.Contracts.Service;

namespace WarSpot.Cloud.Storage
{
    public class Storage
    {
        #region BLOB SECTION VAR 
        private const string CONNECTIONSTRING = "DataConnectionString";
        private const string DBCONNECTIONSTRING = "DBConnectionString";

        private  bool storageInitialized = false;
        private  object gate = new Object();
        private  CloudBlobClient blobStorage;
        private  CloudBlobContainer container;

        #endregion BLOB SECTION END

        #region DATABASE SECTION VAR 
				public DBContext db;
        #endregion DATABASE SECTION END


        public Storage()
        {
            this.InitializeStorage();
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

                try  // инициализируем BLOB-хранилище
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

                try
                {
                    db = new DBContext(new EntityConnection(RoleEnvironment.GetConfigurationSettingValue(DBCONNECTIONSTRING))); // инициализируем базу данных
                }
                catch (Exception)
                {
                    throw new Exception("Error with creating DBContext.");
                }
                

                storageInitialized = true;
            }
        }

        public void UploadIntellect(Guid Account_ID, string name, byte[] data)
        {
            if(db.Intellect == null)
            {
							Trace.WriteLine("db.Intellect == null");
            }

        	string uniqueBlobName = string.Format("intellects/{0}/{1}", Account_ID.ToString(), name);

            db.AddToIntellect(Intellect.CreateIntellect(Guid.NewGuid(), name, Account_ID));
            db.SaveChanges();            

            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);  
            blob.UploadByteArray(data);


        }

        public void UploadReplay(byte[] replay, Guid gameID)
        {
            string uniqueBlobName = string.Format("replay{0}", gameID.ToString());
            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
            blob.UploadByteArray(replay);            
        }

        public byte[] DownloadIntellect(Guid intellectID)
        {
            List<Intellect> test = (from b in db.Intellect
                                    where b.Intellect_ID == intellectID
                                    select b).ToList<Intellect>();

            List<Account> temp = (from b in db.Account
                                  where b.Account_ID == test.First<Intellect>().AccountAccount_ID
                                  select b).ToList<Account>();

            string neededname = string.Format("intellects/{0}/{1}", temp.First<Account>().Account_Name, test.First<Intellect>().Intellect_Name);
            CloudBlockBlob blob = container.GetBlockBlobReference(neededname);
            return blob.DownloadByteArray();
        }

        public Replay GetReplay(Guid gameID)
        {
            string neededname = string.Format("replay{0}", gameID.ToString());
            CloudBlockBlob blob = container.GetBlockBlobReference(neededname);
            return new Replay(gameID, blob.DownloadByteArray());
        }

        #endregion 

        #region DATABASE METHODS
        public bool Register(string username, string password)
        {
            List<Account> test = (from b in db.Account
                        where b.Account_Name == username
                        select b).ToList<Account>();

            if (test.Any())
                return false;
            try
            {
                var acc = Account.CreateAccount(Guid.NewGuid(), username, password);
                db.AddToAccount(acc);
                db.SaveChanges();
            }
            catch (Exception)
            {                
                throw;
            }

            
            return true;
        }

        public bool Login(string username, string password)
        {
            return (from b in db.Account
                        where b.Account_Name == username && b.Account_Password == password
                        select b).Any();
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
                string neededname = string.Format("intellects/{0}/{1}", userID.ToString(), name);
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

        public Guid? CreateGame(List<Guid> intellects, Guid userID)
        {
            Guid gameID = Guid.NewGuid();

            db.AddToGame(Game.CreateGame(gameID, userID));

            foreach (Guid intellect in intellects)
            {
                db.AddToGameIntellect(GameIntellect.CreateGameIntellect(gameID, intellect));
            }

            db.SaveChanges();

            return gameID;
        }
        #endregion


        public List<Guid> GetListOfGames(Guid _userID)
        {
            List<Game> test = (from b in db.Game
                       where b.AccountAccount_ID == _userID
                       select b).ToList<Game>();

            List<Guid> res = new List<Guid>();

            foreach (Game game in test)
            {
                res.Add(game.Game_ID);
            }

            return res;
        }
    }
}
