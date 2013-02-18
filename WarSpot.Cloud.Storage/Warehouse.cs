using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using WarSpot.Contracts.Service;

namespace WarSpot.Cloud.Storage
{
    public static class Warehouse
    {

        #region BLOB SECTION VAR
        private const string CONNECTIONSTRING = "DataConnectionString";
        private const string DBCONNECTIONSTRING = "DBConnectionString";

        private static bool storageInitialized = false;
        private static object gate = new Object();
        private static CloudBlobClient blobStorage;
        private static CloudBlobContainer container;

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
            if (db.Intellect == null)
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



        #endregion

        #region match

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
                    foreach (TournamentPlayer p in (from tp in db.TournamentPlayers
                                                    where tp.TournamentTournament_ID == needed.Tournament_ID
                                                    select tp).ToList<TournamentPlayer>())
                    {

                        db.TournamentPlayers.DeleteObject(p);

                    }

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
                List<Guid> currentusertournaments = (from t in db.TournamentPlayers
                                                     where t.AccountAccount_ID == userID
                                                     select t.TournamentTournament_ID).ToList<Guid>();

                List<Guid> recommendedtournaments = new List<Guid>();

                foreach (Tournament t in actualtournaments)
                {
                    if (!currentusertournaments.Contains<Guid>(t.Tournament_ID))
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
            if ((from t in db.Tournament
                 where t.Tournament_ID == tournamentID
                 select t).ToList<Tournament>().Any<Tournament>())
            {
                if ((from tp in db.TournamentPlayers
                     where tp.AccountAccount_ID == userID && tp.TournamentTournament_ID == tournamentID
                     select tp).Any<TournamentPlayer>())
                {
                    return new ErrorCode(ErrorType.WrongLoginOrPassword, "User already competes in that tournament");
                }
                else
                {
                    try
                    {
                        db.TournamentPlayers.AddObject(TournamentPlayer.CreateTournamentPlayer(new Guid(), tournamentID, userID));

                        db.SaveChanges();

                        return new ErrorCode(ErrorType.Ok, "User " + (from u in db.Account
                                                                      where u.Account_ID == userID
                                                                      select u.Account_Name) + " joined to tournament.");

                    }
                    catch (Exception e)
                    {
                        return new ErrorCode(ErrorType.UnknownException, "Database problems: " + e.ToString());
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
            if ((from t in db.Tournament
                 where t.Tournament_ID == tournamentID
                 select t).ToList<Tournament>().Any<Tournament>())
            {
                TournamentPlayer needed;
                if ((needed = (from tp in db.TournamentPlayers
                               where tp.AccountAccount_ID == userID && tp.TournamentTournament_ID == tournamentID
                               select tp).ToList<TournamentPlayer>().FirstOrDefault<TournamentPlayer>()) != null)
                {
                    try
                    {
                        db.TournamentPlayers.DeleteObject(needed);
                        db.SaveChanges();

                        return new ErrorCode(ErrorType.Ok, "User " + (from u in db.Account
                                                                      where u.Account_ID == userID
                                                                      select u.Account_Name) + " leaved the tournament.");
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
            if (DateTime.Compare(DateTime.Parse((from r in db.UserRole
                                                 where r.AccountAccount_ID == userID && r.Role_Code == 1
                                                 select r).FirstOrDefault<UserRole>().Until), DateTime.Now) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static ErrorCode SetUserRole(short rolecode, Guid userID, String until)
        {
            List<UserRole> thatroleofuser = (from r in db.UserRole
                 where r.AccountAccount_ID == userID && r.Role_Code == rolecode
                 select r).ToList<UserRole>();

            if (thatroleofuser.Any<UserRole>())
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
                        db.UserRole.AddObject(UserRole.CreateUserRole(new Guid(), until, userID, rolecode));
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
                    db.UserRole.AddObject(UserRole.CreateUserRole(new Guid(), until, userID, rolecode));

                    db.SaveChanges();

                    return new ErrorCode(ErrorType.Ok, "Role has been given.");
                }
                catch (Exception e)
                {
                    return new ErrorCode(ErrorType.UnknownException, "Database problems: " + e.ToString());
                }
            }

        }

        public static List<Guid> GetUserRole(Guid userID)
        {
            List<Guid> roleslist;
            if ((roleslist = (from r in db.UserRole
                              where r.AccountAccount_ID == userID
                              select r.Role_ID).ToList<Guid>()).Any<Guid>())
            {
                return roleslist;
            }
            else
            {
                // User didn't have any roles. (IS IT POSSIBLE?)
                return null;
            }
        }

        #endregion

        #endregion



    }
}
