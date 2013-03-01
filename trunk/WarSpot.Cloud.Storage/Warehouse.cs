﻿using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using WarSpot.Contracts.Service;
using WarSpot.Cloud.Common;
using System.IO;

namespace WarSpot.Cloud.Storage
{
    public static class Warehouse
    {

        #region AZURE SECTION VAR
        private const string CONNECTIONSTRING = "DataConnectionString";
        private const string DBCONNECTIONSTRING = "DBConnectionString";

        private static bool storageInitialized = false;
        private static object gate = new Object();

        private static CloudBlobClient blobStorage;
        private static CloudBlobContainer container;

        private static CloudQueue queue;
        private static CloudQueueClient queueClient;

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

                    // create queue for matches
			        queueClient = storageAccount.CreateCloudQueueClient();
			        queue = queueClient.GetQueueReference("queue");
			        queue.CreateIfNotExist();

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

        public static bool UploadIntellect(Guid Account_ID, string name, byte[] data)
        {
            if (db.Intellect == null)
            {
                Trace.WriteLine("db.Intellect == null");
            }

            string uniqueBlobName = string.Format("intellects/{0}/{1}", Account_ID.ToString(), name);

            if ((from i in db.Intellect
                 where i.Intellect_Name == name
                 select i).Any())
                return false;

            db.AddToIntellect(Intellect.CreateIntellect(Guid.NewGuid(), name, Account_ID));
            db.SaveChanges();

            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
            blob.UploadByteArray(data);

            return true;

        }      

        public static byte[] DownloadIntellect(Guid intellectID)
        {
            string name1 = (from b in db.Intellect
                                    where b.Intellect_ID == intellectID
                                    select b.Intellect_Name).First<string>();

            Guid name2 = (from a in db.Account
                            where a.Account_ID == ((from b in db.Intellect
                                                    where b.Intellect_ID == intellectID
                                                    select b.AccountAccount_ID).FirstOrDefault<Guid>())
                            select a.Account_ID).First<Guid>();


            string neededname = string.Format("intellects/{0}/{1}", name2.ToString(), name1);
            CloudBlockBlob blob = container.GetBlockBlobReference(neededname);



            return blob.DownloadByteArray();
        }

        public static Replay GetReplay(Guid gameID)
        {
            try
            {
                string neededname = (from g in db.Game
                                     where g.Game_ID == gameID
                                     select g.Replay).FirstOrDefault<string>();
                if (neededname != null)
                {
                    CloudBlockBlob blob = container.GetBlockBlobReference(neededname);
                    return new Replay(gameID, blob.DownloadByteArray());
                }
                else // Replay has not been uploaded yet.
                    return null;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ErrorCode UploadReplay(byte[] replay, Guid gameID)
        {
            try
            {
                Guid replayID = Guid.NewGuid();
                string uniqueBlobName = string.Format("{0}", replayID.ToString());
                CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
                blob.UploadByteArray(replay);

                Game game = (from g in db.Game
                                   where g.Game_ID == gameID
                                   select g).FirstOrDefault<Game>();

                game.Replay = uniqueBlobName;
                db.SaveChanges();

                return new ErrorCode(ErrorType.Ok, "Replay has been uploaded.");
            }
            catch (Exception e)
            {
                return new ErrorCode(ErrorType.UnknownException, "Blob problems: \n" + e.ToString());
            }

        }

        public static List<ReplayDescription> GetListOfReplays(Guid Account_ID)
        {
            try
            {
                List<ReplayDescription> userreplays = new List<ReplayDescription>();

                List<Game> usergames = (from g in db.Game
                                        where g.AccountAccount_ID == Account_ID
                                        select g).ToList<Game>();

                foreach (Game g in usergames)
                {
                    List<string> intellectsingame = (from i in db.Intellect
                                                     where i.Games.Contains(g)
                                                     select i.Intellect_Name).ToList<string>();

                    ReplayDescription replay = new ReplayDescription(g.Game_ID, g.Game_Name, intellectsingame);

                    userreplays.Add(replay);
                }

                return userreplays;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        #endregion

        #region QUEUE METHODS

        private static void PutMessage(Message message)
        {
            queue.AddMessage(new CloudQueueMessage(message.ToString()));
        }

        public static Message GetMessage()
        {
            CloudQueueMessage queuemessage = queue.GetMessage();
            Message msg = ParseMessage(queuemessage);
            // TODO: Пересмотреть удаление сообщений
            queue.DeleteMessage(queuemessage);
            return msg;
        }

        private static Message ParseMessage(CloudQueueMessage message)
        {
            Message msg = new Message();
            string queuemessage = message.AsString;
            String temp = "";
            bool flag = false;

            for (int i = 0; i < queuemessage.Length; i++)
            {
                if (flag)
                {
                    while (queuemessage[i] != ' ')
                    {
                        temp = temp + queuemessage[i];
                        i++;
                    }
                    msg.ListOfDlls.Add(Guid.Parse(temp));
                    temp = "";

                    try
                    {
                        i++;
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        break;
                    }
                }
                else
                {
                    while (queuemessage[i] != ' ')
                    {
                        temp = temp + queuemessage[i];
                        i++;
                    }
                    msg.ID = Guid.Parse(temp);
                    flag = true;
                    i++;
                    temp = "";
                }
            }

            return msg;
        }


        #endregion

        #region DATABASE METHODS

        #region register & login

        public static bool Register(string username, string password)
        {
            List<Account> test;
            
            if ((test = (from b in db.Account
                         where b.Account_Name == username
                         select b).ToList<Account>()).Any())
                return false;
            try
            { 
                db.AddToAccount(Account.CreateAccount(Guid.NewGuid(), username, password));
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool Login(string username, string password)
        {
            return (from b in db.Account
                    where b.Account_Name == username && b.Account_Password == password
                    select b).Any();
        }

        #endregion

        #region intellects
        public static List<KeyValuePair<Guid,string>> GetListOfIntellects(Guid userID)
        {
            /*
            Old method realisation: GetListOfIntellects returns list of intellects of logged in user.
             * 
            List<KeyValuePair<Guid, string>> result = new List<KeyValuePair<Guid, string>>();

            var test = (from b in db.Intellect
                        where b.AccountAccount_ID == userID
                        select b).ToList<Intellect>();

            foreach (Intellect i in test)
            {
                result.Add(new KeyValuePair<Guid, string> ( i.Intellect_ID, i.Intellect_Name ));
            }

            return result;
             */

            // Temporary realisation: GetListOfIntellect return list of ALL intellects in the system.
            List<KeyValuePair<Guid, string>> result = new List<KeyValuePair<Guid, string>>();

            var test = (from b in db.Intellect
                        select b).ToList<Intellect>();

            foreach (Intellect i in test)
            {
                result.Add(new KeyValuePair<Guid, string>(i.Intellect_ID, i.Intellect_Name));
            }

            return result;
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



        #endregion

        #region match

        public static Guid? BeginMatch(List<Guid> intellects, Guid userID, string title)
        {
            Guid gameID = Guid.NewGuid();
            Game match = Game.CreateGame(gameID, userID, DateTime.Now.ToString(), title);         

            List<Intellect> listofintellects = new List<Intellect>();

            foreach (Guid id in intellects)
            {
                Intellect currentIntellect = (from i in db.Intellect
                                              where i.Intellect_ID == id
                                              select i).FirstOrDefault<Intellect>();
                listofintellects.Add(currentIntellect);
            }

            foreach (Intellect intellect in listofintellects)
            {
                match.Intellects.Add(intellect);
            }

            db.AddToGame(match);

            db.SaveChanges();

            PutMessage(new Message(gameID, intellects));
                        
            return gameID;

        }

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

        #endregion

        #region tournaments

        public static ErrorCode CreateTournament(string title, string startdate, Int64 maxplayers, Guid userID)
        {
            if ((from t in db.Tournament
                 where t.Tournament_Name == title && t.Creator_ID == userID
                 select t).ToList<Tournament>().Any<Tournament>())
                return new ErrorCode(ErrorType.BadFileType, "Bad tournament title. User has already created tournament with the same name = " + title);

            if (DateTime.Compare(DateTime.Parse(startdate), DateTime.Now) <= 0)
            {
                return new ErrorCode(ErrorType.UnknownException, "Bad tournament starttime. User's starttime = " + startdate.ToString() + " is early then currenttime = " + DateTime.Now.ToString());
            }

            try
            { 
                db.AddToTournament(Tournament.CreateTournament(Guid.NewGuid(), maxplayers, startdate, userID, title));
                db.SaveChanges();

                return new ErrorCode(ErrorType.Ok, "Tournament has been created");
            }
            catch (Exception e)
            {
                return new ErrorCode(ErrorType.UnknownException, "Database problems: " + e.ToString());
            }
        }

        public static List<Guid> GetMyTournaments(Guid userID)
        {
            List<Guid> tournamentlist;
            if ((tournamentlist = (from t in db.Tournament
                                   where t.Creator_ID == userID
                                   select t.Tournament_ID).ToList<Guid>()).Any<Guid>())
            {
                return tournamentlist;
            }
            else
            {
                // User didn't create any tournaments.
                return null;
            }

        }

        public static ErrorCode DeleteTournament(Guid tournamentID, Guid userID)
        {
            Tournament needed;

            if ((needed = (from t in db.Tournament
                           where t.Creator_ID == userID && t.Tournament_ID == tournamentID
                           select t).FirstOrDefault<Tournament>()) != null)
            {
                try
                {
                    
                    db.Tournament.DeleteObject(needed);

                    db.SaveChanges();

                    return new ErrorCode(ErrorType.Ok, "Tournament with title " + needed.Tournament_Name + " was deleted.");
                }
                catch (Exception e)
                {
                    return new ErrorCode(ErrorType.UnknownException, "Database problems: " + e.ToString());
                }
            }
            else
                return new ErrorCode(ErrorType.BadFileType, "User " + (from u in db.Account
                                                                       where u.Account_ID == userID
                                                                       select u.Account_Name) + " didn't create that tournament. ");
          
        }

        public static List<Guid> GetAvailableTournaments(Guid userID)
        {
            List<Tournament> actualtournaments;

            if ((actualtournaments = (from t in db.Tournament
                                      where DateTime.Compare(DateTime.Parse(t.When), DateTime.Now) >= 0
                                      select t).ToList<Tournament>()).Any<Tournament>())
            {
                Account user = (from a in db.Account
                                where a.Account_ID == userID
                                select a).FirstOrDefault<Account>();

                List<Guid> recommendedtournaments = new List<Guid>();

                foreach (Tournament t in actualtournaments)
                {
                    if (!user.TournamentPlayer.Contains(t))
                    {
                        recommendedtournaments.Add(t.Tournament_ID);
                    }
                }

                if (recommendedtournaments.Any<Guid>())
                    return recommendedtournaments;
                else
                    // User competes in all actual tournaments.
                    return null;
            }

            else
                // There are no actual tournaments.
                return null;
          
        }

        public static ErrorCode JoinTournament(Guid tournamentID, Guid userID)
        {

            Tournament neededtournament = (from t in db.Tournament
                                           where t.Tournament_ID == tournamentID
                                           select t).FirstOrDefault<Tournament>();



            if (neededtournament !=null)
            {

                Account updatedaccount = (from a in db.Account
                                          where a.Account_ID == userID
                                          select a).FirstOrDefault<Account>();

                if (updatedaccount.TournamentPlayer.Contains(neededtournament))
                {
                    return new ErrorCode(ErrorType.WrongLoginOrPassword, "User already competes in that tournament");
                }
                else
                {
                    try
                    {

                        updatedaccount.TournamentPlayer.Add(neededtournament);
                        db.SaveChanges();

                        return new ErrorCode(ErrorType.Ok, "User " + (from u in db.Account
                                                                      where u.Account_ID == userID
                                                                      select u.Account_Name) + " joined to tournament.");

                    }
                    catch (Exception e)
                    {
                        return new ErrorCode(ErrorType.UnknownException, "Database problems: \n" + e.ToString());
                    }
                }

            }
            else
            {
                return new ErrorCode(ErrorType.BadFileType, "There are no tournaments with that ID.");
            }
          
        }

        public static ErrorCode LeaveTournament(Guid tournamentID, Guid userID)
        {

            Tournament neededtournament = (from t in db.Tournament
                                           where t.Tournament_ID == tournamentID
                                           select t).FirstOrDefault<Tournament>();
            
            if (neededtournament != null)
            {
                
                Account updatedaccount = (from a in db.Account
                                          where a.Account_ID == userID
                                          select a).FirstOrDefault<Account>();

                if (updatedaccount.TournamentPlayer.Contains(neededtournament))
                {
                    try
                    {
                        updatedaccount.TournamentPlayer.Remove(neededtournament);

                        db.SaveChanges();

                        return new ErrorCode(ErrorType.Ok, "User " + updatedaccount.Account_Name + " leaved the tournament.");
                    }
                    catch (Exception e)
                    {
                        return new ErrorCode(ErrorType.UnknownException, "Database problems: " + e.ToString());
                    }
                }
                else
                {
                    return new ErrorCode(ErrorType.WrongLoginOrPassword, "User doesn't compete that tournament");
                }

            }
            else
                return new ErrorCode(ErrorType.BadFileType, "There are no tournaments with that ID.");
          
        }

        #endregion


        #region roles

		public static bool IsUserAdmin(Guid userID)
		{
			var needed = (from r in db.UserRole
			              where r.AccountAccount_ID == userID && r.Role_Code == 1
			              select r).ToList();

			if (DateTime.Compare(DateTime.Parse(needed.FirstOrDefault().Until), DateTime.Now) >= 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool IsUser(RoleType rolecode, Guid userID)
		{
			if ((from r in db.UserRole
			     where r.AccountAccount_ID == userID && r.Role_Code == (int)rolecode
			     select r).Any())
				return true;
			else
				return false;
		}

		public static ErrorCode SetUserRole(RoleType rolecode, Guid userID, String until)
		{
			List<UserRole> thatroleofuser = (from r in db.UserRole
			                                 where r.AccountAccount_ID == userID && r.Role_Code == (int)rolecode
			                                 select r).ToList<UserRole>();

			if (thatroleofuser.Any())
			{
				if (DateTime.Compare(DateTime.Parse(until), DateTime.Now) <= 0)
				{
					return new ErrorCode(ErrorType.UnknownException, "Datetime is in past");
				}
				else
				{
					try
					{

						db.UserRole.DeleteObject(thatroleofuser.First<UserRole>());
						db.UserRole.AddObject(UserRole.CreateUserRole(Guid.NewGuid(), until, userID, (int)rolecode));
						db.SaveChanges();
						return new ErrorCode(ErrorType.Ok, "Role has been given.");

					}
					catch (Exception e)
					{
						return new ErrorCode(ErrorType.UnknownException, "Database problems: " + e.ToString());
					}

				}
			}
			else
			{
				try
				{
					db.UserRole.AddObject(UserRole.CreateUserRole(Guid.NewGuid(), until, userID, (int)rolecode));

					db.SaveChanges();

					return new ErrorCode(ErrorType.Ok, "Role has been given.");
				}
				catch (Exception e)
				{
					return new ErrorCode(ErrorType.UnknownException, "Database problems: " + e.ToString());
				}
			}

		}

		public static string[] GetUserRoles(Guid userID)
		{

            return (from r in db.UserRole
                       where r.AccountAccount_ID == userID
                       select r.Role_Code).ToList().
                           Select(x=>
                               Enum.GetName(typeof(RoleType), (RoleType)x)).ToArray();
		}

		#endregion

        #endregion

    }
}