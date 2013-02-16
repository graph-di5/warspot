using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Entity;
using System.Collections;
using WarSpot.Contracts.Service;

namespace WarSpot.Cloud.Storage
{
   

    public static class Warehouse
    {

        

        #region BLOB SECTION VAR 
        private const string CONNECTIONSTRING = "DataConnectionString";
        private const string DBCONNECTIONSTRING = "DBConnectionString";

        private  static bool storageInitialized = false;
        private  static object gate = new Object();
        private  static CloudBlobClient blobStorage;
        private  static CloudBlobContainer container;

        #endregion BLOB SECTION END

        #region DATABASE SECTION VAR 
				public static DBContext db;
        #endregion DATABASE SECTION END      

        static Warehouse()
        {
            InitializeStorage();
        }


        #region BLOB METHODS 
        private static void InitializeStorage()
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

                    // раньше connectionString парсилась RoleEnviroment.GetConfiguration, но парсилась как-то.. 
                    // плохо. временно заменяю на прямое указание!
                    var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");

                    

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

                // раньше DbconnectionString для EntityConnection парсилась RoleEnviroment.GetConfiguration, но парсилась как-то.. 
                // плохо. временно заменяю на прямое указание!
                db = new DBContext(new EntityConnection(@"metadata=res://*/DBModel.csdl|res://*/DBModel.ssdl|res://*/DBModel.msl;provider=System.Data.SqlClient;provider connection string='data source=localhost\SQLEXPRESS;initial catalog=WarSpotDB;integrated security=True;multipleactiveresultsets=True;App=EntityFramework'")); // инициализируем базу данных
                
                

                storageInitialized = true;
            }
        }

        public static void UploadIntellect(Guid Account_ID, string name, byte[] data)
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

        public static void UploadReplay(byte[] replay, Guid gameID)
        {
            string uniqueBlobName = string.Format("replay{0}", gameID.ToString());
            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
            blob.UploadByteArray(replay);            
        }

        public static byte[] DownloadIntellect(Guid intellectID)
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

        public static Replay GetReplay(Guid gameID)
        {
            string neededname = string.Format("replay{0}", gameID.ToString());
            CloudBlockBlob blob = container.GetBlockBlobReference(neededname);
            return new Replay(gameID, blob.DownloadByteArray());
        }

        #endregion 

        #region DATABASE METHODS
        public static bool Register(string username, string password)
        {
            List<Account> test = (from b in db.Account
                      where b.Account_Name == username
                   select b).ToList<Account>();



            if (test.Any())
                return false;
            try
            {
                Account acc = Account.CreateAccount(Guid.NewGuid(), username, password);
                db.AddToAccount(acc);
                db.SaveChanges();
            }
            catch (Exception)
            {                
                throw;
            }

            
            return true;
        }

        public static bool Login(string username, string password)
        {
            return (from b in db.Account
                        where b.Account_Name == username && b.Account_Password == password
                        select b).Any();
        }

        public static string[] GetListOfIntellects(Guid userID)
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

        public static bool DeleteIntellect(string name, Guid userID)
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

        public static Guid? BeginMatch(List<Guid> intellects, Guid userID)
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


        public static List<Guid> GetListOfGames(Guid _userID)
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
